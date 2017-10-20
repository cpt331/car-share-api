using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels.Bookings
{
    public class TransactionRequest
    {
        public int TransactionID { get; set; }
        public int BookingId { get; set; }
        public int ReceiptID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentMethod { get; set; }

    }
}