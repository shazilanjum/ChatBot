using DemoChatApp.Contracts;
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
using static System.Windows.Forms.Design.AxImporter;

namespace DemoChatApp.Services
{
    internal class OpenAIService : IOpenAIService
    {
        private readonly OpenAIOptions _openAIOptions;

        public OpenAIService(IOptions<OpenAIOptions> options)
        {
            _openAIOptions = options.Value;
        }


        public async Task<string> Chat(List<Models.ChatMessage> chatHistory, ChatModelSettings settings = null)
        {
            List<OpenAI.Chat.ChatMessage> openaiChatMessages = new List<OpenAI.Chat.ChatMessage>();

            if (chatHistory.Any())
            {
                chatHistory.ForEach(message =>
                {
                    switch (message.Sender)
                    {
                        case SenderRoles.User:
                            openaiChatMessages.Add(new UserChatMessage(message.Message));
                            break;

                        case SenderRoles.Assistant:
                            openaiChatMessages.Add(new AssistantChatMessage(message.Message));
                            break;
                    };

                });
            }

            ChatClient client = new(model: settings == null ? OpenAIModels.OpenAIModelsMapping[ChatModels.gpt4o] : OpenAIModels.OpenAIModelsMapping[settings.SelectedModel], apiKey: _openAIOptions.ApiKey);

            if (settings != null)
            {
                ChatCompletionOptions options = new ChatCompletionOptions()
                {
                    TopP = settings.Parameters.TopP,
                    Temperature = settings.Parameters.Temperature,
                    MaxOutputTokenCount = settings.Parameters.MaxTokens,

                };

                var responseWithOptions = await client.CompleteChatAsync(openaiChatMessages, options);
                return responseWithOptions.Value.Content.FirstOrDefault().Text;
            }

            var response = await client.CompleteChatAsync(openaiChatMessages);
            return response.Value.Content.FirstOrDefault().Text;
        }
       
    }
}
