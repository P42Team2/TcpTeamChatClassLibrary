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
        public string? ServerIP { get; internal set; }
        public int ServerPort { get; internal set; }

        // Responces
        public event Action<bool, string> OnLoginResult;       // bool - успешно/нет, string - сообщение об ошибке или ник
        public event Action<bool, string> OnRegisterResult;    // аналогично для регистрации
        public event Action<List<User>> OnContactsReceived;    // передает список найденных или существующих контактов
        public event Action<List<User>> OnBlacklistReceived;   // передает список заблокированных
        public event Action<Message> OnMessageReceived;        // срабатывает при ПРИЁМЕ нового сообщения (в реальном времени)
        public event Action<List<Message>> OnHistoryReceived;  // передает пачку сообщений из истории для отрисовки
        public event Action<List<Chat>> OnChatsListReceived;   // передает список активных диалогов пользователя
        // Ивенты для подтверждения действий с контактами и ЧС
        public event Action<bool, string> OnContactAddedResult;    // Успешно ли добавился контакт, string - текст ошибки/успеха
        public event Action<bool, int> OnContactDeletedResult;     // bool - успех, int - ID удаленного контакта
        public event Action<bool, int> OnBlacklistChangedResult;   // bool - успех, int - ID заблокированного/разблокированного
        public event Action<bool, int, string> OnContactUpdated;   // Локальное переименование: статус, ID контакта, новое имя
        // Глобальное уведомление от сервера (Синхронизация)
        public event Action<int, string> OnGlobalUserChanged;      // Срабатывает, если кто-то ДРУГОЙ в сети сменил глобальный ник (UserId, НовыйНик)

        // autorisation
        public void Connect(string ip, int port) { }
        public void Login(string username, string password) { }
        public void Register(string username, string password) { }
        public void Disconnect() { }

        // Contacts Management
        public void SearchContacts(string usernameQuery) { }
        public void AddContact(int targetUserId) { }
        public void DeleteContact(int targetUserId) { }
        public void LoadContactsList() { }
        public void UpdateContact(int userId, string updatedName) { }

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
        public void LoadChatsList() { }

        // Account`s Settings
        public void ChangePassword(string oldPassword, string newPassword) { }
        public void ChangeMyUsername(string newUsername) { }


        // вспомогательные функции

        public void AddContactByNickname(string targetUsername)
        {
            if (string.IsNullOrEmpty(targetUsername)) return;

            // Создаем локальное временное действие, что бы подписать ее на OnContactsReceived
            Action<List<User>> temporaryHandler = null;

            temporaryHandler = (users) =>
            {
                // Сервер прислал список найденных людей. Ищем среди них того, чей ник совпал на 100%
                var foundUser = users.FirstOrDefault(u => u.Nickname.Equals(targetUsername, StringComparison.OrdinalIgnoreCase));

                if (foundUser != null)
                {
                    // нашли, отправляем запрос на Добавление юзера
                    AddContact(foundUser.Id);
                }
                else
                {
                    // не нашли
                    Console.WriteLine($"Пользователь {targetUsername} не найден в базе.");
                }

                // отписываемся от ивента, чтобы этот код сработал только ОДИН раз
                OnContactsReceived -= temporaryHandler;
            };

            OnContactsReceived += temporaryHandler;

            SearchContacts(targetUsername);
        }
    }
}
