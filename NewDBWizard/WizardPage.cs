//
// WizardPage.cs
//
// Copyright (C) 2002-2002 Steven M. Soloff (mailto:s_soloff@bellsouth.net)
// All rights reserved.
//

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
using System.Windows.Forms;
using System.IO;

namespace SMS.Windows.Forms
{
	/// <summary>
	/// Represents a single page within a wizard dialog.
	/// </summary>
	public class WizardPage : UserControl
	{
        // ==================================================================
        // Public Constructors
        // ==================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="SMS.Windows.Forms.WizardPage">WizardPage</see>
        /// class.
        /// </summary>
        public WizardPage()
		{
            // Required for Windows Form Designer support
            InitializeComponent();
		}


        // ==================================================================
        // Protected Properties
        // ==================================================================
        
        /// <summary>
        /// Gets the <see cref="SMS.Windows.Forms.WizardForm">WizardForm</see>
        /// to which this <see cref="SMS.Windows.Forms.WizardPage">WizardPage</see>
        /// belongs.
        /// </summary>
        protected WizardForm Wizard
        {
            get
            {
                // Return the parent WizardForm
                return (WizardForm)Parent;
            }
        }
        
        
        // ==================================================================
        // Private Methods
        // ==================================================================
        
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Name = "WizardPage";
            Size = new System.Drawing.Size( 497, 313 );

        }
		#endregion


        // ==================================================================
        // Protected Internal Methods
        // ==================================================================
        
        /// <summary>
        /// Called when the page is no longer the active page.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the page was successfully deactivated; otherwise
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Override this method to perform special data validation tasks.
        /// </remarks>
        protected internal virtual bool OnKillActive()
        {
            // Deactivate if validation successful
            return Validate();
        }

        /// <summary>
        /// Called when the page becomes the active page.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the page was successfully set active; otherwise
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Override this method to performs tasks when a page is activated.
        /// Your override of this method should call the default version
        /// before any other processing is done.
        /// </remarks>
        protected internal virtual bool OnSetActive()
        {
            // Activate the page
            return true;
        }
        
        /// <summary>
        /// Called when the user clicks the Back button in a wizard.
        /// </summary>
        /// <returns>
        /// <c>WizardForm.DefaultPage</c> to automatically advance to the
        /// next page; <c>WizardForm.NoPageChange</c> to prevent the page
        /// changing.  To jump to a page other than the next one, return
        /// the <c>Name</c> of the page to be displayed.
        /// </returns>
        /// <remarks>
        /// Override this method to specify some action the user must take
        /// when the Back button is pressed.
        /// </remarks>
        protected internal virtual string OnWizardBack()
        {
            // Move to the default previous page in the wizard
            return WizardForm.NextPage;
        }
        
        /// <summary>
        /// Called when the user clicks the Finish button in a wizard.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the wizard finishes successfully; otherwise
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Override this method to specify some action the user must take
        /// when the Finish button is pressed.  Return <c>false</c> to
        /// prevent the wizard from finishing.
        /// </remarks>
        protected internal virtual bool OnWizardFinish()
        {
            // Finish the wizard
            return true;
        }
        
        /// <summary>
        /// Called when the user clicks the Next button in a wizard.
        /// </summary>
        /// <returns>
        /// <c>WizardForm.DefaultPage</c> to automatically advance to the
        /// next page; <c>WizardForm.NoPageChange</c> to prevent the page
        /// changing.  To jump to a page other than the next one, return
        /// the <c>Name</c> of the page to be displayed.
        /// </returns>
        /// <remarks>
        /// Override this method to specify some action the user must take
        /// when the Next button is pressed.
        /// </remarks>
        protected internal virtual string OnWizardNext()
        {
            // Move to the default next page in the wizard
            return WizardForm.NextPage;
        }
        
        public bool ValidatePath(string _path)
        {
        	string p;
			try {
				p = Path.GetFullPath(_path); //Check for invalid characters
			} catch (Exception) {
				return false;
			}
        	
        	if(Path.GetFileName(p) == "") { //Only directory specified
        		return false;
        	}
			
			return true;
        }
        
        public void CreateFolder(string _path)
		{
			if(!Directory.GetParent(_path).Exists)
			{
				CreateFolder(Directory.GetParent(_path).FullName);
			}
			if(!System.IO.Directory.Exists(_path))
			{
				System.IO.Directory.CreateDirectory(_path);
			}
		}
    }
}
