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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecondSight.Merge
{
	public class MergeCalculatorPage2 : MergeCalculatorPage
    {
        private Label lb_Instructions;
        private Button btn_AddGroup;
        private ListView lv_Groups;
        private ColumnHeader GroupLabel;
        private ColumnHeader NumberToAdd;
        private Label lb_CurrentMaster;
        private Button btn_SelectMaster;
        private Button btn_RemoveSelected;
        private TextBox tb_MinSKU;
        private Label lb_MinSKU;
        private ToolTip tt_ToolTip;
        private IContainer components;

        private System.Data.DataTable dbinfotable;

        //Constructor
        public MergeCalculatorPage2()
        {
            InitializeComponent();
            dbinfotable = new System.Data.DataTable();
            ToolTip tt1 = new ToolTip();
            ToolTip tt2 = new ToolTip();
            tt1.SetToolTip(btn_AddGroup, "Add a group to the plan.");
            tt2.SetToolTip(btn_RemoveSelected, "Remove the selected group from the plan.");
        }

        /// <summary>
        /// On Wizard Next
        /// Does all the calculating of SKU ranges based on the groups specified in the list view on this page
        /// Populates a set of variables for use by the next pages
        /// </summary>
        /// <returns>Name of the page to activate.</returns>
        protected internal override string OnWizardNext()
        {
            //Find sku range based on number of glasses to add
            if(Wizard.masterdb.IsOpen()) {
                Wizard.masterdb.GetCurrentInventory(); //Make sure the inventory is current
            } else {
                MessageBox.Show("You must select a master database to proceed.", "No Matser Database Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return "MergeCalculatorPage2";
            }

            //Calculate the assigned SKU range for each entry
            for(int i = 0; i < lv_Groups.Items.Count; i++) {
                CalculateSKURange(i);
            }

            if(lv_Groups.Items.Count > 0) {
                return "MergeCalculatorPage3";
            } else {
                MessageBox.Show("You must add at least one group to the plan to proceed.", "No Groups",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return "MergeCalculatorPage2";
            }
        }

        protected internal override string OnWizardBack()
        {
            return "MergeCalculatorPage1";
        }

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            try {   //Attempt to populate the master database label
                Wizard.currentdb.GetTable(dbinfotable, SSTable.DBInfo);
                lb_CurrentMaster.Text = "Master Database: " + dbinfotable.Rows[0][0].ToString() + 
                    " (" + dbinfotable.Rows[0][1].ToString() + ")";
            } catch {
                lb_CurrentMaster.Text = "No master database currently loaded.";
            }

            // Enable both the Next and Back buttons on this page    
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next );
            return true;
        }

        /// <summary>
        /// Click event for the Select Master button
        /// </summary>
        private void btn_SelectMaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Wizard.defaultpath;
            ofd.Title = "Select Master Database";
            ofd.Filter = "SecondSight Databases (*.ssd)|*.ssd|All Files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.DefaultExt = ".ssd";

            //Process the open file dialog
            if (ofd.ShowDialog() == DialogResult.OK) {
                SSDataBase ssdbt = new SSDataBase();

                //If the file name is the same as the "current" database's filename, that means the user wants to select the
                //database currently open in the main program.  Don't attempt to open that one a second time.  Otherwise
                //attempt to open the selected database
                if (ofd.FileName == Wizard.currentdb.MyPath) {
                    Wizard.masterdb = Wizard.currentdb; //Set the master to the currently open db
                } else {
                    try { //Attempt to open the selected database
                        ssdbt.OpenDB(ofd.FileName);
                    } catch { //Not a valid secondsight database
                        MessageBox.Show("The selected file is not a valid SecondSight database and cannot be used as the master database for a merge.",
                            "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    Wizard.masterdb = ssdbt; //Set the master to the newly selected db
                }

                //Get the info for the database
                string tname;
                string tloc;
                System.Data.DataTable dt = new System.Data.DataTable();
                try {
                    Wizard.masterdb.GetTable(dt, SSTable.DBInfo);
                    tname = Convert.ToString(dt.Rows[0][0]);
                    tloc = Convert.ToString(dt.Rows[0][1]);
                } catch {
                    MessageBox.Show("The selected file is not a valid SecondSight database and cannot be used as the master database for a merge.",
                        "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                lb_CurrentMaster.Text = "Master Database: " + tname + " (" + tloc + ")";

            }
        }

        /// <summary>
        /// Click event for the Add Group button
        /// </summary>
        private void btn_AddGroup_Click(object sender, EventArgs e)
        {
            string label = String.Format("Group {0}", lv_Groups.Items.Count + 1);

            MCAddGroupDialog agd = new MCAddGroupDialog();
            agd.StartPosition = FormStartPosition.CenterParent;
            if(agd.ShowDialog() == DialogResult.OK) {
                ListViewItem tlvi = new ListViewItem();
                if (agd.ReturnLabel.Length > 0) {
                    foreach (ListViewItem lvi in lv_Groups.Items) {
                        if (lvi.Text == agd.ReturnLabel) {  //Duplicate label entered.  Display error message and exit.
                            MessageBox.Show(String.Format("An entry with the label \"{0}\" already exists.  Please enter a different label name.", agd.ReturnLabel),
                                "Label already exists.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    tlvi.Text = agd.ReturnLabel;
                } else {
                    tlvi.Text = String.Format("Group {0}", lv_Groups.Items.Count + 1);
                }

                tlvi.SubItems.Add(agd.ReturnNumToAdd.ToString());
                lv_Groups.Items.Add(tlvi);
            }
        }

        /// <summary>
        /// Click event for the Remove Selected button
        /// </summary>
        private void btn_RemoveSelected_Click(object sender, EventArgs e)
        {
            if(lv_Groups.SelectedIndices.Count > 0) {
                lv_Groups.Items.RemoveAt(lv_Groups.SelectedIndices[0]);
            }
        }


        /// <summary>
        /// GetSKURange - Utility function for this page that calculates the appropriate range of SKUs to use
        /// for the entry specified by the _index parameter
        /// </summary>
        /// <param name="_index">The index of the ListView entry</param>
        private void CalculateSKURange(int _index)
        {
            int numrecords = Wizard.masterdb.InvResults.Rows.Count; //Number of total records in inventory
            int minsku; //Minimum sku to start with for this entry
            int currentindex = 0; //Index of the entry corresponding to the minimum sku;
            int count = 0;  //Counts the free SKUs
            int currentsku = 1; //Current SKU
            int expectedsku = 1; //Expected SKU - What the SKU would be if the list was complete (no unused SKUs)
            int numtarget = Convert.ToInt16(lv_Groups.Items[_index].SubItems[1].Text);  //Number of glasses that will be added by this group
            numtarget += (numtarget / 20) + 1;  //Add 5% for a buffer

            //Determine minimum sku
            if (Wizard.mcvars.MinSKU.Count > 0) {
                minsku = Wizard.mcvars.MaxSKU[_index - 1]; //Temporarily store the old maximum
            } else { //No old maximums, this is the first calculated set.  Take minimum from textbox or default
                try {
                    minsku = Convert.ToInt16(tb_MinSKU.Text) - 1;
                } catch {
                    minsku = 0;
                }

                if (minsku < 0) {
                    minsku = 0;
                }
            }

            //Find starting index for this range
            while (currentindex < numrecords && Convert.ToInt16(Wizard.masterdb.InvResults.Rows[currentindex][0]) <= minsku) {
                currentindex++;
            }

            minsku++;       //Initial minimum is one greater than the old maximum

            //Loop until we find the first free SKU starting with the initial minimum
            while (currentindex < numrecords && Convert.ToInt16(Wizard.masterdb.InvResults.Rows[currentindex][0]) == minsku) {
                minsku++;
                currentindex++;
            }

            currentsku = expectedsku = minsku;
            //Loop through starting at currentindex and count the free spaces until we hit our target or the end of the current inventory
            while (currentindex < numrecords && count < numtarget) {
                currentsku = Convert.ToInt16(Wizard.masterdb.InvResults.Rows[currentindex][0]);
                if (expectedsku < currentsku) { //This means there's a free SKU, increment the count
                    count++;
                } else { //expectedsku == currentsku, no hole, increment the list index
                    currentindex++;
                }
                expectedsku++;
            }

            //Add the new entry to the MergeCalculatorVars
            Wizard.mcvars.GroupLabel.Add(lv_Groups.Items[_index].Text);
            Wizard.mcvars.MinSKU.Add(minsku);

            //If we reached the end of the current inventory, the max sku is the currentsku plus what's left of the target number
            //Otherwise it's just the currentsku
            if (currentindex < Wizard.masterdb.InvResults.Rows.Count) {
                Wizard.mcvars.MaxSKU.Add(currentsku);
            } else {
                Wizard.mcvars.MaxSKU.Add(currentsku + (numtarget - count));
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lb_Instructions = new System.Windows.Forms.Label();
            this.btn_AddGroup = new System.Windows.Forms.Button();
            this.btn_RemoveSelected = new System.Windows.Forms.Button();
            this.lv_Groups = new System.Windows.Forms.ListView();
            this.GroupLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NumberToAdd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lb_CurrentMaster = new System.Windows.Forms.Label();
            this.btn_SelectMaster = new System.Windows.Forms.Button();
            this.tb_MinSKU = new System.Windows.Forms.TextBox();
            this.lb_MinSKU = new System.Windows.Forms.Label();
            this.tt_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_titleLabel
            // 
            this.m_titleLabel.Text = "Group Planner - Specify Groups";
            // 
            // m_subtitleLabel
            // 
            this.m_subtitleLabel.Text = "Specify each group and how many glasses they are responsible for adding on this p" +
                "age.";
            // 
            // lb_Instructions
            // 
            this.lb_Instructions.Location = new System.Drawing.Point(22, 117);
            this.lb_Instructions.Name = "lb_Instructions";
            this.lb_Instructions.Size = new System.Drawing.Size(450, 30);
            this.lb_Instructions.TabIndex = 7;
            this.lb_Instructions.Text = "For each group of glasses that will be added, click the \"Add Group\" button and fi" +
                "ll in the requested information. ";
            // 
            // btn_AddGroup
            // 
            this.btn_AddGroup.Location = new System.Drawing.Point(397, 283);
            this.btn_AddGroup.Name = "btn_AddGroup";
            this.btn_AddGroup.Size = new System.Drawing.Size(75, 23);
            this.btn_AddGroup.TabIndex = 8;
            this.btn_AddGroup.Text = "&Add Group";
            this.tt_ToolTip.SetToolTip(this.btn_AddGroup, "Add a group to the plan.");
            this.btn_AddGroup.UseVisualStyleBackColor = true;
            this.btn_AddGroup.Click += new System.EventHandler(this.btn_AddGroup_Click);
            // 
            // btn_RemoveSelected
            // 
            this.btn_RemoveSelected.Location = new System.Drawing.Point(264, 283);
            this.btn_RemoveSelected.Name = "btn_RemoveSelected";
            this.btn_RemoveSelected.Size = new System.Drawing.Size(104, 23);
            this.btn_RemoveSelected.TabIndex = 9;
            this.btn_RemoveSelected.Text = "&Remove Selected";
            this.tt_ToolTip.SetToolTip(this.btn_RemoveSelected, "Remove the selected group from the plan.");
            this.btn_RemoveSelected.UseVisualStyleBackColor = true;
            this.btn_RemoveSelected.Click += new System.EventHandler(this.btn_RemoveSelected_Click);
            // 
            // lv_Groups
            // 
            this.lv_Groups.AutoArrange = false;
            this.lv_Groups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GroupLabel,
            this.NumberToAdd});
            this.lv_Groups.FullRowSelect = true;
            this.lv_Groups.GridLines = true;
            this.lv_Groups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Groups.LabelWrap = false;
            this.lv_Groups.Location = new System.Drawing.Point(23, 150);
            this.lv_Groups.MultiSelect = false;
            this.lv_Groups.Name = "lv_Groups";
            this.lv_Groups.Size = new System.Drawing.Size(449, 127);
            this.lv_Groups.TabIndex = 10;
            this.lv_Groups.UseCompatibleStateImageBehavior = false;
            this.lv_Groups.View = System.Windows.Forms.View.Details;
            // 
            // GroupLabel
            // 
            this.GroupLabel.Text = "Label";
            this.GroupLabel.Width = 199;
            // 
            // NumberToAdd
            // 
            this.NumberToAdd.Text = "Number of Glasses to Add";
            this.NumberToAdd.Width = 246;
            // 
            // lb_CurrentMaster
            // 
            this.lb_CurrentMaster.AutoSize = true;
            this.lb_CurrentMaster.Location = new System.Drawing.Point(28, 92);
            this.lb_CurrentMaster.Name = "lb_CurrentMaster";
            this.lb_CurrentMaster.Size = new System.Drawing.Size(79, 13);
            this.lb_CurrentMaster.TabIndex = 12;
            this.lb_CurrentMaster.Text = "Current Master:";
            this.tt_ToolTip.SetToolTip(this.lb_CurrentMaster, "The selected master database.");
            // 
            // btn_SelectMaster
            // 
            this.btn_SelectMaster.Location = new System.Drawing.Point(25, 66);
            this.btn_SelectMaster.Name = "btn_SelectMaster";
            this.btn_SelectMaster.Size = new System.Drawing.Size(82, 23);
            this.btn_SelectMaster.TabIndex = 11;
            this.btn_SelectMaster.Text = "Select Master";
            this.tt_ToolTip.SetToolTip(this.btn_SelectMaster, "Select the master database that the plan will be based on.");
            this.btn_SelectMaster.UseVisualStyleBackColor = true;
            this.btn_SelectMaster.Click += new System.EventHandler(this.btn_SelectMaster_Click);
            // 
            // tb_MinSKU
            // 
            this.tb_MinSKU.Location = new System.Drawing.Point(415, 69);
            this.tb_MinSKU.Name = "tb_MinSKU";
            this.tb_MinSKU.Size = new System.Drawing.Size(57, 20);
            this.tb_MinSKU.TabIndex = 13;
            this.tt_ToolTip.SetToolTip(this.tb_MinSKU, "All assigned SKUs will be greater or equal to this value.");
            // 
            // lb_MinSKU
            // 
            this.lb_MinSKU.AutoSize = true;
            this.lb_MinSKU.Location = new System.Drawing.Point(282, 72);
            this.lb_MinSKU.Name = "lb_MinSKU";
            this.lb_MinSKU.Size = new System.Drawing.Size(127, 13);
            this.lb_MinSKU.TabIndex = 14;
            this.lb_MinSKU.Text = "(Optional) Start with SKU:";
            this.lb_MinSKU.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MergeCalculatorPage2
            // 
            this.Controls.Add(this.lb_MinSKU);
            this.Controls.Add(this.tb_MinSKU);
            this.Controls.Add(this.lb_CurrentMaster);
            this.Controls.Add(this.btn_SelectMaster);
            this.Controls.Add(this.lv_Groups);
            this.Controls.Add(this.btn_AddGroup);
            this.Controls.Add(this.btn_RemoveSelected);
            this.Controls.Add(this.lb_Instructions);
            this.Name = "MergeCalculatorPage2";
            this.Controls.SetChildIndex(this.lb_Instructions, 0);
            this.Controls.SetChildIndex(this.btn_RemoveSelected, 0);
            this.Controls.SetChildIndex(this.btn_AddGroup, 0);
            this.Controls.SetChildIndex(this.m_headerPanel, 0);
            this.Controls.SetChildIndex(this.m_headerSeparator, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
            this.Controls.SetChildIndex(this.m_headerPicture, 0);
            this.Controls.SetChildIndex(this.lv_Groups, 0);
            this.Controls.SetChildIndex(this.btn_SelectMaster, 0);
            this.Controls.SetChildIndex(this.lb_CurrentMaster, 0);
            this.Controls.SetChildIndex(this.tb_MinSKU, 0);
            this.Controls.SetChildIndex(this.lb_MinSKU, 0);
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}