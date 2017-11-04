namespace CarShareApi.ViewModels.Users
{
    public class AddPaymentMethodResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}