using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// OpSoftware.OpTools.DataGridHelper
// OpSoftware.OpLib
namespace KaJourHelper
{
    public class DataGridHelper
    {
        DataGridView _grid;
        Font _defFont;
        Graphics _gr;

        /// <summary>
        /// _dgh = new DataGridHelper()
        /// </summary>
        /// <param name="grid">DataGridView</param>
        /// <param name="gr">var gr = CreateGraphics()</param>
        public DataGridHelper(DataGridView grid, Graphics gr)
        {
            _grid = grid;
            _defFont = grid.DefaultCellStyle.Font;
            _gr = gr;
        }

        /// <summary>
        /// Grid Init
        /// </summary>
        /// <returns></returns>
        public DataGridView Init()
        {
            // Init dataGrid
            // Enable Adding
            _grid.AllowUserToAddRows = false;
            // Enable Editing
            _grid.ReadOnly = false;
            // Enable Delete
            _grid.AllowUserToDeleteRows = false;
            // Enable Col Reorder
            _grid.AllowUserToOrderColumns = true;
            // Resize-Rows
            _grid.AllowUserToResizeRows = false;

            _grid.Rows.Clear();
            _grid.Columns.Clear();
            _grid.RowTemplate.Height = 20;   // def: 22

            _grid.RowHeadersWidth = 24;

            _grid.RowTemplate.DefaultCellStyle.Format = ""; // war auf "N2"

            return _grid;
        }

        /// <summary>
        /// Add Colum (format)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headerText"></param>
        /// <param name="textForWidth"></param>
        /// <param name="align"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public DataGridView ColumAdd(string name, string headerText, string textForWidth, string align, string format)
        {
            //var defFont = this.dataGridViewJourDay.DefaultCellStyle.Font;

            DataGridViewTextBoxColumn col;
            col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                Width = this.TextSize(textForWidth, _defFont)
            };
            if (align == "Alignment_MiddleRight")
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (format != "")
            {
                col.DefaultCellStyle.Format = format;
            }
            _grid.Columns.Add(col);

            return _grid;
        }

        /// <summary>
        /// Add Colum (hidden)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headerText"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public DataGridView ColumAdd(string name, string headerText, bool visible)
        {
            _grid.Columns.Add(name, headerText);
            _grid.Columns[name].Visible = visible;

            return _grid;
        }

        private int TextSize(string text, Font font)
        {
            var ret = 5;

            //var gr = CreateGraphics();
            var tSize = _gr.MeasureString(text, font);

            ret = (int)tSize.Width;

            return ret;
        }

