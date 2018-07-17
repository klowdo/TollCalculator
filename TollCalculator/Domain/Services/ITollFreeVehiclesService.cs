using TollCalculator.Domain.Models;

namespace TollCalculator.Domain.Services
{
    public interface ITollFreeVehiclesService
    {
        bool IsTollFree(IVehicle vehicle);
    }
}