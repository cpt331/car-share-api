using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class CarCategoryRepository : ICarCategoryRepository
    {
        public CarCategoryRepository(CarShareContext context)
        {
            Context = context;
        }

        private CarShareContext Context { get; }

        public CarCategory Add(CarCategory item)
        {
            var category = Context.CarCategories.Add(item);
            Context.SaveChanges();
            return category;
        }

        public CarCategory Find(string id)
        {
            var category = Context.CarCategories.FirstOrDefault(x => x.Category == id);
            return category;
        }

        public List<CarCategory> FindAll()
        {
            return Context.CarCategories.ToList();
        }

        public IQueryable<CarCategory> Query()
        {
            return Context.CarCategories.AsQueryable();
        }

        public CarCategory Update(CarCategory item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(string id)
        {
            var category = Context.CarCategories.FirstOrDefault(x => x.Category == id);
            if (category != null)
            {
                Context.Entry(category).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}