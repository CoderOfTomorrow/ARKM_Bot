using System.Text.Json;

namespace ARKM_Bot
{
    public static class GetHttpRequests
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BodyContent.json");

        public static HttpRequestMessage GetBook()
        {
            string jsonContent = File.ReadAllText(filePath);
            string symbol;

            using (var jsonDoc = JsonDocument.Parse(jsonContent))
            {
                symbol = jsonDoc.RootElement.GetProperty("GetBook").ToString();
            }

            string requestPath = $"/public/book?symbol={Uri.EscapeDataString(symbol)}";

            return CreateRequest(requestPath);
        }

        public static HttpRequestMessage GetBalance()
            => CreateRequest("/account/balances");

        public static HttpRequestMessage GetSymbolBalance()
            => CreateRequest("/account/balances");

        private static HttpRequestMessage CreateRequest(string requestPath)
            => RequestHelper.CreateRequest(string.Empty, requestPath, "GET");
    }
}