﻿using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal  class FlatRateCalculator : IFlatRateCalculator
    {
        public Task<CalculateResult> CalculateAsync(decimal income)
        {
            throw new NotImplementedException();
        }
    }
}