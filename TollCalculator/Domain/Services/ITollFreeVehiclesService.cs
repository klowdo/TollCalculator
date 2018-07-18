using TollCalculator.Domain.Models;
using TollCalculator.Implementation;

namespace TollCalculator.Domain.Services
{
    public interface ITollFreeVehiclesService
    {
        bool IsTollFree(IVehicle vehicle);
    }
    public class TollFreeVehicleVisitor:IVisitor<VehicleTollContext>
    {
        private readonly ITollFreeVehiclesService _freeVehiclesService;

        public TollFreeVehicleVisitor(ITollFreeVehiclesService freeVehiclesService) {
            _freeVehiclesService = freeVehiclesService;
        }
        public void Visit(VehicleTollContext element) {
           if(_freeVehiclesService.IsTollFree(element.Vehicle))
               element.SetTollFree();
        }
    }
}