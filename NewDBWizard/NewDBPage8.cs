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
	public class NewDBPage8 : InteriorWizardPage
	{
        private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;

		public NewDBPage8()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		
		public NewDBPage8(NewDBVars n)
		{
			ndbv = n;
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage8));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tb_Name = new System.Windows.Forms.TextBox();
			this.tb_Location = new System.Windows.Forms.TextBox();
			this.tb_NewDBPath = new System.Windows.Forms.TextBox();
			this.btn_NewDBPathBrowse = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - Database Information";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Enter basic information about the database on this page.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(101, 113);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 38);
			this.label2.TabIndex = 6;
			this.label2.Text = "Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(101, 189);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 38);
			this.label1.TabIndex = 7;
			this.label1.Text = "Filename:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(101, 151);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 38);
			this.label3.TabIndex = 8;
			this.label3.Text = "Location:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tb_Name
			// 
			this.tb_Name.Location = new System.Drawing.Point(169, 110);
			this.tb_Name.Name = "tb_Name";
			this.tb_Name.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tb_Name.Size = new System.Drawing.Size(166, 20);
			this.tb_Name.TabIndex = 9;
			this.tb_Name.TextChanged += new System.EventHandler(this.Tb_NameTextChanged);
			// 
			// tb_Location
			// 
			this.tb_Location.Location = new System.Drawing.Point(169, 148);
			this.tb_Location.Name = "tb_Location";
			this.tb_Location.Size = new System.Drawing.Size(166, 20);
			this.tb_Location.TabIndex = 10;
			this.tb_Location.TextChanged += new System.EventHandler(this.Tb_LocationTextChanged);
			// 
			// tb_NewDBPath
			// 
			this.tb_NewDBPath.Location = new System.Drawing.Point(169, 186);
			this.tb_NewDBPath.Name = "tb_NewDBPath";
			this.tb_NewDBPath.Size = new System.Drawing.Size(166, 20);
			this.tb_NewDBPath.TabIndex = 11;
			this.tb_NewDBPath.TextChanged += new System.EventHandler(this.Tb_NewDBPathTextChanged);
			// 
			// btn_NewDBPathBrowse
			// 
			this.btn_NewDBPathBrowse.CausesValidation = false;
			this.btn_NewDBPathBrowse.Location = new System.Drawing.Point(355, 184);
			this.btn_NewDBPathBrowse.Name = "btn_NewDBPathBrowse";
			this.btn_NewDBPathBrowse.Size = new System.Drawing.Size(75, 23);
			this.btn_NewDBPathBrowse.TabIndex = 14;
			this.btn_NewDBPathBrowse.Text = "Browse";
			this.btn_NewDBPathBrowse.UseVisualStyleBackColor = true;
			this.btn_NewDBPathBrowse.Click += new System.EventHandler(this.Btn_NewDBPathBrowseClick);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(101, 236);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(329, 51);
			this.label4.TabIndex = 15;
			this.label4.Text = "The \'Name\' field is the name of the database.  The \'Location\' field can represent" +
			" either the location of the physical inventory, the clinic or both.";
			// 
			// NewDBPage8
			// 
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btn_NewDBPathBrowse);
			this.Controls.Add(this.tb_NewDBPath);
			this.Controls.Add(this.tb_Location);
			this.Controls.Add(this.tb_Name);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Name = "NewDBPage8";
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.tb_Name, 0);
			this.Controls.SetChildIndex(this.tb_Location, 0);
			this.Controls.SetChildIndex(this.tb_NewDBPath, 0);
			this.Controls.SetChildIndex(this.btn_NewDBPathBrowse, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
        }
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btn_NewDBPathBrowse;
		private System.Windows.Forms.TextBox tb_NewDBPath;
		private System.Windows.Forms.TextBox tb_Location;
		private System.Windows.Forms.TextBox tb_Name;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            this.tb_NewDBPath.Text = ndbv.Path;
			this.tb_NewDBPath.SelectionStart = tb_NewDBPath.TextLength;
			this.tb_NewDBPath.ScrollToCaret();
            
			// Enable the Next and Back buttons on this page depending on textbox values
            if(tb_Name.Text != "" && tb_Location.Text != "" && tb_NewDBPath.Text != "")
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
			else
				Wizard.SetWizardButtons(WizardButton.Back);
            return true;
        }
 
        //"Next" clicked, set the ndbv variables, verify values for 
        //all text boxes, and go to next page
        protected internal override string OnWizardNext()
        {
        	if(ValidatePath(tb_NewDBPath.Text) == true) {
        		if(Path.GetFileName(tb_NewDBPath.Text) == tb_NewDBPath.Text) { //If it's just a filename, add the default directory
        			tb_NewDBPath.Text = ndbv.DefaultPath + tb_NewDBPath.Text;
        		}
        		if(File.Exists(tb_NewDBPath.Text)) {  //If the file already exists, prompt for overwrite.
        			if(MessageBox.Show(String.Format("The file {0} already exists.  Do you wish to overwrite it?", Path.GetFileName(tb_NewDBPath.Text)),
        			                   "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes){
        				File.Delete(tb_NewDBPath.Text);
        			} else {
        				return "NewDBPage8";
        			}
        		} else if(!Directory.Exists(Path.GetDirectoryName(tb_NewDBPath.Text))) {
        			if(MessageBox.Show(String.Format("The directory {0} does not exist.  Create it?", Path.GetDirectoryName(tb_NewDBPath.Text)),
        			                   "Create New Directory?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes) {
        				CreateFolder(Path.GetDirectoryName(tb_NewDBPath.Text));
        			} else {
        				return "NewDBPage8";
        			}
        		}
	        	ndbv.Name = tb_Name.Text;
	        	ndbv.Location = tb_Location.Text;
	        	ndbv.Path = tb_NewDBPath.Text;
	        	return "NewDBPage9";
        	} else {
        		MessageBox.Show("Invalid filename, please specify a path and file with the extension .ssd",
        		               "Invalid Filename", MessageBoxButtons.OK, MessageBoxIcon.Error);
        		return "NewDBPage8";
        	}
        }
        
        protected internal override string OnWizardBack()
        {
        	return "NewDBPage2";
        }
		
        //Click event for the new file path dialog browse button
		void Btn_NewDBPathBrowseClick(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.InitialDirectory = ndbv.DefaultPath;
			sfd.OverwritePrompt = true;
			sfd.Title = "Specify New Database Filename";
			sfd.Filter = "Second Sight Database Files (*.ssd)|*.ssd|All files (*.*)|*.*";
			sfd.AddExtension = true;
			if(sfd.ShowDialog() == DialogResult.OK)
				tb_NewDBPath.Text = sfd.FileName;
		}
		
		void Tb_NameTextChanged(object sender, EventArgs e)
		{
			tb_NewDBPath.Text = ndbv.DefaultPath + tb_Name.Text + ".ssd";
			tb_NewDBPath.SelectionStart = tb_NewDBPath.TextLength;
			tb_NewDBPath.ScrollToCaret();
		}
		
		void Tb_LocationTextChanged(object sender, EventArgs e)
		{
			if(tb_Name.Text != "" && tb_Location.Text != "" && tb_NewDBPath.Text != "")
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
			else
				Wizard.SetWizardButtons(WizardButton.Back);
		}
		
		void Tb_NewDBPathTextChanged(object sender, EventArgs e)
		{
			if(tb_Name.Text != "" && tb_Location.Text != "" && tb_NewDBPath.Text != "")
				Wizard.SetWizardButtons(WizardButton.Back | WizardButton.Next);
			else
				Wizard.SetWizardButtons(WizardButton.Back);
		}
    }
}