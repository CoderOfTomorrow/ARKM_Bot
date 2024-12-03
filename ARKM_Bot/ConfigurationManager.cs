using ARKM_Bot.Models;
using System.Text.Json;

namespace ARKM_Bot
{
    public class ConfigurationManager
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ARKM_Bot.json");

        public static BotConfiguration GetBotConfiguration()
            => GetData<BotConfiguration>("BotConfiguration");

        public static HttpClientConfiguration GetHttpConfiguration()
            => GetData<HttpClientConfiguration>("HttpClientConfiguration");

        public static TradingConfiguration GetTradingConfiguration()
            => GetData<TradingConfiguration>("TradingConfiguration");

        public static List<Endpoint> GetEndpoints()
            => GetData<List<Endpoint>>("Endpoints");

        private static T GetData<T>(string section) where T : class
        {
            using var jsonDoc = JsonDocument.Parse(File.ReadAllText(filePath));
            var configurationData = jsonDoc.RootElement.GetProperty(section).ToString();
            return JsonSerializer.Deserialize<T>(configurationData);
        }
    }
}