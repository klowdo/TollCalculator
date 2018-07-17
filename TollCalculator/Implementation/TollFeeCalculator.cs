using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TollCalculator.Domain;
using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class TollFeeCalculator:ITollCalculator
    {
        private readonly IFeeRuleRepository _feeRuleRepository;
        private readonly ITollFreeDateService _tollFreeDateService;
        private readonly ITollFreeVehiclesService _tollFreeVehiclesService;
        private readonly TollFeeCalculatorConfig _config;
        private readonly CurrencyCode _currencyCode;

        public TollFeeCalculator(
            IFeeRuleRepository feeRuleRepository,
            ITollFreeDateService tollFreeDateService,
            ITollFreeVehiclesService tollFreeVehiclesService,
            TollFeeCalculatorConfig config)
        {
            _feeRuleRepository = feeRuleRepository;
            _tollFreeDateService = tollFreeDateService;
            _tollFreeVehiclesService = tollFreeVehiclesService;
            _config = config;
            _currencyCode = config.CurrencyCode;
        }
        public Money Calculate(IVehicle vehicle, Occurrence occurrence)
        {
            if (_tollFreeVehiclesService.IsTollFree(vehicle))
                return Money.Zero(_currencyCode);

            if(_tollFreeDateService.IsTollFree(occurrence.Date))
                return Money.Zero(_currencyCode);

            var matcingRule = _feeRuleRepository.GetAll()
                .FirstOrDefault(spec => spec.IsSatisfied(occurrence));

            var fee = matcingRule?.Fee ?? Money.Zero(_currencyCode);

            return TruncateFeeExceedsHigestPerDay(fee);
        }

        public Money Calculate(IVehicle vehicle, IEnumerable<Occurrence> passes)
        {

            var sortedPasses = passes.ToList();
            sortedPasses.Sort(Occurrence.Compare);

            var passesPerDay = sortedPasses
                .GroupBy(_uniqueDayOfYear);

            var feesPerDay = new List<Money>();
            foreach (var dayPasses in passesPerDay) {
                feesPerDay.Add(GetFeeForDay(vehicle, dayPasses));
            }

            return Money.Create(feesPerDay
                .Select(TruncateFeeExceedsHigestPerDay)
                .Sum(fee => fee.Value),_currencyCode);
        }

        private Money GetFeeForDay(IVehicle vehicle,  IEnumerable<Occurrence> dayPasses)
        {
            var feeForDay = Money.Zero(_currencyCode);
            var passBysInRange = new List<Occurrence>();
            TimeRange timeRange = null;
            foreach (var dayPass in dayPasses)
            {
                if (timeRange is null)
                    timeRange = GetTimeRange(dayPass);
                if (timeRange.IsInRange(dayPass.Date))
                {
                    passBysInRange.Add(dayPass);
                }
                else
                {
                    timeRange = GetTimeRange(dayPass);
                    feeForDay += passBysInRange.Max(pass => Calculate(vehicle, pass));
                    passBysInRange.Clear();
                    passBysInRange.Add(dayPass);
                }
            } 

            if (passBysInRange.Any())
            {
                var maxInHour = passBysInRange.Max(pass => Calculate(vehicle, pass));
                feeForDay += maxInHour;
            }

            return feeForDay;
        }

        private Money TruncateFeeExceedsHigestPerDay(Money fee) => fee > _config.MaxFeePerDay ? _config.MaxFeePerDay : fee;

         
        private TimeRange GetTimeRange(Occurrence firstPassage)
        {
            var firstPassageTime = Time.CreateFrom(firstPassage.Date);
            return new TimeRange(firstPassageTime, _config.FeeGroupingRange);
        }

        private readonly Func<Occurrence, string> _uniqueDayOfYear = pass => $"{pass.Date.Year}:{pass.Date.Month}:{pass.Date.Day}";

    }
}
