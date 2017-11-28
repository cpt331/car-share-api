//======================================
//
//Name: CloseBookingCheckRequest.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckRequest
    {
        //the close booking request allows a user to close their booking
        //by passing the booking ID and parsing the lat and long so that
        //the system can perform the neccessary calculations (ie is car 
        //being returned within x radius of a city. This check is used when
        //a user is considering checking their vehicle back in and needs a 
        //quote

        public int BookingId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}