using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarShareApi;
using CarShareApi.Controllers;
using CarShareApi.ViewModels;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class CarsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            IEnumerable<CarViewModel> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            var result = controller.Get(2);

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
