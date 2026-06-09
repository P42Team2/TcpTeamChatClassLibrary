namespace ChatClient
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMainContent = new Panel();
            pnlSidebar = new Panel();
            label1 = new Label();
            btnContacts = new Button();
            groupBox1 = new GroupBox();
            lvActiveChats = new ListView();
            btnAccountSettings = new Button();
            lblLoggedInAs = new Label();
            btnBlackList = new Button();
            pnlSidebar.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMainContent
            // 
            pnlMainContent.BorderStyle = BorderStyle.FixedSingle;
            pnlMainContent.Dock = DockStyle.Right;
            pnlMainContent.Location = new Point(202, 0);
            pnlMainContent.Name = "pnlMainContent";
            pnlMainContent.Size = new Size(598, 450);
            pnlMainContent.TabIndex = 0;
            // 
            // pnlSidebar
            // 
            pnlSidebar.BorderStyle = BorderStyle.FixedSingle;
            pnlSidebar.Controls.Add(label1);
            pnlSidebar.Controls.Add(btnContacts);
            pnlSidebar.Controls.Add(groupBox1);
            pnlSidebar.Controls.Add(btnAccountSettings);
            pnlSidebar.Controls.Add(lblLoggedInAs);
            pnlSidebar.Controls.Add(btnBlackList);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(200, 450);
            pnlSidebar.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(135, 15);
            label1.TabIndex = 5;
            label1.Text = "Welcome to our project!";
            // 
            // btnContacts
            // 
            btnContacts.Location = new Point(3, 29);
            btnContacts.Name = "btnContacts";
            btnContacts.Size = new Size(192, 24);
            btnContacts.TabIndex = 1;
            btnContacts.Text = "Contacts";
            btnContacts.UseVisualStyleBackColor = true;
            btnContacts.Click += btnContacts_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lvActiveChats);
            groupBox1.Location = new Point(3, 85);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(192, 304);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chats";
            // 
            // lvActiveChats
            // 
            lvActiveChats.FullRowSelect = true;
            lvActiveChats.Location = new Point(3, 19);
            lvActiveChats.Name = "lvActiveChats";
            lvActiveChats.Size = new Size(186, 274);
            lvActiveChats.TabIndex = 1;
            lvActiveChats.UseCompatibleStateImageBehavior = false;
            lvActiveChats.View = View.Details;
            lvActiveChats.SelectedIndexChanged += lvActiveChats_SelectedIndexChanged;
            // 
            // btnAccountSettings
            // 
            btnAccountSettings.Location = new Point(3, 410);
            btnAccountSettings.Name = "btnAccountSettings";
            btnAccountSettings.Size = new Size(192, 35);
            btnAccountSettings.TabIndex = 3;
            btnAccountSettings.Text = "Acccount settings";
            btnAccountSettings.UseVisualStyleBackColor = true;
            btnAccountSettings.Click += btnAccountSettings_Click;
            // 
            // lblLoggedInAs
            // 
            lblLoggedInAs.AutoSize = true;
            lblLoggedInAs.Location = new Point(3, 392);
            lblLoggedInAs.Name = "lblLoggedInAs";
            lblLoggedInAs.Size = new Size(116, 15);
            lblLoggedInAs.TabIndex = 0;
            lblLoggedInAs.Text = "You are logged in as:";
            // 
            // btnBlackList
            // 
            btnBlackList.Location = new Point(3, 55);
            btnBlackList.Name = "btnBlackList";
            btnBlackList.Size = new Size(192, 24);
            btnBlackList.TabIndex = 2;
            btnBlackList.Text = "Black List";
            btnBlackList.UseVisualStyleBackColor = true;
            btnBlackList.Click += btnBlackList_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlMainContent);
            Name = "Main";
            Text = "Form1";
            FormClosed += Main_FormClosed;
            Load += Main_Load;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMainContent;
        private Panel pnlSidebar;
        private Button btnBlackList;
        private Button btnContacts;
        private Button btnAccountSettings;
        private Label lblLoggedInAs;
        private Label label1;
        private GroupBox groupBox1;
        private ListBox lstActiveChats;
        private ListView lvActiveChats;
    }
}
