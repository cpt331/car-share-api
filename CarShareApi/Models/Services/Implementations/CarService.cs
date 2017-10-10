using CarShareApi.Models.Repositories;
using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Services.Implementations
{
    public class CarService : ICarService
    {
        private ICarRepository CarRepository { get; set; }
        private ICarCategoryRepository CarCategoryRepository { get; set; }

       

        public CarService(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository)
        {
            CarRepository = carRepository;
            CarCategoryRepository = carCategoryRepository;
        }
        public List<CarViewModel> FindCarsByLocation(double lat, double lng)
        {
            var cars = CarRepository.FindAll();

            //dont show inactive cars
            cars.RemoveAll(x => x.Status.Equals(Constants.CarBookedStatus));

            var result = new List<CarViewModel>();
            foreach(var car in cars)
            {
                //use microsofts haversine formula (returns metres)
                var carCoordinate = new GeoCoordinate((double)car.LatPos, (double)car.LongPos);
                var searchCoordinate = new GeoCoordinate(lat, lng);
                var distance = carCoordinate.GetDistanceTo(searchCoordinate);
                result.Add(new CarViewModel(car)
                {
                    Distance = distance
                });
            }
            return result.OrderBy(x=>x.Distance).ToList();
        }

        public List<CarViewModel> SearchCars(CarSearchCriteria criteria)
        {

            //build EF query for search criteria
            var carQuery = CarRepository.Query();

            //dont show inactive cars
            carQuery = carQuery.Where(x => x.Status.Equals(Constants.CarAvailableStatus));

            if (!string.IsNullOrWhiteSpace(criteria.CarCategory))
            {
                carQuery = carQuery.Where(x => x.CarCategory.Equals(criteria.CarCategory));
            }
            if (!string.IsNullOrWhiteSpace(criteria.Suburb))
            {
                carQuery = carQuery.Where(x => x.Suburb.Equals(criteria.Suburb));
            }
            if (!string.IsNullOrWhiteSpace(criteria.Make))
            {
                carQuery = carQuery.Where(x => x.Make.Equals(criteria.Make));
            }
            if (!string.IsNullOrWhiteSpace(criteria.Model))
            {
                carQuery = carQuery.Where(x => x.Model.Equals(criteria.Model));
            }

            //execute query
            var cars  = carQuery.ToList().Select(x=> new CarViewModel(x)).ToList();
            
            //perform distance calculation in memory
            if (criteria.Longitude.HasValue && criteria.Latitude.HasValue)
            {
                foreach (var car in cars)
                {
                    //use microsofts haversine formula (returns metres)
                    var carCoordinate = new GeoCoordinate((double)car.LatPos, (double)car.LongPos);
                    var searchCoordinate = new GeoCoordinate(criteria.Latitude.Value, criteria.Longitude.Value);
                    car.Distance = carCoordinate.GetDistanceTo(searchCoordinate);
                }
            }
            if (criteria.Radius.HasValue)
            {
                cars.RemoveAll(x => !x.Distance.HasValue || x.Distance.Value > criteria.Radius.Value);
            }

            cars = cars.OrderBy(x => x.Distance).ToList();

            if (criteria.MaxResults.HasValue)
            {
                cars = cars.Take(criteria.MaxResults.Value).ToList();
            }

            return cars;
        }

        public List<CarCategoryViewModel> GetCarCategories()
        {
            return CarCategoryRepository.FindAll().Select(x=> new CarCategoryViewModel(x)).ToList();
        }

        public CarViewModel FindCar(int id)
        {
            return new CarViewModel(CarRepository.Find(id));
        }

        public List<CarViewModel> FindAllCars()
        {
            var cars = CarRepository.FindAll();
            return cars.Select(x => new CarViewModel(x)).ToList();
        }

        

        public void DeleteCar(int id)
        {
            CarRepository.Delete(id);
        }
    }
}