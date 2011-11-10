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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace SecondSight.OptionsDialog
{
	/// <summary>
	/// Description of SSOptionsDatabase.
	/// </summary>
	public partial class SSOptionsPage : UserControl
	{		
		private string DEFAULTDBDIR;
		private string DEFAULTBACKUPDIR;
		private const int DEFAULTTIME = 5;  //Default time interval for auto backups in minutes
		private const int DEFAULTOPS = 20;  //Default number of ops between auto backups
		private SSPrefs guiprefs;           //Where the prefs are stored

        public SSOptionsPage()
        {
            InitializeComponent();
        }
		
		/// <summary>
        /// Activate - Sets the controls to reflect the current options in the prefs file
		/// </summary>
		protected internal void Activate()
		{
            guiprefs = ((SSOptionsDialog)Parent).GuiPrefs;

//			System.Reflection.Assembly tasm = System.Reflection.Assembly.GetEntryAssembly();
            DEFAULTDBDIR = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\SecondSight Databases"; //String.Format("{0}\\Databases", System.IO.Path.GetDirectoryName(tasm.Location));
			DEFAULTBACKUPDIR = String.Format("{0}\\Backups", DEFAULTDBDIR);
			
			//Default folders (where the program looks first)
			tb_DBFolder.Text = guiprefs.DefaultDBDir;
			tb_BackupsFolder.Text = guiprefs.DefaultBackupDir;
			
            //Startup prefs
            //Open most recent database? (default is true)
            chb_OpenMostRecent.Checked = guiprefs.OpenMostRecentDB;

			//Automatic backup prefs
			//Perform automatic backups? (default is true)
			chb_AutoBackup.Checked = guiprefs.AutoBackup;
			
			//Auto backup after a certain amount of time? (Default is false)
			chb_AutoBackupTime.Checked = guiprefs.ABAfterTime;
			
			//Amount of time between auto backup in minutes
			if(guiprefs.ABTime > 0) {
				tb_AutoBackupTime.Text = guiprefs.ABTime.ToString();
			} else {
				tb_AutoBackupTime.Text = DEFAULTTIME.ToString();
			}
			
			//Auto-backup after a certain number of ops? (default is true)
			chb_AutoBackupOps.Checked = guiprefs.ABAfterOps;
			
			//Number of ops between each auto-backup
			if(guiprefs.ABOps > 0) {
				tb_AutoBackupOps.Text = guiprefs.ABOps.ToString();
			} else {
				tb_AutoBackupOps.Text = DEFAULTOPS.ToString();
			}
			
			//Number of auto-backup files to keep (X most recent)
			tb_AutoBackupNumberKept.Text = guiprefs.ABNumberKept.ToString();
		}
		
        /// <summary>
        /// Compile the prefs on this page into a SSPrefs object and return it
        /// </summary>
        /// <returns>The SSPrefs object that contains the prefs from this page</returns>
		public SSPrefs Apply()
		{
			SSPrefs tprefs = new SSPrefs();
			
			//Validate Database folder textbox and include it if valid
			try {
				tb_DBFolder.Text = System.IO.Path.GetFullPath(tb_DBFolder.Text);
			} catch (Exception) {
				MessageBox.Show("Database folder: The specified value is not a valid folder.",
				               "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw new ArgumentException();
			}
			
			//Prompt for and perform path creation if folder does not exist
			if(!System.IO.Directory.Exists(tb_DBFolder.Text)) {
				if(MessageBox.Show("Database folder: The specified folder does not exist.  Create it?", 
				                   "Create New Folder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
				                   == DialogResult.Yes) {
					CreateFolder(tb_DBFolder.Text);
				} else {
					tb_DBFolder.Focus();
					throw new System.IO.DirectoryNotFoundException();
				}
			}
			tprefs.DefaultDBDir = tb_DBFolder.Text;
			
			//Validate Backups folder textbox and include it if valid
			try {
				tb_BackupsFolder.Text = System.IO.Path.GetFullPath(tb_BackupsFolder.Text);
			} catch (Exception) {
				MessageBox.Show("Backups folder: The specified value is not a valid folder.",
				               "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb_BackupsFolder.Focus();
				throw new ArgumentException();
			}
			
			//Prompt for and perform path creation if folder does not exist
			if(!System.IO.Directory.Exists(tb_BackupsFolder.Text)) {
				if(MessageBox.Show("Backups folder: The specified folder does not exist.  Create it?", 
				                   "Create New Folder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
				                   == DialogResult.Yes) {
					CreateFolder(tb_BackupsFolder.Text);
				} else {
					tb_BackupsFolder.Focus();
					throw new System.IO.DirectoryNotFoundException();
				}
			}
			tprefs.DefaultBackupDir = tb_BackupsFolder.Text;
			
            //Startup prefs (only the one for now)
            tprefs.OpenMostRecentDB = chb_OpenMostRecent.Checked;

			//Automatic backups options
			//Perform backups?
			tprefs.AutoBackup = chb_AutoBackup.Checked;
			
			//Backup after a time interval?
			if((tprefs.ABAfterTime = chb_AutoBackupTime.Checked) == true) {
			
				try { //Validate specified time
					tprefs.ABTime = Math.Abs(Convert.ToInt16(tb_AutoBackupTime.Text));
					if(tprefs.ABTime <= 0) {
						throw new ArgumentException();
					}
				} catch {
					MessageBox.Show("Auto backup time must be a whole number greater than 0.", "Invalid Value",
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
					tb_AutoBackupTime.Focus();
					throw new ArgumentException();
				}
			} else {
				tprefs.ABTime = DEFAULTTIME;
			}
			
			//Backup after a number of operations?
			if((tprefs.ABAfterOps = chb_AutoBackupOps.Checked) == true) {
				try { //Validate specified number of operations
					tprefs.ABOps = Math.Abs(Convert.ToInt16(tb_AutoBackupOps.Text));
					if(tprefs.ABOps <= 0) {
						throw new ArgumentException();
					}
				} catch {
					MessageBox.Show("Number of operations must be a whole number greater than 0.", "Invalid Value",
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
					tb_AutoBackupOps.Focus();
					throw new ArgumentException();
				}
			} else {
                tprefs.ABOps = DEFAULTOPS;
			}
			
			//Number of automatic backup files to keep
			try {
				tprefs.ABNumberKept = Math.Abs(Convert.ToInt16(tb_AutoBackupNumberKept.Text));
				if(tprefs.ABNumberKept <= 0) {
					throw new ArgumentException();
				}
			} catch {
				MessageBox.Show("Number of automatic backup files to keep must be a whole number greater than 0.", "Invalid Value",
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
				tb_AutoBackupNumberKept.Focus();
				throw new ArgumentException();
			}

			return tprefs;
		}
		
		//Default DB folder button click event
		private void btn_DBFolderDefault_Click(object sender, EventArgs e)
		{
			tb_DBFolder.Text = DEFAULTDBDIR;
		}
		
		//DB folder Browse button click event
		private void btn_DBFolderBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Choose the default folder to store SecondSight Databases";
			if(fbd.ShowDialog() == DialogResult.OK) {
				CreateFolder(fbd.SelectedPath);
				tb_DBFolder.Text = fbd.SelectedPath;
			}
		}
		
		//Default Backups folder button click event
		private void btn_BackupsFolderDefault_Click(object sender, EventArgs e)
		{
			tb_BackupsFolder.Text = DEFAULTBACKUPDIR;
		}
		
		//Backups folder Browse button click event
		private void btn_BackupsFolderBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Choose the default folder to store SecondSight Database backups";
			if(fbd.ShowDialog() == DialogResult.OK) {
				CreateFolder(fbd.SelectedPath);
				tb_BackupsFolder.Text = fbd.SelectedPath;
			}
		}

        /// <summary>
        /// Creates all the folders necessary to make _path a valid path
        /// </summary>
        /// <param name="_path">Path to the folder to be created</param>
        private void CreateFolder(string _path)
        {
            if (!Directory.GetParent(_path).Exists) {
                CreateFolder(Directory.GetParent(_path).FullName);
            }
            if (!System.IO.Directory.Exists(_path)) {
                System.IO.Directory.CreateDirectory(_path);
            }
        }
	}
}
