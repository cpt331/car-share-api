using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.ViewModels;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.ViewModels;
<<<<<<< HEAD
using CarShareApi.Models.Providers;
=======
using CarShareApi.ViewModels.Users;
using NLog;
>>>>>>> master

namespace CarShareApi.Models.Services.Implementations
{
    public class UserService : IUserService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        //repositories used by the service for operation
        private IUserRepository UserRepository { get; set; }
        private IRegistrationRepository RegistrationRepository { get; set; }
        private IBookingRepository BookingRepository { get; set; }
        private IPaymentMethodRepository PaymentMethodRepository { get; set; }

        //repositories are injected to allow easier testing
        public UserService(IUserRepository userRepository, 
            IRegistrationRepository registrationRepository, 
            IBookingRepository bookingRepository, 
            IPaymentMethodRepository paymentMethodRepository)
        {
            Logger.Debug("UserService Instantiated");
            UserRepository = userRepository;
            RegistrationRepository = registrationRepository;
            BookingRepository = bookingRepository;
            PaymentMethodRepository = paymentMethodRepository;
        }

        /// <summary>
        /// Validates username and password provided match a user record in the system
        /// </summary>
        /// <param name="request">The request containing the username and password</param>
        /// <returns>A response indicating success or failure of the operation</returns>
        public LogonResponse Logon(LogonRequest request)
        {
            //find the user first by the email provided
            var user = UserRepository.FindByEmail(request.Email);

            //if found continue
            if (user != null)
            {
                
                //encrypt the provided password so it can be compared against the encrypted values in the database
                if (user.Password.Equals(Encryption.EncryptString(request.Password)))
                {
                    //if the password check passes, check if the user has an active status
                    if (user.Status == Constants.UserActiveStatus || user.Status == Constants.UserPartialStatus)
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
                }
            }

            return new LogonResponse
            {
                Success = false,
                Message = "Invalid email or password.",
                HasOpenBooking = false
            };

        }

        /// <summary>
        /// Registers a user and their associated registration into the database
        /// </summary>
        /// <param name="request">The fields used to populate the registration</param>
        /// <returns>A response indicating success or failure of the operation</returns>
        public RegisterResponse Register(RegisterRequest request)
        {
            
            //ensure the user has not been registered already
            if(UserRepository.FindByEmail(request.Email) != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Unable to register user",
                    Errors = new string[]
                    {
                        "User is already registered"
                    }
                };
            }

            

            
            //checks if the user is above the acceptable age
            DateTime dob = request.DateOfBirth ?? DateTime.Now; //this is because dob could be null

            if (dob.Date > DateTime.Now)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"You must enter a date before today's date",
                    Errors = new string[]
                    {
                        "User does not meet the age requirement"
                    }
                };
            }


            DateTime minAge = DateTime.Now.AddYears(-Constants.UserMinimumAge); //minage is todays date minus 18 years
            if (dob.Date > minAge.Date)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Unable to register user. You must be at least " + Constants.UserMinimumAge + " to register",
                    Errors = new string[]
                    {
                        "User does not meet the age requirement"
                    }
                };
            }

            //generate a one time password
            Random otpgenerator = new Random();
            String otpRecord = otpgenerator.Next(100000, 999999).ToString();
                
            //register the user first
            var user = new User
            {
                FirstName = request.FirstName,
                LastName =request.LastName,
                Email = request.Email,
                Password = Encryption.EncryptString(request.Password),
                OTP = otpRecord,
                Status = Constants.UserActiveStatus,
                UserGroup = Constants.UserGroupName
                //Status = UserInactiveStatus
            };

            WelcomeMailer welcome = new WelcomeMailer(request.Email, request.FirstName, otpRecord);
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


            //return successful operation
            return new RegisterResponse
            {
                Success = true,
                Message = $"User {user.Email} has been created. An email to validate this email address will be sent shortly."
            };
        }

        public AddPaymentMethodResponse AddPaymentMethod(AddPaymentMethodRequest request)
        {
            return new AddPaymentMethodResponse
            {
                Success = false,
                Message = "Shawn hasn't completed his trello card yet..."
            };
        }


        /// <summary>
        /// Finds a user from the database and their associated registration if present
        /// </summary>
        /// <param name="id">The account ID of the user</param>
        /// <returns>A user object with populated registration</returns>
        public UserViewModel FindUser(int id)
        {

            var user = UserRepository.Find(id);

            //if the registration wasn't returned by the user repository then explicitly load from the
            //registration repository
            if (user.Registration == null)
            {
                user.Registration = RegistrationRepository.Find(user.AccountID);
            }

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
        /// Find all users in the system
        /// </summary>
        /// <returns>A list of users</returns>
        public List<UserViewModel> FindUsers()
        {
            throw new NotImplementedException();
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