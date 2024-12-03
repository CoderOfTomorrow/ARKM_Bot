using ARKM_Bot.Models;
using System.Security.Cryptography;
using System.Text;

namespace ARKM_Bot
{
    public class HttpClientHelper
    {
        private readonly HttpClient httpClient;
        private readonly HttpClientConfiguration httpClientConfiguration;

        public HttpClientHelper()
        {
            this.httpClient = new();
            this.httpClientConfiguration = ConfigurationManager.GetHttpConfiguration();
            httpClient.BaseAddress = new Uri(httpClientConfiguration.BaseUrl);
        }

        public async Task<HttpResponseMessage> SendRequest(BaseRequest requestData)
        {
            // Set expiry timestamp (5 minutes from now)
            long expires = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + httpClientConfiguration.Expiration) * 1_000_000;

            // Generate the HMAC key from the API secret
            byte[] hmacKey = Convert.FromBase64String(httpClientConfiguration.ApiSecret);

            // Construct the prehash string
            string prehash = $"{httpClientConfiguration.ApiKey}{expires}{requestData.Method}{requestData.Endpoint}{requestData.Body}";

            // Calculate the HMAC SHA-256 signature
            string signature;
            using (var hmac = new HMACSHA256(hmacKey))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(prehash));
                signature = Convert.ToBase64String(hash);
            }

            // Create the HttpClient
            using HttpClient client = new();
            client.BaseAddress = new Uri(httpClientConfiguration.BaseUrl);

            // Create the request content
            var content = new StringContent(requestData.Body, Encoding.UTF8, "application/json");

            // Create the request
            var request = new HttpRequestMessage
            {
                Method = requestData.HttpMethod,
                RequestUri = new Uri($"{httpClientConfiguration.BaseUrl}{requestData.Endpoint}"),
                Content = content
            };

            // Add headers
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Arkham-Api-Key", httpClientConfiguration.ApiKey);
            request.Headers.Add("Arkham-Expires", expires.ToString());
            request.Headers.Add("Arkham-Signature", signature);

            return await this.httpClient.SendAsync(request);
        }
    }
}