using DemoChatApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChatApp.Data
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatModelSettings> ChatModelSettings { get; set; }
        public DbSet<ModelParameters> ModelParameters { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
    }
}
