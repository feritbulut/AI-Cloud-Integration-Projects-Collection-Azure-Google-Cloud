using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreAI.Project20_RecipeSuggestionWithOpenAI.Models
{
    public class OpenAiService
    {
        private readonly Uri _endpoint = new Uri("https://openaiapifor.openai.azure.com/");
        private readonly string _deploymentName = "gpt-4";
        private readonly string _apiKey = "API_KEY";

        private readonly AzureOpenAIClient _client;
        private readonly ChatClient _chatClient;

        public OpenAiService()
        {
            _client = new AzureOpenAIClient(_endpoint, new AzureKeyCredential(_apiKey));
            _chatClient = _client.GetChatClient(_deploymentName);
        }

        public async Task<string> GetRecipeSuggestionAsync(string ingredients)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("Sen bir aşçısın. Kullanıcının elindeki malzemelere göre yemek tarifi önerirsin."),
                new UserChatMessage($"Elimde şu malzemeler var: {ingredients}. Ne yapabilirim?")
            };

            var response = await _chatClient.CompleteChatAsync(messages);
            return response.Value.Content[0].Text;
        }
    }
}