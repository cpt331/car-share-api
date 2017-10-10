using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services.Implementations
{
    public class BookingService : IBookingService
    {

        private IBookingRepository BookingRepository { get; set; }
        private ICarRepository CarRepository { get; set; }
        private IUserRepository UserRepository { get; set; }


        public BookingService(IBookingRepository bookingRepository, ICarRepository carRepository, IUserRepository userRepository)
        {
            BookingRepository = bookingRepository;
            CarRepository = carRepository;
            UserRepository = userRepository;
        }
        

        public OpenBookingResponse OpenBooking(int vehicleId, int accountId)
        {
            //check car exists and is correct status
            var car = CarRepository.Find(vehicleId);
            if (car == null)
            {
                return new OpenBookingResponse
                {
                    Message = $"Vehicle {vehicleId} does not exist",
                    Success = false
                };
            }
            if (car.Status != Constants.CarAvailableStatus)
            {
                return new OpenBookingResponse
                {
                    Message = $"{car.Make} {car.Model} is not available to be booked",
                    Success = false
                };
            }

            //check user exists and correct status
            var user = UserRepository.Find(accountId);
            if (user == null)
            {
                return new OpenBookingResponse
                {
                    Message = $"Account {accountId} does not exist",
                    Success = false
                };
            }
            if (user.Status != Constants.UserActiveStatus)
            {
                return new OpenBookingResponse
                {
                    Message = $"Only activated users can book cars",
                    Success = false
                };
            }

            //sanity check to ensure the vehicle has no other bookings
            var hasOpenVehicleBookings = BookingRepository
                                            .FindByVehicleId(vehicleId)
                                            .Any(x=>x.BookingStatus == Constants.BookingOpenStatus);
            if (hasOpenVehicleBookings)
            {
                //update the status of the car to be booked
                car.Status = Constants.CarBookedStatus;
                CarRepository.Update(car);

                return new OpenBookingResponse
                {
                    Message = $"{car.Make} {car.Model} is not available to be booked",
                    Success = false
                };
            }

            //sanity check to ensure the account has no other bookings
            var hasOpenAccountBookings = BookingRepository
                .FindByAccountId(accountId)
                .Any(x => x.BookingStatus == Constants.BookingOpenStatus);
            if (hasOpenAccountBookings)
            {
                return new OpenBookingResponse
                {
                    Message = $"User already has an open vehicle booking",
                    Success = false
                };
            }

            //create the booking and save
            var booking = new Booking
            {
                VehicleID = vehicleId,
                AccountID = accountId,
                BookingStatus = Constants.BookingOpenStatus,
                CheckOut = DateTime.Now
            };
            BookingRepository.Add(booking);
        
            //update the status of the car to be booked
            car.Status = Constants.CarBookedStatus;
            CarRepository.Update(car);

            return new OpenBookingResponse
            {
                BookingId = booking.BookingID,
                CheckOutTime = booking.CheckOut.ToString(),
                Success = true,
                Message = $"{car.Make} {car.Model} has been booked out"
            };
        }

        public CloseBookingResponse CloseBooking(CloseBookingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}