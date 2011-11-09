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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SecondSight.Merge
{
    public partial class MergePage1 : MergePage
    {
        public MergePage1()
        {
            InitializeComponent();
        }

        public MergePage1(MergeVars _mvars)
        {
            InitializeComponent();
            mvars = _mvars;
            if(mvars.Masterdbname.Length > 0) {
                lb_CurrentMaster.Text = "Master Database: " + mvars.Masterdbname + " (" + mvars.Masterdblocation + ")";
            } else {
                lb_CurrentMaster.Text = "No master database currently loaded.";
            }
        }

        //Click event for Select Master button
        //Opens an open file dialog and switches the currently selected master database
        //after verifying the selection is valid.
        private void btn_SelectMaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = mvars.defaultDBPath;
            ofd.Title = "Select Master Database";
            ofd.Filter = "SecondSight Databases (*.ssd)|*.ssd|All Files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.DefaultExt = ".ssd";

            //Process the open file dialog
            if (ofd.ShowDialog() == DialogResult.OK) {
                SSDataBase ssdbt = new SSDataBase();
                try {
                    ssdbt.OpenDB(ofd.FileName);
                } catch { //Not a valid secondsight database
                    MessageBox.Show("The selected file is not a valid SecondSight database and cannot be used as the master database for a merge.",
                        "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                //Get the info for the new database
                string tname = mvars.Masterdbname;
                string tloc = mvars.Masterdblocation;
                string tdate = mvars.Masterdbdate;
                DataTable dt = new DataTable();
                try {
                    ssdbt.GetTable(dt, SSTable.DBInfo);
                    mvars.Masterdbname = Convert.ToString(dt.Rows[0][0]);
                    mvars.Masterdblocation = Convert.ToString(dt.Rows[0][1]);
                    mvars.Masterdbdate = String.Format("{0:MM/dd/yyyy}", dt.Rows[0][2]);
                } catch {
                    MessageBox.Show("The selected file is not a valid SecondSight database and cannot be used as the master database for a merge.",
                        "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    mvars.Masterdbname = tname;
                    mvars.Masterdblocation = tloc;
                    mvars.Masterdbdate = tdate;
                    return;
                }

                //Close the previously selected master database, switch the pointer to the new one and
                //update the label on the control
                if(mvars.Masterdb.IsOpen()) {
                    try {
                        mvars.Masterdb.CloseDB();
                    } catch {}
                }
                mvars.Masterdb = ssdbt;
                lb_CurrentMaster.Text = "Master Database: " + mvars.Masterdbname + " (" + mvars.Masterdblocation + ")";
            }
        }

        //Click event for Add Databases button
        //Opens an OpenFileDialog for user to browse for merge-able database files,
        //verifies them, and adds them to the list
        private void btn_AddDatabases_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = mvars.defaultDBPath;
            ofd.Title = "Select Databases to Merge";
            ofd.Filter = "SecondSight Partial Databases (*.ssp)|*.ssp|All Files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            ofd.DefaultExt = ".ssp";

            //Process selected files if user didn't cancel
            if( ofd.ShowDialog() == DialogResult.OK ) {
                string errmsg = ""; //Holds composite error message
                Cursor.Current = Cursors.WaitCursor;

                //For each file selected, open the database, grab the DB info, check DB info against
                //master DB info and exclude if they don't match (display error message after batch processing)
                //check for mergeDB-specific info (the label and the assigned min/max SKUs) and include it in the
                //list box entry if available
                foreach (string fname in ofd.FileNames) {
                    SSDataBase ssdb = new SSDataBase();
                    DataTable dt = new DataTable();
                    string[] info = new string[3];
                    
                    if (fname != mvars.Masterdb.MyPath) {
                        try {
                            ssdb.OpenDB(fname);
                            ssdb.GetTable(dt, SSTable.DBInfo);
                            info[0] = dt.Rows[0][0].ToString();
                            info[1] = dt.Rows[0][1].ToString();
                            info[2] = String.Format("{0:MM/dd/yyyy}", dt.Rows[0][2]);
                        } catch { //An exception will be thrown if something
                            info[0] = info[1] = info[2] = "Unknown";
                        }
                      
                        //If the database info doesn't match, compile an error message, 
                        //otherwise include the info in the listbox and mvars
                        if( (info[0] != mvars.Masterdbname) ||
                            (info[1] != mvars.Masterdblocation) ||
                            (info[2] != mvars.Masterdbdate)) {
                            errmsg += System.IO.Path.GetFileName(fname) + ": " + info[0] + " (" + info[1] + "), Created on " + info[2] + "\n";
                        } else {
                            mvars.mergeDBs.Add(ssdb);
                            string ts = info[0] + " (" + info[1] + ")"; //Compile the listbox item string
                            try {
                                dt.Reset();
                                ssdb.GetTable(dt, SSTable.MergeInfo);
                                ts += " - For " + dt.Rows[0][0].ToString() + " - SKU Assignment: " + Convert.ToInt16(dt.Rows[0][1]) + 
                                    " to " + Convert.ToInt16(dt.Rows[0][2]);
                            }
                            catch {
                                ts += " - No additional information";
                            }

                            lbox_DatabasesToMerge.Items.Add(ts);
                        }
                        try {ssdb.CloseDB();} catch{} //Attempt to close the current merge database
                    }
                }

                Cursor.Current = Cursors.Default;

                //Display the error message if any databases were excluded
                if(errmsg.Length > 0) {
                    errmsg = "The following files you selected are either not valid SecondSight database files or are " +
                        "SecondSight databases that do not match the master database.\n\n" + errmsg + "\n\nThese files " +
                        "will not be included in the merge.";
                    MessageBox.Show(errmsg, "Some Files Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
                //else {
                //    MessageBox.Show("The selected databases were successfully merged into the selected master.", "Merge Successful",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
        }

        //Click event for the Remove Selected button
        //Removes the selected entry from the list and closes the corresponding entry in mvars
        private void btn_RemoveSelected_Click(object sender, EventArgs e)
        {
            if (lbox_DatabasesToMerge.SelectedIndex >= 0) {
                mvars.mergeDBs[lbox_DatabasesToMerge.SelectedIndex].CloseDB();
                mvars.mergeDBs.RemoveAt(lbox_DatabasesToMerge.SelectedIndex);
                lbox_DatabasesToMerge.Items.RemoveAt(lbox_DatabasesToMerge.SelectedIndex);
            }
        }
    }
}
