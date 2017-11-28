//======================================
//
//Name: BookingService.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Device.Location;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;

namespace CarShareApi.Models.Services.Implementations
{
    //this booking module provides the methods to allow users to make a booking
    //and return a car. it has also been extended to handle the transaction 
    //processing against a users payment method
    public class BookingService : IBookingService
    {
        public BookingService(
            IBookingRepository bookingRepository,
            ICarRepository carRepository,
            IUserRepository userRepository,
            ICarCategoryRepository carCategoryRepository,
            ICityRepository cityRepository,
            ITransactionHistoryRepository transactionHistoryRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            BookingRepository = bookingRepository;
            CarRepository = carRepository;
            UserRepository = userRepository;
            CarCategoryRepository = carCategoryRepository;
            CityRepository = cityRepository;
            TransactionHistoryRepository = transactionHistoryRepository;
            PaymentMethodRepository = paymentMethodRepository;
        }

        private IBookingRepository BookingRepository { get; }
        private ICarRepository CarRepository { get; }
        private ICarCategoryRepository CarCategoryRepository { get; }
        private IUserRepository UserRepository { get; }
        private ICityRepository CityRepository { get; }

        private ITransactionHistoryRepository TransactionHistoryRepository
        {
            get;
        }

        private IPaymentMethodRepository PaymentMethodRepository { get; }


        public OpenBookingResponse OpenBooking(int vehicleId, int accountId)
        {
            //this method allows a car to be checked ou
            //check car exists and is correct status
            var car = CarRepository.Find(vehicleId);
            if (car == null)
                return new OpenBookingResponse
                {
                    Message = $"Vehicle {vehicleId} does not exist",
                    Success = false
                };
            if (car.Status != Constants.CarAvailableStatus)
                return new OpenBookingResponse
                {
                    Message =
                        $"{car.Make} {car.Model} is not available to " +
                        "be booked",
                    Success = false
                };

            //check user exists and correct status
            var user = UserRepository.Find(accountId);
            if (user == null)
                return new OpenBookingResponse
                {
                    Message = $"Account {accountId} does not exist",
                    Success = false
                };
            if (user.Status != Constants.UserActiveStatus)
                return new OpenBookingResponse
                {
                    Message = "Only activated users can book cars",
                    Success = false
                };

            //check is payment method exists
            var payment = PaymentMethodRepository.Find(accountId);
            if (payment == null)
                return new OpenBookingResponse
                {
                    Message = "You enter a payment method before booking",
                    Success = false
                };

            //sanity check to ensure the vehicle has no other bookings
            var hasOpenVehicleBookings = BookingRepository
                .FindByVehicleId(vehicleId)
                .Any(x => x.BookingStatus == Constants.BookingOpenStatus);
            if (hasOpenVehicleBookings)
            {
                //update the status of the car to be booked
                car.Status = Constants.CarBookedStatus;
                CarRepository.Update(car);

                return new OpenBookingResponse
                {
                    Message =
                        $"{car.Make} {car.Model} is not available " +
                        $"to be booked",
                    Success = false
                };
            }

            //sanity check to ensure the account has no other bookings
            var hasOpenAccountBookings = BookingRepository
                .FindByAccountId(accountId)
                .Any(x => x.BookingStatus == Constants.BookingOpenStatus);
            if (hasOpenAccountBookings)
                return new OpenBookingResponse
                {
                    Message = "User already has an open vehicle booking",
                    Success = false
                };

            var category = car.CarCategory1;
            if (category == null)
            {
                category = CarCategoryRepository.Find(car.CarCategory);

                if (category == null)
                    return new OpenBookingResponse
                    {
                        Message =
                            "Car has an invalid category and can not " +
                            "be checked out",
                        Success = false
                    };
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
                Message =
                    $"{car.Make} {car.Model} has been booked out " +
                    $"at ${category.BillingRate} per hour"
            };
        }


        public CloseBookingResponse CloseBooking(CloseBookingRequest request,
            int accountId)
        {
            //get booking by id and ensure it exists
            var openBooking = BookingRepository.Find(request.BookingId);
            var car = CarRepository.Find(openBooking.VehicleID);
            var user = UserRepository.Find(accountId);

            if (openBooking == null || openBooking.BookingStatus !=
                Constants.BookingOpenStatus || car == null || car.Status !=
                Constants.CarBookedStatus || user == null)
                return new CloseBookingResponse
                {
                    Message = ValidateClosedBooking(openBooking, car, user),
                    Success = false
                };

            //look through cities and ensure one is close enough for check in
            var selectedCity =
                ValidateCity(request.Latitude, request.Longitude);

            //if city doesn't exist throw error
            if (selectedCity == null)
                return new CloseBookingResponse
                {
                    Message =
                        "No cities are within a " +
                        $"{Constants.BookingMaxRangeFromCityCentre}m radius",
                    Success = false
                };

            //set parameters to return car and calculate billing amount
            var returnDate = DateTime.Now;
            var ts = returnDate - openBooking.CheckOut;
            var totalHours = (int) Math.Ceiling(ts.TotalHours);
            var totalAmount = totalHours * openBooking.BillingRate;


            //update cars status and its new location
            car.Status = Constants.CarAvailableStatus;
            car.Suburb = selectedCity.CityName;
            car.LatPos = request.Latitude;
            car.LongPos = request.Longitude;

            CarRepository.Update(car);

            //update booking record to show this booking is closed
            openBooking.CheckIn = returnDate;
            openBooking.BookingStatus = Constants.BookingClosedStatus;
            openBooking.TimeBilled = totalHours;
            openBooking.AmountBilled = totalAmount;
            openBooking.CityDropOff = selectedCity.CityName;

            BookingRepository.Update(openBooking);

            //return a successful message that booking was closed
            return new CloseBookingResponse
            {
                //return message to show booking and billing details
                City = selectedCity.CityName,
                HourlyRate = openBooking.BillingRate.ToString("C"),
                Message =
                    $"{car.Make} {car.Model} has been returned at " +
                    $"a cost of {totalAmount:C}",
                Success = true,
                TotalHours = totalHours.ToString(),
                TotalAmount = totalAmount.ToString("C")
            };
        }

