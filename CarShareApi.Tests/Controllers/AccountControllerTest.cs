using System;
using System.Linq;
using System.Web.Http;
using CarShareApi.Controllers;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Models.ViewModels;
using CarShareApi.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private IUserRepository UserRepository { get; set; }
        private UserService UserService { get; set; }
        private AccountController Controller { get; set; }

        private string InvalidEmails = @"plainaddress
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
        private string ValidEmails = @"email@example.com
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

        [TestInitialize]
        public void SetupTests()
        {
            UserRepository = new FakeUserRepository();
            UserService = new UserService(UserRepository);
            Controller = new AccountController(UserService);
            Controller.Configuration = new HttpConfiguration();
        }

        //[TestMethod]
        //public void LoginEmailIsProvided()
        //{

        //    var model = new LogonRequest()
        //    {
        //        Email = "homer.simpson@gmail.com",
        //        Password = "Simpson01"
        //    };

        //    Controller.Validate(model);
        //    var response = Controller.Register(model);

        //    Assert.AreEqual("Form has validation errors", response.Message);
        //    Assert.IsTrue(response.Errors.Contains("The FirstName field is required.", StringComparer.InvariantCultureIgnoreCase));

        //}

        [TestMethod]
        public void RegisterCheckEmailIsProvided()
        {

            var model = new RegisterRequest
            {
                Email = "",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01"
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors",response.Message);
            Assert.IsTrue(response.Errors.Contains("The email field is required.", StringComparer.InvariantCultureIgnoreCase));

        }

        [TestMethod]
        public void RegisterCheckFirstNameIsProvided()
        {

            var model = new RegisterRequest
            {
                Email = "homer.simpson@gmail.com",
                FirstName = "",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01"
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains("The FirstName field is required.", StringComparer.InvariantCultureIgnoreCase));

        }

        [TestMethod]
        public void RegisterCheckLastNameIsProvided()
        {

            var model = new RegisterRequest
            {
                Email = "homer.simpson@gmail.com",
                FirstName = "Homer",
                LastName = "",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01"
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains("The LastName field is required.", StringComparer.InvariantCultureIgnoreCase));

        }

        [TestMethod]
        public void RegisterCheckPasswordMatches()
        {

            var model = new RegisterRequest
            {
                Email = "homer.simpson@gmail.com",
                FirstName = "Homer",
                LastName = "",
                Password = "Simpson01",
                ConfirmPassword = "Simpson02"
            };

            Controller.Validate(model);
            var response = Controller.Register(model);

            Assert.AreEqual("Form has validation errors", response.Message);
            Assert.IsTrue(response.Errors.Contains("The password and confirmation password do not match.", StringComparer.InvariantCultureIgnoreCase));

        }

        [TestMethod]
        public void RegisterRejectsInvalidEmails()
        {
            //Test invalid emails
            foreach (var line in InvalidEmails
                .Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>x.Trim()))
            {
                Console.WriteLine($"Testing {line}");


                var model = new RegisterRequest
                {
                    Email = line,
                    FirstName = "Homer",
                    LastName = "Simpson",
                    Password = "123",
                    ConfirmPassword = "123"
                };

                Controller.Validate(model);
                var response = Controller.Register(model);

                Assert.AreEqual("Form has validation errors", response.Message);
                Assert.IsTrue(response.Errors.Contains("The Email field is not a valid e-mail address.", StringComparer.InvariantCultureIgnoreCase));
            }
        }

        [TestMethod]
        public void RegisterAcceptsValidEmails()
        {
            //Test valid emails
            foreach (var line in ValidEmails
                .Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()))
            {
                Console.WriteLine($"Testing {line}");


                var model = new RegisterRequest
                {
                    Email = line,
                    FirstName = "Homer",
                    LastName = "Simpson",
                    Password = "123",
                    ConfirmPassword = "123"
                };

                Controller.Validate(model);
                var response = Controller.Register(model);

                
                Assert.IsFalse(response.Errors.Contains("The Email field is not a valid e-mail address.", StringComparer.InvariantCultureIgnoreCase));
            }

        }

        [TestMethod]
        public void RegisterCanLoginAfter()
        {

            var model = new RegisterRequest
            {
                Email = "homer.simpson@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01"
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
        public void RegisterEncryptsPassword()
        {

            var model = new RegisterRequest
            {
                Email = "homer.simpson@gmail.com",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "Simpson01",
                ConfirmPassword = "Simpson01"
            };

            Controller.Validate(model);

            var registerResponse = Controller.Register(model);

            Assert.IsTrue(registerResponse.Success);

            var user = UserRepository.FindByEmail(model.Email);

            Console.WriteLine("Comparing {0} to {1}", model.Password, user.Password);
            Assert.IsFalse(model.Password.Equals(user.Password, StringComparison.InvariantCultureIgnoreCase));

        }


    }
}
