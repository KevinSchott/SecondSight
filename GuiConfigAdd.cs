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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SecondSight
{
    public partial class MainForm
    {
//    	private DataGridView dgv_Add_InventoryView;
        private BindingSource bs_Add_InventorySource;

		//Configures the data grid view
//        private void Configure_dgv_Add()
//        {
//            bs_Add_InventorySource = new BindingSource();
//            bs_Add_InventorySource.DataSource = Mydb.InvResults;
//            gb_Add_InventoryView.Size = new Size(Width - ADD_GB_W_OFFSET, Height - ADD_GB_H_OFFSET);
			
//            //Configures the column headers
////        	dgv_Add_InventoryView = new DataGridView();
        	
//            dgv_Add_InventoryView.Location = new Point(7, 16);
//            dgv_Add_InventoryView.Size = new Size(Width - ADD_DGV_W_OFFSET, Height - ADD_DGV_H_OFFSET);
//            dgv_Add_InventoryView.MultiSelect = false;
//            dgv_Add_InventoryView.ColumnCount = 15;
//            dgv_Add_InventoryView.ColumnHeadersVisible = true;
//            dgv_Add_InventoryView.RowHeadersVisible = false;
//            dgv_Add_InventoryView.AllowUserToResizeRows = false;
//            dgv_Add_InventoryView.AllowUserToDeleteRows = false;
//            dgv_Add_InventoryView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgv_Add_InventoryView.EditMode = DataGridViewEditMode.EditProgrammatically;
            
//            for (int i = 0; i < 15; i++)
//            {
//                dgv_Add_InventoryView.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
//                dgv_Add_InventoryView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
//            }
//            dgv_Add_InventoryView.Columns[0].Name = "SKU";
//            dgv_Add_InventoryView.Columns[0].DataPropertyName = "SKU";
//            dgv_Add_InventoryView.Columns[0].MinimumWidth = 60;
//            dgv_Add_InventoryView.Columns[1].Name = "OD Sphere";
//            dgv_Add_InventoryView.Columns[1].DataPropertyName = "SphereOD";
//            dgv_Add_InventoryView.Columns[1].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[1].DefaultCellStyle.BackColor = GuiPrefs.ODColumnColor;
//            dgv_Add_InventoryView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[1].MinimumWidth = 90;
//            dgv_Add_InventoryView.Columns[2].Name = "OD Cylinder";
//            dgv_Add_InventoryView.Columns[2].DataPropertyName = "CylinderOD";
//            dgv_Add_InventoryView.Columns[2].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[2].DefaultCellStyle.BackColor = GuiPrefs.ODColumnColor;
//            dgv_Add_InventoryView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[2].MinimumWidth = 100;
//            dgv_Add_InventoryView.Columns[3].Name = "OD Axis";
//            dgv_Add_InventoryView.Columns[3].DataPropertyName = "AxisOD";
//            dgv_Add_InventoryView.Columns[3].DefaultCellStyle.BackColor = GuiPrefs.ODColumnColor;
//            dgv_Add_InventoryView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[3].MinimumWidth = 70;
//            dgv_Add_InventoryView.Columns[4].Name = "OD Add";
//            dgv_Add_InventoryView.Columns[4].DataPropertyName = "AddOD";
//            dgv_Add_InventoryView.Columns[4].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[4].DefaultCellStyle.BackColor = GuiPrefs.ODColumnColor;
//            dgv_Add_InventoryView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[4].MinimumWidth = 70;
//            dgv_Add_InventoryView.Columns[5].Name = "OS Sphere";
//            dgv_Add_InventoryView.Columns[5].DataPropertyName = "SphereOS";
//            dgv_Add_InventoryView.Columns[5].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[5].DefaultCellStyle.BackColor = GuiPrefs.OSColumnColor;
//            dgv_Add_InventoryView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[5].MinimumWidth = 90;
//            dgv_Add_InventoryView.Columns[6].Name = "OS Cylinder";
//            dgv_Add_InventoryView.Columns[6].DataPropertyName = "CylinderOS";
//            dgv_Add_InventoryView.Columns[6].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[6].DefaultCellStyle.BackColor = GuiPrefs.OSColumnColor;
//            dgv_Add_InventoryView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[6].MinimumWidth = 100;
//            dgv_Add_InventoryView.Columns[7].Name = "OS Axis";
//            dgv_Add_InventoryView.Columns[7].DataPropertyName = "AxisOS";
//            dgv_Add_InventoryView.Columns[7].DefaultCellStyle.BackColor = GuiPrefs.OSColumnColor;
//            dgv_Add_InventoryView.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[7].MinimumWidth = 70;
//            dgv_Add_InventoryView.Columns[8].Name = "OS Add";
//            dgv_Add_InventoryView.Columns[8].DataPropertyName = "AddOS";
//            dgv_Add_InventoryView.Columns[8].DefaultCellStyle.Format = "0.00";
//            dgv_Add_InventoryView.Columns[8].DefaultCellStyle.BackColor = GuiPrefs.OSColumnColor;
//            dgv_Add_InventoryView.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgv_Add_InventoryView.Columns[8].MinimumWidth = 70;
//            dgv_Add_InventoryView.Columns[9].Name = "Type";
//            dgv_Add_InventoryView.Columns[9].DataPropertyName = "Type";
//            dgv_Add_InventoryView.Columns[10].Name = "Gender";
//            dgv_Add_InventoryView.Columns[10].DataPropertyName = "Gender";
//            dgv_Add_InventoryView.Columns[11].Name = "Size";
//            dgv_Add_InventoryView.Columns[11].DataPropertyName = "Size";
//            dgv_Add_InventoryView.Columns[12].Name = "Tint";
//            dgv_Add_InventoryView.Columns[12].DataPropertyName = "Tint";
//            dgv_Add_InventoryView.Columns[13].Name = "Date Added";
//            dgv_Add_InventoryView.Columns[13].DataPropertyName = "DateAdded";
//            dgv_Add_InventoryView.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
//            dgv_Add_InventoryView.Columns[13].MinimumWidth = 100;
//            dgv_Add_InventoryView.Columns[14].Name = "Comment";
//            dgv_Add_InventoryView.Columns[14].DataPropertyName = "Comment";
//            dgv_Add_InventoryView.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
//            dgv_Add_InventoryView.Columns[14].Width = 100;
//            dgv_Add_InventoryView.AutoResizeColumnHeadersHeight();
//            dgv_Add_InventoryView.AutoResizeColumns();
//            dgv_Add_InventoryView.AutoResizeRows();
//            dgv_Add_InventoryView.AutoGenerateColumns = false;
//            dgv_Add_InventoryView.TabStop = false;
        	
//            dgv_Add_InventoryView.DataSource = bs_Add_InventorySource;
//            gb_Add_InventoryView.Controls.Add(dgv_Add_InventoryView);
//        }
    }
}