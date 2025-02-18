using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Models
{
    public class ModelParameters
    {
        [Key]
        public int ID { get; set; }

        public int MaxTokens { get; set; }

        public float Temperature { get; set; }

        public float TopP { get; set; }


        public ChatModelSettings ChatModelSettings { get; set; }
        public int ChatModelSettingsID { get; set; }

        public ModelParameters()
        {
            MaxTokens = 1000;
            Temperature = 0.7f;
            TopP = 1.0f;
        }
    }

}
