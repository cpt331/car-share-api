//======================================
//
//Name: FakeCarRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCarRepository : ICarRepository
    {
        //implemented car repository to allow for appropriate methods
        //to enable testing
        public FakeCarRepository(List<Car> cars)
        {
            //implements the car table data set
            Cars = cars;
        }

        public FakeCarRepository()
        {
            //Create three cars for vehicle repo (testing only)
            Cars = new List<Car>
            {
                new Car
                {
                    VehicleID = 1,
                    Model = "Model S",
                    Make = "Tesla",
                    Transmission = "MN",
                    LatPos = 33,
                    LongPos = (decimal) 150.5
                },
                new Car
                {
                    VehicleID = 2,
                    Model = "Model X",
                    Make = "Tesla",
                    LatPos = 33,
                    LongPos = (decimal) 150.8
                },
                new Car
                {
                    VehicleID = 3,
                    Model = "Model 3",
                    Make = "Tesla",
                    LatPos = 33,
                    LongPos = (decimal) 150.3
                }
            };
        }

        public List<Car> Cars { get; set; }

        public Car Add(Car item)
        {
            //add item to the car repo
            item.VehicleID = new Random().Next(int.MinValue, int.MaxValue);
            Cars.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            //remove the car from the table
            Cars.RemoveAll(x => x.VehicleID == id);
        }

        public Car Find(int id)
        {
            //find car based on vehicle ID
            return Cars.FirstOrDefault(x => x.VehicleID == id);
        }

        public List<Car> FindAll()
        {
            //find all cars and return as a list
            return Cars;
        }

        public IQueryable<Car> Query()
        {
            //return querable car list
            return Cars.AsQueryable();
        }


        public Car Update(Car item)
        {
            //update the cars table with new items
            Cars.RemoveAll(x => x.VehicleID == item.VehicleID);
            Cars.Add(item);
            return item;
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}