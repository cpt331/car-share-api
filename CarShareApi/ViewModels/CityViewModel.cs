using System.Collections.Generic;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CityViewModel
    {
        //The cityview model handles the object related to the city which 
        //includes the city names and their geographical position.
        public CityViewModel()
        {
        }

        public CityViewModel(City city)
        {
            CityName = city.CityName;
            LatPos = city.LatPos;
            LongPos = city.LongPos;
        }

        public string CityName { get; set; }
        public decimal LatPos { get; set; }
        public decimal LongPos { get; set; }
        public List<CarViewModel> Cars { get; set; }
    }
}