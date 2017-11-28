//======================================
//
//Name: ITransactionHistoryRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ITransactionHistoryRepository : IRepository<TransactionHistory, int>, IDisposable
    {
    }
}