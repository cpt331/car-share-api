//======================================
//
//Name: TransactionRequest.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;

namespace CarShareApi.ViewModels.Bookings
{
    public class TransactionRequest
    {
        //this allows the user to render a transaction against the transaction
        //table that allows the recording of all the financial records
        //such as receipt, transaction, etc

        public int TransactionID { get; set; }
        public int BookingId { get; set; }
        public int ReceiptID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentMethod { get; set; }
    }
}