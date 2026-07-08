// потім, або ніколи
// про цей клас можна забути
// але я на всякий випадок його залишу
// поки бажано з полями цього класу нічого не коїти
// більшу їх частину буде змінено

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    // new class
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User Admin;
        public int AdminId { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}