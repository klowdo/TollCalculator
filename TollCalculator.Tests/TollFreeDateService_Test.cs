using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollFreeDateServiceTest
    {
        [Test]
        public void When_IsTollFree_TollFreeDate_ReturnTrue()
        {
            var spec = A.Fake<ISpecification<DateTimeOffset>>();
            A.CallTo(spec)
                .WithReturnType<bool>()
                .Returns(true);
            var sut = CreateSut(spec);

            Assert.IsTrue(sut.IsTollFree(A.Dummy<DateTimeOffset>()));
        }

        [Test]
        public void When_IsTollFree_NotTollFreeDate_ReturnFalse()
        {
            var spec = A.Fake<ISpecification<DateTimeOffset>>();
            A.CallTo(spec)
                .WithReturnType<bool>()
                .Returns(false);
            var sut = CreateSut(spec);

            Assert.IsFalse(sut.IsTollFree(A.Dummy<DateTimeOffset>()));
        }

        public static TollFreeDateService CreateSut(params ISpecification<DateTimeOffset>[] specifications )
        {
            return new TollFreeDateService(specifications);
        }
    }
}
