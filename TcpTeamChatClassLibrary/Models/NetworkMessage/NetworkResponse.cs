using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models.NetworkMessage
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResponseType
    {
        LoginSuccess,
        LoginError,
        RegisterSuccess,
        RegisterError,
        MessageReceived,
        MessageError,
        UnexpectedError,
        UserDoesNotExist,
        SuccessContactRequest
    }

    public class NetworkResponse
    {
        public ResponseType Type { get; set; }
        public JsonElement Payload { get; set; }

        public NetworkResponse() { }
        public NetworkResponse(ResponseType type, object? response, JsonSerializerOptions options)
        {
            Type = type;
            Payload = JsonSerializer.SerializeToElement(response, options);
        }
        public NetworkResponse(ResponseType type, JsonElement payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}
