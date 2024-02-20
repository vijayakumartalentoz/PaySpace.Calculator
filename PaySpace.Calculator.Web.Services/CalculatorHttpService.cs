using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services
{
    public class CalculatorHttpService : ICalculatorHttpService
    {

        private readonly IConfiguration _configuration;

        public CalculatorHttpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       

        public async Task<List<PostalCode>> GetPostalCodesAsync()
        {
            string apiUrl = _configuration["CalculatorSettings:ApiUrl"].ToString();

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl + "/" + "api/Calculator/postalcode");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<PostalCode>>() ?? [];
        }

        public async Task<List<CalculatorHistory>> GetHistoryAsync()
        {
            string apiUrl = _configuration["CalculatorSettings:ApiUrl"].ToString();
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl + "/" + "api/Calculator/history");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch Calcualtor History, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<CalculatorHistory>>() ?? [];
        }

        public async Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest)
        {
            string apiUrl = _configuration["CalculatorSettings:ApiUrl"].ToString();
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl + "/" + "api/Calculator/calcuate-tax");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch tax, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<CalculateResult>();
        }
    }
}