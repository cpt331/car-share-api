using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarShareApi;
using CarShareApi.Controllers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Tests.Fakes;
using CarShareApi.ViewModels;
using Newtonsoft.Json;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class CarsControllerTest
    {
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
            var categories = JsonConvert.DeserializeObject<List<CarCategory>>(categoriesJson);
            CarCategoryRepository = new FakeCarCategoryRepository(categories);

            CarService = new CarService(CarRepository, CarCategoryRepository);

            Controller = new CarsController(CarService);
            Controller.Configuration = configuration;
            TestStartupConfiguration.HttpConfiguration = configuration;
            TestStartupConfiguration.CarRepository = CarRepository;
            TestStartupConfiguration.CarService = CarService;
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
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Suburb.Equals(criteria.Suburb, StringComparison.InvariantCultureIgnoreCase));
            }
            
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
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Make.Equals(criteria.Make, StringComparison.InvariantCultureIgnoreCase));
            }

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
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Model.Equals(criteria.Model, StringComparison.InvariantCultureIgnoreCase));
            }

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
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            Assert.IsTrue(result.Count() <= criteria.MaxResults.Value);

        }

        [TestMethod]
        public void CarSearch_Everything_OnlyActiveCars()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
            };

            // Act
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Status.Equals("Active", StringComparison.InvariantCultureIgnoreCase));
            }

        }

        [TestMethod]
        public void CarSearch_NearLocation_ReturnsCalculcatedDistances()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Latitude = (decimal)-33.89806198,
                Longitude = (decimal)151.17925644
            };

            // Act
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Distance.HasValue);
            }

        }

        [TestMethod]
        public void CarSearch_NearLocationAndRadius_ReturnsCarsWithinRadius()
        {
            // Arrange
            var criteria = new CarSearchCriteria
            {
                Latitude = (decimal)-33.89806198,
                Longitude = (decimal)151.17925644,
                Radius = 10000
            };

            // Act
            IEnumerable<CarViewModel> result = Controller.Search(criteria);

            Console.WriteLine("Testing {0} Cars", result.Count());

            foreach (var car in result)
            {
                Assert.IsTrue(car.Distance.HasValue && car.Distance.Value <= criteria.Radius.Value);
            }

        }

        public static TextReader GetInputFile(string filename)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            string path = "CarShareApi.Tests.Fakes.Data";

            return new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + filename));
        }
    }
}
