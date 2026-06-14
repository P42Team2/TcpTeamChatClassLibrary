using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models.NetworkMessage
{
    public enum ResponseType
    {
        LoginSuccess,
        LoginError,
        RegisterSuccess,
        RegisterError,
        MessageReceived
    }

    public class NetworkResponse
    {
        public ResponseType Type { get; set; }
        public string Payload { get; set; }

        public NetworkResponse(ResponseType type, string response)
        {
            Type = type;
            Payload = response;
        }
    }
}
