using Assesment_one.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Assesment_one.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentApiController : ControllerBase
    {
        private const string ApiKey = "796b8b9dbbf46b1d8fd73f68979ae31635da9afabc9dee147adf0440ee7118a8";
        private const string ApiUrl = "https://bkashtest.shabox.mobi/home/MultiTournamentInBuildCheckoutUrl";

        [HttpGet("unique-string")]
        public IActionResult GetUniqueString()
        {
            string uniqueString = Guid.NewGuid().ToString();

            return Ok(uniqueString);
        }

        [HttpGet("random-subscription")]
        public string GenerateRandomSubscriptionJson()
        {
            var random = new Random();

            var subscription = new SubscriptionModel
            {
                Amount = random.Next(100, 1000).ToString(),
                FirstPaymentIncludedInCycle = random.Next(0, 2).ToString(),
                ServiceId = Guid.NewGuid().ToString(),
                Currency = "BDT",
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ExpiryDate = DateTime.Now.AddMonths(random.Next(1, 24)).ToString("dd/MM/yyyy"),
                Frequency = new[] { "Monthly", "Quarterly", "Annual" }[random.Next(0, 3)],
                SubscriptionType = new[] { "Basic", "Premium", "Enterprise" }[random.Next(0, 3)],
                MaxCapRequired = random.Next(0, 2).ToString(),
                MerchantShortCode = $"MSC{random.Next(1000, 9999)}",
                PayerType = new[] { "Individual", "Corporate" }[random.Next(0, 2)],
                PaymentType = new[] { "CreditCard", "DebitCard", "NetBanking" }[random.Next(0, 3)],
                RedirectUrl = $"https://example.com/{Guid.NewGuid()}",
                SubscriptionRequestId = Guid.NewGuid().ToString(),
                SubscriptionReference = $"SUB-{random.Next(10000, 99999)}",
                CKey = Guid.NewGuid().ToString()
            };

            return JsonSerializer.Serialize(subscription, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpPost("generate-checkout")]
        public async Task<IActionResult> GenerateCheckoutUrls()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", ApiKey);

            
            var weeklyRequest = CreateSubscriptionRequest("WEEKLY");
            var weeklyResponse = await PostSubscriptionRequest(httpClient, weeklyRequest);

        
            var monthlyRequest = CreateSubscriptionRequest("MONTHLY");
            var monthlyResponse = await PostSubscriptionRequest(httpClient, monthlyRequest);

            return Ok(new
            {
                Weekly = new { Url = weeklyResponse, SubscriptionRequestId = weeklyRequest.SubscriptionRequestId },
                Monthly = new { Url = monthlyResponse, SubscriptionRequestId = monthlyRequest.SubscriptionRequestId }
            });
        }

        private SubscriptionRequest CreateSubscriptionRequest(string frequency)
        {
            var uniqueId = Guid.NewGuid().ToString();
            return new SubscriptionRequest
            {
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ExpiryDate = DateTime.Now.AddMonths(frequency == "WEEKLY" ? 1 : 3).ToString("dd/MM/yyyy"),
                Frequency = frequency,
                RedirectUrl = "https://example.com",
                SubscriptionRequestId = uniqueId,
                SubscriptionReference = "01712345678"
            };
        }

        private async Task<string> PostSubscriptionRequest(HttpClient httpClient, SubscriptionRequest request)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(ApiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
