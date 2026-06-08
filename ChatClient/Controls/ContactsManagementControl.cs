using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient.Controls
{
    public partial class ContactsManagementControl : UserControl
    {
        private Main _mainForm;

        public ContactsManagementControl(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            Program.NetworkClient.OnContactsReceived += NetworkClient_OnContactsReceived;
        }

        private void txtSearchContact_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();

            if (query.Length >= 2)
            {
                Program.NetworkClient.SearchContacts(query);
            }
        }

        private void NetworkClient_OnContactsReceived(List<User> users)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lvContacts.Items.Clear();

                foreach (var user in users)
                {
                    ListViewItem item = new ListViewItem(user.Nickname);
                    item.Tag = user;
                    lvContacts.Items.Add(item);
                }
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnBlock_Click(object sender, EventArgs e)
        {

        }
    }
}
