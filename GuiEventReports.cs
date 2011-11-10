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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text;
using SecondSight.Extended_Controls;
using System.Text.RegularExpressions;
	
namespace SecondSight
{
    public partial class MainForm
    {    	
    	/// <summary>
    	/// Changes displayed report options based on the currently selected report type.
    	/// For now just affects the top line of controls in the Configure Report group box.
    	/// </summary>
        private void cb_R_ReportType_Changed(object sender, EventArgs e)
    	{
    		//Display the "Group By" controls if Summaries is selected, otherwise hide them.
    		//Alter the description label as necessary.
    		if(cb_R_ReportType.SelectedIndex == 0){  //Full Lists selected
    			lb_R_GroupedBy.Visible = false;
    			cb_R_GroupBy.Visible = false;
    			lb_R_Description.Text = "Display lists of individual glasses from:";
    		} else { //Summaries selected
    			lb_R_GroupedBy.Visible = true;
    			cb_R_GroupBy.Visible = true;
    			lb_R_Description.Text = "Display summaries of glasses from:";
    		}
    	}

        /// <summary>
        /// Visibility changed handler - Used to keep track of visibility between groupbox
        /// collapses.  The Group By combobox will only be visible when the groupbox is 
        /// not collapsed AND the cb_R_ReportType selection is set to Summaries
        /// </summary>
        private void cb_R_ReportType_VisibilityChanged(object sender, EventArgs e)
        {
            cb_R_GroupBy.Visible = (cb_R_ReportType.SelectedIndex == 1 && !gb_R_ConfigureReport.IsCollapsed);
            lb_R_GroupedBy.Visible = cb_R_GroupBy.Visible;
        }
    	
    	/// <summary>
    	/// Alters the available options in any currently displayed filters, based on the 
    	/// currently selected data source.
        /// <remarks>The FilterBy comboboxes are configured throuhg the ReportFilter control.  The GroupBy combobox
        /// is changed directly.</remarks>
    	/// </summary>
        private void cb_R_ReportSource_Changed(object sender, EventArgs e)
    	{
    		if(cb_R_ReportSource.SelectedIndex == 0){ //Source is now Current Inventory
                repf_R_Filter.ConfigureComboBoxes(false);
                if (cb_R_GroupBy.SelectedIndex == 13) {
                    cb_R_GroupBy.SelectedIndex = 0;
                }
                cb_R_GroupBy.Items.Remove("Date Dispensed");
    		} else { //Source is now Dispensed Inventory
                repf_R_Filter.ConfigureComboBoxes(true);
                cb_R_GroupBy.Items.Add("Date Dispensed");
    		}
    	}
    	
    	/// <summary>
    	/// Click event for the Reset button in the Reports screen
    	/// Removes all currently active filters and resets the page to the default state
    	/// </summary>
    	private void btn_R_Reset_Click(object sender, EventArgs e)
    	{
            repf_R_Filter.ClearItems();
            cb_R_ReportSource.SelectedIndex = 0;
            cb_R_ReportType.SelectedIndex = 0;
            cb_R_GroupBy.SelectedIndex = 0;
    	}

