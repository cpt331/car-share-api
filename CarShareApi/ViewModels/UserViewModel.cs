using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class UserViewModel
    {
        public int AccountId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public string LicenceNumber { get; set; }
        public string LicenceState { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }

        public int? OpenBookingId { get; set; }
        public bool HasOpenBooking { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            AccountId = user.AccountID;
            Firstname = user.FirstName;
            Lastname = user.LastName;
            Email = user.Email;
            if (user.Registration != null)
            {
                LicenceNumber = user.Registration.DriversLicenceID;
                LicenceState = user.Registration.DriversLicenceState;
                AddressLine1 = user.Registration.AddressLine1;
                AddressLine2 = user.Registration.AddressLine2;
                Suburb = user.Registration.Suburb;
                State = user.Registration.State;
                Postcode = user.Registration.Postcode;
                PhoneNumber = user.Registration.PhoneNumber;
                DateOfBirth = user.Registration.DateOfBirth.ToString("dd/MM/yyyy");
            }
        }
    }
}