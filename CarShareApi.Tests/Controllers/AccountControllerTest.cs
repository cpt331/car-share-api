using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using CarShareApi.Controllers;
using CarShareApi.Models.Providers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Models.ViewModels;
using CarShareApi.Tests.Fakes;
using CarShareApi.ViewModels;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private readonly string InvalidEmails = @"plainaddress
                #@%^%#$@#$@#.com
                @example.com
                Joe Smith <email@example.com>
                email.example.com
                email@example@example.com
                .email@example.com
                email.@example.com
                email..email@example.com
                あいうえお@example.com
                email@example.com (Joe Smith)
                email@example
                email@-example.com
                email@example.web
                email@111.222.333.44444
                email@example..com
                Abc..123@example.com";

        private readonly string ValidEmails = @"email@example.com
                firstname.lastname@example.com
                email@subdomain.example.com
                firstname+lastname@example.com
                1234567890@example.com
                email@example-one.com
                _______@example.com
                email@example.name
                email@example.museum
                email@example.co.jp
                firstname-lastname@example.com";

        private IUserRepository UserRepository { get; set; }
        private ICarRepository CarRepository { get; set; }
        private IBookingRepository BookingRepository { get; set; }
        private IPaymentMethodRepository PaymentMethodRepository { get; set; }
        private UserService UserService { get; set; }
        private AccountController Controller { get; set; }
        private IRegistrationRepository RegistrationRepository { get; set; }
        private IEmailProvider EmailProvider { get; set; }

        [TestInitialize]
        public void SetupTests()
        {
            var configuration = new HttpConfiguration();

            UserRepository = new FakeUserRepository();
            RegistrationRepository =
                new FakeRegistrationRepository(); //todo make this
            PaymentMethodRepository =
                new FakePaymentMethodRepository(
                    new List<PaymentMethod>()); //todo make this
            EmailProvider = new FakeEmailProvider();

            var carsJson = GetInputFile("Cars.json").ReadToEnd();
            var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson);
            CarRepository = new FakeCarRepository(cars);

            var bookingsJson = GetInputFile("Bookings.json").ReadToEnd();
            var bookings =
                JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);
            BookingRepository = new FakeBookingRepository(bookings);

            UserService = new UserService(UserRepository,
                RegistrationRepository, BookingRepository,
                PaymentMethodRepository, EmailProvider, CarRepository);

            Controller = new AccountController(UserService);
            Controller.Configuration = configuration;
            TestStartupConfiguration.HttpConfiguration = configuration;
            TestStartupConfiguration.UserRepository = UserRepository;
            TestStartupConfiguration.UserService = UserService;
        }

        //http://www.vannevel.net/2015/03/21/how-to-unit-test-your-owin-configured-oauth2-implementation/
        [TestMethod]
        public async Task Login_InvalidUser_ReturnsBadRequest()
        {
            var logonRequest = new LogonRequest
            {
                Email = "fds",
                Password = "fdsfds"
            };

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x =>
                    x.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username",
                            logonRequest.Email),
                        new KeyValuePair<string, string>("password",
                            logonRequest.Password),
                        new KeyValuePair<string, string>("grant_type",
                            "password")
                    })).PostAsync();


                Assert.IsFalse(response.IsSuccessStatusCode);
                Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
            }
        }

        //http://www.vannevel.net/2015/03/21/how-to-unit-test-your-owin-configured-oauth2-implementation/
        [TestMethod]
        public async Task Login_ValidUser_ReturnsToken()
        {
            var logonRequest = new LogonRequest
            {
                Email = "user1@gmail.com",
                Password = "password1"
            };

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x =>
                    x.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username",
                            logonRequest.Email),
                        new KeyValuePair<string, string>("password",
                            logonRequest.Password),
                        new KeyValuePair<string, string>("grant_type",
                            "password")
                    })).PostAsync();


                Assert.IsTrue(response.IsSuccessStatusCode);

                var responseContent =
                    await response.Content.ReadAsStringAsync();


                Console.WriteLine(responseContent);

                Assert.IsFalse(string.IsNullOrWhiteSpace(responseContent));
            }
        }

        [TestMethod]
        public async Task Account_TokenProvided_ReturnsUser()
        {
            Console.WriteLine("Starting test");
            var logonRequest = new LogonRequest
            {
                Email = "user1@gmail.com",
                Password = "password1"
            };

            Console.WriteLine("Loading test server");
            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                Console.WriteLine("Sending token request");
                var response = await server.CreateRequest("/token").And(x =>
                    x.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username",
                            logonRequest.Email),
                        new KeyValuePair<string, string>("password",
                            logonRequest.Password),
                        new KeyValuePair<string, string>("grant_type",
                            "password")
                    })).PostAsync();


                Assert.IsTrue(response.IsSuccessStatusCode);

                var responseContent =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine("Received response");

                var tokenResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        responseContent);
                var token = tokenResponse["access_token"] as string;

                Console.WriteLine(responseContent);

                Assert.IsFalse(string.IsNullOrWhiteSpace(responseContent));

                Console.WriteLine("Requesting current user");
                var accountResponse = await server
                    .CreateRequest("/api/account/current")
                    .AddHeader("Authorization", $"Bearer {token}").GetAsync();

                var accountContent =
                    await accountResponse.Content.ReadAsStringAsync();

                var accountViewModel =
                    JsonConvert
                        .DeserializeObject<UserViewModel>(accountContent);

                Console.WriteLine("Response was: ");
                Console.WriteLine(JsonConvert.SerializeObject(accountViewModel,
                    Formatting.Indented));

                Assert.IsNotNull(accountViewModel);
            }
        }

        [TestMethod]
        public async Task Account_NoTokenProvided_ReturnsNull()
        {
            Console.WriteLine("Starting test");


            Console.WriteLine("Loading test server");
            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                Console.WriteLine("Requesting current user");
                var accountResponse = await server
                    .CreateRequest("/api/account/current").GetAsync();

                var accountContent =
                    await accountResponse.Content.ReadAsStringAsync();

                var accountViewModel =
                    JsonConvert
                        .DeserializeObject<UserViewModel>(accountContent);

                Console.WriteLine("Response was: ");
                Console.WriteLine(JsonConvert.SerializeObject(accountViewModel,
                    Formatting.Indented));

                Assert.IsTrue(
                    accountViewModel == null ||
                    string.IsNullOrWhiteSpace(accountViewModel.Email));
            }
        }


        [TestMethod]
        public void Register_NoEmailProvided_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The email field is required.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_NoLicenceProvided_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "user3@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The licencenumber field is required.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_NoDateOfBirthProvided_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "user2@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "88665522",
                DateOfBirth = null
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The DateOfBirth field is required.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_NoFirstNameProvided_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "homer.simpson6@gmail.com",
                FirstName = "",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The FirstName field is required.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_NoLastNameProvided_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "homer.simpson5@gmail.com",
                FirstName = "Homer",
                LastName = "",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The LastName field is required.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_PasswordMisMatch_ReturnsValidationError()
        {
            var model = new RegisterRequest
            {
                Email = "homer.simpson4@gmail.com",
                FirstName = "Homer",
                LastName = "",
                Password = "Simpson01",
                ConfirmPassword = "Simpson02",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains(
                "The password and confirmation password do not match.",
                StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void Register_InvalidEmailsProvided_ReturnsValidationError()
        {
            //Test invalid emails
            foreach (var line in InvalidEmails
                .Split(new[] {Environment.NewLine},
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()))
            {
                Console.WriteLine($"Testing {line}");


                var model = new RegisterRequest
                {
                    Email = line,
                    FirstName = "Homer",
                    LastName = "Simpson",
                    Password = "123",
                    ConfirmPassword = "123",
                    LicenceNumber = "123456789",
                    DateOfBirth = new DateTime(2000, 1, 1)
                };

                Controller.Validate(model);
                var response = Controller.Register(model);

                Assert.AreEqual("Form has validation errors", response.Message);
                Assert.IsTrue(response.Errors.Contains(
                    "The Email field is not a valid e-mail address.",
                    StringComparer.InvariantCultureIgnoreCase));
            }
        }

        [TestMethod]
        public void Register_ValidEmailsProvided_ReturnsNoValidationError()
        {
            //Test valid emails
            foreach (var line in ValidEmails
                .Split(new[] {Environment.NewLine},
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()))
            {
                Console.WriteLine($"Testing {line}");


                var model = new RegisterRequest
                {
                    Email = line,
                    FirstName = "Homer",
                    LastName = "Simpson",
                    Password = "123",
                    ConfirmPassword = "123",
                    LicenceNumber = "123456789",
                    DateOfBirth = new DateTime(2000, 1, 1)
                };

                Controller.Validate(model);
                var response = Controller.Register(model);


                Assert.IsFalse(response.Errors.Contains(
                    "The Email field is not a valid e-mail address.",
                    StringComparer.InvariantCultureIgnoreCase));
            }
        }

        [TestMethod]
        public void Register_UserIsRegistered_RegistrationIsPresent()
        {
            var model = new RegisterRequest
            {
                Email = "homer.simpson3@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(1970, 1, 1)
            };

            Controller.Validate(model);

            var registerResponse = Controller.Register(model);

            Assert.IsTrue(registerResponse.Success);

            //access repository just to grab the accountId
            var accountId = UserRepository.FindByEmail(model.Email).AccountID;

            var user = UserService.FindUser(accountId);
            Assert.IsNotNull(user.LicenceNumber);
        }

        [TestMethod]
        public void Register_UserIsRegistered_UserCanLogon()
        {
            var model = new RegisterRequest
            {
                Email = "homer.simpson2@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(1970, 1, 1)
            };

            Controller.Validate(model);

            var registerResponse = Controller.Register(model);

            Assert.IsTrue(registerResponse.Success);

            var logonResponse = UserService.Logon(new LogonRequest
            {
                Email = model.Email,
                Password = model.Password
            });

            Assert.IsTrue(logonResponse.Success);
        }


        [TestMethod]
        public void Register_UserIsRegistered_PasswordIsEncrypted()
        {
            Console.WriteLine("Starting test");
            var model = new RegisterRequest
            {
                Email = "homer.simpson1@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01",
                LicenceNumber = "123456789",
                DateOfBirth = new DateTime(1970, 1, 1)
            };

            Console.WriteLine("Validating model");
            Controller.Validate(model);

            Console.WriteLine("Registering model");
            var registerResponse = Controller.Register(model);

            Console.WriteLine("Response was");
            Console.WriteLine(JsonConvert.SerializeObject(registerResponse,
                Formatting.Indented));

            Assert.IsTrue(registerResponse.Success);

            Console.WriteLine("Find user by email");
            var user = UserRepository.FindByEmail(model.Email);
            Console.WriteLine(
                JsonConvert.SerializeObject(user, Formatting.Indented));

            Console.WriteLine("Comparing {0} to {1}", model.Password,
                user.Password);
            Assert.IsFalse(model.Password.Equals(user.Password,
                StringComparison.InvariantCultureIgnoreCase));
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