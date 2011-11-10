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
using System.Text;
using System.Windows.Forms;

namespace SecondSight.Extended_Controls
{
    public partial class ReportFilter : UserControl
    {
        private BindingList<KeyValuePair<string, string> > bs_FilterSource;
        private List<ReportFilterItem> filteritems;

        public List<ReportFilterItem> FilterItems { get {return filteritems;} }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportFilter()
        {
            InitializeComponent();
            bs_FilterSource = new BindingList<KeyValuePair<string, string> >();
            filteritems = new List<ReportFilterItem>(6);

            bs_FilterSource.Add(new KeyValuePair<string, string>("SKU", "SKU"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OD Sphere", "SphereOD"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OD Cylinder", "CylinderOD"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OD Axis", "AxisOD"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OD Add", "AddOD"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OS Sphere", "SphereOS"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OS Cylinder", "CylinderOS"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OS Axis", "AxisOS"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("OS Add", "AddOS"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("Type", "Type"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("Gender", "Gender"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("Size", "Size"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("Tint", "Tint"));
            bs_FilterSource.Add(new KeyValuePair<string, string>("Date Added", "DateAdded"));
        }

        /// <summary>
        /// Alters the existing ReportFilterItem comboboxes depending on if we're seraching in Dispensed or Current inventory
        /// </summary>
        /// <param name="_dispensed">True if we want to set up for DispensedInventory, false for CurrentInventory</param>
        public void ConfigureComboBoxes(bool _dispensed)
        {
            if (_dispensed) { //Switching to dispensed inventory.  Add the Date Dispensed entry and refresh
                if (bs_FilterSource.Count == 15) {
                    return;
                } else {
                    bs_FilterSource.Add(new KeyValuePair<string,string>("Date Dispensed", "DateDispensed"));
                    foreach (ReportFilterItem rfi in filteritems) { //Loop through and refresh each ComboBox's list
                        int si = rfi.cb_FilterBy.SelectedIndex;
                        ConfigReportFilterItem(rfi);
                        rfi.cb_FilterBy.SelectedIndex = si; //Maintain previous selected index
                    }
                }
            } else { //Switching to current inventory.  Remove the Date Dispensed entry and refresh
                if (bs_FilterSource.Count == 14) {
                    return;
                } else {
                    bs_FilterSource.RemoveAt(14);
                    foreach (ReportFilterItem rfi in filteritems) { //Loop through and refresh each ComboBox's list
                        int si = rfi.cb_FilterBy.SelectedIndex < 14 ? rfi.cb_FilterBy.SelectedIndex : 0; //Ensure no index out of range exception
                        ConfigReportFilterItem(rfi);
                        rfi.cb_FilterBy.SelectedIndex = si; //Maintain previous selected value if appropriate
                    }
                }
            }
        }

        /// <summary>
        /// Removes all Reportfilteritems from the form and the List
        /// </summary>
        public void ClearItems()
        {
            foreach (ReportFilterItem rfi in filteritems) {
                flop_Filters.Controls.Remove(rfi);
            }
            filteritems.Clear();
            llb_AddFilter.Visible = true;
        }

        /// <summary>
        /// Formats and validates filter items.  If there is an invalid filter value (some value that cannot be converted into a number),
        /// this function will throw an exception.
        /// </summary>
        /// <exception cref="System.FormatException">Thrown when one of the filter text boxes is invalid</exception>
        public void ValidateItems()
        {
            foreach (ReportFilterItem rfi in filteritems) {
                if (rfi.cb_FilterBy.SelectedIndex < 9) { //Only format and validate fields that use text boxes for their values
                    if (rfi.cb_FilterBy.SelectedIndex == 0 || rfi.cb_FilterBy.SelectedIndex == 3 || rfi.cb_FilterBy.SelectedIndex == 7 ) { //SKU or Axis - validate for integer
                        int valA = 0;
                        int valB = 0;

                        try {
                            valA = Convert.ToInt16(rfi.tb_FilterA.Text);
                        } catch {
                            rfi.tb_FilterA.SelectAll();
                            rfi.tb_FilterA.Focus();
                            throw new FormatException("must be a whole number.");
                        }
                        
                        try {
                            valB = Convert.ToInt16(rfi.tb_FilterB.Text);
                        } catch {
                            rfi.tb_FilterB.SelectAll();
                            rfi.tb_FilterB.Focus();
                            throw new FormatException("must be a whole number.");
                        }

                        //Make sure the values are in the correct order for SQLite
                        if (valA > valB) {
                            rfi.tb_FilterA.Text = valB.ToString();
                            rfi.tb_FilterB.Text = valA.ToString();
                        } else {
                            rfi.tb_FilterA.Text = valA.ToString();
                            rfi.tb_FilterB.Text = valB.ToString();
                        }
                    } else { //Sphere, Cylinder or Add - validate for float
                        double valA = 0.0F;
                        double valB = 0.0F;
                        try {
                            valA = Convert.ToSingle(rfi.tb_FilterA.Text);
                        } catch {
                            rfi.tb_FilterA.SelectAll();
                            rfi.tb_FilterA.Focus();
                            throw new FormatException("must be a real number.");
                        }

                        try {
                            valB = Convert.ToSingle(rfi.tb_FilterB.Text);
                        } catch {
                            rfi.tb_FilterB.SelectAll();
                            rfi.tb_FilterB.Focus();
                            throw new FormatException("must be a real number.");
                        }

                        if (rfi.cb_FilterBy.SelectedIndex == 2 || rfi.cb_FilterBy.SelectedIndex == 6) { //Cylinder - enforce negative
                            valA = -Math.Abs(valA);
                            valB = -Math.Abs(valB);
                        }
                        
                        //Make sure the values are in the correct order for SQLite 
                        //(must have first number <= second number for the BETWEEN keyword in the query)
                        if(valA > valB) {
                            rfi.tb_FilterA.Text = valB.ToString();
                            rfi.tb_FilterB.Text = valA.ToString();
                        } else {
                            rfi.tb_FilterA.Text = valA.ToString();
                            rfi.tb_FilterB.Text = valB.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a single ReportFilterItem to the control
        /// </summary>
        protected internal void AddItem()
        {
            ReportFilterItem rfi = new ReportFilterItem();
            flop_Filters.Controls.Add(rfi);
            if (filteritems.Count > 0) {
                filteritems[filteritems.Count - 1].btn_Add.Visible = false;
            }
            filteritems.Add(rfi);
            ConfigReportFilterItem(rfi);
        }

        /// <summary>
        /// Removes a single ReportFilterItem from the control
        /// </summary>
        /// <param name="_rfi">The ReportFilterItem to remove</param>
        protected internal void RemoveItem(ReportFilterItem _rfi)
        {
            flop_Filters.Controls.Remove(_rfi);
            filteritems.Remove(_rfi);
            if (filteritems.Count == 0) {
                llb_AddFilter.Visible = true;
            } else {
                filteritems[filteritems.Count - 1].btn_Add.Visible = true;
            }
        }

        /// <summary>
        /// Click event for the initial Add Filter linklabel
        /// </summary>
        /// <remarks>Hides the initial Add Filter linklabel and adds the first new ReportFilterItem</remarks>
        private void llb_AddFilter_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddItem();
            llb_AddFilter.Visible = false;
        }

        /// <summary>
        /// Configures the report filter item.
        /// </summary>
        /// <remarks>Binds the control's DataSource to a copy of the BindingList and set the associated attributes.</remarks>
        /// <param name="_rfi">The ReportFilterItem to configure</param>
        private void ConfigReportFilterItem(ReportFilterItem _rfi)
        {
            _rfi.cb_FilterBy.DisplayMember = "Key";
            _rfi.cb_FilterBy.ValueMember = "Value";
            _rfi.cb_FilterBy.DataSource = new BindingList<KeyValuePair<string, string> >(bs_FilterSource);
        }
    }
}
