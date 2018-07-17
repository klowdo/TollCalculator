using System;
using TollCalculator.Domain;
using TollCalculator.Domain.Models;

namespace TollCalculator.Implementation
{
    public class TollFeeCalculatorConfig {
        public CurrencyCode CurrencyCode { get; set; }
        public Money MaxFeePerDay { get; set; }
        public TimeSpan FeeGroupingRange { get; set; } = TimeSpan.FromHours(1);
    }
}