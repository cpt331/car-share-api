using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        public List<User> Users { get; set; }


        public FakeUserRepository()
        {
            if (MemoryCache.Default.Contains("Users"))
            {
                Users = MemoryCache.Default["Users"] as List<User>;
            }

            if (Users == null)
            {
                Users = new List<User>
                {
                    new User { AccountID = 1, Status = "Active", FirstName = "Homer", LastName = "Simpson", Email= "user1@gmail.com", Password = Encryption.EncryptString("password1") },
                    new User { AccountID = 2, Status = "Active", FirstName = "Frank", LastName = "Grimes",  Email = "user2@gmail.com", Password = Encryption.EncryptString("password2") },
                    new User { AccountID = 3, Status = "Active", FirstName = "Marge", LastName = "Simpson", Email = "user3@gmail.com", Password = Encryption.EncryptString("password3") },
                    new User { AccountID = 4, Status = "Inactive", FirstName = "Sideshow", LastName = "Bob",  Email = "user4@gmail.com", Password = Encryption.EncryptString("password4") }
                };
                MemoryCache.Default.Add("Users", Users, DateTime.Now.AddDays(1));
            }
        }
        public User Add(User item)
        {
            item.AccountID = new Random().Next(int.MinValue, int.MaxValue);
            Users.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Users.RemoveAll(x => x.AccountID == id);
        }

        public User Find(int id)
        {
            return Users.FirstOrDefault(x => x.AccountID == id);
        }

        public List<User> FindAll()
        {
            return Users;
        }

        public IQueryable<User> Query()
        {
            return Users.AsQueryable();
        }

        public User FindByEmail(string email)
        {
            return Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User Update(User item)
        {
            Users.RemoveAll(x => x.AccountID == item.AccountID);
            Users.Add(item);
            return item;
        }


        public void Dispose()
        {
            //nah
        }
    }
}
