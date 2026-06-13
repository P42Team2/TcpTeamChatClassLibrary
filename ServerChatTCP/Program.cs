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
                "logs/app-.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        private static ChatDB_Context _context=new ChatDB_Context();

        
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

                Console.WriteLine($"[+] Client connected: {client.Client.RemoteEndPoint}");

                _ = Task.Run(() => HandleClient(client));
            }

            void HandleClient(TcpClient client)
            {
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

                        Console.WriteLine($"Received: {jsonRequest}");

                        NetworkRequest? clientRequest = JsonSerializer.Deserialize<NetworkRequest>(jsonRequest);

                        if (clientRequest == null)
                            continue;

                        switch (clientRequest.Type)
                        {
                            case RequestType.Login:
                                {
                                    Console.WriteLine("Login request");

                                    LoginAndRegisterRequest? dataLogin = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Data);

                                    if (dataLogin == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.LoginError, JsonSerializer.Serialize("Invalid data"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    // тут нужна проверка как раз данных через бд, с ентити сами допилите
                                    // если неправильно или не найдено - то ResponseType.LoginError, если найдено - ResponseType.LoginSuccess
                                    User? acountUser = _context.Users.FirstOrDefault(u => u.Login == dataLogin.Username);
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
                                    onlineUsers[acountUser.Id] = client;

                                    //writer.WriteLine(JsonSerializer.Serialize());

                                    break;
                                }

                            case RequestType.Register:
                                {
                                    Console.WriteLine("Register request");

                                    LoginAndRegisterRequest? dataRegister = JsonSerializer.Deserialize<LoginAndRegisterRequest>(clientRequest.Data);

                                    if (dataRegister == null)
                                    {
                                        var errorResponse = new NetworkResponse(ResponseType.RegisterError, JsonSerializer.Serialize("Invalid data"));

                                        writer.WriteLine(JsonSerializer.Serialize(errorResponse));
                                        break;
                                    }

                                    // тоже самое: если в бд нашли dataRegister.Username - то ошибка, если не нашли, то создаёте нового юзера

                                    break;
                                }

                            case RequestType.SendMessage:
                                {
                                    Console.WriteLine("Send message request");

                                    SendMessageRequest? msg = JsonSerializer.Deserialize<SendMessageRequest>(clientRequest.Data);

                                    if (msg == null)
                                        break;

                                    //
                                    // сохранить сообщение в бд
                                    //

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

                                Console.WriteLine("Add contact request");


                                break;

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
                    Console.WriteLine("Client connection closed");
                }
            }
        }
    }
}