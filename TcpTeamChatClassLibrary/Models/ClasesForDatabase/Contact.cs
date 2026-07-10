using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TcpTeamChatClassLibrary.Models
{
    // новий клас
    public class Contact
    {
        public int Id { get; set; }

        public int ContactUserId { get; set; }
        [JsonIgnore]
        public User ContactUser { get; set; } = default!;

        public int OwnerUserId { get; set; }
        [JsonIgnore]
        public User OwnerUser { get; set; } = default!;

        public string? DisplayName { get; set; }
        public DateTime AddedAt { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
