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
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

namespace SecondSight.Extended_Controls
{
    /// <summary>
    /// DataGridViewSS class
    /// Extension to the DataGridView class for SecondSight
    /// Adds methods to configure the DGV with the correct columns and adds additional cell style options
    /// and capabilities, such as colored cell borders for OD and OS columns
    /// </summary>
    public class DataGridViewSS : DataGridView
    {
        private static int TotalColumns = 17;

        public DataGridViewSS()
        {
            DoubleBuffered = true;
            AutoGenerateColumns = false;
//            ConfigureDGV();
        }

        /// <summary>
        /// ConfigureDGV - Configures the DGV, adds columns, sets styles, etc
        /// All columns are added, including the ones that may be unused for a certain view (Score, DateDispensed)
        /// </summary>
        public void ConfigureDGV()
        {
            //Configure the column-level parameters
            for (int i = 0; i < 5; i++) {
                DataGridViewColumnSS col = new DataGridViewColumnSS();
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Columns.Add(col);
            }

            DataGridViewColumnSSRB rbcol = new DataGridViewColumnSSRB();
            rbcol.SortMode = DataGridViewColumnSortMode.NotSortable;
            //rbcol.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            rbcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns.Add(rbcol);

            DataGridViewColumnSSLB lbcol = new DataGridViewColumnSSLB();
            lbcol.SortMode = DataGridViewColumnSortMode.NotSortable;
            //lbcol.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            lbcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns.Add(lbcol);

            for (int i = 7; i < TotalColumns; i++) {
                DataGridViewColumnSS col = new DataGridViewColumnSS();
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Columns.Add(col);
            }

            //Customize each individual column
            Columns[0].Name = "SKU";
        	Columns[0].DataPropertyName = "SKU";
        	Columns[0].MinimumWidth = 55;
        	Columns[1].Name = "Score";
        	Columns[1].DataPropertyName = "Score";
        	Columns[1].MinimumWidth = 60;
        	Columns[1].DefaultCellStyle.Format = "0.#####";
        	Columns[2].Name = "SphereOD";
            Columns[2].HeaderText = "OD Sphere";
        	Columns[2].DataPropertyName = "SphereOD";
        	Columns[2].DefaultCellStyle.Format = "0.00";
        	Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns[3].Name = "CylinderOD";
        	Columns[3].HeaderText = "OD Cylinder";
        	Columns[3].DataPropertyName = "CylinderOD";
        	Columns[3].DefaultCellStyle.Format = "0.00";
        	Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[4].Name = "AxisOD";
            Columns[4].HeaderText = "OD Axis";
        	Columns[4].DataPropertyName = "AxisOD";
        	Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[5].Name = "AddOD";
            Columns[5].HeaderText = "OD Add";
        	Columns[5].DataPropertyName = "AddOD";
        	Columns[5].DefaultCellStyle.Format = "0.00";
        	Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[6].Name = "SphereOS";
            Columns[6].HeaderText = "OS Sphere";
        	Columns[6].DataPropertyName = "SphereOS";
        	Columns[6].DefaultCellStyle.Format = "0.00";
        	Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[7].Name = "CylinderOS";
            Columns[7].HeaderText = "OS Cylinder";
            Columns[7].DataPropertyName = "CylinderOS";
        	Columns[7].DefaultCellStyle.Format = "0.00";
        	Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[8].Name = "AxisOS";
            Columns[8].HeaderText = "OS Axis";
        	Columns[8].DataPropertyName = "AxisOS";
        	Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[9].Name = "AddOS";
            Columns[9].HeaderText = "OS Add";
        	Columns[9].DataPropertyName = "AddOS";
        	Columns[9].DefaultCellStyle.Format = "0.00";
        	Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        	Columns[10].Name = "Type";
        	Columns[10].DataPropertyName = "Type";
        	Columns[11].Name = "Gender";
        	Columns[11].DataPropertyName = "Gender";
        	Columns[12].Name = "Size";
        	Columns[12].DataPropertyName = "Size";
        	Columns[13].Name = "Tint";
        	Columns[13].DataPropertyName = "Tint";
            Columns[14].Name = "DateAdded";
        	Columns[14].HeaderText = "Date Added";
        	Columns[14].DataPropertyName = "DateAdded";
        	Columns[14].Width = 75;
        	Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            Columns[15].Name = "DateDispensed";
            Columns[15].HeaderText = "Date Dispensed";
        	Columns[15].DataPropertyName = "DateDispensed";
        	Columns[15].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
        	Columns[15].MinimumWidth = 100;
        	Columns[16].Name = "Comment";
        	Columns[16].DataPropertyName = "Comment";
        	Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        	Columns[16].Width = 100;
        }
    }

    /// <summary>
    /// Column class with normal borders (uses default DataGridViewTextBoxCell)
    /// </summary>
    class DataGridViewColumnSS : DataGridViewColumn
    {
        public DataGridViewColumnSS()
        {
            CellTemplate = new DataGridViewTextBoxCell();
        }
    }

    /// <summary>
    /// Column class that gives the cells a thicker border on the right
    /// </summary>
    class DataGridViewColumnSSRB : DataGridViewColumn
    {
        public DataGridViewColumnSSRB()
        {
            CellTemplate = new DataGridViewCellSSRB();
        }
    }

    /// <summary>
    /// Column class that gives the cells a thicker border on the left
    /// </summary>
    class DataGridViewColumnSSLB : DataGridViewColumn
    {
        public DataGridViewColumnSSLB()
        {
            CellTemplate = new DataGridViewCellSSLB();
        }
    }

    class DataGridViewCellSSRB : DataGridViewTextBoxCell
        {
        public override DataGridViewAdvancedBorderStyle AdjustCellBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, 
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool singleVerticalBorderAdded, 
            bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {
            dataGridViewAdvancedBorderStylePlaceholder.Right = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
            dataGridViewAdvancedBorderStylePlaceholder.Left = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceholder.Top = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceholder.Bottom = dataGridViewAdvancedBorderStyleInput.Bottom;
            return dataGridViewAdvancedBorderStylePlaceholder;
        }
    }

    class DataGridViewCellSSLB : DataGridViewTextBoxCell
        {
        public override DataGridViewAdvancedBorderStyle AdjustCellBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, 
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool singleVerticalBorderAdded, 
            bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {
            dataGridViewAdvancedBorderStylePlaceholder.Right = DataGridViewAdvancedCellBorderStyle.Single;
            dataGridViewAdvancedBorderStylePlaceholder.Left = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
            dataGridViewAdvancedBorderStylePlaceholder.Top = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceholder.Bottom = dataGridViewAdvancedBorderStyleInput.Bottom;
            return dataGridViewAdvancedBorderStylePlaceholder;
        }

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        }
    }

} //namespace SecondSight.DGV
