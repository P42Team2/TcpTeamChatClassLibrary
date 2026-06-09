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
            label1.Location = new Point(14, 11);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 9;
            label1.Text = "Search Contact:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(14, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(169, 23);
            txtSearch.TabIndex = 8;
            txtSearch.TextChanged += txtSearchContact_TextChanged;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnBlock);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(398, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 450);
            panel1.TabIndex = 10;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(3, 287);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(192, 35);
            btnAdd.TabIndex = 13;
            btnAdd.Text = "Add ➕";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnBlock
            // 
            btnBlock.Location = new Point(3, 410);
            btnBlock.Name = "btnBlock";
            btnBlock.Size = new Size(192, 35);
            btnBlock.TabIndex = 12;
            btnBlock.Text = "Block 🚫";
            btnBlock.UseVisualStyleBackColor = true;
            btnBlock.Click += btnBlock_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(3, 369);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(192, 35);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "Delete ❌";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(3, 328);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(192, 35);
            btnEdit.TabIndex = 10;
            btnEdit.Text = "Edit ✏️";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(13, 12);
            label2.Name = "label2";
            label2.Size = new Size(80, 21);
            label2.TabIndex = 11;
            label2.Text = "Contacts";
            // 
            // lvContacts
            // 
            lvContacts.FullRowSelect = true;
            lvContacts.Location = new Point(0, 41);
            lvContacts.Name = "lvContacts";
            lvContacts.Size = new Size(392, 409);
            lvContacts.TabIndex = 12;
            lvContacts.UseCompatibleStateImageBehavior = false;
            lvContacts.View = View.Details;
            // 
            // ContactsManagementControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lvContacts);
            Controls.Add(label2);
            Controls.Add(panel1);
            Name = "ContactsManagementControl";
            Size = new Size(598, 450);
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
    }
}
