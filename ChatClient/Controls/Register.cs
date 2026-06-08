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
    public partial class Register : UserControl
    {
        private LoginForm _parentForm;

        public Register(LoginForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;

            Program.NetworkClient.OnRegisterResult += NetworkClient_OnRegisterResult;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Program.NetworkClient.Register(username, password);
        }

        private void NetworkClient_OnRegisterResult(bool success, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (success)
                {
                    MessageBox.Show("Регистрация успешна! Входим в аккаунт...", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string username = txtUsername.Text.Trim();
                    string password = txtPassword.Text;

                    LogIn loginScreen = new LogIn(_parentForm);
                    _parentForm.SwitchScreen(loginScreen);

                    Program.NetworkClient.Login(username, password);
                }
                else
                {
                    MessageBox.Show($"Ошибка регистрации: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
    }
}
