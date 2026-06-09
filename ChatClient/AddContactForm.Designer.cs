namespace ChatClient
{
    partial class AddContactForm
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
            label1 = new Label();
            txtNickname = new TextBox();
            btnConfirm = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(94, 15);
            label1.TabIndex = 0;
            label1.Text = "Enter Nickname:";
            // 
            // txtNickname
            // 
            txtNickname.Location = new Point(12, 27);
            txtNickname.Name = "txtNickname";
            txtNickname.Size = new Size(136, 23);
            txtNickname.TabIndex = 1;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(12, 56);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(136, 23);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Add";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // AddContactForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(160, 92);
            Controls.Add(btnConfirm);
            Controls.Add(txtNickname);
            Controls.Add(label1);
            Name = "AddContactForm";
            Text = "AddContactForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtNickname;
        private Button btnConfirm;
    }
}