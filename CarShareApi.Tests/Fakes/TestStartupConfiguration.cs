using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CarShareApi.Models;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Services;
using Microsoft.Practices.Unity;

namespace CarShareApi.Tests.Fakes
{
    public class TestStartupConfiguration : Startup
    {
        public static HttpConfiguration HttpConfiguration;
        public static IUserRepository UserRepository;
        public static ICarRepository CarRepository;
        public static IUserService UserService;
        public static ICarService CarService;

        public override HttpConfiguration GetInjectionConfiguration()
        {
            var container = new UnityContainer();
            if (UserRepository != null)
            {
                container.RegisterInstance(UserRepository);
            }
            if (CarRepository != null)
            {
                container.RegisterInstance(CarRepository);
            }
            if (UserService != null)
            {
                container.RegisterInstance(UserService);
            }
            if (CarService != null)
            {
                container.RegisterInstance(CarService);
            }
            
            HttpConfiguration.DependencyResolver = new UnityResolver(container);
            return HttpConfiguration;
        }
    }
}
