using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {

        private CarShareContext Context { get; set; }
        public PaymentMethodRepository(CarShareContext context)
        {
            Context = context;
        }


        public PaymentMethod Add(PaymentMethod item)
        {
            var paymentMethod = Context.PaymentMethods.Add(item);
            Context.SaveChanges();
            return paymentMethod;
        }

        public PaymentMethod Find(int id)
        {
            var paymentMethod = Context.PaymentMethods.FirstOrDefault(x => x.AccountID == id);
            return paymentMethod;
        }

        public List<PaymentMethod> FindAll()
        {
            return Context.PaymentMethods.ToList();
        }

        public IQueryable<PaymentMethod> Query()
        {
            return Context.PaymentMethods.AsQueryable();
        }

        public PaymentMethod Update(PaymentMethod item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var paymentMethod = Context.PaymentMethods.FirstOrDefault(x => x.AccountID == id);
            if (paymentMethod != null)
            {
                Context.Entry(paymentMethod).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}