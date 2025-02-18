using DemoChatApp.Models.Enum;
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

        public string SelectedModel { get; set; }

        public Chat Chat { get; set; }
        public int ChatID { get; set; }

        public ModelParameters Parameters { get; set; }

        public ChatModelSettings()
        {
            SelectedModel = "gpt-3.5-turbo";
            Parameters = new ModelParameters();
        }
    }

}
