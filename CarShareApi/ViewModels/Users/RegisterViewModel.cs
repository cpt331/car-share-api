namespace CarShareApi.ViewModels.Users
{
    public class RegisterViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string DriversLicenceID { get; set; }
        public string DriversLicenceState { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
    }
}