//======================================
//
//Name: UserPrincipal.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels
{
    public class CarSearchCriteria
    {
        //This method allows for a user to be able to search for a car based 
        //on search critera including Car category the suburb and the make and
        //model.The latitude and longitude are also used for returning results

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double? Radius { get; set; }
        public int? MaxResults { get; set; }
        public string CarCategory { get; set; }
        public string Suburb { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}