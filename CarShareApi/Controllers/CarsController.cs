//======================================
//
//Name: CarsController.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Cars;
using Newtonsoft.Json;
using NLog;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class CarsController : ApiController
    {
        private static readonly Logger Logger = 
            LogManager.GetCurrentClassLogger();
        private readonly ICarService CarService;

        //inject service to make testing easier
        public CarsController(ICarService carService)
        {
            CarService = carService;
        }


        /// <summary>
        ///     Return a list of all cars
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cars")]
        public IEnumerable<CarViewModel> Get()
        {
            return CarService.FindAllCars();
        }

        /// <summary>
        ///     Retrieve a list of all cars sorted by distance from the passed in coordinates
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="radius">the radius in metres</param>
        /// <param name="max">the maximum results</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cars/coords")]
        public IEnumerable<CarViewModel> Get(double lat, double lng, 
            double radius = 5000, int max = 100,
            string carCategory = "")
        {
            var cars = CarService.FindCarsByLocation(lat, lng);
            return cars.Where(x => x.Distance.HasValue && x.Distance <= 
            radius).Take(max);
        }

        /// <summary>
        ///     Search a list of all cars sorted by distance from the passed in coordinates
        /// </summary>
        /// <param name="criteria">Criteria to perform search on</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cars/search")]
        public IEnumerable<CarViewModel> Search(CarSearchCriteria criteria)
        {
            Logger.Debug("CarSearchCriteria Received: {0}", 
                JsonConvert.SerializeObject(criteria, Formatting.Indented));
            var cars = CarService.SearchCars(criteria);
            return cars;
        }

        /// <summary>
        ///     Get a list of available car categories
        /// </summary>
        /// <returns> Return a list of available car categories</returns>
        [HttpGet]
        [Route("api/cars/categories")]
        public IEnumerable<CarCategoryViewModel> Categories()
        {
            var categories = CarService.GetCarCategories();
            return categories;
        }

        /// <summary>
        ///     Get a list of available car statuses for selection during creation/update
        /// </summary>
        /// <returns>Return a list of available car statuses for selection during creation/update</returns>
        [HttpGet]
        [Route("api/cars/statuses")]
        public IEnumerable<string> Statuses()
        {
            var statuses = CarService.GetCarStatuses();
            return statuses;
        }

        [HttpPost]
        [Route("api/cars/update")]
        public UpdateCarResponse Update(UpdateCarRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => 
                ModelState[key].Errors.Select(x => x.ErrorMessage));
                var response = new UpdateCarResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
                return response;
            }
            //send request to the car service and return the 
            //response (success or fail)
            return CarService.UpdateCar(request);
        }

        // GET api/values/5
        /// <summary>
        ///     Retrieve a single car by passing in its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cars/detail/{id}")]
        public CarViewModel Get(int id)
        {
            return CarService.FindCar(id);
        }

        //// DELETE api/values/5
        [HttpGet]
        [Route("api/cars/delete")]
        public void Delete(int id)
        {
            CarService.DeleteCar(id);
        }
    }
}