using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpTeamChatClassLibrary.Models;

namespace ChatClient.Controls
{
    public partial class BlacklistControl : UserControl
    {
        private Main _mainForm;
        public BlacklistControl(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            Program.NetworkClient.OnBlacklistReceived += NetworkClient_OnBlacklistReceived;
            Program.NetworkClient.OnBlacklistChangedResult += NetworkClient_OnBlacklistChangedResult;

            Program.NetworkClient.LoadBlacklist();
        }

        private void NetworkClient_OnBlacklistReceived(List<User> blockedUsers)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lvBlacklist.Items.Clear();
                foreach (var user in blockedUsers)
                {
                    ListViewItem item = new ListViewItem(user.Nickname);
                    item.Tag = user;
                    lvBlacklist.Items.Add(item);
                }
            });
        }

        private void btnUnblock_Click(object sender, EventArgs e)
        {
            if (lvBlacklist.SelectedItems.Count > 0)
            {
                User selectedUser = (User)lvBlacklist.SelectedItems[0].Tag;

                // Отправляем запрос на удаление из ЧС
                Program.NetworkClient.RemoveFromBlacklist(selectedUser.Id);
            }
        }

        private void NetworkClient_OnBlacklistChangedResult(bool success, int targetUserId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (success)
                {
                    foreach (ListViewItem item in lvBlacklist.Items)
                    {
                        if (item.Tag is User u && u.Id == targetUserId)
                        {
                            item.Remove();
                            break;
                        }
                    }
                }
            });
        }

        private void Control_Disposed(object sender, ControlEventArgs e)
        {
            Program.NetworkClient.OnBlacklistReceived -= NetworkClient_OnBlacklistReceived;
            Program.NetworkClient.OnBlacklistChangedResult -= NetworkClient_OnBlacklistChangedResult;
        }
    }
}
