namespace Mesen.GUI.Forms
{
	partial class frmUpdatePrompt
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLatestVersionString = new System.Windows.Forms.Label();
			this.lblLatestVersion = new System.Windows.Forms.Label();
			this.lblCurrentVersion = new System.Windows.Forms.Label();
			this.txtChangelog = new System.Windows.Forms.TextBox();
			this.lblChangeLog = new System.Windows.Forms.Label();
			this.lblCurrentVersionString = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.lblLatestVersionString, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblLatestVersion, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCurrentVersion, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtChangelog, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblChangeLog, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblCurrentVersionString, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 0);
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 222);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblLatestVersionString
			// 
			this.lblLatestVersionString.AutoSize = true;
			this.lblLatestVersionString.Location = new System.Drawing.Point(91, 10);
			this.lblLatestVersionString.Name = "lblLatestVersionString";
			this.lblLatestVersionString.Size = new System.Drawing.Size(31, 13);
			this.lblLatestVersionString.TabIndex = 7;
			this.lblLatestVersionString.Text = "0.1.1";
			// 
			// lblLatestVersion
			// 
			this.lblLatestVersion.AutoSize = true;
			this.lblLatestVersion.Location = new System.Drawing.Point(8, 10);
			this.lblLatestVersion.Name = "lblLatestVersion";
			this.lblLatestVersion.Size = new System.Drawing.Size(77, 13);
			this.lblLatestVersion.TabIndex = 2;
			this.lblLatestVersion.Text = "Latest Version:";
			// 
			// lblCurrentVersion
			// 
			this.lblCurrentVersion.AutoSize = true;
			this.lblCurrentVersion.Location = new System.Drawing.Point(191, 10);
			this.lblCurrentVersion.Name = "lblCurrentVersion";
			this.lblCurrentVersion.Size = new System.Drawing.Size(82, 13);
			this.lblCurrentVersion.TabIndex = 3;
			this.lblCurrentVersion.Text = "Current Version:";
			// 
			// txtChangelog
			// 
			this.txtChangelog.BackColor = System.Drawing.Color.White;
			this.tableLayoutPanel1.SetColumnSpan(this.txtChangelog, 4);
			this.txtChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtChangelog.Location = new System.Drawing.Point(8, 49);
			this.txtChangelog.Multiline = true;
			this.txtChangelog.Name = "txtChangelog";
			this.txtChangelog.ReadOnly = true;
			this.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtChangelog.Size = new System.Drawing.Size(379, 141);
			this.txtChangelog.TabIndex = 4;
			this.txtChangelog.TabStop = false;
			// 
			// lblChangeLog
			// 
			this.lblChangeLog.AutoSize = true;
			this.lblChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblChangeLog.Location = new System.Drawing.Point(8, 33);
			this.lblChangeLog.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.lblChangeLog.Name = "lblChangeLog";
			this.lblChangeLog.Size = new System.Drawing.Size(61, 13);
			this.lblChangeLog.TabIndex = 5;
			this.lblChangeLog.Text = "Changelog:";
			// 
			// lblCurrentVersionString
			// 
			this.lblCurrentVersionString.AutoSize = true;
			this.lblCurrentVersionString.Location = new System.Drawing.Point(279, 10);
			this.lblCurrentVersionString.Name = "lblCurrentVersionString";
			this.lblCurrentVersionString.Size = new System.Drawing.Size(31, 13);
			this.lblCurrentVersionString.TabIndex = 6;
			this.lblCurrentVersionString.Text = "0.1.0";
			// 
			// flowLayoutPanel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 4);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Controls.Add(this.btnUpdate);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 193);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(385, 29);
			this.flowLayoutPanel1.TabIndex = 8;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(311, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(71, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnUpdate
			// 
			this.btnUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnUpdate.Location = new System.Drawing.Point(230, 3);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 0;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			// 
			// frmUpdatePrompt
			// 
			this.AcceptButton = this.btnUpdate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(395, 222);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmUpdatePrompt";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Mesen - Update Available";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblLatestVersionString;
		private System.Windows.Forms.Label lblLatestVersion;
		private System.Windows.Forms.Label lblCurrentVersion;
		private System.Windows.Forms.TextBox txtChangelog;
		private System.Windows.Forms.Label lblChangeLog;
		private System.Windows.Forms.Label lblCurrentVersionString;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnUpdate;
	}
}