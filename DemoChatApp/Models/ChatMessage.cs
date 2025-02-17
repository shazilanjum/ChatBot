using DemoChatApp.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace DemoChatApp.Models
{
    public class ChatMessage
    {
        [Key]
        public int ID { get; set; }

        public SenderRoles Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public Chat Chat { get; set; }
        public int ChatID { get; set; }

        public ChatMessage(SenderRoles sender, string message)
        {
            Sender = sender;
            Message = message;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"[{Timestamp}] {Sender}: {Message}";
        }
    }

}