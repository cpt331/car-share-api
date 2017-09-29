using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private CarShareContext Context { get; set; }



        public UserRepository(CarShareContext context)
        {
            Context = context;
        }
        public User Add(User item)
        {
            var user = Context.Users.Add(item);
            Context.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            var user = Context.Users.FirstOrDefault(x => x.AccountID == id);
            if (user != null)
            {
                Context.Entry(user).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public User Find(int id)
        {
            var user = Context.Users
                .Include(x=>x.Registration)
                .FirstOrDefault(x => x.AccountID == id);
            return user;
        }

        public List<User> FindAll()
        {
            return Context.Users.ToList();
        }

        public User FindByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User Update(User item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }
    }
}