using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class CarSearchCriteria
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Radius { get; set; }
        public int? MaxResults { get; set;}
        public string CarCategory { get; set; }
        public string Suburb { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

    }
}