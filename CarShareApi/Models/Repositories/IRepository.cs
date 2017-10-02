using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareApi.Models.Repositories
{
    public interface IRepository<T, TK>
    {
        T Add(T item);
        T Find(TK id);
        List<T> FindAll();
        T Update(T item);
        void Delete(TK id);
    }
}
