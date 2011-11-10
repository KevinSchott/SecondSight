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

/// <summary>
/// This file is responsible for additional configuration performed after the 
/// initialization function is called.
/// </summary>

using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace SecondSight
{
	public struct SSPrefs
	{
		//Program size/state prefs (not part of options panel)
		public int		Width;
		public int 		Height;
		public bool		Maximized;
        public bool     NormalDatabase;     //Show all the controls?  True if the opened database is a full SS database, false if it's a merge database
		public string 	OpenDBPath;			//Full path of the currently open database
		public string	OpenDBName;			//Name of the currently open database
		public string	OpenDBLoc;			//Location of the currently open database
		
		//Program behavior customization prefs
		public bool		OpenMostRecentDB;	//Automatically open the last open DB on startup?
		
		//Database prefs
		public string	DefaultBackupDir;	//Default backup folder
		public string	DefaultDBDir;		//Default database folder
		
		//Automatic backup prefs
		public bool		AutoBackup;  //Perform automatic backups?
		public bool		ABAfterTime; //After an amount of time?
		public short	ABTime;		 //How much time
		public bool		ABAfterOps;  //After a number of operations (add/delete/dispense)?
		public short	ABOps;		 //How many ops?
		public short	ABNumberKept;//How many auto backup files are kept.
	}

	partial class MainForm
	{
		//Constants
		private const float SPHERE_MAX_VALUE = 20.0f;
        private const float CYLINDER_MAX_VALUE = 10.0f;
        private const float AXIS_MAX_VALUE = 180.0f;
        private const float ADD_MAX_VALUE = 5.0f;

		private SSDataBase Mydb = new SSDataBase();  //The currently open database
		private FormWindowState LastWindowState;	 //Maximized, minimized, non-maximized
	    private SSPrefs GuiPrefs;					 //Stores user prefs loaded from the prefs file
//	    private string installpath;					 //Application's install directory
        private string mydocspath;                   //User's My Documents path
        private string prefspath;                  //Local appdata path
	    private DataTable dt_V_DispensedTable;		 //Data table that holds dispensed inventory
        private DataTable dt_Add_MergeTable;         //Data table that holds the Merge subset of inventory in a Merge database
	    
        /// <summary>
        /// Performs additional configuration on controls, based on the information set by LoadPrefs
        /// </summary>
		private void ConfigureComponent()
		{
            dt_Add_MergeTable = new DataTable();
			dt_V_DispensedTable = new DataTable();
			LoadPrefs();
			
			Size = new System.Drawing.Size(GuiPrefs.Width, GuiPrefs.Height);
			LastWindowState = WindowState;

            //Set maximized state of the program
            if(GuiPrefs.Maximized) {
            	WindowState = FormWindowState.Maximized;
            } else {
            	WindowState = FormWindowState.Normal;
            }

		#region Add New Item tabpage config
            bs_Add_InventorySource = new BindingSource();
            bs_Add_InventorySource.DataSource = Mydb.InvResults;
            dgv_Add_InventoryView.ConfigureDGV();
            dgv_Add_InventoryView.Columns["Score"].Visible = false;
            dgv_Add_InventoryView.Columns["DateDispensed"].Visible = false;
            dgv_Add_InventoryView.DataSource = bs_Add_InventorySource;

            foreach (DataGridViewColumn col in dgv_Add_InventoryView.Columns) {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
		#endregion

		#region Search tabpage config
            dgv_S_SearchResults.ConfigureDGV();
            dgv_S_SearchResults.Columns["DateDispensed"].Visible = false;
            dgv_S_SearchResults.DataSource = Mydb.DBResults;
            dgv_S_Distance.ConfigureDGV();
            dgv_S_Distance.Columns["DateDispensed"].Visible = false;
            dgv_S_Distance.DataSource = Mydb.DBResults;
            dgv_S_Closeup.ConfigureDGV();
            dgv_S_Closeup.Columns["DateDispensed"].Visible = false;
            dgv_S_Closeup.DataSource = Mydb.DBResultsAux;
		#endregion

		#region Dispense tabpage config
			dispenseTable = new DataTable();
            deleteTable = new DataTable();
		#endregion

		#region View Inventory tabpage config
            bs_V_SearchByField = new BindingList<KeyValuePair<string, string>>();
            bs_V_InventorySource = new BindingSource();
            bs_V_InventorySource.DataSource = Mydb.InvResults;

            bs_V_SearchByField.Add(new KeyValuePair<string, string>("SKU", "SKU"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OD Sphere", "SphereOD"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OD Cylinder", "CylinderOD"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OD Axis", "AxisOD"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OD Add", "AddOD"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OS Sphere", "SphereOS"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OS Cylinder", "CylinderOS"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OS Axis", "AxisOS"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("OS Add", "AddOS"));
            bs_V_SearchByField.Add(new KeyValuePair<string, string>("Date Added", "DateAdded"));

            dgv_V_InventoryView.ConfigureDGV();
            dgv_V_InventoryView.Columns["Score"].Visible = false;
            dgv_V_InventoryView.Columns["DateDispensed"].Visible = false;
            dgv_V_InventoryView.DataSource = bs_V_InventorySource;
            cb_V_SearchByField.DataSource = bs_V_SearchByField; 
            cb_V_SearchByField.DisplayMember = "Key";
            cb_V_SearchByField.ValueMember = "Value";
            cb_V_SearchByField.SelectedIndex = 0;
			cb_V_SearchIn.SelectedIndex = 0;
            foreach (DataGridViewColumn col in dgv_V_InventoryView.Columns) {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

		#endregion
			
		#region Reports tabpage config
            bs_R_FullLists = new BindingSource();
            bs_R_Summaries = new BindingSource();

            cb_R_ReportSource.SelectedIndex = 0;
            cb_R_ReportType.SelectedIndex = 0;
            cb_R_GroupBy.SelectedIndex = 0;

            dgv_R_FullLists.ConfigureDGV();
            dgv_R_FullLists.Columns["Score"].Visible = false;
            dgv_R_FullLists.Columns["DateDispensed"].Visible = false;
            foreach (DataGridViewColumn col in dgv_R_FullLists.Columns) {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            dgv_R_FullLists.DataSource = bs_R_FullLists;
            dgv_R_Summaries.DataSource = bs_R_Summaries;

            ZedGraph.GraphPane gpane = zed_R_Chart.GraphPane;
            gpane.Title.Text = "Report Results";
		#endregion
		

        //A db was opened last time, attempt to open it
            if(GuiPrefs.OpenMostRecentDB) {
                try {
            	    Mydb.OpenDB(GuiPrefs.OpenDBPath); //Valid SecondSight database
	            } catch {
                    return; //Fail quietly since this happens at startup
                }

                UpdateAfterOpenDB();
            }
		}
		
        /// <summary>
        /// Loads and sets the preferences from the prefs file.  Sets any default prefs.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the prefs file is not found</exception>
		private void LoadPrefs()
        {
            //Get the install path and store it in the member variable
//            System.Reflection.Assembly tasm = System.Reflection.Assembly.GetEntryAssembly();
            mydocspath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);   //Get the path to My Documents
            prefspath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);  //Get the local appdata path (for prefs)
//            installpath = System.IO.Path.GetDirectoryName(tasm.Location);

//            string DEFAULTDBPATH = installpath + "\\Databases";
            string DEFAULTDBPATH = mydocspath + "\\SecondSight Databases";
            string DEFAULTBACKUPSPATH = DEFAULTDBPATH + "\\Backups";
            const short DEFAULTSCREENWIDTH = 800;
            const short DEFAULTSCREENHEIGHT = 600;
            const bool DEFAULTOPENRECENT = true;
            const bool DEFAULTISMAXED = false;
            const bool DEFAULTISNORMALDB = true;
            const bool DEFAULTAUTOBACKUP = true;
            const bool DEFAULTABAFTEROPS = true;
            const bool DEFAULTABAFTERTIME = false;
            const short DEFAULTABTIME = 0;
		    const short DEFAULTABOPS = 10;
		    const short DEFAULTABNUMBERKEPT = 5;

            prefspath = prefspath + "\\SecondSight\\prefs.txt";
//            string prefspath = installpath + "\\prefs.txt";
            Hashtable ht = new Hashtable();

        	GuiPrefs.Width = DEFAULTSCREENWIDTH;
        	GuiPrefs.Height = DEFAULTSCREENHEIGHT;
        	GuiPrefs.Maximized = DEFAULTISMAXED;
            GuiPrefs.NormalDatabase = DEFAULTISNORMALDB;    //True while no database is opened
        	GuiPrefs.OpenDBLoc = "";
        	GuiPrefs.OpenDBPath = "";
        	GuiPrefs.OpenDBName = "";

            //If no prefs file exists, load default prefs.  Do not create file, that is handled on program exit
            if (!File.Exists(prefspath)) {
                GuiPrefs.OpenMostRecentDB = DEFAULTOPENRECENT;
                GuiPrefs.ABNumberKept = DEFAULTABNUMBERKEPT;
                GuiPrefs.ABOps = DEFAULTABOPS;
                GuiPrefs.ABAfterOps = DEFAULTABAFTEROPS;
                GuiPrefs.ABAfterTime = DEFAULTABAFTERTIME;
                GuiPrefs.ABTime = DEFAULTABTIME;
                GuiPrefs.AutoBackup = true;
                GuiPrefs.DefaultDBDir = DEFAULTDBPATH;//installpath);
                GuiPrefs.DefaultBackupDir = DEFAULTBACKUPSPATH; //installpath);
                try {
                    Directory.CreateDirectory(GuiPrefs.DefaultBackupDir);
                    Directory.CreateDirectory(Path.GetDirectoryName(prefspath));
                } catch { }
                return;
            }

        	//Read the prefs file and load them into the data structure        	
        	using (StreamReader sr = new StreamReader(prefspath))
		    {
		       	string line;
		       		
		       	while((line = sr.ReadLine()) != null) {
		       		try {
			       		string prefname = line.Substring(0, line.IndexOf("="));
			       		string prefval = line.Substring(line.IndexOf("=")+1);
			       		ht[prefname] = prefval;
		       		} catch (Exception){} //Just ignore lines that aren't in the prefs format
		       	} 
		    }

        	//Programmatically generated prefs
            try {
                GuiPrefs.Width = Convert.ToInt16(ht["Width"]);
            } catch {
                GuiPrefs.Width = DEFAULTSCREENWIDTH;
            }

            try {
                GuiPrefs.Height = Convert.ToInt16(ht["Height"]);
            } catch {
                GuiPrefs.Height = DEFAULTSCREENHEIGHT;
            }

            try {
                if (Convert.ToInt16(ht["Maximized"]) != 0) {
                    GuiPrefs.Maximized = true;
                } else {
                    GuiPrefs.Maximized = false;
                }
            }
            catch { //Pref exists but was not formatted correctly
                GuiPrefs.Maximized = DEFAULTISMAXED;
            }

            try {
                GuiPrefs.OpenDBPath = ht["OpenDBPath"].ToString();
                GuiPrefs.OpenDBName = ht["OpenDBName"].ToString();
                GuiPrefs.OpenDBLoc = ht["OpenDBLoc"].ToString();
            } catch {
                GuiPrefs.OpenDBLoc = "";
                GuiPrefs.OpenDBPath = "";
                GuiPrefs.OpenDBName = "";
            }
	       		
	       	//User-defined prefs
	       	//General prefs
	       	try {
		       	if(ht["OpenMostRecentDB"] != null) {
		       		GuiPrefs.OpenMostRecentDB = Convert.ToBoolean(ht["OpenMostRecentDB"]);
		       	} else {
                    GuiPrefs.OpenMostRecentDB = DEFAULTOPENRECENT; //Pref does not exist, default to true
		       	}
	       	} catch { //Pref exists but was not formatted correctly
                GuiPrefs.OpenMostRecentDB = DEFAULTOPENRECENT;
	       	}
	       		
	       	//Database prefs
	       	//Default database file location
	       	if(ht["DefaultDBDir"] != null) {
	       		try {
	       			GuiPrefs.DefaultDBDir = ht["DefaultDBDir"].ToString();
	       			if(GuiPrefs.DefaultDBDir == "") {
	       				GuiPrefs.DefaultDBDir = DEFAULTDBPATH;
	       			}
	       		} catch {
	       			GuiPrefs.DefaultDBDir = DEFAULTDBPATH;
	       		}
	       	} else {
	       		GuiPrefs.DefaultDBDir = DEFAULTDBPATH;
	       	}
	       		
	       	//Default backup file location
	       	if(ht["DefaultBackupDir"] != null) {
	       		try {
	       			GuiPrefs.DefaultBackupDir = ht["DefaultBackupDir"].ToString();
	       			if(GuiPrefs.DefaultBackupDir == "") {
	       				GuiPrefs.DefaultBackupDir = DEFAULTBACKUPSPATH;
	       			}
	       		} catch {
                    GuiPrefs.DefaultBackupDir = DEFAULTBACKUPSPATH;
	       		}
	       	} else {
                GuiPrefs.DefaultBackupDir = DEFAULTBACKUPSPATH;
	       	}
	       		
	       	//Perform automatic backups
	       	if(ht["AutoBackup"] != null) {
	       		try {
	       			GuiPrefs.AutoBackup = Convert.ToBoolean(ht["AutoBackup"]);
	       		} catch {
                    GuiPrefs.AutoBackup = DEFAULTAUTOBACKUP;
	       		}
	       	} else {
	       		GuiPrefs.AutoBackup = DEFAULTAUTOBACKUP;
	       	}
	       		
	       	//Auto backup after time
	       	if(ht["ABAfterTime"] != null) {
	       		try {
	       			GuiPrefs.ABAfterTime = Convert.ToBoolean(ht["ABAfterTime"]);
	       		} catch {
	       			GuiPrefs.ABAfterTime = DEFAULTABAFTERTIME;
	       		}
	       	} else {
                GuiPrefs.ABAfterTime = DEFAULTABAFTERTIME;
	       	}
	       		
	       	//Auto backup time
	       	if(ht["ABTime"] != null) {
	       		try {
	       			GuiPrefs.ABTime = Convert.ToInt16(ht["ABTime"]);
	       		} catch {
	       			GuiPrefs.ABTime = DEFAULTABTIME;
	       		}
	       	} else {
	       		GuiPrefs.ABTime = DEFAULTABTIME;
	       	}
	       		
	       	//Auto backup after ops
	       	if(ht["ABAfterOps"] != null) {
	       		try {
	       			GuiPrefs.ABAfterOps = Convert.ToBoolean(ht["ABAfterOps"]);
	       		} catch {
	       			GuiPrefs.ABAfterOps = DEFAULTABAFTEROPS;
	       		}
	       	} else {
                GuiPrefs.ABAfterOps = DEFAULTABAFTEROPS;
	       	}
	       		
	       	//Auto backup after number of ops
	       	if(ht["ABOps"] != null) {
	       		try {
	       			GuiPrefs.ABOps = Convert.ToInt16(ht["ABOps"]);
	       		} catch {
	       			GuiPrefs.ABOps = DEFAULTABOPS;
	       		}
	       	} else {
	       		GuiPrefs.ABOps = DEFAULTABOPS;
	       	}
	       		
	       	//Number of automatic backups kept
	       	if(ht["ABNumberKept"] != null) {
	       		try {
	       			GuiPrefs.ABNumberKept = Convert.ToInt16(ht["ABNumberKept"]);
	       		} catch {
	       			GuiPrefs.ABNumberKept = DEFAULTABNUMBERKEPT;
	       		}
	       	} else {
	       		GuiPrefs.ABNumberKept = DEFAULTABNUMBERKEPT;
	       	}
        }
	}
}