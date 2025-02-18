using DemoChatApp.Data;
using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Services
{
    internal class MessageService : IMessageService
    {
        private readonly ChatDbContext _context;
        public MessageService(ChatDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<ChatMessage>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatID == chatId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<bool> AddMessagesInChatAsync(int chatId, List<ChatMessage> messages)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null) return false; // Return -1 if chat does not exist

            if (messages == null || !messages.Any()) return false; // No messages to add

            foreach (var message in messages)
            {
                message.ChatID = chatId;
            }

            _context.ChatMessages.AddRange(messages);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
