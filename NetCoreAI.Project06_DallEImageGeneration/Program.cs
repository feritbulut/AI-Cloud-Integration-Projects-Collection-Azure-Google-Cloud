using System.Net.Http.Headers;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = "API_KEY";
        var endpoint = "https://openaiapifor.openai.azure.com/openai/deployments/dall-e-3/images/generations?api-version=2024-02-01";
        var prompt = "a fantasy landscape with floating islands and waterfalls";

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

        var requestBody = new
        {
            prompt = prompt,
            n = 1,
            size = "1024x1024"
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await httpClient.PostAsync(endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseString);
    }
}
