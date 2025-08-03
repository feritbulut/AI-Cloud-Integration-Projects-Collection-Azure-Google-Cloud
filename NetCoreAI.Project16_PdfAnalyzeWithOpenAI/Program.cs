using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UglyToad.PdfPig;

class Program
{
    // Azure OpenAI bilgileri
    private const string endpoint = "https://openaiapifor.openai.azure.com/";
    private const string apiKey = "API_KEY";
    private const string deploymentId = "gpt-4"; // Örn: gpt-35-turbo

    static async Task Main(string[] args)
    {
        Console.Write("PDF dosya yolu girin: ");
        string pdfPath = Console.ReadLine();

        if (!File.Exists(pdfPath))
        {
            Console.WriteLine("Dosya bulunamadı.");
            return;
        }

        // 1. PDF içeriğini oku
        string content = ExtractTextFromPdf(pdfPath);
        Console.WriteLine("--- PDF Metni Alındı ---");

        // 2. OpenAI ile özetle
        string summary = await GetOpenAISummary(content);
        Console.WriteLine("\n--- PDF Özeti ---\n");
        Console.WriteLine(summary);
    }

    static string ExtractTextFromPdf(string filePath)
    {
        var sb = new StringBuilder();
        using (PdfDocument document = PdfDocument.Open(filePath))
        {
            foreach (var page in document.GetPages())
            {
                sb.AppendLine(page.Text);
            }
        }
        return sb.ToString();
    }

    static async Task<string> GetOpenAISummary(string content)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(endpoint);
        httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

        var prompt = $"Aşağıdaki metni özetle:\n\n{content.Substring(0, Math.Min(content.Length, 2000))}"; // ilk 2000 karakter

        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = "Sen profesyonel bir metin özetleyicisin." },
                new { role = "user", content = prompt }
            },
            temperature = 0.7,
            max_tokens = 300
        };

        var requestJson = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        while (true)
        {
            var response = await httpClient.PostAsync($"/openai/deployments/{deploymentId}/chat/completions?api-version=2023-03-15-preview", requestJson);

            if ((int)response.StatusCode == 429)
            {
                Console.WriteLine("429 hatası - 60 saniye bekleniyor...");
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
