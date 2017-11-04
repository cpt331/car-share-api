namespace CarShareApi.ViewModels.Bookings
{
    public class BookingViewModel
    {
        public string BookingId { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string BookingStatus { get; set; }
        public string CheckOutDate { get; set; }
        public string CityPickUp { get; set; }
        public string CheckInDate { get; set; }
        public string CityDropOff { get; set; }
        public string TotalHours { get; set; }
        public string HourlyRate { get; set; }
        public string TotalAmount { get; set; }
    }
}