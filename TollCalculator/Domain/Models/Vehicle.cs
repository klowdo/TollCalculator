using TollCalculator.Domain;

namespace TollCalculator
{
    public class Vehicle:IVehicle
    {
        public Vehicle(VehicleType type) => VehicleType = type.ToString();
        public string VehicleType { get; }
    }
}