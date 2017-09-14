using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.ViewModels;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; set; }

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
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
                        Id = user.Id,
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
                Id = 12345,
                Firstname = "X",
                Lastname = "Y",
                Email = request.Email,
                Password = Encryption.EncryptString(request.Password)
            };

            UserRepository.Add(user);
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