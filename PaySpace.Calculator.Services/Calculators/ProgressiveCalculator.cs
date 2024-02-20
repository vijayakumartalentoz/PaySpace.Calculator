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
            // Iterate through tax brackets and calculate tax
            var calcualtionSettings = _calculatorSettingsService.GetSettingsAsync(CalculatorType.Progressive).Result;
            foreach ( var bracket  in calcualtionSettings)
            {

               
                decimal previousbracketMaxValue = 0;
               
                if (taxableIncome <= 0)
                    break;

                decimal bracketTaxableAmount = Math.Min((decimal)bracket.To, taxableIncome) - previousbracketMaxValue;
                if (bracketTaxableAmount <= 0)
                    break;

                tax += bracketTaxableAmount * (bracket.Rate/100);
                taxableIncome -= bracketTaxableAmount;
                previousbracketMaxValue = Convert.ToDecimal(bracket.To);
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