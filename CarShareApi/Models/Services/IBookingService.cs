//======================================
//
//Name: IBookingService.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;

namespace CarShareApi.Models.Services
{
    //This interface provides the overarching activities related to
    //The booking service actions
    public interface IBookingService : IDisposable
    {
        OpenBookingResponse OpenBooking(int vehicleId, int accountId);
        CloseBookingResponse CloseBooking(CloseBookingRequest request, int accountId);
        CloseBookingCheckResponse CloseBookingCheck(CloseBookingCheckRequest request, int accountId);
        TransactionResponse RecordTransaction(int bookingId, int accountId);
    }
}