using DemoChatApp.Models;

namespace DemoChatApp.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> Chat(List<Models.ChatMessage> chatHistory = null, ChatModelSettings settings = null);
    }
}