using System.Text.Json.Serialization;

namespace ARKM_Bot.Models
{
    public class SymbolBook
    {
        [JsonPropertyName("asks")]
        public List<Book> Asks { get; set; }

        [JsonPropertyName("bids")]
        public List<Book> Bids { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("lastTime")]
        public float LastTime { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }

    public class Book
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}