//======================================
//
//Name: CityRepository.cs
//Version: 1.0
//Date: 03/12/2017
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
    public class CityRepository : ICityRepository
    {
        //This class inherits the IcityRepository register
        public CityRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public City Add(City item)
        {
            //Function to add a new city item to the DB and save changes
            var city = Context.Cities.Add(item);
            Context.SaveChanges();
            return city;
        }

        public City Find(string id)
        {
            //Finds a city based on the city ID and returns first input
            var city = Context.Cities.FirstOrDefault(x => 
            x.CityName.Equals(id));
            return city;
        }

        public List<City> FindAll()
        {
            //Finds all cities and returns a list
            return Context.Cities.ToList();
        }

        public IQueryable<City> Query()
        {
            return Context.Cities.AsQueryable();
        }

        public City Update(City item)
        {
            //The city state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(string id)
        {
            //Allows the user to delete a city based on ID before saving
            var city = Context.Cities.FirstOrDefault(x => 
            x.CityName.Equals(id));
            if (city != null)
            {
                Context.Entry(city).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}