namespace ChatClient.Controls
{
    partial class ContactsManagementControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtSearch = new TextBox();
            panel1 = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            btnToChat = new Button();
            btnAdd = new Button();
            btnBlock = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            label2 = new Label();
            lvContacts = new ListView();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 15);
            label1.Name = "label1";
            label1.Size = new Size(111, 20);
            label1.TabIndex = 9;
            label1.Text = "Search Contact:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 32);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(193, 27);
            txtSearch.TabIndex = 8;
            txtSearch.TextChanged += txtSearchContact_TextChanged;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(btnToChat);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnBlock);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(455, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(228, 600);
            panel1.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 158);
            label5.Name = "label5";
            label5.Size = new Size(145, 20);
            label5.TabIndex = 18;
            label5.Text = "та натисніть To chat";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 138);
            label4.Name = "label4";
            label4.Size = new Size(152, 20);
            label4.TabIndex = 17;
            label4.Text = "оберіть користувача";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(-1, 118);
            label3.Name = "label3";
            label3.Size = new Size(211, 20);
            label3.TabIndex = 16;
            label3.Text = "Щоб почати переписуватись";
            // 
            // btnToChat
            // 
            btnToChat.Location = new Point(3, 67);
            btnToChat.Margin = new Padding(3, 4, 3, 4);
            btnToChat.Name = "btnToChat";
            btnToChat.Size = new Size(219, 47);
            btnToChat.TabIndex = 14;
            btnToChat.Text = "To chat";
            btnToChat.UseVisualStyleBackColor = true;
            btnToChat.Click += btnToChat_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(3, 383);
            btnAdd.Margin = new Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(219, 47);
            btnAdd.TabIndex = 13;
            btnAdd.Text = "Add ➕";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnBlock
            // 
            btnBlock.Location = new Point(3, 547);
            btnBlock.Margin = new Padding(3, 4, 3, 4);
            btnBlock.Name = "btnBlock";
            btnBlock.Size = new Size(219, 47);
            btnBlock.TabIndex = 12;
            btnBlock.Text = "Block 🚫";
            btnBlock.UseVisualStyleBackColor = true;
            btnBlock.Click += btnBlock_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(3, 492);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(219, 47);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "Delete ❌";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(3, 437);
            btnEdit.Margin = new Padding(3, 4, 3, 4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(219, 47);
            btnEdit.TabIndex = 10;
            btnEdit.Text = "Edit ✏️";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(15, 16);
            label2.Name = "label2";
            label2.Size = new Size(98, 28);
            label2.TabIndex = 11;
            label2.Text = "Contacts";
            // 
            // lvContacts
            // 
            lvContacts.FullRowSelect = true;
            lvContacts.Location = new Point(0, 55);
            lvContacts.Margin = new Padding(3, 4, 3, 4);
            lvContacts.Name = "lvContacts";
            lvContacts.Size = new Size(447, 544);
            lvContacts.TabIndex = 12;
            lvContacts.UseCompatibleStateImageBehavior = false;
            lvContacts.View = View.Details;
            lvContacts.SelectedIndexChanged += lvContacts_SelectedIndexChanged;
            // 
            // ContactsManagementControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lvContacts);
            Controls.Add(label2);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ContactsManagementControl";
            Size = new Size(683, 600);
            ControlRemoved += ContactsManagementControl_Disposed;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSearch;
        private Panel panel1;
        private Button btnBlock;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private Label label2;
        private ListView lvContacts;
        private Label label3;
        private Button btnToChat;
        private Label label5;
        private Label label4;
    }
}
