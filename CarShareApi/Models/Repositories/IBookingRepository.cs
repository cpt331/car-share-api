//======================================
//
//Name: IBookingRepository.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IBookingRepository : IRepository<Booking, int>, IDisposable
    {
        //alows three implementations for booking searches -
        //find booking by user account id, find vehicle ID 
        //and find bookings with both user and and vehicle

        List<Booking> FindByAccountId(int accountId);
        List<Booking> FindByVehicleId(int vehicleId);
        List<Booking> FindByAccountIdAndVehicleId(int accountId, int vehicleId);
    }
}