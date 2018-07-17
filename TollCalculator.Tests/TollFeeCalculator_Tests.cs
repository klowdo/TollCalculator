using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        [Test]
        public void When_Calculate_TollFreeVehicle_ReturnMoneyZero()
        {
            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(Money.CreateSek(100));
            var sut = CreateSut(
                tollFreeVehicles: true,
                passFeeRules:feeRule
            );

            var actual = sut.Calculate(A.Dummy<IVehicle>(), A.Dummy<Occurrence>());


            Assert.AreEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        [Test]
        public void When_Calculate_TollFreeDate_ReturnMoneyZero()
        {
            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(Money.CreateSek(100));
            var sut = CreateSut(
                tollFreeDate: true,
                passFeeRules:feeRule
            );

            var actual = sut.Calculate(A.Dummy<IVehicle>(), A.Dummy<Occurrence>());

            Assert.AreEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        [Test]
        public void When_Calculate_NoMatchinRule_ReturnMoneyZero()
        {
            var sut = CreateSut();

            var actual = sut.Calculate(A.Dummy<IVehicle>(), A.Dummy<Occurrence>());

            Assert.AreEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        [Test]
        public void When_Calculate_MatchingRule_ReturnFeeRuleMoney()
        {
            var expectedMoney = Money.CreateSek(40);
            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(expectedMoney);
            var sut = CreateSut(
                passFeeRules: feeRule
            );

            var actual = sut.Calculate(A.Dummy<IVehicle>(), A.Dummy<Occurrence>());

            Assert.AreEqual(expectedMoney, actual);
        }
        [Test]
        public void When_Calculate_MultipleMatchingRule_ReturnFeeFromFirst()
        {
            _config.MaxFeePerDay = Money.CreateSek(3000);
            var expectedMoney = Money.CreateSek(40);
            var feeRule1 = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule1.Fee)
                .Returns(expectedMoney);

            var feeRule2= CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule2.Fee)
                .Returns(Money.CreateSek(2000));

            var sut = CreateSut(
              passFeeRules:  new [] { feeRule1, feeRule2 }
            );

            var actual = sut.Calculate(A.Dummy<IVehicle>(), A.Dummy<Occurrence>());

            Assert.AreEqual(expectedMoney, actual);
        }

        [Test]
        public void When_Calculate_MultiplePassesWithinHourRange_ReturnsHighestFeeFromOnePass()
        {
            var expectedMoney = Money.CreateSek(100);

            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(expectedMoney);

            var sut = CreateSut(passFeeRules: feeRule);

            var passBy1 = Occurrence.Parse("2018-07-12 06:30");
            var passBy2 = new Occurrence(passBy1.Date.AddMinutes(30));

            var actual = sut.Calculate(A.Dummy<IVehicle>(), new[] {passBy1, passBy2});

            Assert.AreEqual(expectedMoney, actual);
        }

        [Test]
        public void When_Calculate_MultiplePassesOverMoneyHours_Returns200Sek()
        {
            var passFee = Money.CreateSek(100);
            _config.MaxFeePerDay = Money.CreateSek(3000);

            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(passFee);

            var sut = CreateSut(passFeeRules: feeRule);

            var passBy1 = Occurrence.Parse("2018-07-12 06:30");
            var passBy2 = new Occurrence(passBy1.Date.AddHours(2));

            var passBys = new[] { passBy1, passBy2 };
            var actual = sut.Calculate(A.Dummy<IVehicle>(), passBys);

            var expectedMoney = passFee * passBys.Length;
            Assert.AreEqual(expectedMoney, actual);
        }
        [Test]
        public void When_Calculate_MutiplePassesAndmaxFee60Sek_Returns60Sek()
        {
            _config.MaxFeePerDay = Money.CreateSek(60);

            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(Money.CreateSek(100));

            var sut = CreateSut(passFeeRules: feeRule);

            var passBy1 = Occurrence.Parse("2018-07-12 06:30");
            var passBy2 = new Occurrence(passBy1.Date.AddHours(2));

            var passBys = new[] { passBy1, passBy2 };
            var actual = sut.Calculate(A.Dummy<IVehicle>(), passBys);

            Assert.AreEqual(_config.MaxFeePerDay, actual);
        }

        [Test]
        public void When_Calculate_Fee40PerPassBy3Passes2DaysAndmaxFee60Sek_Returns100Sek()
        {
            var feePerPass = Money.CreateSek(40);

            var feeRule = CreateBoolService<IPassFeeRuleSpecification>(returnsOnAllCalls: true);
            A.CallTo(() => feeRule.Fee)
                .Returns(feePerPass);

            var sut = CreateSut(passFeeRules: feeRule);

            var passBy1 = Occurrence.Parse("2018-07-12 06:30");
            var passBy2 = new Occurrence(passBy1.Date.AddHours(2));
            var passBy3 = new Occurrence(passBy1.Date.AddDays(1));

            var passBys = new[] { passBy1, passBy2, passBy3 };
            var actual = sut.Calculate(A.Dummy<IVehicle>(), passBys);

            var expected = Money.CreateSek(100);

            Assert.AreEqual(expected, actual);
        }


        private static TType CreateBoolService<TType>(bool returnsOnAllCalls)
        {
            var service = A.Fake<TType>();
            A.CallTo(service)
                .WithReturnType<bool>()
                .Returns(returnsOnAllCalls);
            return service;
        }

        readonly TollFeeCalculatorConfig _config = new TollFeeCalculatorConfig {
            CurrencyCode = CurrencyCode.SEK,
            MaxFeePerDay = Money.CreateSek(60)
        };
        private TollFeeCalculator CreateSut(
            
            bool tollFreeDate = false,
            bool tollFreeVehicles = false,
            params  IPassFeeRuleSpecification[] passFeeRules)
        {
            var feeRuleRepo = A.Fake<IFeeRuleRepository>();
            A.CallTo(() => feeRuleRepo.GetAll())
                .Returns(passFeeRules?.ToList() ?? A.Fake<IList<IPassFeeRuleSpecification>>());
            return new TollFeeCalculator(
                feeRuleRepo,
                CreateBoolService<ITollFreeDateService>(tollFreeDate),
                CreateBoolService<ITollFreeVehiclesService>(tollFreeVehicles),
                _config);
        }
    }
}
