using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using TcpTeamChatClassLibrary.Models.DTO;
using TcpTeamChatClassLibrary.Models.NetworkMessage;
using TcpTeamChatClassLibrary.Models;
using Serilog;
using Serilog.Core;

namespace Server
{
    internal class Program
    {
        // Логер, записывает в файл и в консоль ошибки
        private static Logger _logWarring = new LoggerConfiguration().MinimumLevel.Warning()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/warlog/app-.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        private static Logger _logInfo = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/infolog/app-.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        private static object _lock = new object();
        internal static ConcurrentDictionary<int, TcpClient> onlineUsers = new();
        internal static int localPort = 10000;
        internal static TcpListener listener = new TcpListener(IPAddress.Any, localPort);
        internal static ChatDB_Context context = new ChatDB_Context();

        // Единая правильная настройка для работы с JSON, исправляющая баг синхронизации
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        static void Main()
        {
            listener.Start();
            _logInfo.Information($"Server started on port {localPort}");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                _logInfo.Information($"[+] Client connected: {client.Client.RemoteEndPoint}");
                _ = Task.Run(() => HandleClient(client));
            }
        }

        static void HandleClient(TcpClient client)
        {
            int currentUserId = -1; // Если отрицательное значение, то считается не инициализированным

            try
            {
                using NetworkStream ns = client.GetStream();
                using StreamReader reader = new StreamReader(ns);
                using StreamWriter writer = new StreamWriter(ns);
                writer.AutoFlush = true;

                while (client.Connected)
                {
                    string? jsonRequest = reader.ReadLine();
                    if (jsonRequest == null)
                        break;

                    _logInfo.Information($"Received: {jsonRequest}");

                    // ИСПРАВЛЕНИЕ БАГА: Передаем _jsonOptions для правильного маппинга JSON пакетов
                    using (JsonDocument doc = JsonDocument.Parse(jsonRequest))
                    {
                        JsonElement root = doc.RootElement;

                        string requestTypeStr = root.GetProperty("Type").GetString() ?? "";
                        string payloadStr = root.GetProperty("Payload").GetString() ?? "";

                        if (!Enum.TryParse<RequestType>(requestTypeStr, true, out RequestType parsedType))
                            continue;

                        var clientRequest = new { Type = parsedType, Payload = payloadStr };

                        switch (clientRequest.Type)
                        {
                            case RequestType.Login:
                                {
                                    _logInfo.Information("Login request");
                                    // ИСПРАВЛЕНИЕ БАГА: Передаем _jsonOptions во внутренний Payload
                                    LoginAndRegisterRequest? dataLogin = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Payload, _jsonOptions);

                                    if (dataLogin == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, "Invalid data", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    User? acountUser = context.Users.FirstOrDefault(u => u.Login == dataLogin.Username);
                                    if (acountUser == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, "Uncorrect Login. This account does not exist.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }
                                    else if (acountUser.Password != dataLogin.Password)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, "Uncorrect Password.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    currentUserId = acountUser.Id;
                                    lock (_lock)
                                    {
                                        context.Users.First(u => u.Id == currentUserId).Status = UserStatus.Online;
                                        context.SaveChanges();
                                    }
                                    onlineUsers[currentUserId] = client;
                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.LoginSuccess, acountUser, _jsonOptions), _jsonOptions));
                                    _logInfo.Information($"Succes login {dataLogin.Username}\n");
                                    break;
                                }

                            case RequestType.Register:
                                {
                                    _logInfo.Information("Register request");
                                    // ИСПРАВЛЕНИЕ БАГА: Передаем _jsonOptions во внутренний Payload регистрации
                                    LoginAndRegisterRequest? dataRegister = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Payload, _jsonOptions);

                                    if (dataRegister == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.RegisterError, "Invalid data", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    User? acountUser = context.Users.FirstOrDefault(u => u.Login == dataRegister.Username);
                                    if (acountUser != null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.RegisterError, "An account with this login has already been created.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    acountUser = new User
                                    {
                                        Login = dataRegister.Username,
                                        Password = dataRegister.Password,
                                        Status = UserStatus.Online,
                                        LastSeen = DateTime.Now,
                                        Nickname = dataRegister.Username
                                    };
                                    lock (_lock)
                                    {
                                        context.Users.Add(acountUser);
                                        context.SaveChanges();
                                    }
                                    onlineUsers[acountUser.Id] = client;
                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.RegisterSuccess, acountUser, _jsonOptions), _jsonOptions));
                                    _logInfo.Information($"Succes registration {dataRegister.Username}\n");
                                    break;
                                }

                            case RequestType.SendMessage:
                                {
                                    _logInfo.Information("Send message request");
                                    SendMessageRequest? msg = JsonSerializer.Deserialize<SendMessageRequest>(clientRequest.Payload, _jsonOptions);
                                    if (msg == null)
                                        break;

                                    if (!context.Users.Any(u => u.Id == msg.ReceiverId))
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.MessageError, "Account of receiver does not exist", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    Contact? contact = context.Contacts.FirstOrDefault(c =>
                                        (c.OwnerUserId == msg.ReceiverId && c.ContactUserId == msg.SenderId) ||
                                        (c.OwnerUserId == msg.SenderId && c.ContactUserId == msg.ReceiverId));

                                    if (contact == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.MessageError, "Contact does not exist", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    Message message = new Message { Text = msg.Text, ReceiverId = msg.ReceiverId, SenderId = msg.SenderId, TimeWhenMessageSended = DateTime.Now, ContactId = contact!.Id };
                                    lock (_lock)
                                    {
                                        context.Messages.Add(message);
                                        context.SaveChanges();
                                    }

                                    if (onlineUsers.TryGetValue(msg.ReceiverId, out TcpClient? receiverClient))
                                    {
                                        try
                                        {
                                            NetworkStream nsReceiver = receiverClient.GetStream();
                                            StreamWriter writerReceiver = new StreamWriter(nsReceiver);
                                            writerReceiver.AutoFlush = true;

                                            var response = new NetworkResponse(ResponseType.MessageReceived, message, _jsonOptions);
                                            string json = JsonSerializer.Serialize(response, _jsonOptions);
                                            writerReceiver.WriteLine(json);
                                            _logInfo.Information("Message sent instantly to online user");
                                        }
                                        catch (Exception ex)
                                        {
                                            _logInfo.Information(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        _logInfo.Information("User offline (только сохранено в бд)");
                                    }
                                    break;
                                }

                            case RequestType.AddContact:
                                {
                                    if (currentUserId < 0) break;
                                    _logInfo.Information("Add contact request");
                                    var request = JsonSerializer.Deserialize<ContactRequest>(clientRequest.Payload, _jsonOptions);
                                    User? userContact = null;

                                    if (request?.Id != null)
                                    {
                                        userContact = context.Users.FirstOrDefault(u => u.Id == request.Id);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(request?.Login))
                                    {
                                        userContact = context.Users.FirstOrDefault(u => u.Login == request.Login);
                                    }
                                    else
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UnexpectedError, "uncorrect payload", _jsonOptions);
                                        _logWarring.Fatal("Error in RequestType.AddContact. Uncorrect payload. \n client request: {Payload} \n request: id {Id} | login {Login}\n", clientRequest.Payload, request?.Id, request?.Login);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    if (userContact == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UserDoesNotExist, "An account with this login/id does not exist.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    Contact? contact = context.Contacts.FirstOrDefault(c =>
                                        (c.OwnerUserId == currentUserId && c.ContactUserId == userContact.Id) ||
                                        (c.OwnerUserId == userContact.Id && c.ContactUserId == currentUserId));

                                    if (contact != null)
                                    {
                                        writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.SuccessContactRequest, contact, _jsonOptions), _jsonOptions));
                                        break;
                                    }

                                    contact = new Contact { OwnerUserId = currentUserId, ContactUserId = userContact.Id, DisplayName = null, AddedAt = DateTime.Now };
                                    lock (_lock)
                                    {
                                        context.Contacts.Add(contact);
                                        context.SaveChanges();
                                    }
                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.SuccessContactRequest, contact, _jsonOptions), _jsonOptions));
                                    break;
                                }

                            case RequestType.DeleteContact:
                                {
                                    _logInfo.Information("Delete contact request");
                                    if (currentUserId < 0) break;
                                    var request = JsonSerializer.Deserialize<ContactRequest>(clientRequest.Payload, _jsonOptions);
                                    User? userContact = null;

                                    if (request?.Id != null)
                                    {
                                        userContact = context.Users.FirstOrDefault(u => u.Id == request.Id);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(request?.Login))
                                    {
                                        userContact = context.Users.FirstOrDefault(u => u.Login == request.Login);
                                    }
                                    else
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UnexpectedError, "uncorrect payload", _jsonOptions);
                                        _logWarring.Fatal("Error in RequestType.DeleteContact. Uncorrect payload. \n client request: {Payload} \n request: id {Id} | login {Login}\n", clientRequest.Payload, request?.Id, request?.Login);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    if (userContact == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UserDoesNotExist, "An account with this login/id does not exist.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }

                                    Contact? contact = context.Contacts.FirstOrDefault(c =>
                                        (c.OwnerUserId == currentUserId && c.ContactUserId == userContact.Id) ||
                                        (c.OwnerUserId == userContact.Id && c.ContactUserId == currentUserId));

                                    if (contact != null)
                                    {
                                        context.Contacts.Remove(contact);
                                        writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.SuccessContactRequest, "Contact was deleted", _jsonOptions), _jsonOptions));
                                        break;
                                    }
                                    break;
                                }

                            case RequestType.AddToBlacklist:
                                _logInfo.Information("Add to blacklist request");
                                break;
                            case RequestType.RemoveFromBlacklist:
                                _logInfo.Information("Remove from blacklist request");
                                break;
                            case RequestType.LoadBlacklist:
                                _logInfo.Information("Load blacklist request");
                                break;

                            case RequestType.LoadChatHistory:
                                {
                                    _logInfo.Information("Load history request");
                                    int contactId = JsonSerializer.Deserialize<int>(clientRequest.Payload, _jsonOptions);
                                    if (context.Contacts.Any(c => c.Id == contactId))
                                    {
                                        Message[] chatHistory = context.Contacts.First(c => c.Id == contactId).Messages.ToArray();
                                        writer.WriteLine(JsonSerializer.Serialize(chatHistory, _jsonOptions));
                                        break;
                                    }
                                    else
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UserDoesNotExist, "A contact with this id does not exist.", _jsonOptions);
                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse, _jsonOptions));
                                        break;
                                    }
                                }

                            case RequestType.CreateGroupChat:
                                _logInfo.Information("Create group request");
                                break;
                            case RequestType.SearchContacts:
                                _logInfo.Information("Search contacts request");
                                break;
                            case RequestType.SearchInChat:
                                _logInfo.Information("Search in chat request");
                                break;
                            case RequestType.GlobalMessageSearch:
                                _logInfo.Information("Global search request");
                                break;
                            default:
                                _logInfo.Information("Unknown request");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logWarring.Warning($"ERROR: {ex.Message}\n");
            }
            finally
            {
                client.Close();
                if (currentUserId > 0)
                {
                    onlineUsers.TryRemove(currentUserId, out _);
                    lock (_lock)
                    {
                        context.Users.First(u => u.Id == currentUserId).Status = UserStatus.Offline;
                        context.SaveChanges();
                    }
                }
                _logInfo.Information("Client connection closed");
            }
        }
    }
}