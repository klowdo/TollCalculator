using System.Collections.Generic;
using TollCalculator.Domain.Models;

namespace TollCalculator.Domain.Services
{
    public interface ITollCalculator
    {
        Money Calculate(IVehicle vehicle, Occurrence occurrence);
        Money Calculate(IVehicle vehicle, IEnumerable<Occurrence> passes);
    }
}
