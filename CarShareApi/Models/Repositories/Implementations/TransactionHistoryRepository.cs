using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private CarShareContext Context { get; set; }

        public TransactionHistoryRepository(CarShareContext context)
        {
            Context = context;
        }

        public TransactionHistory Add(TransactionHistory item)
        {
            var transaction = Context.TransactionHistories.Add(item);
            Context.SaveChanges();
            return transaction;
        }

        public TransactionHistory Find(int id)
        {
            var transaction = Context.TransactionHistories.FirstOrDefault(x => x.TransactionID == id);
            return transaction;
        }

        public List<TransactionHistory> FindAll()
        {
            return Context.TransactionHistories.ToList();
        }

        public IQueryable<TransactionHistory> Query()
        {
            return Context.TransactionHistories.AsQueryable();
        }

        public TransactionHistory Update(TransactionHistory item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var transaction = Context.TransactionHistories.FirstOrDefault(x => x.TransactionID == id);
            if (transaction != null)
            {
                Context.Entry(transaction).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}