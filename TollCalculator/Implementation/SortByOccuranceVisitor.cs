using System.Collections.Generic;
using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class SortByOccuranceVisitor : IVisitor<VehicleTollContext>, IComparer<(Occurrence, Money)>
    {
        public void Visit(VehicleTollContext element) {
            element.FeeOccurrence.Sort(this);
        }
        int IComparer<(Occurrence, Money)>.Compare((Occurrence, Money) x, (Occurrence, Money) y)
        {
            return Occurrence.Compare(x.Item1, y.Item1);
        }
    }
}