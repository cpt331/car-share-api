//======================================
//
//Name: CarRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        //This class inherits the IcarRepository register
        public CarRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public Car Add(Car item)
        {
            //Function to add a new car item to the DB and save changes
            var car = Context.Cars.Add(item);
            Context.SaveChanges();
            return car;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a car based on ID before saving
            var car = Context.Cars.FirstOrDefault(x => x.VehicleID == id);
            if (car != null)
            {
                Context.Entry(car).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public Car Find(int id)
        {
            //Finds a car based on the car ID and returns first input
            var car = Context.Cars.FirstOrDefault(x => x.VehicleID == id);
            return car;
        }

        public List<Car> FindAll()
        {
            //Finds all cars and returns a list
            return Context.Cars.ToList();
        }

        public IQueryable<Car> Query()
        {
            return Context
                .Cars
                .Include(x => x.CarCategory1)
                .AsQueryable();
        }


        public Car Update(Car item)
        {
            //The car state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Dispose()
        {
            //Discards the context
            Context?.Dispose();
        }
    }
}