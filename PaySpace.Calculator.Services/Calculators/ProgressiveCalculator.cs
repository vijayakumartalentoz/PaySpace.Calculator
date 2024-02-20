using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;
using System.Collections.Generic;

namespace PaySpace.Calculator.Services.Calculators
{
    public sealed class ProgressiveCalculator : IProgressiveCalculator
    {
        private readonly ICalculatorSettingsService _calculatorSettingsService;
        public ProgressiveCalculator(ICalculatorSettingsService calculatorSettingsService)
        {
            _calculatorSettingsService = calculatorSettingsService;
        }
            public Task<CalculateResult> CalculateAsync(decimal income)
        {
     

           
            decimal tax = 0m;
            decimal taxableIncome = income;
            decimal previousbracketMaxValue = 0;
            // Iterate through tax brackets and calculate tax
            var calcualtionSettings = _calculatorSettingsService.GetSettingsAsync(CalculatorType.Progressive).Result;
            foreach ( var bracket  in calcualtionSettings)
            {


                if (income <= bracket.From)
                {
                    break;
                }
               // bracket.To = bracket.To + 1;
                decimal incomeInSlab = bracket.To.HasValue ? Math.Min(income, bracket.To.Value) - bracket.From : income - bracket.From;
                tax += Convert.ToDecimal(incomeInSlab * (bracket.Rate / 100));


           
            }

            return Task.FromResult(new CalculateResult()
            {
                Calculator = CalculatorType.Progressive,
                Tax = tax,
            });
            ;
        }
    }
}