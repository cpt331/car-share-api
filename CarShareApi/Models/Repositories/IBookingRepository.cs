using System;
using System.Collections.Generic;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IBookingRepository : IRepository<Booking, int>, IDisposable
    {
        List<Booking> FindByAccountId(int accountId);
        List<Booking> FindByVehicleId(int vehicleId);
        List<Booking> FindByAccountIdAndVehicleId(int accountId, int vehicleId);
    }
}