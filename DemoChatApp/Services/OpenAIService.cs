using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using DemoChatApp.Options;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoChatApp.Services
{
    internal class OpenAIService : IOpenAIService
    {
        private readonly OpenAIOptions _openAIOptions;

        public OpenAIService(IOptions<OpenAIOptions> options)
        {
            _openAIOptions = options.Value;
        }


        public async Task<string> Chat(string userMessage, ChatModelSettings settings, List<Models.ChatMessage> chatHistory = null)
        {
            List<OpenAI.Chat.ChatMessage> chatMessages = new List<OpenAI.Chat.ChatMessage>();

            if (chatHistory.Any())
            {
                chatHistory.ForEach(message =>
                {
                    switch (message.Sender)
                    {
                        case SenderRoles.User:
                            chatMessages.Add(new UserChatMessage(message.Message));
                            break;

                        case SenderRoles.Assistant:
                            chatMessages.Add(new AssistantChatMessage(message.Message));
                            break;
                    };

                });
            }

            chatMessages.Add(new UserChatMessage(userMessage));
            ChatClient client = new(model: settings.SelectedModel.ToString(), apiKey: _openAIOptions.ApiKey);

            ChatCompletionOptions options = new ChatCompletionOptions()
            {
                TopP = settings.Parameters.TopP,
                Temperature = settings.Parameters.Temperature,
                MaxOutputTokenCount = settings.Parameters.MaxTokens,

            };

            var response = await client.CompleteChatAsync(chatMessages, options);
            return response.Value.Content.FirstOrDefault().Text;
        }

        public async Task<string> GenerateChatTitle(string userMessage)
        {
            var prompt = "Based on the following user message, generate a short and meaningful chat title:\n\n" +
                            $"User: {userMessage}\n\n" +
                            "Title:";

            List<OpenAI.Chat.ChatMessage> chatMessages = new List<OpenAI.Chat.ChatMessage>();
            chatMessages.Add(new UserChatMessage(prompt));
            ChatClient client = new(model: "gpt-4", apiKey: _openAIOptions.ApiKey);
            var response = await client.CompleteChatAsync(chatMessages);
            return response.Value.Content.FirstOrDefault().Text;
        }
    }
}