        /// <summary>
        /// Summe | Datums-Differenz in Tagen
        /// </summary>
        /// <returns></returns>
        public string GetCellSumOrDateDiff(bool getDateDiff, bool addNow)
        {
            //DataGridViewSelectedCellCollection selectedCells, 

            string disp = "";

            var selectedCells = _grid.SelectedCells;

            //DataGridViewSelectedCellCollection selectedCells = _selectedCells;
            if (selectedCells.Count == 0)
            {
                return disp;
            }
            if (selectedCells[0].ColumnIndex == 0)
            {
                return disp;
            }
            disp = selectedCells.Count.ToString() + " Selected:";

            if (getDateDiff)
            {
                // wenn 2 Datum(0,1) + now -> Datums-Differenz in Tagen
                // wenn 1 Datum(0) + now
                if (selectedCells.Count >= 1)
                {
                    DateTime dt1;
                    DateTime dt2;
                    DateTime dtx;
                    SortedSet<DateTime> ts = new SortedSet<DateTime>();

                    var value = selectedCells[0].Value;
                    string valueString = "";
                    if (value != null)
                    {
                        valueString = value.ToString();
                    }
                    if (DateTime.TryParse(valueString, out dtx))
                    {
                        ts.Add(dtx);// 1.
                        dt2 = dtx;

                        //if (selectedCells.Count>=3) {
                        //    // Debug
                        //    var debug = true;
                        //}

                        for (int i = 0; i < selectedCells.Count; i++)
                        {
                            value = selectedCells[i].Value;
                            valueString = "";
                            if (value != null)
                            {
                                valueString = value.ToString();
                            }
                            if (DateTime.TryParse(valueString, out dtx))
                            {
                                ts.Add(dtx);    // 2...n
                            }
                        }

                        if (addNow)
                        {
                            dtx = DateTime.Now.Date;
                            ts.Add(dtx);    // n+1
                        }

                        //...
                        //}

                        ////-------------------------------------------------
                        ////var value = selectedCells[0].Value;
                        ////string valueString = "";
                        //if (value != null)
                        //{
                        //    valueString = value.ToString();
                        //}

                        //DateTime dt1;
                        //DateTime dt2;
                        //DateTime dtx;
                        ////Convert.ToDateTime(value)
                        //if (DateTime.TryParse(valueString, out dt1))
                        //{
                        //value = selectedCells[1].Value;
                        //valueString = "";
                        //if (value != null)
                        //{
                        //    valueString = value.ToString();
                        //}
                        //if (DateTime.TryParse(valueString, out dt2))
                        //{
                        //SortedSet<DateTime> ts = new SortedSet<DateTime>();
                        //ts.Add(dt1);
                        //ts.Add(dt2);

                        //if (selectedCells.Count >= 3)
                        //{
                        //    for (int i = 0; i < selectedCells.Count; i++)
                        //    {
                        //        value = selectedCells[i].Value;
                        //        valueString = "";
                        //        if (value != null)
                        //        {
                        //            valueString = value.ToString();
                        //        }
                        //        if (DateTime.TryParse(valueString, out dtx))
                        //        {
                        //            ts.Add(dtx);
                        //        }
                        //    }
                        //}

                        //dt2 = DateTime.Now.Date;
                        //ts.Add(dt2);

                        // 0      1        2
                        // 1.1. - 10.1   - 20.1.
                        // 2      2->1 2   2->1 2
                        int count = 0;
                        foreach (var item in ts)
                        {
                            if (count == 0)
                            {
                                disp = StringHelper.StrAdd(disp, " ", item.ToString("dd.MM.yyyy"));
                                dt2 = item;
                            }
                            else
                            {
                                dt1 = dt2;
                                dt2 = item;

                                double numberOfDays = (dt2 - dt1).TotalDays;
                                disp = StringHelper.StrAdd(disp, " ", "- " + numberOfDays.ToString() + " -");
                                disp = StringHelper.StrAdd(disp, " ", item.ToString("dd.MM.yyyy"));
                            }

                            count++;
                        }

                        return disp;
                    }
                }
            }

            decimal sum = 0;
            for (int i = 0; i < selectedCells.Count; i++)
            {
                var value = selectedCells[i].Value;

                //decimal.TryParse()
                decimal betr = 0;
                //string error = "";
                //try
                //{
                //    betr = Convert.ToDecimal(value);
                //    sum = sum + betr;
                //}
                ////catch (Exception ex) { error = ex.Message; }
                //finally { }
                if (value != null)
                {
                    betr = KaJourHelper.StringHelper.StringToNumber(value.ToString());
                }
                sum = sum + betr;
            }

            disp = KaJourHelper.StringHelper.StrAdd(disp, " ", sum.ToString("#,##0.00")); // ("#,##0.00")

            return disp;
        }
    }

    /*
     Usage:

     KaJourHelper.DataGridHelper _dgh;
     private bool _dispRowNumber = true;

        this.InitGrid();
        this.SetGridWithInitData();

     private void InitGrid(){
       Graphics gr = CreateGraphics();
       _dgh = new KaJourHelper.DataGridHelper(this.dataGridView1, gr);
       _dgh.Init();

       if (_dispRowNumber){
         _dgh.ColumAdd("RowNumber", "Row", "1234_", "Alignment_MiddleRight", "#,##0");
       }
       _dgh.ColumAdd("Nr", "Nr.", "0001__", "", "");
       _dgh.ColumAdd("DEL", "*", "*_", "", "");
       _dgh.ColumAdd("Mandant", "Mandant", "1999__", "", "");
       _dgh.ColumAdd("Info", "Info", "Standardbuchungen          P____", "", "");
       _dgh.ColumAdd("Datei", "Datei/Table", "*JOUT__MANDANT_OP____", "", "");
       _dgh.ColumAdd("SaetzeMandant", "Sätze/Mandant", "123456789__", "Alignment_MiddleRight", "#,##0");
       _dgh.ColumAdd("Saetze", "Sätze", "123456789__", "Alignment_MiddleRight", "#,##0");
       _dgh.ColumAdd("Menge", "Menge", "999.999,99_", "Alignment_MiddleRight", "#,##0.00");
       _dgh.ColumAdd("Betrag", "Betrag", "99.999.999,99_", "Alignment_MiddleRight", "#,##0.00");
       _dgh.ColumAdd("HDatum", "HDatum", false);    _ColKey1 = "HDatum";// hidden
       _dgh.ColumAdd("HBuZeile", "HBuZeile", false);    _ColKey2 = "HBuZeile";// hidden
     }
     private void SetGridWithInitData(){
       foreach (KaJourDAL.Repository.SysControlFullRecord sysControlItem in sysControlListe){
         int newRow = this.dataGridViewTables.Rows.Add();
         this.dataGridViewTables.Rows[newRow].Cells["Nr."].Value = sysControlItem.SysControlKey.CC1;
         this.dataGridViewTables.Rows[newRow].Cells["Info"].Value = sysControlItem.SysControlRecord.CCHINWEIS;
         this.dataGridViewTables.Rows[newRow].Cells["Datei"].Value = tableName;
         this.dataGridViewTables.Rows[newRow].Cells["Index"].Value = "";
         this.dataGridViewTables.Rows[newRow].Cells["Aktion"].Value = "";
         if (ccLw.Substring(1, 1) == "*"){xDel = true;}

         bool toggle = (zeile % 2) == 0;

         // Zeilenfinder
         if (toggle){
           this.dataGridViewTables.Rows[newRow].DefaultCellStyle.BackColor = Color.LightCyan;
         }
         if (xDel == true){
           this.dataGridViewTables.Rows[newRow].DefaultCellStyle.BackColor = Color.LightGray;
         }

         zeile++;
       }
     }
     private void CalcMengenGeruest(){
       // Grid-Clear
       this.dataGridView1.Rows.Clear();

       int zeile = 0;

       int newRow;
       foreach (var item in mGLiset){
         bool toggle = (zeile % 2) == 0;

         // Grid-Data
         newRow = this.dataGridView1.Rows.Add();
         if (_dispRowNumber){
            this.dataGridView1.Rows[newRow].Cells["RowNumber"].Value = newRow + 1;
            //this.dataGridView1.Rows[newRow].Cells["RowNumber"].ToolTipText = "Info:\n" + infoTooltip;
         }
         this.dataGridView1.Rows[newRow].Cells["Nr"].Value = item.Nr;
         this.dataGridView1.Rows[newRow].Cells["Mandant"].Value = item.Mandant;
         this.dataGridView1.Rows[newRow].Cells["Info"].Value = item.Info;
         this.dataGridView1.Rows[newRow].Cells["Datei"].Value = item.Datei;
         this.dataGridView1.Rows[newRow].Cells["SaetzeMandant"].Value = item.SaetzeMandant;
         this.dataGridView1.Rows[newRow].Cells["Saetze"].Value = item.Saetze;

         // Zeilenfinder
         if (toggle) {
           this.dataGridView1.Rows[newRow].DefaultCellStyle.BackColor = Color.LightCyan;
         }

        zeile++;
       }
     }
     private void IndexDeleteAction(){
       DataGridViewCellStyle styleBold = new DataGridViewCellStyle();
       styleBold.Font = new Font(this.dataGridViewTables.Font.FontFamily, this.dataGridViewTables.Font.Size, FontStyle.Bold);

       foreach (var sysControlItem in sysControlListe){
         var row = this.dataGridViewTables.Rows[gridRowIndex];
         row.Cells[(int)DataGridColumns.Nr].Style.ApplyStyle(styleBold); // row.Cells[0].Style.ApplyStyle(styleBold);
         Application.DoEvents();

         row.Cells[(int)DataGridColumns.Index].Style.ApplyStyle(styleBold);     // Index=6
         row.Cells[(int)DataGridColumns.Aktion].Style.ApplyStyle(styleBold);    // Aktion=7
         Application.DoEvents();

         row.Cells[(int)DataGridColumns.Index].Value = indexList + " " + indexNumber + "/" + lastIndex;
         row.Cells[(int)DataGridColumns.Aktion].Value = indexFields;
         Application.DoEvents();

         row.Cells[(int)DataGridColumns.Aktion].Value = "OK";

         gridRowIndex = gridRowIndex + 1;
       }
     }
     private void dataGridView1_SelectionChanged(object sender, EventArgs e){
       string disp = _dgh.GetCellSumOrDateDiff(false, false); // dataGridView1.SelectedCells,
       this.toolStripStatusLabel1.Text = disp;
     }
     private void dataGridViewJournal_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
       dBuDatum = KaJourHelper.StringHelper.StringToDate(dataGridViewJournal.CurrentRow.Cells[_ColKey1].Value.ToString());
       sBuZeile = dataGridViewJournal.CurrentRow.Cells[_ColKey2].Value.ToString();
       if (this.StartEvent != null){
         this.StartEvent(dBuDatum, sBuZeile);
       }
     }
     enum DataGridColumns{
        Nr,
        Info,
        Datei,
        Saetze,
        SaetzeMand,
        DEL,
        Index,
        Aktion,
        Saetze2,
        Saetze2Mand,
        Last
     }
    */
}
