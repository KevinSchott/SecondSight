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

namespace SecondSight.Extended_Controls
{
    partial class ReportFilter
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
            this.flop_Filters = new System.Windows.Forms.FlowLayoutPanel();
            this.llb_AddFilter = new System.Windows.Forms.LinkLabel();
            this.flop_Filters.SuspendLayout();
            this.SuspendLayout();
            // 
            // flop_Filters
            // 
            this.flop_Filters.AutoSize = true;
            this.flop_Filters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flop_Filters.Controls.Add(this.llb_AddFilter);
            this.flop_Filters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flop_Filters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flop_Filters.Location = new System.Drawing.Point(0, 3);
            this.flop_Filters.Name = "flop_Filters";
            this.flop_Filters.Size = new System.Drawing.Size(57, 13);
            this.flop_Filters.TabIndex = 0;
            // 
            // llb_AddFilter
            // 
            this.llb_AddFilter.AutoSize = true;
            this.llb_AddFilter.Location = new System.Drawing.Point(3, 0);
            this.llb_AddFilter.Name = "llb_AddFilter";
            this.llb_AddFilter.Size = new System.Drawing.Size(51, 13);
            this.llb_AddFilter.TabIndex = 0;
            this.llb_AddFilter.TabStop = true;
            this.llb_AddFilter.Text = "Add Filter";
            this.llb_AddFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_AddFilter_Click);
            // 
            // ReportFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flop_Filters);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ReportFilter";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(57, 16);
            this.flop_Filters.ResumeLayout(false);
            this.flop_Filters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flop_Filters;
        public System.Windows.Forms.LinkLabel llb_AddFilter;

    }
}
