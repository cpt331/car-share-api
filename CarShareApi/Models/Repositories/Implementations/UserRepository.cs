using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public List<User> Users { get; set; }

        public UserRepository()
        {
            Users = new List<User>
            {
                new User { Id = 1, Email = "user1@gmail.com", Password = "password1" },
                new User { Id = 2, Email = "user2@gmail.com", Password = "password2" },
                new User { Id = 3, Email = "user3@gmail.com", Password = "password3" },
                new User { Id = 4, Email = "user4@gmail.com", Password = "password4" }
            };
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