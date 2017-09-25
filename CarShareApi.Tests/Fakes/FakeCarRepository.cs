using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCarRepository : ICarRepository
    {
        public Car Add(Car item)
        {
            throw new NotImplementedException();
        }

        public Car Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Car> FindAll()
        {
            throw new NotImplementedException();
        }

        public Car Update(Car item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
