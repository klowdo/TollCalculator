using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TollCalculator.Domain;

namespace TollCalculator.Tests
{
    public class TimeRange_Test
    {
        [Test]
        public void When_IsInRange_TimeIsInRange_ReturnTrue()
        {
            var sut = new TimeRange(new Time(06, 12), TimeSpan.FromHours(1));

            var actual = sut.IsInRange(DateTimeOffset.Parse("2013-05-12 06:15"));

            Assert.IsTrue(actual);
        }
    }
}
