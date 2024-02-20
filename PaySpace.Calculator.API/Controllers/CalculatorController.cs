﻿using MapsterMapper;

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
        IMapper mapper,IPostalCodeService postalCodeService, ICalculatorSettingsService calculatorSettingsService )
        : ControllerBase
    {
        [HttpPost("calculate-tax")]
        public async Task<ActionResult<CalculateResult>> Calculate(CalculateRequest request)
        {
            try
            {
                var actMap = new Dictionary<CalculatorType, dynamic>
                {
                    {CalculatorType.Progressive,     }
                 };
                var result = await postalCodeService.CalculatorTypeAsync(request.PostalCode);

                decimal tax = 0;

                if(actMap.TryGetValue(result.Value, out var action))
                {
                    tax = action.Calculate;
                    

                }

                await historyService.AddAsync(new CalculatorHistory
                {
                    //Tax = result.Tax,
                   // Calculator = result.Calculator,
                    PostalCode = request.PostalCode ?? "Unknown",
                    Income = request.Income
                });

                return this.Ok(mapper.Map<CalculateResultDto>(result));
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
    }
}