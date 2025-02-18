using DemoChatApp.Data;
using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using DemoChatApp.Options;
using DemoChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private ChatModelSettings chatSetting;
        public ChatService(ChatDbContext context, IOpenAIService openAIService, IMessageService messageService)
        {
            _context = context;
            _openAIService = openAIService;
            _messageService = messageService;
        }

        public async Task<List<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats.FindAsync(chatId);
        }

        public List<string> GetModels() => new()
        {
            "gpt-4",
            "gpt-4-turbo",
            "gpt-3.5-turbo"
        };

        public async Task<ChatModelSettings> GetChatSettingsAsync(int chatId)
        {
            return await _context.ChatModelSettings
                .FirstOrDefaultAsync(s => s.ChatID == chatId);
        }

        public async Task<ChatDetails> GetChatDetailsAsync(int chatId)
        {
            var chat = await GetChatByIdAsync(chatId);
            if (chat == null) return null;

            var messages = await _messageService.GetMessagesByChatIdAsync(chatId);
            var settings = await GetChatSettingsAsync(chatId);
            chatSetting = settings;

            return new ChatDetails
            {
                Chat = chat,
                Messages = messages,
                ChatSettings = settings
            };
        }

        public async Task<int> CreateChatAsync(string userMessage)
        {
            string chatTitle = await _openAIService.GenerateChatTitle(userMessage);
            if (string.IsNullOrEmpty(chatTitle))
            {
                return -1;
            }
            var chat = new Chat { Title = chatTitle };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat.ID;
        }

        public async Task<bool> AddChatSettingsAsync(int chatId, ChatModelSettings settings)
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
            chatSetting = settings;
            return true;
        }

        public async Task<string> NewChatResponseAsync(ChatModelSettings settings, string userMessage)
        {
            string gptResponse = await _openAIService.Chat(userMessage, settings);
            if (string.IsNullOrEmpty(gptResponse))
            {
                return null;
            }

            var chatId = await CreateChatAsync(userMessage);
            if (chatId == -1) return null;

            bool settingsAdded = await AddChatSettingsAsync(chatId, settings);
            if (!settingsAdded) return null;

            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new ChatMessage(SenderRoles.User, userMessage));
            messages.Add(new ChatMessage(SenderRoles.Assistant, gptResponse));
            bool MessagesAdded = await _messageService.AddMessagesAsync(chatId, messages);
            if (!MessagesAdded) return null;

            return gptResponse;
        }

        public async Task<string> ChatWithContextResponseAsync(int chatId, string userMessage)
        {
            if (chatSetting == null)
            {
                var settings = await GetChatSettingsAsync(chatId);
                chatSetting = settings;
            }

            List<ChatMessage> messagesList = new List<ChatMessage>();
            messagesList = await _context.ChatMessages
                                   .Where(m => m.ChatID == chatId)
                                   .OrderByDescending(m => m.Timestamp)
                                   .Take(20)
                                   .OrderBy(m => m.Timestamp)
                                   .Select(m => new ChatMessage(m.Sender, m.Message))
                                   .ToListAsync();

            messagesList.Add(new ChatMessage(SenderRoles.User, userMessage));

            string gptResponse = await _openAIService.Chat(userMessage, chatSetting, messagesList);

            if (string.IsNullOrEmpty(gptResponse))
            {
                return null;
            }

            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new ChatMessage(SenderRoles.User, userMessage));
            messages.Add(new ChatMessage(SenderRoles.Assistant, gptResponse));
            bool MessagesAdded = await _messageService.AddMessagesAsync(chatId, messages);
            if (!MessagesAdded) return null;

            return gptResponse;
        }

        public async Task<bool> DeleteChatAsync(int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null) return false;

            var messages = await _context.ChatMessages.Where(m => m.ChatID == chatId).ToListAsync();
            if (messages.Any())
            {
                _context.ChatMessages.RemoveRange(messages);
            }

            var setting = await _context.ChatModelSettings.FirstOrDefaultAsync(s => s.ChatID == chatId);

            if (setting != null)
            {
                int modelSettingId = setting.ID;
                var modelParemeters = await _context.ModelParameters.FirstOrDefaultAsync(s => s.ChatModelSettingsID == modelSettingId);
                if (modelParemeters != null)
                {
                    _context.ModelParameters.Remove(modelParemeters);
                }
                _context.ChatModelSettings.Remove(setting);

            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
