namespace CarShareApi.Models.ViewModels
{
    public class LogonResponse
    {
        //The logon response provides feedback fields for whether a logon 
        //request was successful or not and if successful will also provide 
        //the objects to define whether a user has an open booking or has 
        //admin rights and a payment method attached
        public int? Id { get; set; }
        public bool Success { get;set; }
        public string Message { get; set; }

        public int? OpenBookingId { get; set; }
        public bool HasOpenBooking { get; set; }

        public bool HasAdminRights { get; set; }
        public bool HasPaymentMethod { get; set; }

    }
}
