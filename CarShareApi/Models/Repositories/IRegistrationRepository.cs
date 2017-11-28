﻿//======================================
//
//Name: IRegistrationRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: 
//
//======================================

using System;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface IRegistrationRepository : IRepository<Registration, int>, IDisposable
    {
    }
}