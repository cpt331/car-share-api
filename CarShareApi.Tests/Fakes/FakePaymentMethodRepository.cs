//======================================
//
//Name: FakePaymentMethodRepository.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakePaymentMethodRepository : IPaymentMethodRepository
    {
        //implemented payment repository to allow for appropriate methods
        //to enable testing
        public FakePaymentMethodRepository(List<PaymentMethod> paymentMethods)
        {
            //implements the PaymentMethods table data
            PaymentMethods = paymentMethods;
        }

        private List<PaymentMethod> PaymentMethods { get; }

        public PaymentMethod Add(PaymentMethod item)
        {
            //add item to the PaymentMethods repo
            PaymentMethods.Add(item);
            return item;
        }

        public PaymentMethod Find(int id)
        {
            //find PaymentMethod based on ID
            return PaymentMethods.FirstOrDefault(x => x.AccountID == id);
        }

        public List<PaymentMethod> FindAll()
        {
            //return all PaymentMethod to list
            return PaymentMethods.ToList();
        }

        public IQueryable<PaymentMethod> Query()
        {
            //return querable category list
            return PaymentMethods.AsQueryable();
        }

        public PaymentMethod Update(PaymentMethod item)
        {
            //update the PaymentMethod with new items
            PaymentMethods.RemoveAll(x => x.AccountID == item.AccountID);
            PaymentMethods.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            //remove the Payment Method from the table
            PaymentMethods.RemoveAll(x => x.AccountID == id);
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}