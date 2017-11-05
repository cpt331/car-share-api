using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod, int>, IDisposable
    {
    }
}