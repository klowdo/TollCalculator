namespace TollCalculator.Domain.Models
{
    public interface IVehicle
    {
        string Id { get; set; }
        string VehicleType { get; }
    }
}