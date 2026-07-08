using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    public class User
    {
        // Мабуть так буде легше
        public User()
        {
            CreatedAt = DateTime.Now; LastSeen = DateTime.Now;
        }

        public int Id { get; set; }

        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;

        public UserStatus Status { get; set; }

        // те як користувач відображатиметься у інших
        public string Nickname { get; set; } = default!;

        //коли останній раз був в сети
        public DateTime LastSeen { get; set; }

        // New
        public ICollection<Contact> OwnContacts { get; set; } = new List<Contact>();
        public ICollection<Contact> AddedToContacts { get; set; } = new List<Contact>();

        public IList<Message> SentMessages { get; set; } = new List<Message>();
        public IList<Message> ReceivedMessages { get; set; } = new List<Message>();

        public DateTime CreatedAt { get; private set; }

        // Зайве (поки не видаляти)
        /*
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();

        // кого я заблокував
        public ICollection<BlackList> BlackListedUsers { get; set; } = new List<BlackList>();

        // хто заблокував мене
        public ICollection<BlackList> BlockedByUsers { get; set; } = new List<BlackList>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<Chat> AdminChats { get; set; } = new List<Chat>();
*/
    }
}