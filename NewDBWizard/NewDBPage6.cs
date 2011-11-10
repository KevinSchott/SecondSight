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
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace SecondSight
{
	public class NewDBPage6 : InteriorWizardPage
	{
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;
		private SSDataBase mydb;
		private DataTable dupedt;

		public NewDBPage6()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		
		public NewDBPage6(NewDBVars n, SSDataBase db)
		{
			ndbv = n;
			mydb = db;
			dupedt = new DataTable();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage6));
			this.label5 = new System.Windows.Forms.Label();
			this.rb_IgnoreDupes = new System.Windows.Forms.RadioButton();
			this.rb_MostRecent = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - Import Successful";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Creating the database and importing the REIMS data was successful.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(87, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(325, 48);
			this.label5.TabIndex = 23;
			this.label5.Text = "Data import was successful, but there are records with duplicate SKU numbers in t" +
			"he database.  What would you like to do with the duplicates?";
			// 
			// rb_IgnoreDupes
			// 
			this.rb_IgnoreDupes.Checked = true;
			this.rb_IgnoreDupes.Location = new System.Drawing.Point(143, 157);
			this.rb_IgnoreDupes.Name = "rb_IgnoreDupes";
			this.rb_IgnoreDupes.Size = new System.Drawing.Size(161, 19);
			this.rb_IgnoreDupes.TabIndex = 24;
			this.rb_IgnoreDupes.TabStop = true;
			this.rb_IgnoreDupes.Text = "Ignore Duplicate Records";
			this.rb_IgnoreDupes.UseVisualStyleBackColor = true;
			// 
			// rb_MostRecent
			// 
			this.rb_MostRecent.Location = new System.Drawing.Point(143, 182);
			this.rb_MostRecent.Name = "rb_MostRecent";
			this.rb_MostRecent.Size = new System.Drawing.Size(154, 19);
			this.rb_MostRecent.TabIndex = 25;
			this.rb_MostRecent.TabStop = true;
			this.rb_MostRecent.Text = "Use Most Recent Records";
			this.rb_MostRecent.UseVisualStyleBackColor = true;
			// 
			// NewDBPage6
			// 
			this.Controls.Add(this.rb_MostRecent);
			this.Controls.Add(this.rb_IgnoreDupes);
			this.Controls.Add(this.label5);
			this.Name = "NewDBPage6";
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			this.Controls.SetChildIndex(this.rb_IgnoreDupes, 0);
			this.Controls.SetChildIndex(this.rb_MostRecent, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
        }
		private System.Windows.Forms.RadioButton rb_IgnoreDupes;
		private System.Windows.Forms.RadioButton rb_MostRecent;
		private System.Windows.Forms.Label label5;
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
        	mydb.GetDuplicates(dupedt);
        	if(dupedt.Rows.Count == 0)
        	{
        		label5.Text = "Data import was successful.";
	        	rb_IgnoreDupes.Checked = true;
	        	rb_IgnoreDupes.Visible = false;
	        	rb_MostRecent.Visible = false;
            }
        	else
        	{
        		label5.Text = "Data import was successful, but there are records with duplicate " +
        			"SKU numbers in the database.  What would you like to do with the duplicates?";
        		rb_IgnoreDupes.Checked = true;
	        	rb_IgnoreDupes.Visible = true;
	        	rb_MostRecent.Visible = true;	
        	}
        	Wizard.SetWizardButtons( WizardButton.Next );
            return true;
        }
 
        protected internal override string OnWizardNext()
        {
        	//For each duplicate, figure out which one is more recent and replace if necessary
        	if(rb_MostRecent.Checked) 
        	{
        		DateTime cidate, didate; //Current inventory date and Duped inventory date
        		DataTable mydt = new DataTable();
        		int dupecount = dupedt.Rows.Count;
        		
        		for(int i = 0; i < dupecount; i++)
        		{
        			mydt.Clear();  //Clear the table for new results
        			try {
        				mydb.SKUSearch(Convert.ToInt16(dupedt.Rows[i][0]), mydt);
        			} catch {}
        			cidate = Convert.ToDateTime(mydt.Rows[0][13].ToString());
        			didate = Convert.ToDateTime(dupedt.Rows[i][13].ToString());
        			
        			//Compare the dates, if the duplicate is more recent, replace the record
        			//in CurrentInventory with the one from DupedInventory, otherwise do nothing
        			if(DateTime.Compare(didate, cidate) > 0)  //Duplicate is newer, replace
        			{
        				//Create the record
        				SpecsRecord sr = new SpecsRecord();
        				sr.SKU = Convert.ToUInt16(dupedt.Rows[i][0]);
        				sr.SphereOD = Convert.ToSingle(dupedt.Rows[i][1]);
        				sr.CylOD = Convert.ToSingle(dupedt.Rows[i][2]);
        				sr.AxisOD = Convert.ToInt16(dupedt.Rows[i][3]);
        				sr.AddOD = Convert.ToSingle(dupedt.Rows[i][4]);
        				sr.SphereOS = Convert.ToSingle(dupedt.Rows[i][5]);
        				sr.CylOS = Convert.ToSingle(dupedt.Rows[i][6]);
        				sr.AxisOS = Convert.ToInt16(dupedt.Rows[i][7]);
        				sr.AddOS = Convert.ToSingle(dupedt.Rows[i][8]);
        				sr.Type = dupedt.Rows[i][9].ToString();
        				sr.Gender = dupedt.Rows[i][10].ToString();
        				sr.Size = dupedt.Rows[i][11].ToString();
        				sr.Tint = dupedt.Rows[i][12].ToString();
        				sr.DateAdded = dupedt.Rows[i][13].ToString();
        				
        				//Delete the old record from the table and insert the new one
        				mydb.Delete(sr.SKU, SSTable.Current);
        				mydb.Insert(sr, SSTable.Current);
        			}
        		}
        	}
        	return "NewDBPage7";
        }
    }
}

