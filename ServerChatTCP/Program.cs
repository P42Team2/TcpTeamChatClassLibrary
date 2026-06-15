using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TcpTeamChatClassLibrary.Models.DTO;
using TcpTeamChatClassLibrary.Models.NetworkMessage;
//нові бібліотеки
using ChatClient.Models;
using Serilog;
using Serilog.Core;

namespace Server
{
    internal class Program
    {
        // логер, записує у файл та у консоль помилки
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

        static void Main()
        {
            listener.Start();

            Console.WriteLine($"Server started on port {localPort}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                _logInfo.Information($"[+] Client connected: {client.Client.RemoteEndPoint}");

                _ = Task.Run(() => HandleClient(client));
            }

            void HandleClient(TcpClient client)
            {
                int currentUserId = -1;// якщо від'ємне значення змінної то вона вважається не ініціалізованою
                ChatDB_Context context = new ChatDB_Context();
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

                        NetworkRequest? clientRequest = JsonSerializer.Deserialize<NetworkRequest>(jsonRequest);

                        if (clientRequest == null)
                            continue;

                        switch (clientRequest.Type)
                        {
                            case RequestType.Login:
                                {
                                    Console.WriteLine("Login request");

                                    LoginAndRegisterRequest? dataLogin = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Payload);

                                    if (dataLogin == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, JsonSerializer.Serialize("Invalid data"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    // если неправильно или не найдено - то ResponseType.LoginError, если найдено - ResponseType.LoginSuccess
                                    User? acountUser = context.Users.FirstOrDefault(u => u.Login == dataLogin.Username);
                                    if (acountUser==null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, JsonSerializer.Serialize("Uncorrect Login. This accuont does not existing"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }
                                    else if(acountUser.Password != dataLogin.Password)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, JsonSerializer.Serialize("Uncorrect Password."));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    // после этого если вход успешный, должны вытянуть айди пользователя и записать в переменную, которая потом добавит его в дикшинари 
                                    currentUserId = acountUser.Id;
                                    lock(_lock)
                                    {
                                        context.Users.First(u=>u.Id==currentUserId).Status = UserStatus.Online;
                                        context.SaveChanges();
                                    }
                                    onlineUsers[currentUserId] = client;

                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.LoginSuccess, JsonSerializer.Serialize(acountUser))));

                                    break;
                                }

                            case RequestType.Register:
                                {
                                    Console.WriteLine("Register request");

                                    LoginAndRegisterRequest? dataRegister = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Payload);

                                    if (dataRegister == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.RegisterError, JsonSerializer.Serialize("Invalid data"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }
                                    
                                    User? acountUser = context.Users.FirstOrDefault(u => u.Login == dataRegister.Username);
                                    if (acountUser != null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.RegisterError, JsonSerializer.Serialize("An account with this login has already been created."));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }
                                    acountUser = new User { Login=dataRegister.Username, Password=dataRegister.Password, Status=UserStatus.Online};
                                    acountUser.SetIpAndPort(client.Client);

                                    lock(_lock)
                                    {
                                        context.Users.Add(acountUser);
                                        context.SaveChanges();
                                    }
                                    onlineUsers[acountUser.Id] = client;
                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.RegisterSuccess, JsonSerializer.Serialize(acountUser))));
                                    // тоже самое: если в бд нашли dataRegister.Username - то ошибка, если не нашли, то создаёте нового юзера

                                    break;
                                }

                            case RequestType.SendMessage:
                                {
                                    Console.WriteLine("Send message request");

                                    SendMessageRequest? msg = JsonSerializer.Deserialize<SendMessageRequest>(clientRequest.Payload);

                                    if (msg == null)
                                        break;

                                    if (!context.Users.Any(u=>u.Id==msg.ReceiverId))
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.MessageError, JsonSerializer.Serialize("Accuont of receiver does not existing"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    lock(_lock)
                                    {
                                        context.Messages.Add(new Message { Text=msg.Text, ReceiverId=msg.ReceiverId, SenderId=msg.SenderId, TimeWhenMessageSended=DateTime.Now});
                                        context.SaveChanges();
                                    }

                                    if (onlineUsers.TryGetValue(msg.ReceiverId, out TcpClient? receiverClient))
                                    {
                                        try
                                        {
                                            NetworkStream nsReceiver = receiverClient.GetStream();
                                            StreamWriter writerReceiver = new StreamWriter(nsReceiver);
                                            writerReceiver.AutoFlush = true;

                                            var response = new NetworkResponse(ResponseType.MessageReceived, JsonSerializer.Serialize(msg));

                                            string json = JsonSerializer.Serialize(response);

                                            writerReceiver.WriteLine(json);

                                            Console.WriteLine("Message sent instantly to online user");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("User offline (только сохранено в бд)");

                                        // соо сохраняеться в бд всегда, поэтому когда пользователь которому ты это послал запустит прогу, оно должно выкачать все смс с бд 
                                    }

                                    break;
                                }

                            case RequestType.AddContact:
                                {
                                    if (currentUserId<0)
                                    {
                                        break;
                                    }
                                    Console.WriteLine("Add contact request");
                                    var request = JsonSerializer.Deserialize<ContactRequest>(clientRequest.Payload);
                                    User? userContact = null;

                                    if (request?.Id != null)
                                    {
                                        userContact = context.Users
                                            .FirstOrDefault(u => u.Id == request.Id);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(request?.Login))
                                    {
                                        userContact = context.Users
                                            .FirstOrDefault(u => u.Login == request.Login);
                                    }
                                    else
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UnexpectedError, JsonSerializer.Serialize("uncorrect payload"));

                                        _logWarring.Fatal("Error in RequestType.AddContact. Uncorrect payload. ResponseType.UnexpectedError. \n" +
                                            $"client request: {clientRequest.Payload}" +
                                            $"\n request: id {request?.Id} | login {request?.Login}\n");

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    if (userContact == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.UserDoesNotExist, JsonSerializer.Serialize("An account with this login/id does not existing."));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    Contact contact = new Contact { OwnerUserId=currentUserId, ContactUserId=userContact.Id, DisplayName=null, AddedAt=DateTime.Now};

                                    lock (_lock)
                                    {
                                        context.Contacts.Add(contact);
                                        context.SaveChanges();
                                    }

                                    writer.WriteLine(JsonSerializer.Serialize(new NetworkResponse(ResponseType.SuccessContactRequest, JsonSerializer.Serialize(contact))));

                                    break;
                                    // чем дальше в лес - if else if else
                                }
                            case RequestType.DeleteContact:

                                Console.WriteLine("Delete contact request");


                                break;

                            case RequestType.AddToBlacklist:

                                Console.WriteLine("Add to blacklist request");


                                break;

                            case RequestType.RemoveFromBlacklist:

                                Console.WriteLine("Remove from blacklist request");


                                break;

                            case RequestType.LoadBlacklist:

                                Console.WriteLine("Load blacklist request");


                                break;

                            case RequestType.LoadChatHistory:

                                Console.WriteLine("Load history request");


                                break;

                            case RequestType.CreateGroupChat:

                                Console.WriteLine("Create group request");

                                break;

                            case RequestType.SearchContacts:

                                Console.WriteLine("Search contacts request");


                                break;

                            case RequestType.SearchInChat:

                                Console.WriteLine("Search in chat request");


                                break;

                            case RequestType.GlobalMessageSearch:

                                Console.WriteLine("Global search request");


                                break;

                            default:

                                Console.WriteLine("Unknown request");

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // воно записує у файл та у консоль інформацію про помилки
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
                    Console.WriteLine("Client connection closed");
                }
            }
        }
    }
}