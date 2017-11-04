namespace CarShareApi.ViewModels.Bookings
{
    public class BookingHistoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public BookingViewModel[] Bookings { get; set; }
    }
}