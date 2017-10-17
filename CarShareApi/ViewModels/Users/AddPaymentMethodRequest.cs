using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels.Users
{
    public class AddPaymentMethodRequest
    {
        public int AccountId { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CardVerificationValue { get; set; }
    }
}