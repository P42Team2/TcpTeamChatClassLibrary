using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Models;
using Message = ChatClient.Models.Message;

namespace ChatClient
{
    internal class NetworkClient
    {
        // Responces
        public event Action<bool, string> OnLoginResult;       // bool - успешно/нет, string - сообщение об ошибке или ник
        public event Action<bool, string> OnRegisterResult;    // аналогично для регистрации
        public event Action<List<User>> OnContactsReceived;    // передает список найденных или существующих контактов
        public event Action<List<User>> OnBlacklistReceived;   // передает список заблокированных
        public event Action<Message> OnMessageReceived;        // срабатывает при ПРИЁМЕ нового сообщения (в реальном времени)
        public event Action<List<Message>> OnHistoryReceived;  // передает пачку сообщений из истории для отрисовки

        // я замінив Chat на Contact бо класу чат тепер немає
        public event Action<List<Contact>> OnChatsListReceived;   // передает список активных диалогов пользователя

        // autorisation
        public void Connect(string ip, int port) { }
        public void Login(string username, string password) { }
        public void Register(string username, string password) { }
        public void Disconnect() { }

        // Contacts Management
        public void SearchContacts(string usernameQuery) { }
        public void AddContact(int targetUserId) { }
        public void DeleteContact(int targetUserId) { }

        // Black list Management
        public void AddToBlacklist(int targetUserId) { }
        public void RemoveFromBlacklist(int targetUserId) { }
        public void LoadBlacklist() { }

        // Chats and Messages
        public void SendMessage(int chatId, string messageText) { }
        public void LoadChatHistory(int chatId, int lastMessageId = 0) { }
        public void CreateGroupChat(string groupName, List<int> memberIds) { }
        public void SearchInChat(int chatId, string searchQuery) { }
        public void GlobalMessageSearch(string searchQuery) { }
    }
}
