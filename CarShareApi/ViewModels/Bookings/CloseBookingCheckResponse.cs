namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckResponse
    {
        //provides the message to the end user to show whether the the return
        //was successful or a failure and display a message. if successful 
        //return additional booking details

        public bool Success { get; set; }
        public string Message { get; set; }

        //if check was successful
        public string City { get; set; }

        public string TotalHours { get; set; }
        public string HourlyRate { get; set; }
        public string TotalAmount { get; set; }
    }
}