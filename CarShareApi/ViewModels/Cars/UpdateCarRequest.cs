namespace CarShareApi.ViewModels
{
    public class UpdateCarRequest
    {
        //Update Car request provides the user with the necessary Fields to 
        //make an update to a current car within the system
        public int? Id { get; set; }

        public string Model { get; set; }
        public string Make { get; set; }
        public string CarCategory { get; set; }
        public string Transmission { get; set; }
        public string Status { get; set; }
        public decimal LatPos { get; set; }
        public decimal LongPos { get; set; }
    }
}