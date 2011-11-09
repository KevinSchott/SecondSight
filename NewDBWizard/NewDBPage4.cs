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


namespace SecondSight
{
	public class NewDBPage4 : InteriorWizardPage
	{
        private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;

		public NewDBPage4()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		
		public NewDBPage4(NewDBVars n)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage4));
			this.label1 = new System.Windows.Forms.Label();
			this.tb_REIMSPath = new System.Windows.Forms.TextBox();
			this.btn_REIMSPathBrowse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - REIMS Database";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Specify the location of the REIMS database to import.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(75, 189);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 38);
			this.label1.TabIndex = 7;
			this.label1.Text = "REIMS Location:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tb_REIMSPath
			// 
			this.tb_REIMSPath.Location = new System.Drawing.Point(169, 186);
			this.tb_REIMSPath.Name = "tb_REIMSPath";
			this.tb_REIMSPath.Size = new System.Drawing.Size(166, 20);
			this.tb_REIMSPath.TabIndex = 11;
			this.tb_REIMSPath.TextChanged += new System.EventHandler(this.Tb_REIMSPathTextChanged);
			// 
			// btn_REIMSPathBrowse
			// 
			this.btn_REIMSPathBrowse.CausesValidation = false;
			this.btn_REIMSPathBrowse.Location = new System.Drawing.Point(355, 184);
			this.btn_REIMSPathBrowse.Name = "btn_REIMSPathBrowse";
			this.btn_REIMSPathBrowse.Size = new System.Drawing.Size(75, 23);
			this.btn_REIMSPathBrowse.TabIndex = 14;
			this.btn_REIMSPathBrowse.Text = "Browse";
			this.btn_REIMSPathBrowse.UseVisualStyleBackColor = true;
			this.btn_REIMSPathBrowse.Click += new System.EventHandler(this.Btn_REIMSPathBrowseClick);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(75, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(355, 37);
			this.label2.TabIndex = 15;
			this.label2.Text = "Specify the location of the REIMS database files.  The specified directory must c" +
			"ontain at least the following two files: \n\n";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(75, 124);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 15);
			this.label3.TabIndex = 16;
			this.label3.Text = "GLSKU.DBF";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(75, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 17);
			this.label4.TabIndex = 17;
			this.label4.Text = "DISPENSE.DBF";
			// 
			// NewDBPage4
			// 
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btn_REIMSPathBrowse);
			this.Controls.Add(this.tb_REIMSPath);
			this.Controls.Add(this.label1);
			this.Name = "NewDBPage4";
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.tb_REIMSPath, 0);
			this.Controls.SetChildIndex(this.btn_REIMSPathBrowse, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
        }
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btn_REIMSPathBrowse;
		private System.Windows.Forms.TextBox tb_REIMSPath;
		private System.Windows.Forms.Label label2;
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            // Enable Back/Next buttons depending on textbox values
            if(tb_REIMSPath.Text != "")
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
			else
				Wizard.SetWizardButtons(WizardButton.Back);
            return true;
        }
 
        protected internal override string OnWizardNext()
        {
        	ndbv.REIMSPath = tb_REIMSPath.Text;
            return "NewDBPage5";
        }
        
        //Click event for the reims file path dialog browse button
		void Btn_REIMSPathBrowseClick(object sender, EventArgs e)
		{
			FolderBrowserDialog ofd = new FolderBrowserDialog();
//			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Description = "Specify REIMS Database Path";
			if(ofd.ShowDialog() == DialogResult.OK) {
				tb_REIMSPath.Text = ofd.SelectedPath + "\\";
			} else {
				tb_REIMSPath.Text = "";
			}
			
//			ofd.Title = "Specify REIMS Database Path";
//			ofd.Filter = "REIMS Database Files (*.dbf)|*.dbf|All files (*.*)|*.*";
//			if(ofd.ShowDialog() == DialogResult.OK)
//				tb_REIMSPath.Text = ofd.FileName.Substring(0, ofd.FileName.LastIndexOf("\\") + 1);
//			else
//				tb_REIMSPath.Text = "";
		}
		
		//Changed event for the reims file path text box
		void Tb_REIMSPathTextChanged(object sender, EventArgs e)
		{
			if(tb_REIMSPath.Text != "")
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
			else
				Wizard.SetWizardButtons(WizardButton.Back);
		}
    }
}