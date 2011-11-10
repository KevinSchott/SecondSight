// Copyright 2009, 2010, 2011 Kevin Schott

// This file is part of SecondSight.

// SecondSight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// SecondSight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with SecondSight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelLibrary.Office.Excel;
using System.Data;

namespace SecondSight.Excel
{
    /// <summary>
    /// Excel helper functions.  Used to export the contents of the database 
    /// and reports to excel-compatible spreadsheet files.
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Exports the entire contents of a SecondSight database to an excel-compatible spreadsheet file.
        /// This function overwrites the file chosen if it already exists.
        /// </summary>
        /// <param name="_db">The SecondSight database to export</param>
        /// <param name="_path">The full path to the file being created (or overwritten)</param>
        public static void ExportFullDatabase(SSDataBase _db, string _path)
        {
            DataTable currentinv = new DataTable();
            DataTable dispensedinv = new DataTable();
            DataTable dbinfo = new DataTable();

            Workbook wb = new Workbook();
            Worksheet ciws = new Worksheet("Current Inventory");
            Worksheet diws = new Worksheet("Dispensed Inventory");
            Worksheet infows = new Worksheet("Database Information");

            _db.GetTable(currentinv, SSTable.Current);
            _db.GetTable(dispensedinv, SSTable.Dispensed);
            _db.GetTable(dbinfo, SSTable.DBInfo);

            //Populate the Database Information page
            infows.Cells[1, 1] = new Cell("Name");
            infows.Cells[1, 2] = new Cell("Location");
            infows.Cells[1, 3] = new Cell("Date Created");
            infows.Cells[2, 1] = new Cell(dbinfo.Rows[0][0].ToString());
            infows.Cells[2, 2] = new Cell(dbinfo.Rows[0][1].ToString());
            infows.Cells[2, 3] = new Cell(String.Format("{0:MM/dd/yyyy}", dbinfo.Rows[0][2]));

            //Populate the Current Inventory page (headers)
            ciws.Cells[1, 1] = new Cell("SKU");
            ciws.Cells[1, 2] = new Cell("OD Sphere");
            ciws.Cells[1, 3] = new Cell("OD Cylinder");
            ciws.Cells[1, 4] = new Cell("OD Axis");
            ciws.Cells[1, 5] = new Cell("OD Add");
            ciws.Cells[1, 6] = new Cell("OS Sphere");
            ciws.Cells[1, 7] = new Cell("OS Cylinder");
            ciws.Cells[1, 8] = new Cell("OS Axis");
            ciws.Cells[1, 9] = new Cell("OS Add");
            ciws.Cells[1, 10] = new Cell("Type");
            ciws.Cells[1, 11] = new Cell("Gender");
            ciws.Cells[1, 12] = new Cell("Size");
            ciws.Cells[1, 13] = new Cell("Tint");
            ciws.Cells[1, 14] = new Cell("Date Added");
            ciws.Cells[1, 15] = new Cell("Comment");
            int tindex = 0;

            //Populate the Current Inventory page (data)
            for (int i = 0; i < currentinv.Rows.Count; i++) {
                tindex = i;
                ciws.Cells[i + 2, 1] = new Cell(currentinv.Rows[i][0].ToString());
                ciws.Cells[i + 2, 2] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][1]))); //Sphere
                ciws.Cells[i + 2, 3] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][2]))); //Cylinder
                ciws.Cells[i + 2, 4] = new Cell(currentinv.Rows[i][3].ToString()); //Axis
                ciws.Cells[i + 2, 5] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][4]))); //Add
                ciws.Cells[i + 2, 6] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][5]))); //Sphere
                ciws.Cells[i + 2, 7] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][6]))); //Cylinder
                ciws.Cells[i + 2, 8] = new Cell(currentinv.Rows[i][7].ToString()); //Axis
                ciws.Cells[i + 2, 9] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(currentinv.Rows[i][8]))); //Add
                ciws.Cells[i + 2, 10] = new Cell(currentinv.Rows[i][9].ToString());
                ciws.Cells[i + 2, 11] = new Cell(currentinv.Rows[i][10].ToString());
                ciws.Cells[i + 2, 12] = new Cell(currentinv.Rows[i][11].ToString());
                ciws.Cells[i + 2, 13] = new Cell(currentinv.Rows[i][12].ToString());
                ciws.Cells[i + 2, 14] = new Cell(String.Format("{0:MM/dd/yyyy}", currentinv.Rows[i][13])); //Date Added
                ciws.Cells[i + 2, 15] = new Cell(currentinv.Rows[i][14].ToString());
            }

            //Populate the Dispensed Inventory page (headers)
            diws.Cells[1, 1] = new Cell("SKU");
            diws.Cells[1, 2] = new Cell("OD Sphere");
            diws.Cells[1, 3] = new Cell("OD Cylinder");
            diws.Cells[1, 4] = new Cell("OD Axis");
            diws.Cells[1, 5] = new Cell("OD Add");
            diws.Cells[1, 6] = new Cell("OS Sphere");
            diws.Cells[1, 7] = new Cell("OS Cylinder");
            diws.Cells[1, 8] = new Cell("OS Axis");
            diws.Cells[1, 9] = new Cell("OS Add");
            diws.Cells[1, 10] = new Cell("Type");
            diws.Cells[1, 11] = new Cell("Gender");
            diws.Cells[1, 12] = new Cell("Size");
            diws.Cells[1, 13] = new Cell("Tint");
            diws.Cells[1, 14] = new Cell("Date Added");
            diws.Cells[1, 15] = new Cell("Date Dispensed");
            diws.Cells[1, 16] = new Cell("Comment");

            //Populate the Dispensed Inventory page (data)
            for (int i = 0; i < dispensedinv.Rows.Count; i++) {
                diws.Cells[i + 2, 1] = new Cell(dispensedinv.Rows[i][0].ToString());
                diws.Cells[i + 2, 2] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][1]))); //Sphere
                diws.Cells[i + 2, 3] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][2]))); //Cylinder
                diws.Cells[i + 2, 4] = new Cell(dispensedinv.Rows[i][3].ToString()); //Axis
                diws.Cells[i + 2, 5] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][4]))); //Add
                diws.Cells[i + 2, 6] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][5]))); //Sphere
                diws.Cells[i + 2, 7] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][6]))); //Cylinder
                diws.Cells[i + 2, 8] = new Cell(dispensedinv.Rows[i][7].ToString()); //Axis
                diws.Cells[i + 2, 9] = new Cell(String.Format("{0:0.00}", Convert.ToSingle(dispensedinv.Rows[i][8]))); //Add
                diws.Cells[i + 2, 10] = new Cell(dispensedinv.Rows[i][9].ToString());
                diws.Cells[i + 2, 11] = new Cell(dispensedinv.Rows[i][10].ToString());
                diws.Cells[i + 2, 12] = new Cell(dispensedinv.Rows[i][11].ToString());
                diws.Cells[i + 2, 13] = new Cell(dispensedinv.Rows[i][12].ToString());
                diws.Cells[i + 2, 14] = new Cell(String.Format("{0:MM/dd/yyyy}", dispensedinv.Rows[i][13])); //Date Added
                diws.Cells[i + 2, 15] = new Cell(String.Format("{0:MM/dd/yyyy}", dispensedinv.Rows[i][14])); //Date Dispensed
                diws.Cells[i + 2, 16] = new Cell(dispensedinv.Rows[i][15].ToString());
            }

            //Add worksheets to the workbook
            wb.Worksheets.Add(ciws);
            wb.Worksheets.Add(diws);
            wb.Worksheets.Add(infows);

            //Save the workbook
            wb.Save(_path);
        }

        /// <summary>
        /// Exports the contents of the last report to an excel-compatible spreadsheet file (.xls)
        /// </summary>
        /// <param name="_dt">The data table that contains the report data</param>
        /// <param name="_groupby">The group by field (empty for full list)</param>
        /// <param name="_description">The report query, in plain english (used to title the spreadsheet)</param>
        /// <param name="_path">The path of the spreadsheet file to export to (can exist or not)</param>
        public static void ExportReport(DataTable _dt, string _groupby, string _description, string _path)
        {
            Workbook wb;
            if (File.Exists(_path)) {
                wb = Workbook.Open(_path); //Add to the existing spreadsheet
            } else {
                wb = new Workbook();
            }
            StringBuilder wsname = new StringBuilder("Report ").Append(wb.Worksheets.Count);
            Worksheet ws = new Worksheet(wsname.ToString());

            ws.Cells[1, 1] = new Cell(_description);

            if (_groupby.Length > 0) { //Summary report
                ws.Cells[3, 1] = new Cell(_groupby);
                ws.Cells[3, 2] = new Cell("Count");

                //for (int i = 0; i < _dt.Rows.Count; i++) {
                //    ws.Cells[i + 4, 1] = new Cell(_dt.Rows[i][0]);
                //    ws.Cells[i + 4, 2] = new Cell(_dt.Rows[i][1]);
                //}

            } else { //Full list report
                //Populate the worksheet (headers)
                ws.Cells[3, 1] = new Cell("SKU");
                ws.Cells[3, 2] = new Cell("OD Sphere");
                ws.Cells[3, 3] = new Cell("OD Cylinder");
                ws.Cells[3, 4] = new Cell("OD Axis");
                ws.Cells[3, 5] = new Cell("OD Add");
                ws.Cells[3, 6] = new Cell("OS Sphere");
                ws.Cells[3, 7] = new Cell("OS Cylinder");
                ws.Cells[3, 8] = new Cell("OS Axis");
                ws.Cells[3, 9] = new Cell("OS Add");
                ws.Cells[3, 10] = new Cell("Type");
                ws.Cells[3, 11] = new Cell("Gender");
                ws.Cells[3, 12] = new Cell("Size");
                ws.Cells[3, 13] = new Cell("Tint");
                ws.Cells[3, 14] = new Cell("Date Added");

                if (_dt.Columns.Count == 16) {
                    ws.Cells[3, 15] = new Cell("Date Dispensed");
                    ws.Cells[3, 16] = new Cell("Comment");
                } else {
                    ws.Cells[3, 15] = new Cell("Comment");
                }
            }

            //Loop through the data table and write each value to the worksheet
            for (int i = 0; i < _dt.Rows.Count; i++) {
                for (int j = 0; j < _dt.Columns.Count; j++) {
                    ws.Cells[i + 4, j + 1] = new Cell(_dt.Rows[i][j].ToString());
                }
            }

            wb.Worksheets.Add(ws);
            wb.Save(_path);
        }
    }
}
