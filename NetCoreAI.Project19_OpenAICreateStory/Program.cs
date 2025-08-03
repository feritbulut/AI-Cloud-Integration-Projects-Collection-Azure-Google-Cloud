using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    static void Main(string[] args)
    {
       
        var endpoint = new Uri("https://openaiapifor.openai.azure.com/");
        var deploymentName = "gpt-4";
        var apiKey = "API_KEY";
        
        // Kullanıcıdan hikaye detaylarını alma
        Console.WriteLine("--- Hikaye Oluşturucu ---");

        Console.Write("Hikaye türü (örn: bilim kurgu, fantastik, romantik): ");
        string genre = Console.ReadLine();

        Console.Write("Hikayenin geçtiği mekan (örn: Mars, Orta Dünya, İstanbul 2050): ");
        string location = Console.ReadLine();

        Console.Write("Hikaye uzunluğu (kısa/orta/uzun): ");
        string length = Console.ReadLine();

        Console.Write("Ana karakterin adı ve özellikleri: ");
        string mainCharacter = Console.ReadLine();

        // Azure OpenAI istemcisini oluştur
        var client = new AzureOpenAIClient(endpoint, new AzureKeyCredential(apiKey));
        var chatClient = client.GetChatClient(deploymentName);

        // Sistem mesajını ve kullanıcı prompt'unu hazırla
        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage("Sen bir yaratıcı hikaye yazarısın. Kullanıcının verdiği öğeleri kullanarak ilgi çekici hikayeler oluşturursun."),
            new UserChatMessage($"Şu öğeleri kullanarak bir hikaye yaz:\n" +
                              $"Tür: {genre}\n" +
                              $"Mekan: {location}\n" +
                              $"Uzunluk: {length}\n" +
                              $"Ana karakter: {mainCharacter}\n\n" +
                              $"Hikayeye uygun bir başlık da oluştur. Hikayeyi direkt metin olarak yaz, başka açıklama yapma.")
        };

        Console.WriteLine("\nHikaye oluşturuluyor, lütfen bekleyin...");

        try
        {
            // API'ye istek gönder
            var response = chatClient.CompleteChat(messages);

            // Sonucu göster
            Console.WriteLine("\n--- Oluşturulan Hikaye ---");
            Console.WriteLine(response.Value.Content[0].Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata oluştu: {ex.Message}");
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}