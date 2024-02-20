using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class ProgressiveCalculatorTests
    {
        private PaySpace.Calculator.Web.Services.CalculatorHttpService httpService;
        private IConfiguration _configuration;


        [SetUp]
        public void Setup()
        {
            try
            {
                _configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();
            }
            catch (Exception)
            {
                _configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("CalculatorSettings:ApiUrl", "https://localhost:7119")
                    })
                    .Build();
            }
            httpService = new Web.Services.CalculatorHttpService(_configuration);

        }

       [TestCase(-1, 0)]
        [TestCase(50, 5)]
        [TestCase(8350.1, 835.01)]
        [TestCase(8351, 835)]
        [TestCase(33951, 4674.85)]
        [TestCase(82251, 16749.60)]
        [TestCase(171550, 41753.32)]
        [TestCase(999999, 327681.79)]

        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {

            // Arrange
            Web.Services.Models.CalculateRequest request = new Web.Services.Models.CalculateRequest()
            {
                Income = income,
                PostalCode = "7441"
            };

            // Act
            Web.Services.Models.CalculateResult result = await httpService.CalculateTaxAsync(request);

            // Assert
            Assert.AreEqual(Convert.ToDouble(expectedTax), Convert.ToDouble(result.Tax), 1);
        
        }
    }
}