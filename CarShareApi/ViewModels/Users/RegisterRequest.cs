﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CarShareApi.Models.ViewModels
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string LicenceNumber { get; set; }

        [MaxLength(5)]
        public string LicenceState { get; set; }

        [MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(50)]
        public string Suburb { get; set; }

        [MaxLength(10)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Postcode { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }
    }
}
