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
		//Leave event for tb_S_SphereOD
		//Checks for a valid value, reports invalid values to the user,
		//and updates tb_S_SphereOS if the OD Rx = OS Rx checkbox is checked
		private void tb_S_SphereOD_Leave(object sender, EventArgs e)
		{
			Rx_TextBox_Valid(tb_S_SphereOD, RxBox.Sphere);
		}
		
		//Leave event for tb_S_SphereOS
		private void tb_S_SphereOS_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_SphereOS, RxBox.Sphere);
		}
		
		//Leave event for tb_S_CylOD
		private void tb_S_CylOD_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_CylOD, RxBox.Cyl);
		}
		
		//Leave event for tb_S_CylOS
		private void tb_S_CylOS_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_CylOS, RxBox.Cyl);
		}
		
		//Leave event for tb_S_AxisOD
		private void tb_S_AxisOD_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_AxisOD, RxBox.Axis);
		}
		
		//Leave event for tb_S_AxisOS
		private void tb_S_AxisOS_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_AxisOS, RxBox.Axis);
		}
		
		//Leave event for tb_S_AddOD
		private void tb_S_AddOD_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_AddOD, RxBox.Add);
		    
		    try
		    {
		    	if (Convert.ToSingle(tb_S_AddOD.Text) != 0) { //If AddOD is a nonzero number
		    		rb_S_Multi.Checked = true;
                } else if (Convert.ToSingle(tb_S_AddOS.Text) == 0) { //If AddOS (not a typo) is also zero
		    		rb_S_Single.Checked = true;
                }
		    }
		    catch{}

            //If the text in AddOS is invalid, remove it
            try {
                Convert.ToSingle(tb_S_AddOS.Text);
            } catch {
                tb_S_AddOS.Clear();
            }

            //Duplicate the value of this textbox on the OS side if nothing had been entered previously
            if (tb_S_AddOS.Text.Length == 0) {
                tb_S_AddOS.Text = tb_S_AddOD.Text;
            }
		}
		
		//Leave event for tb_S_AddOS
		private void tb_S_AddOS_Leave(object sender, EventArgs e)
		{
		    Rx_TextBox_Valid(tb_S_AddOS, RxBox.Add);
		    
		    try
		    {
		    	if (Convert.ToSingle(tb_S_AddOS.Text) != 0) { //If AddOS is a nonzero number
		    		rb_S_Multi.Checked = true;
                } else if (Convert.ToSingle(tb_S_AddOD.Text) == 0) { //If AddOD (not a typo) is also zero
		    		rb_S_Single.Checked = true;
                }
		    }
		    catch{}

            //If the text in AddOD is invalid, remove it
            try {
                Convert.ToSingle(tb_S_AddOD.Text);
            } catch {
                tb_S_AddOD.Clear();
            }

            //Duplicate the value of this textbox on the OS side if nothing had been entered previously
            if (tb_S_AddOD.Text.Length == 0) {
                tb_S_AddOD.Text = tb_S_AddOS.Text;
            }
		}
		
		//Search button clicked.
		private void btn_S_Search_Click(object sender, EventArgs e)
		{
			if (GuiPrefs.OpenDBPath == "") {
				return;
			}
			
		    //If the controls have valid fields, process the search
		    if (SD_Controls_Valid())
		    {
		    	SpecsRecord nr = new SpecsRecord();
		        int deye;
		    	
		    	//Displays/hides the auxiliary data grid view and resizes them both
		    	if(chb_S_SplitMultifocals.Checked)
		    	{
                    dgv_S_Distance.Visible = true;
                    dgv_S_Closeup.Visible = true;
                    dgv_S_SearchResults.Visible = false;
		    		lb_S_Distance.Visible = true;
		    		lb_S_Closeup.Visible = true;
		    	}
		    	else
		    	{
                    dgv_S_Distance.Visible = false;
                    dgv_S_Closeup.Visible = false;
                    dgv_S_SearchResults.Visible = true;
		    		lb_S_Distance.Visible = false;
		    		lb_S_Closeup.Visible = false;
		    	}
		    	MainForm_ResizeEnd(sender, e);
		
		        //Step 1 - Create the search record
		        nr.SphereOD = Convert.ToSingle(tb_S_SphereOD.Text);
		        nr.CylOD = Convert.ToSingle(tb_S_CylOD.Text);
		        nr.AxisOD = Convert.ToInt16(tb_S_AxisOD.Text);
		        nr.AddOD = Convert.ToSingle(tb_S_AddOD.Text);
		        nr.SphereOS = Convert.ToSingle(tb_S_SphereOS.Text);
		        nr.CylOS = Convert.ToSingle(tb_S_CylOS.Text);
		        nr.AxisOS = Convert.ToInt16(tb_S_AxisOS.Text);
		        nr.AddOS = Convert.ToSingle(tb_S_AddOS.Text);
		
		        //Multi/single type check
		        if (rb_S_Single.Checked)
		            nr.Type = SpecType.Single;
		        else
		            nr.Type = SpecType.Multi;
		        
		        //Gender check
		        if(rb_S_AnyGender.Checked)
		        	nr.Gender = null;
		        else if(rb_S_Male.Checked)
		        	nr.Gender = SpecGender.Male;
		        else if(rb_S_Female.Checked)
		        	nr.Gender = SpecGender.Female;
		        else
		        	nr.Gender = SpecGender.Uni;
		        
		        //Size check
		        if(rb_S_AnySize.Checked)
		        	nr.Size = null;
		        else if(rb_S_Small.Checked)
		        	nr.Size = SpecSize.Small;
		        else if(rb_S_Medium.Checked)
		        	nr.Size = SpecSize.Medium;
		        else if(rb_S_Large.Checked)
		        	nr.Size = SpecSize.Large;
		        else
		        	nr.Size = SpecSize.Child;
		        
		        //Dominant eye check
		        if (rb_S_OD.Checked)
		        	deye = (int)DomEye.OD;
		        else if (rb_S_OS.Checked)
		        	deye = (int)DomEye.OS;
		        else
		        	deye = (int)DomEye.OU;
		        
		        //Step 2 - Call the Rx Search function to search and score matching specs
		        //		 - Sort by score
		        Mydb.RxSearch(nr, (DomEye)deye, chb_S_SplitMultifocals.Checked);
		        if(dgv_S_SearchResults.Visible) {
                    dgv_S_SearchResults.Sort(dgv_S_SearchResults.Columns[1], ListSortDirection.Ascending);
                } else {
                    dgv_S_Distance.Sort(dgv_S_Distance.Columns[1], ListSortDirection.Ascending);
                    dgv_S_Closeup.Sort(dgv_S_Closeup.Columns[1], ListSortDirection.Ascending);
                }
		        //dgv_S_AuxResults.Sort(dgv_S_AuxResults.Columns[1], ListSortDirection.Ascending);
		        
		        //Step 3 - If empty, display a message box indicating no results, otherwise scroll to the top of the results
		        if(Mydb.DBResults.Rows.Count > 0) {
		        	dgv_S_SearchResults.FirstDisplayedScrollingRowIndex = 0;
                    dgv_S_Distance.FirstDisplayedScrollingRowIndex = 0;
                } else {
                    MessageBox.Show("No matches found within acceptable tolerance of specified prescription.", 
		        	                "No matches.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                //Do the same for the Closeup table
                if(Mydb.DBResultsAux.Rows.Count > 0) {
                    dgv_S_Closeup.FirstDisplayedScrollingRowIndex = 0;
                }
		        
		        //Step 4 -Clear selections for both data grid views
//				dgv_S_AuxResults.FirstDisplayedScrollingRowIndex = 0;
		        dgv_S_SearchResults.ClearSelection();
                dgv_S_Distance.ClearSelection();
                dgv_S_Closeup.ClearSelection();
//		        dgv_S_AuxResults.ClearSelection();
		    }
		}
		
		//Click event for btn_S_Clear - Resets all the input form elements to their defaults
		//for the Search/Dispense tab
		private void btn_S_Clear_Click(object sender, EventArgs e)
		{
		    tb_S_SphereOD.Clear();
		    tb_S_SphereOS.Clear();
		    tb_S_CylOD.Clear();
		    tb_S_CylOS.Clear();
		    tb_S_AxisOD.Clear();
		    tb_S_AxisOS.Clear();
		    tb_S_AddOD.Clear();
		    tb_S_AddOS.Clear();
		    Mydb.DBResults.Clear();
		    Mydb.DBResultsAux.Clear();
		
		    chb_S_SplitMultifocals.Checked = false;
		    rb_S_Single.Checked = true;
		}
		
		//Click event for btn_S_Dispense
		//Moves the selected records from Current Inventory to Dispensed Inventory
//        private void btn_S_Dispense_Click(object sender, EventArgs e)
//        {
//            bool main_selected = false;
//            bool aux_selected = false;
		
//            SpecsRecord sr = new SpecsRecord();
//            SpecsRecord sra = new SpecsRecord(); //Aux record
			
//            //Handle the search results
//            if (chb_S_SplitMultifocals.Checked) {
//                int rc = dgv_S_SearchResults.Rows.Count;

//                for (int i = 0; i < rc; i++)
//                {
//                    if(dgv_S_SearchResults.Rows[i].Selected == true)
//                    {
//                        sr.SKU = Convert.ToUInt16(dgv_S_SearchResults[0, i].Value);
//                        sr.SphereOD = Convert.ToSingle(dgv_S_SearchResults[2, i].Value);
//                        sr.CylOD = Convert.ToSingle(dgv_S_SearchResults[3, i].Value);
//                        sr.AxisOD = Convert.ToInt16(dgv_S_SearchResults[4, i].Value);
//                        sr.AddOD = Convert.ToSingle(dgv_S_SearchResults[5, i].Value);
//                        sr.SphereOS = Convert.ToSingle(dgv_S_SearchResults[6, i].Value);
//                        sr.CylOS = Convert.ToSingle(dgv_S_SearchResults[7, i].Value);
//                        sr.AxisOS = Convert.ToInt16(dgv_S_SearchResults[8, i].Value);
//                        sr.AddOS = Convert.ToSingle(dgv_S_SearchResults[9, i].Value);
//                        sr.Type = dgv_S_SearchResults[10, i].Value.ToString();
//                        sr.Gender = dgv_S_SearchResults[11, i].Value.ToString();
//                        sr.Size = dgv_S_SearchResults[12, i].Value.ToString();
//                        sr.Tint = dgv_S_SearchResults[13, i].Value.ToString();
//                        sr.DateAdded = dgv_S_SearchResults[14, i].Value.ToString();
//                        sr.DateDispensed = DateTime.Now.ToString();
//                        sr.Comment = dgv_S_SearchResults[15, i].Value.ToString();
//                        main_selected = true;
//                        break;	//No multiple selects allowed, so exit the loop once we've found the one
//                    }
//                }
//            } else {
//            //Handle the auxiliary selection (only when "Split Multifocals" is checked
////			if (chb_S_SplitMultifocals.Checked)
////			{
//                int rc = dgv_S_Closeup.Rows.Count;
////				rc = dgv_S_AuxResults.Rows.Count;
//                for (int i = 0; i < rc; i++)
//                {
////		    		if(dgv_S_AuxResults.Rows[i].Selected == true)
//                    if(dgv_S_Closeup.Rows[i].Selected == true)
//                    {
//                        sra.SKU = Convert.ToUInt16(dgv_S_Closeup[0, i].Value);
//                        sra.SphereOD = Convert.ToSingle(dgv_S_Closeup[2, i].Value);
//                        sra.CylOD = Convert.ToSingle(dgv_S_Closeup[3, i].Value);
//                        sra.AxisOD = Convert.ToInt16(dgv_S_Closeup[4, i].Value);
//                        sra.AddOD = Convert.ToSingle(dgv_S_Closeup[5, i].Value);
//                        sra.SphereOS = Convert.ToSingle(dgv_S_Closeup[6, i].Value);
//                        sra.CylOS = Convert.ToSingle(dgv_S_Closeup[7, i].Value);
//                        sra.AxisOS = Convert.ToInt16(dgv_S_Closeup[8, i].Value);
//                        sra.AddOS = Convert.ToSingle(dgv_S_Closeup[9, i].Value);
//                        sra.Type = dgv_S_Closeup[10, i].Value.ToString();
//                        sra.Gender = dgv_S_Closeup[11, i].Value.ToString();
//                        sra.Size = dgv_S_Closeup[12, i].Value.ToString();
//                        sra.Tint = dgv_S_Closeup[13, i].Value.ToString();
//                        sra.DateAdded = dgv_S_Closeup[14, i].Value.ToString();
//                        sra.DateDispensed = DateTime.Now.ToString();
//                        sra.Comment = dgv_S_Closeup[15, i].Value.ToString();
//                        aux_selected = true;
//                        break;	//No multiple selects allowed, so exit the loop once we've found the one
//                    }
//                }
//            }
			
//            //Dispense
//            if(!chb_S_SplitMultifocals.Checked)
//            {
//                if(main_selected)
//                {
//                    Mydb.Dispense(sr, true);
//                    MessageBox.Show(String.Format("Glasses with the " +
//                        "following SKU number have been dispensed.\n\n{0}", sr.SKU),
//                         "Glasses Dispensed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
//                    Mydb.GetCurrentInventory();
//                }
//                else
//                    MessageBox.Show("Please select one available record from the list to dispense.", 
//                                    "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//            else
//            {
//                if(main_selected && aux_selected)
//                {
//                    Mydb.Dispense(sr, true);
//                    Mydb.Dispense(sra, false);
//                    MessageBox.Show(String.Format("Glasses with the " +
//                        "following SKU numbers have been dispensed.\n\n{0}\n{1}", sr.SKU, sra.SKU),
//                         "Glasses Dispensed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
//                    Mydb.GetCurrentInventory();
//                }
//                else
//                    MessageBox.Show("Please select one available record from each of the lists to dispense.", 
//                                    "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
		
		private bool SD_Controls_Valid()
        {
            string ErrString = "Invalid Prescription - Please correct the following:\n\n";

            //Checks the OD text boxes
            if (!Rx_TextBox_Valid(tb_S_SphereOD, RxBox.Sphere))// || !Rx_TextBox_ValidRange(tb_S_SphereOD))
                ErrString += "OD Sphere must be a number between -" + SPHERE_MAX_VALUE + " and " + SPHERE_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_SphereOD, RxBox.Sphere))
            	ErrString += "OD Sphere must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_S_CylOD, RxBox.Cyl))// || !Rx_TextBox_ValidRange(tb_S_CylOD))
                ErrString += "OD Cylinder must be a number between 0 and -" + CYLINDER_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_CylOD, RxBox.Cyl))
            	ErrString += "OD Cylinder must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_S_AxisOD, RxBox.Axis))// || !Rx_TextBox_ValidRange(tb_S_AxisOD))
                ErrString += "OD Axis must be a number between 0 and " + AXIS_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_AxisOD, RxBox.Axis))
            	ErrString += "OD Axis must be expressed in increments of 1.\n";
            
            if (!Rx_TextBox_Valid(tb_S_AddOD, RxBox.Add))// || !Rx_TextBox_ValidRange(tb_S_AddOD))
                ErrString += "OD Add must be a number between 0 and " + ADD_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_AddOD, RxBox.Add))
            	ErrString += "OD Add must be expressed in increments of 0.25.\n";

            if (!Rx_TextBox_Valid(tb_S_SphereOS, RxBox.Sphere))// || !Rx_TextBox_ValidRange(tb_S_SphereOS))
                ErrString += "OS Sphere must be a number between -" + SPHERE_MAX_VALUE + " and " + SPHERE_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_SphereOS, RxBox.Sphere))
            	ErrString += "OS Sphere must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_S_CylOS, RxBox.Cyl))// || !Rx_TextBox_ValidRange(tb_S_CylOS))
                ErrString += "OS Cylinder must be a number between 0 and -" + CYLINDER_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_CylOS, RxBox.Cyl))
            	ErrString += "OS Cylinder must be expressed in increments of 0.25.\n";
            
            if (!Rx_TextBox_Valid(tb_S_AxisOS, RxBox.Axis))// || !Rx_TextBox_ValidRange(tb_S_AxisOS))
                ErrString += "OS Axis must be a number between 0 and " + AXIS_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_AxisOS, RxBox.Axis))
            	ErrString += "OS Axis must be expressed in increments of 1.\n";
            
            if (!Rx_TextBox_Valid(tb_S_AddOS, RxBox.Add))// || !Rx_TextBox_ValidRange(tb_S_AddOS))
                ErrString += "OS Add must be a number between 0 and " + ADD_MAX_VALUE + ".\n";
            else if(!Rx_TextBox_ValidIncrement(tb_S_AddOS, RxBox.Add))
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
    }
}