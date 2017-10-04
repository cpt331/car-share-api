using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class CityRepository : ICityRepository
    {
        private CarShareContext Context { get; set; }

        public CityRepository(CarShareContext context)
        {
            Context = context;
        }

        public City Add(City item)
        {
            var city = Context.Cities.Add(item);
            Context.SaveChanges();
            return city;
        }

        public City Find(string id)
        {
            var city = Context.Cities.FirstOrDefault(x => x.CityName.Equals(id));
            return city;
        }

        public List<City> FindAll()
        {
            return Context.Cities.ToList();
        }

        public IQueryable<City> Query()
        {
            return Context.Cities.AsQueryable();
        }

        public City Update(City item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(string id)
        {
            var city = Context.Cities.FirstOrDefault(x => x.CityName.Equals(id));
            if (city != null)
            {
                Context.Entry(city).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }
    }
}