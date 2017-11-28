//======================================
//
//Name: FakeCityRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCityRepository : ICityRepository
    {
        //implemented city repository to allow for appropriate methods
        //to enable testing
        public FakeCityRepository(List<City> cities)
        {
            //implements the categories table data
            Cities = cities;
        }

        public List<City> Cities { get; set; }

        public City Add(City item)
        {
            //adds city to the city table
            Cities.Add(item);
            return item;
        }

        public City Find(string id)
        {
            //find a city by searching the city ID
            return Cities.FirstOrDefault(x =>
                x.CityName.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public List<City> FindAll()
        {
            //returns all the cities
            return Cities;
        }

        public IQueryable<City> Query()
        {
            //return querable city list
            return Cities.AsQueryable();
        }

        public City Update(City item)
        {
            //update the city with new items
            Cities.RemoveAll(x => x.CityName.Equals(item.CityName,
                StringComparison.InvariantCultureIgnoreCase));
            Cities.Add(item);
            return item;
        }

        public void Delete(string id)
        {
            //remove the city from the table
            Cities.RemoveAll(x =>
                x.CityName.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}