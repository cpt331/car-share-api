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

        public CitiesController(ICityService cityService)
        {
            CityService = cityService;
        }
        public List<CityViewModel> Get()
        {
            return CityService.FindAllCities();
        }
    }
}
