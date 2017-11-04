namespace CarShareApi.ViewModels
{
    public class CarSearchCriteria
    {
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