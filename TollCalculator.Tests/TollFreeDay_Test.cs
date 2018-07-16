using System;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollFreeDay_Test
    {
        [TestCase("2018-07-15 06:00", Description = "Sunday")]
        [TestCase("2018-07-14 06:00", Description = "Saturday")]
        [TestCase("2013-01-01 06:00")]
        [TestCase("2013-03-28 06:00")]
        [TestCase("2013-03-29 06:00")]
        [TestCase("2013-04-01 06:00")]
        [TestCase("2013-04-30 06:00")]
        [TestCase("2013-05-01 06:00")]
        [TestCase("2013-05-08 06:00")]
        [TestCase("2013-05-09 06:00")]
        [TestCase("2013-06-05 06:00")]
        [TestCase("2013-06-06 06:00")]
        [TestCase("2013-06-21 06:00")]
        [TestCase("2013-07-21 06:00")]
        [TestCase("2013-11-01 06:00")]
        [TestCase("2013-12-24 06:00")]
        [TestCase("2013-12-25 06:00")]
        [TestCase("2013-12-26 06:00")]
        [TestCase("2013-12-31 06:00")]
        public void When_GetTollFee_TollFreeDate_ReturnsZeroFee(string passage)
        {
            var passBy = PassBy.Parse(passage);
            var sut = CreateSut(passBy.Date);
            var normalVehicle = A.Fake<IVehicle>();

            var actual = sut.Calculate(normalVehicle, passBy);

            Assert.AreEqual(Money.Zero(CurrencyCode.SEK), actual);

            actual = sut.Calculate(normalVehicle, new PassBy(passBy.Date.AddHours(1)));
            Assert.AreNotEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        private TollFeeCalculator CreateSut(DateTimeOffset freeDate)
        {
            var feeRuleRepo = A.Fake<IFeeRuleRepository>();
            var truthySpec = A.Fake<IPassFeeRuleSpecification>();
            A.CallTo(() => truthySpec.IsSatisfied(A<PassBy>._))
                .Returns(true);
            A.CallTo(() => truthySpec.Fee)
                .Returns(Money.CreateSek(100));

            A.CallTo(() => feeRuleRepo.GetAll())
                .Returns(new []{ truthySpec });

            var tollFreeDateService = A.Fake<ITollFreeDateService>();

            A.CallTo(() => tollFreeDateService.IsTollFree(A<DateTimeOffset>.That.IsEqualTo(freeDate)))
                .Returns(true);
            


            return new TollFeeCalculator(
                feeRuleRepository: feeRuleRepo,
                tollFreeDateService: tollFreeDateService,
                tollFreeVehiclesService: A.Fake<ITollFreeVehiclesService>(),
                config: new TollFeeCalculatorConfig {
                    MaxFeePerDay = Money.CreateSek(60),
                    CurrencyCode = CurrencyCode.SEK
                });
        }
    }
}