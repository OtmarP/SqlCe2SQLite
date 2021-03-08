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
            this.checkBoxTestNRecords.Checked = true;
            this.textBoxTestNRecords.Text = "50";
            this.checkBoxBulkInsert.Checked = true;

            this.Text = "SqlCe2SQLite v:2021.03.08";
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
            // Mo.08.03.2021 16:42:16 -op- v:2021.03.08
            //                             V2: Count: Tables: 22, Rows: 83935, Rec/Sec: 6044,44276733292 (Microsoft Surface Book)
            //                                 Duration: 16:18:09 - 16:18:23 -> 00:00:13.8863090
            // Mo.08.03.2021 11:44:48 -op- Performance mit Bulk insert (V2)
            // Mo.08.03.2021 11:43:29 -op- Display Data (1 Table, max. 20 Rows)
            // So.07.03.2021 18:37:39 -op- Open File-Dialog für .sdf und .db3 #3
            // So.07.03.2021 17:52:39 -op- Errorhandling verbessert #2
            //                             V1: Count: Tables: 22, Rows: 83935, Rec/Sec: 36,375030792145 (Microsoft Surface Book)
            //                                 Duration: 16:20:49 - 16:59:17 -> 00:38:27.4894556
            // So.07.03.2021 16:15:03 -op- Display Statistics (Tables, Rows, Rec/Sec, Duration) #1
            // So.07.03.2021 15:13:30 -op- Display Record# with modulo
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
            // .sdf
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = this.textBoxSqlCe.Text;
            fileDialog.Filter = "sdf files (*.sdf)|*.sdf|All files (*.*)|*.*";
            var ret = fileDialog.ShowDialog();
            if (ret == DialogResult.OK)
            {
                this.textBoxSqlCe.Text = fileDialog.FileName;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // .db3
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = this.textBoxSQLite.Text;
            fileDialog.Filter = "db3 files (*.db3)|*.db3|All files (*.*)|*.*";
            var ret = fileDialog.ShowDialog();
            if (ret == DialogResult.OK)
            {
                this.textBoxSQLite.Text = fileDialog.FileName;
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // Copy
            this.DisableEnabledControls(false);

            if (this.checkBoxBulkInsert.Checked)
            {
                var ret = this.CopyDataV2();
            }
            else {
                var ret = this.CopyData();
            }

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

        private void buttonDispData_Click(object sender, EventArgs e)
        {
            // Disp Data
            this.DisableEnabledControls(false);

            var ret = this.DispTarget();

            this.DisableEnabledControls(true);
        }

        private void DisableEnabledControls(bool enabled)
        {
            this.buttonExit.Enabled = enabled;
            this.buttonStatus.Enabled = enabled;
            this.buttonDelTarget.Enabled = enabled;
            this.buttonDispData.Enabled = enabled;
            this.buttonCopy.Enabled = enabled;
            Application.DoEvents();
        }

        private bool DispStatus()
        {
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            int countTables = 0;
            int countRows = 0;

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
            sb.AppendLine(KaJourDAL.KaJour_Global_CE.SQLProvider + ":");

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            DataTable tablesCE = null;
            try
            {
                sqlCe.Connect();
                tablesCE = sqlCe.GetTableList("", false);
                sqlCe.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesCE != null)
            {
                for (int iTable = 0; iTable < tablesCE.Rows.Count; iTable++)
                {
                    countTables++;

                    var tableName = tablesCE.Rows[iTable][0].ToString();
                    sqlCe.Connect();
                    var tableRec1 = sqlCe.GetTableRecCount(tableName);
                    sqlCe.DisConnect();

                    countRows += tableRec1;

                    this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesCE.Rows.Count;
                    Application.DoEvents();

                    sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());
                }
            }
            sb.AppendLine("   Count: Tables: " + countTables.ToString() + ", Rows: " + countRows.ToString());

            countTables = 0;
            countRows = 0;

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");

            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            DataTable tablesLITE = null;
            try
            {
                sqLITE.Connect();
                tablesLITE = sqLITE.GetTableList("", false);
                sqLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesLITE != null)
            {
                for (int iTable = 0; iTable < tablesLITE.Rows.Count; iTable++)
                {
                    countTables++;

                    var tableName = tablesLITE.Rows[iTable][0].ToString();
                    sqLITE.Connect();
                    var tableRec1 = sqLITE.GetTableRecCount(tableName);
                    sqLITE.DisConnect();

                    countRows += tableRec1;

                    this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesLITE.Rows.Count;
                    Application.DoEvents();

                    sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());
                }
            }
            sb.AppendLine("  Count: Tables: " + countTables.ToString() + ", Rows: " + countRows.ToString());

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }

        private bool DelTarget()
        {
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            int countTables = 0;
            int countRows = 0;

            StringBuilder sb = new StringBuilder();

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");

            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            DataTable tablesLITE = null;
            try
            {
                sqLITE.Connect();
                tablesLITE = sqLITE.GetTableList("", false);
                sqLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesLITE != null)
            {
                for (int iTable = 0; iTable < tablesLITE.Rows.Count; iTable++)
                {
                    countTables++;

                    this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesLITE.Rows.Count;
                    Application.DoEvents();

                    var tableName = tablesLITE.Rows[iTable][0].ToString();
                    sqLITE.Connect();
                    var tableRec1 = sqLITE.GetTableRecCount(tableName);
                    sqLITE.DisConnect();

                    countRows += tableRec1;

                    // Delete
                    var del = sqLITE.DeleteBuilder(tableName);
                    var retDel = sqLITE.ExecuteNonQuery("DELETE", del);

                    sqLITE.Connect();
                    var tableRec2 = sqLITE.GetTableRecCount(tableName);
                    sqLITE.DisConnect();

                    sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString() + "   Del:" + retDel.ToString() + "   Rec:" + tableRec2.ToString());
                }
            }
            sb.AppendLine("  Count: Tables: " + countTables.ToString() + ", Rows: " + countRows.ToString());

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }

        private bool DispTarget()
        {
            bool ret = false;

            this.textBoxAction.Text = "";
            this.toolStripProgressBarTable.Value = 0;
            Application.DoEvents();

            int countTables = 0;
            int countRows = 0;

            StringBuilder sb = new StringBuilder();

            KaJourDAL.KaJour_Global_LITE.SQLProvider = "SQLITE";
            KaJourDAL.KaJour_Global_LITE.SQLConnStr = "Data Source='" + this.textBoxSQLite.Text + "'";

            // ##############################################
            sb.AppendLine(KaJourDAL.KaJour_Global_LITE.SQLProvider + ":");

            var sqLITE = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_LITE.SQLProvider, KaJourDAL.KaJour_Global_LITE.SQLConnStr);
            DataTable tablesLITE = null;
            try
            {
                sqLITE.Connect();
                tablesLITE = sqLITE.GetTableList("", false);
                sqLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesLITE != null)
            {
                for (int iTable = 0; iTable < tablesLITE.Rows.Count; iTable++)
                {
                    countTables++;

                    var tableName = tablesLITE.Rows[iTable][0].ToString();
                    sqLITE.Connect();
                    var tableRec1 = sqLITE.GetTableRecCount(tableName);
                    sqLITE.DisConnect();

                    this.toolStripProgressBarTable.Value = ((iTable + 1) * 100) / tablesLITE.Rows.Count;
                    Application.DoEvents();

                    sb.AppendLine("  " + tableName + "   Rec:" + tableRec1.ToString());

                    //countRows += tableRec1;

                    // Display
                    var sel = "SELECT * FROM " + tableName;
                    DataTable tableSelect = sqLITE.Execute("SELECT", sel);
                    for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++)
                    {
                        countRows++;

                        for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++)
                        {
                            var colVal = tableSelect.Rows[iRow][iCol];
                            sb.Append(colVal);
                            sb.Append(", ");
                        }
                        sb.AppendLine("");

                        if (iRow >= 20)
                        {
                            break;  //====================>
                        }
                    }

                    if (iTable >= 0)
                    {
                        break;  //====================>
                    }
                }
            }

            this.textBoxAction.Text = sb.ToString();
            this.toolStripProgressBarTable.Value = 100;

            return ret;
        }

        private bool CopyDataV2(){

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
            try
            {
                sqLITE.Connect();
                sqLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            DataTable tablesCE = null;
            try
            {
                sqlCe.Connect();
                tablesCE = sqlCe.GetTableList("", false);
                sqlCe.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesCE != null){
                for (int iTable = 0; iTable < tablesCE.Rows.Count; iTable++){
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

                    bool paraOk = false;
                    //var par = sqlCe.InitParameterList();
                    string sqlFieldList = "";
                    string sqlValueList = "";
                    string sqlIns = sqLITE.InsertBuilder(tableName);    // "insert into Table"

                    // using (var transaction = connection.BeginTransaction())
                    // var command = connection.CreateCommand();
                    // command.CommandText = "INSERT INTO data VALUES ($value)"
                    // var parameter = command.CreateParameter();
                    // parameter.ParameterName = "$value";
                    // command.Parameters.Add(parameter);
                    // // Insert a lot of data
                    //  parameter.Value = random.Next();
                    //  command.ExecuteNonQuery();
                    // transaction.Commit();

                    sqLITE.BeginTransaction();

                    var tableSelect = sqlCe.Execute("SELECT", "SELECT * FROM " + tableName);
                    for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++){
                        countRows++;

                        // Parameter
                        //par = sqlCe.InitParameterList();
                        if (!paraOk){
                            // Parameter
                            List<string> paramList = new List<string>();

                            sqlFieldList = "";
                            sqlValueList = "";
                            for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++){
                                var colVal = tableSelect.Rows[iRow][iCol];
                                var colName = tableSelect.Columns[iCol].ColumnName;

                                //par.Add(colName, colVal);
                                paramList.Add(colName);

                                // (Fld1) values (@Fld1)
                                if (sqlFieldList != "") { sqlFieldList += ","; }
                                sqlFieldList += " " + colName;

                                if (sqlValueList != "") { sqlValueList += ","; }
                                sqlValueList += " @" + colName;
                            }

                            // insert into SQLite
                            //sqlIns += " (" + sqlFieldList + ") VALUES (" + sqlValueList + ")";  // " (Fld1) values (@Fld1)"
                            sqlIns += " VALUES (" + sqlValueList + ")";  // " values (@Fld1)"

                            sqLITE.CreateCommand(sqlIns);
                            sqLITE.AddParameters(paramList);

                            paraOk = true;
                        }

                        List<object> paramValues = new List<object>();
                        for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++){
                            var colVal = tableSelect.Rows[iRow][iCol];
                            var colName = tableSelect.Columns[iCol].ColumnName;

                            //par[colName] = colVal;
                            paramValues.Add(colVal);
                        }
                        sqLITE.SetParameters(paramValues);

                        this.toolStripStatusLabel2.Text = " " + (iTable + 1).ToString() + "/" + tablesCE.Rows.Count.ToString() + " " + tableName + " " + (iRow + 1).ToString() + "/" + tableRec1.ToString() + " ";
                        bool doEvents = CalcModulo(iRow);
                        if (doEvents)
                        {
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

                        // Insert
                        var retIns = sqLITE.ExecuteNonQuery("INSERT");
                        var exc = sqLITE.GetException();
                        if (exc != null)
                        {
                            MessageBox.Show("Error:" + exc.Message);
                            sb.AppendLine("--------------------");
                            sb.AppendLine("Error:" + exc.Message);
                            sb.AppendLine("--------------------");

                            error = true;
                            break;  //=================>
                        }
                    }
                    if (error)
                    {
                        break;  //=================>
                    }

                    sqLITE.EndTransaction();

                    ret = true;
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
            try
            {
                sqLITE.Connect();
                sqLITE.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }

            var sqlCe = new KaJourDAL.SQL(KaJourDAL.KaJour_Global_CE.SQLProvider, KaJourDAL.KaJour_Global_CE.SQLConnStr);
            DataTable tablesCE = null;
            try
            {
                sqlCe.Connect();
                tablesCE = sqlCe.GetTableList("", false);
                sqlCe.DisConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ret;
            }
            if (tablesCE != null)
            {
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

                    bool paraOk = false;
                    var par = sqlCe.InitParameterList();
                    string sqlFieldList = "";
                    string sqlValueList = "";
                    string sqlIns = sqLITE.InsertBuilder(tableName);    // "insert into Table"

                    var tableSelect = sqlCe.Execute("SELECT", "SELECT * FROM " + tableName);
                    for (int iRow = 0; iRow < tableSelect.Rows.Count; iRow++)
                    {
                        countRows++;

                        // Parameter
                        //par = sqlCe.InitParameterList();
                        if (!paraOk)
                        {
                            // Parameter
                            sqlFieldList = "";
                            sqlValueList = "";
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
                            sqlIns += " (" + sqlFieldList + ") VALUES (" + sqlValueList + ")";  // (Fld1) values (@Fld1)

                            //...

                            paraOk = true;
                        }

                        for (int iCol = 0; iCol < tableSelect.Columns.Count; iCol++)
                        {
                            var colVal = tableSelect.Rows[iRow][iCol];
                            var colName = tableSelect.Columns[iCol].ColumnName;
                            //par.Add(colName, colVal);

                            par[colName] = colVal;
                        }

                        this.toolStripStatusLabel2.Text = " " + (iTable + 1).ToString() + "/" + tablesCE.Rows.Count.ToString() + " " + tableName + " " + (iRow + 1).ToString() + "/" + tableRec1.ToString() + " ";

                        bool doEvents = CalcModulo(iRow);
                        if (doEvents)
                        {
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

                        // Insert
                        var retIns = sqLITE.ExecuteNonQuery("INSERT", sqlIns, par);
                        var exc = sqLITE.GetException();
                        if (exc != null)
                        {
                            MessageBox.Show("Error:" + exc.Message);
                            sb.AppendLine("--------------------");
                            sb.AppendLine("Error:" + exc.Message);
                            sb.AppendLine("--------------------");

                            error = true;
                            break;  //=================>
                        }
                    }
                    if (error)
                    {
                        break;  //=================>
                    }

                    ret = true;
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

        private bool CalcModulo(int iRow)
        {
            bool doEvents = false;

            // - 1,2,3,4,5,6,7,8,9,10
            // - 20,30,40,50,60,70,80,90,100
            // - 200,300,400,500,600,700,800,900,1000
            // - 2000,3000,4000,5000,6000,7000,8000,9000,10000
            // - 20000,30000,40000,50000,60000,70000, ...
            var iRowP1 = iRow + 1;
            if (iRowP1 <= 10)
            {
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
            // ...

            return doEvents;
        }
    }
}
