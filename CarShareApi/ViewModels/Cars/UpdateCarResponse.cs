namespace CarShareApi.ViewModels.Cars
{
    public class UpdateCarResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}