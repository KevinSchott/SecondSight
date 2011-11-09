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
	
namespace SecondSight
{
    public partial class MainForm
    {
        //Leave event for tb_Add_SphereOD
        //Checks for a valid value, reports invalid values to the user
        private void tb_Add_SphereOD_Leave(object sender, EventArgs e)
        {
        	Rx_TextBox_Valid(tb_Add_SphereOD, RxBox.Sphere);
        	Add_SphereLock_Process(true);
        }

        //Leave event for tb_Add_SphereOS
        private void tb_Add_SphereOS_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_SphereOS, RxBox.Sphere);
            Add_SphereLock_Process(false);
        }

        //Leave event for tb_Add_CylinderOD
        private void tb_Add_CylinderOD_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_CylOD, RxBox.Cyl);
        }

        //Leave event for tb_Add_CylinderOS
        private void tb_Add_CylinderOS_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_CylOS, RxBox.Cyl);
        }

        //Leave event for tb_Add_AxisOD
        private void tb_Add_AxisOD_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_AxisOD, RxBox.Axis);
        }

        //Leave event for tb_Add_AxisOS
        private void tb_Add_AxisOS_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_AxisOS, RxBox.Axis);
        }

        //Leave event for tb_Add_AddOD
        private void tb_Add_AddOD_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_AddOD, RxBox.Add);

		    try
		    {
		    	if (Convert.ToSingle(tb_Add_AddOD.Text) != 0) { //If AddOD is a nonzero number
		    		rb_Add_Multi.Checked = true;
                } else if (Convert.ToSingle(tb_Add_AddOS.Text) == 0) { //else if AddOS (not a typo) is also zero
		    		rb_Add_Single.Checked = true;
                }
		    }
		    catch{}
		    
            //If the text in AddOS is invalid, remove it
            try {
                Convert.ToSingle(tb_Add_AddOS.Text);
            } catch {
                tb_Add_AddOS.Clear();
            }

		    //Duplicate the value of this textbox on the OS side if nothing had been entered previously
		    if (tb_Add_AddOS.Text.Length == 0) {
                tb_Add_AddOS.Text = tb_Add_AddOD.Text;
            }
        }

        //Leave event for tb_Add_AddOS
        private void tb_Add_AddOS_Leave(object sender, EventArgs e)
        {
            Rx_TextBox_Valid(tb_Add_AddOS, RxBox.Add);
            
		    try
		    {
		    	if (Convert.ToSingle(tb_Add_AddOS.Text) != 0) { //If AddOS is a nonzero number
		    		rb_Add_Multi.Checked = true;
                } else if (Convert.ToSingle(tb_Add_AddOD.Text) == 0) { //else if AddOD (not a typo) is also zero
                    rb_Add_Single.Checked = true;
                }
		    }
		    catch{}
		    
            //If the text in AddOD is invalid, remove it
            try {
                Convert.ToSingle(tb_Add_AddOD.Text);
            } catch {
                tb_Add_AddOD.Clear();
            }

            //Duplicate the value of this textbox on the OD side if nothing valid had been entered previously
            if (tb_Add_AddOD.Text.Length == 0) {
                tb_Add_AddOD.Text = tb_Add_AddOS.Text;
            }
        }

        //CheckedChanged event for chb_Add_SphereLockOD
        private void chb_Add_SphereLockOD_CheckedChanged(object sender, EventArgs e)
        {
        	Add_SphereLock_Process(true);
        }
        
        //CheckedChanged event for chb_Add_SphereLockOS
        private void chb_Add_SphereLockOS_CheckedChanged(object sender, EventArgs e)
		{
			Add_SphereLock_Process(false);
		}
        
		//Processes a sphere text box based on the Sphere Lock check boxes
        private void Add_SphereLock_Process(bool _isOD)
        {
        	try {
	        	if(_isOD == true) {
	        		if(chb_Add_SphereLockOD.Checked == true) { //Change OD sphere to negative
        				tb_Add_SphereOD.Text = (-Math.Abs(Convert.ToSingle(tb_Add_SphereOD.Text))).ToString("0.00");
		        	} //Do not change a negative sphere to positive if the box is unchecked
	        	} else {
        			if(chb_Add_SphereLockOS.Checked == true) { //Change OS sphere to negative
        				tb_Add_SphereOS.Text = (-Math.Abs(Convert.ToSingle(tb_Add_SphereOS.Text))).ToString("0.00");
        			} //Do not change a negative sphere to positive if the box is unchecked
	        	}
        	} catch {} //If there's a problem with the textbox's text, do nothing
        }
        
        //Add New Item button clicked.  Checks all the fields for valid values, 
        //and if no errors, creates the record and adds it to the db.
        private void btn_Add_AddItem_Click(object sender, EventArgs e)
        {
        	if(GuiPrefs.OpenDBPath == "")
        	{
        		MessageBox.Show("You must load a database before you can add items.", 
        		                "No Database Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
        		return;
        	}
            //Add the item if all the fields have valid values
            if (Add_Controls_Valid())
            {
                SpecsRecord nr = new SpecsRecord();
                int skunew = 1;
                int skumin, skumax;
                
                //Step 1 - Find the first unused SKU in the database
                try {
                	tb_Add_MinSKU.Text = (skumin = Math.Abs(Convert.ToInt16(tb_Add_MinSKU.Text))).ToString();
                }
				catch { //If not a valid integer, set min to 1
                	skumin = 1;
                	tb_Add_MinSKU.Text = "";
                }
                
                try {
                	tb_Add_MaxSKU.Text = (skumax = Math.Abs(Convert.ToInt16(tb_Add_MaxSKU.Text))).ToString();
                }
				catch { //If not a valid integer, set max to -1 (function will read as no limit)
                	skumax = -1;
                }
                
                if(skumax < skumin) { //Blanks max if it's less than min
                	tb_Add_MaxSKU.Text = "";
                }
                
            	try {
            		skunew = Mydb.GetNextFreeSKU(skumin, skumax);
            	} catch(IndexOutOfRangeException) {
            		MessageBox.Show("There is no available SKU number in the specified range.", 
            		                "No available SKU", MessageBoxButtons.OK, MessageBoxIcon.Error);
            		return;
            	}
//                }
//                else { //No valid range specified, search entire inventory
//                	skunew = Mydb.GetNextFreeSKU();
//                }

                //Step 2 - Set all the values of the SpecsRecord
                nr.SKU = (uint)skunew;
                nr.SphereOD = Convert.ToSingle(tb_Add_SphereOD.Text);
                nr.SphereOS = Convert.ToSingle(tb_Add_SphereOS.Text);
                nr.CylOD = Convert.ToSingle(tb_Add_CylOD.Text);
                nr.CylOS = Convert.ToSingle(tb_Add_CylOS.Text);
                nr.AxisOD = Convert.ToInt16(tb_Add_AxisOD.Text);
                nr.AxisOS = Convert.ToInt16(tb_Add_AxisOS.Text);
                nr.AddOD = Convert.ToSingle(tb_Add_AddOD.Text);
                nr.AddOS = Convert.ToSingle(tb_Add_AddOS.Text);

                if (rb_Add_Male.Checked)
                    nr.Gender = SpecGender.Male;
                else if (rb_Add_Female.Checked)
                    nr.Gender = SpecGender.Female;
                else
                    nr.Gender = SpecGender.Uni;

                if (rb_Add_Single.Checked)
                    nr.Type = SpecType.Single;
                else
                    nr.Type = SpecType.Multi;

                if (rb_Add_NoTint.Checked)
                    nr.Tint = SpecTint.None;
                else if (rb_Add_LightTint.Checked)
                    nr.Tint = SpecTint.Light;
                else
                    nr.Tint = SpecTint.Dark;
				
                if(rb_Add_Small.Checked)
                	nr.Size = SpecSize.Small;
                else if (rb_Add_Medium.Checked)
                	nr.Size = SpecSize.Medium;
                else if (rb_Add_Large.Checked)
                	nr.Size = SpecSize.Large;
                else
                	nr.Size = SpecSize.Child;
                
                nr.Comment = tb_Add_Comment.Text;
                nr.DateAdded = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

                //Step 3 - Add the new SpecsRecord to the database
                Mydb.Insert(nr, SSTable.Current);
                Mydb.GetCurrentInventory(); //Refresh the displayed inventory
                IncrementOps(); //Increment the number of ops by 1 if the appropriate prefs are set

                //If the database is not normal (that is, if it is a merge database), also add the record to the 
                //merge table and refresh it for display
                if (!GuiPrefs.NormalDatabase) {
                    Mydb.Insert(nr, SSTable.MergeItems);
                    dt_Add_MergeTable.Clear();
                    Mydb.GetTable(dt_Add_MergeTable, SSTable.MergeItems);
                }

                //Step 4 - Clear the entry fields and refresh the inventory
                Add_Clear_Controls();
                
                //Step 5 - Highlight the new item in the DataGridView
                for (int j = 0; j < dgv_Add_InventoryView.Rows.Count; j++)
                {
                    try {
                	    if(Convert.ToInt16(dgv_Add_InventoryView[0, j].Value) == skunew)
                	    {
                		    dgv_Add_InventoryView.Rows[j].Selected = true;
                		    if(j < 3)
                			    dgv_Add_InventoryView.FirstDisplayedScrollingRowIndex = 0;
                		    else
                			    dgv_Add_InventoryView.FirstDisplayedScrollingRowIndex = j - 2;
                	    }
                    } catch {}
                }
            }
        }

        //Click event for btn_Add_Clear - Resets all the input form elements to their defaults
        //for the Add/Remove tab
        private void btn_Add_Clear_Click(object sender, EventArgs e)
        {
        	Add_Clear_Controls();
        }

        //Click event for btn_Add_DeleteSelected
        private void btn_Add_DeleteSelected_Click(object sender, EventArgs e)
        {
        	//No db loaded, leave function
        	if(GuiPrefs.OpenDBPath == "") {
        		return;
        	}
        	
        	int numrecords = dgv_Add_InventoryView.Rows.Count;
        	int skutodel = -1;
        	int scrolling = dgv_Add_InventoryView.FirstDisplayedScrollingRowIndex;
        	
            for (int i = 0; i < numrecords; i++)
            {
            	if (dgv_Add_InventoryView.Rows[i].Selected)
                {
            		skutodel = Convert.ToInt16(dgv_Add_InventoryView[0, i].Value);
                    break;
                }
            }
            
            if(skutodel >= 0) //Record selected, confirm and delete.
            {
            	string str_delconfirm = String.Format("You are about to permanently delete " +
					"the record with SKU# {0} from the database.\n\n" +
					"-= DO NOT USE THIS SCREEN TO DISPENSE GLASSES =-\n\nAre you sure " +
					"you want to delete this record?", skutodel);
            	string str_caption = String.Format("Confirm Delete - SKU# {0}", skutodel);
            	if(MessageBox.Show(str_delconfirm, str_caption, 
            	                   MessageBoxButtons.YesNo, 
            	                   MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            	{
            		Mydb.Delete((uint)skutodel, SSTable.Current);
            		Mydb.GetCurrentInventory();  //Refreshes the list
                    if (!GuiPrefs.NormalDatabase) {
                        Mydb.Delete((uint)skutodel, SSTable.MergeItems);
                        dt_Add_MergeTable.Clear();
                        Mydb.GetTable(dt_Add_MergeTable, SSTable.MergeItems);
                    }
            		
            		if(scrolling < dgv_Add_InventoryView.Rows.Count - 1) {
            			dgv_Add_InventoryView.FirstDisplayedScrollingRowIndex = scrolling;
            		}
            		
            		IncrementOps(); //Increment ops by 1 if the appropriate prefs are set
            	}
            }
            else //No record selected when button was pushed
            	MessageBox.Show("Please select a record to delete from the list.",
            	           "No Record Selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        
                //Checks the value of each text box and keeps track of any invalid values.
        //If there are any problems, it tells the user and highlights the first control
        //with an error.
        private bool Add_Controls_Valid()
        {
            string ErrString = "Invalid Prescription - Please correct the following:\n\n";
            
            //Checks the RX text boxes
            if (!Rx_TextBox_Valid(tb_Add_SphereOD, RxBox.Sphere)) //|| !Rx_TextBox_ValidRange(tb_Add_SphereOD))
                ErrString += "OD Sphere must be a number between -" + SPHERE_MAX_VALUE + " and " + SPHERE_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_SphereOD, RxBox.Sphere))
            	ErrString += "OD Sphere must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_CylOD, RxBox.Cyl))// || !Rx_TextBox_ValidRange(tb_Add_CylOD))
                ErrString += "OD Cylinder must be a number between 0 and -" + CYLINDER_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_CylOD,  RxBox.Cyl))
            	ErrString += "OD Cylinder must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_AxisOD, RxBox.Axis))// || !Rx_TextBox_ValidRange(tb_Add_AxisOD))
                ErrString += "OD Axis must be a number between 0 and " + AXIS_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_AxisOD, RxBox.Axis))
            	ErrString += "OD Axis must be expressed in increments of 1.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_AddOD, RxBox.Add))// || !Rx_TextBox_ValidRange(tb_Add_AddOD))
                ErrString += "OD Add must be a number between 0 and " + ADD_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_AddOD, RxBox.Add))
            	ErrString += "OD Add must be expressed in increments of 0.25.\n";

            if (!Rx_TextBox_Valid(tb_Add_SphereOS, RxBox.Sphere))// || !Rx_TextBox_ValidRange(tb_Add_SphereOS))
                ErrString += "OS Sphere must be a number between -" + SPHERE_MAX_VALUE + " and " + SPHERE_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_SphereOS, RxBox.Sphere))
            	ErrString += "OS Sphere must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_CylOS, RxBox.Cyl))// || !Rx_TextBox_ValidRange(tb_Add_CylOS))
                ErrString += "OS Cylinder must be a number between 0 and -" + CYLINDER_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_CylOS, RxBox.Cyl))
            	ErrString += "OS Cylinder must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_AxisOS, RxBox.Axis))// || !Rx_TextBox_ValidRange(tb_Add_AxisOS))
                ErrString += "OS Axis must be a number between 0 and " + AXIS_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_AxisOS, RxBox.Axis))
            	ErrString += "OS Axis must be expressed in increments of 1.\n";
            
            if (!Rx_TextBox_Valid(tb_Add_AddOS, RxBox.Add))// || !Rx_TextBox_ValidRange(tb_Add_AddOS))
                ErrString += "OS Add must be a number between 0 and " + ADD_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_Add_AddOS, RxBox.Add))
            	ErrString += "OS Add must be expressed in increments of 0.25.\n";

            //If there are no errors.
            if (ErrString == "Invalid Prescription - Please correct the following:\n\n")
                return true;
            else
            {
                MessageBox.Show(ErrString);
                ErrString = "Invalid Prescription - Please correct the following:\n\n";
                return false;
            }
        }
        
        /// <summary>
        /// Clears the controls on the Add New Items tab page
        /// </summary>
        private void Add_Clear_Controls()
        {
        	tb_Add_SphereOD.Clear();
            tb_Add_SphereOS.Clear();
            tb_Add_CylOD.Clear();
            tb_Add_CylOS.Clear();
            tb_Add_AxisOD.Clear();
            tb_Add_AxisOS.Clear();
            tb_Add_AddOD.Clear();
            tb_Add_AddOS.Clear();
            tb_Add_Comment.Clear();
            dgv_Add_InventoryView.ClearSelection();
			tb_Add_SphereOD.Focus();
        }
	}
}