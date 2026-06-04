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
    }
}
