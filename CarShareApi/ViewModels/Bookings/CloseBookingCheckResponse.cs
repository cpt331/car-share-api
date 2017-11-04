namespace CarShareApi.ViewModels
{
    public class CloseBookingCheckResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        //if check was successful
        public string City { get; set; }

        public string TotalHours { get; set; }
        public string HourlyRate { get; set; }
        public string TotalAmount { get; set; }
    }
}