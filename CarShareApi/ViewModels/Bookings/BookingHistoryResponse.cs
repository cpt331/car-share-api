namespace CarShareApi.ViewModels.Bookings
{
    public class BookingHistoryResponse
    {
        //this allows the outcome of a request by passing the success/failure
        //of the action, a message that will be displayed to the user as well 
        //as additional valiables holding the count of bookings, number of 
        //pages, the current page number and list of all bookings. This is
        //interacting with the bookings table and grouping the bookings into
        //pagable views

        public bool Success { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public BookingViewModel[] Bookings { get; set; }
    }
}