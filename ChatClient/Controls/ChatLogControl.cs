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
    public partial class ChatLogControl : UserControl
    {
        private Main _mainForm;
        private int _chatId;

        // Храним список сообщений этого чата в памяти для локального поиска
        private List<Message> _currentChatMessages = new List<Message>();

        public ChatLogControl(Main mainForm, int chatId)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _chatId = chatId;

            Program.NetworkClient.OnMessageReceived += NetworkClient_OnMessageReceived;
            Program.NetworkClient.OnHistoryReceived += NetworkClient_OnHistoryReceived;

            Program.NetworkClient.LoadChatHistory(_chatId);
        }

        private void NetworkClient_OnHistoryReceived(List<Message> messages)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _currentChatMessages = messages;
                DisplayMessages(_currentChatMessages);
            });
        }

        private void NetworkClient_OnMessageReceived(Message msg)
        {
            // Проверяем: это сообщение прилетело ИМЕННО в тот чат, который сейчас открыт на экране
            // В нас немає чатів є лише контакти. Тому я тут змінив код
            // часу занадто мало тепер
            if (msg.ContactId == _chatId)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    _currentChatMessages.Add(msg);

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
            string text = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            Program.NetworkClient.SendMessage(_chatId, text);
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
                DisplayMessages(_currentChatMessages);
            }
            else
            {
                var filtered = _currentChatMessages
                    .Where(m => m.Text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                m.Sender.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
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
            rtbChatHistory.AppendText($"{msg.Sender.Nickname}: ");

            rtbChatHistory.SelectionFont = new Font(rtbChatHistory.Font, FontStyle.Regular);
            rtbChatHistory.AppendText($"{msg.Text}\r\n");

            rtbChatHistory.ScrollToCaret();
        }

        private void Control_Disposed(object sender, ControlEventArgs e)
        {
            Program.NetworkClient.OnMessageReceived -= NetworkClient_OnMessageReceived;
            Program.NetworkClient.OnHistoryReceived -= NetworkClient_OnHistoryReceived;
        }
    }
}
