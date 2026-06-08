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
            LoginForm loginForm = new LoginForm();

            Application.Run(loginForm);
        }
    }
}