namespace SqlCe2SQLite
{
    partial class SqlCe2SQLiteMain
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.checkBoxTestNRecords = new System.Windows.Forms.CheckBox();
            this.textBoxTestNRecords = new System.Windows.Forms.TextBox();
            this.buttonDelTarget = new System.Windows.Forms.Button();
            this.buttonStatus = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textBoxSQLite = new System.Windows.Forms.TextBox();
            this.textBoxSqlCe = new System.Windows.Forms.TextBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarTable = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxAction = new System.Windows.Forms.TextBox();
            this.buttonDispData = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonDispData);
            this.panelTop.Controls.Add(this.checkBoxTestNRecords);
            this.panelTop.Controls.Add(this.textBoxTestNRecords);
            this.panelTop.Controls.Add(this.buttonDelTarget);
            this.panelTop.Controls.Add(this.buttonStatus);
            this.panelTop.Controls.Add(this.buttonCopy);
            this.panelTop.Controls.Add(this.linkLabel2);
            this.panelTop.Controls.Add(this.linkLabel1);
            this.panelTop.Controls.Add(this.textBoxSQLite);
            this.panelTop.Controls.Add(this.textBoxSqlCe);
            this.panelTop.Controls.Add(this.buttonExit);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(735, 65);
            this.panelTop.TabIndex = 0;
            // 
            // checkBoxTestNRecords
            // 
            this.checkBoxTestNRecords.AutoSize = true;
            this.checkBoxTestNRecords.Location = new System.Drawing.Point(344, 35);
            this.checkBoxTestNRecords.Name = "checkBoxTestNRecords";
            this.checkBoxTestNRecords.Size = new System.Drawing.Size(99, 17);
            this.checkBoxTestNRecords.TabIndex = 11;
            this.checkBoxTestNRecords.Text = "Test n Records";
            this.checkBoxTestNRecords.UseVisualStyleBackColor = true;
            // 
            // textBoxTestNRecords
            // 
            this.textBoxTestNRecords.Location = new System.Drawing.Point(449, 33);
            this.textBoxTestNRecords.Name = "textBoxTestNRecords";
            this.textBoxTestNRecords.Size = new System.Drawing.Size(41, 20);
            this.textBoxTestNRecords.TabIndex = 9;
            // 
            // buttonDelTarget
            // 
            this.buttonDelTarget.Location = new System.Drawing.Point(545, 31);
            this.buttonDelTarget.Name = "buttonDelTarget";
            this.buttonDelTarget.Size = new System.Drawing.Size(75, 23);
            this.buttonDelTarget.TabIndex = 8;
            this.buttonDelTarget.Text = "Del Target";
            this.buttonDelTarget.UseVisualStyleBackColor = true;
            this.buttonDelTarget.Click += new System.EventHandler(this.buttonDelTarget_Click);
            // 
            // buttonStatus
            // 
            this.buttonStatus.Location = new System.Drawing.Point(3, 31);
            this.buttonStatus.Name = "buttonStatus";
            this.buttonStatus.Size = new System.Drawing.Size(75, 23);
            this.buttonStatus.TabIndex = 7;
            this.buttonStatus.Text = "Status";
            this.buttonStatus.UseVisualStyleBackColor = true;
            this.buttonStatus.Click += new System.EventHandler(this.buttonStatus_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(357, 3);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 6;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(438, 8);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(39, 13);
            this.linkLabel2.TabIndex = 5;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "SQLite";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(93, 8);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "SqlCe";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // textBoxSQLite
            // 
            this.textBoxSQLite.Location = new System.Drawing.Point(483, 5);
            this.textBoxSQLite.Name = "textBoxSQLite";
            this.textBoxSQLite.Size = new System.Drawing.Size(218, 20);
            this.textBoxSQLite.TabIndex = 2;
            // 
            // textBoxSqlCe
            // 
            this.textBoxSqlCe.Location = new System.Drawing.Point(134, 5);
            this.textBoxSqlCe.Name = "textBoxSqlCe";
            this.textBoxSqlCe.Size = new System.Drawing.Size(218, 20);
            this.textBoxSqlCe.TabIndex = 1;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(3, 3);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBarTable,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 398);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(735, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBarTable
            // 
            this.toolStripProgressBarTable.Name = "toolStripProgressBarTable";
            this.toolStripProgressBarTable.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBarTable.Value = 50;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // textBoxAction
            // 
            this.textBoxAction.Location = new System.Drawing.Point(12, 89);
            this.textBoxAction.Multiline = true;
            this.textBoxAction.Name = "textBoxAction";
            this.textBoxAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAction.Size = new System.Drawing.Size(478, 287);
            this.textBoxAction.TabIndex = 2;
            // 
            // buttonDispData
            // 
            this.buttonDispData.Location = new System.Drawing.Point(626, 31);
            this.buttonDispData.Name = "buttonDispData";
            this.buttonDispData.Size = new System.Drawing.Size(75, 23);
            this.buttonDispData.TabIndex = 12;
            this.buttonDispData.Text = "Disp Data";
            this.buttonDispData.UseVisualStyleBackColor = true;
            this.buttonDispData.Click += new System.EventHandler(this.buttonDispData_Click);
            // 
            // SqlCe2SQLiteMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 420);
            this.Controls.Add(this.textBoxAction);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelTop);
            this.Name = "SqlCe2SQLiteMain";
            this.Text = "SqlCe2SQLite";
            this.Load += new System.EventHandler(this.SqlCe2SQLiteMain_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarTable;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox textBoxSQLite;
        private System.Windows.Forms.TextBox textBoxSqlCe;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.TextBox textBoxAction;
        private System.Windows.Forms.Button buttonDelTarget;
        private System.Windows.Forms.Button buttonStatus;
        private System.Windows.Forms.TextBox textBoxTestNRecords;
        private System.Windows.Forms.CheckBox checkBoxTestNRecords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button buttonDispData;
    }
}

