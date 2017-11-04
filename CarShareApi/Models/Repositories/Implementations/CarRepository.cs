using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        public CarRepository(CarShareContext context)
        {
            Context = context;
        }

        private CarShareContext Context { get; }

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

        public IQueryable<Car> Query()
        {
            return Context
                .Cars
                .Include(x => x.CarCategory1)
                .AsQueryable();
        }


        public Car Update(Car item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}