using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeCarRepository : ICarRepository
    {
        public List<Car> Cars { get; set; }
        public FakeCarRepository()
        {
            Cars = new List<Car>() {
                new Car {
                    VehicleID = 1,
                    Model = "Model S",
                    Make = "Tesla",
                    Transmission = "MN",
                    LatPos = (decimal)33,
                    LongPos = (decimal)150.5
                },
                new Car {
                    VehicleID = 2,
                    Model = "Model X",
                    Make = "Tesla",
                    LatPos = (decimal)33,
                    LongPos = (decimal)150.8
                },
                new Car {
                    VehicleID = 3,
                    Model = "Model 3",
                    Make = "Tesla",
                    LatPos = (decimal)33,
                    LongPos = (decimal)150.3
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
            Cars.RemoveAll(x => x.VehicleID == id);
        }

        public Car Find(int id)
        {
            return Cars.FirstOrDefault(x => x.VehicleID == id);
        }

        public List<Car> FindAll()
        {
            return Cars;
        }

        public IQueryable<Car> Query()
        {
            return Cars.AsQueryable();
        }


        public Car Update(Car item)
        {
            Cars.RemoveAll(x => x.VehicleID == item.VehicleID);
            Cars.Add(item);
            return item;
        }
    }
}
