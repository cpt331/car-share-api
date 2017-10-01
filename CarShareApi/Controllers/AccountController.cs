using CarShareApi.Models;
using CarShareApi.Models.Providers;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using CarShareApi.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Configuration;
using System.Web.Http;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels;
using Newtonsoft.Json;
using NLog;

namespace CarShareApi.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    /// 
   
    [Authorize]
    public class AccountController : ApiController
    {
        private IUserService UserService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        //inject service to make testing easier
        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpGet, Route("api/account/current")]
        public UserViewModel Current()
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                var user = UserService.FindUser(userPrincipal.Id.Value);
                if (user != null)
                {
                    return new UserViewModel(user);
                }
            }
            return null;
        }

        /// <summary>
        /// Register a new user into the system
        /// </summary>
        /// <param name="request">The view model from the client</param>
        /// <returns>A response indicating success and relevant error messages</returns>
        [HttpPost, Route("api/account/register"), AllowAnonymous]
        public RegisterResponse Register(RegisterRequest request)
        {
            Logger.Debug("Register Request Received: {0}", JsonConvert.SerializeObject(request, Formatting.Indented));

            RegisterResponse response;
            //use in built data annotations to ensure model has binded correctly
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new RegisterResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
                
            }
            else
            {
                //send request to the user service and return the response (success or fail)
                response = UserService.Register(request);
                
            }
            Logger.Debug("Sent Register Response: {0}",
                JsonConvert.SerializeObject(response, Formatting.Indented));
            return response;
        }

      

    }
}
