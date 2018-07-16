using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using TollCalculator.Domain;
using TollCalculator.Implementation;

namespace TollCalculator.Tests
{
    public class TollFreeVehicles_Test
    {
        [TestCase(VehicleType.Tractor)]
        [TestCase(VehicleType.Motorbike)]
        [TestCase(VehicleType.Tractor)]
        [TestCase(VehicleType.Emergency)]
        [TestCase(VehicleType.Diplomat)]
        [TestCase(VehicleType.Foreign)]
        [TestCase(VehicleType.Military)]
        public void When_Calculate_TollFreeVehicle_RetursZero(VehicleType vehicleType)
        {
            var sut = CreateSut();
            var vehicle = A.Fake<IVehicle>();
            A.CallTo(() => vehicle.VehicleType)
                .Returns(vehicleType.ToString());

            var actual = sut.Calculate(vehicle, A.Dummy<PassBy>());

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

            var actual = sut.Calculate(vehicle, A.Dummy<PassBy>());

            Assert.AreNotEqual(Money.Zero(CurrencyCode.SEK), actual);
        }

        
        private TollFeeCalculator CreateSut()
        {
            var feeRuleRepo = A.Fake<IFeeRuleRepository>();
            var truthySpec = A.Fake<IPassFeeRuleSpecification>();
            A.CallTo(() => truthySpec.IsSatisfied(A<PassBy>._))
                .Returns(true);
            A.CallTo(() => truthySpec.Fee)
                .Returns(Money.CreateSek(60));

            A.CallTo(() => feeRuleRepo.GetAll())
                .Returns(new[] { truthySpec });


            var freeVehicleTypes = new List<string>
            {
                VehicleType.Tractor.ToString(),
                VehicleType.Motorbike.ToString(),
                VehicleType.Tractor.ToString(),
                VehicleType.Emergency.ToString(),
                VehicleType.Diplomat.ToString(),
                VehicleType.Foreign.ToString(),
                VehicleType.Military.ToString(),
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
