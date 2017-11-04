namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckRequest
    {
        public int BookingId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}