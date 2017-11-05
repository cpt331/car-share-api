using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IUserRepository : IRepository<User, int>, IDisposable
    {
        //defines the finduser by using an email
        User FindByEmail(string email);
    }
}