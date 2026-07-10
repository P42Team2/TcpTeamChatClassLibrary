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
    public partial class ContactsManagementControl : UserControl
    {
        private Main _mainForm;

        public ContactsManagementControl(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            Program.NetworkClient.OnContactsReceived += NetworkClient_OnContactsReceived;

            lvContacts.View = View.Details;

            lvContacts.Columns.Add("Id", 35);
            lvContacts.Columns.Add("Nickname", 120);
            lvContacts.Columns.Add("Login", 100);
            lvContacts.Columns.Add("Last seen", 120);
            
            _ = Task.Run(async () => { await Task.Delay(500); Program.NetworkClient.LoadContactsList(); });
        }

        private void ContactsManagementControl_Disposed(object sender, EventArgs e)
        {
            Program.NetworkClient.OnContactsReceived -= NetworkClient_OnContactsReceived;
        }

        private void txtSearchContact_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();

            if (query.Length >= 2)
            {
                Task.Run(() => Program.NetworkClient.SearchContacts(query));
            }
        }

        private void NetworkClient_OnContactsReceived(List<User> users)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lvContacts.Items.Clear();

                DateTime today = DateTime.Today;

                foreach (var user in users)
                {
                    ListViewItem item = new ListViewItem(user.Id.ToString());
                    item.SubItems.Add(user.Nickname);
                    item.SubItems.Add(user.Login);
                    if (user.Status == UserStatus.Online)
                    {
                        item.SubItems.Add(user.Status.ToString());
                    }
                    else
                    {
                        if (user.LastSeen.Date == today)
                        {
                            item.SubItems.Add(user.LastSeen.ToString("HH:mm"));
                        }
                        else
                        {
                            item.SubItems.Add(user.LastSeen.ToString("dd.MM.yyyy"));
                        }
                    }

                    item.Tag = user;
                    lvContacts.Items.Add(item);
                }
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (AddContactForm addForm = new AddContactForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    string nickToSend = addForm.InputNickname;

                    Program.NetworkClient.AddContactByNickname(nickToSend);

                    MessageBox.Show($"Запрос на добавление {nickToSend} отправлен!");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count > 0)
            {
                User selectedUser = (User)lvContacts.SelectedItems[0].Tag;

                using (EditContactForm editForm = new EditContactForm(selectedUser.Nickname))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        string updatedName = editForm.NewNickname;

                        Program.NetworkClient.UpdateContact(selectedUser.Id, updatedName);

                        selectedUser.Nickname = updatedName;
                        lvContacts.SelectedItems[0].Text = updatedName;

                        MessageBox.Show("Контакт успешно изменен!");
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count > 0)
            {
                User selectedUser = (User)lvContacts.SelectedItems[0].Tag;

                Program.NetworkClient.DeleteContact(selectedUser.Id);

                lvContacts.SelectedItems[0].Remove();
            }
            else
            {
                MessageBox.Show("Выберите контакт для удаления!");
            }
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count > 0)
            {
                User selectedUser = (User)lvContacts.SelectedItems[0].Tag;

                Program.NetworkClient.AddToBlacklist(selectedUser.Id);
                lvContacts.SelectedItems[0].Remove();
            }
            else
            {
                MessageBox.Show("Выберите пользователя для блокировки!");
            }
        }

        private void ContactsManagementControl_Disposed(object sender, ControlEventArgs e)
        {

        }

        private void lvContacts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnToChat_Click(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count > 0)
            {
                User selectedUser = (User)lvContacts.SelectedItems[0].Tag;
                _mainForm.SwitchScreen(new ContactLogControl(_mainForm, selectedUser.Id));
            }
        }
    }
}
