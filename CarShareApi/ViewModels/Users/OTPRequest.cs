using System;
using System.ComponentModel.DataAnnotations;

namespace CarShareApi.ViewModels.Users
{
    public class OTPRequest
    {
        [Required]
        [MaxLength(6)]
        public string OTP { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}