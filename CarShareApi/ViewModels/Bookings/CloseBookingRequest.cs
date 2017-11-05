namespace CarShareApi.ViewModels
{
    public class CloseBookingRequest
    {
        //the close booking request allows a user to close their booking
        //by passing the booking ID and parsing the lat and long so that
        //the system can perform the neccessary calculations (ie is car 
        //being returned within x radius of a city

        public int BookingId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}