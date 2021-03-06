﻿//======================================
//
//Name: BookingsControllerTest.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CarShareApi.Controllers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Tests.Fakes;
using CarShareApi.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CarShareApi.Tests.Controllers
{
    //this class provides the unit testing for bookings
    [TestClass]
    public class BookingsControllerTest
    {
        //set up all interfaces
        private BookingsController Controller { get; set; }

        private IUserRepository UserRepository { get; set; }
        private IRegistrationRepository RegistrationRepository { get; set; }
        private ICarCategoryRepository CarCategoryRepository { get; set; }
        private ICarRepository CarRepository { get; set; }
        private ICityRepository CityRepository { get; set; }

        private ITransactionHistoryRepository TransactionHistoryRepository
        {
            get;
            set;
        }

        private IBookingRepository BookingRepository { get; set; }
        private IPaymentMethodRepository PaymentMethodRepository { get; set; }

        private BookingService BookingService { get; set; }

        [TestInitialize]
        public void SetupTests()
        {
            //initiating testin configurations / connections
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

            var bookingsJson = GetInputFile("Bookings.json").ReadToEnd();
            var bookings =
                JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            BookingRepository = new FakeBookingRepository(bookings);

            TransactionHistoryRepository =
                new FakeTransactionHistoryRepository(
                    new List<TransactionHistory>());
            UserRepository = new FakeUserRepository();
            RegistrationRepository = new FakeRegistrationRepository();
            PaymentMethodRepository =
                new FakePaymentMethodRepository(
                    new List<PaymentMethod>()
                    {
                        new PaymentMethod
                        {
                            AccountID =1,
                            CardName = "Visa",
                            CardNumber = "123456789",
                            CardVerificationValue = "551",
                            ExpiryMonth = 11,
                            ExpiryYear = 2020
                        }
                    }); //todo make this

            BookingService = new BookingService(
                BookingRepository,
                CarRepository,
                UserRepository,
                CarCategoryRepository,
                CityRepository,
                TransactionHistoryRepository,
                PaymentMethodRepository
            );


            Controller = new BookingsController(BookingService);
            Controller.Configuration = configuration;
            TestStartupConfiguration.HttpConfiguration = configuration;
            TestStartupConfiguration.UserRepository = UserRepository;
            TestStartupConfiguration.BookingRepository = BookingRepository;
            TestStartupConfiguration.CityRepository = CityRepository;
            TestStartupConfiguration.CarCategoryRepository =
                CarCategoryRepository;
            TestStartupConfiguration.RegistrationRepository =
                RegistrationRepository;
            TestStartupConfiguration.TransactionHistoryRepository =
                TransactionHistoryRepository;
            TestStartupConfiguration.CarRepository = CarRepository;
            TestStartupConfiguration.BookingService = BookingService;
        }

        [TestMethod]
        public void BookingOpen_VehicleIsAvailable_CarIsBooked()
        {
            //pass login and check if vehicle is open and can be booked
            Thread.CurrentPrincipal = new TestPrincipal(
                new Claim("name", "John Doe"),
                new Claim(ClaimTypes.PrimarySid, "1"));

            // Arrange
            var vehicleId = 95;

            // Act
            var result = Controller.Open(vehicleId);

            Console.WriteLine($"{result.Success}: {result.Message}");

            //assert
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void BookingOpen_VehicleIsUnavailable_CarIsNotBooked()
        {
            //Controller.RequestContext.Principal = 
            Thread.CurrentPrincipal = new TestPrincipal(
                new Claim("name", "John Doe"),
                new Claim(ClaimTypes.PrimarySid, "1"));

            // Arrange
            var vehicleId = 129;

            // Act
            var result = Controller.Open(vehicleId);

            Console.WriteLine($"{result.Success}: {result.Message}");

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void BookingCheck_VehicleIsInRange_CheckInIsAllowed()
        {
            //Controller.RequestContext.Principal = 
            Thread.CurrentPrincipal = new TestPrincipal(
                new Claim("name", "John Doe"),
                new Claim(ClaimTypes.PrimarySid, "1"));

            // Arrange
            var request = new CloseBookingCheckRequest
            {
                BookingId = 5,
                Latitude = (decimal) -33.1,
                Longitude = (decimal) 151.1
            };

            // Act
            var result = Controller.CloseCheck(request);

            Console.WriteLine($"{result.Success}: {result.Message}");

            //assert
            Assert.IsFalse(result.Success);
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