using CarShareApi.Models;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services;
using CarShareApi.Models.Services.Implementations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace CarShareApi.Controllers
{
    public class AccountController : ApiController
    {
        private IUserService UserService;
        public AccountController()
        {
            UserService = new UserService(new UserRepository());
        }
        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost]
        public LogonResponse Logon([FromBody]LogonRequest request)
        {
            var response = UserService.Logon(request);
            if (response.Success && response.Id.HasValue)
            {
                var identity = CreateIdentity(UserService.FindUser(response.Id.Value));
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
                new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
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
    }
}