        public CloseBookingCheckResponse CloseBookingCheck(
            CloseBookingCheckRequest request, int accountId)
        {
            //get booking by id and ensure it exists
            var openBooking = BookingRepository.Find(request.BookingId);
            var car = CarRepository.Find(openBooking.VehicleID);
            var user = UserRepository.Find(accountId);

            //cycle through all errors and return the appropriate message
            if (openBooking == null || openBooking.BookingStatus !=
                Constants.BookingOpenStatus || car == null || car.Status !=
                Constants.CarBookedStatus || user == null)
                return new CloseBookingCheckResponse
                {
                    Message = ValidateClosedBooking(openBooking, car, user),
                    Success = false
                };

            //look through cities and ensure one is close enough for check in
            var selectedCity = ValidateCity(request.Latitude,
                request.Longitude);
            if (selectedCity == null)
                return new CloseBookingCheckResponse
                {
                    Message =
                        "No cities are within a " +
                        $"{Constants.BookingMaxRangeFromCityCentre}m radius",
                    Success = false
                };

            //assign variables to show the billing details
            var ts = DateTime.Now - openBooking.CheckOut;
            var totalHours = (int) Math.Ceiling(ts.TotalHours);
            var totalAmount = totalHours * (double) openBooking.BillingRate;

            return new CloseBookingCheckResponse
            {
                //return values to show the current booking
                City = selectedCity.CityName,
                HourlyRate = openBooking.BillingRate.ToString("C"),
                Message =
                    $"{car.Make} {car.Model} is eligible " +
                    $"for return at a cost of {totalAmount:C}",
                Success = true,
                TotalHours = totalHours.ToString(),
                TotalAmount = totalAmount.ToString("C")
            };
        }

        public TransactionResponse RecordTransaction(int bookingId,
            int accountId)
        {
            //this method allows the closed booking to be recorded as a 
            //transaction using the users payment method

            //bring in the users account, payment method and booking
            var closedBooking = BookingRepository.Find(bookingId);
            var paymentMethod =
                PaymentMethodRepository.Find(closedBooking.AccountID);
            var userAccount = UserRepository.Find(closedBooking.AccountID);

            //error check to make sure a payment method has been supplied
            if (paymentMethod == null)
                return new TransactionResponse
                {
                    Message =
                        "Payment method for account " +
                        $"{closedBooking.AccountID} does not exist.",
                    Success = false
                };

            //error check to make sure user does exist
            if (userAccount == null)
                return new TransactionResponse
                {
                    Message =
                        $"The user with ID " +
                        $"{closedBooking.AccountID} does not exist.",
                    Success = false
                };

            //bundle a new transaction with neccessary values and add tsx
            var tsx = new TransactionHistory
            {
                BookingID = bookingId,
                TransactionDate = DateTime.Now,
                TransactionStatus = Constants.TransactionClearedStatus,
                PaymentMethod = paymentMethod.CardType,
                PaymentAmount = closedBooking.AmountBilled
            };
            TransactionHistoryRepository.Add(tsx);

            //return a successful transaction
            return new TransactionResponse
            {
                Success = true,
                Message =
                    "Transaction history has been " +
                    $"recorded for booking {bookingId}"
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

        public string ValidateClosedBooking(Booking booking, Car car,
            User user)
        {
            //if booking doesnt exist or booking is not the correct
            //status then return this error
            if (booking == null || booking.BookingStatus !=
                Constants.BookingOpenStatus)
                return "No open bookings were found for  " +
                       "this account and vehicle";
            //if the car doesn't exist within the booking throw an error
            if (car == null)
                return $"Vehicle {booking.VehicleID} does not exist";
            //if the car at question doesnt have a booked status throw error
            if (car.Status != Constants.CarBookedStatus)
                return $"{car.Make} {car.Model} is not booked and " +
                       "can not be returned";
            //sanity check to catch user account issues
            if (user == null)
                return "Account does not exist";
            return "";
        }

        public City ValidateCity(decimal lat, decimal longitude)
        {
            //look through cities and ensure one is close enough for check in
            var cities = CityRepository.FindAll();
            City selectedCity = null;
            foreach (var city in cities)
            {
                //use microsofts haversine formula (returns metres)
                //then assign current coordinates to work out the distance
                var cityCoordinate = new GeoCoordinate((double) city.LatPos,
                    (double) city.LongPos);
                var currentCoordinate = new GeoCoordinate(
                    (double) lat, (double) longitude);
                var distance = cityCoordinate.GetDistanceTo(currentCoordinate);
                if (distance < Constants.BookingMaxRangeFromCityCentre)
                {
                    //loop through all stored cities and if city is within
                    //the city limit assign the variable to return
                    selectedCity = city;
                    break;
                }
            }
            return selectedCity;
        }
    }
}