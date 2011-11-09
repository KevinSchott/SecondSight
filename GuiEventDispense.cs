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
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
	
namespace SecondSight
{
    public partial class MainForm
    {
        /// <summary>
        /// Add To List function for the Dispense tab.  Adds an SKU to either the dispense or delete list, depending on the parameter
        /// </summary>
        /// <param name="_isdispensing">True if the user wants to add to the dispense list, 
        /// false if the user wants to add the item to the delete list</param>
        private void D_AddToList(bool _isdispensing)
        {
            DataTable tempTable = new DataTable();
            DataTable ddTable;
            int skuToAdd = 0;
            TextBox ttb;
            ListBox tlb;

            //Set the temporary controls depending on whether we're working with dispense or delete
            if(_isdispensing) {
                ttb = tb_D_EnterSKU;
                tlb = lbox_D_ToDispense;
                ddTable = dispenseTable;
            } else {
                ttb = tb_D_EnterSKUDel;
                tlb = lbox_D_ToDelete;
                ddTable = deleteTable;
            }

            try {
    			skuToAdd = Convert.ToInt16(ttb.Text);
    		} catch (FormatException) {
    			return;
    		}

            if (skuToAdd < 0) {
    			return;
    		}
    		
    		//See if the SKU exists in inventory, if not then return without doing anything else
    		try {
    			Mydb.SKUSearch(skuToAdd, tempTable);
    		} catch (InvalidOperationException) {
    			MessageBox.Show("Please open a database before attempting to dispense or delete glasses.", 
    			                "No Database Open", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    			return;
    		}
    		
    		if(tempTable.Rows.Count == 0) {
    			MessageBox.Show(String.Format("The specified SKU, {0}, does not exist in inventory.", skuToAdd),
    			                "SKU Not Found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    			ttb.Focus();
    			ttb.SelectAll();
    			return;
    		}
    		
    		int i = tlb.Items.Count;
    		//input is a valid SKU, find the best place to insert it into the listbox
    		while (i > 0 && (int)(tlb.Items[i - 1]) >= skuToAdd) {
    			i--;
    		}
    		
    		if (i < tlb.Items.Count) {
	    		//input already exists, don't add anything else
	    		if ((int)(tlb.Items[i]) == skuToAdd) {
                    tlb.SelectedIndex = i;
	    			return;
	    		}
    		}

    		//Insert item into list in the correct position and include it in the data table
    		ddTable.Merge(tempTable);
    		tlb.Items.Insert(i, skuToAdd);
    		tlb.SelectedIndex = i;   
    		ttb.Clear();
    		ttb.Focus();
        }

        /// <summary>
        /// When a new index in either list box is selected, get the information about the item corresponding to the selected SKU
    	/// and display it in the appropriate area in the center.
        /// </summary>
        /// <param name="_isdispensing">True if the selected item is from the Dispense list box, false if it is from the Delete list box</param>
        private void D_UpdateInfo(bool _isdispensing)
        {
            int selectedSku = 0;
    		DataRow selectedDataRow = null;  //Data table index
            ListBox tlb;
            DataTable tempTable;

            if(_isdispensing) {
                tlb = lbox_D_ToDispense;
                tempTable = dispenseTable;
            } else {
                tlb = lbox_D_ToDelete;
                tempTable = deleteTable;
            }

    		//Convert the selected value to an integer
    		try {
    			selectedSku = (int)tlb.SelectedItem;
    		} catch (FormatException) {  //The selected value cannot convert to integer (should never get here)
    			return;
    		} catch (NullReferenceException) { //Nothing selected
    			return;
    		}

    		//Find the entry in the data table
    		foreach (DataRow dr in tempTable.Rows) {
    			if(selectedSku == Convert.ToInt16(dr[0])) {
    				selectedDataRow = dr;
    				break;
    			}
    		}
    		
    		//If this happens, there's a problem with the Add To List click function
    		if (selectedDataRow == null) {
    			throw new DataException("Selected SKU does not correspond to an entry in the data table.");
    		}
    		
    		//Populate the information area on the right
    		//RX info and comment
    		tb_D_SphereOD.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[1]));
    		tb_D_CylOD.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[2]));
    		tb_D_AxisOD.Text = selectedDataRow[3].ToString();
    		tb_D_AddOD.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[4]));
    		tb_D_SphereOS.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[5]));
    		tb_D_CylOS.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[6]));
    		tb_D_AxisOS.Text = selectedDataRow[7].ToString();
    		tb_D_AddOS.Text = String.Format("{0:0.00}", Convert.ToSingle(selectedDataRow[8]));
    		tb_D_Comment.Text = selectedDataRow[14].ToString();
    		
    		//Other info selection
    		String ts = selectedDataRow[9].ToString(); //Type
    		if (ts == "S") {
    			rb_D_Single.Checked = true;
    			rb_D_Multi.Checked = false;
    		} else {
    			rb_D_Single.Checked = false;
    			rb_D_Multi.Checked = true;
    		}
    		
    		ts = selectedDataRow[10].ToString();  //Gender
    		if (ts == "U") {
    			rb_D_Unisex.Checked = true;
    			rb_D_Male.Checked = false;
    			rb_D_Female.Checked = false;
    		} else if (ts == "M") {
    			rb_D_Unisex.Checked = false;
    			rb_D_Male.Checked = true;
    			rb_D_Female.Checked = false;
    		} else {
    			rb_D_Unisex.Checked = false;
    			rb_D_Male.Checked = false;
    			rb_D_Female.Checked = true;
    		}
    		
    		ts = selectedDataRow[11].ToString();  //Size
    		if (ts == "L") {
    			rb_D_Small.Checked = false;
    			rb_D_Medium.Checked = false;
    			rb_D_Large.Checked = true;
//    			rb_D_Child.Checked = false;
    		} else if (ts == "M") {
    			rb_D_Small.Checked = false;
    			rb_D_Medium.Checked = true;
    			rb_D_Large.Checked = false;
//    			rb_D_Child.Checked = false;
    		} else if (ts == "S") {
    			rb_D_Small.Checked = true;
    			rb_D_Medium.Checked = false;
    			rb_D_Large.Checked = false;
//                rb_D_Child.Checked = false;
    		} else {
    			rb_D_Small.Checked = false;
    			rb_D_Medium.Checked = false;
    			rb_D_Large.Checked = false;
//    			rb_D_Child.Checked = true;
    		}
    		
    		ts = selectedDataRow[12].ToString();  //Tint
    		if (ts == "N") {
    			rb_D_NoTint.Checked = true;
    			rb_D_LightTint.Checked = false;
    			rb_D_DarkTint.Checked = false;
    		} else if (ts == "L") {
    			rb_D_NoTint.Checked = false;
    			rb_D_LightTint.Checked = true;
    			rb_D_DarkTint.Checked = false;
    		} else {
    			rb_D_NoTint.Checked = false;
    			rb_D_LightTint.Checked = false;
    			rb_D_DarkTint.Checked = true;
    		}
        }

        /// <summary>
        /// Remove From List function for the Dispense tab.  Removes an SKU from either the dispense or delete list, depending on the parameter
        /// </summary>
        /// <param name="_isdispensing">True if the selected item is from the Dispense list box, false if it is from the Delete list box</param>
        private void D_RemoveFromList(bool _isdispensing)
        {
            ListBox tlb;
            DataTable ddTable;
            int selectedSku = 0;
    		int dtSelectedIndex = -1;
            int lbSelectedIndex;

            //Set the temporary controls depending on whether we're working with dispense or delete
            if(_isdispensing) {
                tlb = lbox_D_ToDispense;
                ddTable = dispenseTable;
            } else {
                tlb = lbox_D_ToDelete;
                ddTable = deleteTable;
            }

    		lbSelectedIndex = tlb.SelectedIndex;

    		//Do nothing if the listbox is empty
    		if (tlb.Items.Count == 0) {
    			return;
    		}
    		
    		//Convert the selected value to an integer
    		try {
    			selectedSku = (int)tlb.SelectedItem;
    		} catch (FormatException) {  //The selected value cannot convert to integer (should never get here)
    			return;
    		}
    		
    		//Find the index to the entry in data table
    		for (int i = 0; i < ddTable.Rows.Count; i++) {
    			if(selectedSku == Convert.ToInt16(ddTable.Rows[i][0])) {
    				dtSelectedIndex = i;
    				break;
    			}
    		}
    		
    		//Delete the appropriate entry from the table and the listbox
    		if (dtSelectedIndex >= 0) { //Negative means there's no corresponding entry in the data table
    			ddTable.Rows.RemoveAt(dtSelectedIndex);
    		}
    		tlb.Items.RemoveAt(lbSelectedIndex);
    		
    		//MessageBox.Show(String.Format("Selected Index:{0}\nNumber of Itmes:{1}", lbSelectedIndex, tlb.Items.Count));
    		//Update the selected item, clear the controls if the box is empty
    		if (tlb.Items.Count > 0) {  //Listbox not empty, update selected index
	    		if (lbSelectedIndex < tlb.Items.Count) { //Keep the selected index the same
	    			tlb.SelectedIndex = lbSelectedIndex;
    			} else { //Decrement the selected index
    				tlb.SelectedIndex = lbSelectedIndex - 1;
    			}
    		} else { //Listbox empty, reset all the controls in the Detailed Information groupbox
    			tb_D_SphereOD.Clear();
    			tb_D_CylOD.Clear();
    			tb_D_AxisOD.Clear();
    			tb_D_AddOD.Clear();
    			tb_D_SphereOS.Clear();
    			tb_D_CylOS.Clear();
    			tb_D_AxisOS.Clear();
    			tb_D_AddOS.Clear();
    			tb_D_Comment.Clear();
    			rb_D_NoTint.Checked = true;
    			rb_D_LightTint.Checked = false;
    			rb_D_DarkTint.Checked = false;
    			rb_D_Small.Checked = true;
    			rb_D_Medium.Checked = false;
    			rb_D_Large.Checked = false;
    			rb_D_Unisex.Checked = true;
    			rb_D_Male.Checked = false;
    			rb_D_Female.Checked = false;
    			rb_D_Single.Checked = true;
    			rb_D_Multi.Checked = false;
    		}
        }

    	/// <summary>
    	/// Click event for the Add To List button in the Dispense screen
    	/// Adds a new SKU to the list box based on the value in the Enter SKU text box.
    	/// If the box is blank or contains an invalid sku, no action is taken.
        /// This handler is used by btn_D_AddToList and btn_D_AddToListDel
    	/// </summary>
        private void btn_D_AddToList_Click(object sender, EventArgs e)
    	{
            D_AddToList(((Button)sender).Name == "btn_D_AddToList");
    	}
    	
        /// <summary>
        /// KeyPress event for the two EnterSKU text boxes.  Duplicates the AddToList button functionality as in btn_D_AddToList_Click
        /// </summary>
    	private void tb_D_EnterSKU_KeyPress(object sender, KeyPressEventArgs e)
    	{
    		if(e.KeyChar == 13) {
                if(((TextBox)sender).Name == "tb_D_EnterSKU") {
                    D_AddToList(true);
                } else {
                    D_AddToList(false);
                }
    		}
    	}

        /// <summary>
        /// SelectedIndexChanged event for the SKUs To Dispense and Delete listboxes
    	/// When a new index is selected, get the information about the item corresponding to the selected SKU
    	/// and display it in the appropriate informational controls in the center.  This handler is used
        /// by lbox_ToDispense and lboxToDelete
        /// </summary>
    	private void lbox_D_ToDispense_SelectedValueChanged(object sender, EventArgs e)
    	{
            D_UpdateInfo(((ListBox)sender).Name == "lbox_D_ToDispense");
    	}
    	
    	

        /// <summary>
        /// Click event for the Remove buttons in the Dispense tab
    	/// Removes the selected SKU from the SKUs To Dispense or Delete listbox and
    	/// the dispenseTable or deleteTable datatable.  This handler is used by btn_D_Remove and btn_D_RemoveDel
        /// </summary>
    	private void btn_D_Remove_Click(object sender, EventArgs e)
    	{
            D_RemoveFromList(((Button)sender).Name == "btn_D_Remove");
    	}
    	
        /// <summary>
    	/// Click event for the Dispense button in the Dispense tab.
    	/// Dispenses the items corresponding to each SKU in the listbox.
        /// </summary>
    	private void btn_D_Dispense_Click(object sender, EventArgs e)
    	{
    		SpecsRecord sr = new SpecsRecord();
    		int tdskus = 0;
    		
    		foreach (DataRow dr in dispenseTable.Rows) {
    			//Assemble the record
    			sr.SKU = Convert.ToUInt16(dr[0]);
    			sr.SphereOD = Convert.ToSingle(dr[1]);
    			sr.CylOD = Convert.ToSingle(dr[2]);
    			sr.AxisOD = Convert.ToInt16(dr[3]);
    			sr.AddOD = Convert.ToSingle(dr[4]);
    			sr.SphereOS = Convert.ToSingle(dr[5]);
    			sr.CylOS = Convert.ToSingle(dr[6]);
    			sr.AxisOS = Convert.ToInt16(dr[7]);
    			sr.AddOS = Convert.ToSingle(dr[8]);
    			sr.Type = dr[9].ToString();
    			sr.Gender = dr[10].ToString();
    			sr.Size = dr[11].ToString();
    			sr.Tint = dr[12].ToString();
    			sr.DateAdded = dr[13].ToString();
    			sr.DateDispensed = (DateTime.Today).ToString();
    			sr.Comment = dr[14].ToString();
    			
                tdskus++;
    			Mydb.Dispense(sr, true);  //Dispense the record
    		}
    		
    		if(tdskus > 0) {
	    		//Clean up the controls and the form.
	    		dispenseTable.Clear();
	    		lbox_D_ToDispense.Items.Clear();
	    		tb_D_SphereOD.Clear();
	    		tb_D_CylOD.Clear();
	    		tb_D_AxisOD.Clear();
	    		tb_D_AddOD.Clear();
	    		tb_D_SphereOS.Clear();
	    		tb_D_CylOS.Clear();
	    		tb_D_AxisOS.Clear();
	    		tb_D_AddOS.Clear();
	    		tb_D_Comment.Clear();
	    		rb_D_NoTint.Checked = true;
				rb_D_LightTint.Checked = false;
				rb_D_DarkTint.Checked = false;
				rb_D_Small.Checked = true;
				rb_D_Medium.Checked = false;
				rb_D_Large.Checked = false;
				rb_D_Unisex.Checked = true;
				rb_D_Male.Checked = false;
				rb_D_Female.Checked = false;
				rb_D_Single.Checked = true;
				rb_D_Multi.Checked = false;
				tb_D_EnterSKU.Clear();
				tb_D_EnterSKU.Focus();
	    		
				//Reload the inventory display
				Mydb.GetCurrentInventory();
				dt_V_DispensedTable.Clear();
				Mydb.GetTable(dt_V_DispensedTable, SSTable.Dispensed);
				
				if(tdskus > 1) {
					MessageBox.Show(String.Format("Successfully dispensed {0} pairs of glasses.", tdskus), 
					                "Dispense Successful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				} else {
					MessageBox.Show("Successfully dispensed 1 pair of glasses.",
					                "Dispense Successful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				
				IncrementOps(tdskus); //Increment ops by the number of records dispensed, if the appropriate prefs are set
    		} else {
    			MessageBox.Show("No glasses were dispensed; no SKUs were specified.", 
    			                "Dispense Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
    		}
    	}

        /// <summary>
        /// Click event for the Delete button in the Dispense tab.
        /// Deletes the items corresponding to each SKU in the listbox after confirmation.
        /// </summary>
        private void btn_D_Delete_Click(object sender, EventArgs e)
        {
            int tdskus = 0;

            if (deleteTable.Rows.Count > 0) {
                if(MessageBox.Show("You are about to permanently remove 1 or more records from inventory.\n\n-= DO NOT USE THIS BUTTON TO DISPENSE GLASSES =-\n\nAre you sure you want to proceed?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No) {
                        return;
                }
            }

            foreach (DataRow dr in deleteTable.Rows) {
                Mydb.Delete(Convert.ToUInt16(dr[0]), SSTable.Current);
                tdskus++;
            }

            if(tdskus > 0) {
	    		//Clean up the controls and the form.
	    		deleteTable.Clear();
	    		lbox_D_ToDelete.Items.Clear();
	    		tb_D_SphereOD.Clear();
	    		tb_D_CylOD.Clear();
	    		tb_D_AxisOD.Clear();
	    		tb_D_AddOD.Clear();
	    		tb_D_SphereOS.Clear();
	    		tb_D_CylOS.Clear();
	    		tb_D_AxisOS.Clear();
	    		tb_D_AddOS.Clear();
	    		tb_D_Comment.Clear();
	    		rb_D_NoTint.Checked = true;
				rb_D_LightTint.Checked = false;
				rb_D_DarkTint.Checked = false;
				rb_D_Small.Checked = true;
				rb_D_Medium.Checked = false;
				rb_D_Large.Checked = false;
				rb_D_Unisex.Checked = true;
				rb_D_Male.Checked = false;
				rb_D_Female.Checked = false;
				rb_D_Single.Checked = true;
				rb_D_Multi.Checked = false;
                tb_D_EnterSKUDel.Clear();
				tb_D_EnterSKUDel.Focus();
	    		
				//Reload the inventory display
				Mydb.GetCurrentInventory();
				
				if(tdskus > 1) {
					MessageBox.Show(String.Format("Successfully deleted {0} pairs of glasses.", tdskus), 
					                "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				} else {
					MessageBox.Show("Successfully deleted 1 pair of glasses.",
					                "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				
				IncrementOps(tdskus); //Increment ops by the number of records deleted, if the appropriate prefs are set
    		} else {
    			MessageBox.Show("No glasses were deleted; no SKUs were specified.", 
    			                "Delete Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
    		}
        }
    }
}