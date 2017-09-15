using CarShareApi.Models.Repositories;
using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models.Services.Implementations
{
    public class CarService : ICarService
    {
        private ICarRepository CarRepository { get; set; }

        public CarService(ICarRepository carRepository)
        {
            CarRepository = carRepository;
        }
        public List<CarViewModel> FindCarsByLocation(double lat, double lng)
        {
            var cars = CarRepository.FindAll();
            var result = new List<CarViewModel>();
            foreach(var car in cars)
            {
                var distance = Haversine(lat, car.Lat, lng, car.Lng);
                result.Add(new CarViewModel(car)
                {
                    Distance = distance
                });
            }
            return result.OrderBy(x=>x.Distance).ToList();
        }

        public CarViewModel FindCar(int id)
        {
            return new CarViewModel(CarRepository.Find(id));
        }

        public List<CarViewModel> FindAllCars()
        {
            return CarRepository.FindAll().Select(x => new CarViewModel(x)).ToList();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/41621957/a-more-efficient-haversine-function
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon1"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        private static double Haversine(double lat1, double lat2, double lon1, double lon2)
        {
            const double r = 6371; // meters

            var sdlat = Math.Sin((lat2 - lat1) / 2);
            var sdlon = Math.Sin((lon2 - lon1) / 2);
            var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return d;
        }

        public void DeleteCar(int id)
        {
            CarRepository.Delete(id);
        }
    }
}