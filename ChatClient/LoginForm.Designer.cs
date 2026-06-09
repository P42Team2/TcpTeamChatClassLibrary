namespace ChatClient
{
    partial class LoginForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLogIn = new Button();
            btnRegister = new Button();
            pnlLogIn = new Panel();
            groupBox1 = new GroupBox();
            txtPort = new TextBox();
            txtIP = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnLogIn
            // 
            btnLogIn.Location = new Point(7, 6);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(75, 23);
            btnLogIn.TabIndex = 0;
            btnLogIn.Text = "Log in";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(88, 6);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(75, 23);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // pnlLogIn
            // 
            pnlLogIn.Location = new Point(4, 35);
            pnlLogIn.Name = "pnlLogIn";
            pnlLogIn.Size = new Size(163, 129);
            pnlLogIn.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtIP);
            groupBox1.Location = new Point(4, 170);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(163, 82);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Address";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(6, 51);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 5;
            txtPort.Text = "6767";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(6, 22);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(100, 23);
            txtIP.TabIndex = 4;
            txtIP.Text = "127.0.0.1";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(171, 255);
            Controls.Add(groupBox1);
            Controls.Add(pnlLogIn);
            Controls.Add(btnRegister);
            Controls.Add(btnLogIn);
            Name = "LoginForm";
            Text = "LoginForm";
            FormClosed += LoginForm_FormClosed;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnLogIn;
        private Button btnRegister;
        private Panel pnlLogIn;
        private GroupBox groupBox1;
        private TextBox txtPort;
        private TextBox txtIP;
    }
}