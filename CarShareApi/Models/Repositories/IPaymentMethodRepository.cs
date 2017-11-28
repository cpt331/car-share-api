﻿//======================================
//
//Name: IPaymentMethodRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: 
//
//======================================

using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod, int>, IDisposable
    {
    }
}