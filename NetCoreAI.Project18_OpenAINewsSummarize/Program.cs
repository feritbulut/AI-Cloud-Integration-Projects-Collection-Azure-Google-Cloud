using System;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

class Program
{
    private const string endpoint = "https://openaiapifor.openai.azure.com/";
    private const string apiKey = "API_KEY";
    private const string deploymentId = "gpt-4"; // GPT-3.5 Turbo

    static async Task Main(string[] args)
    {
        string rssUrl = "https://www.trthaber.com/manset_articles.rss"; // İstediğin haber sitesinin RSS adresi
        var feed = await LoadRssFeed(rssUrl);

        foreach (var item in feed.Items)
        {
            Console.WriteLine($"📰 Başlık: {item.Title.Text}");

            var summary = await GetOpenAISummary(item.Summary?.Text ?? item.Title.Text);
            Console.WriteLine($"🧠 Özet: {summary}\n");
            await Task.Delay(2000); // API rate limit'e girmemek için bekleme
        }
    }

    static async Task<SyndicationFeed> LoadRssFeed(string rssUrl)
    {
        using var reader = XmlReader.Create(rssUrl);
        return SyndicationFeed.Load(reader);
    }

    static async Task<string> GetOpenAISummary(string content)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(endpoint);
        httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

        var prompt = $"Aşağıdaki haber içeriğini kısa ve öz bir şekilde özetle:\n\n{content.Substring(0, Math.Min(content.Length, 1000))}";

        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = "Sen bir haber editörüsün ve haberleri kısa özetlersin." },
                new { role = "user", content = prompt }
            },
            temperature = 0.5,
            max_tokens = 150
        };

        var requestJson = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        while (true)
        {
            var response = await httpClient.PostAsync($"/openai/deployments/{deploymentId}/chat/completions?api-version=2023-03-15-preview", requestJson);

            if ((int)response.StatusCode == 429)
            {
                Console.WriteLine("429 hatası alındı. 60 saniye bekleniyor...");
                await Task.Delay(60000);
                continue;
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseContent);
            return jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}
