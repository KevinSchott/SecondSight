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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecondSight.Merge
{
    class MCAddGroupDialog : Form
    {
        private Label label1;
        private Button btn_OK;
        private Button btn_Cancel;
        private TextBox tb_NumToAdd;
        private Label lb_Label;
        private Label label3;
        private TextBox tb_Label;

        private string return_label;
        private int return_numtoadd;

        public string ReturnLabel
        {
            get {return return_label;}
        }

        public int ReturnNumToAdd
        {
            get {return return_numtoadd;}
        }
    
        public MCAddGroupDialog()
        {
            return_label = "";
            return_numtoadd = 0;

            InitializeComponent();
        }

        /// <summary>
        /// Click event for the OK button
        /// </summary>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            if(tb_NumToAdd.Text.Length > 0) {
                try {
                    return_numtoadd = Convert.ToInt16(tb_NumToAdd.Text);
                } catch {
                    MessageBox.Show("You must enter a whole number in \"Number of Glasses\" to continue.", "Invalid Number of Glasses",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    tb_NumToAdd.Focus();
                    return;
                }
                
                //Set the label if there's something in the text box
                if(tb_Label.Text.Length > 0) {
                    return_label = tb_Label.Text;
                }

                DialogResult = DialogResult.OK;
                Close();
            } else {
                MessageBox.Show("You must enter a whole number in \"Number of Glasses\" to continue.", "Invalid Number of Glasses",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Click event for the Cancel button
        /// </summary>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// OnActivated overridden event handler.  Sets the cursor focus to the label textbox
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            tb_Label.Focus();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCAddGroupDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.tb_Label = new System.Windows.Forms.TextBox();
            this.tb_NumToAdd = new System.Windows.Forms.TextBox();
            this.lb_Label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(259, 146);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "&OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(173, 146);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "&Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // tb_Label
            // 
            this.tb_Label.Location = new System.Drawing.Point(173, 67);
            this.tb_Label.Name = "tb_Label";
            this.tb_Label.Size = new System.Drawing.Size(161, 20);
            this.tb_Label.TabIndex = 3;
            // 
            // tb_NumToAdd
            // 
            this.tb_NumToAdd.Location = new System.Drawing.Point(173, 102);
            this.tb_NumToAdd.Name = "tb_NumToAdd";
            this.tb_NumToAdd.Size = new System.Drawing.Size(61, 20);
            this.tb_NumToAdd.TabIndex = 4;
            // 
            // lb_Label
            // 
            this.lb_Label.AutoSize = true;
            this.lb_Label.Location = new System.Drawing.Point(131, 70);
            this.lb_Label.Name = "lb_Label";
            this.lb_Label.Size = new System.Drawing.Size(36, 13);
            this.lb_Label.TabIndex = 5;
            this.lb_Label.Text = "Label:";
            this.lb_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Number of Glasses:";
            // 
            // MCAddGroupDialog
            // 
            this.ClientSize = new System.Drawing.Size(461, 181);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_Label);
            this.Controls.Add(this.tb_NumToAdd);
            this.Controls.Add(this.tb_Label);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MCAddGroupDialog";
            this.Text = "Add Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
} //namespace SecondSight.Merge