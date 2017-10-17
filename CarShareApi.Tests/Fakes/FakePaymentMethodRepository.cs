using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakePaymentMethodRepository : IPaymentMethodRepository
    {
        public PaymentMethod Add(PaymentMethod item)
        {
            throw new NotImplementedException();
        }

        public PaymentMethod Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<PaymentMethod> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<PaymentMethod> Query()
        {
            throw new NotImplementedException();
        }

        public PaymentMethod Update(PaymentMethod item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
