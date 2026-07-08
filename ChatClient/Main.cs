using ChatClient.Controls;
using TcpTeamChatClassLibrary.Models;

namespace ChatClient
{
    public partial class Main : Form
    {
        public string CurrentUsername { get; private set; }
        public int CurrentUserId { get; private set; }

        // Флаг: true, если юзер нажал Logout. По умолчанию false.
        public bool IsLoggingOut { get; set; } = false;

        public Main(string username, int userId)
        {
            InitializeComponent();
            CurrentUsername = username;
            CurrentUserId = userId;

            Program.NetworkClient.OnChatsListReceived += NetworkClient_OnChatsListReceived;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lblLoggedInAs.Text = $"You are logged in as: {CurrentUsername}";
            SwitchScreen(new WelcomeControl());

            Program.NetworkClient.LoadChatsList();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.NetworkClient.OnChatsListReceived -= NetworkClient_OnChatsListReceived;

            // Если мы НЕ в процессе логаута — значит, юзер реально закрыл программу крестиком
            if (!IsLoggingOut)
            {
                Application.Exit();
            }
        }

        public void SwitchScreen(UserControl newScreen)
        {
            pnlMainContent.Controls.Clear();
            newScreen.Dock = DockStyle.Fill;
            pnlMainContent.Controls.Add(newScreen);
        }

        private void btnContacts_Click(object sender, EventArgs e)
        {
            SwitchScreen(new ContactsManagementControl(this));
        }

        private void btnBlackList_Click(object sender, EventArgs e)
        {
            SwitchScreen(new BlacklistControl(this));
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {
            SwitchScreen(new SettingsControl(this));
        }

        private void lvActiveChats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvActiveChats.SelectedItems.Count > 0)
            {
                if (lvActiveChats.SelectedItems[0].Tag is Chat selectedChat)
                {
                    ChatLogControl chatScreen = new ChatLogControl(this, selectedChat.Id);
                    SwitchScreen(chatScreen);
                }
            }
        }

        private void NetworkClient_OnChatsListReceived(List<Chat> chats)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lvActiveChats.Items.Clear();
                foreach (var chat in chats)
                {
                    ListViewItem item = new ListViewItem(chat.Name);
                    item.Tag = chat;

                    lvActiveChats.Items.Add(item);
                }
            });
        }
    }
}
