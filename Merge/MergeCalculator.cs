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
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using SMS.Windows.Forms;

namespace SecondSight.Merge
{
	/// <summary>
	/// Description of NewDBWizard.
	/// </summary>
	public partial class MergeCalculator : WizardForm
    {
        private Button btn_Help;
        protected internal MergeCalculatorVars mcvars;
        protected internal string defaultpath;
        protected internal SSDataBase currentdb;
        private ToolTip tt_ToolTip;
        private System.ComponentModel.IContainer components;
        protected internal SSDataBase masterdb;

        public MergeCalculatorVars MCVars
        {
            get {return mcvars;}
        }

        //Constructor
        public MergeCalculator(SSDataBase _ssdb, string _path)
        {
            InitializeComponent();

            currentdb = _ssdb;
            masterdb = _ssdb;

            defaultpath = _path;
            mcvars = new MergeCalculatorVars();

            Controls.Add(new MergeCalculatorPage1());
            Controls.Add(new MergeCalculatorPage2());
            Controls.Add(new MergeCalculatorPage3());
        }

        /// <summary>
        /// Click event for the Help button
        /// </summary>
        private void btn_Help_Click(object sender, EventArgs e)
        {
            
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_Help = new System.Windows.Forms.Button();
            this.tt_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btn_Help
            // 
            this.btn_Help.Location = new System.Drawing.Point(12, 327);
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.Size = new System.Drawing.Size(75, 23);
            this.btn_Help.TabIndex = 12;
            this.btn_Help.Text = "&Help";
            this.tt_ToolTip.SetToolTip(this.btn_Help, "Help - What this tool is and how to use it.");
            this.btn_Help.UseVisualStyleBackColor = true;
            // 
            // MergeCalculator
            // 
            this.ClientSize = new System.Drawing.Size(497, 360);
            this.Controls.Add(this.btn_Help);
            this.Name = "MergeCalculator";
            this.Text = "Group Data Entry Planner";
            this.Controls.SetChildIndex(this.btn_Help, 0);
            this.Controls.SetChildIndex(this.m_separator, 0);
            this.Controls.SetChildIndex(this.m_finishButton, 0);
            this.Controls.SetChildIndex(this.m_cancelButton, 0);
            this.Controls.SetChildIndex(this.m_nextButton, 0);
            this.Controls.SetChildIndex(this.m_backButton, 0);
            this.ResumeLayout(false);

        }
    }

    //Vars 
    public class MergeCalculatorVars
    {
        public List<int> MinSKU;
        public List<int> MaxSKU;
        public List<string> GroupLabel;

        public MergeCalculatorVars()
        {
            MinSKU = new List<int>();
            MaxSKU = new List<int>();
            GroupLabel = new List<string>();
        }

        //Clears the lists
        public void Clear()
        {
            MinSKU.Clear();
            MaxSKU.Clear();
            GroupLabel.Clear();
        }
    }

} //namespace SecondSight