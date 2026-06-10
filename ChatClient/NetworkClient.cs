using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text.Json;     
using Message = ChatClient.Models.Message;

namespace ChatClient
{
    public class NetworkEnvelope
    {
        public string Type { get; set; }
        public string Payload { get; set; } // Внутренняя JSON-строка
    }

    public class LoginResponsePayload
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }

    //
    // public class NewMessageResponsePayload
    // public class ChatListResponsePayload
    // ...
    //

    internal class NetworkClient
    {
        // Поля для работы с TCP-сокетами
        private TcpClient _tcpClient;
        private StreamReader _reader;
        private StreamWriter _writer;
        private Thread _receiveThread;
        private bool _isConnected;

        // Настройка, чтобы C# не ругался, если сервер пришлет "type" вместо "Type"
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public string ServerIP { get; private set; }
        public int ServerPort { get; private set; }

        // Responces
        public event Action<bool, string> OnLoginResult;           // bool - успешно/нет, string - сообщение об ошибке или ник
        public event Action<bool, string> OnRegisterResult;        // аналогично для регистрации
        public event Action<List<User>> OnContactsReceived;        // передает список найденных или существующих контактов
        public event Action<List<User>> OnBlacklistReceived;       // передает список заблокированных
        public event Action<Message> OnMessageReceived;            // срабатывает при ПРИЁМЕ нового сообщения (в реальном времени)
        public event Action<List<Message>> OnHistoryReceived;      // передает пачку сообщений из истории для отрисовки
        public event Action<List<Chat>> OnChatsListReceived;       // передает список активных диалогов пользователя
        // Ивенты для подтверждения действий с контактами и ЧС
        public event Action<bool, string> OnContactAddedResult;    // Успешно ли добавился контакт, string - текст ошибки/успеха
        public event Action<bool, int> OnContactDeletedResult;     // bool - успех, int - ID удаленного контакта
        public event Action<bool, int> OnBlacklistChangedResult;   // bool - успех, int - ID заблокированного/разблокированного
        public event Action<bool, int, string> OnContactUpdated;   // Локальное переименование: статус, ID контакта, новое имя
        // Глобальное уведомление от сервера (Синхронизация)
        public event Action<int, string> OnGlobalUserChanged;      // Срабатывает, если кто-то ДРУГОЙ в сети сменил глобальный ник (UserId, НовыйНик)

        // autorisation
        public void Connect(string ip, int port)
        {
            try
            {
                if (_isConnected) return;

                ServerIP = ip;
                ServerPort = port;

                // Создаем подключение
                _tcpClient = new TcpClient();
                _tcpClient.Connect(ip, port);

                // Настраиваем потоки чтения и записи
                var networkStream = _tcpClient.GetStream();
                _reader = new StreamReader(networkStream, Encoding.UTF8);
                _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

                _isConnected = true;

                // Запускаем фоновый поток, который бесконечно слушает ответы от сервера
                _receiveThread = new Thread(ReceiveMessagesLoop)
                {
                    IsBackground = true // Чтобы поток умирал сам при закрытии приложения
                };
                _receiveThread.Start();

                Console.WriteLine("Успешное подключение к серверу!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                _isConnected = false;
            }
        }

        // Блок бесконечного слушания сервера
        private void ReceiveMessagesLoop()
        {
            try
            {
                while (_isConnected && _reader != null)
                {
                    // Ждем строку от сервера (сервер должен слать сообщение и в конце '\n')
                    string rawPacket = _reader.ReadLine();
                    if (rawPacket == null)
                    {
                        // Если прилетел null — сервер разорвал соединение
                        Disconnect();
                        break;
                    }

                    // Передаем сырую строку на разбор в обработчик
                    ParseServerPacket(rawPacket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Поток чтения остановлен: {ex.Message}");
                Disconnect();
            }
        }

        private void ParseServerPacket(string rawPacket)
        {
            if (string.IsNullOrEmpty(rawPacket)) return;

            try
            {
                var envelope = JsonSerializer.Deserialize<NetworkEnvelope>(rawPacket, _jsonOptions);

                if (envelope == null) return;

                switch (envelope.Type)
                {
                    case "LoginResult":
                        var loginResult = JsonSerializer.Deserialize<LoginResponsePayload>(envelope.Payload, _jsonOptions);

                        if (loginResult != null)
                        {
                            OnLoginResult?.Invoke(loginResult.Success, loginResult.Message);
                        }
                        break;

                    case "RegisterResult":
                        var regResult = JsonSerializer.Deserialize<LoginResponsePayload>(envelope.Payload, _jsonOptions);

                        if (regResult != null)
                        {
                            OnRegisterResult?.Invoke(regResult.Success, regResult.Message);
                        }
                        break;

                    //
                    // Сюда нужно будет добавлять новые кейсы (ChatsList, NewMessage и т.д.)
                    //

                    default:
                        Console.WriteLine($"Неизвестный тип пакета: {envelope.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга JSON: {ex.Message}");
            }
        }

        public void Login(string username, string password)
        {
            if (!_isConnected)
            {
                OnLoginResult?.Invoke(false, "Нет соединения с сервером.");
                return;
            }

            var loginData = new { Username = username, Password = password };
            string internalJson = JsonSerializer.Serialize(loginData);

            var envelope = new NetworkEnvelope
            {
                Type = "Login",
                Payload = internalJson
            };

            string finalJson = JsonSerializer.Serialize(envelope);
            _writer.WriteLine(finalJson);
        }

        public void Register(string username, string password)
        {
            if (!_isConnected)
            {
                OnRegisterResult?.Invoke(false, "Нет соединения с сервером.");
                return;
            }

            var registerData = new { Username = username, Password = password };
            string internalJson = JsonSerializer.Serialize(registerData);

            var envelope = new NetworkEnvelope
            {
                Type = "Register",
                Payload = internalJson
            };

            string finalJson = JsonSerializer.Serialize(envelope);
            _writer.WriteLine(finalJson);
        }

        public void Disconnect()
        {
            if (!_isConnected) return;
            _isConnected = false;

            try
            {
                var envelope = new NetworkEnvelope { Type = "Disconnect", Payload = "" };
                _writer?.WriteLine(JsonSerializer.Serialize(envelope));

                _reader?.Close();
                _writer?.Close();
                _tcpClient?.Close();
            }
            catch { }
            finally
            {
                _tcpClient = null;
                _reader = null;
                _writer = null;
                Console.WriteLine("[INTERNAL] Сеть успешно отключена.");
            }
        }

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
