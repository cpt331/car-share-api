using System;
using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Providers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.ViewModels;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;
using CarShareApi.ViewModels.Users;
using NLog;

namespace CarShareApi.Models.Services.Implementations
{
    public class UserService : IUserService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //repositories are injected to allow easier testing
        public UserService(IUserRepository userRepository,
            IRegistrationRepository registrationRepository,
            IBookingRepository bookingRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IEmailProvider emailProvider, ICarRepository carRepository)
        {
            Logger.Debug("UserService Instantiated");
            UserRepository = userRepository;
            RegistrationRepository = registrationRepository;
            BookingRepository = bookingRepository;
            PaymentMethodRepository = paymentMethodRepository;
            EmailProvider = emailProvider;
            CarRepository = carRepository;
        }

        //repositories used by the service for operation
        private IUserRepository UserRepository { get; }

        private IRegistrationRepository RegistrationRepository { get; }
        private IBookingRepository BookingRepository { get; }
        private IPaymentMethodRepository PaymentMethodRepository { get; }
        private IEmailProvider EmailProvider { get; }
        private ICarRepository CarRepository { get; }
        private ITemplateRepository TemplateRepository { get; }

        /// <summary>
        ///     Validates username and password provided match a user record in the system
        /// </summary>
        /// <param name="request">The request containing the username and password</param>
        /// <returns>A response indicating success or failure of the operation</returns>
        public LogonResponse Logon(LogonRequest request)
        {
            //find the user first by the email provided
            var user = UserRepository.FindByEmail(request.Email);

            //if found continue
            if (user != null)
                if (user.Password.Equals(Encryption.EncryptString(request.Password)))
                    if (user.Status == Constants.UserOTPStatus)
                    {
                        var response = new LogonResponse
                        {
                            Success = false,
                            Message = "Account activation required. Please activate your account.",
                            HasOpenBooking = false
                        };
                        return response;
                    }
                    //if the password check passes, check if the user has an active status
                    else if (user.Status == Constants.UserActiveStatus || user.Status == Constants.UserPartialStatus)
                    {
                        var response = new LogonResponse
                        {
                            Id = user.AccountID,
                            Success = true,
                            Message = "Logon was successful.",
                            HasOpenBooking = false
                        };

                        var openBooking = BookingRepository.FindByAccountId(user.AccountID)
                            .FirstOrDefault(x => x.BookingStatus == Constants.BookingOpenStatus);

                        if (openBooking != null)
                        {
                            response.HasOpenBooking = true;
                            response.OpenBookingId = openBooking.BookingID;
                        }

                        return response;
                    }

            return new LogonResponse
            {
                Success = false,
                Message = "Invalid email or password.",
                HasOpenBooking = false
            };
        }

        /// <summary>
        ///     Registers a user and their associated registration into the database
        /// </summary>
        /// <param name="request">The fields used to populate the registration</param>
        /// <returns>A response indicating success or failure of the operation</returns>
        public RegisterResponse Register(RegisterRequest request)
        {
            //ensure the user has not been registered already
            if (UserRepository.FindByEmail(request.Email) != null)
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Unable to register user",
                    Errors = new[]
                    {
                        "User is already registered"
                    }
                };


            //checks if the user is above the acceptable age
            var dob = request.DateOfBirth ?? DateTime.Now; //this is because dob could be null

            if (dob.Date > DateTime.Now)
                return new RegisterResponse
                {
                    Success = false,
                    Message = "You must enter a date before today's date",
                    Errors = new[]
                    {
                        "User does not meet the age requirement"
                    }
                };


