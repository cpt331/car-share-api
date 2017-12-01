//======================================
//
//Name: ICarService.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Cars;

namespace CarShareApi.Models.Services
{
    //This interface provides the overarching activities related to
    //The car service actions

    public interface ICarService : IDisposable
    {
        CarViewModel FindCar(int id);
        List<CarViewModel> FindAllCars();
        List<CarViewModel> FindCarsByLocation(double lat, double lng);
        List<CarViewModel> SearchCars(CarSearchCriteria criteria);
        List<CarCategoryViewModel> GetCarCategories();
        UpdateCarResponse UpdateCar(UpdateCarRequest request);
        List<string> GetCarStatuses();
        void DeleteCar(int id);
    }
}