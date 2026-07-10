//using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;     
using System.Threading.Tasks;
using Azure;
// Поточна бібліотека бази данних
using TcpTeamChatClassLibrary.Models;
// Бібліотека написана Дмитром
// з нею буде легше приймати відповіді сервера
using TcpTeamChatClassLibrary.Models.NetworkMessage;
using Message = TcpTeamChatClassLibrary.Models.Message;
using UserStatus = TcpTeamChatClassLibrary.Models.UserStatus;

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
        public bool isConnected {  get; private set; }

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
                if (isConnected) return;

                ServerIP = ip;
                ServerPort = port;

                // Создаем подключение
                _tcpClient = new TcpClient();
                _tcpClient.Connect(ip, port);

                // Настраиваем потоки чтения и записи
                var networkStream = _tcpClient.GetStream();
                _reader = new StreamReader(networkStream, Encoding.UTF8);
                _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

                isConnected = true;

                // Запускаем фоновый поток, который бесконечно слушает ответы от сервера
                _receiveThread = new Thread(ReceiveMessagesLoop)
                {
                    IsBackground = true // Чтобы поток умирал сам при закрытии приложения
                };
                _receiveThread.Start();

                Thread.Sleep(100);

                Console.WriteLine("Успешное подключение к серверу!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                isConnected = false;
            }
        }

        private void ReceiveMessagesLoop()
        {
            try
            {
                byte[] buffer = new byte[4096];
                var networkStream = _tcpClient?.GetStream();

                while (isConnected && networkStream != null)
                {
                    if (!networkStream.DataAvailable)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Disconnect();
                        break;
                    }

                    string rawPacket = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

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
                    case "ContactsReceived":
                    case "ContactsList":
                    case "SearchContactsResult":
                    case "LoadContactsListResult":
                        {
                            var users = DeserializeListPayload<User>(envelope.Payload, "Users", "Contacts", "FoundUsers", "Data", "Items");
                            OnContactsReceived?.Invoke(users);
                            break;
                        }

                    case "NewMessage":
                    case "MessageReceived":
                        {
                            var message = DeserializePayload<Message>(envelope.Payload);
                            if (message != null)
                                OnMessageReceived?.Invoke(message);
                            break;
                        }

                    case "HistoryReceived":
                    case "ChatHistory":
                    case "LoadChatHistoryResult":
                    case "SearchInChatResult":
                    case "GlobalMessageSearchResult":
                        {
                            var messages = DeserializeListPayload<Message>(envelope.Payload, "Messages", "History", "Results", "Data", "Items");
                            OnHistoryReceived?.Invoke(messages);
                            break;
                        }

                    case "ChatsListReceived":
                    case "ChatsList":
                    case "LoadChatsListResult":
                        {
                            var chats = DeserializeListPayload<Chat>(envelope.Payload, "Chats", "Data", "Items");
                            OnChatsListReceived?.Invoke(chats);
                            break;
                        }


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

        public async Task Login(string username, string password)
        {
            if (!SendRequest("Login", new { Username = username, Password = password }))
                OnLoginResult?.Invoke(false, "Нет соединения с сервером.");
            else
            {
                try
                {
                    string? json = await _reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        OnLoginResult?.Invoke(false, "Сервер закрив з'єднання.");
                        return;
                    }

                    NetworkResponse? response = JsonSerializer.Deserialize<NetworkResponse>(json, _jsonOptions);
                    User? user = null;
                    if (response == null)
                        return;

                    switch (response.Type)
                    {
                        case ResponseType.LoginSuccess:
                            {
                                user = response.Payload.Deserialize<User>(_jsonOptions);
                                break;
                            }

                        case ResponseType.LoginError:
                            {
                                string? message = response.Payload.Deserialize<string>(_jsonOptions);
                                OnLoginResult?.Invoke(false, message!);
                                return;
                            }
                    }

                    OnLoginResult?.Invoke(true, user!.Id.ToString());
                }
                catch (JsonException ex)
                {
                    OnLoginResult?.Invoke(false, "Помилка обробки JSON: " + ex.Message);
                }
                catch (IOException ex)
                {
                    OnLoginResult?.Invoke(false, "Помилка мережі: " + ex.Message);
                }
                catch (Exception ex)
                {
                    OnLoginResult?.Invoke(false, "Сталася неочікувана помилка.\nДетальніше: " + ex.Message);
                }
            }
        }

        public async Task Register(string username, string password)
        {
            if (!SendRequest("Register", new { Username = username, Password = password }))
                OnRegisterResult?.Invoke(false, "Нет соединения с сервером.");
            else
            {
                try
                {
                    string? json = await _reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        OnLoginResult?.Invoke(false, "Сервер закрив з'єднання.");
                        return;
                    }

                    NetworkResponse? response = JsonSerializer.Deserialize<NetworkResponse>(json, _jsonOptions);
                    User? user = null;
                    if (response == null)
                        return;

                    switch (response.Type)
                    {
                        case ResponseType.RegisterSuccess:
                            {
                                user = response.Payload.Deserialize<User>(_jsonOptions);
                                break;
                            }

                        case ResponseType.RegisterError:
                            {
                                string? message = response.Payload.Deserialize<string>(_jsonOptions);
                                OnLoginResult?.Invoke(false, message!);
                                return;
                            }
                    }

                    OnLoginResult?.Invoke(true, user!.Id.ToString());
                }
                catch (JsonException ex)
                {
                    OnLoginResult?.Invoke(false, "Помилка обробки JSON: " + ex.Message);
                }
                catch (IOException ex)
                {
                    OnLoginResult?.Invoke(false, "Помилка мережі: " + ex.Message);
                }
                catch (Exception ex)
                {
                    OnLoginResult?.Invoke(false, "Сталася неочікувана помилка.\nДетальніше: " + ex.Message);
                }
            }
        }

        public void Disconnect()
        {
            if (!isConnected) return;
            isConnected = false;

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
        public async Task SearchContacts(string usernameQuery)
        {
            if (string.IsNullOrWhiteSpace(usernameQuery))
            {
                OnContactsReceived?.Invoke(new List<User>());
                return;
            }

            if (!SendRequest("SearchContacts", usernameQuery ))
                OnContactsReceived?.Invoke(new List<User>());
            else
            {
                string? json = await _reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(json))
                {
                    OnContactsReceived?.Invoke(new List<User>());
                    return;
                }
                NetworkResponse? response = JsonSerializer.Deserialize<NetworkResponse>(json, _jsonOptions);
                List<User>? users = null;
                if (response == null)
                {
                    OnContactsReceived?.Invoke(new List<User>());
                    return;
                }

                switch (response.Type)
                {
                    case ResponseType.SuccessContactRequest:
                        {
                            users = response.Payload.Deserialize<List<User>>(_jsonOptions);
                            if(users == null)
                                users = new List<User>();
                            break;
                        }

                    case ResponseType.UnexpectedError:
                        {
                            string? message = response.Payload.Deserialize<string>(_jsonOptions);
                            OnContactsReceived?.Invoke(new List<User>());
                            return;
                        }
                }
                OnContactsReceived?.Invoke(users!);

            }
        }
        public void AddContact(int targetUserId) 
        {
            if (!SendRequest("AddContact", new { TargetUserId = targetUserId }))
                OnContactAddedResult?.Invoke(false, "Нет соединения с сервером.");
        }
        public void DeleteContact(int targetUserId) 
        {
            if (!SendRequest("DeleteContact", new { TargetUserId = targetUserId }))
                OnContactDeletedResult?.Invoke(false, targetUserId);
        }
        public void LoadContactsList() 
        {
            if (!SendRequest("LoadContactsList", new { }))
                OnContactsReceived?.Invoke(new List<User>());
        }
        public void UpdateContact(int userId, string updatedName) 
        {
            if (!SendRequest("UpdateContact", new { UserId = userId, UpdatedName = updatedName }))
                OnContactUpdated?.Invoke(false, userId, updatedName);
        }

        // Black list Management
        public void AddToBlacklist(int targetUserId) { }
        public void RemoveFromBlacklist(int targetUserId) { }
        public void LoadBlacklist() { }

        // Chats and Messages
        public void SendMessage(int chatId, string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText)) return;

            SendRequest("SendMessage", new
            {
                ChatId = chatId,
                MessageText = messageText
            });
        }

        public void LoadChatHistory(int chatId, int lastMessageId = 0)
        {
            if (!SendRequest("LoadChatHistory", new { ChatId = chatId, LastMessageId = lastMessageId }))
                OnHistoryReceived?.Invoke(new List<Message>());
        }

        public void CreateGroupChat(string groupName, List<int> memberIds)
        {
            if (memberIds == null) memberIds = new List<int>();

            SendRequest("CreateGroupChat", new
            {
                GroupName = groupName,
                MemberIds = memberIds
            });
        }

        public void SearchInChat(int chatId, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                OnHistoryReceived?.Invoke(new List<Message>());
                return;
            }

            if (!SendRequest("SearchInChat", new { ChatId = chatId, SearchQuery = searchQuery }))
                OnHistoryReceived?.Invoke(new List<Message>());
        }

        public void GlobalMessageSearch(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                OnHistoryReceived?.Invoke(new List<Message>());
                return;
            }

            if (!SendRequest("GlobalMessageSearch", new { SearchQuery = searchQuery }))
                OnHistoryReceived?.Invoke(new List<Message>());
        }

        public void LoadChatsList()
        {
            if (!SendRequest("LoadChatsList", new { }))
                OnChatsListReceived?.Invoke(new List<Chat>());
        }

        // Account`s Settings
        public void ChangePassword(string oldPassword, string newPassword) { }
        public void ChangeMyUsername(string newUsername) { }


        // вспомогательные функции

        private bool SendRequest(string type, object payload)
        {
            if (!isConnected || _writer == null)
            {
                Console.WriteLine($"Нельзя отправить {type}: нет соединения с сервером.");
                return false;
            }

            try
            {
                string internalJson = payload == null ? "" : JsonSerializer.Serialize(payload, _jsonOptions);

                var envelope = new NetworkEnvelope
                {
                    Type = type,
                    Payload = internalJson
                };
                string finalJson = JsonSerializer.Serialize(envelope, _jsonOptions);

                _writer.WriteLine(finalJson);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки запроса {type}: {ex.Message}");
                return false;
            }
        }

        private T DeserializePayload<T>(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return default(T);
            return JsonSerializer.Deserialize<T>(payload, _jsonOptions);
        }

        private List<T> DeserializeListPayload<T>(string payload, params string[] propertyNames)
        {
            if (string.IsNullOrWhiteSpace(payload)) return new List<T>();

            try
            {
                // Вариант 1: Payload сразу является массивом: [{...}, {...}]
                var directList = JsonSerializer.Deserialize<List<T>>(payload, _jsonOptions);
                if (directList != null) return directList;
            }
            catch
            {
                // Если это не массив, ниже пробуем разобрать как объект-обертку.
            }

            try
            {
                // Вариант 2: Payload является объектом: { "Users": [{...}, {...}] }
                using (var document = JsonDocument.Parse(payload))
                {
                    if (document.RootElement.ValueKind != JsonValueKind.Object)
                        return new List<T>();

                    foreach (string propertyName in propertyNames)
                    {
                        if (document.RootElement.TryGetProperty(propertyName, out JsonElement element))
                        {
                            var list = JsonSerializer.Deserialize<List<T>>(element.GetRawText(), _jsonOptions);
                            return list ?? new List<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга списка: {ex.Message}");
            }

            return new List<T>();
        }

        public async void AddContactByNickname(string targetUsername)
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

            await SearchContacts(targetUsername);
        }
    }
}
