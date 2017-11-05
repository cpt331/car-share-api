using System;
using System.ComponentModel.DataAnnotations;

namespace CarShareApi.ViewModels.Users
{
    public class PasswordResetRequest
    {
        //The password reset request provides the objects needed to facilitate
        //a password reset for the user including email password and 
        //confirmation password a date of birth and the licence number

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = 
            "The {0} must be at least {2} characters long.", 
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = 
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = 
            "The {0} must be at least {2} characters long.", 
            MinimumLength = 1)]
        public string LicenceNumber { get; set; }
    }
}