using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class OpenBookingResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        //if booking was successful
        public int? BookingId { get; set; }
        public string CheckOutTime { get; set; }
    }
}