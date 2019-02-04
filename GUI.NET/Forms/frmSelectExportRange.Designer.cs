using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms
{
	partial class frmSelectExportRange
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
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblStartTime = new System.Windows.Forms.Label();
			this.lblEndTime = new System.Windows.Forms.Label();
			this.dtpEnd = new System.Windows.Forms.DateTimePicker();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 61);
			this.baseConfigPanel.Size = new System.Drawing.Size(198, 29);
			// 
			// dtpStart
			// 
			this.dtpStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.dtpStart.CustomFormat = "HH:mm:ss";
			this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpStart.Location = new System.Drawing.Point(63, 3);
			this.dtpStart.MaxDate = new System.DateTime(3000, 1, 1, 0, 0, 0, 0);
			this.dtpStart.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.ShowUpDown = true;
			this.dtpStart.Size = new System.Drawing.Size(69, 20);
			this.dtpStart.TabIndex = 0;
			this.dtpStart.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.dtpStart, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblStartTime, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblEndTime, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.dtpEnd, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 61);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// lblStartTime
			// 
			this.lblStartTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblStartTime.AutoSize = true;
			this.lblStartTime.Location = new System.Drawing.Point(3, 6);
			this.lblStartTime.Name = "lblStartTime";
			this.lblStartTime.Size = new System.Drawing.Size(54, 13);
			this.lblStartTime.TabIndex = 2;
			this.lblStartTime.Text = "Start time:";
			// 
			// lblEndTime
			// 
			this.lblEndTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEndTime.AutoSize = true;
			this.lblEndTime.Location = new System.Drawing.Point(3, 32);
			this.lblEndTime.Name = "lblEndTime";
			this.lblEndTime.Size = new System.Drawing.Size(51, 13);
			this.lblEndTime.TabIndex = 3;
			this.lblEndTime.Text = "End time:";
			// 
			// dtpEnd
			// 
			this.dtpEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.dtpEnd.CustomFormat = "HH:mm:ss";
			this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpEnd.Location = new System.Drawing.Point(63, 29);
			this.dtpEnd.MaxDate = new System.DateTime(3000, 1, 1, 0, 0, 0, 0);
			this.dtpEnd.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.ShowUpDown = true;
			this.dtpEnd.Size = new System.Drawing.Size(69, 20);
			this.dtpEnd.TabIndex = 1;
			this.dtpEnd.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			// 
			// frmSelectExportRange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(198, 90);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSelectExportRange";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Custom range...";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblEndTime;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private System.Windows.Forms.Label lblStartTime;
	}
}