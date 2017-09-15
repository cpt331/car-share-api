using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        //temporarily place users in memory until database is ready.
        public List<User> Users { get; set; }


        public UserRepository()
        {
            if (MemoryCache.Default.Contains("Users"))
            {
                Users = MemoryCache.Default["Users"] as List<User>;
            }

            if (Users == null)
            {
                Users = new List<User>
                    {
                        new User { Id = 1, Firstname = "Homer", Lastname = "Simpson", Email="user1@gmail.com", Password = Encryption.EncryptString("password1") },
                        new User { Id = 2, Firstname = "Frank", Lastname = "Grimes",  Email = "user2@gmail.com", Password = Encryption.EncryptString("password2") },
                        new User { Id = 3, Firstname = "Marge", Lastname = "Simpson", Email = "user3@gmail.com", Password = Encryption.EncryptString("password3") },
                        new User { Id = 4, Firstname = "Sideshow", Lastname = "Bob",  Email = "user4@gmail.com", Password = Encryption.EncryptString("password4") }
                    };
                MemoryCache.Default.Add("Users", Users, DateTime.Now.AddDays(1));
            }
        }
        public User Add(User item)
        {
            Users.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Users.RemoveAll(x => x.Id == id);
        }

        public User Find(int id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }

        public List<User> FindAll()
        {
            return Users;
        }

        public User FindByEmail(string email)
        {
            return Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User Update(User item)
        {
            Users.RemoveAll(x => x.Id == item.Id);
            Users.Add(item);
            return item;
        }
    }
}