namespace CarShareApi.ViewModels.Users
{
    public class OTPResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}