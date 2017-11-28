//======================================
//
//Name: UserRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        //This class inherits the IuserRepository register
        public UserRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public User Add(User item)
        {
            //Function to add a new user item to the DB and save changes
            var user = Context.Users.Add(item);
            Context.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a user based on ID before saving
            var user = Context.Users.FirstOrDefault(x => x.AccountID == id);
            if (user != null)
            {
                Context.Entry(user).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public User Find(int id)
        {
            //Finds a user based on the user ID and returns first input
            var user = Context.Users
                .Include(x => x.Registration)
                .FirstOrDefault(x => x.AccountID == id);
            return user;
        }

        public List<User> FindAll()
        {
            //Finds all users and returns a list
            return Context.Users.ToList();
        }

        public IQueryable<User> Query()
        {
            return Context.Users.AsQueryable();
        }

        public User FindByEmail(string email)
        {
            //Finds user by parsing an email and a user object first found
            return Context.Users.FirstOrDefault(x =>
                x.Email.Equals(email, 
                StringComparison.InvariantCultureIgnoreCase));
        }

        public User Update(User item)
        {
            //The user state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}