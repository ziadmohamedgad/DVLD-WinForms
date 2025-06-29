namespace DVLD.Tests
{
    partial class frmListTestTypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListTestTypes));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvTestTypes = new System.Windows.Forms.DataGridView();
            this.cmsTestTypes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbApplicationTypesmage = new System.Windows.Forms.PictureBox();
            this.editTestTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestTypes)).BeginInit();
            this.cmsTestTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbApplicationTypesmage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.AutoEllipsis = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(807, 638);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 36);
            this.btnClose.TabIndex = 113;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblRecordsCount
            // 
            this.lblRecordsCount.AutoSize = true;
            this.lblRecordsCount.Location = new System.Drawing.Point(114, 646);
            this.lblRecordsCount.Name = "lblRecordsCount";
            this.lblRecordsCount.Size = new System.Drawing.Size(27, 20);
            this.lblRecordsCount.TabIndex = 117;
            this.lblRecordsCount.Text = "??";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 646);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 116;
            this.label2.Text = "# Records:";
            // 
            // dgvTestTypes
            // 
            this.dgvTestTypes.AllowUserToAddRows = false;
            this.dgvTestTypes.AllowUserToDeleteRows = false;
            this.dgvTestTypes.AllowUserToResizeRows = false;
            this.dgvTestTypes.BackgroundColor = System.Drawing.Color.White;
            this.dgvTestTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTestTypes.ContextMenuStrip = this.cmsTestTypes;
            this.dgvTestTypes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTestTypes.Location = new System.Drawing.Point(12, 276);
            this.dgvTestTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvTestTypes.MultiSelect = false;
            this.dgvTestTypes.Name = "dgvTestTypes";
            this.dgvTestTypes.ReadOnly = true;
            this.dgvTestTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTestTypes.Size = new System.Drawing.Size(931, 354);
            this.dgvTestTypes.TabIndex = 118;
            // 
            // cmsTestTypes
            // 
            this.cmsTestTypes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTestTypeToolStripMenuItem});
            this.cmsTestTypes.Name = "cmsTestTypes";
            this.cmsTestTypes.Size = new System.Drawing.Size(197, 64);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTitle.Location = new System.Drawing.Point(254, 215);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(446, 39);
            this.lblTitle.TabIndex = 120;
            this.lblTitle.Text = "Manage Test Types";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbApplicationTypesmage
            // 
            this.pbApplicationTypesmage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbApplicationTypesmage.Image = ((System.Drawing.Image)(resources.GetObject("pbApplicationTypesmage.Image")));
            this.pbApplicationTypesmage.InitialImage = null;
            this.pbApplicationTypesmage.Location = new System.Drawing.Point(360, 21);
            this.pbApplicationTypesmage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbApplicationTypesmage.Name = "pbApplicationTypesmage";
            this.pbApplicationTypesmage.Size = new System.Drawing.Size(220, 189);
            this.pbApplicationTypesmage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbApplicationTypesmage.TabIndex = 121;
            this.pbApplicationTypesmage.TabStop = false;
            // 
            // editTestTypeToolStripMenuItem
            // 
            this.editTestTypeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editTestTypeToolStripMenuItem.Image")));
            this.editTestTypeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editTestTypeToolStripMenuItem.Name = "editTestTypeToolStripMenuItem";
            this.editTestTypeToolStripMenuItem.Size = new System.Drawing.Size(196, 38);
            this.editTestTypeToolStripMenuItem.Text = "&Edit Test Type";
            this.editTestTypeToolStripMenuItem.Click += new System.EventHandler(this.editTestTypeToolStripMenuItem_Click);
            // 
            // frmListTestTypes
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(954, 692);
            this.Controls.Add(this.pbApplicationTypesmage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvTestTypes);
            this.Controls.Add(this.lblRecordsCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmListTestTypes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List Test Types";
            this.Load += new System.EventHandler(this.frmListTestTypes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestTypes)).EndInit();
            this.cmsTestTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbApplicationTypesmage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvTestTypes;
        private System.Windows.Forms.ContextMenuStrip cmsTestTypes;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbApplicationTypesmage;
        private System.Windows.Forms.ToolStripMenuItem editTestTypeToolStripMenuItem;
    }
}