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
            lblIpAndPort = new Label();
            groupBox2 = new GroupBox();
            btnUpdatePassword = new Button();
            label3 = new Label();
            txtNewPassword = new TextBox();
            label2 = new Label();
            txtOldPassword = new TextBox();
            btnLogout = new Button();
            groupBox3 = new GroupBox();
            btnUpdateUsername = new Button();
            label4 = new Label();
            txtNewUsername = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
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
            groupBox1.Controls.Add(lblIpAndPort);
            groupBox1.Controls.Add(lblLoggedInAs);
            groupBox1.Location = new Point(3, 38);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(592, 44);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            // 
            // lblIpAndPort
            // 
            lblIpAndPort.AutoSize = true;
            lblIpAndPort.Location = new Point(294, 19);
            lblIpAndPort.Name = "lblIpAndPort";
            lblIpAndPort.Size = new Size(68, 15);
            lblIpAndPort.TabIndex = 2;
            lblIpAndPort.Text = "IP and Port:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnUpdatePassword);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtNewPassword);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtOldPassword);
            groupBox2.Location = new Point(3, 88);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(592, 68);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Change Password";
            // 
            // btnUpdatePassword
            // 
            btnUpdatePassword.Location = new Point(394, 19);
            btnUpdatePassword.Name = "btnUpdatePassword";
            btnUpdatePassword.Size = new Size(192, 41);
            btnUpdatePassword.TabIndex = 9;
            btnUpdatePassword.Text = "Update Password";
            btnUpdatePassword.UseVisualStyleBackColor = true;
            btnUpdatePassword.Click += btnUpdatePassword_Click;
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
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(200, 37);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(188, 23);
            txtNewPassword.TabIndex = 7;
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
            // txtOldPassword
            // 
            txtOldPassword.Location = new Point(6, 37);
            txtOldPassword.Name = "txtOldPassword";
            txtOldPassword.Size = new Size(188, 23);
            txtOldPassword.TabIndex = 5;
            // 
            // btnLogout
            // 
            btnLogout.ForeColor = Color.Red;
            btnLogout.Location = new Point(3, 236);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(592, 26);
            btnLogout.TabIndex = 5;
            btnLogout.Text = "Log Out";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnUpdateUsername);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(txtNewUsername);
            groupBox3.Location = new Point(3, 162);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(592, 68);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Change Username";
            // 
            // btnUpdateUsername
            // 
            btnUpdateUsername.Location = new Point(394, 19);
            btnUpdateUsername.Name = "btnUpdateUsername";
            btnUpdateUsername.Size = new Size(192, 41);
            btnUpdateUsername.TabIndex = 9;
            btnUpdateUsername.Text = "Update Username";
            btnUpdateUsername.UseVisualStyleBackColor = true;
            btnUpdateUsername.Click += btnUpdateUsername_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 19);
            label4.Name = "label4";
            label4.Size = new Size(89, 15);
            label4.TabIndex = 8;
            label4.Text = "New username:";
            // 
            // txtNewUsername
            // 
            txtNewUsername.Location = new Point(6, 37);
            txtNewUsername.Name = "txtNewUsername";
            txtNewUsername.Size = new Size(382, 23);
            txtNewUsername.TabIndex = 7;
            // 
            // SettingsControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox3);
            Controls.Add(btnLogout);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Name = "SettingsControl";
            Size = new Size(598, 450);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLoggedInAs;
        private Label label1;
        private GroupBox groupBox1;
        private Label lblIpAndPort;
        private GroupBox groupBox2;
        private Label label3;
        private TextBox txtNewPassword;
        private Label label2;
        private TextBox txtOldPassword;
        private Button btnUpdatePassword;
        private Button btnLogout;
        private GroupBox groupBox3;
        private Button btnUpdateUsername;
        private Label label4;
        private TextBox txtNewUsername;
    }
}
