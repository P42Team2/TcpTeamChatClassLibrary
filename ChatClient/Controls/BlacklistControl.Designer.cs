namespace ChatClient.Controls
{
    partial class BlacklistControl
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
            btnUnblock = new Button();
            lvBlockedUsers = new ListView();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 13);
            label1.Name = "label1";
            label1.Size = new Size(121, 21);
            label1.TabIndex = 0;
            label1.Text = "Blocked Users";
            // 
            // btnUnblock
            // 
            btnUnblock.Location = new Point(13, 407);
            btnUnblock.Name = "btnUnblock";
            btnUnblock.Size = new Size(570, 28);
            btnUnblock.TabIndex = 12;
            btnUnblock.Text = "Unblock";
            btnUnblock.UseVisualStyleBackColor = true;
            // 
            // lvBlockedUsers
            // 
            lvBlockedUsers.FullRowSelect = true;
            lvBlockedUsers.Location = new Point(13, 37);
            lvBlockedUsers.Name = "lvBlockedUsers";
            lvBlockedUsers.Size = new Size(570, 364);
            lvBlockedUsers.TabIndex = 13;
            lvBlockedUsers.UseCompatibleStateImageBehavior = false;
            lvBlockedUsers.View = View.Details;
            // 
            // BlacklistControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lvBlockedUsers);
            Controls.Add(btnUnblock);
            Controls.Add(label1);
            Name = "BlacklistControl";
            Size = new Size(598, 450);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnUnblock;
        private ListView lvBlockedUsers;
    }
}
