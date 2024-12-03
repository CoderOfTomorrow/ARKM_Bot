using ARKM_Bot.Models;
using System.Text.Json;

namespace ARKM_Bot
{
    public static class PostHttpRequests
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BodyContent.json");

        public static HttpRequestMessage CreateNewOrder(string price, string size, string side)
        {
            string jsonContent = File.ReadAllText(filePath);
            string requestPath = "/orders/new";
            string body;

            using (var jsonDoc = JsonDocument.Parse(jsonContent))
            {
                var jsonData = jsonDoc.RootElement.GetProperty("CreateNewOrder").ToString();
                var model = JsonSerializer.Deserialize<NewOrderRequest>(jsonData);

                model.Price = price;
                model.Size = size;
                model.Side = side;
                model.Symbol = jsonDoc.RootElement.GetProperty("GetBook").ToString();

                body = JsonSerializer.Serialize(model);
            }

            return CreateRequest(body, requestPath);
        }

        public static HttpRequestMessage CancellAllOrders()
            => CreateRequest("{}", "/orders/cancel/all");

        private static HttpRequestMessage CreateRequest(string body, string requestPath)
            => RequestHelper.CreateRequest(body, requestPath, "POST");
    }
}