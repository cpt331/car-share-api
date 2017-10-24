using CarShareApi.Models.ViewModels;
using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels.Bookings;
using CarShareApi.ViewModels.Users;

namespace CarShareApi.Models.Services
{
    public interface IUserService : IDisposable
    {
        List<UserViewModel> FindUsers();
        UserViewModel FindUser(int id);
        LogonResponse Logon(LogonRequest request);
        RegisterResponse Register(RegisterRequest request);
        AddPaymentMethodResponse AddPaymentMethod(AddPaymentMethodRequest request, int accountId);
        BookingHistoryResponse GetBookingHistory(int accountId, int pageNumber, int pageSize);
        RegisterViewModel GetRegistrationRecord(int accoundId);
    }
}
