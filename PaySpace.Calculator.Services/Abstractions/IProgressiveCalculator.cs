using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IProgressiveCalculator
    {
        Task<CalculateResult> CalculateAsync(decimal income);

    }
}