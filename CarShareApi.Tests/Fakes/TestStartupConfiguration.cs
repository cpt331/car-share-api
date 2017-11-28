//======================================
//
//Name: TestStartupConfiguration.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Web.Http;
using CarShareApi.Models;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Services;
using Microsoft.Practices.Unity;

namespace CarShareApi.Tests.Fakes
{
    public class TestStartupConfiguration : Startup
    {
        //class assists in testing by 
        //1) creation of all repositories and then
        //2) testing all the tables being handled
        public static HttpConfiguration HttpConfiguration;

        public static IUserRepository UserRepository;
        public static ICarRepository CarRepository;
        public static IBookingRepository BookingRepository;
        public static IRegistrationRepository RegistrationRepository;
        public static ICarCategoryRepository CarCategoryRepository;
        public static ICityRepository CityRepository;

        public static ITransactionHistoryRepository TransactionHistoryRepository
            ;

        public static IUserService UserService;
        public static ICarService CarService;
        public static IBookingService BookingService;

        public override HttpConfiguration GetInjectionConfiguration()
        {
            
            var container = new UnityContainer();
            if (UserRepository != null)
                container.RegisterInstance(UserRepository);
            if (CarRepository != null)
                container.RegisterInstance(CarRepository);
            if (CarCategoryRepository != null)
                container.RegisterInstance(CarCategoryRepository);
            if (BookingRepository != null)
                container.RegisterInstance(BookingRepository);
            if (RegistrationRepository != null)
                container.RegisterInstance(RegistrationRepository);
            if (TransactionHistoryRepository != null)
                container.RegisterInstance(TransactionHistoryRepository);
            if (CityRepository != null)
                container.RegisterInstance(CityRepository);

            if (UserService != null)
                container.RegisterInstance(UserService);
            if (CarService != null)
                container.RegisterInstance(CarService);
            if (BookingService != null)
                container.RegisterInstance(BookingService);

            HttpConfiguration.DependencyResolver = new UnityResolver(container);
            return HttpConfiguration;
        }
    }
}