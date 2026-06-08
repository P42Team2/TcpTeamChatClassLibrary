using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string IpAddressStr { get; set; } = "127.0.0.1";

        public int Port { get; set; }

        public UserStatus Status { get; set; }



        // Нове
        // те як користувач відображатиметься у інших
        public string Nickname { get; set; }

        public ICollection<Chat> Chats { get; set; } = new List<Chat>();

        // кого я заблокував
        public ICollection<BlackList> BlackListedUsers { get; set; } = new List<BlackList>();

        // хто заблокував мене
        public ICollection<BlackList> BlockedByUsers { get; set; } = new List<BlackList>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<Chat> AdminChats { get; set; } = new List<Chat>();


        //коли останній раз був в сети

        public DateTime LastSeen { get; set; }
    }
}
