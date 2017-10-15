using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeBookingRepository : IBookingRepository
    {

        private List<Booking> Bookings { get; set; }

        public FakeBookingRepository(List<Booking> bookings)
        {
            Bookings = bookings;
        }

        public Booking Add(Booking item)
        {
            item.BookingID = new Random().Next(int.MinValue, int.MaxValue);
            Bookings.Add(item);
            return item;
        }

        public Booking Find(int id)
        {
            return Bookings.FirstOrDefault(x => x.BookingID == id);
        }

        public List<Booking> FindAll()
        {
            return Bookings;
        }

        public IQueryable<Booking> Query()
        {
            return Bookings.AsQueryable();
        }

        public Booking Update(Booking item)
        {
            Bookings.RemoveAll(x => x.BookingID == item.BookingID);
            Bookings.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Bookings.RemoveAll(x => x.BookingID == id);
        }

        public List<Booking> FindByAccountId(int accountId)
        {
            return Bookings.Where(x => x.AccountID == accountId).ToList();
        }

        public List<Booking> FindByVehicleId(int vehicleId)
        {
            return Bookings.Where(x => x.VehicleID == vehicleId).ToList();
        }

        public List<Booking> FindByAccountIdAndVehicleId(int accountId, int vehicleId)
        {
            return Bookings.Where(x => x.VehicleID == vehicleId && x.AccountID == accountId).ToList();
        }
    }
}
