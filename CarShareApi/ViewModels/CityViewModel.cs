using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CityViewModel
    {
        public string CityName { get; set; }
        public decimal LatPos { get; set; }
        public decimal LongPos { get; set; }
        public List<CarViewModel> Cars { get; set; }

        public CityViewModel() { }

        public CityViewModel(City city)
        {
            CityName = city.CityName;
            LatPos = city.LatPos;
            LongPos = city.LongPos;
        }
    }
}