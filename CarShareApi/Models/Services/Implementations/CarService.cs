using CarShareApi.Models.Repositories;
using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels.Cars;

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
                    var searchCoordinate = new GeoCoordinate((double)criteria.Latitude.Value, (double)criteria.Longitude.Value);
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

        public UpdateCarResponse UpdateCar(UpdateCarRequest request)
        {
           

            var category = CarCategoryRepository.Find(request.CarCategory);
            if (category == null)
            {
                return new UpdateCarResponse
                {
                    Message = $"Category {request.CarCategory} does not exist",
                    Success = false
                };
            }

            Car car;
            if (request.Id.HasValue)
            {
                car = CarRepository.Find(request.Id.Value);
                if (car == null)
                {
                    return new UpdateCarResponse
                    {
                        Message = $"Vehicle {request.Id} does not exist",
                        Success = false
                    };
                }
            }
            else
            {
                car = new Car();
            }

            car.CarCategory = request.CarCategory;
            car.Make = request.Make;
            car.Model = request.Model;
            car.Status = request.Status;
            car.Suburb = request.Suburb;
            car.LatPos = request.LatPos;
            car.LongPos = request.LongPos;
            car.Transmission = request.Transmission;

            if (request.Id.HasValue)
            {
                var updatedCar = CarRepository.Update(car);
            }
            else
            {
                var updatedCar = CarRepository.Add(car);
            }
                

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