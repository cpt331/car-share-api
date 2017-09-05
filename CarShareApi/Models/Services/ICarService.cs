﻿using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareApi.Models.Services
{
    public interface ICarService
    {
        CarViewModel FindCar(int id);
        List<CarViewModel> FindAllCars();
        List<CarViewModel> FindCarsByLocation(double lat, double lng);
        void DeleteCar(int id);
    }
}