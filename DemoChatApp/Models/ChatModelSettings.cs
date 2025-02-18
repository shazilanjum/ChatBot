using DemoChatApp.Contracts;
using DemoChatApp.Models.Enum;
using DemoChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DemoChatApp.Models
{
    public class ChatModelSettings
    {
        [Key]
        public int ID { get; set; }

        public ChatModels SelectedModel { get; set; }

        public Chat Chat { get; set; }
        public int ChatID { get; set; }

        public ModelParameters Parameters { get; set; }

        public ChatModelSettings()
        {
            SelectedModel = ChatModels.gpt3_5;
            Parameters = new ModelParameters();
        }

        public ChatModelSettings(ChatModelSettingsViewModel settingsViewModel)
        {
            SelectedModel = OpenAIModels.OpenAIModelsMapping.FirstOrDefault(kv => kv.Value == settingsViewModel.SelectedModel).Key;
            Parameters = new ModelParameters 
            { 
                MaxTokens = settingsViewModel.MaxTokens,
                Temperature = settingsViewModel.Temperature,
                TopP = settingsViewModel.TopP,
            };
        }
    }

}