            var minAge = DateTime.Now.AddYears(-Constants.UserMinimumAge); //minage is todays date minus 18 years
            if (dob.Date > minAge.Date)
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Unable to register user. You must be at least " + Constants.UserMinimumAge +
                              " to register",
                    Errors = new[]
                    {
                        "User does not meet the age requirement"
                    }
                };

            //generate a one time password
            var otpgenerator = new Random();
            var otpRecord = otpgenerator.Next(100000, 999999).ToString();

            //register the user first
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = Encryption.EncryptString(request.Password),
                OTP = otpRecord,
                //Status = Constants.UserActiveStatus,
                UserGroup = Constants.UserGroupName,
                Status = Constants.UserOTPStatus
            };


            //Mail.SMTPMailer(request.Email, request.FirstName, otpRecord);
            UserRepository.Add(user);

            //populate the registration table now using the account ID of the registered user
            var registration = new Registration
            {
                AccountID = user.AccountID,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                DateOfBirth = request.DateOfBirth.Value,
                DriversLicenceID = request.LicenceNumber,
                DriversLicenceState = request.LicenceState,
                PhoneNumber = request.PhoneNumber,
                Postcode = request.Postcode,
                State = request.State,
                Suburb = request.Suburb
            };
            RegistrationRepository.Add(registration);

            var emailTemplate = TemplateRepository.Find(1);

            if (emailTemplate == null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "An error has occurred",
                    Errors = new[]
                    {
                        "User account registered. No email template defined."
                    }
                };
            }


            EmailProvider.Send(request.Email, request.FirstName, otpRecord, emailTemplate);

            //return successful operation
            return new RegisterResponse
            {
                Success = true,
                Message =
                    $"User {user.Email} has been created. An email to validate this email address will be sent shortly."
            };
        }

        public AddPaymentMethodResponse AddPaymentMethod(AddPaymentMethodRequest request, int accountId)
        {
            var expiry = new DateTime(request.ExpiryYear, request.ExpiryMonth,
                DateTime.DaysInMonth(request.ExpiryYear, request.ExpiryMonth));

            var user = UserRepository.Find(accountId);
            if (user == null)
                return new AddPaymentMethodResponse
                {
                    Message = $"Account {accountId} does not exist",
                    Success = false
                };

            if (user.Status != Constants.UserActiveStatus)
                return new AddPaymentMethodResponse
                {
                    Message = "Only activated users can add payment methods",
                    Success = false
                };

            var existingPaymentMethod = PaymentMethodRepository.Find(accountId);
            if (existingPaymentMethod != null)
                return new AddPaymentMethodResponse
                {
                    Message = $"Payment method already exists for account {accountId}",
                    Success = false
                };

            if (string.IsNullOrEmpty(request.CardNumber) || string.IsNullOrEmpty(request.CardVerificationValue))
                return new AddPaymentMethodResponse
                {
                    Message = "A credit card is required",
                    Success = false
                };

            var sumOfDigits = request.CardNumber.Where(e => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => ((int) e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum(e => e / 10 + e % 10);

            if (sumOfDigits % 10 != 0)
                return new AddPaymentMethodResponse
                {
                    Message = "The entered card number is invalid.",
                    Success = false
                };

            if (DateTime.Now > expiry)
                return new AddPaymentMethodResponse
                {
                    Message = "The entered credit card has expired.",
                    Success = false
                };

            string cardType;
            switch (request.CardNumber.Substring(0, 1))
            {
                case "3":
                    cardType = "AMEX";
                    break;
                case "4":
                    cardType = "Visa";
                    break;
                case "5":
                    cardType = "Mastercard";
                    break;
                default:
                    cardType = "Mastercard";
                    break;
            }

            try
            {
                var payment = new PaymentMethod
                {
                    AccountID = accountId,
                    CardNumber = request.CardNumber,
                    CardName = request.CardName,
                    CardType = cardType,
                    ExpiryMonth = request.ExpiryMonth,
                    ExpiryYear = request.ExpiryYear,
                    CardVerificationValue = request.CardVerificationValue
                };
                PaymentMethodRepository.Add(payment);
            }
            catch (Exception e)
            {
                return new AddPaymentMethodResponse
                {
                    Message = $"Error in updating payment method. Error: {e}",
                    Success = false
                };
            }

            return new AddPaymentMethodResponse
            {
                Success = true,
                Message = "Payment method has been successfull added!"
            };
        }

        /// <summary>
        ///     Return a list of bookings for a user
        /// </summary>
        /// <param name="accountId">The user account id</param>
        /// <param name="pageNumber">The page number of the list</param>
        /// <param name="pageSize">the paging size required</param>
        /// <returns>A page of bookings based on the input params</returns>
        public BookingHistoryResponse GetBookingHistory(int accountId, int pageNumber, int pageSize)
        {
            var user = UserRepository.Find(accountId);

            //check user is real
            if (user == null)
                return new BookingHistoryResponse
                {
                    Success = false,
                    Message = $"User account {accountId} does not exist"
                };

            //grab all user bookings
            var bookings = BookingRepository.FindByAccountId(accountId) ?? new List<Booking>();

            if (!bookings.Any())
                return new BookingHistoryResponse
                {
                    Success = false,
                    Count = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = 0,
                    Message = "No bookings for this user exist"
                };

            //only grab closed bookings
            bookings = bookings
                .Where(x => x.BookingStatus.Equals(Constants.BookingClosedStatus))
                .OrderByDescending(x => x.CheckIn)
                .ToList();


            //build response
            var response = new BookingHistoryResponse
            {
                Success = true,
                Count = bookings.Count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int) Math.Ceiling(Convert.ToDouble(bookings.Count) / Convert.ToDouble(pageSize))
            };

            //grab the records that relate to the requested page only
            var bookingsPage = bookings.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var bookingViewModels = new List<BookingViewModel>();

            //build the booking view models for the requested page
            foreach (var booking in bookingsPage)
            {
                if (booking.Car == null)
                    booking.Car = CarRepository.Find(booking.VehicleID);

                if (booking.AmountBilled != null)
                {
                    var viewModel = new BookingViewModel
                    {
                        BookingId = booking.BookingID.ToString(),
                        BookingStatus = booking.BookingStatus,
                        CarMake = booking.Car.Make,
                        CarModel = booking.Car.Model,
                        CheckInDate = booking.CheckIn?.ToString() ?? "",
                        CheckOutDate = booking.CheckOut.ToString(),
                        CityDropOff = booking.CityDropOff,
                        CityPickUp = booking.CityPickUp,
                        TotalHours = booking.TimeBilled.ToString(),
                        TotalAmount = booking.AmountBilled.Value.ToString("C"),
                        HourlyRate = booking.BillingRate.ToString("C")
                    };
                    bookingViewModels.Add(viewModel);
                }
            }

            response.Bookings = bookingViewModels.ToArray();
            response.Message = $"Found {response.Bookings.Length} bookings";
            return response;
        }


        /// <summary>
        ///     Finds a user from the database and their associated registration if present
        /// </summary>
        /// <param name="id">The account ID of the user</param>
        /// <returns>A user object with populated registration</returns>
        public UserViewModel FindUser(int id)
        {
            var user = UserRepository.Find(id);

            //if the registration wasn't returned by the user repository then explicitly load from the
            //registration repository
            if (user.Registration == null)
                user.Registration = RegistrationRepository.Find(user.AccountID);

            //if the payment method wasn't returned by the user repository then explicitly load from the
            //registration repository
            if (user.PaymentMethod == null)
                user.PaymentMethod = PaymentMethodRepository.Find(user.AccountID);

            var viewModel = new UserViewModel(user);

            var openBooking = BookingRepository.FindByAccountId(user.AccountID)
                .FirstOrDefault(x => x.BookingStatus == Constants.BookingOpenStatus);

            if (openBooking != null)
            {
                viewModel.HasOpenBooking = true;
                viewModel.OpenBookingId = openBooking.BookingID;
            }

            return viewModel;
        }

        /// <summary>
        ///     Find all users in the system
        /// </summary>
        /// <returns>A list of users</returns>
        public List<UserViewModel> FindUsers()
        {
            throw new NotImplementedException();
        }

        public RegisterViewModel GetRegistrationRecord(int accountId)
        {
            var user = UserRepository.Find(accountId);

            //check user is real
            if (user == null)
                return new RegisterViewModel
                {
                    Success = false,
                    Message = $"User account {accountId} does not exist"
                };

            //grab users registration record and check if it exists
            var registration = RegistrationRepository.Find(accountId);

            if (registration == null)
                return new RegisterViewModel
                {
                    Success = false,
                    Message = $"User account {accountId} registration record doesn't exists",
                    DriversLicenceID = "",
                    DriversLicenceState = "",
                    AddressLine1 = "",
                    AddressLine2 = "",
                    Suburb = "",
                    State = "",
                    Postcode = "",
                    PhoneNumber = "",
                    DateOfBirth = DateTime.Now.ToString("dd/MM/yyyy")
                };
            return new RegisterViewModel
            {
                Success = true,
                Message = $"User account {accountId} registration record exists",
                DriversLicenceID = registration.DriversLicenceID,
                DriversLicenceState = registration.DriversLicenceState,
                AddressLine1 = registration.AddressLine1,
                AddressLine2 = registration.AddressLine2,
                Suburb = registration.Suburb,
                State = registration.State,
                Postcode = registration.Postcode,
                PhoneNumber = registration.PhoneNumber,
                DateOfBirth = registration.DateOfBirth.ToString("dd/MM/yyyy")
            };
        }

        public InterfaceResponse UpdateRegistration(RegisterUpdateRequest request, int accountId)
        {
            var user = UserRepository.Find(accountId);

            //check user is real
            if (user == null)
                return new InterfaceResponse
                {
                    Success = false,
                    Message = $"User account {accountId} does not exist"
                };

            var dob = request.DateOfBirth ?? DateTime.Now; //this is because dob could be null

            if (dob.Date > DateTime.Now)
                return new InterfaceResponse
                {
                    Success = false,
                    Message = "You must enter a date before today's date",
                    Errors = new[]
                    {
                        "User does not meet the age requirement"
                    }
                };

            var minAge = DateTime.Now.AddYears(-Constants.UserMinimumAge); //minage is todays date minus 18 years
            if (dob.Date > minAge.Date)
                return new InterfaceResponse
                {
                    Success = false,
                    Message = "You must be at least " + Constants.UserMinimumAge + " to register",
                    Errors = new[]
                    {
                        "User does not meet the age requirement"
                    }
                };

            var record = RegistrationRepository.Find(accountId);


            if (record == null)
            {
                var registration = new Registration
                {
                    AccountID = user.AccountID,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    DateOfBirth = request.DateOfBirth.Value,
                    DriversLicenceID = request.LicenceNumber,
                    DriversLicenceState = request.LicenceState,
                    PhoneNumber = request.PhoneNumber,
                    Postcode = request.Postcode,
                    State = request.State,
                    Suburb = request.Suburb
                };
                RegistrationRepository.Add(registration);
            }
            else
            {
                record.AccountID = user.AccountID;
                record.AddressLine1 = request.AddressLine1;
                record.AddressLine2 = request.AddressLine2;
                record.DateOfBirth = request.DateOfBirth.Value;
                record.DriversLicenceID = request.LicenceNumber;
                record.DriversLicenceState = request.LicenceState;
                record.PhoneNumber = request.PhoneNumber;
                record.Postcode = request.Postcode;
                record.State = request.State;
                record.Suburb = request.Suburb;
                RegistrationRepository.Update(record);
            }

            return new InterfaceResponse
            {
                Success = true,
                Message = "Registration record updated"
            };
        }

        public PasswordResetResponse ResetPassword(PasswordResetRequest request)
        {
            var licence = request.LicenceNumber;
            var dob = request.DateOfBirth;

            var user = UserRepository.FindByEmail(request.Email);
            if (user == null)
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Password reset failed email mismatch"
                };

            var registration = RegistrationRepository.Find(user.AccountID);
            if (registration == null)
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Password reset failed no registration"
                };

            if (licence != registration.DriversLicenceID || dob != registration.DateOfBirth)
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Password reset failed drivers licence and dob"
                };

            user.Password = Encryption.EncryptString(request.Password);
            UserRepository.Update(user);
            return new PasswordResetResponse
            {
                Success = true,
                Message = "Password successfully reset"
            };
        }

        public OTPResponse OtpActivation(OTPRequest request)
        {
            var email = request.Email;
            var otp = request.OTP;

            var user = UserRepository.FindByEmail(request.Email);
            if (user == null)
                return new OTPResponse
                {
                    Success = false,
                    Message = "A user with this email address doesn't exist"
                };

            if (user.Status != Constants.UserOTPStatus)
                return new OTPResponse
                {
                    Success = false,
                    Message = "User account has already been activated"
                };

            if (otp != user.OTP.Substring(0, 6))
                return new OTPResponse
                {
                    Success = false,
                    Message = "The incorrect passcode has been applied. Check your email"
                };

            user.Status = Constants.UserActiveStatus;
            UserRepository.Update(user);
            return new OTPResponse
            {
                Success = true,
                Message = "Your account has now been activated"
            };
        }

        public void Dispose()
        {
            Logger.Debug("UserService Disposed");
            UserRepository?.Dispose();
            RegistrationRepository?.Dispose();
            BookingRepository?.Dispose();
            PaymentMethodRepository?.Dispose();
        }
    }
}