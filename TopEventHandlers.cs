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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using ExcelLibrary.Office.Excel;
using SecondSight.OptionsDialog;
using SecondSight.Merge;

namespace SecondSight
{
	partial class MainForm
	{
		enum RxBox {Sphere = 1, Cyl, Axis, Add};
		
		private const int ADD_GB_W_OFFSET = 258;
		private const int ADD_GB_H_OFFSET = 120;
		private const int ADD_DGV_W_OFFSET = 272;
		private const int ADD_DGV_H_OFFSET = 146;
		private const int ADD_BTN_W_OFFSET = 122;
		private const int ADD_BTN_H_OFFSET = 6;
		private const int VIEW_DGV_W_OFFSET = 40;
		private const int VIEW_DGV_H_OFFSET = 152;
		
		//KeyDown event for any Rx text box
        //Handles: Enter
        private void RxTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
		
        /// <summary>
        /// Window size change event.  
        /// </summary>
		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
            if(WindowState != FormWindowState.Maximized)
            {
            	GuiPrefs.Width = Width;
	            GuiPrefs.Height = Height;
            }          
		}
		
		//Closed event for the main form - Writes the current prefs to the prefs.txt file
		private void MainForm_FormClosed(object sender, EventArgs e)
		{
            string tp = prefspath; //String.Format("{0}\\prefs.txt", mydocspath); //installpath);
			
			//Make an auto backup on exit if pref is set
			try {
				if(GuiPrefs.AutoBackup) {
					PerformAutoBackup();
				}
			} catch {}
			
        	using (StreamWriter sw = new StreamWriter(tp, false))
        	{
        		//File description and warning
        		sw.WriteLine("//This is the SecondSight preferences file, prefs.txt.  If these values are modified, the program " +
        		             "may crash or behave unexpectedly.  Do not manually modify this file unless you know what you're doing." +
        		             Environment.NewLine + "//The file can be safely deleted in the case of problems, as the program will generate a " +
        		             "new one if prefs.txt cannot be found.");
        		//Automatic prefs (program state)
        		sw.WriteLine(Environment.NewLine + "//Automatic Prefs: These items are generated automatically and represent information about the" +
	        		Environment.NewLine + "//  state the program was in when it was last closed." +
	        		Environment.NewLine + "//Width\t\t- The width of the program window" +
	        		Environment.NewLine + "//Height\t- The height of the program window" +
	        		Environment.NewLine + "//Maximized\t- Whether the program was maximized when it was last closed" +
	        		Environment.NewLine + "//OpenDBName\t- The name of the last open database" +
	        		Environment.NewLine + "//OpenDBLoc\t- The physical location of the last open database (not the file path)" +
	        		Environment.NewLine + "//OpenDBPath\t- The file path to the last open database file");
        		sw.WriteLine(String.Format("Width={0}", GuiPrefs.Width));
	            sw.WriteLine(String.Format("Height={0}", GuiPrefs.Height));
	            if(WindowState == FormWindowState.Maximized)
	            	sw.WriteLine("Maximized=1");
	            else
	            	sw.WriteLine("Maximized=0");
	            
	            sw.WriteLine(String.Format("OpenDBName={0}", GuiPrefs.OpenDBName));
	            sw.WriteLine(String.Format("OpenDBLoc={0}", GuiPrefs.OpenDBLoc));
	            sw.WriteLine(String.Format("OpenDBPath={0}", GuiPrefs.OpenDBPath));
	            
	            //General prefs
	            sw.WriteLine(Environment.NewLine + "//General Prefs: These are general user-defined preferences" +
	                         Environment.NewLine + "//OpenMostRecentDB\t- Whether to open the most recently open database when the program starts" +
	                         Environment.NewLine + "//ODColumnColor\t\t- The color of the OD Rx columns in the database grid views" +
	                         Environment.NewLine + "//OSColumnColor\t\t- The color of the OS Rx columns in the database grid views");
	            sw.WriteLine(String.Format("OpenMostRecentDB={0}", GuiPrefs.OpenMostRecentDB));
//	            sw.WriteLine(String.Format("ODColumnColor={0}", GuiPrefs.ODColumnColor.ToArgb()));
//	            sw.WriteLine(String.Format("OSColumnColor={0}", GuiPrefs.OSColumnColor.ToArgb()));
	            
	            //Database prefs
	            sw.WriteLine(Environment.NewLine + "//Database prefs: these define database file locations and automatic backup preferences." +
					Environment.NewLine + "//DefaultDBDir\t\t- The path to the folder where SecondSight will first look for database files." +
					Environment.NewLine + "//DefaultBackupDir\t- The path to the folder where SecondSight will first look for backup files." +
					Environment.NewLine + "//AutoBackup\t\t- Perform automatic backups? 0 = No, nonzero = Yes" +
					Environment.NewLine + "//ABAfterTime\t\t- Backup current database after a set period of time? 0 = No, nonzero = Yes" +
					Environment.NewLine + "//ABTime\t\t- Time interval for automatic backup, in minutes (ignored if ABAfterTime is 0)" +
					Environment.NewLine + "//ABAfterOps\t\t\t- Backup current database after a number of operations (Add/Delete/Dispense)? 0 = No, nonzero = Yes" +
					Environment.NewLine + "//ABOps\t\t- Number of operations between automatic backups (ignored if ABAfterOps is 0)" +
					Environment.NewLine + "//ABNumberKept\t\t- Number of the most recent backup files kept on disk.");
	            sw.WriteLine(String.Format("DefaultDBDir={0}", GuiPrefs.DefaultDBDir));
	            sw.WriteLine(String.Format("DefaultBackupDir={0}", GuiPrefs.DefaultBackupDir));
	            sw.WriteLine(String.Format("AutoBackup={0}", GuiPrefs.AutoBackup));
	            sw.WriteLine(String.Format("ABAfterTime={0}", GuiPrefs.ABAfterTime));
	            sw.WriteLine(String.Format("ABTime={0}", GuiPrefs.ABTime));
	            sw.WriteLine(String.Format("ABAfterOps={0}", GuiPrefs.ABAfterOps));
	            sw.WriteLine(String.Format("ABOps={0}", GuiPrefs.ABOps));
	            sw.WriteLine(String.Format("ABNumberKept={0}", GuiPrefs.ABNumberKept));
	            
	            Mydb.CloseDB();
        	}
		}
		
		
		
