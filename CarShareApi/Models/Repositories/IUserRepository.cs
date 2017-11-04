using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IUserRepository : IRepository<User, int>, IDisposable
    {
        User FindByEmail(string email);
    }
}