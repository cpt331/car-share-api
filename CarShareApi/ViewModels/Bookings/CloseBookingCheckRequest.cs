using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckRequest
    {
        public int BookingId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}