		#region Event Handler Support Functions

        /// <summary>
        /// UpdateAfterOpenDB - Updates the controls and other important variables after a database is opened.
        /// </summary>
        private void UpdateAfterOpenDB()
        {
            DataTable dt = new DataTable();
            Mydb.GetCurrentInventory();  //Load the current inventory into the datatable used by View and Search
        	dt_V_DispensedTable.Clear();
        	Mydb.GetTable(dt_V_DispensedTable, SSTable.Dispensed); //Load the dispensed inventory into the datatable used by View
        	Mydb.GetDBInfo(dt); //Load the database info

            GuiPrefs.OpenDBPath = Mydb.MyPath;
        	if(dt.Rows.Count > 0)
        	{
        		//Set the database info to corresponding member variables
        		GuiPrefs.OpenDBName = dt.Rows[0][0].ToString();
        		GuiPrefs.OpenDBLoc = dt.Rows[0][1].ToString();
                
                string codb = String.Format("Database: {0} ({1})", GuiPrefs.OpenDBName, GuiPrefs.OpenDBLoc);
        		lb_Add_CurrentOpenDB.Text = codb;
        		lb_S_CurrentOpenDB.Text = codb;
        		lb_D_CurrentOpenDB.Text = codb;
        		lb_V_CurrentOpenDB.Text = codb;
                lb_R_CurrentOpenDB.Text = codb;
        	}

            //Check for merge info and alter display if necessary
            dt.Reset();
            dt_Add_MergeTable.Clear();
            try {
                Mydb.GetTable(dt, SSTable.MergeInfo);
                tb_Add_MinSKU.Text = dt.Rows[0][1].ToString();
                tb_Add_MaxSKU.Text = dt.Rows[0][2].ToString();
                Mydb.GetTable(dt_Add_MergeTable, SSTable.MergeItems);
                bs_Add_InventorySource.DataSource = dt_Add_MergeTable;
                Enable_AllControls(false);
            } catch { //If there's an exception, it's not a merge db
                bs_Add_InventorySource.DataSource = Mydb.InvResults;
                tb_Add_MinSKU.Clear();
                tb_Add_MaxSKU.Clear();
                Enable_AllControls(true);
            }

        	Enable_MenuItems(true);
        	ResetOps(); //Reset number of operations performed since last auto backup
        	ConfigureAutoBackupTimer(); //Configures (and resets) the timer for auto backups
        }
		
        /// <summary>
        /// Enables or disables the menu items that require an open database
        /// </summary>
        /// <param name="_enable">True to enable some of the menu items, false to disable them</param>
		private void Enable_MenuItems(bool _enable)
		{
			menu_FileBackupMenuItem.Enabled = _enable;
            menu_FileRestoreMenuItem.Enabled = _enable;
			menu_FileCloseDatabaseMenuItem.Enabled = _enable;
			menu_ToolsExportMenuItem.Enabled = _enable;
		}
		
