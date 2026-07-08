using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient.Controls;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            SwitchScreen(new LogIn(this));
        }

        public void SwitchScreen(UserControl newScreen)
        {
            pnlLogIn.Controls.Clear();
            newScreen.Dock = DockStyle.Fill;
            pnlLogIn.Controls.Add(newScreen);
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            SwitchScreen(new LogIn(this));
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            SwitchScreen(new Register(this));
        }

        public void OnLoginSuccess(string username, int userId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Main mainForm = new Main(username, userId);
                mainForm.Show();
                this.Hide();
            });
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Program.NetworkClient.Connect(txtIP.Text, int.Parse(txtPort.Text));
            label1.Text = Program.NetworkClient.isConnected.ToString();
        }
    }
}
