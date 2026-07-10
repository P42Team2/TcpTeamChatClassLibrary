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
    public partial class LogIn : UserControl
    {
        private LoginForm _parentForm;

        public LogIn(LoginForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;

            Program.NetworkClient.OnLoginResult += NetworkClient_OnLoginResult;
        }

        private async void btnLogIn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await Program.NetworkClient.Login(username, password);
        }

        private void NetworkClient_OnLoginResult(bool success, string message)
        {
            if (success)
            {
                int userId = int.Parse(message);

                _parentForm.OnLoginSuccess(txtUsername.Text.Trim(), userId);
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"Ошибка авторизации: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void LogIn_Disposed(object sender, ControlEventArgs e)
        {
            Program.NetworkClient.OnLoginResult -= NetworkClient_OnLoginResult;
        }
    }
}
