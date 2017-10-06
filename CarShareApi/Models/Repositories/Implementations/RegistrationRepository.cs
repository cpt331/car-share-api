using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private CarShareContext Context {get;set;}

        public RegistrationRepository(CarShareContext context)
        {
            Context = context;
        }

        public Registration Add(Registration item)
        {
            var registration = Context.Registrations.Add(item);
            Context.SaveChanges();
            return registration;
        }

        public Registration Find(int id)
        {
            var registration = Context.Registrations.FirstOrDefault(x => x.AccountID == id);
            return registration;
        }

        public List<Registration> FindAll()
        {
            return Context.Registrations.ToList();
        }

        public IQueryable<Registration> Query()
        {
            return Context.Registrations.AsQueryable();
        }

        public Registration Update(Registration item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var registration = Context.Registrations.FirstOrDefault(x => x.AccountID == id);
            if (registration != null)
            {
                Context.Entry(registration).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }
    }
}