using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ITransactionHistoryRepository : IRepository<TransactionHistory, int>, IDisposable
    {
    }
}