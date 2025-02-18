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

        Task<Chat> CreateChatAsync(string userMessage, ChatModelSettings chatModelSettings);

        Task<string> ChatWithAI(string userMessage, Chat chat, bool settingsChanged = false);
        
        Task<bool> DeleteChatAsync(int chatId);
    }
}
