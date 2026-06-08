using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models.DTO
{
    public class LoginAndRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
