using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models.NetworkMessage
{
    public enum RequestType
    {
        Login,
        Register,
        SearchContacts,
        AddContact,
        DeleteContact,
        AddToBlacklist,
        RemoveFromBlacklist,
        LoadBlacklist,
        SendMessage,
        LoadChatHistory,
        CreateGroupChat,
        SearchInChat,
        GlobalMessageSearch
    }

    public class NetworkRequest
    {
        public NetworkRequest(RequestType type, string request)
        {
            Type = type;
            Request = request;
        }

        public RequestType Type { get; set; }
        public string Request { get; set; }
    }
}
