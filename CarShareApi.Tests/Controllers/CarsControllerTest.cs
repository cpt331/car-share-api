﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using CarShareApi.Controllers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Tests.Fakes;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Cars;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class CarsControllerTest
    {
        private ICityRepository CityRepository { get; set; }
        private ICarCategoryRepository CarCategoryRepository { get; set; }
        private ICarRepository CarRepository { get; set; }
        private ICarService CarService { get; set; }
        private CarsController Controller { get; set; }

        [TestInitialize]
        public void SetupTests()
        {
            var configuration = new HttpConfiguration();

            var carsJson = GetInputFile("Cars.json").ReadToEnd();
            var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson);
            CarRepository = new FakeCarRepository(cars);

            var categoriesJson = GetInputFile("Categories.json").ReadToEnd();
            var categories =
                JsonConvert
                    .DeserializeObject<List<CarCategory>>(categoriesJson);
            CarCategoryRepository = new FakeCarCategoryRepository(categories);

            var citiesJson = GetInputFile("Cities.json").ReadToEnd();
            var cities = JsonConvert.DeserializeObject<List<City>>(citiesJson);
            CityRepository = new FakeCityRepository(cities);

            CarService = new CarService(CarRepository, 
                                        CarCategoryRepository,
                                        CityRepository);

            Controller = new CarsController(CarService);
            Controller.Configuration = configuration;
            TestStartupConfiguration.HttpConfiguration = configuration;
            TestStartupConfiguration.CarRepository = CarRepository;
            TestStartupConfiguration.CarService = CarService;
        }

        [TestMethod]
        public void CarUpdate_OutsideCityLimits_DoesNotUpdate()
        {
            //Arrange
            var request = new UpdateCarRequest
            {
                Id = 95,
                CarCategory = "Sedan",
                Make = "Mercedes-Benz",
                Model = "CLS250",
                Transmission = "AT",
                Status = "Available",
                LatPos = (decimal)-17.571280,
                LongPos = (decimal)152.964494
            };

            // Act
            UpdateCarResponse result = Controller.Update(request);

            //Assert
            Assert.AreEqual("No cities are within a 50000m radius", result.Message);
        }

        [TestMethod]
        public void CarUpdate_InsideCityLimits_DoesUpdate()
        {
            //Arrange
            var request = new UpdateCarRequest
            {
                Id = 95,
                CarCategory = "Sedan",
                Make = "Mercedes-Benz",
                Model = "CLS250",
                Transmission = "AT",
                Status = "Available",
                LatPos = (decimal)-37.813600,
                LongPos = (decimal)144.963100
            };

            // Act
            UpdateCarResponse result = Controller.Update(request);

            //Assert
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void CarSearch_InSuburb_OnlyReturnsThatSuburb()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Suburb = "Sydney"
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Suburb.Equals(criteria.Suburb,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void CarSearch_ByMake_OnlyReturnsThatMake()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Make = "Toyota"
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Make.Equals(criteria.Make,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void CarSearch_ByModel_OnlyReturnsThatModel()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Model = "CLS250"
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Model.Equals(criteria.Model,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void CarSearch_WithMaxResults_DoesNotExceedLimit()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                MaxResults = 10
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            Assert.IsTrue(result.Count() <= criteria.MaxResults.Value);
        }

        [TestMethod]
        public void CarSearch_Everything_OnlyActiveCars()
        {
            // Arrange
            var criteria = new CarSearchCriteria();

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Status.Equals("Available",
                    StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void CarSearch_NearLocation_ReturnsCalculcatedDistances()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Latitude = (decimal) -33.89806198,
                Longitude = (decimal) 151.17925644
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Distance.HasValue);
        }

        [TestMethod]
        public void CarSearch_NearLocationAndRadius_ReturnsCarsWithinRadius()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Latitude = (decimal) -33.89806198,
                Longitude = (decimal) 151.17925644,
                Radius = 10000
            };

            // Act
            var result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
                Assert.IsTrue(car.Distance.HasValue &&
                              car.Distance.Value <= criteria.Radius.Value);
        }

        public static TextReader GetInputFile(string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            var path = "CarShareApi.Tests.Fakes.Data";

            return new StreamReader(
                thisAssembly.GetManifestResourceStream(path + "." + filename));
        }
    }
}