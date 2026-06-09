namespace ChatClient.Controls
{
    partial class ChatLogControl
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
            panel1 = new Panel();
            btnSend = new Button();
            txtInput = new TextBox();
            panel2 = new Panel();
            label1 = new Label();
            txtSearch = new TextBox();
            lblChatInfo = new Label();
            lblChatName = new Label();
            rtbChatHistory = new RichTextBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnSend);
            panel1.Controls.Add(txtInput);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 416);
            panel1.Name = "panel1";
            panel1.Size = new Size(598, 34);
            panel1.TabIndex = 0;
            // 
            // btnSend
            // 
            btnSend.Image = Properties.Resources.send__1_;
            btnSend.Location = new Point(550, 3);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(43, 23);
            btnSend.TabIndex = 1;
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtInput
            // 
            txtInput.Location = new Point(3, 3);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(541, 23);
            txtInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(lblChatInfo);
            panel2.Controls.Add(lblChatName);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(598, 64);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(433, 13);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 6;
            label1.Text = "Search:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(433, 26);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(142, 23);
            txtSearch.TabIndex = 5;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblChatInfo
            // 
            lblChatInfo.AutoSize = true;
            lblChatInfo.Location = new Point(15, 34);
            lblChatInfo.Name = "lblChatInfo";
            lblChatInfo.Size = new Size(70, 15);
            lblChatInfo.TabIndex = 3;
            lblChatInfo.Text = "Was Online:";
            // 
            // lblChatName
            // 
            lblChatName.AutoSize = true;
            lblChatName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChatName.Location = new Point(15, 13);
            lblChatName.Name = "lblChatName";
            lblChatName.Size = new Size(88, 21);
            lblChatName.TabIndex = 2;
            lblChatName.Text = "Chat Name";
            // 
            // rtbChatHistory
            // 
            rtbChatHistory.Location = new Point(4, 70);
            rtbChatHistory.Name = "rtbChatHistory";
            rtbChatHistory.ReadOnly = true;
            rtbChatHistory.Size = new Size(590, 340);
            rtbChatHistory.TabIndex = 2;
            rtbChatHistory.Text = "";
            // 
            // ChatLogControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(rtbChatHistory);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ChatLogControl";
            Size = new Size(598, 450);
            ControlRemoved += Control_Disposed;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSend;
        private TextBox txtInput;
        private Panel panel2;
        private Label lblChatName;
        private Label lblChatInfo;
        private Label label1;
        private TextBox txtSearch;
        private RichTextBox rtbChatHistory;
    }
}
