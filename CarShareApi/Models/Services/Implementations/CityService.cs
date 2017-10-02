using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class CityService : ICityService
    {
        private ICityRepository CityRepository { get; set; }

        public CityService(ICityRepository cityRepository)
        {
            CityRepository = cityRepository;
        }
        public List<CityViewModel> FindAllCities()
        {
            return CityRepository.FindAll().Select(x=> new CityViewModel(x)).ToList();
        }
        public CityViewModel FindCity(string cityName)
        {
            var city = new CityViewModel(CityRepository.Find(cityName));
            return city;
        }
    }
}