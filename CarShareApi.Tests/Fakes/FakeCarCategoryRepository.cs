using System;
using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCarCategoryRepository : ICarCategoryRepository
    {
        public FakeCarCategoryRepository(List<CarCategory> categories)
        {
            Categories = categories;
        }

        public FakeCarCategoryRepository()
        {
            Categories = new List<CarCategory>
            {
                new CarCategory {Category = "Small", BillingRate = 5},
                new CarCategory {Category = "Medium", BillingRate = 6},
                new CarCategory {Category = "Large", BillingRate = 7}
            };
        }

        public List<CarCategory> Categories { get; set; }


        public CarCategory Add(CarCategory item)
        {
            Categories.Add(item);
            return item;
        }

        public CarCategory Find(string id)
        {
            return Categories.FirstOrDefault(x =>
                x.Category.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public List<CarCategory> FindAll()
        {
            return Categories;
        }

        public IQueryable<CarCategory> Query()
        {
            return Categories.AsQueryable();
        }

        public CarCategory Update(CarCategory item)
        {
            Categories.RemoveAll(x => x.Category.Equals(item.Category,
                StringComparison.InvariantCultureIgnoreCase));
            Categories.Add(item);
            return item;
        }

        public void Delete(string id)
        {
            Categories.RemoveAll(x =>
                x.Category.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            //nah
        }
    }
}