        /// <summary>
        /// Click event for the Generate Report button.  Validates all input and then assembles the
        /// data structure that is passed into the SSDataBase.CustomQuery function
        /// </summary>
        private void btn_R_GenerateReport_Click(object sender, EventArgs e)
        {
            SSTable querytable;
            Dictionary<int, LinkedList<string>> filterparams = new Dictionary<int, LinkedList<string>>();

            if (!Mydb.IsOpen()) {
                MessageBox.Show("Please load a database before attempting to generate a report.", "No database loaded.",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (cb_R_ReportSource.SelectedIndex == 0) {
                querytable = SSTable.Current;
            } else {
                querytable = SSTable.Dispensed;
            }
            
            //Validate inputs where necessary
            try{
                repf_R_Filter.ValidateItems();
            } catch (FormatException fex) {
                MessageBox.Show("Invalid filter value.  Selected filter value " + fex.Message);
                return;
            }

            //Build and store the plain english version of the new query
            R_LastPlainEnglishQuery = BuildPlainEnglishQuery();

            //Loop through each filter, validate inputs as necessary, and add the parameters to the Dictionary
            foreach (ReportFilterItem rfi in repf_R_Filter.FilterItems) {
                //If the "filter by" parameter doesn't exist in the dictionary yet, add it
                if (!filterparams.ContainsKey(rfi.cb_FilterBy.SelectedIndex)) {
                    filterparams.Add(rfi.cb_FilterBy.SelectedIndex, new LinkedList<string>());
                }
                
                if (rfi.cb_FilterBy.SelectedIndex < 9) { //Rx fields - validate and add values from TextBoxes
                    filterparams[rfi.cb_FilterBy.SelectedIndex].AddLast(rfi.tb_FilterA.Text);
                    filterparams[rfi.cb_FilterBy.SelectedIndex].AddLast(rfi.tb_FilterB.Text);
                } else if (rfi.cb_FilterBy.SelectedIndex > 12) { //Date fields - add values from DateTimePickers
                    filterparams[rfi.cb_FilterBy.SelectedIndex].AddLast(rfi.dtp_FilterA.Value.Date.ToString("yyyy-MM-dd"));
                    filterparams[rfi.cb_FilterBy.SelectedIndex].AddLast(rfi.dtp_FilterB.Value.Date.ToString("yyyy-MM-dd"));
                } else { //Selection fields - type, gender, size, tint - add values from ComboBox
                    filterparams[rfi.cb_FilterBy.SelectedIndex].AddLast(rfi.cb_Selections.SelectedValue.ToString());
                }
            }

            DataTable resultstable = new DataTable();

            //Run the query
            resultstable = Mydb.ReportQuery(querytable, 
                cb_R_ReportType.SelectedIndex == 1, 
                cb_R_GroupBy.SelectedIndex + 1, filterparams);

            //Configure the results display
            if (cb_R_ReportType.SelectedIndex == 1) { //Summarries - configure results to display in small DGV
                DataTable sortedtable = new DataTable();
                sortedtable = SortSummaryResults(resultstable);
                bs_R_Summaries.DataSource = sortedtable;//resultstable;
                bs_R_FullLists.DataSource = null; //Null this so Export Report can figure out which data to export
                R_LastGroupBy = cb_R_GroupBy.Text;
                dgv_R_Summaries.Columns[0].HeaderText = cb_R_GroupBy.Text;
                dgv_R_Summaries.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv_R_Summaries.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                panel_R_Summarries.Visible = true;
                dgv_R_FullLists.Visible = false;
                //Graph configuration
                ConfigureReportGraph(sortedtable);
            } else { //Full lists - configure results to display in full SSDGV
                if (cb_R_ReportSource.SelectedIndex == 0) {
                    dgv_R_FullLists.Columns["DateDispensed"].Visible = false;
                } else {
                    dgv_R_FullLists.Columns["DateDispensed"].Visible = true;
                }
                bs_R_FullLists.DataSource = resultstable;
                bs_R_Summaries.DataSource = null; //Null this so Export Report can figure out which data to export
                R_LastGroupBy = "";
                dgv_R_FullLists.Visible = true;
                panel_R_Summarries.Visible = false;
            }
        }

        /// <summary>
        /// Configures and displays the graph for the last-run report.
        /// </summary>
        /// <param name="_data">The data table that contains the report data.  Only reads two columns.</param>
        private void ConfigureReportGraph(DataTable _data)
        {
            ZedGraph.GraphPane gp = zed_R_Chart.GraphPane;
            gp.CurveList.Clear();
            gp.XAxis.Title.Text = cb_R_GroupBy.Items[cb_R_GroupBy.SelectedIndex].ToString();
            gp.YAxis.Title.Text = cb_R_ReportSource.Items[cb_R_ReportSource.SelectedIndex].ToString();

            List<string> labels = new List<string>();
            List<double> counts = new List<double>();
            gp.XAxis.Type = ZedGraph.AxisType.Text;

            //Loop through each row and add the values to the lists
            foreach (DataRow dr in _data.Rows) {
                labels.Add(dr[0].ToString());
                counts.Add(Convert.ToDouble(dr[1]));
            }
            gp.XAxis.Scale.TextLabels = labels.ToArray();
            gp.AddBar(null, null, counts.ToArray(), Color.Red);

            zed_R_Chart.AxisChange();
            zed_R_Chart.Refresh();
        }

        /// <summary>
        /// Builds the plain english version of the report query based on the selected filters (if any)
        /// </summary>
        /// <returns>The plain english query</returns>
        private string BuildPlainEnglishQuery()
        {
            StringBuilder sb = new StringBuilder();

            //A full list or a summary
            if (cb_R_ReportType.SelectedIndex == 0) {
                sb.Append("A full list of ");
            } else {
                sb.Append("A summary of ");
            }

            //From current or dispensed inventory
            if (cb_R_ReportSource.SelectedIndex == 0) {
                sb.Append("glasses currently in inventory ");
            } else {
                sb.Append("glasses dispensed ");
            }

            if (repf_R_Filter.FilterItems.Count > 0) {
                sb.Append("that correspond to the following criteria:");
            }

            foreach (ReportFilterItem rfi in repf_R_Filter.FilterItems) {
                sb.Append("\n").Append(rfi.cb_FilterBy.Text);
                if (rfi.cb_FilterBy.SelectedIndex < 9) { //SKU or Rx fields
                    sb.Append(" between ").Append(rfi.tb_FilterA.Text).Append(" and ").Append(rfi.tb_FilterB.Text);
                } else if (rfi.cb_FilterBy.SelectedIndex > 12) { //Date fields
                    sb.Append(" between ").Append(rfi.dtp_FilterA.Value.Date.ToString("yyyy-MM-dd")).Append(" and ")
                        .Append(rfi.dtp_FilterB.Value.Date.ToString("yyyy-MM-dd"));
                } else { //Selection fields - type, gender, size, tint
                    sb.Append(" equals ").Append(rfi.cb_Selections.Text);
                }
            }

            if (cb_R_ReportType.SelectedIndex == 1) {
                sb.Append("Grouped by ").Append(cb_R_GroupBy.Text);
            }
            return sb.ToString();
        }

        private DataTable SortSummaryResults(DataTable _unsorted)
        {
            DataTable sorted = _unsorted.Clone();
            List<KeyValuePair<object, DataRow>> gl = new List<KeyValuePair<object, DataRow>>(10);
            IComparer<KeyValuePair<object, DataRow>> ic;
            
            //Populate the list
            if (cb_R_GroupBy.SelectedIndex > 11) { //Date fields (they sort well enough on their own)
                return _unsorted;
            } else if (cb_R_GroupBy.SelectedIndex == 2 
                    || cb_R_GroupBy.SelectedIndex == 6 
                    || cb_R_GroupBy.SelectedIndex > 7) { //Axis and selection fields (explicitly defined sorts based on characters)
                foreach (DataRow dr in _unsorted.Rows) {
                    char c = dr[0].ToString()[0];
                    if (c != '-')
                        gl.Add(new KeyValuePair<object, DataRow>(c, dr));
                }

            } else { //Rx fields that are sorted by number (a natural sort)
                Regex r = new Regex("(-?[0-9]*[.]?[0-9]+){1}");
                Match m;
                foreach (DataRow dr in _unsorted.Rows) {
                    if ((m = r.Match(dr[0].ToString())).Success) {
                        gl.Add(new KeyValuePair<object, DataRow>(Convert.ToSingle(m.ToString()), dr));
                    }
                }
            }

            //Assign the IComparer
            switch (cb_R_GroupBy.SelectedIndex) {
                case 2:
                case 6: //Axis
                    ic = new AxisSortComparer();
                    break;
                case 1:
                case 5: //Cylinder
                    ic = new CylinderSortComparer();
                    break;
                case 8: //Type
                    ic = new TypeSortComparer();
                    break;
                case 9: //Gender
                    ic = new GenderSortComparer();
                    break;
                case 10: //Size
                    ic = new SizeSortComparer();
                    break;
                case 11: //Tint
                    ic = new TintSortComparer();
                    break;
                default:
                    ic = new RxSortComparer();
                    break;
            }

            //Sort the list and populate the sorted DataTable
            gl.Sort(ic);
            foreach (KeyValuePair<object, DataRow> kvp in gl)
                sorted.Rows.Add(kvp.Value[0], kvp.Value[1]);

            return sorted;
        }

    }
}