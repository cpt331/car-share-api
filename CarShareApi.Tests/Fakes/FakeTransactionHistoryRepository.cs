using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeTransactionHistoryRepository : ITransactionHistoryRepository
    {
        private List<TransactionHistory> TransactionHistories { get; set; }

        public FakeTransactionHistoryRepository(List<TransactionHistory> transactionHistories)
        {
            TransactionHistories = transactionHistories;
        }

        public TransactionHistory Add(TransactionHistory item)
        {
            TransactionHistories.Add(item);
            return item;
        }

        public TransactionHistory Find(int id)
        {
            return TransactionHistories.FirstOrDefault(x => x.TransactionID == id);
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
            TransactionHistories.RemoveAll(x => x.TransactionID == item.TransactionID);
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
