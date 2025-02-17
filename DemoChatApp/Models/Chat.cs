using DemoChatApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoChatApp.Models.ChatMessage;

namespace DemoChatApp.Models
{
    public class Chat : Metadata
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public ChatModelSettings ModelSettings { get; set; }

        public List<ChatMessage> ChatHistory { get; set; }

        public Chat()
        {
            ChatHistory = new List<ChatMessage>();
            CreatedDate = DateTime.UtcNow;
            ModelSettings = new ChatModelSettings();
        }

        public void AddMessage(SenderRoles sender, string message)
        {
            var chatMessage = new ChatMessage(sender, message);
            ChatHistory.Add(chatMessage);
        }

        public string GetChatSummary()
        {
            return $"Title: {Title} | Model: {nameof(ModelSettings.SelectedModel)} | Messages: {ChatHistory.Count}";
        }
    }

}
