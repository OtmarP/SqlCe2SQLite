using System;
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
    public partial class SqlCe2SQLiteMain : Form
    {
        public SqlCe2SQLiteMain()
        {
            InitializeComponent();

            this.Top = 50;
            this.Left = 50;

            this.toolStripStatusLabel1.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            this.textBoxAction.Dock = DockStyle.Fill;

#if DEBUG
            this.textBoxSqlCe.Text = @"C:\temp\KaJour\Prod\Prod2\KAJOUR4.sdf";
            this.textBoxSQLite.Text = @"C:\temp\KaJour\Prod\Prod2\KAJOUR4.db3";
#endif
        }

        private void SqlCe2SQLiteMain_Load(object sender, EventArgs e)
        {
            //--------------------------------- History: letzter oben
            // Mi.03.03.2021 12:30:45 -op- NuGet (Sqlite, SqlCe), DispStaus, DelTarget
            // Di.02.03.2021 17:30:00 -op- Cr. (4.7.1)
            //---------------------------------
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            // Exit
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("not implemented");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("not implemented");
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // Copy
            this.DisableEnabledControls(false);

            var ret = this.CopyData();

            this.DisableEnabledControls(true);
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            // Disp Status
            this.DisableEnabledControls(false);

            var ret = this.DispStatus();

            this.DisableEnabledControls(true);
        }

        private void buttonDelTarget_Click(object sender, EventArgs e)
        {
            // Del Target
            this.DisableEnabledControls(false);

            var ret = this.DelTarget();

            this.DisableEnabledControls(true);
        }

        private void DisableEnabledControls(bool enabled) {
            this.buttonExit.Enabled = enabled;
            this.buttonStatus.Enabled = enabled;
            this.buttonDelTarget.Enabled = enabled;
            this.buttonCopy.Enabled = enabled;
            Application.DoEvents();
        }

        private bool DispStatus()
        {
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            StringBuilder sb = new StringBuilder();

            // 1. Check Databases
            // 2. Check Tables in SQLite
            // 3. Delete Records in SQLite
            // 4. Select Records from SQLCe and insert to SQLite
            // 5. Check RecordCount from SQLCe and SQLite

            //
            KaJourDAL.KaJour_Global_CE.SQLProvider = "SQLCE";
            KaJourDAL.KaJour_Global_CE.SQLConnStr = "Data Source='" + this.textBoxSqlCe.Text + "'";

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_CE.SQLProvider+":");

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            sqlCe.Connect();
            DataTable tablesCE = sqlCe.GetTableList("", false);
            sqlCe.DisConnect();
            for (int iTable = 0; iTable < tablesCE.Rows.Count; iTable++)
            {
                // 0 / 6 ... 0
                // 1 / 6 ...
                // 2 / 6 ...
                // 3 / 6 ... 50
                // 4 / 6 ...
                // 5 / 6 ...
                // 6 / 6 ... 100
                this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesCE.Rows.Count;
                Application.DoEvents();

                var tableName = tablesCE.Rows[iTable][0].ToString();
                sqlCe.Connect();
                var tableRec1 = sqlCe.GetTableRecCount(tableName);
                sqlCe.DisConnect();

                sb.AppendLine("  "+tableName + "   Rec:" + tableRec1.ToString());
            }
            sb.AppendLine("  Count: " + tablesCE.Rows.Count.ToString());

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");

            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            sqLITE.Connect();
            DataTable tablesLITE = sqLITE.GetTableList("", false);
            sqLITE.DisConnect();
            for (int iTable = 0; iTable < tablesLITE.Rows.Count; iTable++){
                this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesLITE.Rows.Count;
                Application.DoEvents();

                var tableName = tablesLITE.Rows[iTable][0].ToString();
                sqLITE.Connect();
                var tableRec1 = sqLITE.GetTableRecCount(tableName);
                sqLITE.DisConnect();

                sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());
            }
            sb.AppendLine("  Count: " + tablesLITE.Rows.Count.ToString());

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }

        private bool DelTarget(){
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            StringBuilder sb = new StringBuilder();

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");
            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            sqLITE.Connect();
            DataTable tablesLITE = sqLITE.GetTableList("", false);
            sqLITE.DisConnect();
            for (int iTable = 0; iTable < tablesLITE.Rows.Count; iTable++){
                this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesLITE.Rows.Count;
                Application.DoEvents();

                var tableName = tablesLITE.Rows[iTable][0].ToString();
                sqLITE.Connect();
                var tableRec1 = sqLITE.GetTableRecCount(tableName);
                sqLITE.DisConnect();

                // Delete
                var del = sqLITE.DeleteBuilder(tableName);
                var retDel = sqLITE.ExecuteNonQuery("DELETE", del);

                sqLITE.Connect();
                var tableRec2 = sqLITE.GetTableRecCount(tableName);
                sqLITE.DisConnect();

                sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString()+ "   Del:" + retDel.ToString() + "   Rec:" + tableRec2.ToString());
            }
            sb.AppendLine("  Count: " + tablesLITE.Rows.Count.ToString());

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }

        private bool CopyData()
        {
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            StringBuilder sb = new StringBuilder();

            //
            KaJourDAL.KaJour_Global_CE.SQLProvider = "SQLCE";
            KaJourDAL.KaJour_Global_CE.SQLConnStr = "Data Source='" + this.textBoxSqlCe.Text + "'";

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_CE.SQLProvider + ":");

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            sqlCe.Connect();
            DataTable tablesCE = sqlCe.GetTableList("", false);
            sqlCe.DisConnect();
            for (int iTable = 0; iTable < tablesCE.Rows.Count; iTable++){
                this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesCE.Rows.Count;
                Application.DoEvents();

                var tableName = tablesCE.Rows[iTable][0].ToString();
                sqlCe.Connect();
                var tableRec1 = sqlCe.GetTableRecCount(tableName);
                sqlCe.DisConnect();

                sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());

                var tableSelect = sqlCe.Execute("SELECT", "SELECT * FROM " + tableName);
                for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++)
                {
                    var par = sqlCe.InitParameterList();
                    for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++)
                    {
                        var colVal = tableSelect.Rows[iRow][iCol];
                        var colName = tableSelect.Columns[iCol].ColumnName;
                        par.Add(colName, colVal);
                    }

                    //// insert into SQLite
                    //var sqlLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
                    ////string sqlIns = "INSERT ";
                    //var ins = sqlLITE.InsertBuilder(tableName);

                    //string sqlIns = " set Fld1=@Val1";

                }

            }
            sb.AppendLine("  Count: " + tablesCE.Rows.Count.ToString());

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }
    }
}
