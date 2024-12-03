using ARKM_Bot.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ARKM_Bot

{
    public class Program
    {
        private static async Task Main()
        {
            //Parameters:
            int iterations = 20;
            Random random = new();
            int randomNumber = random.Next(30, 61);

            using HttpClient client = new();
            client.BaseAddress = new Uri("https://arkm.com/api");

            for (var i = 0; i < iterations; i++)
            {
                await Buy(client);
                await Sell(client);

                await Task.Delay(1000 * randomNumber);
            }
        }

        private static async Task Buy(HttpClient client)
        {
            while (true)
            {
                var buyBalance = await client.SendAsync(GetHttpRequests.GetBalance());
                await CheckResponse(buyBalance);
                var balance = GetBalance(await buyBalance.Content.ReadAsStringAsync());

                if (balance < 5)
                    break;

                var buyOrderbookResponse = await client.SendAsync(GetHttpRequests.GetBook());
                await CheckResponse(buyOrderbookResponse);

                var buyOrderbook = JsonSerializer.Deserialize<SymbolBook>(await buyOrderbookResponse.Content.ReadAsStringAsync()).Asks.Last().Price;
                var size = GetSize(buyOrderbook, balance);

                var buyOrder = await client.SendAsync(PostHttpRequests.CreateNewOrder(buyOrderbook, Math.Round(size, 4).ToString(), "buy"));
                await CheckResponse(buyOrder);

                await Task.Delay(1000);

                var cancellResponse = await client.SendAsync(PostHttpRequests.CancellAllOrders());
                await CheckResponse(cancellResponse);
            }
        }

        private static async Task Sell(HttpClient client)
        {
            while (true)
            {
                var sellBalance = await client.SendAsync(GetHttpRequests.GetBalance());
                await CheckResponse(sellBalance);
                var balance = GetSymbolBalance(await sellBalance.Content.ReadAsStringAsync());

                var sellOrderbookResponse = await client.SendAsync(GetHttpRequests.GetBook());
                await CheckResponse(sellOrderbookResponse);

                var sellOrderbook = JsonSerializer.Deserialize<SymbolBook>(await sellOrderbookResponse.Content.ReadAsStringAsync()).Bids.Last().Price;

                if (balance * Convert.ToDecimal(sellOrderbook) < 5)
                    break;

                var sellOrder = await client.SendAsync(PostHttpRequests.CreateNewOrder(sellOrderbook, balance.ToString(), "sell"));
                await CheckResponse(sellOrder);

                await Task.Delay(1000);

                var cancellResponse = await client.SendAsync(PostHttpRequests.CancellAllOrders());
                await CheckResponse(cancellResponse);
            }
        }

        private static decimal GetBalance(string json)
        {
            using var jsonDoc = JsonDocument.Parse(json);
            var stringdata = jsonDoc.RootElement[0].GetProperty("balanceUSDT").GetString();
            return Convert.ToDecimal(stringdata);
        }

        private static decimal GetSymbolBalance(string json)
        {
            JsonNode jsonArray = JsonNode.Parse(json);

            if (jsonArray is JsonArray array)
            {
                var ethNode = array.FirstOrDefault(node => node?["symbol"]?.ToString() == "ETH");
                var ethBalance = ethNode["balance"].ToString();
                return Convert.ToDecimal(ethBalance);
            }

            throw new Exception();
        }

        private static decimal GetSize(string price, decimal balance)
            => balance / Convert.ToDecimal(price) * 0.99M;

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