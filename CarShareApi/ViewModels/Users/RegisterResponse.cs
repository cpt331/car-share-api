namespace CarShareApi.Models.ViewModels
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}
