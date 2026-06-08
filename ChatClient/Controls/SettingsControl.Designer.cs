namespace ChatClient.Controls
{
    partial class SettingsControl
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
            lblLoggedInAs = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            lblPortIP = new Label();
            groupBox2 = new GroupBox();
            txtOldPswd = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtNewPswd = new TextBox();
            btnSaveNewPswd = new Button();
            button1 = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // lblLoggedInAs
            // 
            lblLoggedInAs.AutoSize = true;
            lblLoggedInAs.Location = new Point(6, 19);
            lblLoggedInAs.Name = "lblLoggedInAs";
            lblLoggedInAs.Size = new Size(116, 15);
            lblLoggedInAs.TabIndex = 1;
            lblLoggedInAs.Text = "You are logged in as:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 14);
            label1.Name = "label1";
            label1.Size = new Size(75, 21);
            label1.TabIndex = 2;
            label1.Text = "Settings";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblPortIP);
            groupBox1.Controls.Add(lblLoggedInAs);
            groupBox1.Location = new Point(3, 38);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(592, 44);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            // 
            // lblPortIP
            // 
            lblPortIP.AutoSize = true;
            lblPortIP.Location = new Point(294, 19);
            lblPortIP.Name = "lblPortIP";
            lblPortIP.Size = new Size(68, 15);
            lblPortIP.TabIndex = 2;
            lblPortIP.Text = "IP and Port:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnSaveNewPswd);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtNewPswd);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtOldPswd);
            groupBox2.Location = new Point(3, 88);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(592, 68);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Change Password";
            // 
            // txtOldPswd
            // 
            txtOldPswd.Location = new Point(6, 37);
            txtOldPswd.Name = "txtOldPswd";
            txtOldPswd.Size = new Size(188, 23);
            txtOldPswd.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 19);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 6;
            label2.Text = "Old password:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(200, 19);
            label3.Name = "label3";
            label3.Size = new Size(87, 15);
            label3.TabIndex = 8;
            label3.Text = "New password:";
            // 
            // txtNewPswd
            // 
            txtNewPswd.Location = new Point(200, 37);
            txtNewPswd.Name = "txtNewPswd";
            txtNewPswd.Size = new Size(188, 23);
            txtNewPswd.TabIndex = 7;
            // 
            // btnSaveNewPswd
            // 
            btnSaveNewPswd.Location = new Point(394, 19);
            btnSaveNewPswd.Name = "btnSaveNewPswd";
            btnSaveNewPswd.Size = new Size(192, 41);
            btnSaveNewPswd.TabIndex = 9;
            btnSaveNewPswd.Text = "Save Password";
            btnSaveNewPswd.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.ForeColor = Color.Red;
            button1.Location = new Point(3, 162);
            button1.Name = "button1";
            button1.Size = new Size(592, 26);
            button1.TabIndex = 5;
            button1.Text = "Log Out";
            button1.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Name = "SettingsControl";
            Size = new Size(598, 450);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLoggedInAs;
        private Label label1;
        private GroupBox groupBox1;
        private Label lblPortIP;
        private GroupBox groupBox2;
        private Label label3;
        private TextBox txtNewPswd;
        private Label label2;
        private TextBox txtOldPswd;
        private Button btnSaveNewPswd;
        private Button button1;
    }
}
