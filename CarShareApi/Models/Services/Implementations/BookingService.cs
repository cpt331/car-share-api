using System;
using System.Collections.Generic;
using System.Device.Location;
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
        private ICarCategoryRepository CarCategoryRepository { get; set; }
        private IUserRepository UserRepository { get; set; }
        private ICityRepository CityRepository { get; set; }
        private ITransactionHistoryRepository TransactionHistoryRepository { get; set; }

        public BookingService(
            IBookingRepository bookingRepository,
            ICarRepository carRepository, 
            IUserRepository userRepository, 
            ICarCategoryRepository carCategoryRepository, 
            ICityRepository cityRepository,
            ITransactionHistoryRepository transactionHistoryRepository)
        {
            BookingRepository = bookingRepository;
            CarRepository = carRepository;
            UserRepository = userRepository;
            CarCategoryRepository = carCategoryRepository;
            CityRepository = cityRepository;
            TransactionHistoryRepository = transactionHistoryRepository;
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

            CarCategory category = car.CarCategory1;
            if (category == null)
            {
                category = CarCategoryRepository.Find(car.CarCategory);

                if (category == null)
                {
                    return new OpenBookingResponse
                    {
                        Message = $"Car has an invalid category and can not be checked out",
                        Success = false
                    };
                }
            }

            //create the booking and save
            var booking = new Booking
            {
                VehicleID = vehicleId,
                AccountID = accountId,
                BookingStatus = Constants.BookingOpenStatus,
                BillingRate = category.BillingRate,
                CheckOut = DateTime.Now,
                CityPickUp = car.Suburb
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
                Message = $"{car.Make} {car.Model} has been booked out at ${category.BillingRate} per hour"
            };
        }

        public CloseBookingResponse CloseBooking(CloseBookingRequest request, int accountId)
        {
            //get booking by id and ensure it exists
            var openBooking = BookingRepository.Find(request.BookingId);
            if (openBooking == null || openBooking.BookingStatus != Constants.BookingOpenStatus)
            {
                return new CloseBookingResponse
                {
                    Message = $"No open bookings were found for this account and vehicle",
                    Success = false
                };
            }

            //check car exists and is correct status
            var car = CarRepository.Find(openBooking.VehicleID);
            if (car == null)
            {
                return new CloseBookingResponse
                {
                    Message = $"Vehicle {openBooking.VehicleID} does not exist",
                    Success = false
                };
            }
            if (car.Status != Constants.CarBookedStatus)
            {
                return new CloseBookingResponse
                {
                    Message = $"{car.Make} {car.Model} is not booked and can not be returned",
                    Success = false
                };
            }


            //check user exists
            var user = UserRepository.Find(accountId);
            if (user == null)
            {
                return new CloseBookingResponse
                {
                    Message = $"Account {accountId} does not exist",
                    Success = false
                };
            }

            //look through cities and ensure one is close enough for check in
            var cities = CityRepository.FindAll();
            City selectedCity = null;
            foreach (var city in cities)
            {
                //use microsofts haversine formula (returns metres)
                var cityCoordinate = new GeoCoordinate((double)city.LatPos, (double)city.LongPos);
                var currentCoordinate = new GeoCoordinate((double)request.Latitude, (double)request.Longitude);
                var distance = cityCoordinate.GetDistanceTo(currentCoordinate);
                if (distance < Constants.BookingMaxRangeFromCityCentre)
                {
                    selectedCity = city;
                    break;
                }
            }

            if (selectedCity == null)
            {
                return new CloseBookingResponse
                {
                    Message = $"No cities are within a {Constants.BookingMaxRangeFromCityCentre}m radius",
                    Success = false
                };
            }

            var returnDate = DateTime.Now;
            var ts = returnDate - openBooking.CheckOut;
            var totalHours = (int)Math.Ceiling(ts.TotalHours);
            var totalAmount = totalHours * openBooking.BillingRate;


            //update cars status and its new location
            car.Status = Constants.CarAvailableStatus;
            car.Suburb = selectedCity.CityName;
            car.LatPos = (decimal)request.Latitude;
            car.LongPos = (decimal)request.Longitude;

            CarRepository.Update(car);

            //update booking record to show this booking is closed
            openBooking.CheckIn = returnDate;
            openBooking.BookingStatus = Constants.BookingClosedStatus;
            openBooking.TimeBilled = totalHours;
            openBooking.AmountBilled = totalAmount;
            openBooking.CityDropOff = selectedCity.CityName;

            BookingRepository.Update(openBooking);


            //TODO: record transaction or debt somewhere

            return new CloseBookingResponse
            {
                City = selectedCity.CityName,
                HourlyRate = openBooking.BillingRate.ToString("C"),
                Message = $"{car.Make} {car.Model} has been returned at a cost of {totalAmount:C}",
                Success = true,
                TotalHours = totalHours.ToString(),
                TotalAmount = totalAmount.ToString("C")
            };
        }

        public CloseBookingCheckResponse CloseBookingCheck(CloseBookingCheckRequest request, int accountId)
        {
            //get booking by id and ensure it exists
            var openBooking = BookingRepository.Find(request.BookingId);
            if (openBooking == null || openBooking.BookingStatus != Constants.BookingOpenStatus)
            {
                return new CloseBookingCheckResponse
                {
                    Message = $"No open bookings were found for this account and vehicle",
                    Success = false
                };
            }

            //check car exists and is correct status
            var car = CarRepository.Find(openBooking.VehicleID);
            if (car == null)
            {
                return new CloseBookingCheckResponse
                {
                    Message = $"Vehicle {openBooking.VehicleID} does not exist",
                    Success = false
                };
            }
            if (car.Status != Constants.CarBookedStatus)
            {
                return new CloseBookingCheckResponse
                {
                    Message = $"{car.Make} {car.Model} is not booked and can not be returned",
                    Success = false
                };
            }

            //check user exists
            var user = UserRepository.Find(accountId);
            if (user == null)
            {
                return new CloseBookingCheckResponse
                {
                    Message = $"Account {accountId} does not exist",
                    Success = false
                };
            }

            //look through cities and ensure one is close enough for check in
            var cities = CityRepository.FindAll();
            City selectedCity = null;
            foreach (var city in cities)
            {
                //use microsofts haversine formula (returns metres)
                var cityCoordinate = new GeoCoordinate((double)city.LatPos, (double)city.LongPos);
                var currentCoordinate = new GeoCoordinate((double)request.Latitude, (double)request.Longitude);
                var distance = cityCoordinate.GetDistanceTo(currentCoordinate);
                if (distance < Constants.BookingMaxRangeFromCityCentre)
                {
                    selectedCity = city;
                    break;
                }
            }

            if (selectedCity == null)
            {
                return new CloseBookingCheckResponse
                {
                    Message = $"No cities are within a {Constants.BookingMaxRangeFromCityCentre}m radius",
                    Success = false
                };
            }

            var ts = DateTime.Now - openBooking.CheckOut;
            var totalHours = (int)Math.Ceiling(ts.TotalHours);
            var totalAmount = totalHours * (double)openBooking.BillingRate;

            return new CloseBookingCheckResponse
            {
                City = selectedCity.CityName,
                HourlyRate = openBooking.BillingRate.ToString("C"),
                Message = $"{car.Make} {car.Model} is eligible for return at a cost of {totalAmount:C}",
                Success = true,
                TotalHours = totalHours.ToString(),
                TotalAmount = totalAmount.ToString("C")
            };

        }

        public void Dispose()
        {
            BookingRepository?.Dispose();
            CarRepository?.Dispose();
            CarCategoryRepository?.Dispose();
            UserRepository?.Dispose();
            CityRepository?.Dispose();
            TransactionHistoryRepository?.Dispose();
        }
    }
}