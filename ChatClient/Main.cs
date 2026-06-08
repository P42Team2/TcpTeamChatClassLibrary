using ChatClient.Controls;

namespace ChatClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            SwitchScreen(new WelcomeControl());
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
