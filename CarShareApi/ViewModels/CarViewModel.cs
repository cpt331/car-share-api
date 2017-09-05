using CarShareApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class CarViewModel
    {
        //object fields
        public int Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        //calculated fields
        public double? Distance { get; set; }
        public bool IsBooked { get; set; }

        
        public CarViewModel() { }
        public CarViewModel(Car car)
        {
            Id = car.Id;
            Model = car.Model;
            Manufacturer = car.Manufacturer;
            Year = car.Year;
            Lat = car.Lat;
            Lng = car.Lng;

        }
    }
}