using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    public sealed class FlatRateCalculator : IFlatRateCalculator
    {
        private readonly ICalculatorSettingsService _calculatorSettingsService;
        public FlatRateCalculator(ICalculatorSettingsService calculatorSettingsService)
        {
            _calculatorSettingsService = calculatorSettingsService;
        }
        public Task<CalculateResult> CalculateAsync(decimal income)
        {
            decimal tax = 0m;
            decimal flatrate = 0.175m;

            tax = income * flatrate;


            return Task.FromResult(new CalculateResult()
            {
                Calculator = CalculatorType.FlatValue,
                Tax = tax,
            });


        }
    }
}