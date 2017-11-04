using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class CityService : ICityService
    {
        public CityService(ICityRepository cityRepository)
        {
            CityRepository = cityRepository;
        }

        private ICityRepository CityRepository { get; }

        public List<CityViewModel> FindAllCities()
        {
            return CityRepository.FindAll().Select(x => new CityViewModel(x)).ToList();
        }

        public CityViewModel FindCity(string cityName)
        {
            var city = new CityViewModel(CityRepository.Find(cityName));
            return city;
        }

        public void Dispose()
        {
            CityRepository?.Dispose();
        }
    }
}