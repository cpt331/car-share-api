using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class CarCategoryViewModel
    {
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