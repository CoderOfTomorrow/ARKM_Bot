using ARKM_Bot.DataModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ARKM_Bot

{
    public class Program
    {
        private static async Task Main()
        {
            var botConfiguration = ConfigurationManager.GetBotConfiguration();
            var tradingConfiguration = ConfigurationManager.GetTradingConfiguration();
            var httpClientHelper = new HttpClientHelper();
            var random = new Random();
            UserVolume volume;

            Console.WriteLine("Trading Bot for Arkham by CoderOfTomorrow \n");

            try
            {
                do
                {
                    await Buy(httpClientHelper);
                    await Sell(httpClientHelper, tradingConfiguration.Symbol);

                    var result = await httpClientHelper.SendRequest(RequestsFactory.GetUserVolume());
                    volume = JsonSerializer.Deserialize<UserVolume>(await result.Content.ReadAsStringAsync());
                    var delay = random.Next(botConfiguration.MinDelay, botConfiguration.MaxDelay);
                    Console.WriteLine("Traded volume : " + volume.SpotVolume + ", next trade in : " + delay + " seconds.");

                    await Task.Delay(1000 * delay);
                } while (Convert.ToDecimal(volume.SpotVolume) < botConfiguration.MaxVolume);

                Console.WriteLine("\n Maximum volume was traded. The Bot will stop working.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        private static async Task Buy(HttpClientHelper helper)
        {
            while (true)
            {
                var assetsResponse = await helper.SendRequest(RequestsFactory.GetAssets());
                await CheckResponse(assetsResponse);

                var balance = GetBalance(await assetsResponse.Content.ReadAsStringAsync(), "USDT");

                if (balance < 5)
                    break;

                var orderbookResponse = await helper.SendRequest(RequestsFactory.GetOrderbook());
                await CheckResponse(orderbookResponse);

                var buyPrice = JsonSerializer.Deserialize<OrderBook>(await orderbookResponse.Content.ReadAsStringAsync()).Asks.Last().Price;
                var size = balance / Convert.ToDecimal(buyPrice) * 0.99M;

                var orderResponse = await helper.SendRequest(RequestsFactory.CreateNewOrder(buyPrice, Math.Round(size, 4).ToString(), "buy"));
                await CheckResponse(orderResponse);

                await Task.Delay(1000);

                var cancellResponse = await helper.SendRequest(RequestsFactory.CancellAllOrders());
                await CheckResponse(cancellResponse);
            }
        }

        private static async Task Sell(HttpClientHelper helper, string symbol)
        {
            while (true)
            {
                var assetsResponse = await helper.SendRequest(RequestsFactory.GetAssets());
                await CheckResponse(assetsResponse);

                var balance = GetBalance(await assetsResponse.Content.ReadAsStringAsync(), symbol);

                var orderbookResponse = await helper.SendRequest(RequestsFactory.GetOrderbook());
                await CheckResponse(orderbookResponse);

                var sellPrice = JsonSerializer.Deserialize<OrderBook>(await orderbookResponse.Content.ReadAsStringAsync()).Bids.Last().Price;

                if (balance * Convert.ToDecimal(sellPrice) < 5)
                    break;

                var orderResponse = await helper.SendRequest(RequestsFactory.CreateNewOrder(sellPrice, balance.ToString(), "sell"));
                await CheckResponse(orderResponse);

                await Task.Delay(1000);

                var cancellResponse = await helper.SendRequest(RequestsFactory.CancellAllOrders());
                await CheckResponse(cancellResponse);
            }
        }

        private static decimal GetBalance(string json, string symbol)
        {
            JsonNode jsonArray = JsonNode.Parse(json);

            if (jsonArray is JsonArray array)
            {
                var node = array.FirstOrDefault(node => node?["symbol"]?.ToString() == symbol);
                var balance = node["balance"].ToString();
                return Convert.ToDecimal(balance);
            }

            throw new Exception();
        }

        private static async Task CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                return;
            }
        }
    }
}