using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Domain.Models;
using TollCalculator.Domain.Services;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollFreeVehicles_Test
    {
        [TestCase(TollFreeVehicleTypes.Tractor)]
        [TestCase(TollFreeVehicleTypes.Motorbike)]
        [TestCase(TollFreeVehicleTypes.Tractor)]
        [TestCase(TollFreeVehicleTypes.Emergency)]
        [TestCase(TollFreeVehicleTypes.Diplomat)]
        [TestCase(TollFreeVehicleTypes.Foreign)]
        [TestCase(TollFreeVehicleTypes.Military)]
        public void When_Calculate_TollFreeVehicle_RetursZero(TollFreeVehicleTypes tollFreeVehicleTypes)
        {
            var sut = CreateSut();
            var vehicle = A.Fake<IVehicle>();
            A.CallTo(() => vehicle.VehicleType)
                .Returns(tollFreeVehicleTypes.ToString());

            var actual = sut.Calculate(vehicle, A.Dummy<Occurrence>());

            Assert.AreEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        [TestCase("Car")]
        [TestCase("Plane")]
        [TestCase("bobbyCar")]
        public void When_Calculate_NonTollFreeVehicle_RetursFee(string vehicleType)
        {
            var sut = CreateSut();
            var vehicle = A.Fake<IVehicle>();
            A.CallTo(() => vehicle.VehicleType)
                .Returns(vehicleType);

            var actual = sut.Calculate(vehicle, A.Dummy<Occurrence>());

            Assert.AreNotEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        
        private TollFeeCalculator CreateSut()
        {
            var feeRuleRepo = A.Fake<IFeeRuleRepository>();
            var truthySpec = A.Fake<IPassFeeRuleSpecification>();
            A.CallTo(() => truthySpec.IsSatisfied(A<Occurrence>._))
                .Returns(true);
            A.CallTo(() => truthySpec.Fee)
                .Returns(Money.CreateSek(60));

            A.CallTo(() => feeRuleRepo.GetAll())
                .Returns(new[] { truthySpec });


            var freeVehicleTypes = new List<string>
            {
                TollFreeVehicleTypes.Tractor.ToString(),
                TollFreeVehicleTypes.Motorbike.ToString(),
                TollFreeVehicleTypes.Tractor.ToString(),
                TollFreeVehicleTypes.Emergency.ToString(),
                TollFreeVehicleTypes.Diplomat.ToString(),
                TollFreeVehicleTypes.Foreign.ToString(),
                TollFreeVehicleTypes.Military.ToString(),
            };


            var tollFreeVehiclesService = A.Fake<ITollFreeVehiclesService>();

            A.CallTo(() => tollFreeVehiclesService.IsTollFree(A<IVehicle>._))
                .WhenArgumentsMatch(args => freeVehicleTypes.Contains(args.Get<IVehicle>(0).VehicleType))
                .Returns(true);
            return new TollFeeCalculator(
                feeRuleRepository: feeRuleRepo,
                tollFreeDateService: A.Fake<ITollFreeDateService>(),
                tollFreeVehiclesService: tollFreeVehiclesService,
                config: new TollFeeCalculatorConfig {
                    MaxFeePerDay = Money.CreateSek(60),
                    CurrencyCode = CurrencyCode.SEK
                });
        }
    }
}
