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
using Newtonsoft.Json;
using NLog;

namespace CarShareApi.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    /// 
   
    public class AccountController : ApiController
    {
        private IUserService UserService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        //public AccountController()
        //{
        //    UserService = new UserService(new UserRepository(new CarShareContext()));
        //}
        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Returns a list of users in the system (DEBUG ONLY)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/account/list")]
        public List<User> List()
        {

            //check application is in debug mode before returning this list
            CompilationSection compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");
            bool isDebugEnabled = compilationSection.Debug;
            if (!isDebugEnabled)
            {
                return null;
            }
            return UserService.FindUsers();
        }

        /// <summary>
        /// Register a new user into the system
        /// </summary>
        /// <param name="request">The view model from the client</param>
        /// <returns>A response indicating success and relevant error messages</returns>
        [HttpPost, Route("api/account/register")]
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

        #region Old Cookie Based Auth (NOT USED)
        [HttpPost]
        public LogonResponse Logon([FromBody]LogonRequest request)
        {
            var response = UserService.Logon(request);
            if (response.Success && response.Id.HasValue)
            {
                var user = UserService.FindUser(response.Id.Value);
                var identity = CreateIdentity(user);


                OwinLogon(identity);
            }
            return response;
        }

        [HttpGet]
        public UserPrincipal CurrentUser()
        {
            return new UserPrincipal(ClaimsPrincipal.Current);
        }

        private ClaimsIdentity CreateIdentity(User user)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.PrimarySid, user.AccountID.ToString()),
                new Claim(ClaimTypes.Role, "User")
            }, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }

        private void OwinLogon(ClaimsIdentity identity)
        {
            var owinContext = Request.GetOwinContext();

            owinContext.Authentication.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        private void OwinLogout()
        {
            var owinContext = Request.GetOwinContext();
            owinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        #endregion

    }
}
