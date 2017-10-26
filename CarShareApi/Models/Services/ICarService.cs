using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.ViewModels.Cars;

namespace CarShareApi.Models.Services
{
    public interface ICarService : IDisposable
    {
        CarViewModel FindCar(int id);
        List<CarViewModel> FindAllCars();
        List<CarViewModel> FindCarsByLocation(double lat, double lng);
        List<CarViewModel> SearchCars(CarSearchCriteria criteria);
        List<CarCategoryViewModel> GetCarCategories();
        UpdateCarResponse UpdateCar(UpdateCarRequest request);
        List<string> GetCarStatuses();
        void DeleteCar(int id);
    }
}
