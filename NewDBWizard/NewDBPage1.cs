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
	public class NewDBPage1 : ExteriorWizardPage
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;

		public NewDBPage1()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		
		public NewDBPage1(NewDBVars n)
		{
			InitializeComponent();
			ndbv = n;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_watermarkPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_titleLabel
            // 
            this.m_titleLabel.Text = "Create New Database";
            // 
            // m_watermarkPicture
            // 
            this.m_watermarkPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_watermarkPicture.Image")));
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(170, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 57);
            this.label1.TabIndex = 2;
            this.label1.Text = "This wizard guides you through the process of creating a new Second Sight databas" +
                "e.  You may either start from scratch with an empty database or import the conte" +
                "nts of an existing REIMS database.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(170, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(292, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Click Next to continue.";
            // 
            // NewDBPage1
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "NewDBPage1";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_watermarkPicture, 0);
            ((System.ComponentModel.ISupportInitialize)(this.m_watermarkPicture)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            // Enable only the Next button on the this page    
            Wizard.SetWizardButtons( WizardButton.Next );
            return true;
        }
    }
}

