using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class CitiesController : ApiController
    {
        private ICityService CityService { get; set; }

        // inject service to make testing easier
        public CitiesController(ICityService cityService)
        {
            CityService = cityService;
        }

        /// <summary>
        /// Produces a list of all cities available in the system
        /// </summary>
        /// <returns>A list of cities</returns>
        public List<CityViewModel> Get()
        {
            return CityService.FindAllCities();
        }
    }
}
