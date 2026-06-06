using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }


        // тут все понятно
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime TimeWhenMessageSended { get; set; }

        public Chat Chat { get; set; }
        public int ChatId { get; set; }
    }
}
