using DemoChatApp.Models;
using DemoChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Interfaces
{
    public interface IChatService
    {
        Task<List<Chat>> GetChatsListAsync();

        Task<Chat> GetChatByIdAsync(int chatId);

        Task<Chat> AddOrUpdateChatAsync(Chat chat);

        Task<string> ChatWithAI(string userMessage, Chat chat);
        
        Task<bool> DeleteChatAsync(int chatId);

        Task<string> GenerateChatTitle(string userMessage);
    }
}
