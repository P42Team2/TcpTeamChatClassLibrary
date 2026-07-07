using System;
using System.Windows.Forms;

namespace ChatClient
{
    internal static class Program
    {
        public static NetworkClient NetworkClient { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            NetworkClient = new NetworkClient();

            NetworkClient.Connect("127.0.0.1", 10000);

            LoginForm loginForm = new LoginForm();
            Application.Run(loginForm);
        }
    }
}