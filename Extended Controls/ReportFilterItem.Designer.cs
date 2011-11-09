namespace SecondSight.Extended_Controls
{
    partial class ReportFilterItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lb_FilterBy = new System.Windows.Forms.Label();
            this.cb_FilterBy = new System.Windows.Forms.ComboBox();
            this.lb_Between = new System.Windows.Forms.Label();
            this.tb_FilterA = new System.Windows.Forms.TextBox();
            this.lb_And = new System.Windows.Forms.Label();
            this.tb_FilterB = new System.Windows.Forms.TextBox();
            this.dtp_FilterA = new System.Windows.Forms.DateTimePicker();
            this.dtp_FilterB = new System.Windows.Forms.DateTimePicker();
            this.cb_Selections = new System.Windows.Forms.ComboBox();
            this.lb_Equals = new System.Windows.Forms.Label();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.tt_ReportFilterItem = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lb_FilterBy
            // 
            this.lb_FilterBy.AutoSize = true;
            this.lb_FilterBy.Location = new System.Drawing.Point(64, 4);
            this.lb_FilterBy.Name = "lb_FilterBy";
            this.lb_FilterBy.Size = new System.Drawing.Size(47, 13);
            this.lb_FilterBy.TabIndex = 0;
            this.lb_FilterBy.Text = "Filter By:";
            // 
            // cb_FilterBy
            // 
            this.cb_FilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_FilterBy.FormattingEnabled = true;
            this.cb_FilterBy.Items.AddRange(new object[] {
            "Date Dispensed"});
            this.cb_FilterBy.Location = new System.Drawing.Point(117, 1);
            this.cb_FilterBy.MaxDropDownItems = 15;
            this.cb_FilterBy.Name = "cb_FilterBy";
            this.cb_FilterBy.Size = new System.Drawing.Size(112, 21);
            this.cb_FilterBy.TabIndex = 1;
            this.tt_ReportFilterItem.SetToolTip(this.cb_FilterBy, "Select a field to filter by.");
            this.cb_FilterBy.SelectedIndexChanged += new System.EventHandler(this.cb_FilterBy_SelectedIndexChanged);
            // 
            // lb_Between
            // 
            this.lb_Between.AutoSize = true;
            this.lb_Between.Location = new System.Drawing.Point(236, 4);
            this.lb_Between.Name = "lb_Between";
            this.lb_Between.Size = new System.Drawing.Size(49, 13);
            this.lb_Between.TabIndex = 2;
            this.lb_Between.Text = "Between";
            // 
            // tb_FilterA
            // 
            this.tb_FilterA.Location = new System.Drawing.Point(291, 1);
            this.tb_FilterA.Name = "tb_FilterA";
            this.tb_FilterA.Size = new System.Drawing.Size(60, 20);
            this.tb_FilterA.TabIndex = 3;
            // 
            // lb_And
            // 
            this.lb_And.AutoSize = true;
            this.lb_And.Location = new System.Drawing.Point(357, 4);
            this.lb_And.Name = "lb_And";
            this.lb_And.Size = new System.Drawing.Size(25, 13);
            this.lb_And.TabIndex = 4;
            this.lb_And.Text = "and";
            // 
            // tb_FilterB
            // 
            this.tb_FilterB.Location = new System.Drawing.Point(388, 1);
            this.tb_FilterB.Name = "tb_FilterB";
            this.tb_FilterB.Size = new System.Drawing.Size(60, 20);
            this.tb_FilterB.TabIndex = 5;
            // 
            // dtp_FilterA
            // 
            this.dtp_FilterA.Location = new System.Drawing.Point(291, 1);
            this.dtp_FilterA.Name = "dtp_FilterA";
            this.dtp_FilterA.Size = new System.Drawing.Size(200, 20);
            this.dtp_FilterA.TabIndex = 8;
            // 
            // dtp_FilterB
            // 
            this.dtp_FilterB.Location = new System.Drawing.Point(528, 1);
            this.dtp_FilterB.Name = "dtp_FilterB";
            this.dtp_FilterB.Size = new System.Drawing.Size(200, 20);
            this.dtp_FilterB.TabIndex = 9;
            // 
            // cb_Selections
            // 
            this.cb_Selections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Selections.FormattingEnabled = true;
            this.cb_Selections.Location = new System.Drawing.Point(291, 1);
            this.cb_Selections.Name = "cb_Selections";
            this.cb_Selections.Size = new System.Drawing.Size(91, 21);
            this.cb_Selections.TabIndex = 10;
            this.cb_Selections.Visible = false;
            // 
            // lb_Equals
            // 
            this.lb_Equals.AutoSize = true;
            this.lb_Equals.Location = new System.Drawing.Point(236, 4);
            this.lb_Equals.Name = "lb_Equals";
            this.lb_Equals.Size = new System.Drawing.Size(39, 13);
            this.lb_Equals.TabIndex = 11;
            this.lb_Equals.Text = "Equals";
            this.lb_Equals.Visible = false;
            // 
            // btn_Remove
            // 
            this.btn_Remove.BackgroundImage = global::SecondSight.Properties.Resources.minus;
            this.btn_Remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Remove.FlatAppearance.BorderSize = 0;
            this.btn_Remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Remove.Location = new System.Drawing.Point(46, 5);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(12, 12);
            this.btn_Remove.TabIndex = 16;
            this.tt_ReportFilterItem.SetToolTip(this.btn_Remove, "Remove this filter.");
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.BackColor = System.Drawing.Color.Transparent;
            this.btn_Add.BackgroundImage = global::SecondSight.Properties.Resources.plus;
            this.btn_Add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Add.FlatAppearance.BorderSize = 0;
            this.btn_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Add.Location = new System.Drawing.Point(28, 5);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(12, 12);
            this.btn_Add.TabIndex = 17;
            this.btn_Add.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tt_ReportFilterItem.SetToolTip(this.btn_Add, "Add another filter.");
            this.btn_Add.UseVisualStyleBackColor = false;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // ReportFilterItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.btn_Remove);
            this.Controls.Add(this.lb_Equals);
            this.Controls.Add(this.cb_Selections);
            this.Controls.Add(this.dtp_FilterB);
            this.Controls.Add(this.dtp_FilterA);
            this.Controls.Add(this.tb_FilterB);
            this.Controls.Add(this.lb_And);
            this.Controls.Add(this.tb_FilterA);
            this.Controls.Add(this.lb_Between);
            this.Controls.Add(this.cb_FilterBy);
            this.Controls.Add(this.lb_FilterBy);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ReportFilterItem";
            this.Size = new System.Drawing.Size(882, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lb_FilterBy;
        public System.Windows.Forms.ComboBox cb_FilterBy;
        public System.Windows.Forms.Label lb_Between;
        public System.Windows.Forms.TextBox tb_FilterA;
        public System.Windows.Forms.Label lb_And;
        public System.Windows.Forms.TextBox tb_FilterB;
        public System.Windows.Forms.Label lb_Equals;
        public System.Windows.Forms.DateTimePicker dtp_FilterA;
        public System.Windows.Forms.DateTimePicker dtp_FilterB;
        public System.Windows.Forms.ComboBox cb_Selections;
        public System.Windows.Forms.Button btn_Remove;
        public System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.ToolTip tt_ReportFilterItem;
    }
}
