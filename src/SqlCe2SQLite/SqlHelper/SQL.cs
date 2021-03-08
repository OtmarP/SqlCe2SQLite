using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Reflection;

// OpSoftware.OpTools.SQL
namespace KaJourDAL
{
    public class SQL
    {
        private bool _LogInfo = false;

        public enum SQLProvider
        {
            NULL,
            MSSQL,
            SQLCE,
            SQLITE
        };

        public enum SQLCE_RepairOption
        {
            DeleteCorruptedRows = 0,
            RecoverCorruptedRows = 1,
            RecoverAllPossibleRows = 1,
            RecoverAllOrFail = 2
        };

        public enum SQLCE_VerifyOption
        {
            Default = 0,
            Enhanced = 1
        };

        private string _SqlProvider;
        private string _SqlConnStr;

        private SQLProvider _SqlType;

        private SqlConnection _SqlDbConn;   // m_SQL_Connection;
        private SqlCeConnection _SqlCEConn;
        private SQLiteConnection _SqliteConn;
        // Exception
        private System.Exception _SqlException;

        private SQLiteTransaction _SqliteTransaction;
        private SQLiteCommand _SqliteCommand;
        //private SQLiteParameterCollection _SqliteParameterCollection;
        private SQLiteParameter[] _SqliteParameterArray;

        // ******************
        // Constructor
        // ******************
        public SQL(string sqlProvider, string sqlConnStr)
        {
            _SqlProvider = sqlProvider;
            _SqlConnStr = sqlConnStr;

            _SqlDbConn = new System.Data.SqlClient.SqlConnection();
            _SqlCEConn = new System.Data.SqlServerCe.SqlCeConnection();
            _SqliteConn = new SQLiteConnection();

            _SqlException = null;
            KaJourDAL.SQLError.Exception = null;
        }

        //~SQL()
        //{
        //    //throw new System.NotImplementedException();
        //}

