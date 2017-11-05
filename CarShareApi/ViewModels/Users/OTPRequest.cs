using System.ComponentModel.DataAnnotations;

namespace CarShareApi.ViewModels.Users
{
    public class OTPRequest
    {
        //OTP request provides for the passphrase and email address objects 
        //to facilitate the user account activation.

        [Required]
        [MaxLength(6)]
        public string OTP { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}