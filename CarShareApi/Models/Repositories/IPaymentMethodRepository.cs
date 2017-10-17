using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod, int>, IDisposable
    {
    }
}
