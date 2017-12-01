//======================================
//
//Name: FakeCarCategoryRepository.cs
//Version: 1.0
//Date: 03/12/2017
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
    public class FakeCarCategoryRepository : ICarCategoryRepository
    {
        //implemented category repository to allow for appropriate methods
        //to enable testing

        public FakeCarCategoryRepository(List<CarCategory> categories)
        {
            //implements the categories table data
            Categories = categories;
        }

        public FakeCarCategoryRepository()
        {
            Categories = new List<CarCategory>
            {
                //Create three categories for vehicle types (testing only)
                new CarCategory {Category = "Small", BillingRate = 5},
                new CarCategory {Category = "Medium", BillingRate = 6},
                new CarCategory {Category = "Large", BillingRate = 7}
            };
        }

        public List<CarCategory> Categories { get; set; }


        public CarCategory Add(CarCategory item)
        {
            //add item to the car category repo
            Categories.Add(item);
            return item;
        }

        public CarCategory Find(string id)
        {
            //find car category based on ID
            return Categories.FirstOrDefault(x =>
                x.Category.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public List<CarCategory> FindAll()
        {
            //return all car categories to list
            return Categories;
        }

        public IQueryable<CarCategory> Query()
        {
            //return querable category list
            return Categories.AsQueryable();
        }

        public CarCategory Update(CarCategory item)
        {
            //update the car category with new items
            Categories.RemoveAll(x => x.Category.Equals(item.Category,
                StringComparison.InvariantCultureIgnoreCase));
            Categories.Add(item);
            return item;
        }

        public void Delete(string id)
        {
            //remove the car category from the table
            Categories.RemoveAll(x =>
                x.Category.Equals(id,
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            //not implemented
        }
    }
}