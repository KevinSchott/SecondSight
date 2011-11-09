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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SecondSight.OptionsDialog
{
    public partial class SSOptionsDialog : Form
    {
        private SSPrefs guiprefs;

        public SSPrefs GuiPrefs
        {
            get { return guiprefs; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_guiprefs">Where the current options are and where to store them on apply.</param>
        public SSOptionsDialog(SSPrefs _guiprefs)
        {
            InitializeComponent();
            guiprefs = _guiprefs;
            OptionsPageGeneral.Activate();
        }

        /// <summary>
        /// Click event for the OK button.  Applies any prefs changes.
        /// </summary>
        private void btn_OK_Click(Object sender, EventArgs e)
        {
            SSPrefs tprefs = new SSPrefs();
            try {
                tprefs = OptionsPageGeneral.Apply();
            } catch {
                return; //Do not close the dialog if there was a problem
			}

            guiprefs.OpenMostRecentDB = tprefs.OpenMostRecentDB;
            guiprefs.ABAfterOps = tprefs.ABAfterOps;
            guiprefs.ABAfterTime = tprefs.ABAfterTime;
            guiprefs.ABNumberKept = tprefs.ABNumberKept;
            guiprefs.ABOps = tprefs.ABOps;
            guiprefs.ABTime = tprefs.ABTime;
            guiprefs.AutoBackup = tprefs.AutoBackup;
            guiprefs.DefaultBackupDir = tprefs.DefaultBackupDir;
            guiprefs.DefaultDBDir = tprefs.DefaultDBDir;

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        private void btn_Cancel_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
