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
	public class NewDBPage7 : InteriorWizardPage
	{
		private System.ComponentModel.IContainer components = null;
		private NewDBVars ndbv;

		public NewDBPage7()
		{
			
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		
		public NewDBPage7(NewDBVars n)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDBPage7));
			this.label5 = new System.Windows.Forms.Label();
			this.tb_FilenameConf = new System.Windows.Forms.TextBox();
			this.tb_LocConf = new System.Windows.Forms.TextBox();
			this.tb_NameConf = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Text = "Create New Database - Finished";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Text = "Database creation was successful.";
			// 
			// m_headerPicture
			// 
			this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(87, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(361, 48);
			this.label5.TabIndex = 23;
			this.label5.Text = "A new database was successfully created with the following information:";
			// 
			// tb_FilenameConf
			// 
			this.tb_FilenameConf.Location = new System.Drawing.Point(196, 186);
			this.tb_FilenameConf.Name = "tb_FilenameConf";
			this.tb_FilenameConf.ReadOnly = true;
			this.tb_FilenameConf.Size = new System.Drawing.Size(199, 20);
			this.tb_FilenameConf.TabIndex = 29;
			// 
			// tb_LocConf
			// 
			this.tb_LocConf.Location = new System.Drawing.Point(196, 156);
			this.tb_LocConf.Name = "tb_LocConf";
			this.tb_LocConf.ReadOnly = true;
			this.tb_LocConf.Size = new System.Drawing.Size(199, 20);
			this.tb_LocConf.TabIndex = 28;
			// 
			// tb_NameConf
			// 
			this.tb_NameConf.Location = new System.Drawing.Point(196, 130);
			this.tb_NameConf.Name = "tb_NameConf";
			this.tb_NameConf.ReadOnly = true;
			this.tb_NameConf.Size = new System.Drawing.Size(199, 20);
			this.tb_NameConf.TabIndex = 27;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(20, 159);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(170, 30);
			this.label4.TabIndex = 26;
			this.label4.Text = "Clinic/Inventory Location:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(70, 189);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 30);
			this.label3.TabIndex = 25;
			this.label3.Text = "Database Filename:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(70, 133);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 30);
			this.label2.TabIndex = 24;
			this.label2.Text = "Database Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(87, 241);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(343, 48);
			this.label1.TabIndex = 30;
			this.label1.Text = "You may now use this database for eyeglass inventory management.";
			// 
			// NewDBPage7
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tb_FilenameConf);
			this.Controls.Add(this.tb_LocConf);
			this.Controls.Add(this.tb_NameConf);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label5);
			this.Name = "NewDBPage7";
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.tb_NameConf, 0);
			this.Controls.SetChildIndex(this.tb_LocConf, 0);
			this.Controls.SetChildIndex(this.tb_FilenameConf, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
        }
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tb_FilenameConf;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tb_NameConf;
		private System.Windows.Forms.TextBox tb_LocConf;
		private System.Windows.Forms.Label label5;
		#endregion

        protected internal override bool OnSetActive()
        {
            if( !base.OnSetActive() )
                return false;
            
            tb_NameConf.Text = ndbv.Name;
            tb_LocConf.Text = ndbv.Location;
            tb_FilenameConf.Text = ndbv.Path;
            tb_NameConf.DeselectAll();
            Wizard.SetWizardButtons( WizardButton.Finish );
            return true;
        }
    }
}
