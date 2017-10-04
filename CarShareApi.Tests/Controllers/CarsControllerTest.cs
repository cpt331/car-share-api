using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarShareApi;
using CarShareApi.Controllers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Tests.Fakes;
using CarShareApi.ViewModels;

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
            CarRepository = new FakeCarRepository();
            CarCategoryRepository = new FakeCarCategoryRepository();
            CarService = new CarService(CarRepository, CarCategoryRepository);

            Controller = new CarsController(CarService);
            Controller.Configuration = configuration;
            TestStartupConfiguration.HttpConfiguration = configuration;
            TestStartupConfiguration.CarRepository = CarRepository;
            TestStartupConfiguration.CarService = CarService;
        }

        [TestMethod]
        public void Get()
        {
            // Arrange
            //CarsController controller = new CarsController();

            // Act
            IEnumerable<CarViewModel> result = Controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            //CarsController controller = new CarsController();

            // Act
            var result = Controller.Get(2);

            // Assert
            Assert.AreEqual("Model X", result.Model);
        }

        //[TestMethod]
        //public void Post()
        //{
        //    // Arrange
        //    CarsController controller = new CarsController();

        //    // Act
        //    controller.Post("value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Put()
        //{
        //    // Arrange
        //    CarsController controller = new CarsController();

        //    // Act
        //    controller.Put(5, "value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    CarsController controller = new CarsController();

        //    // Act
        //    controller.Delete(5);

        //    // Assert
        //}
    }
}
