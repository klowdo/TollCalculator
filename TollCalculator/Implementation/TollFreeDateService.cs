using System;
using System.Collections.Generic;
using System.Linq;
using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class TollFreeDateService:ITollFreeDateService
    {
        private readonly IEnumerable<ISpecification<DateTimeOffset>> _specifications;

        public TollFreeDateService(IEnumerable<ISpecification<DateTimeOffset>> specifications)
        {
            _specifications = specifications;
        }
        public bool IsTollFree(DateTimeOffset passBy) => _specifications.Any(x => x.IsSatisfied(passBy));
    }
}
