using System.Collections.Generic;
using System.Linq;
using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class TimeRangeGroupVisitor:IVisitor<VehicleTollContext>
    {
        private readonly TollFeeCalculatorConfig _config;

        public TimeRangeGroupVisitor(TollFeeCalculatorConfig config) {
            _config = config;
        }
        public void Visit(VehicleTollContext element) {
           
            var newFees = new List<(Occurrence occurrence, Money fee)>();
            var passBysInRange = new List<(Occurrence occurrence, Money fee)>();
            TimeRange timeRange = null;
            foreach (var feeOccurnce in element.FeeOccurrence) {
                if (timeRange is null)
                    timeRange = GetTimeRange(feeOccurnce.occurrence);
                if (timeRange.IsInRange(feeOccurnce.occurrence.Date)) {
                    passBysInRange.Add(feeOccurnce);
                }
                else {
                    timeRange = GetTimeRange(feeOccurnce.occurrence);
                    var money = passBysInRange.Max(pass => pass.fee);
                    newFees.Add(passBysInRange.First(x => x.fee.Equals(money)));
                    passBysInRange.Clear();
                    passBysInRange.Add(feeOccurnce);
                }
            }

            if (passBysInRange.Any()) {
                var money = passBysInRange.Max(pass => pass.fee);
                newFees.Add(passBysInRange.First(x => x.fee.Equals(money)));
            }

            element.FeeOccurrence = newFees;
        }
        private TimeRange GetTimeRange(Occurrence firstPassage)
        {
            var firstPassageTime = Time.CreateFrom(firstPassage.Date);
            return new TimeRange(firstPassageTime, _config.FeeGroupingRange);
        }
    }
}