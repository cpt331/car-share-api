namespace CarShareApi.ViewModels.Users
{
    public class AddPaymentMethodRequest
    {
        //this request will attempt to store the payment details of the 
        //user including card name, card type, card number, expiry and cvv
        public string CardName { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CardVerificationValue { get; set; }
    }
}