namespace DVLD.Users
{
    partial class frmListUsers
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListUsers));
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.cmsUsers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDetaulsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.sendEmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phoneCallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFilterBy = new System.Windows.Forms.Label();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.lblManageUsers = new System.Windows.Forms.Label();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.lblRecords = new System.Windows.Forms.Label();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.cbIsActive = new System.Windows.Forms.ComboBox();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.cmsUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.ContextMenuStrip = this.cmsUsers;
            this.dgvUsers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.TabStop = false;
            this.dgvUsers.DoubleClick += new System.EventHandler(this.dgvUsers_DoubleClick);
            // 
            // cmsUsers
            // 
            this.cmsUsers.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsUsers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDetaulsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.addNewUserToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.toolStripMenuItem2,
            this.sendEmailToolStripMenuItem,
            this.phoneCallToolStripMenuItem});
            this.cmsUsers.Name = "cmsUsers";
            resources.ApplyResources(this.cmsUsers, "cmsUsers");
            // 
            // showDetaulsToolStripMenuItem
            // 
            resources.ApplyResources(this.showDetaulsToolStripMenuItem, "showDetaulsToolStripMenuItem");
            this.showDetaulsToolStripMenuItem.Name = "showDetaulsToolStripMenuItem";
            this.showDetaulsToolStripMenuItem.Click += new System.EventHandler(this.showDetailsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // addNewUserToolStripMenuItem
            // 
            resources.ApplyResources(this.addNewUserToolStripMenuItem, "addNewUserToolStripMenuItem");
            this.addNewUserToolStripMenuItem.Name = "addNewUserToolStripMenuItem";
            this.addNewUserToolStripMenuItem.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // editToolStripMenuItem
            // 
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // changePasswordToolStripMenuItem
            // 
            resources.ApplyResources(this.changePasswordToolStripMenuItem, "changePasswordToolStripMenuItem");
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // sendEmailToolStripMenuItem
            // 
            resources.ApplyResources(this.sendEmailToolStripMenuItem, "sendEmailToolStripMenuItem");
            this.sendEmailToolStripMenuItem.Name = "sendEmailToolStripMenuItem";
            this.sendEmailToolStripMenuItem.Click += new System.EventHandler(this.NotImplementedFeatures);
            // 
            // phoneCallToolStripMenuItem
            // 
            resources.ApplyResources(this.phoneCallToolStripMenuItem, "phoneCallToolStripMenuItem");
            this.phoneCallToolStripMenuItem.Name = "phoneCallToolStripMenuItem";
            this.phoneCallToolStripMenuItem.Click += new System.EventHandler(this.NotImplementedFeatures);
            // 
            // lblFilterBy
            // 
            resources.ApplyResources(this.lblFilterBy, "lblFilterBy");
            this.lblFilterBy.Name = "lblFilterBy";
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbFilterBy, "cbFilterBy");
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Items.AddRange(new object[] {
            resources.GetString("cbFilterBy.Items"),
            resources.GetString("cbFilterBy.Items1"),
            resources.GetString("cbFilterBy.Items2"),
            resources.GetString("cbFilterBy.Items3"),
            resources.GetString("cbFilterBy.Items4"),
            resources.GetString("cbFilterBy.Items5")});
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cbFilterBy_SelectedIndexChanged);
            // 
            // lblManageUsers
            // 
            resources.ApplyResources(this.lblManageUsers, "lblManageUsers");
            this.lblManageUsers.ForeColor = System.Drawing.Color.DarkRed;
            this.lblManageUsers.Name = "lblManageUsers";
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtFilterValue, "txtFilterValue");
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.TextChanged += new System.EventHandler(this.txtFilterValue_TextChanged);
            this.txtFilterValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterValue_KeyPress);
            // 
            // lblRecords
            // 
            resources.ApplyResources(this.lblRecords, "lblRecords");
            this.lblRecords.Name = "lblRecords";
            // 
            // lblRecordsCount
            // 
            resources.ApplyResources(this.lblRecordsCount, "lblRecordsCount");
            this.lblRecordsCount.Name = "lblRecordsCount";
            // 
            // cbIsActive
            // 
            this.cbIsActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIsActive.FormattingEnabled = true;
            this.cbIsActive.Items.AddRange(new object[] {
            resources.GetString("cbIsActive.Items"),
            resources.GetString("cbIsActive.Items1"),
            resources.GetString("cbIsActive.Items2")});
            resources.ApplyResources(this.cbIsActive, "cbIsActive");
            this.cbIsActive.Name = "cbIsActive";
            this.cbIsActive.SelectedIndexChanged += new System.EventHandler(this.cbIsActive_SelectedIndexChanged);
            // 
            // btnAddUser
            // 
            resources.ApplyResources(this.btnAddUser, "btnAddUser");
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // frmListUsers
            // 
            this.AcceptButton = this.btnAddUser;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.cbIsActive);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblRecordsCount);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.lblFilterBy);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.cbFilterBy);
            this.Controls.Add(this.txtFilterValue);
            this.Controls.Add(this.lblManageUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmListUsers";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmListUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.cmsUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Label lblFilterBy;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblManageUsers;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbIsActive;
        private System.Windows.Forms.ContextMenuStrip cmsUsers;
        private System.Windows.Forms.ToolStripMenuItem showDetaulsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addNewUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sendEmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phoneCallToolStripMenuItem;
    }
}