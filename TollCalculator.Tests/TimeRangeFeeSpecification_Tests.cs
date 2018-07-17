using NUnit.Framework;
using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TimeRangeFeeSpecification_Tests
    {
        [Test]
        public void When_IsSatisfied_WithInRange_ReturnTrue()
        {
            var fee = Money.CreateSek(100);
            var sut = CreateSut(fee);

            var actual = sut.IsSatisfied(Occurrence.Parse("2013-01-05 06:05"));

            Assert.IsTrue(actual);
            Assert.AreEqual(fee, sut.Fee);
        }
        [Test]
        public void When_IsSatisfied_OutOfRange_ReturnFalse()
        {
            var fee = Money.CreateSek(100);
            var sut = CreateSut(fee);

            var actual = sut.IsSatisfied(Occurrence.Parse("2013-01-05 06:30"));

            Assert.IsFalse(actual);
            Assert.AreEqual(fee, sut.Fee);
        }

        private static IPassFeeRuleSpecification CreateSut(Money fee)
        {
            var start = new Time(06, 00);
            var end = new Time(06, 29);
            var timeRange = new TimeRange(start: start, end: end);
            return new TimeRangeFeeSpecification(timeRange, fee);
        }
    }
}
