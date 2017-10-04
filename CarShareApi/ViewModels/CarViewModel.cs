using CarShareApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CarViewModel
    {
        //object fields
        public int Id { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string CarCategory { get; set; }
        public string Transmission { get; set; }
        public string Suburb { get; set; }
        public string Status { get; set; }
        public decimal LatPos { get; set; }
        public decimal LongPos { get; set; }

        //calculated fields
        public double? Distance { get; set; }
        public bool IsBooked { get; set; }

        
        public CarViewModel() { }
        public CarViewModel(Car car)
        {
            Id = car.VehicleID;
            Model = car.Model;
            Make = car.Make;
            CarCategory = car.CarCategory;
            Transmission = car.Transmission;
            Suburb = car.Suburb;
            Status = car.Status;
            LatPos = car.LatPos;
            LongPos = car.LongPos;

        }
    }
}