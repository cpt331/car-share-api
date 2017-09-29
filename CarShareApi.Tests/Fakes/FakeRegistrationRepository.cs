using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeRegistrationRepository : IRegistrationRepository
    {
        public Registration Add(Registration item)
        {
            throw new NotImplementedException();
        }

        public Registration Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Registration> FindAll()
        {
            throw new NotImplementedException();
        }

        public Registration Update(Registration item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
