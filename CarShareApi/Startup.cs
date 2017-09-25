﻿using CarShareApi.Models.Providers;
using CarShareApi.Models.Repositories.Implementations;
using CarShareApi.Models.Services.Implementations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CarShareApi.Models.Repositories.Data;
using System.Web.Http.Cors;
using CarShareApi.Models;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Services;
using Microsoft.Practices.Unity;

[assembly: OwinStartup(typeof(CarShareApi.Startup))]
namespace CarShareApi
{
    public class Startup
    {
        public virtual HttpConfiguration GetInjectionConfiguration()
        {
            var configuration = new HttpConfiguration();
            var container = new UnityContainer();
            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<ICarRepository, CarRepository>(new TransientLifetimeManager());
            container.RegisterType<IUserService, UserService>(new TransientLifetimeManager());
            container.RegisterType<ICarService, CarService>(new TransientLifetimeManager());

            configuration.DependencyResolver = new UnityResolver(container);

            return configuration;
        }

        public void Configuration(IAppBuilder app)
        {
            //use dependency injection for services
            var config = GetInjectionConfiguration();

            var openAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CarShareAuthorisationServerProvider((IUserService)config.DependencyResolver.GetService(typeof(IUserService)))
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(openAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            EnableCrossSiteRequests(config);
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            
        }
        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*");
            config.EnableCors(cors);
        }
    }
}
