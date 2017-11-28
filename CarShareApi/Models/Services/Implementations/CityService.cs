//======================================
//
//Name: CityService.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class CityService : ICityService
    {
        //this module allows for cities within the cities table to be maanged

        public CityService(ICityRepository cityRepository)
        {
            //create city repo
            CityRepository = cityRepository;
        }

        private ICityRepository CityRepository { get; }

        public List<CityViewModel> FindAllCities()
        {
            //return a list of all cities
            return CityRepository.FindAll().Select(x => 
            new CityViewModel(x)).ToList();
        }

        public CityViewModel FindCity(string cityName)
        {
            //method to find city using the city name
            var city = new CityViewModel(CityRepository.Find(cityName));
            return city;
        }

        public void Dispose()
        {
            CityRepository?.Dispose();
        }
    }
}