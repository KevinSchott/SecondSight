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

namespace SecondSight.Merge
{
    partial class MergePage1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbox_DatabasesToMerge = new System.Windows.Forms.ListBox();
            this.lb_Description = new System.Windows.Forms.Label();
            this.btn_SelectMaster = new System.Windows.Forms.Button();
            this.lb_CurrentMaster = new System.Windows.Forms.Label();
            this.lb_Instructions = new System.Windows.Forms.Label();
            this.btn_AddDatabases = new System.Windows.Forms.Button();
            this.btn_RemoveSelected = new System.Windows.Forms.Button();
            this.tt_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lbox_DatabasesToMerge
            // 
            this.lbox_DatabasesToMerge.FormattingEnabled = true;
            this.lbox_DatabasesToMerge.Location = new System.Drawing.Point(20, 134);
            this.lbox_DatabasesToMerge.Name = "lbox_DatabasesToMerge";
            this.lbox_DatabasesToMerge.Size = new System.Drawing.Size(387, 121);
            this.lbox_DatabasesToMerge.TabIndex = 0;
            // 
            // lb_Description
            // 
            this.lb_Description.Location = new System.Drawing.Point(3, 5);
            this.lb_Description.Name = "lb_Description";
            this.lb_Description.Size = new System.Drawing.Size(422, 30);
            this.lb_Description.TabIndex = 1;
            this.lb_Description.Text = "This tool allows you to merge additional SecondSight databases with the currently" +
                " loaded  master database.  Only items that have been added will be merged.";
            // 
            // btn_SelectMaster
            // 
            this.btn_SelectMaster.Location = new System.Drawing.Point(20, 44);
            this.btn_SelectMaster.Name = "btn_SelectMaster";
            this.btn_SelectMaster.Size = new System.Drawing.Size(82, 23);
            this.btn_SelectMaster.TabIndex = 2;
            this.btn_SelectMaster.Text = "Select Master";
            this.tt_ToolTip.SetToolTip(this.btn_SelectMaster, "Select the master database that will contain all the glasses merged in from the o" +
                    "ther databases.");
            this.btn_SelectMaster.UseVisualStyleBackColor = true;
            this.btn_SelectMaster.Click += new System.EventHandler(this.btn_SelectMaster_Click);
            // 
            // lb_CurrentMaster
            // 
            this.lb_CurrentMaster.AutoSize = true;
            this.lb_CurrentMaster.Location = new System.Drawing.Point(20, 70);
            this.lb_CurrentMaster.Name = "lb_CurrentMaster";
            this.lb_CurrentMaster.Size = new System.Drawing.Size(79, 13);
            this.lb_CurrentMaster.TabIndex = 3;
            this.lb_CurrentMaster.Text = "Current Master:";
            // 
            // lb_Instructions
            // 
            this.lb_Instructions.Location = new System.Drawing.Point(20, 102);
            this.lb_Instructions.Name = "lb_Instructions";
            this.lb_Instructions.Size = new System.Drawing.Size(272, 32);
            this.lb_Instructions.TabIndex = 4;
            this.lb_Instructions.Text = "Click the \"Add Database(s)\" button to browse for databases to merge with the curr" +
                "ent master.";
            // 
            // btn_AddDatabases
            // 
            this.btn_AddDatabases.Location = new System.Drawing.Point(313, 261);
            this.btn_AddDatabases.Name = "btn_AddDatabases";
            this.btn_AddDatabases.Size = new System.Drawing.Size(94, 23);
            this.btn_AddDatabases.TabIndex = 5;
            this.btn_AddDatabases.Text = "Add Database(s)";
            this.tt_ToolTip.SetToolTip(this.btn_AddDatabases, "Add one or more databases to the list.  These will be merged with the currently s" +
                    "elected master database.");
            this.btn_AddDatabases.UseVisualStyleBackColor = true;
            this.btn_AddDatabases.Click += new System.EventHandler(this.btn_AddDatabases_Click);
            // 
            // btn_RemoveSelected
            // 
            this.btn_RemoveSelected.Location = new System.Drawing.Point(20, 261);
            this.btn_RemoveSelected.Name = "btn_RemoveSelected";
            this.btn_RemoveSelected.Size = new System.Drawing.Size(101, 23);
            this.btn_RemoveSelected.TabIndex = 6;
            this.btn_RemoveSelected.Text = "Remove Selected";
            this.tt_ToolTip.SetToolTip(this.btn_RemoveSelected, "Remove the selected database from the list.");
            this.btn_RemoveSelected.UseVisualStyleBackColor = true;
            this.btn_RemoveSelected.Click += new System.EventHandler(this.btn_RemoveSelected_Click);
            // 
            // MergePage1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_RemoveSelected);
            this.Controls.Add(this.btn_AddDatabases);
            this.Controls.Add(this.lb_Instructions);
            this.Controls.Add(this.lb_CurrentMaster);
            this.Controls.Add(this.btn_SelectMaster);
            this.Controls.Add(this.lb_Description);
            this.Controls.Add(this.lbox_DatabasesToMerge);
            this.Name = "MergePage1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbox_DatabasesToMerge;
        private System.Windows.Forms.Label lb_Description;
        private System.Windows.Forms.Button btn_SelectMaster;
        private System.Windows.Forms.Label lb_CurrentMaster;
        private System.Windows.Forms.Label lb_Instructions;
        private System.Windows.Forms.Button btn_AddDatabases;
        private System.Windows.Forms.Button btn_RemoveSelected;
        private System.Windows.Forms.ToolTip tt_ToolTip;
    }
}
