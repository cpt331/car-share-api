//======================================
//
//Name: CarService.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Cars;

namespace CarShareApi.Models.Services.Implementations
{
    //this module provides the functionality for a user to search for a car

    public class CarService : ICarService
    {

        private ICityRepository CityRepository { get; set; }
        private ICarRepository CarRepository { get; }
        private ICarCategoryRepository CarCategoryRepository { get; }

        public CarService(
            ICarRepository carRepository, 
            ICarCategoryRepository carCategoryRepository,
            ICityRepository cityRepository)
        {
            CarRepository = carRepository;
            CarCategoryRepository = carCategoryRepository;
            CityRepository = cityRepository;
        }

        

        public List<CarViewModel> FindCarsByLocation(double lat, double lng)
        {
            //method returns all cars from the car table in the database 
            //then applies filters to show cars that are available

            var cars = CarRepository.FindAll();

            //dont show inactive cars
            cars.RemoveAll(x => x.Status.Equals(Constants.CarBookedStatus));

            var result = new List<CarViewModel>();
            foreach (var car in cars)
            {
                //use microsofts haversine formula (returns metres)
                var carCoordinate = new GeoCoordinate((double) car.LatPos, (double) car.LongPos);
                var searchCoordinate = new GeoCoordinate(lat, lng);
                var distance = carCoordinate.GetDistanceTo(searchCoordinate);
                result.Add(new CarViewModel(car)
                {
                    Distance = distance
                });
            }
            return result.OrderBy(x => x.Distance).ToList();
        }

        public List<CarViewModel> SearchCars(CarSearchCriteria criteria)
        {
            //build EF query for search criteria
            var carQuery = CarRepository.Query();

            //dont show inactive cars
            carQuery = carQuery.Where(x => x.Status.Equals(Constants.CarAvailableStatus));

            if (!string.IsNullOrWhiteSpace(criteria.CarCategory))
                carQuery = carQuery.Where(x => x.CarCategory.Equals(criteria.CarCategory));
            if (!string.IsNullOrWhiteSpace(criteria.Suburb))
                carQuery = carQuery.Where(x => x.Suburb.Equals(criteria.Suburb));
            if (!string.IsNullOrWhiteSpace(criteria.Make))
                carQuery = carQuery.Where(x => x.Make.Equals(criteria.Make));
            if (!string.IsNullOrWhiteSpace(criteria.Model))
                carQuery = carQuery.Where(x => x.Model.Equals(criteria.Model));

            //execute query
            var cars = carQuery.ToList().Select(x => new CarViewModel(x)).ToList();

            //perform distance calculation in memory
            if (criteria.Longitude.HasValue && criteria.Latitude.HasValue)
                foreach (var car in cars)
                {
                    //use microsofts haversine formula (returns metres)
                    var carCoordinate = new GeoCoordinate((double) car.LatPos, (double) car.LongPos);
                    var searchCoordinate = new GeoCoordinate((double) criteria.Latitude.Value,
                        (double) criteria.Longitude.Value);
                    car.Distance = carCoordinate.GetDistanceTo(searchCoordinate);
                }
            if (criteria.Radius.HasValue)
                cars.RemoveAll(x => !x.Distance.HasValue || x.Distance.Value > criteria.Radius.Value);

            cars = cars.OrderBy(x => x.Distance).ToList();

            if (criteria.MaxResults.HasValue)
                cars = cars.Take(criteria.MaxResults.Value).ToList();

            return cars;
        }

        public List<CarCategoryViewModel> GetCarCategories()
        {
            return CarCategoryRepository.FindAll().Select(x => new CarCategoryViewModel(x)).ToList();
        }

        public UpdateCarResponse UpdateCar(UpdateCarRequest request)
        {
            //this method allows cars to be updated after passing some basic checks

            //validate the a car category exists and fail if it doesnt
            var category = CarCategoryRepository.Find(request.CarCategory);
            if (category == null)
                return new UpdateCarResponse
                {
                    Message = $"Category {request.CarCategory} does not exist",
                    Success = false
                };

            //validate the a car exists and fail if it doesnt
            Car car;
            if (request.Id.HasValue)
            {
                car = CarRepository.Find(request.Id.Value);
                if (car == null)
                    return new UpdateCarResponse
                    {
                        Message = $"Vehicle {request.Id} does not exist",
                        Success = false
                    };
            }
            else
            {
                car = new Car();
            }

            //look through cities and ensure one is close enough for check in
            var cities = CityRepository.FindAll();
            City selectedCity = null;
            foreach (var city in cities)
            {
                //use microsofts haversine formula (returns metres)
                var cityCoordinate = new GeoCoordinate((double)city.LatPos, (double)city.LongPos);
                var currentCoordinate = new GeoCoordinate((double)request.LatPos, (double)request.LongPos);
                var distance = cityCoordinate.GetDistanceTo(currentCoordinate);
                if (distance < Constants.BookingMaxRangeFromCityCentre)
                {
                    selectedCity = city;
                    break;
                }
            }

            //validates that a selected city exists
            if (selectedCity == null)
            {
                return new UpdateCarResponse
                {
                    Message = "No cities are within a " +
                         $"{Constants.BookingMaxRangeFromCityCentre}m radius",
                    Success = false
                };
            }

            //assigns car values based on the parsed request to change the car
            car.CarCategory = request.CarCategory;
            car.Make = request.Make;
            car.Model = request.Model;
            car.Status = request.Status;
            car.Suburb = selectedCity.CityName;
            car.LatPos = request.LatPos;
            car.LongPos = request.LongPos;
            car.Transmission = request.Transmission;

            //switch to determine whether the car needs to be updated or added
            //to the car table

            if (request.Id.HasValue)
            {
                var updatedCar = CarRepository.Update(car);
            }
            else
            {
                var updatedCar = CarRepository.Add(car);
            }

            //message returned after car has been updated

            var response = new UpdateCarResponse
            {
                Success = true,
                Message = $"{request.Make} {request.Model} has been updated",
                Errors = null
            };

            return response;
        }

        public List<string> GetCarStatuses()
        {
            //returns the car status - this is a constants list for 
            //front end work
            return new List<string>
            {
                Constants.CarAvailableStatus,
                Constants.CarRemovedStatus
            };
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

        public void Dispose()
        {
            CarRepository?.Dispose();
            CarCategoryRepository?.Dispose();
        }
    }
}