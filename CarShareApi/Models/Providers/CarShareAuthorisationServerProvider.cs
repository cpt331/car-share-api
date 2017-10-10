﻿using CarShareApi.Models.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CarShareApi.Models.Repositories.Data;
using Microsoft.Owin.Security;
using CarShareApi.Models.ViewModels;
using CarShareApi.ViewModels;
using Newtonsoft.Json;
using NLog;

namespace CarShareApi.Models.Providers
{
    //the following URL was used in assisting to create this provider
    //http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
    public class CarShareAuthorisationServerProvider : OAuthAuthorizationServerProvider
    {

        //class is constructed by passing in a user service
        private IUserService UserService { get; set; }
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public CarShareAuthorisationServerProvider(IUserService userService)
        {
            UserService = userService;
        }


        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //yes this application is authorised to make credential requests
            context.Validated();
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Logger.Debug("Logon Request Received: {0}", JsonConvert.SerializeObject(new LogonRequest
            {
                Email = context.UserName,
                Password = context.Password
            }, Formatting.Indented));

            //allow requests from all domains (unsecure)
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //confirm username and password is valid
            var response = UserService.Logon(new LogonRequest
            {
                Email = context.UserName,
                Password = context.Password
            });

            Logger.Debug("Sent Logon Response: {0}",
                JsonConvert.SerializeObject(response, Formatting.Indented));

            //return error if not successful
            if (!response.Success || !response.Id.HasValue)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");

                return;
            }

            //create a claims identity based on the user
            var user = UserService.FindUser(response.Id.Value);
            var identity = CreateIdentity(
                user, 
                context.Options.AuthenticationType);

            //add some additional fields to the ticket so the client application can consume
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "Name", $"{user.Firstname} {user.Lastname}"
                },
                {
                    "Email", user.Email
                },
                {
                    "Id", user.AccountId.ToString()
                },
                {
                    "HasOpenBooking", user.HasOpenBooking.ToString()
                }
            });

            if (user.OpenBookingId.HasValue)
            {
                props.Dictionary.Add("OpenBookingId", user.OpenBookingId.Value.ToString());
            }


            var ticket = new AuthenticationTicket(identity, props);

           

            //validate request and return token
            context.Validated(ticket);

           
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //add each property in the authentication properties to the output response
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                bool propertyBool;
                int propertyInt;
                if (bool.TryParse(property.Value, out propertyBool))
                {
                    context.AdditionalResponseParameters.Add(property.Key, propertyBool);
                }
                else if (int.TryParse(property.Value, out propertyInt))
                {
                    context.AdditionalResponseParameters.Add(property.Key, propertyInt);
                }
                else
                {
                    if (property.Value != null)
                    {
                        context.AdditionalResponseParameters.Add(property.Key, property.Value);
                    }
                    
                }
                
            }


            return Task.FromResult<object>(null);
        }

        private ClaimsIdentity CreateIdentity(UserViewModel user, string type)
        {
            //creates a claims identity based on the supplied user
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.PrimarySid, user.AccountId.ToString()),
                new Claim(ClaimTypes.Role, "User")
            }, type);
            return identity;
        }
    }
}