using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    static void Main(string[] args)
    {
        var endpoint = new Uri("https://openaiapifor.openai.azure.com/"); // Azure portalında görülen uç nokta
        var deploymentName = "gpt-4"; // Azure'daki deployment adı (sen ne verdiysen o)
        var apiKey = "API_KEY"; // Azure portalından kopyaladığın API anahtarı

        var client = new AzureOpenAIClient(endpoint, new AzureKeyCredential(apiKey));
        var chatClient = client.GetChatClient(deploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage("You are a mood analyst who analyzes the emotions in a given text and classifies them as negative, positive and neutral."),
        };

        Console.WriteLine("Lütfen bir soru yazın:");
        var userInput = Console.ReadLine();
        messages.Add(new UserChatMessage(userInput));

        var response = chatClient.CompleteChat(messages);

        Console.WriteLine("\nDuygu durum analizi:");
        Console.WriteLine(response.Value.Content[0].Text);
    }
}
