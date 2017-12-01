//======================================
//
//Name: RegisterViewModel.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels.Users
{
    public class RegisterViewModel
    {
        //Register view model provides the objects to handle receiving the
        //registration details of the user and in addition pass / failure
        //boolean and a message that will give a display to the user.

        public bool Success { get; set; }
        public string Message { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DriversLicenceID { get; set; }
        public string DriversLicenceState { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
    }
}