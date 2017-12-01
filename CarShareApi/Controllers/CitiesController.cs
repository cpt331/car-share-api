//======================================
//
//Name: CitiesController.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Web.Http;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class CitiesController : ApiController
    {
        // inject service to make testing easier
        public CitiesController(ICityService cityService)
        {
            CityService = cityService;
        }

        private ICityService CityService { get; }

        /// <summary>
        ///     Produces a list of all cities available in the system
        /// </summary>
        /// <returns>A list of cities</returns>
        public List<CityViewModel> Get()
        {
            return CityService.FindAllCities();
        }
    }
}