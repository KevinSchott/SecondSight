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

namespace SecondSight.Extended_Controls
{
    public partial class ReportFilterItem : UserControl
    {
        private BindingList<KeyValuePair<string, string>> bs_TypeSelections;
        private BindingList<KeyValuePair<string, string>> bs_GenderSelections;
        private BindingList<KeyValuePair<string, string>> bs_SizeSelections;
        private BindingList<KeyValuePair<string, string>> bs_TintSelections;

        public ReportFilterItem()
        {
            InitializeComponent();

            bs_TypeSelections = new BindingList<KeyValuePair<string, string>>();
            bs_GenderSelections = new BindingList<KeyValuePair<string, string>>();
            bs_SizeSelections = new BindingList<KeyValuePair<string, string>>();
            bs_TintSelections = new BindingList<KeyValuePair<string, string>>();

            bs_TypeSelections.Add(new KeyValuePair<string, string>("S", "Single"));
            bs_TypeSelections.Add(new KeyValuePair<string, string>("M", "Multi"));

            bs_GenderSelections.Add(new KeyValuePair<string, string>("U", "Unisex"));
            bs_GenderSelections.Add(new KeyValuePair<string, string>("M", "Male"));
            bs_GenderSelections.Add(new KeyValuePair<string, string>("F", "Female"));

            bs_SizeSelections.Add(new KeyValuePair<string, string>("S", "Small"));
            bs_SizeSelections.Add(new KeyValuePair<string, string>("M", "Medium"));
            bs_SizeSelections.Add(new KeyValuePair<string, string>("L", "Large"));

            bs_TintSelections.Add(new KeyValuePair<string, string>("N", "None"));
            bs_TintSelections.Add(new KeyValuePair<string, string>("L", "Light"));
            bs_TintSelections.Add(new KeyValuePair<string, string>("D", "Dark"));
        }

        /// <summary>
        /// Click event for the Add linklabel
        /// </summary>
        private void btn_Add_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ((ReportFilter)(Parent.Parent)).AddItem();
//            Parent.Controls.Add(new ReportFilterItem());
        }

        /// <summary>
        /// Click event for the Remove linklabel
        /// </summary>
        private void btn_Remove_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ((ReportFilter)(Parent.Parent)).RemoveItem(this);
        }

        private void cb_FilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Change visibility and position of certain controls based on the Combo Box's selected index
            //Also populate the Selections combo box if type/gender/size/tint
            if (cb_FilterBy.SelectedIndex < 9) { //SKU and Rx fields
                lb_And.Location = new Point(tb_FilterA.Location.X + tb_FilterA.Width + 6, lb_And.Location.Y);
                dtp_FilterA.Visible = dtp_FilterB.Visible = lb_Equals.Visible = cb_Selections.Visible = false;
                lb_Between.Visible = tb_FilterA.Visible = lb_And.Visible = tb_FilterB.Visible = true;
            } else if (cb_FilterBy.SelectedIndex > 12) { //Date fields
                lb_And.Location = new Point(dtp_FilterA.Location.X + dtp_FilterA.Width + 6, lb_And.Location.Y);
                tb_FilterA.Visible = tb_FilterB.Visible = lb_Equals.Visible = cb_Selections.Visible = false;
                lb_Between.Visible = dtp_FilterA.Visible = lb_And.Visible = dtp_FilterB.Visible = true;
            } else { //Selection fields (type, gender, size, tint)
                PopulateSelections();
                lb_Between.Visible = dtp_FilterA.Visible = lb_And.Visible = dtp_FilterB.Visible = tb_FilterA.Visible = tb_FilterB.Visible = false;
                lb_Equals.Visible = cb_Selections.Visible = true;
            }
        }

        /// <summary>
        /// Populates the cb_Selections Combo Box depending on which value is selected in the FilterBy Combo Box
        /// </summary>
        private void PopulateSelections()
        {
            cb_Selections.ValueMember = "Key";
            cb_Selections.DisplayMember = "Value";
            switch (cb_FilterBy.SelectedIndex)
            {
                case 9:
                    cb_Selections.DataSource = bs_TypeSelections;
                    break;
                case 10:
                    cb_Selections.DataSource = bs_GenderSelections;
                    break;
                case 11:
                    cb_Selections.DataSource = bs_SizeSelections;
                    break;
                case 12:
                    cb_Selections.DataSource = bs_TintSelections;
                    break;
                default:
                    break;
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            ((ReportFilter)(Parent.Parent)).AddItem();
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            ((ReportFilter)(Parent.Parent)).RemoveItem(this);
        }
    }
}
