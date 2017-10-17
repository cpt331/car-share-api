using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CarCategoryViewModel
    {
        public string Name { get; set; }
        public decimal BillingRate { get; set; }

        public CarCategoryViewModel() { }

        public CarCategoryViewModel(CarCategory category)
        {
            Name = category.Category;
            BillingRate = category.BillingRate;
        }
    }
}