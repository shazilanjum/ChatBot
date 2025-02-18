using DemoChatApp.Contracts;
using DemoChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.ViewModels
{
    public class ChatModelSettingsViewModel
    {
        public string SelectedModel { get; set; }

        public int MaxTokens { get; set; }

        public float Temperature { get; set; }

        public float TopP { get; set; }

        public ChatModelSettingsViewModel(ChatModelSettings modelSettings)
        {
            SelectedModel = OpenAIModels.OpenAIModelsMapping[modelSettings.SelectedModel];
            MaxTokens = modelSettings.Parameters.MaxTokens;
            Temperature = modelSettings.Parameters.Temperature;
            TopP = modelSettings.Parameters.TopP;
        }

        public ChatModelSettingsViewModel()
        {
            SelectedModel = OpenAIModels.OpenAIModelsMapping[DemoChatApp.Models.Enum.ChatModels.gpt3_5];
            MaxTokens = 1000;
            Temperature = 0.7f;
            TopP = 1.0f;
        }
    }
}
