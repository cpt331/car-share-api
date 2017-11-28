//======================================
//
//Name: CarCategoryViewModel.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CarCategoryViewModel
    {
        //this view model allows the parsing of car category information
        //as the billing rate is dependant on the car category within the
        //car category table. this holds two fields being type (ie suv, sedan)
        //and an hourly rate

        public CarCategoryViewModel()
        {
        }

        public CarCategoryViewModel(CarCategory category)
        {
            Name = category.Category;
            BillingRate = category.BillingRate;
        }

        public string Name { get; set; }
        public decimal BillingRate { get; set; }
    }
}