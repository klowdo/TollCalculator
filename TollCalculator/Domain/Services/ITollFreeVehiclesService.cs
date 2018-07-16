namespace TollCalculator.Domain
{
    public interface ITollFreeVehiclesService
    {
        bool IsTollFree(IVehicle vehicle);
    }
}