using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Models
{
    public class Metadata
    {
        public DateTime CreatedDate { get; set; }  // When the conversation was created

        public Metadata()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }

}
