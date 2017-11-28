//======================================
//
//Name: OpenBookingResponse.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels
{
    public class OpenBookingResponse
    {
        //this allows the outcome of a request by passing the success/failure
        //of the action, a message that will be displayed to the user and if
        //a booking exists, the booking ID and check out time

        public bool Success { get; set; }
        public string Message { get; set; }

        //if booking was successful
        public int? BookingId { get; set; }

        public string CheckOutTime { get; set; }
    }
}