/*
 * Created by SharpDevelop.
 * User: Che
 * Date: 12/5/2010
 * Time: 3:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SecondSight.OptionsDialog
{
	partial class SSOptionsPage
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_DBFolder = new System.Windows.Forms.TextBox();
            this.btn_DBFolderBrowse = new System.Windows.Forms.Button();
            this.btn_DBFolderDefault = new System.Windows.Forms.Button();
            this.tb_BackupsFolder = new System.Windows.Forms.TextBox();
            this.btn_BackupsFolderDefault = new System.Windows.Forms.Button();
            this.btn_BackupsFolderBrowse = new System.Windows.Forms.Button();
            this.chb_AutoBackup = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_AutoBackupNumberKept = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_AutoBackupOps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_AutoBackupTime = new System.Windows.Forms.TextBox();
            this.chb_AutoBackupOps = new System.Windows.Forms.CheckBox();
            this.chb_AutoBackupTime = new System.Windows.Forms.CheckBox();
            this.chb_OpenMostRecent = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database folder:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Backups folder:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_DBFolder
            // 
            this.tb_DBFolder.Location = new System.Drawing.Point(96, 3);
            this.tb_DBFolder.Name = "tb_DBFolder";
            this.tb_DBFolder.Size = new System.Drawing.Size(369, 20);
            this.tb_DBFolder.TabIndex = 0;
            // 
            // btn_DBFolderBrowse
            // 
            this.btn_DBFolderBrowse.Location = new System.Drawing.Point(390, 29);
            this.btn_DBFolderBrowse.Name = "btn_DBFolderBrowse";
            this.btn_DBFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.btn_DBFolderBrowse.TabIndex = 2;
            this.btn_DBFolderBrowse.Text = "Browse";
            this.btn_DBFolderBrowse.UseVisualStyleBackColor = true;
            this.btn_DBFolderBrowse.Click += new System.EventHandler(this.btn_DBFolderBrowse_Click);
            // 
            // btn_DBFolderDefault
            // 
            this.btn_DBFolderDefault.Location = new System.Drawing.Point(309, 29);
            this.btn_DBFolderDefault.Name = "btn_DBFolderDefault";
            this.btn_DBFolderDefault.Size = new System.Drawing.Size(75, 23);
            this.btn_DBFolderDefault.TabIndex = 1;
            this.btn_DBFolderDefault.Text = "Use Default";
            this.btn_DBFolderDefault.UseVisualStyleBackColor = true;
            this.btn_DBFolderDefault.Click += new System.EventHandler(this.btn_DBFolderDefault_Click);
            // 
            // tb_BackupsFolder
            // 
            this.tb_BackupsFolder.Location = new System.Drawing.Point(96, 64);
            this.tb_BackupsFolder.Name = "tb_BackupsFolder";
            this.tb_BackupsFolder.Size = new System.Drawing.Size(369, 20);
            this.tb_BackupsFolder.TabIndex = 3;
            // 
            // btn_BackupsFolderDefault
            // 
            this.btn_BackupsFolderDefault.Location = new System.Drawing.Point(309, 90);
            this.btn_BackupsFolderDefault.Name = "btn_BackupsFolderDefault";
            this.btn_BackupsFolderDefault.Size = new System.Drawing.Size(75, 23);
            this.btn_BackupsFolderDefault.TabIndex = 4;
            this.btn_BackupsFolderDefault.Text = "Use Default";
            this.btn_BackupsFolderDefault.UseVisualStyleBackColor = true;
            this.btn_BackupsFolderDefault.Click += new System.EventHandler(this.btn_BackupsFolderDefault_Click);
            // 
            // btn_BackupsFolderBrowse
            // 
            this.btn_BackupsFolderBrowse.Location = new System.Drawing.Point(390, 90);
            this.btn_BackupsFolderBrowse.Name = "btn_BackupsFolderBrowse";
            this.btn_BackupsFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.btn_BackupsFolderBrowse.TabIndex = 5;
            this.btn_BackupsFolderBrowse.Text = "Browse";
            this.btn_BackupsFolderBrowse.UseVisualStyleBackColor = true;
            this.btn_BackupsFolderBrowse.Click += new System.EventHandler(this.btn_BackupsFolderBrowse_Click);
            // 
            // chb_AutoBackup
            // 
            this.chb_AutoBackup.Checked = true;
            this.chb_AutoBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_AutoBackup.Location = new System.Drawing.Point(6, 19);
            this.chb_AutoBackup.Name = "chb_AutoBackup";
            this.chb_AutoBackup.Size = new System.Drawing.Size(281, 24);
            this.chb_AutoBackup.TabIndex = 6;
            this.chb_AutoBackup.Text = "Backup currently open database automatically";
            this.chb_AutoBackup.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_AutoBackupNumberKept);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_AutoBackupOps);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_AutoBackupTime);
            this.groupBox1.Controls.Add(this.chb_AutoBackupOps);
            this.groupBox1.Controls.Add(this.chb_AutoBackupTime);
            this.groupBox1.Controls.Add(this.chb_AutoBackup);
            this.groupBox1.Location = new System.Drawing.Point(5, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 136);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Automatic Backup";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(130, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "most recent automatic backups.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(37, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Keep the";
            // 
            // tb_AutoBackupNumberKept
            // 
            this.tb_AutoBackupNumberKept.Location = new System.Drawing.Point(95, 109);
            this.tb_AutoBackupNumberKept.Name = "tb_AutoBackupNumberKept";
            this.tb_AutoBackupNumberKept.Size = new System.Drawing.Size(29, 20);
            this.tb_AutoBackupNumberKept.TabIndex = 11;
            this.tb_AutoBackupNumberKept.Text = "5";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(243, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Actions";
            // 
            // tb_AutoBackupOps
            // 
            this.tb_AutoBackupOps.Location = new System.Drawing.Point(204, 81);
            this.tb_AutoBackupOps.Name = "tb_AutoBackupOps";
            this.tb_AutoBackupOps.Size = new System.Drawing.Size(33, 20);
            this.tb_AutoBackupOps.TabIndex = 10;
            this.tb_AutoBackupOps.Text = "10";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(243, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Minutes";
            // 
            // tb_AutoBackupTime
            // 
            this.tb_AutoBackupTime.Location = new System.Drawing.Point(204, 51);
            this.tb_AutoBackupTime.Name = "tb_AutoBackupTime";
            this.tb_AutoBackupTime.Size = new System.Drawing.Size(33, 20);
            this.tb_AutoBackupTime.TabIndex = 8;
            // 
            // chb_AutoBackupOps
            // 
            this.chb_AutoBackupOps.Checked = true;
            this.chb_AutoBackupOps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_AutoBackupOps.Location = new System.Drawing.Point(40, 79);
            this.chb_AutoBackupOps.Name = "chb_AutoBackupOps";
            this.chb_AutoBackupOps.Size = new System.Drawing.Size(158, 24);
            this.chb_AutoBackupOps.TabIndex = 9;
            this.chb_AutoBackupOps.Text = "After a number of actions:";
            this.chb_AutoBackupOps.UseVisualStyleBackColor = true;
            // 
            // chb_AutoBackupTime
            // 
            this.chb_AutoBackupTime.Location = new System.Drawing.Point(40, 49);
            this.chb_AutoBackupTime.Name = "chb_AutoBackupTime";
            this.chb_AutoBackupTime.Size = new System.Drawing.Size(139, 24);
            this.chb_AutoBackupTime.TabIndex = 7;
            this.chb_AutoBackupTime.Text = "After time has passed:";
            this.chb_AutoBackupTime.UseVisualStyleBackColor = true;
            // 
            // chb_OpenMostRecent
            // 
            this.chb_OpenMostRecent.AutoSize = true;
            this.chb_OpenMostRecent.Location = new System.Drawing.Point(11, 121);
            this.chb_OpenMostRecent.Name = "chb_OpenMostRecent";
            this.chb_OpenMostRecent.Size = new System.Drawing.Size(278, 17);
            this.chb_OpenMostRecent.TabIndex = 10;
            this.chb_OpenMostRecent.Text = "Open most recent database when SecondSight starts";
            this.chb_OpenMostRecent.UseVisualStyleBackColor = true;
            // 
            // SSOptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chb_OpenMostRecent);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_BackupsFolderDefault);
            this.Controls.Add(this.btn_BackupsFolderBrowse);
            this.Controls.Add(this.tb_BackupsFolder);
            this.Controls.Add(this.btn_DBFolderDefault);
            this.Controls.Add(this.btn_DBFolderBrowse);
            this.Controls.Add(this.tb_DBFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SSOptionsPage";
            this.Size = new System.Drawing.Size(470, 286);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button btn_DBFolderBrowse;
		private System.Windows.Forms.TextBox tb_DBFolder;
		private System.Windows.Forms.Button btn_DBFolderDefault;
		private System.Windows.Forms.TextBox tb_BackupsFolder;
		private System.Windows.Forms.Button btn_BackupsFolderDefault;
		private System.Windows.Forms.Button btn_BackupsFolderBrowse;
		private System.Windows.Forms.CheckBox chb_AutoBackup;
		private System.Windows.Forms.TextBox tb_AutoBackupNumberKept;
		private System.Windows.Forms.TextBox tb_AutoBackupOps;
		private System.Windows.Forms.TextBox tb_AutoBackupTime;
		private System.Windows.Forms.CheckBox chb_AutoBackupOps;
        private System.Windows.Forms.CheckBox chb_AutoBackupTime;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chb_OpenMostRecent;
	}
}
