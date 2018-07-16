using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollCalculatorTimeTest
    {
        [TestCase("2018-07-13 06:00", ExpectedResult = 8, Description = "06:00 <> 06:29 8 SEK")]
        [TestCase("2018-07-13 06:30", ExpectedResult = 13, Description = "06:30 <> 06:59 13 SEK")]
        [TestCase("2018-07-13 07:00", ExpectedResult = 18, Description = "07:00 <> 07:59 18 SEK")]
        [TestCase("2018-07-13 08:10", ExpectedResult = 13, Description = "08:00 <> 08:29 13 SEK")]
        [TestCase("2018-07-13 08:30", ExpectedResult = 8, Description = "08:30 <> 14:59 8 SEK")]
        [TestCase("2018-07-13 15:00", ExpectedResult = 13, Description = "15:00 <> 15:29 13 SEK")]
        [TestCase("2018-07-13 15:30", ExpectedResult = 18, Description = "15:30 <> 16:59 18 SEK")]
        [TestCase("2018-07-13 17:00", ExpectedResult = 13, Description = "17:00 <> 17:59 13 SEK")]
        [TestCase("2018-07-13 18:00", ExpectedResult = 8, Description = "18:00 <> 18:29 8 SEK")]
        public decimal When_GetTollFee_FeeDate_ReturnsExpectedFee(string passage)
        {
            var sut = CreateSut();
            var normalVehicle = A.Dummy<IVehicle>();

            var expected = sut.Calculate(normalVehicle, PassBy.Parse(passage));

            return expected;
        }

        private ITollCalculator CreateSut()
        {
            var feeRuleRepo = A.Fake<IFeeRuleRepository>();
            A.CallTo(() => feeRuleRepo.GetAll())
                .Returns(new List<IPassFeeRuleSpecification>
                {
                    CreateSpec(new Time(06, 00), new Time(06, 29), Money.CreateSek(8)),
                    CreateSpec(new Time(06, 30), new Time(06, 59), Money.CreateSek(13)),
                    CreateSpec(new Time(07, 00), new Time(07, 59), Money.CreateSek(18)),
                    CreateSpec(new Time(08, 00), new Time(08, 29), Money.CreateSek(13)),
                    CreateSpec(new Time(08, 30), new Time(14, 59), Money.CreateSek(8)),
                    CreateSpec(new Time(15, 00), new Time(15, 29), Money.CreateSek(13)),
                    CreateSpec(new Time(15, 30), new Time(16, 59), Money.CreateSek(18)),
                    CreateSpec(new Time(10, 00), new Time(17, 59), Money.CreateSek(13)),
                    CreateSpec(new Time(18, 00), new Time(18, 29), Money.CreateSek(8)),
                });

            
            return new TollFeeCalculator(
                feeRuleRepository:feeRuleRepo,
                tollFreeDateService:A.Fake<ITollFreeDateService>(),
                tollFreeVehiclesService: A.Fake<ITollFreeVehiclesService>(),
                config: new TollFeeCalculatorConfig
                {
                    MaxFeePerDay = Money.CreateSek(60),
                    CurrencyCode = CurrencyCode.SEK
                });
        }

        private IPassFeeRuleSpecification CreateSpec(Time start, Time end, Money fee)
        {
           return new TimeRangeFeeSpecification(new TimeRange(start, end), fee);
        }
    }
}
