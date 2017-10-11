namespace CarShareApi.Models.ViewModels
{
    public class LogonResponse
    {
        public int? Id { get; set; }
        public bool Success { get;set; }
        public string Message { get; set; }

        public int? OpenBookingId { get; set; }
        public bool HasOpenBooking { get; set; }
    }
}
