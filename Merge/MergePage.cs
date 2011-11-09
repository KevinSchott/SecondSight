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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SecondSight.Merge
{
    public partial class MergePage : UserControl
    {
        protected MergeVars mvars;
        //protected List<SSDataBase> mergeDBs;
        //protected string defaultDBPath;

        public MergePage()
        {
            InitializeComponent();
        }

        public MergePage(MergeVars _mvars)
//        public MergePage(List<SSDataBase> _mergeDBs, string _defaultDBPath)
        {
            mvars = _mvars;
            //mergeDBs = _mergeDBs;
            //defaultDBPath = _defaultDBPath;
        }
    }
}
