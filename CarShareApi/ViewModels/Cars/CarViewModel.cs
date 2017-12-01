//======================================
//
//Name: CarViewModel.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CarViewModel
    {
        //The car view model allows the user to be able to view details on 
        //the car including make model car category transmission suburbs 
        //status and position as well as the billing rate.
        public CarViewModel()
        {
        }

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
            if (car.CarCategory1 != null)
                BillingRate = car.CarCategory1.BillingRate;
        }

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

        public decimal? BillingRate { get; set; }
    }
}