using System.Text.Json;


namespace ForexAlert;

public static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine("Forex Alert!");

        var currentPrice = await GetPriceApiAsync("usd", "vnd");
        Console.WriteLine(currentPrice);
    }

    private static string GetInput()
    {
        var input = Console.ReadLine();
        while (input == null)
        {
            Console.WriteLine("Please try again!");
            input = Console.ReadLine();
        }

        return input;
    }

    private static async Task<string> GetPriceApiAsync(string currencyFrom, string currencyTo)
    {
        var date = "latest";
        var apiVersion = "v1";
        var endpoint = $"currencies/{currencyFrom}";
        var url = $"https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@{date}/{apiVersion}/{endpoint}.json";

        using var client = new HttpClient();

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            JsonDocument jsonDocument = JsonDocument.Parse(content);
            JsonElement root = jsonDocument.RootElement;

            root.TryGetProperty(currencyFrom, out JsonElement currencyFromElement);
            currencyFromElement.TryGetProperty(currencyTo, out JsonElement currencyToElement);

            return currencyToElement.ToString();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return null;
        }
    }
}