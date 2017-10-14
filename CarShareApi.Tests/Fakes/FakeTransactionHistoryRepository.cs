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
        public TransactionHistory Add(TransactionHistory item)
        {
            throw new NotImplementedException();
        }

        public TransactionHistory Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<TransactionHistory> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TransactionHistory> Query()
        {
            throw new NotImplementedException();
        }

        public TransactionHistory Update(TransactionHistory item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
