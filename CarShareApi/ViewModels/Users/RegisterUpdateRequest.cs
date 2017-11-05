using System;
using System.ComponentModel.DataAnnotations;

namespace CarShareApi.ViewModels.Users
{
    public class RegisterUpdateRequest
    {
        //Register update request provides the objects needed to allow the 
        //user to update their registration records which includes all of 
        //the same Fields used when they originally registered period 
        //again this will interact with the user and registrations table.
        
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
        [StringLength(20, ErrorMessage = 
            "The {0} must be at least {2} characters long.", 
            MinimumLength = 1)]
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