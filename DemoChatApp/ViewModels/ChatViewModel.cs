using DemoChatApp.Contracts;
using DemoChatApp.Interfaces;
using DemoChatApp.Models;
using DemoChatApp.Models.Enum;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.ViewModels
{
    public class ChatViewModel
    {
        private readonly IChatService _chatService;

        public List<string> OpenAIModelsList { get; set; } = OpenAIModels.OpenAIModelsMapping.Values.ToList();

        public List<ChatListViewModel> Chats { get; private set; } = new();

        public Chat SelectedChat { get; set; }

        public IEnumerable<ChatListViewModel> SelectedChatSelection { get; set; }

        public ChatModelSettingsViewModel ChatModelSettingsViewModel { get; set; } = new();

        public List<ChatMessage> Messages { get; private set; } = new();
        public string UserMessage { get; set; } = string.Empty;
        public string ChatText { get; private set; } = "";
        public IEnumerable<string> Models { get; } = new List<string> { "GPT-4", "GPT-3.5", "Custom Model" };

        public ChatViewModel(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task LoadChats()
        {
            var chats = await _chatService.GetChatsListAsync();

            Chats = chats.Select(chat => new ChatListViewModel
            {
                ChatID = chat.ID,
                ChatTitle = chat.Title,
            }).ToList();
        }

        public async Task StartNewChat()
        {
            UserMessage = "";
            SelectedChat = null;
            ChatModelSettingsViewModel = new ChatModelSettingsViewModel();
            SelectedChatSelection = new List<ChatListViewModel>();
        }

        public async Task SendMessage()
        {
            ChatModelSettings chatModelSettings = new(ChatModelSettingsViewModel);

            bool firstChat = SelectedChat is null;

            if (firstChat)
            {
                SelectedChat = new Chat
                {
                    ModelSettings = chatModelSettings,
                    Title = await _chatService.GenerateChatTitle(UserMessage)
                };
            }
            else
            {
                SelectedChat.ModelSettings = chatModelSettings;
            }

            var gptResponse = await _chatService.ChatWithAI(UserMessage, SelectedChat);

            if (!string.IsNullOrWhiteSpace(gptResponse))
            {
                SelectedChat.ChatHistory.Add(new(SenderRoles.User, UserMessage));
                SelectedChat.ChatHistory.Add(new(SenderRoles.Assistant, gptResponse));

                var createdChat = await _chatService.AddOrUpdateChatAsync(SelectedChat);

                SelectedChat.ID = createdChat.ID;

                UserMessage = "";

                if (firstChat)
                {
                    Chats.Add(new()
                    {
                        ChatID = SelectedChat.ID,
                        ChatTitle = SelectedChat.Title,
                    });
                }
            }
        }

        public async Task OnClickChatChange(IEnumerable<ChatListViewModel> chats)
        {
            if (chats != null && chats.Any())
            {
                var chat = await _chatService.GetChatByIdAsync(chats.FirstOrDefault().ChatID);

                if (chat != null)
                {
                    SelectedChat = chat;

                    ChatModelSettingsViewModel = new(chat.ModelSettings);
                }   
            }
        }

        private bool CheckIfSettingsChanged(ChatModelSettings chatModelSettings)
        {
            return SelectedChat.ModelSettings != chatModelSettings;
        }

        public async Task DeleteChat(int chatId)
        {
            await _chatService.DeleteChatAsync(chatId);

            var chatToDelete = Chats.FirstOrDefault(chat => chat.ChatID == chatId);

            Chats.Remove(chatToDelete);

            await StartNewChat();
        }
    }

}
