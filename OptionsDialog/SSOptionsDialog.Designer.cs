namespace SecondSight.OptionsDialog
{
    partial class SSOptionsDialog
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Separator = new System.Windows.Forms.GroupBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.OptionsPageGeneral = new SecondSight.OptionsDialog.SSOptionsPage();
            this.SuspendLayout();
            // 
            // Separator
            // 
            this.Separator.Location = new System.Drawing.Point(-1, 300);
            this.Separator.Margin = new System.Windows.Forms.Padding(0);
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(498, 10);
            this.Separator.TabIndex = 1;
            this.Separator.TabStop = false;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(326, 319);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(407, 319);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // OptionsPageGeneral
            // 
            this.OptionsPageGeneral.Location = new System.Drawing.Point(12, 12);
            this.OptionsPageGeneral.Name = "OptionsPageGeneral";
            this.OptionsPageGeneral.Size = new System.Drawing.Size(479, 298);
            this.OptionsPageGeneral.TabIndex = 0;
            // 
            // SSOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 353);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.Separator);
            this.Controls.Add(this.OptionsPageGeneral);
            this.Name = "SSOptionsDialog";
            this.Text = "SecondSight Options";
            this.ResumeLayout(false);

        }

        #endregion

        private SSOptionsPage OptionsPageGeneral;
        private System.Windows.Forms.GroupBox Separator;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
    }
}