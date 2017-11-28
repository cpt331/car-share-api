//======================================
//
//Name: CarCategoryRepository.cs
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
    public class CarCategoryRepository : ICarCategoryRepository
    {
        //This class inherits the ICarCategoryRepository register
        public CarCategoryRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }

        public CarCategory Add(CarCategory item)
        {
            //Function to add a new car category item to the DB and save
            var category = Context.CarCategories.Add(item);
            Context.SaveChanges();
            return category;
        }

        public CarCategory Find(string id)
        {
            //Finds a booking based on the car ID and returns first input
            var category = Context.CarCategories.FirstOrDefault(x => 
            x.Category == id);
            return category;
        }

        public List<CarCategory> FindAll()
        {
            //Finds all car categories and returns a list
            return Context.CarCategories.ToList();
        }

        public IQueryable<CarCategory> Query()
        {
            return Context.CarCategories.AsQueryable();
        }

        public CarCategory Update(CarCategory item)
        {
            //car category state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(string id)
        {
            //Allows the user to delete a car category based on ID 
            var category = Context.CarCategories.FirstOrDefault(x => 
            x.Category == id);
            if (category != null)
            {
                Context.Entry(category).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            //Discards the context
            Context?.Dispose();
        }
    }
}