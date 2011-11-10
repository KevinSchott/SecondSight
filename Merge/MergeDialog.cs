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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SecondSight.Merge
{
    public partial class MergeDialog : Form
    {
        //Pages
        private MergePage1 page1;

        //Stores the path to the database that was open before the merge tool was started
        private string oldDBPath;

        //Vars to share between pages
        private MergeVars mvars;

        public MergeDialog()
        {
            InitializeComponent();
        }

        public MergeDialog(string _path, SSDataBase _mydb)
        {
            InitializeComponent();

            //Additional initialization
            DataTable dt = new DataTable();
            
            mvars = new MergeVars();
            mvars.defaultDBPath = _path;
            mvars.Masterdb = _mydb;
            oldDBPath = _mydb.MyPath;
            mvars.mergeDBs = new List<SSDataBase>();

            //Check the info of the currently loaded master database and copy it into mvars for later use
            try {
                mvars.Masterdb.GetDBInfo(dt);
                mvars.Masterdbname = Convert.ToString(dt.Rows[0][0]);
                mvars.Masterdblocation = Convert.ToString(dt.Rows[0][1]);
                mvars.Masterdbdate = String.Format("{0:MM/dd/yyyy}", dt.Rows[0][2]);
            } catch (InvalidOperationException) { //Happens when no database is loaded
                mvars.Masterdbname = "";
                mvars.Masterdblocation = "";
            }

            page1 = new MergePage1(mvars);

            Controls.Add(page1);
        }

        //Click event for the Merge button
        private void btn_Merge_Click(object sender, EventArgs e)
        {
            if (!mvars.Masterdb.IsOpen()) {  //If there's no master database selected
                MessageBox.Show("A master database must be selected in order for a merge to take place.  Please select one before continuing.", "No Master Database", 
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else if (mvars.mergeDBs.Count == 0) { //If there's no merge databases selected
                MessageBox.Show("Please add at least one database to be merged with the master database to the list.", "No Merge Databases Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else { //Ask for confirmation and merge
                if (MessageBox.Show(
                    String.Format("The master database will be merged with the {0} selected partial database{1}.  Only the current inventories " +
                    "will be merged into the master database. " +
                    "\n\nPress OK to perform this merge, or Cancel to go back.", mvars.mergeDBs.Count, mvars.mergeDBs.Count > 1 ? "s":""),
                    "Confirm Merge?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                {
                    //Perform Merge
                    Cursor.Current = Cursors.WaitCursor;

                    try {
                        SmallMerge(); //Perform the merge - added items only
                        MessageBox.Show("The selected partial databases were successfully merged with the master database.",
                            "Merge Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch {
                        MessageBox.Show("The merge could not be completed.  Check to make sure you have permission to write to the folder containing the master database.",
                            "Merge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    CloseMergeDBs();
                    
                    Cursor.Current = Cursors.Default;

                    DialogResult = DialogResult.OK;
                    Close();    //Done with the dialog
                }
            }
        }

        //Click event for the Cancel button
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            CloseMergeDBs();    //Close any merge databases in the list

            //If the master database was changed at some point, reload the old one
            try {
                if (mvars.Masterdb.MyPath != oldDBPath) {
                    mvars.Masterdb.CloseDB();
                    mvars.Masterdb.OpenDB(oldDBPath);
                }
            } catch {}

            Close(); //Close the dialog
        }

        //CloseMergeDBs - Closes all the database connections made when adding databases to the merge list, if any
        private void CloseMergeDBs()
        {
            foreach (SSDataBase ssdb in mvars.mergeDBs) {
                try {
                    ssdb.CloseDB();
                } catch {} //Database is already closed
            }
        }

        //SmallMerge - Merges the selected databases with the master, only affects the CurrentInventory
        //table and only adds new items.  This function is only for distributed adds, which means the databases should
        //all be copies of the original master (as opposed to different databases).
        private void SmallMerge()
        {
            //For each database, go through the inventory and 
            foreach (SSDataBase mergedb in mvars.mergeDBs) {
                mvars.Masterdb.SmallMerge(mergedb);
            }
        }
    }

    //Vars that the pages in the merge dialog need to have access to
    public class MergeVars
    {
        public SSDataBase Masterdb;
        public string Masterdbname;
        public string Masterdblocation;
        public string Masterdbdate;
        public string defaultDBPath;
        public List<SSDataBase> mergeDBs;
    }
}
