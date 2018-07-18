using System.Collections.Generic;
using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class VisitorTollCalculator : ITollCalculator
    {
        private readonly IEnumerable<IVisitor<VehicleTollContext>> _visitors;
        private readonly TollFeeCalculatorConfig _config;

        public VisitorTollCalculator(IEnumerable<IVisitor<VehicleTollContext>> visitors, TollFeeCalculatorConfig config) {
            _visitors = visitors;
            _config = config;
        }

        public Money Calculate(IVehicle vehicle, Occurrence occurrence) => this.Calculate(vehicle, new[] {occurrence});

        public Money Calculate(IVehicle vehicle, IEnumerable<Occurrence> occurrences) {
            var context = new VehicleTollContext(vehicle, occurrences, _config.CurrencyCode);
          
            foreach (var visitor in _visitors.ForEachWhile(() => !context.IsTollFree))
            {
                context.Accept(visitor);
            }
            return context.IsTollFree 
                ? Money.Zero(_config.CurrencyCode) 
                : context.Total;
        }
    }
}
