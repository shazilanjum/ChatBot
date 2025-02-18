using DemoChatApp.Data;
using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
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
        private readonly ChatDbContext _dbContext;

        public ChatService(IOpenAIService openAIService, ChatDbContext chatDbContext) 
        {
            _openAIService = openAIService;
            _dbContext = chatDbContext;
        }


        public async Task<Chat> StartNewChat(string message, ChatModelSettings modelSettings = null)
        {
            string title = await GetChatTitleFromUserMesssage(message);

            Chat chat = new()
            {
                Title = title,
                ModelSettings = modelSettings ?? new ChatModelSettings(),
                ChatHistory = [new(SenderRoles.User, message)]
            };

            _dbContext.Chats.Add(chat);

            await _dbContext.SaveChangesAsync();

            return chat;
        }

        private async Task<string> GetChatTitleFromUserMesssage(string userMessage)
        {
            var promptMessage = "Based on the following user message, generate a short and meaningful chat title. Just give me the title. Nothing Else:\n\n" +
                 $"User: {userMessage}\n\n";

            List<ChatMessage> chatMessages =
            [
                new ChatMessage(SenderRoles.User, promptMessage)
            ];

            return await _openAIService.Chat(chatMessages);
        }
    }
}
