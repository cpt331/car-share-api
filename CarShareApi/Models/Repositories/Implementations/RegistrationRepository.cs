//======================================
//
//Name: RegistrationRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class RegistrationRepository : IRegistrationRepository
    {
        //This class inherits the IregistrationRepository register
        public RegistrationRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public Registration Add(Registration item)
        {
            //Function to add a new registration item to the DB and save
            var registration = Context.Registrations.Add(item);
            Context.SaveChanges();
            return registration;
        }

        public Registration Find(int id)
        {
            //Finds a registration based on the account ID and returns first
            var registration = Context.Registrations.FirstOrDefault(x => 
            x.AccountID == id);
            return registration;
        }

        public List<Registration> FindAll()
        {
            //Finds all registrations and returns a list
            return Context.Registrations.ToList();
        }

        public IQueryable<Registration> Query()
        {
            return Context.Registrations.AsQueryable();
        }

        public Registration Update(Registration item)
        {
            //The registration state that is currently modified is saved
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a registration based on ID
            var registration = Context.Registrations.FirstOrDefault(x => 
            x.AccountID == id);
            if (registration != null)
            {
                Context.Entry(registration).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}