using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using System.Threading.Tasks;

class Program
{
    // Azure OpenAI bilgileri
    private const string endpoint = "https://openaiapifor.openai.azure.com/";
    private const string apiKey = "API_KEY";
    private const string deploymentId = "gpt-4"; // örneğin: gpt-35-turbo

    static async Task Main(string[] args)
    {
        Console.Write("Web sitesinin URL'sini girin: ");
        string url = Console.ReadLine();

        // Web sayfası içeriğini al
        string pageContent = await ScrapeWebsiteContent(url);
        Console.WriteLine("\n--- Web İçeriği Başarıyla Alındı ---\n");

        // OpenAI ile özetle
        string summary = await GetOpenAISummary(pageContent);

        Console.WriteLine("\n--- Özet ---\n");
        Console.WriteLine(summary);
    }

    static async Task<string> ScrapeWebsiteContent(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        var sb = new StringBuilder();

        var nodes = doc.DocumentNode.SelectNodes("//p");
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                sb.AppendLine(node.InnerText.Trim());
            }
        }

        return sb.ToString();
    }

    static async Task<string> GetOpenAISummary(string content)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(endpoint);
        httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

        var prompt = $"Aşağıdaki metni özetle:\n\n{content}";

        var requestBody = new
        {
            messages = new[]
            {
            new { role = "system", content = "Sen deneyimli bir metin özetleyicisin." },
            new { role = "user", content = prompt }
        },
            temperature = 0.7,
            max_tokens = 300,
        };

        var requestJson = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        while (true)
        {
            var response = await httpClient.PostAsync($"/openai/deployments/{deploymentId}/chat/completions?api-version=2023-03-15-preview", requestJson);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                Console.WriteLine("429 hatası alındı. 60 saniye bekleniyor...");
                await Task.Delay(60000); // 60 saniye bekle
                continue;
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseContent);
            return jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }

}
