using Core;

using Microsoft.Extensions.Options;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Business
{
    public class ShamCashService: IShamCashService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ShamCashService(HttpClient httpClient, IOptions<PaymentOptions> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.Setting!;        
            _httpClient.BaseAddress = new Uri(_apiKey);
        }            



        public async Task<Payment> InitiatePaymentAsync(Payment request)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var jsonContent = new StringContent(JsonSerializer.Serialize(request),Encoding.UTF8,"application/json");
            var jsonContent2 = JsonContent.Create(request);

            var response = await _httpClient.PostAsync("transactions/init", jsonContent);
            var response2 = await _httpClient.PostAsync("v1/transactions", jsonContent2);

            response.EnsureSuccessStatusCode();
            response2.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            await response2.Content.ReadFromJsonAsync<Payment>();

            var paymentResponse = await JsonSerializer.DeserializeAsync<Payment>(responseStream);

            return paymentResponse!;
        }
    }

}
