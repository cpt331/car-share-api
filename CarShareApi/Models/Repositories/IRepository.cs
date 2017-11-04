using System.Collections.Generic;
using System.Linq;

namespace CarShareApi.Models.Repositories
{
    public interface IRepository<T, TK>
    {
        T Add(T item);
        T Find(TK id);
        List<T> FindAll();
        IQueryable<T> Query();
        T Update(T item);
        void Delete(TK id);
    }
}