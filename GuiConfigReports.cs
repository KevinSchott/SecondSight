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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
	
namespace SecondSight
{
    public partial class MainForm
    {
    	//Constants
    	//DEBUG -Make sure to change these numbers
    	private const int RLIMITS_H_INITOFFSET = 50;  //First row vertical offset
    	private const int RLIMITS_H_HEIGHT = 30;  //Height of each row
    	private const int RREPORT_H_OFFSET = 100; //Amount to subtract from the form Height in addition to the height of gb_R_ConfigureReport
    	private static readonly int [] RLIMITS_H_OFFSET = new int[]
    		{4, 0, 4, 1, 4, 1, 4, 4, 0};
    	private static readonly int [] RLIMITS_W_OFFSET = new int[]
    		{10, 70, 151, 200, 272, 300, 370, 450, 200}; //Horizontal location
    	private static readonly int [] RLIMITS_W_WIDTH = new int[]
    		{60, 80, 49, 70, 28, 70, 80, 70, 70};  //Width of each control
    	
    	//What each option in the filter comboboxes translates to (all of these are database fields)
    	private static readonly string [] RLIMITS_FILTERSTRING = new string[]
    		{"SphereOD BETWEEN ", "CylinderOD BETWEEN ", "AxisOD BETWEEN ", "AddOD BETWEEN ", 
    		"SphereOS BETWEEN ", "CylinderOS BETWEEN ", "AxisOS BETWEEN ", "AddOS BETWEEN ", 
    		"Type=", "Gender=", "Size=", "Tint=", "DateAdded BETWEEN ", "DateDispensed BETWEEN "};

        private BindingSource bs_R_FullLists;  //BindingSource connecting the full list reports dgv and the result set
        private BindingSource bs_R_Summaries; //BindingSource connecting the summary reports dgv and the result set

        private string R_LastPlainEnglishQuery; //The last query string, as a plain english sentence (to identify the report when exported to excel)
        private string R_LastGroupBy;           //The Group By text for the last summary query
    }
}