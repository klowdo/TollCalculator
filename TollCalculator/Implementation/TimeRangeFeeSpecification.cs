using System;
using System.Collections.Generic;
using System.Text;
using TollCalculator.Domain;

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
        public bool IsSatisfied(PassBy input) => _timeRange.IsInRange(input.Date);

        public Money Fee { get; }
    }
}
