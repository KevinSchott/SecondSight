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
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using SecondSight.OptionsDialog;
using SecondSight.Merge;
using SecondSight.Excel;

namespace SecondSight
{
	partial class MainForm
	{
        //"File -> New Database" Menu item.
        private void menu_FileNewMenuItem_Click(object sender, EventArgs e)
        {
        	DataTable dt = new DataTable();
        	//Start the new database wizard
        	NewDBWizard twiz = new NewDBWizard(Mydb, GuiPrefs.DefaultDBDir);
        	if(twiz.ShowDialog() == DialogResult.OK) { //Database creation is handled inside the wizard
                UpdateAfterOpenDB();
        	}
        }

        //"File -> Open Database" Menu item.
        private void menu_FileOpenMenuItem_Click(object sender, EventArgs e)
        {
        	DataTable dt = new DataTable();
        	OpenFileDialog ofd = new OpenFileDialog();
        	ofd.InitialDirectory = GuiPrefs.DefaultDBDir;
        	ofd.Filter = "Second Sight Database Files (*.ssd, *.ssp)|*.ssd;*.ssp|All files (*.*)|*.*";
        	ofd.Title = "Load Existing Second Sight Database";
        	
        	if(ofd.ShowDialog() == DialogResult.OK) {
        		try {
        			Mydb.OpenDB(ofd.FileName);
        		} catch (Exception ex) {
        			MessageBox.Show(ex.Message, "Could not open database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        			return;
        		}

                UpdateAfterOpenDB(); //Update the controls and variables
        	}
        }
        

        ///<summary>
        /// Click event for the File -> Backup Database menu item.
        /// </summary>
        private void menu_FileBackupMenuItem_Click(object sender, EventArgs e)
        {
        	string tdate = "";
        	SaveFileDialog sfd = new SaveFileDialog();
        	sfd.InitialDirectory = GuiPrefs.DefaultBackupDir;
        	sfd.Filter = "Second Sight Database Files (*.ssd)|*.ssd|All files (*.*)|*.*";
        	sfd.Title = "Back up the currently open database.";
        	tdate = DateTime.Today.ToShortDateString();
        	tdate = tdate.Replace("/", "-");
        	sfd.FileName = String.Format("{1}({2})-{0}.ssd", 
        		tdate, GuiPrefs.OpenDBName, GuiPrefs.OpenDBLoc);

        	if (!Mydb.IsOpen()) { //If no database is loaded
        		MessageBox.Show("No database is currently loaded.  Please load a database " +
        		                "before attempting to back up the database.", 
        		                "No Loaded Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
        	} else {
        		if(sfd.ShowDialog() == DialogResult.OK) {
        			try {
        				File.Delete(sfd.FileName);
        				File.Copy(GuiPrefs.OpenDBPath, sfd.FileName);
                        MessageBox.Show("The database was successfully backed up.",
                                "Backup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			}
        			catch {
                        MessageBox.Show("The database could not be backed up.  Make sure you have permission to write to the destination folder.",
                                "Backup Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
        		}
        	}
        }

        /// <summary>
        /// Click event for the File->Restore menu item
        /// </summary>
        private void menu_FileRestoreMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = GuiPrefs.DefaultBackupDir;
            ofd.Filter = "Second Sight Database Files (*.ssd, *.ssb)|*.ssd;*.ssb|All files (*.*)|*.*";
            ofd.Title = "Restore the current database from a backup.";
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (!Mydb.IsOpen()) {
                MessageBox.Show("No database is currently loaded.  Please load a database " +
                                "before attempting to restore a database from a backup.",
                                "No Loaded Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else { //If no database is loaded
                if (ofd.ShowDialog() == DialogResult.OK) {
                    if (MessageBox.Show("The current database will be permanently replaced with the backup you selected.  Are you sure you want to continue?", 
                        "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes) {
                        string currentdbpath = GuiPrefs.OpenDBPath;
                        try {
                            
                            Mydb.CloseDB();
                            File.Copy(ofd.FileName, currentdbpath, true);
                            Mydb.OpenDB(currentdbpath);
                            MessageBox.Show("The database was successfully restored.",
                                "Restore Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } catch {
                            MessageBox.Show("The database could not be restored.  Make sure you have permission to write to the destination folder.",
                                "Restore Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } finally {
                            Mydb.OpenDB(currentdbpath); 
                            UpdateAfterOpenDB();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Click event for the File->Close Database menu item
        /// </summary>
        private void menu_FileCloseDatabaseMenuItem_Click(object sender, EventArgs e)
        {
        	Mydb.CloseDB();
        	GuiPrefs.OpenDBName = "";
        	GuiPrefs.OpenDBLoc = "";
        	GuiPrefs.OpenDBPath = "";
        	lb_Add_CurrentOpenDB.Text = "No Database Loaded";
        	lb_S_CurrentOpenDB.Text = "No Database Loaded";
        	lb_V_CurrentOpenDB.Text = "No Database Loaded";
        	lb_D_CurrentOpenDB.Text = "No Database Loaded";
            lb_R_CurrentOpenDB.Text = "No Database Loaded";

            Enable_AllControls(true);
        	Enable_MenuItems(false);
        	timer_AutoBackup.Stop();
        }
        
        //"File -> Exit"  Menu item.
        private void menu_FileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //"Tools -> Merge -> Merge Databases" menu item
        private void menu_ToolsMergeMergeDBMenuItem_Click(object sender, EventArgs e)
        {
            MergeDialog mdialog = new MergeDialog(GuiPrefs.DefaultDBDir, Mydb);
            mdialog.StartPosition = FormStartPosition.CenterParent;
            if(mdialog.ShowDialog() == DialogResult.OK) {
                if (!Mydb.IsOpen()) {
                    Mydb.OpenDB(Mydb.MyPath);
                }
                UpdateAfterOpenDB();
            }
        }

        //"Tools -> Merge -> Planner" menu item
        private void menu_ToolsMergePlannerMenuItem_Click(object sender, EventArgs e)
        {
            MergeCalculator mcalc = new MergeCalculator(Mydb, GuiPrefs.DefaultDBDir);
            mcalc.StartPosition = FormStartPosition.CenterParent;
            if (mcalc.ShowDialog() == DialogResult.OK) {
                
            }
        }

        /// <summary>
        /// "Tools -> Export -> Export Database" menu item.  Exports the contents of the entire database to an Excel spreadsheet (.xls)
        /// file.  This operation completely overwrites the chosen file.
        /// </summary>
        private void menu_ToolsExportDBMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            //Open the save dialog to get the path of the file to export the database to
            sfd.Title = "Export the current database to a Microsoft Excel spreadsheet.";
            sfd.InitialDirectory = GuiPrefs.DefaultDBDir;
            sfd.DefaultExt = "xls";
            sfd.Filter = "Microsoft Excel Spreadsheet Files (*.xls)|*.xls|All files (*.*)|*.*";
            sfd.FileName = String.Format("SecondSight Data-{0}({1}).xls",
                GuiPrefs.OpenDBName, GuiPrefs.OpenDBLoc);
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() == DialogResult.Cancel) {  //Exit function if cancelled
                return;
            }

            //Attempt the export.
            try {
                ExcelHelper.ExportFullDatabase(Mydb, sfd.FileName);
            } catch {
                MessageBox.Show("Database could not be exported.  Make sure you have permission to write to the destination folder and no other programs are using the destination file.",
                    "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Database succesfully exported to " + sfd.FileName, "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// "Tools -> Export -> Export Report" menu item.  Exports the contents of the last report to 
        /// an Excel spreadsheet (.xls) file
        /// </summary>
        private void menu_ToolsExportReportMenuItem_Click(object sender, EventArgs e)
        {
            if (R_LastPlainEnglishQuery == null) {
                MessageBox.Show("No report has been generated this session, so nothing can be exported.", "No repot to export",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();

            //Open the save dialog to get the path of the file to export the database to
            sfd.Title = "Export the current database to a Microsoft Excel spreadsheet.";
            sfd.InitialDirectory = GuiPrefs.DefaultDBDir;
            sfd.DefaultExt = "xls";
            sfd.Filter = "Microsoft Excel Spreadsheet Files (*.xls)|*.xls|All files (*.*)|*.*";
            sfd.FileName = String.Format("SecondSight Reports-{0}({1}).xls",
                GuiPrefs.OpenDBName, GuiPrefs.OpenDBLoc);
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = false;

            if (sfd.ShowDialog() == DialogResult.Cancel) {  //Exit function if cancelled
                return;
            }

            //Attempt the export.
            DataTable dt = (DataTable)(bs_R_FullLists.Count == 0 ? bs_R_Summaries.DataSource : bs_R_FullLists.DataSource);
            try {
                ExcelHelper.ExportReport(dt, R_LastGroupBy, R_LastPlainEnglishQuery, sfd.FileName);
            } catch {
                MessageBox.Show("Report could not be exported.  Make sure you have permission to write to the destination folder and no other programs are using the destination file.",
                    "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        
        //"Tools -> Options" menu item
        private void menu_ToolsOptionsItem_Click(object sender, EventArgs e)
        {
        	OptionsDialog.SSOptionsDialog ssod = new OptionsDialog.SSOptionsDialog(GuiPrefs);
            ssod.StartPosition = FormStartPosition.CenterParent;
        	if(ssod.ShowDialog() == DialogResult.OK) {
        		//OK was pressed, update the program with new prefs
        		GuiPrefs = ssod.GuiPrefs;
                ConfigureAutoBackupTimer();
        	}
        }
        
        //"Help -> About" menu item
        private void menu_HelpAboutMenuItem_Click(object sender, EventArgs e)
        {
        	AboutPage a = new AboutPage();
            a.StartPosition = FormStartPosition.CenterParent;
        	a.ShowDialog();
        }
        
        //"Help -> Help Topics" menu item
        private void menu_HelpHelptopicsMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly tasm = System.Reflection.Assembly.GetEntryAssembly();
            string installpath = System.IO.Path.GetDirectoryName(tasm.Location);
            string helppath = String.Format("{0}\\Documentation\\SecondSight Help.chm", installpath);
            if (File.Exists(helppath)) {
                Help.ShowHelp(this, helppath);
            } else {
                MessageBox.Show("SecondSight cannot find the help file.  Help could not be displayed.  For technical support, " +
                                "send an e-mail to secondsightsupport@gmail.com", "Help file not found.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        
        //"Help -> Support Website" menu itme
        private void menu_HelpSupportWebsiteMenuItem_Click(object sender, EventArgs e)
        {
        	System.Diagnostics.Process.Start("http://www.amigovision.org/");
        }
    }
} //namespace SecondSight