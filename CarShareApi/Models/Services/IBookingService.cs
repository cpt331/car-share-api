using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services
{
    public interface IBookingService
    {
        OpenBookingResponse OpenBooking(int vehicleId, int accountId);
        CloseBookingResponse CloseBooking(CloseBookingRequest request);
        
    }
}
