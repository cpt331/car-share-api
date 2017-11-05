using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        //This class inherits the IBookingRepository register
        public BookingRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public Booking Add(Booking item)
        {
            //Function to add a new booking item to the DB and save changes
            var booking = Context.Bookings.Add(item);
            Context.SaveChanges();
            return booking;
        }

        public Booking Find(int id)
        {
            //Finds a booking based on the booking ID and returns first input
            var booking = Context.Bookings.FirstOrDefault(x => 
            x.BookingID == id);
            return booking;
        }

        public List<Booking> FindAll()
        {
            //Finds all bookings and returns a list
            return Context.Bookings.ToList();
        }

        public IQueryable<Booking> Query()
        {
            return Context.Bookings.AsQueryable();
        }

        public Booking Update(Booking item)
        {
            //The booking state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a booking based on ID before saving
            var city = Context.Bookings.FirstOrDefault(x => 
            x.BookingID == id);
            if (city != null)
            {
                Context.Entry(city).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public List<Booking> FindByAccountId(int accountId)
        {
            //creates a list of bookings that belong to a user ID
            var bookings =
                Context.Bookings
                    .Include(x => x.Car)
                    .Where(x => x.AccountID == accountId).ToList();
            return bookings;
        }

        public List<Booking> FindByVehicleId(int vehicleId)
        {
            //creates a list of bookings that belong to a vehicle ID
            var bookings = Context.Bookings.Where(x => x.VehicleID == vehicleId).ToList();
            return bookings;
        }

        public List<Booking> FindByAccountIdAndVehicleId(int accountId, 
            int vehicleId)
        {
            //creates a list of bookings that belongs to a user and a vehicle
            var bookings = Context.Bookings.Where(x => x.VehicleID == vehicleId
            && x.AccountID == accountId).ToList();
            return bookings;
        }

        public void Dispose()
        {
            //Discards the context
            Context?.Dispose();
        }
    }
}