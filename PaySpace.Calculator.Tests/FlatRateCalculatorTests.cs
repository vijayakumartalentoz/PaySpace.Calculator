using Moq;

using NUnit.Framework;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class FlatRateCalculatorTests
    {
        [SetUp]
        public void Setup()
        {            
        }
        private readonly ICalculatorSettingsService _calculatorSettingsService;
        [TestCase(999999, 174999.825)]
        [TestCase(1000, 175)]
        [TestCase(5, 0.875)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange



            // Act
            var result = new PaySpace.Calculator.Services.Calculators.FlatRateCalculator(_calculatorSettingsService);

            // Assert
            Assert.IsTrue(result.CalculateAsync(income).Result.Tax.Equals(expectedTax));
        }
    }
}
