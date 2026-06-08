using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models.DTO
{
    public class SendMessageRequest
    {
        public int ChatId { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; }
    }
}
