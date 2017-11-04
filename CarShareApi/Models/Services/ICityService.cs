using System;
using System.Collections.Generic;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services
{
    public interface ICityService : IDisposable
    {
        List<CityViewModel> FindAllCities();
        CityViewModel FindCity(string cityName);
    }
}