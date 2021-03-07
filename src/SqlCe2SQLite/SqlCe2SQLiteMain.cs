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
            this.toolStripStatusLabel2.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            this.textBoxAction.Dock = DockStyle.Fill;
            this.textBoxTestNRecords.Text = "50";

            this.Text = "SqlCe2SQLite v:2021.03.04";
#if DEBUG
            this.Text += "   ***DEBUG***";
#endif

#if DEBUG
            this.textBoxSqlCe.Text = @"C:\temp\KaJour\Prod\Prod2\KAJOUR4.sdf";
            this.textBoxSQLite.Text = @"C:\temp\KaJour\Prod\Prod2\KAJOUR4.db3";
#endif
        }

        private void SqlCe2SQLiteMain_Load(object sender, EventArgs e)
        {
            //--------------------------------- History: letzter oben
            // Do.04.03.2021 22:15:33 -op- v:2021.03.04
            // Do.04.03.2021 18:42:29 -op- CopyData, mit TestNRecords
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

            DateTime dtStart = DateTime.Now;

            int countTables = 0;
            int countRows = 0;

            StringBuilder sb = new StringBuilder();

            //
            KaJourDAL.KaJour_Global_CE.SQLProvider = "SQLCE";
            KaJourDAL.KaJour_Global_CE.SQLConnStr = "Data Source='" + this.textBoxSqlCe.Text + "'";

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            bool error = false;

            bool testNRecords = this.checkBoxTestNRecords.Checked;
            int testNRecordCount = -1;
            try
            {
                testNRecordCount = Convert.ToInt32(this.textBoxTestNRecords.Text);
            }
            catch (Exception)
            {
            }

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_CE.SQLProvider + ":");

            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            sqLITE.Connect();
            sqLITE.DisConnect();

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            sqlCe.Connect();
            DataTable tablesCE = sqlCe.GetTableList("", false);
            sqlCe.DisConnect();
            for (int iTable = 0; iTable < tablesCE.Rows.Count; iTable++)
            {
                countTables++;

                var tableName = tablesCE.Rows[iTable][0].ToString();
                sqlCe.Connect();
                var tableRec1 = sqlCe.GetTableRecCount(tableName);
                sqlCe.DisConnect();

                this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesCE.Rows.Count;
                this.toolStripStatusLabel2.Text = " " + (iTable + 1).ToString() + "/" + tablesCE.Rows.Count.ToString() + " " + tableName + " 0/" + tableRec1.ToString() + " ";
                Application.DoEvents();

                sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());

                // delete SQLite
                var del = sqLITE.DeleteBuilder(tableName);
                var retDel = sqLITE.ExecuteNonQuery("DELETE", del);

                var tableSelect = sqlCe.Execute("SELECT", "SELECT * FROM " + tableName);
                for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++)
                {
                    countRows++;

                    this.toolStripStatusLabel2.Text = " " + (iTable + 1).ToString() + "/" + tablesCE.Rows.Count.ToString() + " " + tableName + " " + (iRow + 1).ToString() + "/" + tableRec1.ToString() + " ";
                    // 1,2,3,4,5,6,7,8,9,10
                    // - 20,30,40,50,60,70,80,90,100
                    // - 200,300,400,500,600,700,800,900,1000
                    // - 2000,3000,4000,5000,6000,7000,8000,9000,10000
                    // - 20000,30000,40000,50000,60000,70000, ...
                    bool doEvents = false;
                    var iRowP1 = iRow + 1;
                    if (iRowP1 <= 10) {
                        doEvents = true;
                    }
                    else if (iRowP1 <= 100)
                    {
                        // mod 10
                        if ((iRowP1 % 10) == 0) { doEvents = true; }
                    }
                    else if (iRowP1 <= 1000)
                    {
                        // mod 100
                        if ((iRowP1 % 100) == 0) { doEvents = true; }
                    }
                    else if (iRowP1 <= 10000)
                    {
                        // mod 1000
                        if ((iRowP1 % 1000) == 0) { doEvents = true; }
                    }
                    else if (iRowP1 <= 100000)
                    {
                        // mod 10000
                        if ((iRowP1 % 10000) == 0) { doEvents = true; }
                    }
                    else if (iRowP1 <= 1000000)
                    {
                        // mod 100000
                        if ((iRowP1 % 100000) == 0) { doEvents = true; }
                    }
                    else if (iRowP1 <= 10000000)
                    {
                        // mod 1000000
                        if ((iRowP1 % 1000000) == 0) { doEvents = true; }
                    }

                    //
                    if (doEvents) {
                        Application.DoEvents();
                    }

                    // Test
                    if (testNRecords)
                    {
                        if (testNRecordCount > 0)
                        {
                            if (iRow >= testNRecordCount)
                            {
                                break;  //=================>
                            }
                        }
                    }

                    var par = sqlCe.InitParameterList();
                    string sqlFieldList = "";
                    string sqlValueList = "";
                    for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++)
                    {
                        var colVal = tableSelect.Rows[iRow][iCol];
                        var colName = tableSelect.Columns[iCol].ColumnName;
                        par.Add(colName, colVal);
                        // (Fld1) values (@Fld1)
                        if (sqlFieldList != "") { sqlFieldList += ","; }
                        sqlFieldList += " " + colName;

                        if (sqlValueList != "") { sqlValueList += ","; }
                        sqlValueList += " @" + colName;
                    }

                    // insert into SQLite
                    //var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
                    string sqlIns = sqLITE.InsertBuilder(tableName);    // "insert into Table"
                    // (Fld1) values (@Fld1)
                    sqlIns += " (" + sqlFieldList + ") VALUES (" + sqlValueList + ")";
                    var retIns = sqLITE.ExecuteNonQuery("INSERT", sqlIns, par);
                    var exc = sqLITE.GetException();
                    if (exc != null) {
                        MessageBox.Show("Error:" + exc.Message);
                        sb.AppendLine("--------------------");
                        sb.AppendLine("Error:" + exc.Message);
                        sb.AppendLine("--------------------");

                        error = true;
                        break;  //=================>
                    }

                    
                }
                if (error) {
                    break;  //=================>
                }
            }

            this.toolStripProgressBarTable.Value = 100;

            DateTime dtEnde = DateTime.Now;

            TimeSpan ts = dtEnde - dtStart;
            string timings = "Duration: " + dtStart.ToString("HH:mm:ss") + " - " + dtEnde.ToString("HH:mm:ss") + " -> " + ts.ToString();
            double RecPerSec = countRows / ts.TotalSeconds;
            sb.AppendLine("Count: Tables: " + countTables.ToString() + ", Rows: " + countRows.ToString() + ", Rec/Sec: " + RecPerSec.ToString());
            sb.AppendLine("" + timings);
            this.textBoxAction.Text = sb.ToString();

            this.toolStripStatusLabel2.Text = " Fertig: " + timings;

            return ret;
        }
    }
}
