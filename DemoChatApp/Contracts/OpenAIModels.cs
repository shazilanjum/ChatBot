using DemoChatApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Contracts
{
    public static class OpenAIModels
    {
        public static Dictionary<ChatModels, string> OpenAIModelsMapping = new Dictionary<ChatModels, string>
        {
            {ChatModels.gpt4, "gpt4" },
            {ChatModels.gpt4o, "gpt-4o" },
            {ChatModels.gpt3_5, "gpt-3.5-turbo" },
        };

    }
}
