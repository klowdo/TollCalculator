using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class TimeRangeFeeSpecification:IPassFeeRuleSpecification
    {
        private readonly TimeRange _timeRange;

        public TimeRangeFeeSpecification(TimeRange timeRange, Money fee)
        {
            _timeRange = timeRange;
            Fee = fee;
        }
        public bool IsSatisfied(Occurrence input) => _timeRange.IsInRange(input.Date);

        public Money Fee { get; }
    }
}
