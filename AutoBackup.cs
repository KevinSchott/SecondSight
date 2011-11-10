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

/// <summary>
/// This file handles the auto backup functions.
/// It includes the timer and op functions and the function that creates and organizes
/// the automatic backup files
/// </summary>

using System;
using System.IO;
using System.Windows.Forms;

namespace SecondSight
{
	partial class MainForm
	{
		private int NumberOps;						//Number of ops performed
		
		//Timer tick event
		private void timer_AutoBackup_Tick(object sender, EventArgs e)
		{
			PerformAutoBackup();
		}
		
		//Configure the auto backup timer if the pref is set
		private void ConfigureAutoBackupTimer()
		{
			timer_AutoBackup.Stop();
			if(GuiPrefs.AutoBackup && GuiPrefs.ABAfterTime && GuiPrefs.ABTime > 0) {
				timer_AutoBackup.Interval = GuiPrefs.ABTime * 60000; //Convert GuiPrefs.ABTime in minutes to miliseconds
				timer_AutoBackup.Start();
			}
		}
		
		//Increment ops by the 1, calls PerformAutoBackup when the
		//number of ops specified in GuiPrefs.ABOps is reached.  An op is 
		//a single add or dispense
		private void IncrementOps()
		{
			//Only do these things if auto backups are on and set to ops
			if(GuiPrefs.AutoBackup && GuiPrefs.ABAfterOps) { 
				if(++NumberOps >= GuiPrefs.ABOps) {
					PerformAutoBackup();
					NumberOps = 0;
				}
			}
		}
		
		//Same as IncrementOps() but increments by the number in param _inc
		private void IncrementOps(int _inc)
		{
			//Only do these things if auto backups are on and set to ops
			if(GuiPrefs.AutoBackup && GuiPrefs.ABAfterOps) { 
				if((NumberOps += _inc) >= GuiPrefs.ABOps) {
					PerformAutoBackup();
					NumberOps = 0;
				}
			}
		}
		
		private void ResetOps()
		{
			NumberOps = 0;
		}
		
		//Performs an automatic backup
		//If the number of existing automatic backups for the current database already equals the 
		//maximum number (specified by GuiPrefs.ABNumberKept), delete the oldest first
		//Uses an arbitrary naming scheme for the automatic backups based on the filename of the
		//currently open database
		private void PerformAutoBackup()
		{
			//If the file does not exist or there's something wrong with the file name, do not auto backup
			try {
				if(!File.Exists(GuiPrefs.OpenDBPath)) {
					return;
				}
			} catch { return; }
			
			//Check the default backup directory for existing auto backups for the currently open file
			string spattern = "*" + Path.GetFileName(GuiPrefs.OpenDBPath) + ".ssb";
			
			//Delete as many files as needed, starting from the oldest, until there are less than the maximum specified by GuiPrefs.ABNumberKept
			int abcount = Directory.GetFileSystemEntries(GuiPrefs.DefaultBackupDir, spattern).Length - GuiPrefs.ABNumberKept;
			while(abcount >= 0) {
				string[] existingabs = Directory.GetFileSystemEntries(GuiPrefs.DefaultBackupDir, spattern);
				int oldestindex = 0;
				for(int i = 1; i < existingabs.Length; i++) {
					if (File.GetCreationTime(existingabs[i]) < File.GetCreationTime(existingabs[oldestindex])) {
						oldestindex = i;
					}
				}
				File.Delete(existingabs[oldestindex]);
				abcount--;
			}
			
			//Create the auto backup by copying the currently open database
			//Filename format: CurrentDateTime-OriginalFilename.ssd.ssb
			File.Copy(GuiPrefs.OpenDBPath, String.Format("{0}\\{1}-{2}.ssb", GuiPrefs.DefaultBackupDir, DateTime.Now.Ticks, Path.GetFileName(GuiPrefs.OpenDBPath)));
		}
	}
}