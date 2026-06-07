using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpTeamChatClassLibrary.Models;

namespace TcpTeamChatClassLibrary
{
    internal class ChatDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }

        /* OnConfiguring */
    }
}
