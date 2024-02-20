using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    public sealed class FlatValueCalculator : IFlatValueCalculator
    {
        private readonly ICalculatorSettingsService _calculatorSettingsService;
        public FlatValueCalculator(ICalculatorSettingsService calculatorSettingsService)
        {
            _calculatorSettingsService = calculatorSettingsService;
        }
        public Task<CalculateResult> CalculateAsync(decimal income)
        {
           
            decimal flatValue = 10000m;
            decimal rateValue = 0.05m;

            decimal tax = flatValue;
            if (income < 200000)
            {
                tax = income * rateValue;
            }
          
            return Task.FromResult(new CalculateResult()
            {
                Calculator = CalculatorType.FlatValue,
                Tax = tax,
            });
        }
    }
}