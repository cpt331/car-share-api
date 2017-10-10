using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models
{
    public static class Constants
    {
        //user service constants
        public const string UserActiveStatus = "Active";
        public const string UserClosedStatus = "Closed";
        public const string UserInactiveStatus = "Inactive";
        public const string UserPartialStatus = "Partial";
        public const int UserMinimumAge = 18;

        //car constants
        public const string CarAvailableStatus = "Active";
        public const string CarBookedStatus = "Inactive";

        //booking constants
        public const string BookingOpenStatus = "Open";
        public const string BookingClosedStatus = "Closed";
        public const string BookingCancelledStatus = "Cancelled";
    }
}