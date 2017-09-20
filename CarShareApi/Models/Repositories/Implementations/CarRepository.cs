using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;


namespace CarShareApi.Models.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        private CarShareContext Context { get; set; }
        public CarRepository(CarShareContext context)
        {
            Context = context;
        }
        public Car Add(Car item)
        {
            var car = Context.Cars.Add(item);
            Context.SaveChanges();
            return car;
        }

        public void Delete(int id)
        {
            var car = Context.Cars.FirstOrDefault(x => x.VehicleID == id);
            if (car != null)
            {
                Context.Entry(car).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public Car Find(int id)
        {
            var car = Context.Cars.FirstOrDefault(x => x.VehicleID == id);
            return car;
        }

        public List<Car> FindAll()
        {
            return Context.Cars.ToList();
        }


        public Car Update(Car item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }
    }
}