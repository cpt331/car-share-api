﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;

namespace CarShareApi.Models.Services
{
    public interface IBookingService : IDisposable
    {
        OpenBookingResponse OpenBooking(int vehicleId, int accountId);
        CloseBookingResponse CloseBooking(CloseBookingRequest request, int accountId);
        CloseBookingCheckResponse CloseBookingCheck(CloseBookingCheckRequest request, int accountId);
        TransactionResponse RecordTransaction(int bookingId, int accountId);
    }
}
