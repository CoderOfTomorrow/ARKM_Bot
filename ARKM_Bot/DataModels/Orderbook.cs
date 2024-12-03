using System.Text.Json.Serialization;

namespace ARKM_Bot.DataModels
{
    public class OrderBook
    {
        [JsonPropertyName("asks")]
        public List<PriceData> Asks { get; set; }

        [JsonPropertyName("bids")]
        public List<PriceData> Bids { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("lastTime")]
        public float LastTime { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}