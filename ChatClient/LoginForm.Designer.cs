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
            btnLogIn.Location = new Point(8, 8);
            btnLogIn.Margin = new Padding(3, 4, 3, 4);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(86, 31);
            btnLogIn.TabIndex = 0;
            btnLogIn.Text = "Log in";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(101, 8);
            btnRegister.Margin = new Padding(3, 4, 3, 4);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(86, 31);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // pnlLogIn
            // 
            pnlLogIn.Location = new Point(5, 47);
            pnlLogIn.Margin = new Padding(3, 4, 3, 4);
            pnlLogIn.Name = "pnlLogIn";
            pnlLogIn.Size = new Size(186, 172);
            pnlLogIn.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtIP);
            groupBox1.Location = new Point(5, 227);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(186, 109);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Address";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(7, 68);
            txtPort.Margin = new Padding(3, 4, 3, 4);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(114, 27);
            txtPort.TabIndex = 5;
            txtPort.Text = "10000";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(7, 29);
            txtIP.Margin = new Padding(3, 4, 3, 4);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(114, 27);
            txtIP.TabIndex = 4;
            txtIP.Text = "127.0.0.1";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(195, 340);
            Controls.Add(groupBox1);
            Controls.Add(pnlLogIn);
            Controls.Add(btnRegister);
            Controls.Add(btnLogIn);
            Margin = new Padding(3, 4, 3, 4);
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