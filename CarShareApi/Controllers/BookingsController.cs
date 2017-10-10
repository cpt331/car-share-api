using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using CarShareApi.Models;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels;
using NLog;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class BookingsController : ApiController
    {
        private IBookingService BookingService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        //inject service to make testing easier
        public BookingsController(IBookingService bookingService)
        {
            BookingService = bookingService;
        }

        [HttpGet, Route("api/bookings/open/{vehicleId}")]
        public OpenBookingResponse Open(int vehicleId)
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                return BookingService.OpenBooking(vehicleId, userPrincipal.Id.Value);
            }
            return new OpenBookingResponse
            {
                Success = false,
                Message = "No user logged on"
            };
        }
    }
}
