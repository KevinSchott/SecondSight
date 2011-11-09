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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace SecondSight.Merge
{
	public class MergeCalculatorPage : InteriorWizardPage
	{
        public MergeCalculatorPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="SecondSight.Merge.MergeCalculator"/>MergeCalculator</see>
        /// to which this <see cref="SecondSight.Merge.MergeCalculatorPage"/>MergeCalculatorPage</see>
        /// belongs.
        /// </summary>
        new protected MergeCalculator Wizard
        {
            get
            {
                return (MergeCalculator)Parent;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeCalculatorPage));
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_headerPicture
            // 
            this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
            // 
            // MergeCalculatorPage
            // 
            this.Name = "MergeCalculatorPage";
            ((System.ComponentModel.ISupportInitialize)(this.m_headerPicture)).EndInit();
            this.ResumeLayout(false);

        }
    }
} //namespace SecondSight.Merge