using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.ViewModels;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class UserService : IUserService
    {
        //repositories used by the service for operation
        private IUserRepository UserRepository { get; set; }
        private IRegistrationRepository RegistrationRepository { get; set; }

        private const string UserActiveStatus = "Active";
        private const string UserClosedStatus = "Closed";
        private const string UserInactiveStatus = "Inactive";
        private const string UserPartialStatus = "Partial";
        private const int UserMinimumAge = 18;

        //repositories are injected to allow easier testing
        public UserService(IUserRepository userRepository, IRegistrationRepository registrationRepository)
        {
            UserRepository = userRepository;
            RegistrationRepository = registrationRepository;
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
                    return new LogonResponse
                    {
                        Id = user.AccountID,
                        Success = true,
                        Message = "Logon was successful."
                    };
                }
            }

            return new LogonResponse
            {
                Success = false,
                Message = "Invalid email or password."
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
                
            DateTime dob = request.DateOfBirth ?? DateTime.Now;
            DateTime minAge = dob.AddYears(-UserMinimumAge);
            if (dob.Date <= minAge.Date)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Unable to register user. You must be at least " + UserMinimumAge + " to register",
                    Errors = new string[]
                    {
                        "User does not meet the age requirement"
                    }
                };
            }
                
            //register the user first
            var user = new User
            {
                FirstName = request.FirstName,
                LastName =request.LastName,
                Email = request.Email,
                Password = Encryption.EncryptString(request.Password),
                Status = UserInactiveStatus
            };

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

        
        /// <summary>
        /// Finds a user from the database and their associated registration if present
        /// </summary>
        /// <param name="id">The account ID of the user</param>
        /// <returns>A user object with populated registration</returns>
        public User FindUser(int id)
        {

            var user = UserRepository.Find(id);

            //if the registration wasn't returned by the user repository then explicitly load from the
            //registration repository
            if (user.Registration == null)
            {
                user.Registration = RegistrationRepository.Find(user.AccountID);
            }

            return user;
        }

        /// <summary>
        /// Find all users in the system
        /// </summary>
        /// <returns>A list of users</returns>
        public List<User> FindUsers()
        {
            return UserRepository.FindAll();
        }
    }
}