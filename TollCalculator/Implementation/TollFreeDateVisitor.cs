using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class TollFreeDateVisitor:IVisitor<VehicleTollContext>
    {
        private readonly ITollFreeDateService _tollFreeDateService;

        public TollFreeDateVisitor(ITollFreeDateService tollFreeDateService) {
            _tollFreeDateService = tollFreeDateService;
        }
        public void Visit(VehicleTollContext element) {
            element.FeeOccurrence.RemoveAll(x => _tollFreeDateService.IsTollFree(x.occurrence.Date));
        }
    }
}