namespace ARKM_Bot.Models
{
    public class HttpClientConfiguration
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string BaseUrl { get; set; }
        public int Expiration { get; set; }
    }
}