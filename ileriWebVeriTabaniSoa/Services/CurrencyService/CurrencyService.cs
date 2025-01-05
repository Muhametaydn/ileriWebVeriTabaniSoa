using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ileriWebVeriTabaniSoa.Services.CurrencyService

{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetDollarRateAsync()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:4000/dollar");
            var rateData = JsonConvert.DeserializeObject<RateResponse>(response);
            return rateData?.Rate ?? 0;
        }

        public class RateResponse
        {
            public decimal Rate { get; set; }
        }
    }
}
