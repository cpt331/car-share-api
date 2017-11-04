using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ICityRepository : IRepository<City, string>, IDisposable
    {
    }
}