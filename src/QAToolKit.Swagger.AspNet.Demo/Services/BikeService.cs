using QAToolKit.Swagger.AspNet.Demo.Models;
using System.Collections.Generic;
using System.Linq;

namespace QAToolKit.Swagger.AspNet.Demo.Services
{
    public class BikeService
    {
        private List<Bicycle> bicycles = new List<Bicycle>();

        public BikeService()
        {
            bicycles = new List<Bicycle>() {
            new Bicycle(){
                Id = 1,
                Name = "Foil",
                Brand = "Scott",
                Type = BicycleType.Road
            },
            new Bicycle(){
                Id = 2,
                Name = "CAADX",
                Brand = "Cannondale",
                Type = BicycleType.Gravel
            },
            new Bicycle(){
                Id = 3,
                Name = "Turbo Vado SL Equipped",
                Brand = "Specialized",
                Type = BicycleType.City
            },
            new Bicycle(){
                Id = 4,
                Name = "EXCEED CFR",
                Brand = "Canyon",
                Type = BicycleType.Mountain
            }
            };
        }

        public List<Bicycle> GetAll(BicycleType? bikeType)
        {
            if (bikeType == null)
            {
                return bicycles.ToList();
            }
            else
            {
                return bicycles.Where(b => b.Type == bikeType).ToList();
            }
        }

        public Bicycle GetById(int bikeId)
        {
            return bicycles.FirstOrDefault(b => b.Id == bikeId);
        }

        public Bicycle CreateNew(Bicycle bicycle)
        {
            return new Bicycle()
            {
                Id = 5,
                Name = "EXCEED CFR",
                Brand = "Giant",
                Type = BicycleType.Mountain
            };
        }

        public Bicycle Update(int id, Bicycle bicycle)
        {
            return bicycle;
        }

        public void Delete(int id)
        {
            return;
        }
    }
}
