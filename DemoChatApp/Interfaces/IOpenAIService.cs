using DemoChatApp.Models;

namespace DemoChatApp.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> Chat(string userMessage, ChatModelSettings settings, List<Models.ChatMessage> chatHistory = null);
        Task<string> GenerateChatTitle(string userMessage);
    }
}