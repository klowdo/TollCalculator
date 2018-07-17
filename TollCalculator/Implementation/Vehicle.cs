using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class Vehicle:IVehicle
    {
        public string Id { get; set; }
        public Vehicle(string type) => VehicleType = type;
        public string VehicleType { get; }
    }
}