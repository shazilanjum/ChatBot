using DemoChatApp.Data;
using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using DemoChatApp.Options;
using DemoChatApp.ViewModels;
using DevExpress.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMessageService _messageService;
        private readonly ChatDbContext _context;
        public ChatService(ChatDbContext context, IOpenAIService openAIService, IMessageService messageService)
        {
            _context = context;
            _openAIService = openAIService;
            _messageService = messageService;
        }

        public async Task<List<Chat>> GetChatsListAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(chat => chat.ID == chatId);

            return chat;
        }

        public async Task<Chat> CreateChatAsync(string userMessage, ChatModelSettings chatModelSettings)
        {
            string chatTitle = await GenerateChatTitle(userMessage);
            
            if (string.IsNullOrEmpty(chatTitle))
            {
                return null;
            }

            var chat = new Chat 
            { 
                Title = chatTitle, 
                ModelSettings = chatModelSettings, 
                ChatHistory =
                [
                    new(SenderRoles.User, userMessage)
                ] 
            };

            _context.Chats.Add(chat);
            
            await _context.SaveChangesAsync();
           
            return chat;
        }


        public async Task<string> ChatWithAI(string userMessage, Chat chat, bool settingsChanged = false)
        {
            if (settingsChanged)
            {
                await AddChatSettingsAsync(chat.ID, chat.ModelSettings);
            }

            
            List<Models.ChatMessage> messagesList = chat.ChatHistory
                                .OrderByDescending(m => m.Timestamp)
                                .Take(20)
                                .Select(m => new Models.ChatMessage(m.Sender, m.Message))
                                .ToList();

            messagesList.Add(new Models.ChatMessage(SenderRoles.User, userMessage));

            var gptResponse = await _openAIService.Chat(userMessage, messagesList, chat.ModelSettings);

            if (string.IsNullOrEmpty(gptResponse))
            {
                return null;
            }


            List<ChatMessage> messages = [new(SenderRoles.User, userMessage), new(SenderRoles.Assistant, gptResponse)];

            bool messagesAdded = await _messageService.AddMessagesInChatAsync(chat.ID, messages);

            if (!messagesAdded) return null;

            return gptResponse;
        }

        public async Task<bool> DeleteChatAsync(int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat == null) return false;

            _context.Chats.Remove(chat);

            await _context.SaveChangesAsync();

            return true;
        }
        
        private async Task<bool> AddChatSettingsAsync(int chatId, ChatModelSettings settings)
        {
            var existingSetting = await _context.ChatModelSettings.FirstOrDefaultAsync(s => s.ChatID == chatId);

            if (existingSetting != null)
            {
                existingSetting.SelectedModel = settings.SelectedModel;
                existingSetting.Parameters = settings.Parameters;


                _context.ChatModelSettings.Update(existingSetting);
            }
            else
            {
                var newChatSetting = new ChatModelSettings
                {
                    ChatID = chatId,
                    Parameters = settings.Parameters
                };

                _context.ChatModelSettings.Add(newChatSetting);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> GenerateChatTitle(string userMessage)
        {
            var prompt = "Based on the following user message, generate a short and meaningful chat title:\n\n" +
                            $"User: {userMessage}\n\n" +
                            "Title:";

            return await _openAIService.Chat(prompt);
        }
    }
}
