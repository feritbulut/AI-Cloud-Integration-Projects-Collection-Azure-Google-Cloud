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

        var client = new AzureOpenAIClient(endpoint, new AzureKeyCredential(apiKey));
        var chatClient = client.GetChatClient(deploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage("You are a helpful assistant."),
        };

        Console.WriteLine("Lütfen bir soru yazın:");
        var userInput = Console.ReadLine();
        messages.Add(new UserChatMessage(userInput));

        var response = chatClient.CompleteChat(messages);

        Console.WriteLine("\nYanıt:");
        Console.WriteLine(response.Value.Content[0].Text);
    }
}
