using System;
using CarShareApi.Controllers;
using CarShareApi.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarShareApi.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void RegisterCheckEmailIsProvided()
        {
            AccountController controller = new AccountController();
            var response = controller.Register(new RegisterRequest
            {
                Email = "",
                FirstName = "Homer",
                LastName = "Simpson",
                Password = "123",
                ConfirmPassword = "123"
            });

            Assert.AreEqual("",response.Message);

        }
    }
}
