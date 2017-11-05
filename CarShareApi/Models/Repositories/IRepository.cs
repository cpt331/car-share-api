﻿using System.Collections.Generic;
using System.Linq;

namespace CarShareApi.Models.Repositories
{
    public interface IRepository<T, TK>
    {
        //defines the base interface for other interfaces allowing a minimum of
        //add a new item, find an existing item, return all items, query items,
        //update existing item and delete an item

        T Add(T item);
        T Find(TK id);
        List<T> FindAll();
        IQueryable<T> Query();
        T Update(T item);
        void Delete(TK id);
    }
}