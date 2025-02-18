using DemoChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.ViewModels
{
    public class ChatDetails
    {
        public Chat Chat { get; set; }
        public List<ChatMessage>? Messages { get; set; }
        public ChatModelSettings ChatSettings { get; set; }
    }
}
