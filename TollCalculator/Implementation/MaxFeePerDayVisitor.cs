using System;
using System.Collections.Generic;
using System.Linq;
using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class MaxFeePerDayVisitor:IVisitor<VehicleTollContext>
    {
        private readonly TollFeeCalculatorConfig _config;

        public MaxFeePerDayVisitor(TollFeeCalculatorConfig config)
        {
            _config = config;
        }
        public void Visit(VehicleTollContext element) {
            var passesPerDay = element.FeeOccurrence
                .GroupBy(_uniqueDayOfYear)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
            var newPasses = new Dictionary<string, IEnumerable<(Occurrence, Money)>>();

            foreach (var passInDay in passesPerDay)
            {
                if (passInDay.Value.Sum(x => x.Item2) > _config.MaxFeePerDay)
                {
                    newPasses[passInDay.Key] = new[] {(passInDay.Value.First().occurrence, _config.MaxFeePerDay)};
                }
            }

            foreach (var newPass in newPasses)
                passesPerDay[newPass.Key] = newPass.Value;
            element.FeeOccurrence = passesPerDay.Values.SelectMany(x => x).ToList();
        }
        private readonly Func<(Occurrence occurrence, Money), string> _uniqueDayOfYear = pass
            => $"{pass.occurrence.Date.Year}:{pass.occurrence.Date.Month}:{pass.occurrence.Date.Day}";

    }
}