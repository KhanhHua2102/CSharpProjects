namespace ForexAlert;

class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Forex Alert!");

        var currencyCode = "vnd";
        var date = DateTime.Today.ToString();
        var apiVersion = "v1";
        var endpoint = $"/currencies/{currencyCode}";
        var url = $"https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@{date}/{apiVersion}/{endpoint}.json";

    }

    // static void GetUserInput();

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

    public static async Task<string> GetPriceApiAsync(string url)
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return response.RequestMessage.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}