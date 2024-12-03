using System.Text.Json.Serialization;

namespace ARKM_Bot.Models
{
    public class NewOrderRequest
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
        public string Type { get; set; }
    }
}