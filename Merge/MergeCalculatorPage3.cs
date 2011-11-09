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

using SMS.Windows.Forms;
using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;


namespace SecondSight.Merge
{
	public class MergeCalculatorPage3 : MergeCalculatorPage
	{
        private ListView lv_Groups;
        private ColumnHeader MinSKU;
        private ColumnHeader MaxSKU;
        private ColumnHeader GroupLabel;
        private Label label1;
        private CheckBox ch_OpenFolder;
        private Label lb_Instructions;

        private static string inst_header = "INSTRUCTIONS - ADDING ITEMS TO YOUR SECONDSIGHT DATABASE" + Environment.NewLine + Environment.NewLine +
            "Before you begin: These instructions assume that you have the SecondSight Database Inventory Management System program installed on your computer." +
            Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "This document is a set of instructions for adding items to a SecondSight partial database " +
            "in preparation for a merge with a larger master database.  This document should be accompanied " +
            "by a SecondSight partial database file (extension .ssp) " +
            "to which you can add glasses.  The step-by-step process is detailed below." + Environment.NewLine + Environment.NewLine;
        private static string inst_list = "2)  Run the SecondSight program." + Environment.NewLine +
            "3)  In the menu on the top-left, click File, then click Load Database." + Environment.NewLine +
            "4)  Find the SecondSight partial database file from step 1, select it and click Open." + Environment.NewLine +
            "5)  The program should display the name and location of the database at the top of the screen."  + Environment.NewLine +
            "    Verify that these are correct before proceeding." + Environment.NewLine +
            "6)  Near the bottom left of the program, there is a section labeled \"Limit SKU Range\"." + Environment.NewLine + 
            "    Verify that the text boxes filled in automatically with your assigned SKU minimum and maximum (detailed below)." + Environment.NewLine +
            "7)  Add your glasses to the database using the form on the left side of the program." + Environment.NewLine +
            "    Each item you add will be displayed on the list to the right." + Environment.NewLine + Environment.NewLine;

        private static string inst_footer = "After all the glasses have been added to the database, exit the SecondSight program by clicking on the File menu entry in the top-left " +
            "of the program, and clicking Exit.  The SecondSight partial database file is now ready to be sent to the person responsible for merging it with the master database.";

