using ARKM_Bot.DataModels;
using ARKM_Bot.Models;
using System.Text.Json;

namespace ARKM_Bot
{
    public static class RequestsFactory
    {
        public static readonly List<Endpoint> endpoints = ConfigurationManager.GetEndpoints();
        public static readonly TradingConfiguration tradingConfiguration = ConfigurationManager.GetTradingConfiguration();

        public static PostRequest CancellAllOrders()
            => new()
            {
                Endpoint = endpoints.FirstOrDefault(x => x.Name == "CancellAllOrders").Path
            };

        public static PostRequest CreateNewOrder(string price, string size, string side)
        {
            var order = Order.CreateLimit(price, side, size, tradingConfiguration.Pair);

            return new()
            {
                Endpoint = endpoints.FirstOrDefault(x => x.Name == "CreateNewOrder").Path,
                Body = JsonSerializer.Serialize(order)
            };
        }

        public static GetRequest GetOrderbook()
        {
            var symbol = tradingConfiguration.Pair;

            return new()
            {
                Endpoint = endpoints.FirstOrDefault(x => x.Name == "GetOrderbook").Path + Uri.EscapeDataString(symbol)
            };
        }

        public static GetRequest GetAssets()
        {
            return new()
            {
                Endpoint = endpoints.FirstOrDefault(x => x.Name == "GetAssets").Path
            };
        }
    }
}