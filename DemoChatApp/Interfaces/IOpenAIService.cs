namespace DemoChatApp.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> Chat(List<Models.ChatMessage> chatMessage);
    }
}