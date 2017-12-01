//======================================
//
//Name: TransactionHistoryRepository.cs
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
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        //This class inherits the ITransactionHistoryRepository register
        public TransactionHistoryRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public TransactionHistory Add(TransactionHistory item)
        {
            //Function to add a new tsxhistory item to the DB and save changes
            var transaction = Context.TransactionHistories.Add(item);
            Context.SaveChanges();
            return transaction;
        }

        public TransactionHistory Find(int id)
        {
            //Finds a tsxhistory based on the tsx ID and returns first input
            var transaction = Context.TransactionHistories.FirstOrDefault(x => 
            x.TransactionID == id);
            return transaction;
        }

        public List<TransactionHistory> FindAll()
        {
            //Finds all tsxs and returns a list
            return Context.TransactionHistories.ToList();
        }

        public IQueryable<TransactionHistory> Query()
        {
            return Context.TransactionHistories.AsQueryable();
        }

        public TransactionHistory Update(TransactionHistory item)
        {
            //The tsxhistory state currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a tsxhistory based on ID before saving
            var transaction = Context.TransactionHistories.FirstOrDefault(x => 
            x.TransactionID == id);
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