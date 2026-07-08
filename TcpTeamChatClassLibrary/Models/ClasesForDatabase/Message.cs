using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;


        // тут все понятно
        public User Sender { get; set; } = default!;
        public int SenderId { get; set; }
        public DateTime TimeWhenMessageSended { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; } = default!;

        public Contact Contact { get; set; } = default!;
        public int ContactId { get; set; }
        /*
        public Chat? Chat { get; set; }
        public int ChatId { get; set; }
        */
    }
}
