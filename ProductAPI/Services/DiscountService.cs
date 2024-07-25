using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;
using ProductAPI.Interfaces.Services;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
using Serilog;
using System.Text.Json;

namespace ProductAPI.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient, IOptions<DiscountServiceSettings> settings)
        {
            _httpClient = httpClient;
            if (!string.IsNullOrEmpty(settings.Value.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(settings.Value.BaseUrl);
            }
        }
        public async Task<double> GetDiscount(int productId)
        {
            try
            {
                string url = $"/discounts/{productId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<DiscountServiceResponse>(body);
                    return result?.discount ?? 0;
                }
            }
            catch (BrokenCircuitException)
            { 
                Log.Error("The discount service is temporarily unavailable");  
            }

            return 0;
        }
    }
}
