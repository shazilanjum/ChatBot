using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using DemoChatApp.Options;
using Microsoft.Extensions.Options;
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


        public async Task<string> Chat(List<Models.ChatMessage> chatMessage)
        {

            List<OpenAI.Chat.ChatMessage> chatMessages = new List<OpenAI.Chat.ChatMessage>();

            chatMessage.ForEach(message =>
            {
                switch(message.Sender)
                {
                    case SenderRoles.User:
                        chatMessages.Add(new UserChatMessage(message.Message));
                        break;
                    
                    case SenderRoles.Bot:
                        chatMessages.Add(new AssistantChatMessage(message.Message));
                        break;
                };

            });
            

            ChatClient client = new(model: "gpt-4o", apiKey: _openAIOptions.ApiKey);

            var response = await client.CompleteChatAsync(chatMessages);

            return response.Value.Content.FirstOrDefault().Text;

           
        }
    }
}
