//======================================
//
//Name: PaymentMethodRepository.cs
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
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        //This class inherits the IpaymentmethodRepository register
        public PaymentMethodRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }


        public PaymentMethod Add(PaymentMethod item)
        {
            //Function to add a new paymentmethod item to the DB and save
            var paymentMethod = Context.PaymentMethods.Add(item);
            Context.SaveChanges();
            return paymentMethod;
        }

        public PaymentMethod Find(int id)
        {
            //Finds a paymentmethod based on the paymentmethod ID and returns first inp
            var paymentMethod = Context.PaymentMethods.FirstOrDefault(x => 
            x.AccountID == id);
            return paymentMethod;
        }

        public List<PaymentMethod> FindAll()
        {
            //Finds all paymentmethods and returns a list
            return Context.PaymentMethods.ToList();
        }

        public IQueryable<PaymentMethod> Query()
        {
            return Context.PaymentMethods.AsQueryable();
        }

        public PaymentMethod Update(PaymentMethod item)
        {
            //The paymentmethod state that is currently modified is saved
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a paymentmethod based on ID then save
            var paymentMethod = Context.PaymentMethods.FirstOrDefault(x => 
            x.AccountID == id);
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