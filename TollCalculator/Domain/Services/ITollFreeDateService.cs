using System;

namespace TollCalculator.Domain.Services
{
    public interface ITollFreeDateService
    {
        bool IsTollFree(DateTimeOffset passBy);
    }
}
