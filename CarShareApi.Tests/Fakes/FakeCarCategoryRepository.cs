using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCarCategoryRepository : ICarCategoryRepository
    {
        public CarCategory Add(CarCategory item)
        {
            throw new NotImplementedException();
        }

        public CarCategory Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<CarCategory> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<CarCategory> Query()
        {
            throw new NotImplementedException();
        }

        public CarCategory Update(CarCategory item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
