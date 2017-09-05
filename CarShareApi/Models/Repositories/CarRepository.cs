using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        public List<Car> Cars { get; set; }
        public CarRepository()
        {
            Cars = new List<Car>() {
                new Car {
                    Id = 1,
                    Model = "Model S",
                    Manufacturer = "Tesla",
                    Year = 2017,
                    Lat = 33,
                    Lng = 150.5
                },
                new Car {
                    Id = 2,
                    Model = "Model X",
                    Manufacturer = "Tesla",
                    Year = 2017,
                    Lat = 33,
                    Lng = 150.8
                },
                new Car {
                    Id = 3,
                    Model = "Model 3",
                    Manufacturer = "Tesla",
                    Year = 2017,
                    Lat = 33,
                    Lng = 150.3
                }
            };
        }
        public Car Add(Car item)
        {
            Cars.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Cars.RemoveAll(x => x.Id == id);
        }

        public Car Find(int id)
        {
            return Cars.Single(x => x.Id == id);
        }

        public List<Car> FindAll()
        {
            return Cars;
        }


        public Car Update(Car item)
        {
            Cars.RemoveAll(x => x.Id == item.Id);
            Cars.Add(item);
            return item;
        }
    }
}