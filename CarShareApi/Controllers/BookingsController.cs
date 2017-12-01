//======================================
//
//Name: BookingsController.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Security.Claims;
using System.Web.Http;
using CarShareApi.Models;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels;
//using NLog;

namespace CarShareApi.Controllers
{
    [Authorize]
    public class BookingsController : ApiController
    {
        private readonly IBookingService BookingService;

        //inject service to make testing easier
        public BookingsController(IBookingService bookingService)
        {
            BookingService = bookingService;
        }

        /// <summary>
        ///     Opens a booking for the vehicle secified and the logged on user
        /// </summary>
        /// <param name="vehicleId">The id of the vehicle to be booked</param>
        /// <returns>A response indicating sucess and the booking rate per hour</returns>
        [HttpGet]
        [Route("api/bookings/open/{vehicleId}")]
        public OpenBookingResponse Open(int vehicleId)
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
                return BookingService.OpenBooking(
                    vehicleId, userPrincipal.Id.Value);
            return new OpenBookingResponse
            {
                Success = false,
                Message = "No user logged on"
            };
        }

        /// <summary>
        ///     Verifies a booking can be closed and quotes the amount oweing
        /// </summary>
        /// <param name="request">The close booking request with booking id and coordinates</param>
        /// <returns>The response indicating success and calculated costs</returns>
        [HttpPost]
        [Route("api/bookings/check")]
        public CloseBookingCheckResponse CloseCheck(
            CloseBookingCheckRequest request)
        {
            var userPrincipal = new UserPrincipal(
                ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
                return BookingService.CloseBookingCheck(
                    request, userPrincipal.Id.Value);
            return new CloseBookingCheckResponse
            {
                Success = false,
                Message = "No user logged on"
            };
        }

        /// <summary>
        ///     Closes an open booking and finalises the amount oweing for the booking
        /// </summary>
        /// <param name="request">The close booking request with booking id and coordinates</param>
        /// <returns>The response indicating success and calculated costs</returns>
        [HttpPost]
        [Route("api/bookings/close")]
        public CloseBookingResponse Close(CloseBookingRequest request)
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
                return BookingService.CloseBooking(
                    request, userPrincipal.Id.Value);
            return new CloseBookingResponse
            {
                Success = false,
                Message = "No user logged on"
            };
        }
    }
}