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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
	
namespace SecondSight
{
    public partial class MainForm
    {
        private enum SearchFieldType {INT, FLOAT, DATE};
        /// <summary>
        /// Click event for the Search By Field button
        /// </summary>
        /// <remarks>
        /// Finds the nearest entry in the data grid view that corresponds to the search parameters
    	/// and sorts the DGV by the column by which the user searches.
        /// </remarks>
    	private void btn_V_SearchByFieldField_Click(object sender, EventArgs e)
        {
            SearchFieldType sft;
            int position = 0;       //Position of the nearest match
            string colval = cb_V_SearchByField.SelectedValue.ToString();

            //Determine the type of search field we're checking.  SKU and axis are integers, others are floats, except for dates
            if (cb_V_SearchByField.SelectedIndex == 0 || cb_V_SearchByField.SelectedIndex == 3 || cb_V_SearchByField.SelectedIndex == 7) { //SKU or OD/OS Axis selected
                sft = SearchFieldType.INT;  
            } else if (cb_V_SearchByField.SelectedIndex < 9) { //Any other non-date selection
                sft = SearchFieldType.FLOAT;
            } else { //Dates
                sft = SearchFieldType.DATE;
            }

            //Search routine, separate branch for INT, FLOAT and DATE
            if (sft == SearchFieldType.INT) { //Search for INT types
                int searchval = 0;
                try {
                    searchval = Convert.ToInt16(tb_V_SearchByField.Text);
                } catch {
                    MessageBox.Show("Search value must be a positive whole number.", "Invalid Search Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_V_SearchByField.SelectAll();
                    tb_V_SearchByField.Focus();
                    return;
                }
                
                //Sort the list in ascending order by column to be searched
                dgv_V_InventoryView.Sort(dgv_V_InventoryView.Columns[colval], ListSortDirection.Ascending);

                //Loop through each row, break if we find a value greater or equal to the target value.
                foreach (DataRowView drv in bs_V_InventorySource) {
                    if (Convert.ToInt16(drv[colval]) >= searchval) {
                        break;
                    }
                    position++;
                }
            } else if (sft == SearchFieldType.FLOAT) { //Search for FLOAT types
                float searchval = 0;
                try {
                    searchval = Convert.ToSingle(tb_V_SearchByField.Text);
                } catch {
                    MessageBox.Show("Search value must be a real number.", "Invalid Search Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_V_SearchByField.SelectAll();
                    tb_V_SearchByField.Focus();
                    return;
                }
                
                if (searchval < 0) {
                    //Sort the list in descending order by column to be searched
                    dgv_V_InventoryView.Sort(dgv_V_InventoryView.Columns[colval], ListSortDirection.Descending);

                    //Loop through each row, break if we find a value greater or equal to the target value.
                    foreach (DataRowView drv in bs_V_InventorySource) {
                        if (Convert.ToSingle(drv[colval]) <= searchval) {
                            break;
                        }
                        position++;
                    }
                } else {
                    //Sort the list in ascending order by column to be searched
                    dgv_V_InventoryView.Sort(dgv_V_InventoryView.Columns[colval], ListSortDirection.Ascending);

                    //Loop through each row, break if we find a value greater or equal to the target value.
                    foreach (DataRowView drv in bs_V_InventorySource) {
                        if (Convert.ToSingle(drv[colval]) >= searchval) {
                            break;
                        }
                        position++;
                    }
                }
            } else { //Search for DATE types
                DateTime searchval = dtp_V_SearchByDate.Value.Date;

                dgv_V_InventoryView.Sort(dgv_V_InventoryView.Columns[colval], ListSortDirection.Ascending);

                //Loop through each row, break if we find a value greater or equal to the target value.
                foreach (DataRowView drv in bs_V_InventorySource) {
                    if (Convert.ToDateTime(drv[colval]).Date >= searchval) {
                        break;
                    }
                    position++;
                }
            }

            //Jump to the area in the datagridview that contains the search results
            dgv_V_InventoryView.ClearSelection();
            if(position > 1) {
                dgv_V_InventoryView.FirstDisplayedScrollingRowIndex = position - 2;
            } else {
                dgv_V_InventoryView.FirstDisplayedScrollingRowIndex = position;

            }
			
			tb_V_SearchByField.Focus();
			
    	}
    	
    	/// <summary>
    	/// SelectedIndexChanged event for cb_V_SearchIn
    	/// </summary>
        /// <remarks>
        /// Changes inventory table displayed by the datagridview in the View Inventory tab, either Current Inventory or Dispensed Inventory.
        /// Adds or removes the "Dispensed Date" option in the cb_V_SearchByField combobox as appropriate.
        /// </remarks>
    	private void cb_V_SearchIn_SelectedIndexChanged(object sender, EventArgs e)
    	{
    		try {
	    		if(cb_V_SearchIn.SelectedIndex == 0) { //Current Inventory selected
	    			dgv_V_InventoryView.Columns["DateDispensed"].Visible = false;
	    			bs_V_InventorySource.DataSource = Mydb.InvResults;
                    bs_V_SearchByField.RemoveAt(10);
	    		} else { //Dispensed Inventory selected
	    			dgv_V_InventoryView.Columns["DateDispensed"].Visible = true;
	    			bs_V_InventorySource.DataSource = dt_V_DispensedTable;
                    bs_V_SearchByField.Add(new KeyValuePair<string, string>("Date Dispensed", "DateDispensed"));
	    		}
    		} catch {}
    	}

        /// <summary>
        /// SelectedIndexChanged event for the cb_V_SearchByField
        /// </summary>
        /// <remarks>
        /// Alters the visibility and position of controls based on whether a date or non-date field is selected.
        /// If a date field is selected, the DateTimePicker is shown, otherwise the TextBox is shown.
        /// </remarks>
        private void cb_V_SearchByField_SelectedIndexChangd(object sender, EventArgs e)
        {
            if (cb_V_SearchByField.SelectedIndex > 8) {
                panel_V_SearchBy.Width = 274;
                dtp_V_SearchByDate.Visible = true;
                tb_V_SearchByField.Visible = false;
            } else {
                panel_V_SearchBy.Width = 175;
                dtp_V_SearchByDate.Visible = false;
                tb_V_SearchByField.Visible = true;
            }
        }
    }
}