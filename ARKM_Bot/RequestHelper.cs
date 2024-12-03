using System.Security.Cryptography;
using System.Text;

namespace ARKM_Bot
{
    public static class RequestHelper
    {
        private static readonly string apiKey = "";
        private static readonly string apiSecret = "";
        private static readonly string baseUrl = "https://arkm.com/api";

        public static HttpRequestMessage CreateRequest(string body, string requestPath, string method)
        {
            // Set expiry timestamp (5 minutes from now)
            long expires = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 300) * 1_000_000;

            // Generate the HMAC key from the API secret
            byte[] hmacKey = Convert.FromBase64String(apiSecret);

            // Construct the prehash string
            string prehash = $"{apiKey}{expires}{method}{requestPath}{body}";

            // Calculate the HMAC SHA-256 signature
            string signature;
            using (var hmac = new HMACSHA256(hmacKey))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(prehash));
                signature = Convert.ToBase64String(hash);
            }

            // Create the HttpClient
            using HttpClient client = new();
            client.BaseAddress = new Uri(baseUrl);

            // Create the request content
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var httpMethod = method.Equals("POST") ? HttpMethod.Post : HttpMethod.Get;
            // Create the request
            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri($"{baseUrl}{requestPath}"),
                Content = content
            };

            // Add headers
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Arkham-Api-Key", apiKey);
            request.Headers.Add("Arkham-Expires", expires.ToString());
            request.Headers.Add("Arkham-Signature", signature);

            return request;
        }
    }
}