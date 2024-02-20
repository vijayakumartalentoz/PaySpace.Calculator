using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;



namespace PaySpace.Calculator.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public sealed class CalculatorController(
        ILogger<CalculatorController> logger,
        IHistoryService historyService,
        IMapper mapper, IPostalCodeService postalCodeService, ICalculatorSettingsService calculatorSettingsService)
        : ControllerBase
    {
        [HttpPost("calculate-tax")]
        public async Task<ActionResult<CalculateResult>> Calculate(CalculateRequest request)
        {
            try
            {
                var actionMapping = new Dictionary<CalculatorType, dynamic>
                {
                    {CalculatorType.Progressive,   new PaySpace.Calculator.Services.Calculators.ProgressiveCalculator(calculatorSettingsService) },
                    {CalculatorType.FlatValue,   new PaySpace.Calculator.Services.Calculators.FlatValueCalculator(calculatorSettingsService) },
                    {CalculatorType.FlatRate,   new PaySpace.Calculator.Services.Calculators.FlatRateCalculator(calculatorSettingsService)   }
                 };
                var resultcalculatorType = await postalCodeService.CalculatorTypeAsync(request.PostalCode);
                var resultCalculator = new CalculateResult();
                if (actionMapping.TryGetValue(resultcalculatorType.Value, out var resultcalculator))
                {
                    resultCalculator = resultcalculator.CalculateAsync(request.Income).Result;
                }

                await historyService.AddAsync(new CalculatorHistory
                {
                    Tax = resultCalculator.Tax,
                    Calculator = resultCalculator.Calculator,
                    PostalCode = request.PostalCode ?? "Unknown",
                    Income = request.Income
                });

                return this.Ok(mapper.Map<CalculateResultDto>(resultCalculator));
            }
            catch (CalculatorException e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<CalculatorHistory>>> History()
        {
            var history = await historyService.GetHistoryAsync();

            return this.Ok(mapper.Map<List<CalculatorHistoryDto>>(history));
        }

        [HttpGet("postalcode")]
        public async Task<ActionResult<List<PostalCode>>>  PostalCodes()
        {
            var postalCodes = await postalCodeService.GetPostalCodesAsync();

            return this.Ok(mapper.Map<List<PostalCodeDto>>(postalCodes));
        }
    }
}