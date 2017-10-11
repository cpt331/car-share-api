using CarShareApi.Models.Providers;
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

            //specify concrete instances of classes that should be injected when interface is marked
            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<IBookingRepository, BookingRepository>(new TransientLifetimeManager());
            container.RegisterType<IRegistrationRepository, RegistrationRepository>(new TransientLifetimeManager());
            container.RegisterType<ICarRepository, CarRepository>(new TransientLifetimeManager());
            container.RegisterType<ICarCategoryRepository, CarCategoryRepository>(new TransientLifetimeManager());
            container.RegisterType<ICityRepository, CityRepository>(new TransientLifetimeManager());
            container.RegisterType<ITransactionHistoryRepository, TransactionHistoryRepository>(new TransientLifetimeManager());

            container.RegisterType<IUserService, UserService>(new TransientLifetimeManager());
            container.RegisterType<ICarService, CarService>(new TransientLifetimeManager());
            container.RegisterType<IBookingService, BookingService>(new TransientLifetimeManager());

            //set this container as the http configurations dependency injection provider
            configuration.DependencyResolver = new UnityResolver(container);

            return configuration;
        }

        public void Configuration(IAppBuilder app)
        {
            //use dependency injection for services
            var config = GetInjectionConfiguration();

            //open authorisation configuration options
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

            //enable other domain applications to request resources
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
