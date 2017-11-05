using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ICarCategoryRepository : IRepository<CarCategory, string>, IDisposable
    {
    }
}