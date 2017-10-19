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

        private List<PaymentMethod> PaymentMethods { get; set; }

        public FakePaymentMethodRepository(List<PaymentMethod> paymentMethods)
        {
            PaymentMethods = paymentMethods;
        }

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
