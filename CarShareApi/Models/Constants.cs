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
        public const string UserGroupName = "User";
        public const string UserAdminGroupName = "Admin";

        //car constants
        public const string CarAvailableStatus = "Available";
        public const string CarBookedStatus = "Booked";
        public const string CarRemovedStatus = "Removed";

        //booking constants
        public const string BookingOpenStatus = "Open";
        public const string BookingClosedStatus = "Closed";
        public const string BookingCancelledStatus = "Cancelled";
        public const double BookingMaxRangeFromCityCentre = 10000; //metres

        //transaction constants
        public const string TransactionUnpaidStatus = "Unpaid";
        public const string TransactionPendingStatus = "Pending";
        public const string TransactionClearedStatus = "Cleared";

    }
}