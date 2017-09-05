using CarShareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareApi.Models.Services
{
    public interface ICarService
    {
        CarViewModel Find(int id);
        List<CarViewModel> FindAll();
        List<CarViewModel> FindByLocation(double lat, double lng);
    }
}
