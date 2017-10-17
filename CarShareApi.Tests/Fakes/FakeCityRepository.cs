using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCityRepository : ICityRepository
    {

        public List<City> Cities { get; set; }

        public FakeCityRepository(List<City> cities)
        {
            Cities = cities;
        }

        public City Add(City item)
        {
            Cities.Add(item);
            return item;
        }

        public City Find(string id)
        {
            return Cities.FirstOrDefault(x => x.CityName.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        public List<City> FindAll()
        {
            return Cities;
        }

        public IQueryable<City> Query()
        {
            return Cities.AsQueryable();
        }

        public City Update(City item)
        {
            Cities.RemoveAll(x => x.CityName.Equals(item.CityName, StringComparison.InvariantCultureIgnoreCase));
            Cities.Add(item);
            return item;
        }

        public void Delete(string id)
        {
            Cities.RemoveAll(x => x.CityName.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            //nah
        }
    }
}
