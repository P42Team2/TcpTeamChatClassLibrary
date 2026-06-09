using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class AddContactForm : Form
    {
        public string InputNickname { get; private set; }
        public AddContactForm()
        {
            InitializeComponent();
            // Чтобы форма открывалась ровно по центру родительского окна
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string nick = txtNickname.Text.Trim();

            if (string.IsNullOrEmpty(nick))
            {
                MessageBox.Show("Введите никнейм пользователя!", "Внимание");
                return;
            }

            // Сохраняем результат и закрываем форму с успешным статусом
            InputNickname = nick;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
