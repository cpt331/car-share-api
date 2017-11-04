using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class
        FakeTransactionHistoryRepository : ITransactionHistoryRepository
    {
        public FakeTransactionHistoryRepository(
            List<TransactionHistory> transactionHistories)
        {
            TransactionHistories = transactionHistories;
        }

        private List<TransactionHistory> TransactionHistories { get; }

        public TransactionHistory Add(TransactionHistory item)
        {
            TransactionHistories.Add(item);
            return item;
        }

        public TransactionHistory Find(int id)
        {
            return TransactionHistories.FirstOrDefault(x =>
                x.TransactionID == id);
        }

        public List<TransactionHistory> FindAll()
        {
            return TransactionHistories.ToList();
        }

        public IQueryable<TransactionHistory> Query()
        {
            return TransactionHistories.AsQueryable();
        }

        public TransactionHistory Update(TransactionHistory item)
        {
            TransactionHistories.RemoveAll(x =>
                x.TransactionID == item.TransactionID);
            TransactionHistories.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            TransactionHistories.RemoveAll(x => x.TransactionID == id);
        }

        public void Dispose()
        {
        }
    }
}