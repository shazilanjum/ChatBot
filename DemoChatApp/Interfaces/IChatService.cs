using DemoChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Interfaces
{
    internal interface IChatService
    {
        Task<Chat> StartNewChat(string message, ChatModelSettings modelSettings = null);
    }
}
