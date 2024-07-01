using System.Text.Json;


namespace ForexAlert;

public static class Program
{
    private static async Task Main()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Forex Alert");

        var currencyFrom = GetInput("Please input first currency:");
        var currencyTo = GetInput("Please input second currency:");

        var currentPrice = await GetPriceApiAsync(currencyFrom, currencyTo);
        Console.WriteLine(Math.Round((decimal)currentPrice, 2));
    }

    private static string GetInput(string inputString)
    {
        Console.WriteLine(inputString);

        var input = Console.ReadLine();
        while (input == null)
        {
            Console.WriteLine("Please try again!");
            input = Console.ReadLine();
        }

        return input;
    }

    private static async Task<float?> GetPriceApiAsync(string currencyFrom, string currencyTo)
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

            float.TryParse(currencyToElement.ToString(), out float result);

            return result;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return null;
        }
    }
}
