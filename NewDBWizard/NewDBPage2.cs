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


namespace SecondSight
{
	public class NewDBPage2 : InteriorWizardPage
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;

		public NewDBPage2()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		
		public NewDBPage2(NewDBVars n)
		{
			ndbv = n;
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage2));
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - Data Configuration";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Use this page to select the data configuration of the new database.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(41, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(412, 26);
			this.label1.TabIndex = 5;
			this.label1.Text = "Select whether to create a blank database or import the data from an existing REI" +
			"MS database.";
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(143, 157);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(152, 19);
			this.radioButton1.TabIndex = 7;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "&New Blank Database";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(143, 182);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(152, 19);
			this.radioButton2.TabIndex = 8;
			this.radioButton2.Text = "&Import from REIMS";
			// 
			// NewDBPage2
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.radioButton2);
			this.Name = "NewDBPage2";
			this.Controls.SetChildIndex(this.radioButton2, 0);
			this.Controls.SetChildIndex(this.radioButton1, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
        }
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            // Enable both the Next and Back buttons on this page    
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next );
            return true;
        }
 
        protected internal override string OnWizardNext()
        {
            return radioButton1.Checked ? "NewDBPage8" : "NewDBPage3";
        }
    }
}

