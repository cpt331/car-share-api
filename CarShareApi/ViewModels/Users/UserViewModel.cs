//======================================
//
//Name: UserViewModel.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using CarShareApi.Models;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class UserViewModel
    {
        //User view model provides the objects needed to handle displaying 
        //a user account and their corresponding details for fields that 
        //relate to the user and registrations table.
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
            if (!string.IsNullOrWhiteSpace(user.UserGroup))
                HasAdminRights = Constants.UserAdminGroupName.Equals(
                    user.UserGroup.Trim(),
                    StringComparison.InvariantCultureIgnoreCase);
            else
                HasAdminRights = false;
            if (user.PaymentMethod != null)
                HasPaymentMethod = true;
        }

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
        public bool HasAdminRights { get; set; }
        public bool HasPaymentMethod { get; set; }
    }
}