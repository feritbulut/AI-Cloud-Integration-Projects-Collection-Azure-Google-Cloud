using System.Text;
using System.Text.Json;

class Program
{
    private static readonly string googleApiKey = "API_KEY";
    private static readonly string imagePAth= "C:\\Users\\Arif\\Desktop\\IMG_20250410_133841.jpg";

    static async Task Main()
    {
        Console.WriteLine("Google vision api ile gorsel nesne tespiti yapiliyor...");
        string response = await DetectObjects(imagePAth);

        Console.WriteLine("Gorsel nesne tespiti sonucu:");
        Console.WriteLine(response);
    }

    static async Task<string> DetectObjects(string path)
    {
        using var client = new HttpClient();
        
         string apiUrl = $"https://vision.googleapis.com/v1/images:annotate?key={googleApiKey}";


        byte[]imageBytes = File.ReadAllBytes(path);
        string base64Image = Convert.ToBase64String(imageBytes);

        var requestBody = new
        {
            requests = new[]
            {
                new
                {
                    image = new { content = base64Image },
                    features = new[]
                    {
                        new
                        {
                            type = "OBJECT_LOCALIZATION",
                            maxResults = 10
                        }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;


    }
}