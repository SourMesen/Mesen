namespace Mesen.GUI.Forms.Config
{
	partial class frmPreferences
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
			if(disposing && (components != null)) {
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
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.chkAutoLoadIps = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkSingleInstance = new System.Windows.Forms.CheckBox();
			this.grpFileAssociations = new System.Windows.Forms.GroupBox();
			this.tlpFileFormat = new System.Windows.Forms.TableLayoutPanel();
			this.chkNesFormat = new System.Windows.Forms.CheckBox();
			this.chkMmoFormat = new System.Windows.Forms.CheckBox();
			this.chkFdsFormat = new System.Windows.Forms.CheckBox();
			this.chkMstFormat = new System.Windows.Forms.CheckBox();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.grpFileAssociations.SuspendLayout();
			this.tlpFileFormat.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 143);
			this.baseConfigPanel.Size = new System.Drawing.Size(359, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.chkAutoLoadIps, 0, 1);
			this.tlpMain.Controls.Add(this.flowLayoutPanel6, 0, 0);
			this.tlpMain.Controls.Add(this.grpFileAssociations, 0, 2);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 4;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.Size = new System.Drawing.Size(359, 143);
			this.tlpMain.TabIndex = 1;
			// 
			// chkAutoLoadIps
			// 
			this.chkAutoLoadIps.AutoSize = true;
			this.chkAutoLoadIps.Location = new System.Drawing.Point(3, 26);
			this.chkAutoLoadIps.Name = "chkAutoLoadIps";
			this.chkAutoLoadIps.Size = new System.Drawing.Size(132, 17);
			this.chkAutoLoadIps.TabIndex = 9;
			this.chkAutoLoadIps.Text = "Auto-load IPS patches";
			this.chkAutoLoadIps.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.chkSingleInstance);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(359, 23);
			this.flowLayoutPanel6.TabIndex = 10;
			// 
			// chkSingleInstance
			// 
			this.chkSingleInstance.AutoSize = true;
			this.chkSingleInstance.Location = new System.Drawing.Point(3, 3);
			this.chkSingleInstance.Name = "chkSingleInstance";
			this.chkSingleInstance.Size = new System.Drawing.Size(228, 17);
			this.chkSingleInstance.TabIndex = 11;
			this.chkSingleInstance.Text = "Only allow one instance of Mesen at a time";
			this.chkSingleInstance.UseVisualStyleBackColor = true;
			// 
			// grpFileAssociations
			// 
			this.grpFileAssociations.Controls.Add(this.tlpFileFormat);
			this.grpFileAssociations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFileAssociations.Location = new System.Drawing.Point(3, 49);
			this.grpFileAssociations.Name = "grpFileAssociations";
			this.grpFileAssociations.Size = new System.Drawing.Size(353, 64);
			this.grpFileAssociations.TabIndex = 11;
			this.grpFileAssociations.TabStop = false;
			this.grpFileAssociations.Text = "File Associations";
			// 
			// tlpFileFormat
			// 
			this.tlpFileFormat.ColumnCount = 2;
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.Controls.Add(this.chkNesFormat, 0, 0);
			this.tlpFileFormat.Controls.Add(this.chkFdsFormat, 0, 1);
			this.tlpFileFormat.Controls.Add(this.chkMmoFormat, 1, 0);
			this.tlpFileFormat.Controls.Add(this.chkMstFormat, 1, 1);
			this.tlpFileFormat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpFileFormat.Location = new System.Drawing.Point(3, 16);
			this.tlpFileFormat.Name = "tlpFileFormat";
			this.tlpFileFormat.RowCount = 4;
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpFileFormat.Size = new System.Drawing.Size(347, 45);
			this.tlpFileFormat.TabIndex = 0;
			// 
			// chkNesFormat
			// 
			this.chkNesFormat.AutoSize = true;
			this.chkNesFormat.Location = new System.Drawing.Point(3, 3);
			this.chkNesFormat.Name = "chkNesFormat";
			this.chkNesFormat.Size = new System.Drawing.Size(51, 17);
			this.chkNesFormat.TabIndex = 10;
			this.chkNesFormat.Text = ".NES";
			this.chkNesFormat.UseVisualStyleBackColor = true;
			// 
			// chkMmoFormat
			// 
			this.chkMmoFormat.AutoSize = true;
			this.chkMmoFormat.Enabled = false;
			this.chkMmoFormat.Location = new System.Drawing.Point(176, 3);
			this.chkMmoFormat.Name = "chkMmoFormat";
			this.chkMmoFormat.Size = new System.Drawing.Size(133, 17);
			this.chkMmoFormat.TabIndex = 11;
			this.chkMmoFormat.Text = ".MMO (Mesen Movies)";
			this.chkMmoFormat.UseVisualStyleBackColor = true;
			// 
			// chkFdsFormat
			// 
			this.chkFdsFormat.AutoSize = true;
			this.chkFdsFormat.Enabled = false;
			this.chkFdsFormat.Location = new System.Drawing.Point(3, 26);
			this.chkFdsFormat.Name = "chkFdsFormat";
			this.chkFdsFormat.Size = new System.Drawing.Size(162, 17);
			this.chkFdsFormat.TabIndex = 12;
			this.chkFdsFormat.Text = ".FDS (Famicom Disk System)";
			this.chkFdsFormat.UseVisualStyleBackColor = true;
			// 
			// chkMstFormat
			// 
			this.chkMstFormat.AutoSize = true;
			this.chkMstFormat.Enabled = false;
			this.chkMstFormat.Location = new System.Drawing.Point(176, 26);
			this.chkMstFormat.Name = "chkMstFormat";
			this.chkMstFormat.Size = new System.Drawing.Size(144, 17);
			this.chkMstFormat.TabIndex = 13;
			this.chkMstFormat.Text = ".MST (Mesen Savestate)";
			this.chkMstFormat.UseVisualStyleBackColor = true;
			// 
			// frmPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 172);
			this.Controls.Add(this.tlpMain);
			this.Name = "frmPreferences";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tlpMain, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.grpFileAssociations.ResumeLayout(false);
			this.tlpFileFormat.ResumeLayout(false);
			this.tlpFileFormat.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkAutoLoadIps;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.CheckBox chkSingleInstance;
		private System.Windows.Forms.GroupBox grpFileAssociations;
		private System.Windows.Forms.TableLayoutPanel tlpFileFormat;
		private System.Windows.Forms.CheckBox chkNesFormat;
		private System.Windows.Forms.CheckBox chkFdsFormat;
		private System.Windows.Forms.CheckBox chkMmoFormat;
		private System.Windows.Forms.CheckBox chkMstFormat;
	}
}