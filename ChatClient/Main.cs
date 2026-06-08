using ChatClient.Controls;

namespace ChatClient
{
    public partial class Main : Form
    {
        public string CurrentUsername { get; private set; }
        public int CurrentUserId { get; private set; }

        public Main()
        {
            InitializeComponent();
            SwitchScreen(new WelcomeControl());
        }

        public Main(string username, int userId)
        {
            InitializeComponent();
            SwitchScreen(new WelcomeControl());

            CurrentUsername = username;
            CurrentUserId = userId;

            lblLoggedInAs.Text = username;
        }

        public void SwitchScreen(UserControl newScreen)
        {
            pnlMainContent.Controls.Clear();
            newScreen.Dock = DockStyle.Fill;
            pnlMainContent.Controls.Add(newScreen);
        }

        private void btnContacts_Click(object sender, EventArgs e)
        {
            SwitchScreen(new ContactsManagementControl());
        }

        private void btnBlackList_Click(object sender, EventArgs e)
        {
            SwitchScreen(new BlacklistControl());
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {
            SwitchScreen(new SettingsControl());
        }
    }
}
