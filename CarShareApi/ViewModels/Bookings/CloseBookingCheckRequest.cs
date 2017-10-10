using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckRequest
    {
        public int VehicleId { get; set; }
        public int AccountId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}