﻿using CarShareApi.Models;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class CarsController : ApiController
    {
        private ICarService CarService;
        //public CarsController()
        //{
        //    CarService = new CarService(new CarRepository(new CarShareContext()));
        //}
        public CarsController(ICarService carService)
        {
            CarService = carService;
        }


        /// <summary>
        /// Return a list of all cars
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/cars")]
        public IEnumerable<CarViewModel> Get()
        {
            return CarService.FindAllCars();
        }

        /// <summary>
        /// Retrieve a list of all cars sorted by distance from the passed in coordinates
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        [HttpGet, Route("api/cars/coords")]
        public IEnumerable<CarViewModel> Get(double lat, double lng)
        {
            return CarService.FindCarsByLocation(lat,lng);
        }

        // GET api/values/5
        /// <summary>
        /// Retrieve a single car by passing in its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("api/cars/detail")]
        public CarViewModel Get(int id)
        {
            return CarService.FindCar(id);
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        [HttpGet, Route("api/cars/delete")]
        public void Delete(int id)
        {
            CarService.DeleteCar(id);
        }
    }
}
