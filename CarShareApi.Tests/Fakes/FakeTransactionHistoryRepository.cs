//======================================
//
//Name: FakeTransactionHistoryRepository.cs
//Version: 1.0
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
    //implemented TransactionHistory repository to allow 
    //for appropriate methods to enable testing. This records the transactions
    //for each booking within ewebah

    public class
        FakeTransactionHistoryRepository : ITransactionHistoryRepository
    {
        public FakeTransactionHistoryRepository(
            List<TransactionHistory> transactionHistories)
        {
            //implements the TransactionHistory table data
            TransactionHistories = transactionHistories;
        }

        private List<TransactionHistory> TransactionHistories { get; }

        public TransactionHistory Add(TransactionHistory item)
        {
            //add item to the TransactionHistory repo
            TransactionHistories.Add(item);
            return item;
        }

        public TransactionHistory Find(int id)
        {
            //find TransactionHistory based on ID
            return TransactionHistories.FirstOrDefault(x =>
                x.TransactionID == id);
        }

        public List<TransactionHistory> FindAll()
        {
            //return all TransactionHistory to list
            return TransactionHistories.ToList();
        }

        public IQueryable<TransactionHistory> Query()
        {
            //return querable TransactionHistory list
            return TransactionHistories.AsQueryable();
        }

        public TransactionHistory Update(TransactionHistory item)
        {
            //update the TransactionHistory with new entry
            TransactionHistories.RemoveAll(x =>
                x.TransactionID == item.TransactionID);
            TransactionHistories.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            //remove the TransactionHistory from the table based on the ID
            TransactionHistories.RemoveAll(x => x.TransactionID == id);
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}