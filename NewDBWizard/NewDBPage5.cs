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
	public class NewDBPage5 : InteriorWizardPage
	{
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;
		private SSDataBase mydb;

		public NewDBPage5()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		
		public NewDBPage5(NewDBVars n, SSDataBase db)
		{
			ndbv = n;
			mydb = db;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage5));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tb_NameConf = new System.Windows.Forms.TextBox();
			this.tb_LocConf = new System.Windows.Forms.TextBox();
			this.tb_NewFileConf = new System.Windows.Forms.TextBox();
			this.tb_REIMSPathConf = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - Confirm Import";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Confirm the information entered before creating the database.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(70, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 30);
			this.label2.TabIndex = 15;
			this.label2.Text = "Database Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(70, 182);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 30);
			this.label1.TabIndex = 16;
			this.label1.Text = "REIMS Database Path:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(70, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 30);
			this.label3.TabIndex = 17;
			this.label3.Text = "New Database File:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(20, 122);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(170, 30);
			this.label4.TabIndex = 18;
			this.label4.Text = "Clinic/Inventory Location:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tb_NameConf
			// 
			this.tb_NameConf.Location = new System.Drawing.Point(196, 89);
			this.tb_NameConf.Name = "tb_NameConf";
			this.tb_NameConf.ReadOnly = true;
			this.tb_NameConf.Size = new System.Drawing.Size(199, 20);
			this.tb_NameConf.TabIndex = 19;
			// 
			// tb_LocConf
			// 
			this.tb_LocConf.Location = new System.Drawing.Point(196, 119);
			this.tb_LocConf.Name = "tb_LocConf";
			this.tb_LocConf.ReadOnly = true;
			this.tb_LocConf.Size = new System.Drawing.Size(199, 20);
			this.tb_LocConf.TabIndex = 20;
			// 
			// tb_NewFileConf
			// 
			this.tb_NewFileConf.Location = new System.Drawing.Point(196, 149);
			this.tb_NewFileConf.Name = "tb_NewFileConf";
			this.tb_NewFileConf.ReadOnly = true;
			this.tb_NewFileConf.Size = new System.Drawing.Size(199, 20);
			this.tb_NewFileConf.TabIndex = 21;
			// 
			// tb_REIMSPathConf
			// 
			this.tb_REIMSPathConf.Location = new System.Drawing.Point(196, 179);
			this.tb_REIMSPathConf.Name = "tb_REIMSPathConf";
			this.tb_REIMSPathConf.ReadOnly = true;
			this.tb_REIMSPathConf.Size = new System.Drawing.Size(199, 20);
			this.tb_REIMSPathConf.TabIndex = 22;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(70, 212);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(325, 49);
			this.label5.TabIndex = 23;
			this.label5.Text = "If the above information is correct, click the \'Next\' button to create the databa" +
			"se and import the data from REIMS.  If not, click \'Back\' and correct the informa" +
			"tion.";
			// 
			// NewDBPage5
			// 
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tb_REIMSPathConf);
			this.Controls.Add(this.tb_NewFileConf);
			this.Controls.Add(this.tb_LocConf);
			this.Controls.Add(this.tb_NameConf);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Name = "NewDBPage5";
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.tb_NameConf, 0);
			this.Controls.SetChildIndex(this.tb_LocConf, 0);
			this.Controls.SetChildIndex(this.tb_NewFileConf, 0);
			this.Controls.SetChildIndex(this.tb_REIMSPathConf, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
        }
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tb_NameConf;
		private System.Windows.Forms.TextBox tb_REIMSPathConf;
		private System.Windows.Forms.TextBox tb_NewFileConf;
		private System.Windows.Forms.TextBox tb_LocConf;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
			
            tb_NameConf.Text = ndbv.Name;
            tb_LocConf.Text = ndbv.Location;
            tb_NewFileConf.Text = ndbv.Path;
            tb_REIMSPathConf.Text = ndbv.REIMSPath;
            
            //Enable both the Back and Next buttons on this page    
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next);
            
            return true;
        }
 
        protected internal override string OnWizardNext()
        {
        	//Change the mouse cursor to hourglass
            //Cursor = Cursors.WaitCursor;
            
            //This is a hack so that we won't create the file if the import
            //path is wrong and then when the user fixes the import path,
            //a file already exists exception is thrown.
            if(!File.Exists(ndbv.REIMSPath + "GLSKU.DBF") || !File.Exists(ndbv.REIMSPath + "DISPENSE.DBF"))
           	{
           		MessageBox.Show("One of the REIMS database files does not exist.  The data could not " +
               	                "be imported.", "Error: Import Failure",
            	                MessageBoxButtons.OK, MessageBoxIcon.Error);
               	return "NewDBPage4";
           	}
            
            //Create the database file
            try
            {
            	mydb.CreateNewDB(ndbv.Path, ndbv.Name, ndbv.Location);
            }
            catch (IOException e)
            {
            	MessageBox.Show(e.Message, "Error: Database File Exists", 
            	                MessageBoxButtons.OK, MessageBoxIcon.Error);
            	return "NewDBPage3";
            }
            
            //Import
            try
            {
            	mydb.ImportREIMS(ndbv.REIMSPath);
            }
            catch (Exception e)
            {
            	MessageBox.Show(e.Message, "Error: Import Failure",
            	                MessageBoxButtons.OK, MessageBoxIcon.Error);
            	return "NewDBPage3";
            }
            return "NewDBPage6";
        }
    }
}

