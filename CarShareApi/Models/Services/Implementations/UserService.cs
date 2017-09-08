using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.ViewModels;
using CarShareApi.Models.Repositories;

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
                if (user.Password.Equals(request.Password))
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
            throw new NotImplementedException();
        }

        public User FindUser(int id)
        {
            return UserRepository.Find(id);
        }
    }
}