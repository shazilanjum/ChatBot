using DemoChatApp.Models;
using DemoChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Interfaces
{
    internal interface IChatService
    {
        Task<List<Chat>> GetAllChatsAsync();
        List<string> GetModels();
        Task<Chat> GetChatByIdAsync(int chatId);
        Task<ChatModelSettings> GetChatSettingsAsync(int chatId);
        Task<ChatDetails> GetChatDetailsAsync(int chatId);
        Task<int> CreateChatAsync(string userMessage);
        Task<bool> AddChatSettingsAsync(int chatId, ChatModelSettings settings);
        Task<string> NewChatResponseAsync(ChatModelSettings settings, string userMessage);
        Task<string> ChatWithContextResponseAsync(int chatId, string userMessage);
        Task<bool> DeleteChatAsync(int chatId);
    }
}
