using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Domain.Models;

namespace TollCalculator.Tests
{
    public class MoneyTest
    {
        [Test]
        public void When_Addition_onePlusOneSek_Returns2()
        {
            var actual = Money.CreateSek(1) + Money.CreateSek(1);
            Assert.AreEqual(2m, actual.Value);
        }

        [Test]
        public void When_Subtraction_oneMinusOneSek_Returns2()
        {
            var actual = Money.CreateSek(1) - Money.CreateSek(1);
            Assert.AreEqual(decimal.Zero, actual.Value);
        }

        [Test]
        public void When_Multiply_oneMultiplyWith2_Returns2()
        {
            var actual = Money.CreateSek(1) * 2;
            Assert.AreEqual(2m, actual.Value);
        }
        [Test]
        public void When_MoneyZero_Returns0()
        {
            var actual = Money.Zero(CurrencyCode.SEK);
            Assert.AreEqual(decimal.Zero, actual.Value);
        }

        [Test]
        public void When_MoneyEquals_MoneyValueAndCurrencyEqual_ReturnsTrue()
        {
            var money1 = Money.CreateSek(2);
            var money2 = Money.CreateSek(2);
            Assert.AreEqual(money1, money2);
        }
        [Test]
        public void When_MoneyEquals_MoneyValueAndCurrencyNotEqual_ReturnsFalse()
        {
            Assert.AreNotEqual(Money.CreateSek(2), Money.CreateSek(4));
            Assert.AreNotEqual(Money.Create(1,CurrencyCode.NOK), Money.Create(1, CurrencyCode.SEK));
        }
    }
}
