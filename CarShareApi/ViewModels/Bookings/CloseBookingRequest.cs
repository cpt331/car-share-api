namespace CarShareApi.ViewModels
{
    public class CloseBookingRequest
    {
        public int BookingId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}