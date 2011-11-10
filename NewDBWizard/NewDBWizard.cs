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
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using SMS.Windows.Forms;

namespace SecondSight
{
	/// <summary>
	/// Description of NewDBWizard.
	/// </summary>s
	public partial class NewDBWizard : WizardForm
	{
		private NewDBVars ndbv;  //Holds everything that the main program needs
		private SSDataBase mydb;
		
		public NewDBWizard(SSDataBase db, string dbpath)
		{
//			System.Reflection.Assembly tasm = System.Reflection.Assembly.GetEntryAssembly();
			ndbv = new NewDBVars();
			mydb = db;
            ndbv.DefaultPath = dbpath + "\\"; //System.IO.Path.GetDirectoryName(tasm.Location) + "\\Databases\\";
			ndbv.Path = ndbv.DefaultPath;
			InitializeComponent();
			
			Controls.AddRange(new Control[] {
			                  new NewDBPage1(),
			                  new NewDBPage2(),
			                  new NewDBPage3(ndbv),
			                  new NewDBPage4(ndbv),
			                  new NewDBPage5(ndbv, mydb),
			                  new NewDBPage6(ndbv, mydb),
			                  new NewDBPage7(ndbv),
			                  new NewDBPage8(ndbv),
			                  new NewDBPage9(ndbv, mydb)});
		}
	}
	
	//Container class for the variables that the calling class needs
	public class NewDBVars
	{
		public string DefaultPath;	//Default path
		public string Path;			//Path to the new database
		public string REIMSPath;	//Path to the old REIMS database
		public string Name;			//Name of the database
		public string Location; 	//Location of the database (geographically)
	}
}