        public MergeCalculatorPage3()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Wizard Finish - Copies the database the requisite number of times
        /// and creates and populates the additional information table in each one
        /// that stores information about the assigned sku range.
        /// </summary>
        /// <returns>true</returns>
        protected internal override bool OnWizardFinish()
        {
            //Locate or create the folder where the copies will be stored
            string planpath = String.Format("{0}/MergePlan", Wizard.defaultpath);
            if (!Directory.Exists(planpath)) {
                if(Wizard.defaultpath != "") { //If the default is defined
                    try {
                        Directory.CreateDirectory(String.Format("{0}/MergePlan", Wizard.defaultpath));
                    } catch { //Can't create the directory for some reason, dump it in the same folder where the master database is
                        planpath = Path.GetDirectoryName(Wizard.masterdb.MyPath);
                    }
                } else { //Undefined default path, create the MergePlan folder where the current master database is located
                    try {
                        planpath = (Directory.CreateDirectory(String.Format("{0}/MergePlan", Path.GetDirectoryName(Wizard.masterdb.MyPath)))).FullName;
                    } catch { //Can't create the directory for some reason, dump it in the same folder where the master database is
                        planpath = Path.GetDirectoryName(Wizard.masterdb.MyPath);
                    }
                }
            }

            //Loop through each entry in mcvars (the MergeCalculatorVars stored in the parent)
            //and create and populate the data table with the additional merge plan info in it
            for (int i = 0; i < Wizard.mcvars.MinSKU.Count; i++) {
                string fullpath = String.Format("{0}/{{Merge Plan - {1}}} {2}.ssp", planpath, Wizard.mcvars.GroupLabel[i],
                                    Path.GetFileNameWithoutExtension(Wizard.masterdb.MyPath));

                //Step one - Create a copy of the master and name it descriptively
                try {
                    if (File.Exists(fullpath)) {
                        File.Delete(fullpath);    //Delete the old version of the merge plan database file
                    }
                    File.Copy(Wizard.masterdb.MyPath, fullpath);    
                } catch {
                    MessageBox.Show(String.Format("Unable to copy the database.  Please make sure write permissions are granted for the \"{0}\"folder.", planpath),
                        "Unable to Copy Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Step 2 - Open the database copy and create the table with the additional information
                SQLiteConnection dbcon = new SQLiteConnection();
                SQLiteCommand dbcmd = dbcon.CreateCommand();
                SQLiteParameter labelparam = new SQLiteParameter("@plabel");
                SQLiteParameter skuminparam = new SQLiteParameter("@pskumin");
                SQLiteParameter skumaxparam = new SQLiteParameter("@pskumax");

                dbcon.ConnectionString = new SQLiteConnectionStringBuilder(String.Format(@"Data Source={0}", fullpath)).ConnectionString;
                dbcmd.Parameters.Add(labelparam);
                dbcmd.Parameters.Add(skuminparam);
                dbcmd.Parameters.Add(skumaxparam);

                //Open the database
                try {
                    dbcon.Open();
                } catch {
                    MessageBox.Show(String.Format("One of the copies could not be properly configured.  Please make sure write permissions are granted for the \"{0}\"folder.", planpath),
                        "Unable to Configure Copies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Set up the transaction to create and populate the additional tables
                using (SQLiteTransaction dbt = dbcon.BeginTransaction())
                {
                    //Create the additional info table
                    dbcmd.CommandText = "CREATE TABLE MergeInfo(GroupLabel TEXT, MinSKU, MaxSKU)";
                    dbcmd.ExecuteNonQuery();

                    //Create the separate storage table
                    dbcmd.CommandText = "CREATE TABLE MergeItems(SKU INTEGER UNIQUE, SphereOD REAL, " +
                        "CylinderOD REAL, AxisOD INTEGER, AddOD REAL, SphereOS REAL, CylinderOS REAL, " +
                        "AxisOS INTEGER, AddOS REAL, Type TEXT, Gender TEXT, Size TEXT, " +
                        "Tint TEXT, DateAdded DATETIME, Comment TEXT)";
                    dbcmd.ExecuteNonQuery();

                    //Insert the info into the new table
                    dbcmd.CommandText = "INSERT INTO MergeInfo VALUES(@plabel, @pskumin, @pskumax)";
                    labelparam.Value = Wizard.mcvars.GroupLabel[i];
                    skuminparam.Value = Wizard.mcvars.MinSKU[i];
                    skumaxparam.Value = Wizard.mcvars.MaxSKU[i];
                    dbcmd.ExecuteNonQuery();

                    dbt.Commit();
                }

                dbcon.Close(); //Modification complete, close the database

                //Step 3 - Create the instruction file
                string readmepath = planpath + "/Instructions - " + Path.GetFileNameWithoutExtension(fullpath) + ".txt";
                try {
                    if (File.Exists(readmepath)) {
                        File.Delete(readmepath);
                    }
                } catch {
                    MessageBox.Show(String.Format("One of the copies could not be properly configured.  Please make sure write permissions are granted for the \"{0}\"folder.", planpath),
                        "Unable to Configure Copies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string merge_filename = Path.GetFileName(fullpath);
                //Create the file and write 
                using (StreamWriter outfile = new StreamWriter(readmepath))
                {
                    outfile.Write(inst_header);
                    outfile.Write(String.Format("1)  Save or copy the SecondSight database file named \"{0}\" to an easily accessible location on your hard drive or other storage device.{1}",
                        merge_filename, Environment.NewLine));
                    outfile.Write(inst_list);
                    outfile.Write("=== YOUR SKU RANGE ASSIGNMENT ===" + Environment.NewLine);
                    outfile.Write(String.Format("Min: {0}{2}Max: {1}{2}{2}", Wizard.mcvars.MinSKU[i], Wizard.mcvars.MaxSKU[i], Environment.NewLine));
                    outfile.Write(inst_footer);
                }
            }

            //Step 4 - Open the folder if the checkbox is checked
            if (ch_OpenFolder.Checked) {
                System.Diagnostics.Process.Start(planpath);
            }

            return true;
        }

        protected internal override string OnWizardBack()
        {
            Wizard.mcvars.Clear();
            lv_Groups.Items.Clear();
            return "MergeCalculatorPage2";
        }

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;

            //Populate the list view on this page with the calculated values from the previous page
            for(int i = 0; i < Wizard.mcvars.MinSKU.Count; i++) {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Wizard.mcvars.GroupLabel[i];
                lvi.SubItems.Add(Wizard.mcvars.MinSKU[i].ToString());
                lvi.SubItems.Add(Wizard.mcvars.MaxSKU[i].ToString());
                lv_Groups.Items.Add(lvi);
            }

            // Enable both the Finish and Back buttons on this page    
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Finish );
            return true;
        }

        private void InitializeComponent()
        {
            this.lb_Instructions = new System.Windows.Forms.Label();
            this.lv_Groups = new System.Windows.Forms.ListView();
            this.GroupLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MinSKU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MaxSKU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.ch_OpenFolder = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_titleLabel
            // 
            this.m_titleLabel.Text = "Group Planner - Results";
            // 
            // m_subtitleLabel
            // 
            this.m_subtitleLabel.Text = "Details of the plan are summarized on this page.";
            // 
            // lb_Instructions
            // 
            this.lb_Instructions.Location = new System.Drawing.Point(23, 63);
            this.lb_Instructions.Name = "lb_Instructions";
            this.lb_Instructions.Size = new System.Drawing.Size(449, 26);
            this.lb_Instructions.TabIndex = 5;
            this.lb_Instructions.Text = "Each group has been assigned its own range of SKUs to use when adding new items t" +
                "o the database.  Assignments are listed in the box below.";
            // 
            // lv_Groups
            // 
            this.lv_Groups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GroupLabel,
            this.MinSKU,
            this.MaxSKU});
            this.lv_Groups.FullRowSelect = true;
            this.lv_Groups.GridLines = true;
            this.lv_Groups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Groups.Location = new System.Drawing.Point(23, 94);
            this.lv_Groups.Name = "lv_Groups";
            this.lv_Groups.Size = new System.Drawing.Size(449, 142);
            this.lv_Groups.TabIndex = 6;
            this.lv_Groups.UseCompatibleStateImageBehavior = false;
            this.lv_Groups.View = System.Windows.Forms.View.Details;
            // 
            // GroupLabel
            // 
            this.GroupLabel.Text = "Label";
            this.GroupLabel.Width = 270;
            // 
            // MinSKU
            // 
            this.MinSKU.Text = "Minimum SKU";
            this.MinSKU.Width = 88;
            // 
            // MaxSKU
            // 
            this.MaxSKU.Text = "Maximum SKU";
            this.MaxSKU.Width = 87;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(23, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 41);
            this.label1.TabIndex = 7;
            this.label1.Text = "Copies of the master database will now be created and named based on the above in" +
                "formaiton, along with an instructional text file for each group.  Distribute the" +
                "se files to their corrseponding groups.";
            // 
            // ch_OpenFolder
            // 
            this.ch_OpenFolder.AutoSize = true;
            this.ch_OpenFolder.Checked = true;
            this.ch_OpenFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ch_OpenFolder.Location = new System.Drawing.Point(216, 283);
            this.ch_OpenFolder.Name = "ch_OpenFolder";
            this.ch_OpenFolder.Size = new System.Drawing.Size(256, 17);
            this.ch_OpenFolder.TabIndex = 8;
            this.ch_OpenFolder.Text = "Open folder location of the copies when finished.";
            this.ch_OpenFolder.UseVisualStyleBackColor = true;
            // 
            // MergeCalculatorPage3
            // 
            this.Controls.Add(this.ch_OpenFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_Instructions);
            this.Controls.Add(this.lv_Groups);
            this.Name = "MergeCalculatorPage3";
            this.Controls.SetChildIndex(this.lv_Groups, 0);
            this.Controls.SetChildIndex(this.lb_Instructions, 0);
            this.Controls.SetChildIndex(this.m_headerPanel, 0);
            this.Controls.SetChildIndex(this.m_headerSeparator, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
            this.Controls.SetChildIndex(this.m_headerPicture, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ch_OpenFolder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
} //namespace SecondSight.Merge