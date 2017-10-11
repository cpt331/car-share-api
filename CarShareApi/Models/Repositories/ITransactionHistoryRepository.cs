using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ITransactionHistoryRepository : IRepository<TransactionHistory, int>
    {
    }
}