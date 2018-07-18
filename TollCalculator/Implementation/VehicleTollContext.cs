using System.Collections.Generic;
using System.Linq;
using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class VehicleTollContext:IAcceptVisitor<VehicleTollContext>
    {
        private readonly CurrencyCode _currencyCode;
        public bool IsTollFree { get; private set; } = false;
        public IVehicle Vehicle { get; }
        public IEnumerable<Occurrence> Occurrences { get; }
        public List<(Occurrence occurrence, Money fee)> FeeOccurrence = new List<(Occurrence occurrence, Money fee)>();

        public VehicleTollContext(IVehicle vehicle, IEnumerable<Occurrence> occurrences, CurrencyCode currencyCode) {
            _currencyCode = currencyCode;
            Vehicle = vehicle;
            Occurrences = occurrences;
        }

        public void SetTollFree() {
            IsTollFree = true;
        }

        public Money Total => Money.Create(FeeOccurrence.Sum(c => c.fee), _currencyCode);
        public void Accept(IVisitor<VehicleTollContext> visitor) {
            visitor.Visit(this);
        }
    }
}