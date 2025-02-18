using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.ViewModels
{
    public class ChatListViewModel
    {
        public int ChatID { get; set; }

        public string ChatTitle { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ChatListViewModel typedObj)
            {
                return (this.ChatID == typedObj.ChatID) && (this.ChatTitle == typedObj.ChatTitle);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ChatID, ChatTitle);
        }
    }
}
