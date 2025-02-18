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
        public string ChatText { get; private set; } = "End users cannot change the Memo value";
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
            ChatText = "";
            Messages = new();
            UserMessage = "";
            SelectedChat = null;
            ChatModelSettingsViewModel = new ChatModelSettingsViewModel();
            SelectedChatSelection = new List<ChatListViewModel>();

        }

        public async Task SendMessage()
        {

            ChatModelSettings chatModelSettings = new(ChatModelSettingsViewModel);


            if (SelectedChat == null)
            {

                SelectedChat = await _chatService.CreateChatAsync(UserMessage, chatModelSettings);



            }

            bool settingsChanged = CheckIfSettingsChanged(chatModelSettings);

            SelectedChat.ModelSettings = chatModelSettings;

            var gptResponse = await _chatService.ChatWithAI(UserMessage, SelectedChat, settingsChanged);

            if (!string.IsNullOrWhiteSpace(gptResponse))
            {
                Messages.Add(new(SenderRoles.User, UserMessage));
                Messages.Add(new(SenderRoles.Assistant, gptResponse));
                UserMessage = string.Empty;
            }
        }

        public async Task OnClickChatChange(IEnumerable<ChatListViewModel> chats)
        {
            if (chats != null && chats.Any())
            {
                var chat = await _chatService.GetChatByIdAsync(chats.FirstOrDefault().ChatID);

                SelectedChat = chat;
            }
            
        }

        private bool CheckIfSettingsChanged(ChatModelSettings chatModelSettings)
        {
            return SelectedChat.ModelSettings != chatModelSettings;
        }
    }

}
