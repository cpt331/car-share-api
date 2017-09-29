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
        private IUserRepository UserRepository { get; set; }
        private IRegistrationRepository RegistrationRepository { get; set; }

        public UserService(IUserRepository userRepository, IRegistrationRepository registrationRepository)
        {
            UserRepository = userRepository;
            RegistrationRepository = registrationRepository;
        }

        public LogonResponse Logon(LogonRequest request)
        {
            var user = UserRepository.FindByEmail(request.Email);
            if(user != null)
            {
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

        public RegisterResponse Register(RegisterRequest request)
        {
            

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
            
            var user = new User
            {
                FirstName = request.FirstName,
                LastName =request.LastName,
                Email = request.Email,
                Password = Encryption.EncryptString(request.Password),
                Status = "Active"
            };

            UserRepository.Add(user);

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

            return new RegisterResponse
            {
                Success = true,
                Message = $"User {user.Email} has been created"
            };
        }

        

        public User FindUser(int id)
        {
            return UserRepository.Find(id);
        }

        public List<User> FindUsers()
        {
            return UserRepository.FindAll();
        }
    }
}