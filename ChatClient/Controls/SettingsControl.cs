using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Controls
{
    public partial class SettingsControl : UserControl
    {
        private Main _mainForm;

        public SettingsControl(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            lblLoggedInAs.Text = $"You are logged in as: {_mainForm.CurrentUsername}";

            string ip = Program.NetworkClient.ServerIP ?? "127.0.0.1";
            int port = Program.NetworkClient.ServerPort != 0 ? Program.NetworkClient.ServerPort : 6767;

            lblIpAndPort.Text = $"IP and Port: {ip}:{port}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Выход",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _mainForm.IsLoggingOut = true;

                Program.NetworkClient.Disconnect();

                _mainForm.Close();

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string oldPass = txtOldPassword.Text;
            string newPass = txtNewPassword.Text;

            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Заполните поля паролей!");
                return;
            }

            Program.NetworkClient.ChangePassword(oldPass, newPass);
            MessageBox.Show("Запрос на смену пароля отправлен.");
        }

        private void btnUpdateUsername_Click(object sender, EventArgs e)
        {
            string newNick = txtNewUsername.Text.Trim();

            if (string.IsNullOrEmpty(newNick)) return;

            Program.NetworkClient.ChangeMyUsername(newNick);
        }
    }
}
