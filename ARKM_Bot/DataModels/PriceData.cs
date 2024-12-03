using System.Text.Json.Serialization;

namespace ARKM_Bot.DataModels
{
    public class PriceData
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}