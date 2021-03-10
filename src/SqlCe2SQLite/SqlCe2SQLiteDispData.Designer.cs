namespace SqlCe2SQLite
{
    partial class SqlCe2SQLiteDispData
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonDisplayData = new System.Windows.Forms.Button();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.radioButtonSQLite = new System.Windows.Forms.RadioButton();
            this.radioButtonSQLCe = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxAction = new System.Windows.Forms.TextBox();
            this.checkBoxTop = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 409);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(808, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.checkBoxTop);
            this.panelTop.Controls.Add(this.buttonDisplayData);
            this.panelTop.Controls.Add(this.textBoxTop);
            this.panelTop.Controls.Add(this.textBoxTableName);
            this.panelTop.Controls.Add(this.radioButtonSQLite);
            this.panelTop.Controls.Add(this.radioButtonSQLCe);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(808, 26);
            this.panelTop.TabIndex = 1;
            // 
            // buttonDisplayData
            // 
            this.buttonDisplayData.Location = new System.Drawing.Point(410, 0);
            this.buttonDisplayData.Name = "buttonDisplayData";
            this.buttonDisplayData.Size = new System.Drawing.Size(75, 23);
            this.buttonDisplayData.TabIndex = 5;
            this.buttonDisplayData.Text = "Display Data";
            this.buttonDisplayData.UseVisualStyleBackColor = true;
            this.buttonDisplayData.Click += new System.EventHandler(this.buttonDisplayData_Click);
            // 
            // textBoxTop
            // 
            this.textBoxTop.Location = new System.Drawing.Point(342, 2);
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(62, 20);
            this.textBoxTop.TabIndex = 4;
            this.textBoxTop.TextChanged += new System.EventHandler(this.textBoxTop_TextChanged);
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Location = new System.Drawing.Point(131, 2);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(151, 20);
            this.textBoxTableName.TabIndex = 2;
            // 
            // radioButtonSQLite
            // 
            this.radioButtonSQLite.AutoSize = true;
            this.radioButtonSQLite.Location = new System.Drawing.Point(68, 3);
            this.radioButtonSQLite.Name = "radioButtonSQLite";
            this.radioButtonSQLite.Size = new System.Drawing.Size(57, 17);
            this.radioButtonSQLite.TabIndex = 1;
            this.radioButtonSQLite.TabStop = true;
            this.radioButtonSQLite.Text = "SQLite";
            this.radioButtonSQLite.UseVisualStyleBackColor = true;
            // 
            // radioButtonSQLCe
            // 
            this.radioButtonSQLCe.AutoSize = true;
            this.radioButtonSQLCe.Location = new System.Drawing.Point(3, 3);
            this.radioButtonSQLCe.Name = "radioButtonSQLCe";
            this.radioButtonSQLCe.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSQLCe.TabIndex = 0;
            this.radioButtonSQLCe.TabStop = true;
            this.radioButtonSQLCe.Text = "SQLCe";
            this.radioButtonSQLCe.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(155, 90);
            this.dataGridView1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 44);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAction);
            this.splitContainer1.Size = new System.Drawing.Size(420, 277);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 3;
            // 
            // textBoxAction
            // 
            this.textBoxAction.Location = new System.Drawing.Point(14, 16);
            this.textBoxAction.Multiline = true;
            this.textBoxAction.Name = "textBoxAction";
            this.textBoxAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAction.Size = new System.Drawing.Size(211, 104);
            this.textBoxAction.TabIndex = 3;
            // 
            // checkBoxTop
            // 
            this.checkBoxTop.AutoSize = true;
            this.checkBoxTop.Location = new System.Drawing.Point(288, 4);
            this.checkBoxTop.Name = "checkBoxTop";
            this.checkBoxTop.Size = new System.Drawing.Size(48, 17);
            this.checkBoxTop.TabIndex = 6;
            this.checkBoxTop.Text = "TOP";
            this.checkBoxTop.UseVisualStyleBackColor = true;
            this.checkBoxTop.CheckedChanged += new System.EventHandler(this.checkBoxTop_CheckedChanged);
            // 
            // SqlCe2SQLiteDispData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 431);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip1);
            this.Name = "SqlCe2SQLiteDispData";
            this.Text = "SqlCe2SQLiteDispData";
            this.Load += new System.EventHandler(this.SqlCe2SQLiteDispData_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonDisplayData;
        private System.Windows.Forms.TextBox textBoxTop;
        private System.Windows.Forms.TextBox textBoxTableName;
        private System.Windows.Forms.RadioButton radioButtonSQLite;
        private System.Windows.Forms.RadioButton radioButtonSQLCe;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxAction;
        private System.Windows.Forms.CheckBox checkBoxTop;
    }
}