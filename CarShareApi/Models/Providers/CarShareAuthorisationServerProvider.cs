using CarShareApi.Models.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;

namespace CarShareApi.Models.Providers
{
    //http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
    public class CarShareAuthorisationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUserService UserService { get; set; }
        public CarShareAuthorisationServerProvider(IUserService userService)
        {
            UserService = userService;
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var response = UserService.Logon(new LogonRequest
            {
                Email = context.UserName,
                Password = context.Password
            });
            if (!response.Success || !response.Id.HasValue)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var user = UserService.FindUser(response.Id.Value);
            var identity = CreateIdentity(
                user, 
                context.Options.AuthenticationType);

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "Name", $"{user.Firstname} {user.Lastname}"
                },
                {
                    "Email", user.Email
                },
                {
                    "Id", user.Id.ToString()
                }
            });

            context.Validated(new AuthenticationTicket(identity, props));

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        private ClaimsIdentity CreateIdentity(User user, string type)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            }, type);
            return identity;
        }
    }
}