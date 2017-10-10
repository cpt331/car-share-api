using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {

        private CarShareContext Context { get; set; }

        public BookingRepository(CarShareContext context)
        {
            Context = context;
        }

        public Booking Add(Booking item)
        {
            var booking = Context.Bookings.Add(item);
            Context.SaveChanges();
            return booking;
        }

        public Booking Find(int id)
        {
            var booking = Context.Bookings.FirstOrDefault(x => x.BookingID == id);
            return booking;
        }

        public List<Booking> FindAll()
        {
            return Context.Bookings.ToList();
        }

        public IQueryable<Booking> Query()
        {
            return Context.Bookings.AsQueryable();
        }

        public Booking Update(Booking item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var city = Context.Bookings.FirstOrDefault(x => x.BookingID == id);
            if (city != null)
            {
                Context.Entry(city).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public List<Booking> FindByAccountId(int accountId)
        {
            var bookings = Context.Bookings.Where(x => x.AccountID == accountId).ToList();
            return bookings;
        }

        public List<Booking> FindByVehicleId(int vehicleId)
        {
            var bookings = Context.Bookings.Where(x => x.VehicleID == vehicleId).ToList();
            return bookings;
        }
    }
}