using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakePaymentMethodRepository : IPaymentMethodRepository
    {
        public FakePaymentMethodRepository(List<PaymentMethod> paymentMethods)
        {
            PaymentMethods = paymentMethods;
        }

        private List<PaymentMethod> PaymentMethods { get; }

        public PaymentMethod Add(PaymentMethod item)
        {
            PaymentMethods.Add(item);
            return item;
        }

        public PaymentMethod Find(int id)
        {
            return PaymentMethods.FirstOrDefault(x => x.AccountID == id);
        }

        public List<PaymentMethod> FindAll()
        {
            return PaymentMethods.ToList();
        }

        public IQueryable<PaymentMethod> Query()
        {
            return PaymentMethods.AsQueryable();
        }

        public PaymentMethod Update(PaymentMethod item)
        {
            PaymentMethods.RemoveAll(x => x.AccountID == item.AccountID);
            PaymentMethods.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            PaymentMethods.RemoveAll(x => x.AccountID == id);
        }

        public void Dispose()
        {
        }
    }
}