﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlCe2SQLite
{
    public partial class SqlCe2SQLiteDispData : Form
    {
        private string _SQLProvider;
        private string _SQLDB;

        public SqlCe2SQLiteDispData()
        {
            InitializeComponent();

            this.toolStripStatusLabel1.Text = "";
            this.textBoxTop.Text = "20";

            this.radioButtonSQLite.Enabled = false;
            this.radioButtonSQLCe.Enabled = false;

            this.textBoxTableName.Enabled = false;

            this.checkBoxTop.Checked = true;

            this.splitContainer1.Dock = DockStyle.Fill;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.textBoxAction.Dock = DockStyle.Fill;
        }

        private void SqlCe2SQLiteDispData_Load(object sender, EventArgs e)
        {
            // Load
            this.Top = 150;
            this.Left = 150;
        }

        public void SetData(string sqlCeOrSQLite, string tableName, string dBName) {
            if (sqlCeOrSQLite== "SQLCE") {
                this.radioButtonSQLCe.Checked = true;
                _SQLProvider = sqlCeOrSQLite;
            }
            if (sqlCeOrSQLite == "SQLITE")
            {
                this.radioButtonSQLite.Checked = true;
                _SQLProvider = sqlCeOrSQLite;
            }
            this.textBoxTableName.Text = tableName;
            _SQLDB = dBName;
        }

        private void buttonDisplayData_Click(object sender, EventArgs e)
        {
            // Display Data
            this.DisplayData();
        }

        private bool DisplayData()
        {
            bool ret = false;

            this.toolStripStatusLabel1.Text = "Load Data...";
            this.textBoxAction.Text = "";
            this.toolStripProgressBar1.Value = 0;
            Application.DoEvents();

            int maxRows = -1;
            if (this.checkBoxTop.Checked)
            {
                try
                {
                    maxRows = Convert.ToInt32(this.textBoxTop.Text);
                }
                catch (Exception)
                {
                }
            }

            int countRows = 0;

            StringBuilder sb = new StringBuilder();

            KaJourDAL.KaJour_Global_LITE.SQLProvider = _SQLProvider;
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + _SQLDB + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");

            var sqCEorLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            DataTable tablesLITE = null;
            try
            {
                sqCEorLITE.Connect();
                tablesLITE = sqCEorLITE.GetTableList("", false);
                sqCEorLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }

            var tableName = this.textBoxTableName.Text;
            sqCEorLITE.Connect();
            var tableRec1 = sqCEorLITE.GetTableRecCount(tableName);
            sqCEorLITE.DisConnect();

            sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());

            // Display
            var sel = "SELECT * FROM " + tableName;
            if (this.checkBoxTop.Checked)
            {
                sel = sqCEorLITE.TopBuilder(tableName, "SELECT ", "* FROM {0}", maxRows);
            }
            DataTable tableSelect = sqCEorLITE.Execute("SELECT", sel);
            for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++)
            {
                countRows++;

                this.toolStripProgressBar1.Value = ((iRow+1) * 100) / tableRec1;
                this.toolStripStatusLabel1.Text = "Load Data... " + (iRow+1).ToString();
                var doEvents = UXHelper.CalcModulo(iRow);
                if (doEvents) {
                    Application.DoEvents();
                }

                for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++)
                {
                    var colVal = tableSelect.Rows[iRow][iCol];
                    sb.Append(colVal);
                    sb.Append(", ");
                }
                sb.AppendLine("");
            }

            this.toolStripStatusLabel1.Text = "Load Data Ok.";
            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBar1.Value = 100;

            return ret;
        }

        private void checkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxTop.Checked)
            {
                this.textBoxTop.BackColor = Color.LightCyan;
                this.textBoxTop.Enabled = true;
            }
            else {
                this.textBoxTop.BackColor = System.Drawing.SystemColors.Window;
                this.textBoxTop.Enabled = false;
            }
        }

        private void textBoxTop_TextChanged(object sender, EventArgs e)
        {
            //
        }
    }
}
