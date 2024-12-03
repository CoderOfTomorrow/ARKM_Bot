using System.Text.Json.Serialization;

namespace ARKM_Bot.DataModels
{
    public class Order
    {
        [JsonPropertyName("clientOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;

        [JsonPropertyName("postOnly")]
        public bool PostOnly { get; set; } = false;

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("side")]
        public string Side { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("subaccountId")]
        public int SubaccountId { get; set; } = 0;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "limitGtc";

        public static Order CreateLimit(string price, string side, string size, string pair)
            => new()
            {
                Price = price,
                Side = side,
                Size = size,
                Symbol = pair,
                Type = "limitGtc"
            };

        public static Order CreateMarket(string side, string size, string pair)
            => new()
            {
                Price = "0",
                Side = side,
                Size = size,
                Symbol = pair,
                Type = "market"
            };
    }
}