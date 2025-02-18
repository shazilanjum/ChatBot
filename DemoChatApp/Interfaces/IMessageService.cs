using DemoChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Interfaces
{
    public interface IMessageService
    {
        Task<List<ChatMessage>> GetMessagesByChatIdAsync(int chatId);

        Task<bool> AddMessagesInChatAsync(int chatId, List<ChatMessage> messages);
    }
}
