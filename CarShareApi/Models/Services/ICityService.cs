//======================================
//
//Name: ICityService.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using CarShareApi.ViewModels;

namespace CarShareApi.Models.Services
{
    //This interface provides the overarching activities related to
    //The city service actions

    public interface ICityService : IDisposable
    {
        List<CityViewModel> FindAllCities();
        CityViewModel FindCity(string cityName);
    }
}