        /// <summary>
        /// Enables or disables some of the controls
        /// </summary>
        /// <remarks>This function changes the program GUI based on the type of open database</remarks>
        /// <param name="_enable">True if all the controls are to be enabled, false if some are to be disabled</param>
        private void Enable_AllControls(bool _enable)
        {
            if (_enable) {
                GuiPrefs.NormalDatabase = true;
                if (tabc_MainForm.TabPages.Count == 1) { //If the tabs have been removed, re-add them, otherwise do nothing
                    tabc_MainForm.TabPages.Add(tabp_Search);
                    tabc_MainForm.TabPages.Add(tabp_Dispense);
                    tabc_MainForm.TabPages.Add(tabp_ViewInventory);
                    tabc_MainForm.TabPages.Add(tabp_Reports);
                }
                tb_Add_MinSKU.Enabled = true;
                tb_Add_MaxSKU.Enabled = true;
            } else {
                GuiPrefs.NormalDatabase = false;
                tabc_MainForm.TabPages.Remove(tabp_Search);
                tabc_MainForm.TabPages.Remove(tabp_Dispense);
                tabc_MainForm.TabPages.Remove(tabp_ViewInventory);
                tabc_MainForm.TabPages.Remove(tabp_Reports);
                tb_Add_MinSKU.Enabled = false;
                tb_Add_MaxSKU.Enabled = false;
            }
        }
        
        //Checks for valid input for Rx textboxes.  Checks the box passed in through "tb, "
        //using different criteria based on parameter the box represents (determined by "box").
        //Checks for valid ranges, does not check for valid increments.

        /// <summary>
        /// Checks for valid input for an Rx textbox
        /// </summary>
        /// <param name="tb">The box to check the contents of</param>
        /// <param name="box">The Rx parameter - Sphere, Cylinder, Axis or Add</param>
        /// <returns>True if the contents are valid, false otherwise</returns>
        /// <exception cref="System.FormatException">Thrown when the text is not a proper integer or real number</exception>
        private bool Rx_TextBox_Valid(TextBoxBase tb, RxBox box)
        {
        	int pos = tb.Text.LastIndexOf('.');
		    float val;
		    
		    if(box == RxBox.Axis)
		    {
		    	if(tb.TextLength == 0) //Blank
		    		tb.Text = "0";
		    	
		    	try { //Is the contents a valid integer?
		    		val = Math.Abs(Convert.ToInt16(tb.Text));
		    	}
		    	catch {
		    		return false;
		    	}
		    	
	    		tb.DeselectAll();
	    		tb.Text = val.ToString("0");
		    	
		    	//Checks for valid range
		    	if(val > AXIS_MAX_VALUE) {
		    		return false;
		    	}
		    }
		    else //Sphere, Cylinder or Add
		    {
		    	if(tb.TextLength == 0)
		    		tb.Text = "0.00";
		    	
		    	try { //Is the contents a valid real number?
		    		val = Convert.ToSingle(tb.Text);
		    	}
		    	catch {
		    		return false;
		    	}
		    	
	    		tb.DeselectAll();
	    		if(pos > -1) //Handle decimal
	    		{
	    			string ts = tb.Text.Substring(pos + 1);
	    			if(ts == "2" || ts == "7")
	    			{
	    				if(val < 0)
	    					val -= 0.05F;
	    				else
	    					val += 0.05F;
	    			}
	    		}
	    		if(box == RxBox.Cyl)
	    			val = -Math.Abs(val);
	    		tb.Text = val.ToString("0.00");
		    	
		    	//Checks for valid range
		    	if(box == RxBox.Sphere) {
		    		if(val > SPHERE_MAX_VALUE) {
		    			return false;
		    		}
		    	} else if(box == RxBox.Cyl) {
		    		if(-val > CYLINDER_MAX_VALUE) {
		    			return false;
		    		}
		    	} else {
		    		if(val > ADD_MAX_VALUE) {
		    			return false;
		    		}
		    	}
		    }
		    return true;
        }

        /// <summary>
        /// Checks if the contents of a textbox are in valid incrememnts for that parameter
        /// </summary>
        /// <remarks>For Axis, a valid increment is 1, for other parameters, a valid increment is 0.25</remarks>
        /// <param name="tb">The textbox to check</param>
        /// <param name="box">The Rx parameter - Sphere, Cylinder, Axis or Add</param>
        /// <returns>True if the increment is valid, false otherwise</returns>
        private bool Rx_TextBox_ValidIncrement(TextBox tb, RxBox box)
        {
        	float f = Math.Abs(Convert.ToSingle(tb.Text));
        	
        	if(box == RxBox.Axis) {
        		if(f % 1 != 0)
        			return false;
        	} else {
        		if(f % 0.25 != 0)
        			return false;
        	}
        	return true;
        }

        #endregion
	}
}