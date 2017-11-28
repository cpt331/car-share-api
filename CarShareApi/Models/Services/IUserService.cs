//======================================
//
//Name: IUserService.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using CarShareApi.Models.ViewModels;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;
using CarShareApi.ViewModels.Users;

namespace CarShareApi.Models.Services
{
    public interface IUserService : IDisposable
    {
        //This interface provides the overarching activities related to
        //The users service actions

        List<UserViewModel> FindUsers();
        UserViewModel FindUser(int id);
        LogonResponse Logon(LogonRequest request);
        RegisterResponse Register(RegisterRequest request);

        AddPaymentMethodResponse AddPaymentMethod
            (AddPaymentMethodRequest request, int accountId);

        BookingHistoryResponse GetBookingHistory
            (int accountId, int pageNumber, int pageSize);

        RegisterViewModel GetRegistrationRecord(int accoundId);
        PasswordResetResponse ResetPassword(PasswordResetRequest request);
        OTPResponse OtpActivation(OTPRequest request);

        InterfaceResponse UpdateRegistration
            (RegisterUpdateRequest request, int accountId);
    }
}