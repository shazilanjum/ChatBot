using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoChatApp.Services
{
    public class ChatService : IChatService
    {
        private readonly IOpenAIService _openAIService;

        public ChatService(IOpenAIService openAIService) 
        {
            _openAIService = openAIService;
        }


        public async Task CreateNewChat()
        {

        }

        public async Task<string> GetChatTitle()
        {
            var message = "Based on the following user message, generate a short and meaningful chat title. Just give me the title. Nothing Else:\n\n" +
                 $"User: Act as a C# developer and help me generate a chat bot using OpenAI\n\n";

            List<ChatMessage> chatMessages = new List<ChatMessage>
            {
                new ChatMessage(Models.Enum.SenderRoles.User, message)
            };

            string title = await _openAIService.Chat(chatMessages);

            return title;
        }
    }
}
