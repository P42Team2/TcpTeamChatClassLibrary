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
using Message = TcpTeamChatClassLibrary.Models.Message;

namespace ChatClient.Controls
{
    public partial class ContactLogControl : UserControl
    {
        private Main _mainForm;
        private Contact? _contact = null;
        private CancellationTokenSource _isCancelledToken = new CancellationTokenSource();
        private int _receiverId;

        // Храним список сообщений этого чата в памяти для локального поиска
        private List<Message> _currentContactMessages = new List<Message>();

        public ContactLogControl(Main mainForm, int receiverId)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _receiverId = receiverId;

            Program.NetworkClient.OnMessageReceived += NetworkClient_OnMessageReceived;
            Program.NetworkClient.OnHistoryReceived += NetworkClient_OnHistoryReceived;
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);

                BeginInvoke(() =>
                {
                    _contact = Program.NetworkClient.LoadContactHistory(receiverId);
                    if (_contact == null)
                    {
                        lblContactName.Text = "NULL"; return;
                    }
                    lblContactName.Text = _contact.DisplayName;
                    NetworkClient_OnHistoryReceived(_contact.Messages.ToList());
                });

                while (true)
                {
                    await Task.Delay(1500);
                    _isCancelledToken.Token.ThrowIfCancellationRequested();
                    BeginInvoke(() =>
                    {
                        _contact = Program.NetworkClient.LoadContactHistory(_receiverId);
                        if (_contact != null)
                        {
                            NetworkClient_OnHistoryReceived(_contact.Messages.ToList());
                        }
                    });
                }
            });
        }

        private void NetworkClient_OnHistoryReceived(List<Message> messages)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _currentContactMessages = messages;
                DisplayMessages(_currentContactMessages);
            });
        }

        private void NetworkClient_OnMessageReceived(Message msg)
        {
            // Проверяем: это сообщение прилетело ИМЕННО в тот чат, который сейчас открыт на экране
            // В нас немає чатів є лише контакти. Тому я тут змінив код
            // часу занадто мало тепер
            if(_contact==null) return;

            if (msg.ContactId == _contact.Id)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    _currentContactMessages.Add(msg);

                    // Если в данный момент в поиске ничего не введено, просто дописываем его на экран
                    if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
                    {
                        AppendMessageToRichTextBox(msg);
                    }
                    else
                    {
                        // Если поиск активен — перерисовываем чат с учетом фильтра
                        FilterMessages(txtSearch.Text.Trim());
                    }
                });
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_contact == null) return;

            string text = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            Program.NetworkClient.SendMessage(_mainForm.CurrentUserId, _receiverId, text);
            txtInput.Clear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            FilterMessages(query);
        }

        private void FilterMessages(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                DisplayMessages(_currentContactMessages);
            }
            else
            {
                var filtered = _currentContactMessages
                    .Where(m => m.Text.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                DisplayMessages(filtered);
            }
        }

        private void DisplayMessages(List<Message> messagesList)
        {
            rtbChatHistory.Clear();

            foreach (var msg in messagesList)
            {
                AppendMessageToRichTextBox(msg);
            }
        }

        private void AppendMessageToRichTextBox(Message msg)
        {
            rtbChatHistory.SelectionFont = new Font(rtbChatHistory.Font, FontStyle.Bold);
            if (msg.SenderId == _mainForm.CurrentUserId)
            {
                rtbChatHistory.AppendText("You: ");
            }
            else
            {
                rtbChatHistory.AppendText($"{_contact!.DisplayName}: ");
            }
            rtbChatHistory.SelectionFont = new Font(rtbChatHistory.Font, FontStyle.Regular);
            rtbChatHistory.AppendText($"{msg.Text}\r\n");

            rtbChatHistory.ScrollToCaret();
        }

        private void Control_Disposed(object sender, ControlEventArgs e)
        {
            _isCancelledToken.Cancel();
            _isCancelledToken.Dispose();
            Program.NetworkClient.OnMessageReceived -= NetworkClient_OnMessageReceived;
            Program.NetworkClient.OnHistoryReceived -= NetworkClient_OnHistoryReceived;
        }
    }
}
