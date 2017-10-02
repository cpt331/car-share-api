using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services
{
    public interface ICityService
    {
        List<CityViewModel> FindAllCities();
        CityViewModel FindCity(string cityName);
    }
}