        // ******************
        // Properties
        // ******************
        public string ConnectionString
        {
            get
            {
                if (_SqlType == SQLProvider.SQLCE)
                {
                    return _SqlCEConn.ConnectionString;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    return _SqlDbConn.ConnectionString;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    return _SqliteConn.ConnectionString;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            set
            {
                if (_SqlType == SQLProvider.SQLCE)
                {
                    _SqlCEConn.ConnectionString = value;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    _SqlDbConn.ConnectionString = value;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    _SqliteConn.ConnectionString = value;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public System.Data.ConnectionState State
        {
            get
            {
                if (_SqlType == SQLProvider.SQLCE)
                {
                    return _SqlCEConn.State;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    return _SqlDbConn.State;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    return _SqliteConn.State;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Database , needs: sql.Connect();
        /// </summary>
        public string Database
        {
            get
            {
                //this.Connect();

                if (_SqlType == SQLProvider.SQLCE)
                {
                    //if (this.State!= ConnectionState.Open)
                    //{
                    //    this.Connect();
                    //}
                    var ret = _SqlCEConn.Database;
                    //this.DisConnect();
                    // "C:\Users\op6\__OP\_LW_H\__OP\SDPP\KaJour\DB\KAJOUR4.sdf"
                    // "C:\Dev\TFS\Git\KaJourWin\KaJour\DB\KAJOUR4.sdf"
                    return ret;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    // "KAJOUR"
                    return _SqlDbConn.Database;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    var ret = _SqliteConn.Database; // "main"
                    ret += ", "+ _SqliteConn.FileName;
                    return ret;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// ServerVersion , needs: sql.Connect();
        /// </summary>
        public string ServerVersion
        {
            get
            {
                //this.Connect();

                if (_SqlType == SQLProvider.SQLCE)
                {
                    // "4.0.8482.1" // Toshiba
                    // "4.0.8876.1" // OPMEDION14B
                    var ret = _SqlCEConn.ServerVersion;
                    //this.DisConnect();
                    return ret;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    // "10.00.2531"
                    return _SqlDbConn.ServerVersion;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    return _SqliteConn.ServerVersion;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Assembly Version
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                if (_SqlType == SQLProvider.SQLCE)
                {
                    // Assembly Version
                    var typeOf = typeof(System.Data.SqlServerCe.SqlCeConnection);
                    var assemly = typeOf.Assembly;
                    var getName = assemly.GetName();
                    var version = getName.Version.ToString();

                    var ret = version;
                    return ret;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    //// "10.00.2531"
                    //return _SqlDbConn.ServerVersion;
                    throw new NotImplementedException();
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    var typeOf = typeof(System.Data.SQLite.SQLiteConnection);
                    var assemly = typeOf.Assembly;
                    var getName = assemly.GetName();
                    var version = getName.Version.ToString();

                    var ret = version;
                    return ret;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public string GetAssemblyLocation()
        {
            string fullPath = "";

            if (_SqlType == SQLProvider.SQLCE)
            {
                fullPath = System.Reflection.Assembly.GetAssembly(typeof(System.Data.SqlServerCe.SqlCeConnection)).Location;
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                fullPath = System.Reflection.Assembly.GetAssembly(typeof(System.Data.SQLite.SQLiteConnection)).Location;
            }
            else
            {
                throw new NotImplementedException();
            }

            return fullPath;
        }

        /// <summary>
        /// FileSize , needs: sql.Connect();
        /// </summary>
        public long FileSize
        {
            get
            {
                long sRet = 0;

                //this.Connect();

                if (_SqlType == SQLProvider.SQLCE)
                {
                    sRet = new System.IO.FileInfo(_SqlCEConn.Database).Length;
                    //this.DisConnect();
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    throw new NotImplementedException();    //sRet = -1;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    //sRet = new System.IO.FileInfo(_SqliteConn.Database).Length;
                    sRet = new System.IO.FileInfo(_SqliteConn.FileName).Length;
                }
                else
                {
                    throw new NotImplementedException();
                }

                return sRet;
            }
        }

        /// <summary>
        /// Size , needs: sql.Connect();
        /// </summary>
        public string Size
        {
            get
            {
                string sRet;

                if (_SqlType == SQLProvider.SQLCE)
                {
                    //new System.IO.FileInfo(oCEConn.Database).Length.ToString("#,##0")
                    sRet = new System.IO.FileInfo(_SqlCEConn.Database).Length.ToString("#,##0") + " Bytes";
                    return sRet;
                }
                else if (_SqlType == SQLProvider.MSSQL)
                {
                    // sp_helpdb kajour

                    //name    db_size       owner          dbid   created     status                                                                                                                                                                                                                        compatibility_level
                    //------- ------------- -------------- ------ ----------- ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- -------------------
                    //KAJOUR       29.38 MB TOSHIBA12\op6  5      Jan 24 2012 Status=ONLINE, Updateability=READ_WRITE, UserAccess=MULTI_USER, Recovery=SIMPLE, Version=655, Collation=Latin1_General_CI_AS, SQLSortOrder=0, IsAutoClose, IsAutoCreateStatistics, IsAutoUpdateStatistics, IsFullTextEnabled  100

                    //name        fileid filename                                                                           filegroup  size               maxsize            growth             usage
                    //----------- ------ ---------------------------------------------------------------------------------- ---------- ------------------ ------------------ ------------------ ---------
                    //KAJOUR      1      c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\KAJOUR.mdf     PRIMARY    24832 KB           Unlimited          1024 KB            data only
                    //KAJOUR_log  2      c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\KAJOUR_log.LDF NULL       5248 KB            2147483648 KB      10%                log only

                    string sSqlStr;
                    System.Data.SqlClient.SqlCommand olDbCmd;
                    System.Data.SqlClient.SqlDataReader olDbRS;
                    //string sRet;

                    sRet = "-1";

                    sSqlStr = "sp_helpdb kajour";
                    // "     29.38 MB"
                    olDbCmd = new System.Data.SqlClient.SqlCommand(sSqlStr, _SqlDbConn);
                    olDbCmd.CommandType = CommandType.Text;
                    olDbRS = olDbCmd.ExecuteReader();
                    while (olDbRS.Read())
                    {
                        sRet = olDbRS[1].ToString();
                    }
                    olDbRS.Close();
                    return sRet;
                }
                else if (_SqlType == SQLProvider.SQLITE)
                {
                    //sRet = new System.IO.FileInfo(_SqliteConn.Database).Length.ToString("#,##0") + " Bytes";
                    sRet = new System.IO.FileInfo(_SqliteConn.FileName).Length.ToString("#,##0") + " Bytes";
                    return sRet;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        // ret.Add(info + " DatabaseIdentifier: " + conn.DatabaseIdentifier);

        // ******************
        // Methods
        // ******************

        public string TopBuilder(string tableName, string select, string sql, int maxRecords)
        {
            // select: "SELECT "
            // sql: " * FROM {0} ORDER BY DAT DESC, TIM DESC"
            string ret = "";

            ret = string.Format(sql, tableName);

            string topRecords = " TOP " + maxRecords.ToString() + " ";
            if (maxRecords==-1) {
                topRecords = " ";
            }

            if (_SqlType == SQLProvider.SQLCE)
            {
                // SELECT TOP # * FROM...
                ret = select + topRecords + ret;
            }
            else if (_SqlType == SQLProvider.MSSQL) {
                // SELECT TOP # * FROM...
                ret = select + topRecords + ret;
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                // SELECT * FROM Table ORDER BY XY LIMIT #
                //  LIMIT row_count OFFSET offset;
                topRecords = " LIMIT " + maxRecords.ToString() + " ";
                if (maxRecords == -1)
                {
                    topRecords = " ";
                }
                ret = select + ret + topRecords;
            }

            return ret;
        }

        public string InsertBuilder(string tableName)
        {
            string ret = "INSERT " + tableName + " ";
            // @"INSERT {0}"

            if (_SqlType == SQLProvider.SQLITE){
                // INSERT INTO JOUT__LWU_OP
                ret = "INSERT INTO " + tableName + " ";
            }

            return ret;
        }

        /// <summary>
        /// "DELETE [FROM] <Table> "
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <returns>"DELETE [FROM] <Table> "</returns>
        public string DeleteBuilder(string tableName)
        {
            // @"DELETE {0}

            string ret = "DELETE " + tableName + " ";

            if (_SqlType == SQLProvider.SQLCE)
            {
                ret = "DELETE " + tableName + " ";
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                ret = "DELETE " + tableName + " ";
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                // SQLite
                // DELETE FROM table

                ret = "DELETE FROM " + tableName + " ";
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        public void SetProviderFromGlobalString(string sqlProvider)
        {
            //KaJourDAL.KaJour_Global.SQLProvider

            _SqlProvider = sqlProvider;

            if (sqlProvider == "SQLCE")
            {
                this.SetProvider(SQLProvider.SQLCE);
            }
            else if (sqlProvider == "MSSQL")
            {
                this.SetProvider(SQLProvider.MSSQL);
            }
            else if (sqlProvider == "SQLITE")
            {
                this.SetProvider(SQLProvider.SQLITE);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void SetProvider(SQLProvider sqlType)
        {
            //if (sqlType == SQLProvider.MSSQL)
            //{
            //    var debug = 1;
            //}

            _SqlType = sqlType;
        }

        /// <summary>
        /// GetDBInfo , needs: sql.Connect(); 
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string>> GetDBInfo(){
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            if (_SqlType == SQLProvider.SQLCE)
            {
                //this.Connect();

                var dbInfo = _SqlCEConn.GetDatabaseInfo();
                foreach (var item in dbInfo)
                {
                    var tuple = Tuple.Create("DatabaseInfo: " + item.Key, item.Value);

                    tuples.Add(tuple);
                }

                //this.DisConnect();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if(_SqlType == SQLProvider.SQLITE)
            {
                //var dbInfo = _SqliteConn.GetDatabaseInfo();

                //var dbInfo1 = SQLiteConnection.ProviderVersion
                //var dbInfo1 = SQLiteConnection.SharedFlags;
                //var dbInfo2 = SQLiteConnection.SQLiteCompileOptions;
                //var dbInfo3 = SQLiteConnection.SQLiteSourceId;
                //var dbInfo4 = SQLiteConnection.SQLiteVersion;

                // ToDo: SQLITE
            }
            else
            {
                throw new NotImplementedException();
            }

            return tuples;
        }

        // DB-Info:                                                   DB-Info:
        // SQLCE                                                      SQLITE
        // --------------------------------------------------------------------------------------------------------
        // AutoCommit: -                                              AutoCommit: True
        // BusyTimeout: -                                             BusyTimeout: 0
        // Changes: -                                                 Changes: 0
        // ConnectionString: Data Source=.\DATA\Test4.sdf             ConnectionString: Data Source=.\DATA\Test3.db3
        // ConnectionTimeout: 0                                       ConnectionTimeout: 15
        // Container: NULL                                            Container: NULL
        // Database: .\DATA\Test4.sdf                                 Database: main
        // DatabaseIdentifier: 591f4bff-c0d3-4199-88fb-711155a74d2f   DatabaseIdentifier: -
        // DataSource: .\DATA\Test4.sdf                               DataSource: Test3
        // DefaultDbType: -                                           DefaultDbType: 
        // DefaultTimeout: -                                          DefaultTimeout: 30
        // DefaultTypeName: -                                         DefaultTypeName: 
        // FileName: -                                                FileName: C:\Dev\Op\Git\Samples\NuGetConsumer\NuGetConsumer\bin\Debug\DATA\Test3.db3
        // Flags: -                                                   Flags: Default
        // LastInsertRowId: -                                         LastInsertRowId: 0
        // MemoryHighwater: -                                         MemoryHighwater: 57815
        // MemoryUsed: -                                              MemoryUsed: 57815
        // OwnHandle: -                                               OwnHandle: True
        // ParseViaFramework: -                                       ParseViaFramework: False
        // PoolCount: -                                               PoolCount: 0
        // PrepareRetries: -                                          PrepareRetries: 3
        // ProgressOps: -                                             ProgressOps: 0
        // ServerVersion: 4.0.8876.1                                  ServerVersion: 3.32.1
        // Site: NULL                                                 Site: NULL
        // State: Open                                                State: Open
        // VfsName: -                                                 VfsName: 
        // WaitTimeout: -                                             WaitTimeout: 30000
        // GetDatabaseInfo.Locale Identifier: 3079                    GetDatabaseInfo.: -
        // GetDatabaseInfo.Encryption Mode:                           GetDatabaseInfo.: -
        // GetDatabaseInfo.Case Sensitive: False                      GetDatabaseInfo.: -
        // --------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Set ConnectionString and .Open()
        /// </summary>
        public void Connect()
        {
            // string sqlProvider, string sqlConnStr

            //KaJourDAL.KaJour_Global.SQLProvider
            //KaJourDAL.KaJour_Global.SQLConnStr

            //_SqlProvider = sqlProvider;

            if (_SqlProvider == "SQLCE")
            {
                this.SetProvider(SQLProvider.SQLCE);
            }
            else if (_SqlProvider == "MSSQL")
            {
                this.SetProvider(SQLProvider.MSSQL);
            }
            else if (_SqlProvider == "SQLITE")
            {
                this.SetProvider(SQLProvider.SQLITE);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (_SqlType == SQLProvider.SQLCE)
            {
                _SqlCEConn.ConnectionString = _SqlConnStr;
                _SqlCEConn.Open();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                _SqlDbConn.ConnectionString = _SqlConnStr;
                _SqlDbConn.Open();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                _SqliteConn.ConnectionString = _SqlConnStr;
                _SqliteConn.Open();
            }
            else
            {
                throw new NotImplementedException();
            }

            //if (_SqlDbConn == null)
            //{
            //    _SqlDbConn = new SqlConnection();
            //    // Data Source=.\SQLEXPRESS;Initial Catalog=KAJOUR;Integrated Security=True;Pooling=False
            //    _SqlDbConn.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KAJOUR;Integrated Security=True;Pooling=False";
            //    _SqlDbConn.Open();
            //    if (_SqlDbConn.State == System.Data.ConnectionState.Open)
            //    {
            //        //return true;
            //    }
            //}
            //if (_SqlDbConn.State == System.Data.ConnectionState.Open)
            //{
            //    //return true;
            //}
            ////return false;
        }

        public void DisConnect()
        {
            if (_SqlType == SQLProvider.SQLCE)
            {
                _SqlCEConn.Close();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                _SqlDbConn.Close();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                _SqliteConn.Close();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // ToString()
        public string GetProviderType()
        {
            if (_SqlType == SQLProvider.SQLCE)
            {
                // "System.Data.SqlServerCe.SqlCeConnection"
                // _SqlCEConn.GetType().ToString()
                return _SqlCEConn.ToString();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                // "System.Data.SqlClient.SqlConnection"
                // _SqlDbConn.GetType().ToString()
                return _SqlDbConn.ToString();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                return _SqliteConn.ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public System.Exception GetException()
        {
            return _SqlException;
        }

        //sqlCeEngine.Compact()
        //sqlCeEngine.Upgrade()

        public bool Repair(/*string sqlProvider,*/ string sqlConnStr, SQLCE_RepairOption ceoption)
        {
            //string sqlProvider, string sqlConnStr

            // RepairOption option
            bool ret = false;

            RepairOption option;
            option = (RepairOption)ceoption;

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE)
            {
                try
                {
                    SqlCeEngine sqlCeEngine = new SqlCeEngine(sqlConnStr);
                    sqlCeEngine.Repair(null, option);
                    //RepairOption.DeleteCorruptedRows ... 0
                    //RepairOption.RecoverAllOrFail ... 2
                    //RecoverAllPossibleRows ... 1
                    //RecoverCorruptedRows ... 1 - deprecated
                    ret = true;
                }
                catch (Exception ex)
                {
                    this.Log("Repair", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                // sqlite3 new.db "PRAGMA integrity_check"

                // sqlite3 broken.db ".recover" | sqlite3 new.db

                //PRAGMA writable_schema=ON;
                //VACUUM;

                // ToDo: SQLITE Repair, PRAGMA integrity_check

                //System.Data.SQLite.SQLiteCommand olCECmd;
                //System.Data.SQLite.SQLiteDataReader olCERS;

                string sSqlStr;
                sSqlStr = "PRAGMA integrity_check";
                //olCECmd = new System.Data.SQLite.SQLiteCommand(sSqlStr, _SqliteConn);
                //olCECmd.CommandType = CommandType.Text;
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, sqlConnStr);
                try
                {
                    //olCERS = olCECmd.ExecuteReader();
                    int RA = sql.ExecuteNonQuery("Repair", sSqlStr);
                    if (RA == -1) {
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    this.Log("Repair", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        public bool Shrink(/*string sqlProvider,*/ string sqlConnStr)
        {
            // string sqlProvider, string sqlConnStr

            bool ret = false;

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE){
                try
                {
                    SqlCeEngine sqlCeEngine = new SqlCeEngine(sqlConnStr);
                    sqlCeEngine.Shrink();
                    ret = true;
                }
                catch (Exception ex)
                {
                    this.Log("Shrink", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                //sqlite3 /path/to/your/db/foo.db 'VACUUM;'

                // ToDo: SQLITE Shrink, vacuum

                string sSqlStr;
                sSqlStr = "vacuum";
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, sqlConnStr);
                try
                {
                    //cmd.CommandText = "vacuum";
                    //cmd.ExecuteNonQuery();
                    int RA = sql.ExecuteNonQuery("Shrink", sSqlStr);
                    if (RA == 0)
                    {
                        ret = true;
                    }
                }
                catch (Exception ex )
                {
                    this.Log("Shrink", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        public bool Verify(/*string sqlProvider,*/ string sqlConnStr, SQLCE_VerifyOption ceoption) {
            // string sqlProvider, string sqlConnStr

            // VerifyOption option
            bool ret = false;

            VerifyOption option;
            option = (VerifyOption)ceoption;

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE){
                try
                {
                    SqlCeEngine sqlCeEngine = new SqlCeEngine(sqlConnStr);
                    ret = sqlCeEngine.Verify(option);
                    //VerifyOption.Default ... 0
                    //VerifyOption.Enhanced) ... 1
                }
                catch (Exception ex)
                {
                    this.Log("Verify", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                //pragma integrity_check;

                //PRAGMA quick_check

                // ToDo: SQLITE Verify, PRAGMA integrity_check

                string sSqlStr;
                sSqlStr = "PRAGMA integrity_check";
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, sqlConnStr);
                try
                {
                    int RA = sql.ExecuteNonQuery("Verify", sSqlStr);
                    if (RA == -1)
                    {
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    this.Log("Verify", sqlConnStr, "", null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        /// <summary>
        /// GetTableList , needs: sql.Connect();
        /// </summary>
        /// <param name="spTableName"></param>
        /// <param name="bpExact"></param>
        /// <returns></returns>
        public System.Data.DataTable GetTableList(string spTableName, bool bpExact)
        {
            System.Data.DataTable oDT;
            string sSqlStr;

            _SqlException = null;

            oDT = new DataTable();
            oDT.TableName = "TABLES";

            if (_SqlType == SQLProvider.SQLCE)
            {
                System.Data.SqlServerCe.SqlCeCommand olCECmd;
                System.Data.SqlServerCe.SqlCeDataReader olCERS;

                sSqlStr = "";
                sSqlStr = sSqlStr + "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ";
                //sSqlStr = sSqlStr + " WHERE TABLE_TYPE = 'SYSTEM TABLE' ";
                if (spTableName != "")
                {
                    if (bpExact)
                    {
                        sSqlStr = sSqlStr + " WHERE TABLE_NAME = '" + spTableName + "' ";
                    }
                    else
                    {
                        sSqlStr = sSqlStr + " WHERE TABLE_NAME like '" + spTableName + "' ";
                    }
                }
                sSqlStr = sSqlStr + " order by TABLE_NAME asc ";

                olCECmd = new System.Data.SqlServerCe.SqlCeCommand(sSqlStr, _SqlCEConn);
                olCECmd.CommandType = CommandType.Text;
                olCERS = olCECmd.ExecuteReader();

                oDT.Load(olCERS);
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                System.Data.SqlClient.SqlCommand olDbCmd;
                System.Data.SqlClient.SqlDataReader olDbRS;

                sSqlStr = "";
                sSqlStr = sSqlStr + "select ";
                sSqlStr = sSqlStr + " * ";
                sSqlStr = sSqlStr + " from ";
                //sSqlStr = sSqlStr + " KAJOUR..sysobjects ";
                sSqlStr = sSqlStr + " sysobjects ";
                sSqlStr = sSqlStr + " where xtype='U' and status=0 ";
                //sSqlStr = sSqlStr + "  and name like 'JOUT%' ";
                if (spTableName != "")
                {
                    if (bpExact)
                    {
                        sSqlStr = sSqlStr + " and name = '" + spTableName + "' ";
                    }
                    else
                    {
                        sSqlStr = sSqlStr + " and name like '" + spTableName + "' ";
                    }
                }
                sSqlStr = sSqlStr + " order by name asc ";

                olDbCmd = new System.Data.SqlClient.SqlCommand(sSqlStr, _SqlDbConn);
                olDbCmd.CommandType = CommandType.Text;
                olDbRS = olDbCmd.ExecuteReader();

                oDT.Load(olDbRS);
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                System.Data.SQLite.SQLiteCommand olCECmd;
                System.Data.SQLite.SQLiteDataReader olCERS;

                sSqlStr = "";
                sSqlStr = sSqlStr + "SELECT name FROM sqlite_master WHERE type='table' ";
                //                  "SELECT name FROM sqlite_master WHERE type='table'"
                if (spTableName != "")
                {
                    if (bpExact)
                    {
                        sSqlStr = sSqlStr + " AND name = '" + spTableName + "' COLLATE NOCASE ";
                    }
                    else
                    {
                        sSqlStr = sSqlStr + " AND name like '" + spTableName + "' COLLATE NOCASE ";
                    }
                }
                sSqlStr = sSqlStr + " order by name COLLATE NOCASE asc ";

                olCECmd = new System.Data.SQLite.SQLiteCommand(sSqlStr, _SqliteConn);
                olCECmd.CommandType = CommandType.Text;
                olCERS = olCECmd.ExecuteReader();

                oDT.Load(olCERS);
            }
            else
            {
                throw new NotImplementedException();
            }

            return oDT;
        }

        public System.Data.DataTable GetColumnListSimple(string spTableName){
            System.Data.DataTable oDT;
            string sSqlStr;

            _SqlException = null;

            DataSet oDS = new DataSet();
            oDS.EnforceConstraints = false; // wegen Error:

            oDT = new DataTable();
            oDT.TableName = "COLUMNS";
            oDT.Constraints.Clear();

            oDS.Tables.Add(oDT);

            if (_SqlType == SQLProvider.SQLCE){
                System.Data.SqlServerCe.SqlCeCommand olCECmd;
                System.Data.SqlServerCe.SqlCeDataReader olCERS;

                // SQLCE
                // TABLE_NAME   COLUMN_NAME ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS IS_NULLABLE DATA_TYPE CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION NUMERIC_SCALE DATETIME_PRECISION
                // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                // JOUT_SDD__OP PMANDNR     1                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
                // JOUT_SDD__OP PCODE       2                0                 NULL           106          YES         nvarchar  3                        6                      NULL              NULL          NULL
                // JOUT_SDD__OP PICODE      3                0                 NULL           106          YES         nvarchar  1                        2                      NULL              NULL          NULL
                // JOUT_SDD__OP PTEXT       4                0                 NULL           106          YES         nvarchar  30                       60                     NULL              NULL          NULL
                // JOUT_SDD__OP PLW         5                0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
                // JOUT_SDD__OP PBETRAG     6                0                 NULL           122          YES         numeric   NULL                     NULL                   18                2             NULL
                // JOUT_SDD__OP PSOLL       7                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
                // JOUT_SDD__OP PHABEN      8                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
                // JOUT_SDD__OP PFELD       9                0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
                // JOUT_SDD__OP DEL         10               0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
                // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // SQLCE - Convert
                // TABLE_NAME   COLUMN_NAME ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS IS_NULLABLE DATA_TYPE CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION NUMERIC_SCALE DATETIME_PRECISION
                // ORDINAL_POSITION-1 COLUMN_NAME DATA_TYPE(CHARACTER_MAXIMUM_LENGTH)        IS_NULLABLE pk
                //                                         (NUMERIC_PRECISION,NUMERIC_SCALE) YES-0 No-1
                // ----------------------------------------------------------------------------------------
                // 0                  PMANDNR     nvarchar(7)                                0           .
                // 1                  PCODE       nvarchar(3)                                0           .
                // 2                  PICODE      nvarchar(1)                                0           .
                // 3                  PTEXT       nvarchar(30)                               0           .
                // 4                  PLW         int                                        0           .
                // 5                  PBETRAG     numeric(18,2)                              0           .
                // 6                  PSOLL       nvarchar(7)                                0           .
                // 7                  PHABEN      nvarchar(7)                                0           .
                // 8                  PFELD       int                                        0           .
                // 9                  DEL         int                                        0           .
                // ----------------------------------------------------------------------------------------

                sSqlStr = "";
                sSqlStr = sSqlStr + "SELECT ";
                sSqlStr = sSqlStr + " ORDINAL_POSITION ";
                sSqlStr = sSqlStr + " , COLUMN_NAME ";
                sSqlStr = sSqlStr + " , DATA_TYPE ";
                sSqlStr = sSqlStr + " , COLUMN_DEFAULT ";
                sSqlStr = sSqlStr + " , IS_NULLABLE ";
                sSqlStr = sSqlStr + " , CHARACTER_MAXIMUM_LENGTH ";
                sSqlStr = sSqlStr + " , NUMERIC_PRECISION ";
                sSqlStr = sSqlStr + " , NUMERIC_SCALE ";
                sSqlStr = sSqlStr + " FROM INFORMATION_SCHEMA.COLUMNS ";
                if (spTableName != ""){
                    // bpExact
                    sSqlStr = sSqlStr + " WHERE TABLE_NAME = '" + spTableName + "' ";

                }
                sSqlStr = sSqlStr + " ORDER BY TABLE_NAME, ORDINAL_POSITION";

                olCECmd = new System.Data.SqlServerCe.SqlCeCommand(sSqlStr, _SqlCEConn);
                olCECmd.CommandType = CommandType.Text;
                try
                {
                    oDT.Columns.Add("cid", typeof(Int64));
                    oDT.Columns.Add("name", typeof(string));
                    oDT.Columns.Add("type", typeof(string));
                    oDT.Columns.Add("notnull", typeof(Int64));
                    oDT.Columns.Add("dflt_value", typeof(string));
                    oDT.Columns.Add("pk", typeof(Int64));

                    olCERS = olCECmd.ExecuteReader();
                    //oDT.Load(olCERS);
                    while (olCERS.Read())
                    {
                        int ORDINAL_POSITION = (int)olCERS[0];
                        string COLUMN_NAME = olCERS[1].ToString();
                        string DATA_TYPE = olCERS[2].ToString();
                        string COLUMN_DEFAULT = null;
                        if (olCERS[3] != System.DBNull.Value)
                        {
                            COLUMN_DEFAULT = olCERS[3].ToString();
                        }
                        string IS_NULLABLE = olCERS[4].ToString();
                        int CHARACTER_MAXIMUM_LENGTH = -1;
                        if (olCERS[5]!=System.DBNull.Value) {
                            CHARACTER_MAXIMUM_LENGTH = (int)olCERS[5];
                        }
                        short NUMERIC_PRECISION =-1;
                        if (olCERS[6] != System.DBNull.Value)
                        {
                            NUMERIC_PRECISION = (short)olCERS[6];
                        }
                        short NUMERIC_SCALE =-1;
                        if (olCERS[7] != System.DBNull.Value)
                        {
                            NUMERIC_SCALE = (short)olCERS[7];
                        }

                        int cid = ORDINAL_POSITION - 1;
                        string name = COLUMN_NAME;
                        string type = DATA_TYPE;
                        if (DATA_TYPE == "nvarchar") {
                            type += "(" + CHARACTER_MAXIMUM_LENGTH.ToString() + ")";
                        }
                        if (DATA_TYPE == "numeric") {
                            type += "("+ NUMERIC_PRECISION.ToString() +","+ NUMERIC_SCALE.ToString() + ")";
                        }
                        int notnull = 0;
                        // YES ... 0
                        // NO .... 1
                        if (IS_NULLABLE== "NO") { notnull = 1; }
                        string dflt_value = COLUMN_DEFAULT;
                        int pk = 0;

                        oDT.Rows.Add(cid, name, type, notnull, dflt_value,pk);
                    }

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetColumnListSimple", spTableName, sSqlStr, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE){
                //PRAGMA table_info('table_name')
                System.Data.SQLite.SQLiteCommand olCECmd;
                System.Data.SQLite.SQLiteDataReader olCERS;

                // PRAGMA table_info('JOUT_SDD__OP')
                // cid name    type          notnull dflt_value pk
                // ---------------------------------------------------
                // 0   PMANDNR nvarchar(7)   0       NULL       0
                // 1   PCODE   nvarchar(3)   0       NULL       0
                // 2   PICODE  nvarchar(1)   0       NULL       0
                // 3   PTEXT   nvarchar(30)  0       NULL       0
                // 4   PLW     int           0       NULL       0
                // 5   PBETRAG numeric(18,2) 0       NULL       0
                // 6   PSOLL   nvarchar(7)   0       NULL       0
                // 7   PHABEN  nvarchar(7)   0       NULL       0
                // 8   PFELD   int           0       NULL       0
                // 9   DEL     int           0       NULL       0
                // ---------------------------------------------------

                sSqlStr = "";
                sSqlStr = sSqlStr + "PRAGMA table_info('" + spTableName + "') ";
                // ORDER ???

                olCECmd = new System.Data.SQLite.SQLiteCommand(sSqlStr, _SqliteConn);
                olCECmd.CommandType = CommandType.Text;
                try
                {
                    olCERS = olCECmd.ExecuteReader();
                    oDT.Load(olCERS);

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetColumnListSimple", spTableName, sSqlStr, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return oDT;
        }

        public System.Data.DataTable GetColumnsListMSSQL(string spTableName, bool bpExact)
        {
            System.Data.DataTable oDT;
            string sSqlStr;

            _SqlException = null;

            DataSet oDS = new DataSet();
            oDS.EnforceConstraints = false; // wegen Error:

            oDT = new DataTable();
            oDT.TableName = "COLUMNS";
            oDT.Constraints.Clear();

            oDS.Tables.Add(oDT);

            if (_SqlType == SQLProvider.SQLCE){
                // SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='adatcjou' ORDER BY ORDINAL_POSITION
                System.Data.SqlServerCe.SqlCeCommand olCECmd;
                System.Data.SqlServerCe.SqlCeDataReader olCERS;

                // select ... FROM INFORMATION_SCHEMA.COLUMNS 
                // TABLE_NAME   COLUMN_NAME ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS IS_NULLABLE DATA_TYPE CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION NUMERIC_SCALE DATETIME_PRECISION
                // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                // JOUT_BDD__OP BMANDNR     1                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
                // JOUT_BDD__OP BBKZ        2                0                 NULL           106          YES         nvarchar  4                        8                      NULL              NULL          NULL
                // JOUT_BDD__OP DEL         3                0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL

                // SQLCE
// TABLE_NAME   COLUMN_NAME ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS IS_NULLABLE DATA_TYPE CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION NUMERIC_SCALE DATETIME_PRECISION
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// JOUT_SDD__OP PMANDNR     1                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
// JOUT_SDD__OP PCODE       2                0                 NULL           106          YES         nvarchar  3                        6                      NULL              NULL          NULL
// JOUT_SDD__OP PICODE      3                0                 NULL           106          YES         nvarchar  1                        2                      NULL              NULL          NULL
// JOUT_SDD__OP PTEXT       4                0                 NULL           106          YES         nvarchar  30                       60                     NULL              NULL          NULL
// JOUT_SDD__OP PLW         5                0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
// JOUT_SDD__OP PBETRAG     6                0                 NULL           122          YES         numeric   NULL                     NULL                   18                2             NULL
// JOUT_SDD__OP PSOLL       7                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
// JOUT_SDD__OP PHABEN      8                0                 NULL           106          YES         nvarchar  7                        14                     NULL              NULL          NULL
// JOUT_SDD__OP PFELD       9                0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
// JOUT_SDD__OP DEL         10               0                 NULL           122          YES         int       NULL                     NULL                   10                NULL          NULL
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // SQLCE - Convert
// TABLE_NAME   COLUMN_NAME ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS IS_NULLABLE DATA_TYPE CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION NUMERIC_SCALE DATETIME_PRECISION
// ORDINAL_POSITION-1 COLUMN_NAME DATA_TYPE(CHARACTER_MAXIMUM_LENGTH)        IS_NULLABLE pk
//                                         (NUMERIC_PRECISION,NUMERIC_SCALE) YES-0 No-1
// ----------------------------------------------------------------------------------------
// 0                  PMANDNR     nvarchar(7)                                0           .
// 1                  PCODE       nvarchar(3)                                0           .
// 2                  PICODE      nvarchar(1)                                0           .
// 3                  PTEXT       nvarchar(30)                               0           .
// 4                  PLW         int                                        0           .
// 5                  PBETRAG     numeric(18,2)                              0           .
// 6                  PSOLL       nvarchar(7)                                0           .
// 7                  PHABEN      nvarchar(7)                                0           .
// 8                  PFELD       int                                        0           .
// 9                  DEL         int                                        0           .
// ----------------------------------------------------------------------------------------


                // TABLE_CATALOG TABLE_SCHEMA TABLE_NAME COLUMN_NAME
                // NULL          NULL         adatcjou   CC1

                // COLUMN_GUID COLUMN_PROPID ORDINAL_POSITION COLUMN_HASDEFAULT COLUMN_DEFAULT COLUMN_FLAGS
                // NULL        NULL          1                0                 NULL           106

                // IS_NULLABLE DATA_TYPE TYPE_GUID CHARACTER_MAXIMUM_LENGTH CHARACTER_OCTET_LENGTH NUMERIC_PRECISION
                // YES         nvarchar  NULL      4                        8                      NULL

                // NUMERIC_SCALE DATETIME_PRECISION CHARACTER_SET_CATALOG CHARACTER_SET_SCHEMA CHARACTER_SET_NAME COLLATION_CATALOG
                // NULL          NULL               NULL                  NULL                 NULL               NULL

                // COLLATION_SCHEMA COLLATION_NAME DOMAIN_CATALOG DOMAIN_SCHEMA DOMAIN_NAME DESCRIPTION AUTOINC_MIN AUTOINC_MAX
                // NULL             NULL           NULL           NULL          NULL        NULL        NULL        NULL

                // AUTOINC_NEXT AUTOINC_SEED AUTOINC_INCREMENT
                // NULL         NULL         NULL

                // COLUMN_DEFAULT = NULL
                // Error: Mindestens eine Zeile enthält Werte die die Einschränkungen non-null, unique or foreign-key verletzen.
                // Error: Failed to enable constraints. One or more rows contain values violating non-null, unique, or foreign-key constraints.

                sSqlStr = "";
                sSqlStr = sSqlStr + "SELECT ";
                sSqlStr = sSqlStr + " TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, COLUMN_HASDEFAULT ";
                sSqlStr = sSqlStr + " , COLUMN_DEFAULT ";   // Error bei oDT.Load(olCERS)
                sSqlStr = sSqlStr + " , COLUMN_FLAGS ";
                ////sSqlStr = sSqlStr + " , IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, CHARACTER_OCTET_LENGTH ";
                sSqlStr = sSqlStr + " , IS_NULLABLE ";
                sSqlStr = sSqlStr + " , DATA_TYPE ";
                sSqlStr = sSqlStr + " , CHARACTER_MAXIMUM_LENGTH ";   // Error bei oDT.Load(olCERS)
                sSqlStr = sSqlStr + " , CHARACTER_OCTET_LENGTH ";   // Error bei oDT.Load(olCERS)
                ////sSqlStr = sSqlStr + " , NUMERIC_PRECISION, NUMERIC_SCALE, DATETIME_PRECISION ";
                sSqlStr = sSqlStr + " , NUMERIC_PRECISION ";   // Error bei oDT.Load(olCERS)
                sSqlStr = sSqlStr + " , NUMERIC_SCALE ";   // Error bei oDT.Load(olCERS)
                sSqlStr = sSqlStr + " , DATETIME_PRECISION ";   // Error bei oDT.Load(olCERS)
                sSqlStr = sSqlStr + " FROM INFORMATION_SCHEMA.COLUMNS ";
                if (spTableName != ""){
                    if (bpExact)
                    {
                        sSqlStr = sSqlStr + " WHERE TABLE_NAME = '" + spTableName + "' ";
                    }
                    else {
                        sSqlStr = sSqlStr + " WHERE TABLE_NAME like '" + spTableName + "' ";
                    }
                }
                sSqlStr = sSqlStr + " ORDER BY TABLE_NAME, ORDINAL_POSITION";

                olCECmd = new System.Data.SqlServerCe.SqlCeCommand(sSqlStr, _SqlCEConn);
                olCECmd.CommandType = CommandType.Text;
                try
                {
                    olCERS = olCECmd.ExecuteReader();
                    oDT.Load(olCERS);

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetColumnsList", spTableName, sSqlStr, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                //PRAGMA table_info(table_name);
                //PRAGMA table_info('table_name')

                //.schema tablename

                //.headers ON
                //.mode column
                //select * from mytable;

                // ToDo: SQLITE / SQLCE GetColumnsList: Columns

                System.Data.SQLite.SQLiteCommand olCECmd;
                System.Data.SQLite.SQLiteDataReader olCERS;

                // PRAGMA table_info('JOUT_SDD__OP')
                // cid name    type          notnull dflt_value pk
                // ---------------------------------------------------
                // 0   PMANDNR nvarchar(7)   0       NULL       0
                // 1   PCODE   nvarchar(3)   0       NULL       0
                // 2   PICODE  nvarchar(1)   0       NULL       0
                // 3   PTEXT   nvarchar(30)  0       NULL       0
                // 4   PLW     int           0       NULL       0
                // 5   PBETRAG numeric(18,2) 0       NULL       0
                //...

                sSqlStr = "";
                sSqlStr = sSqlStr + "PRAGMA table_info('" + spTableName + "') ";
                // bpExact ... egal
                // ORDER ???

                olCECmd = new System.Data.SQLite.SQLiteCommand(sSqlStr, _SqliteConn);
                olCECmd.CommandType = CommandType.Text;
                try
                {
                    olCERS = olCECmd.ExecuteReader();
                    oDT.Load(olCERS);

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetColumnsList", spTableName, sSqlStr, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return oDT;
        }

        public bool TableExists(/*string sqlProvider, string sqlConnStr,*/ string spTableName)
        {
            bool ret = false;

            KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);
            sql.Connect();
            var tableList = sql.GetTableList(spTableName, false);
            _SqlException = sql._SqlException;
            sql.DisConnect();
            if (tableList != null)
            {
                if (tableList.Rows.Count == 1)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public int GetTableRecCount(string spTableName)
        {
            int nRet;
            string sSql2Str;

            _SqlException = null;

            nRet = -1;
            if (_SqlType == SQLProvider.SQLCE)
            {
                System.Data.SqlServerCe.SqlCeCommand olCE2Cmd;
                System.Data.SqlServerCe.SqlCeDataReader olCE2RS;

                sSql2Str = "";
                sSql2Str = sSql2Str + "select ";
                sSql2Str = sSql2Str + " count(1)";
                sSql2Str = sSql2Str + " from " + spTableName;
                olCE2Cmd = new System.Data.SqlServerCe.SqlCeCommand(sSql2Str, _SqlCEConn);
                try
                {
                    olCE2RS = olCE2Cmd.ExecuteReader();
                    olCE2RS.Read();

                    nRet = (int)olCE2RS[0];

                    olCE2RS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetTableRecCount", spTableName, sSql2Str, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                System.Data.SqlClient.SqlCommand olDb2Cmd;
                System.Data.SqlClient.SqlDataReader olDb2RS;

                sSql2Str = "";
                sSql2Str = sSql2Str + "select ";
                sSql2Str = sSql2Str + " count(1)";
                sSql2Str = sSql2Str + " from " + spTableName;
                olDb2Cmd = new System.Data.SqlClient.SqlCommand(sSql2Str, _SqlDbConn);
                try
                {
                    olDb2RS = olDb2Cmd.ExecuteReader();
                    olDb2RS.Read();

                    nRet = (int)olDb2RS[0];

                    olDb2RS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetTableRecCount", spTableName, sSql2Str, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                System.Data.SQLite.SQLiteCommand olCE2Cmd;
                System.Data.SQLite.SQLiteDataReader olCE2RS;

                sSql2Str = "";
                sSql2Str = sSql2Str + "select ";
                sSql2Str = sSql2Str + " count(1)";
                sSql2Str = sSql2Str + " from " + spTableName;
                olCE2Cmd = new System.Data.SQLite.SQLiteCommand(sSql2Str, _SqliteConn);
                try
                {
                    olCE2RS = olCE2Cmd.ExecuteReader();
                    olCE2RS.Read();

                    //nRet = (int)olCE2RS[0];    // SQLite: long
                    var x = olCE2RS[0];
                    nRet = Convert.ToInt32(x);

                    olCE2RS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("GetTableRecCount", spTableName, sSql2Str, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return nRet;
        }

        /// <summary>
        /// IndexExists , needs NO Connect
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public bool IndexExists(/*string sqlProvider, string sqlConnStr,*/ string tableName, string indexName)
        {
            bool ret = false;

            this.Connect();
            var index = this.GetTableIndexExists(/*sqlProvider, sqlConnStr,*/tableName, indexName);
            this.DisConnect();
            // -1 Error
            if (index>0) {
                ret = true;
            }

            return ret;
        }

        public int GetTableIndexExists(/*string sqlProvider, string sqlConnStr,*/ string tableName, string indexName)
        {
            int indexExists = -1;

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE)
            {
                // SELECT * FROM INFORMATION_SCHEMA.INDEXES where TABLE_NAME='JOUT_JDD__OP' and INDEX_NAME='JOUI_JD1__OP'
                // für jedes Feld ein Record

                string sqlCmd = 
@"SELECT TABLE_NAME, INDEX_NAME FROM INFORMATION_SCHEMA.INDEXES 
WHERE TABLE_NAME=@TABLE_NAME AND INDEX_NAME=@INDEX_NAME";

                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);

                var parameters = sql.InitParameterList();
                parameters.Add("@TABLE_NAME", tableName);
                parameters.Add("@INDEX_NAME", indexName);

                try
                {
                    DataTable SQLDT = sql.Execute("GetTableIndexExists", sqlCmd, parameters);
                    if (SQLDT.Rows.Count > 0)
                    {
                        indexExists = 1;
                    }
                    else {
                        indexExists = 0;
                    }

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("GetTableIndexExists", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                //var result = await conn.ExecuteScalarAsync<int>("SELECT count(*) FROM sqlite_master WHERE type='index' and name=?;", new string[] { "someIndexName" });

                // Fields: type name tbl_name rootpage sql
                string sqlCmd =
@"SELECT name FROM sqlite_master 
WHERE type='index' and tbl_name=@TABLE_NAME COLLATE NOCASE AND name=@INDEX_NAME COLLATE NOCASE";

                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);

                var parameters = sql.InitParameterList();
                parameters.Add("@TABLE_NAME", tableName);
                parameters.Add("@INDEX_NAME", indexName);

                try
                {
                    DataTable SQLDT = sql.Execute("GetTableIndexExists", sqlCmd, parameters);
                    if (SQLDT.Rows.Count > 0)
                    {
                        indexExists = 1;
                    }
                    else {
                        indexExists = 0;
                    }

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("GetTableIndexExists", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return indexExists;
        }

        /// <summary>
        /// DropIndex , needs: sql.Connect();
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        /// <returns>-1,0,1,2,3,4</returns>
        public int DropIndex(/*string sqlProvider, string sqlConnStr,*/ string tableName, string indexName) {
            int ret = -1;

            // DROP INDEX [TableName].IndexName
            // drop index [JOUT_JDD__OP].[JOUI_JD1__OP]
            // ALTER TABLE 'ABC' DROP CONSTRAINT 'idxABC'

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE)
            {
                string sqlCmd = "DROP INDEX [" + tableName + "]." + indexName;
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);
                try
                {
                    int RA = sql.ExecuteNonQuery("DropIndex", sqlCmd);
                    ret = RA;

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("DropIndex", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                // drop index TKto1
                // drop index if EXISTS TKto1

                string sqlCmd = "DROP INDEX [" + indexName + "]";
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);
                try
                {
                    int RA = sql.ExecuteNonQuery("DropIndex", sqlCmd);
                    if (RA==0) {
                        RA = -1;
                    }
                    ret = RA;

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("DropIndex", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        /// <summary>
        /// CreateIndex , needs: sql.Connect();
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        /// <param name="indexFields"></param>
        /// <param name="indexUnique"></param>
        /// <returns></returns>
        public int CreateIndex(/*string sqlProvider, string sqlConnStr,*/ string tableName, string indexName, string indexFields, int indexUnique)
        {
            int ret = -1;

            // CREATE UNIQUE INDEX [JOUI_JD2__OP] ON [JOUT_JDD__OP] ([JMANDNR], [JTEXT], [JIDATUM], [JZEILE])

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE){
                string unique = "";
                if (indexUnique == 1)
                {
                    unique = "UNIQUE";
                }
                string sqlCmd = "CREATE " + unique + " INDEX [" + indexName + "] ";
                sqlCmd += " ON [" + tableName + "](" + indexFields + ")";
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);
                try
                {
                    int RA = sql.ExecuteNonQuery("CreateIndex", sqlCmd);
                    // -1 ... OK
                    ret = RA;

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("CreateIndex", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL) {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                // CREATE [UNIQUE] INDEX index_name 
                // ON table_name(column_list);

                string unique = "";
                if (indexUnique == 1)
                {
                    unique = "UNIQUE";
                }
                string sqlCmd = "CREATE " + unique + " INDEX [" + indexName + "] ";
                sqlCmd += " ON [" + tableName + "](" + indexFields + ")";
                KaJourDAL.SQL sql = new KaJourDAL.SQL(_SqlProvider, _SqlConnStr);
                try
                {
                    int RA = sql.ExecuteNonQuery("CreateIndex", sqlCmd);
                    // 0 ... OK
                    if (RA==0) {
                        RA = -1;
                    }
                    ret = RA;

                    _SqlException = sql._SqlException;
                }
                catch (Exception ex)
                {
                    this.Log("CreateIndex", tableName + " " + indexName, sqlCmd, null, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        // ReiIndex

        //public List<SqlCeParameter> InitParameterList()
        //{
        //    List<SqlCeParameter> parameters = new List<SqlCeParameter>();
        //    return parameters;
        //}

        public Dictionary<string, object> InitParameterList()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            return parameters;
        }

        // using (var transaction = connection.BeginTransaction())
        public void BeginTransaction() {
            if (_SqlType == SQLProvider.SQLCE){
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE){
                this.Connect();
                _SqliteTransaction = _SqliteConn.BeginTransaction();
            }
        }

        public void EndTransaction() {
            if (_SqlType == SQLProvider.SQLCE)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                _SqliteTransaction.Commit();
                this.DisConnect();
            }
        }

        public void CreateCommand(string sqlCommand){
            if (_SqlType == SQLProvider.SQLCE)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                _SqliteCommand = _SqliteConn.CreateCommand();
                _SqliteCommand.CommandText = sqlCommand;
            }
        }

        public void AddParameters(List<string> parameterList){
            if (_SqlType == SQLProvider.SQLCE)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                _SqliteParameterArray = new SQLiteParameter[parameterList.Count];
                //_SqliteParameterArray = new SQLiteParameter[] { new SQLiteParameter("@val") };
                int i = 0;
                foreach (var item in parameterList)
                {
                    var parameter = _SqliteCommand.CreateParameter();
                    parameter.ParameterName = item;
                    _SqliteParameterArray[i] = parameter;
                    i++;
                }
                _SqliteCommand.Parameters.AddRange(_SqliteParameterArray);

                //parameter.Value = "nextVal";
                //command.ExecuteNonQuery();
            }
        }

        public void SetParameters(List<object> parameterList)
        {
            if (_SqlType == SQLProvider.SQLCE)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                //_SqliteParameterArray = new SQLiteParameter[parameterList.Count];
                //_SqliteParameterArray = new SQLiteParameter[] { new SQLiteParameter("@val") };
                int i = 0;
                foreach (var item in parameterList)
                {
                    //var parameter = _SqliteCommand.CreateParameter();
                    //parameter.ParameterName = item;
                    _SqliteParameterArray[i].Value = item;
                    i++;
                }
                //_SqliteCommand.Parameters.AddRange(_SqliteParameterArray);

                //parameter.Value = "nextVal";
            }
        }

        public int ExecuteNonQuery(string info)
        {
            int RA = -1;

            if (_SqlType == SQLProvider.SQLCE)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
            }
            else if (_SqlType == SQLProvider.SQLITE){
                RA = _SqliteCommand.ExecuteNonQuery();
                
                //command.ExecuteNonQuery();
            }

            return RA;
        }

        /// <summary>
        /// ExecuteNonQuery , needs NO Connect
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string info, string sql)
        {
            int RA;

            RA = ExecuteNonQuery(info, sql, null);

            return RA;
        }

        /// <summary>
        /// ExecuteNonQuery , needs NO Connect
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string info, string sql, Dictionary<string, object> parameters)
        {
            //List<SqlCeParameter> parameters

            int RA;

            RA = -1;

            this.Connect();

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE)
            {
                System.Data.SqlServerCe.SqlCeCommand olCECmd;
                olCECmd = new System.Data.SqlServerCe.SqlCeCommand(sql, _SqlCEConn);
                if (parameters != null)
                {
                    //olCECmd.Parameters.AddRange(parameters.ToArray());
                    foreach (var item in parameters)
                    {
                        olCECmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                if (_LogInfo) {
                    // Info-Log
                    this.Log("ExecuteNonQuery", info, sql, parameters, null);
                }

                try
                {
                    RA = olCECmd.ExecuteNonQuery();
                    // For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
                    // For all other types of statements, the return value is -1.
                }
                catch (Exception ex)
                {
                    this.Log("ExecuteNonQuery", info, sql, parameters, ex);

                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
                //SqlCommand olDbCmd;
                //olDbCmd = new SqlCommand(sql, _SqlDbConn);
                //RA = olDbCmd.ExecuteNonQuery();
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                System.Data.SQLite.SQLiteCommand olCECmd;
                olCECmd = new System.Data.SQLite.SQLiteCommand(sql, _SqliteConn);
                if (parameters != null){
                    foreach (var item in parameters)
                    {
                        olCECmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                if (_LogInfo) {
                    // Info-Log
                    this.Log("ExecuteNonQuery", info, sql, parameters, null);
                }

                try
                {
                    RA = olCECmd.ExecuteNonQuery();
                    // The number of rows inserted, updated, or deleted. -1 for SELECT statements.
                }
                catch (Exception ex)
                {
                    this.Log("ExecuteNonQuery", info, sql, parameters, ex);

                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            this.DisConnect();

            return RA;
        }

        /// <summary>
        /// Execute , needs NO Connect
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Execute(string info, string sql)
        {
            DataTable oDT;

            oDT = Execute(info,sql, new Dictionary<string, object>());

            return oDT;
        }

        //public DataTable ExecuteXXXSL(string sql, Dictionary<string, object> parameters)
        //{
        //    DataTable oDT;

        //    //List<SqlCeParameter> pl = new List<SqlCeParameter>();
        //    //if (parameters != null){
        //    //    foreach (var item in parameters)
        //    //    {
        //    //        pl.Add(new SqlCeParameter(item.Key, item.Value));
        //    //        //olCECmd.Parameters.AddWithValue(item.Key, item.Value);
        //    //    }

        //    //}
        //    oDT = Execute(sql, parameters);

        //    return oDT;
        //}

        /// <summary>
        /// Execute , needs NO Connect
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable Execute(string info, string sql, Dictionary<string, object> parameters)
        {
            // List<SqlCeParameter> parameters

            DataTable oDT;

            DataSet oDS = new DataSet();
            oDS.EnforceConstraints = false; // wegen Error:

            oDT = new DataTable();
            oDT.TableName = "RESULT";
            oDT.Constraints.Clear();

            oDS.Tables.Add(oDT);

            this.Connect();

            _SqlException = null;

            if (_SqlType == SQLProvider.SQLCE)
            {
                System.Data.SqlServerCe.SqlCeCommand olCECmd;
                System.Data.SqlServerCe.SqlCeDataReader olCERS;

                olCECmd = new System.Data.SqlServerCe.SqlCeCommand(sql, _SqlCEConn);
                if (parameters != null)
                {
                    //olCECmd.Parameters.AddRange(parameters.ToArray());
                    foreach (var item in parameters)
                    {
                        olCECmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                olCECmd.CommandType = CommandType.Text;

                if (_LogInfo) {
                    // Info-Log
                    this.Log("Execute", info, sql, parameters, null);
                }

                try
                {
                    olCERS = olCECmd.ExecuteReader();

                    oDT.Load(olCERS);

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("Execute", info, sql, parameters, ex);

                    oDT.TableName = "ERROR:";
                    _SqlException = ex;
                }
            }
            else if (_SqlType == SQLProvider.MSSQL)
            {
                throw new NotImplementedException();
                //SqlCommand oSQLCMD;
                //SqlDataReader oSQLDR;

                //oSQLCMD = new SqlCommand(sql, _SqlDbConn);
                //oSQLCMD.CommandType = CommandType.Text;
                //try
                //{
                //    oSQLDR = oSQLCMD.ExecuteReader();

                //    oDT.Load(oSQLDR);

                //    oSQLDR.Close();
                //}
                //catch (Exception ex)
                //{
                //    oDT.TableName = "ERROR:";
                //    _SqlException = ex;
                //}
            }
            else if (_SqlType == SQLProvider.SQLITE)
            {
                System.Data.SQLite.SQLiteCommand olCECmd;
                System.Data.SQLite.SQLiteDataReader olCERS;

                olCECmd = new System.Data.SQLite.SQLiteCommand(sql, _SqliteConn);
                if (parameters != null){
                    foreach (var item in parameters)
                    {
                        olCECmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                olCECmd.CommandType = CommandType.Text;

                if (_LogInfo) {
                    // Info-Log
                    this.Log("Execute", info, sql, parameters, null);
                }

                try
                {
                    olCERS = olCECmd.ExecuteReader();

                    oDT.Load(olCERS);
                    // Sql: SELECT * FROM JOUT_BDD__OP WHERE BMANDNR=@BMANDNR AND BBKZ=@BBKZ
                    // Error: Einschränkungen konnten nicht aktiviert werden. Mindestens eine Zeile enthält Werte die die Einschränkungen non-null, unique or foreign-key verletzen.

                    olCERS.Close();
                }
                catch (Exception ex)
                {
                    this.Log("Execute", info, sql, parameters, ex);

                    oDT.TableName = "ERROR:";
                    _SqlException = ex;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            this.DisConnect();

            return oDT;
        }

        public void Log(string command, string info, string sql, Dictionary<string, object> parameters, Exception ex)
        {
            string dataTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff");

            string line = command + ": Info:" + info + " Sql:" + sql;
            if (ex!=null) {
                line += " Error: " + ex.Message;
            }
            Console.WriteLine(line);

            //System.Diagnostics.Debug.WriteLine("Debug.WriteLine");

            //Trace.WriteLine("############################################## " + dataTime);
            //Trace.WriteLine(command + ": " + sql);
            //Trace.WriteLine("Error: " + ex.Message);
            //Trace.WriteLine("##############################################");

#if DEBUG
            var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logFile = Path.Combine(exeDir, "Log.txt");

            using (StreamWriter sw = File.AppendText(logFile))
            {
                string line2 = "------------------ " + _SqlType.ToString() + " ------------------";
                if (ex != null)
                {
                       line2 = "################## " + _SqlType.ToString() + " ##################";
                }
                StringBuilder parList = new StringBuilder();
                if (parameters != null) {
                    parList.AppendFormat("Par: {0} ", parameters.Count);

                    foreach (var item in parameters)
                    {
                        StringBuilder p = new StringBuilder();
                        p.AppendFormat("({0}='{1}')", item.Key, item.Value);

                        parList.Append(p);
                    }

                }
                sw.WriteLine(line2 + " " + dataTime);   // ##############################################
                sw.WriteLine(command + ": " + info + " " + parList.ToString());
                sw.WriteLine("Sql: " + sql);
                if (ex != null)
                {
                    sw.WriteLine("Error: " + ex.Message);
                }
                //sw.WriteLine(line2);    // ##############################################
                sw.WriteLine("");
            }
#endif
        }

        ///// <summary>
        ///// for SELECT, return SqlDataReader
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns>SqlDataReader</returns>
        //public SqlDataReader Execute___(string sql)
        //{
        //    this.Connect();

        //    SqlCommand oSQLCMD = new SqlCommand(sql, _SqlDbConn);
        //    SqlDataReader oSQLDR;
        //    oSQLDR = oSQLCMD.ExecuteReader();
        //    //oSQLDR.RecordsAffected

        //    if (oSQLDR.HasRows)
        //    {
        //        //oSQLDR.Read();
        //    }

        //    return oSQLDR;
        //}

        ///// <summary>
        ///// for UPDATE, return RecordsAffected
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns>RA ... RecordsAffected</returns>
        //public int ExecuteNQ___(string sql)
        //{
        //    this.Connect();

        //    SqlCommand oSQLCMD = new SqlCommand(sql, _SqlDbConn);
        //    int RA;
        //    RA = oSQLCMD.ExecuteNonQuery();

        //    return RA;
        //}
    }
}
