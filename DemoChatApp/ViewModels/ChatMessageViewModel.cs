using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.ViewModels
{
    public class ChatMessageViewModel
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public ChatMessageViewModel(ChatMessage chatMessage)
        {
            Sender = nameof(chatMessage.Sender);
            Message = chatMessage.Message;
            Timestamp = chatMessage.Timestamp;
        }
    }
}
