using CarShareApi.Models.Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareApi.Models.Repositories
{
    public interface ICityRepository : IRepository<City, string>
    {
    }
}
