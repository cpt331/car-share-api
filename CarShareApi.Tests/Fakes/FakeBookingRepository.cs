//======================================
//
//Name: FakeBookingRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeBookingRepository : IBookingRepository
    {
        //implemented booking repository to allow for appropriate methods
        //to enable testing

        public FakeBookingRepository(List<Booking> bookings)
        {
            //evokes the bookings repo from the entity framework
            Bookings = bookings;
        }

        private List<Booking> Bookings { get; }

        public Booking Add(Booking item)
        {
            //creates the add method to write new values to the booking table
            item.BookingID = new Random().Next(int.MinValue, int.MaxValue);
            Bookings.Add(item);
            return item;
        }

        public Booking Find(int id)
        {
            //find booking based on the booking ID
            return Bookings.FirstOrDefault(x => x.BookingID == id);
        }

        public List<Booking> FindAll()
        {
            //returns all bookings in the table
            return Bookings;
        }

        public IQueryable<Booking> Query()
        {
            //query bookings and return
            return Bookings.AsQueryable();
        }

        public Booking Update(Booking item)
        {
            //replace booking information with new item
            Bookings.RemoveAll(x => x.BookingID == item.BookingID);
            Bookings.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            //option the delet the booking
            Bookings.RemoveAll(x => x.BookingID == id);
        }

        public List<Booking> FindByAccountId(int accountId)
        {
            //find bookings that share the same account ID
            return Bookings.Where(x => x.AccountID == accountId).ToList();
        }

        public List<Booking> FindByVehicleId(int vehicleId)
        {
            //return bookings that share a vehicle ID
            return Bookings.Where(x => x.VehicleID == vehicleId).ToList();
        }

        public List<Booking> FindByAccountIdAndVehicleId(int accountId,
            int vehicleId)
        {
            //return vehicles that shre an account and vehicle ID
            return Bookings.Where(x =>
                x.VehicleID == vehicleId && x.AccountID == accountId).ToList();
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}