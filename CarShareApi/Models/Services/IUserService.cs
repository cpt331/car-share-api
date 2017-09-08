using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareApi.Models.Services
{
    public interface IUserService
    {
        User FindUser(int id);
        LogonResponse Logon(LogonRequest request);
        RegisterResponse Register(RegisterRequest request);
    }
}
