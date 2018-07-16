using System.Collections.Generic;
using TollCalculator.Domain.Models;

namespace TollCalculator.Domain.Services
{
    public interface ITollCalculator
    {
        Money Calculate(IVehicle vehicle, PassBy passBy);
        Money Calculate(IVehicle vehicle, IEnumerable<PassBy> passes);
    }
}
