using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	partial class frmSetScriptTimeout
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
			this.nudTimeout = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblTimeout = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblMs = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 29);
			this.baseConfigPanel.Size = new System.Drawing.Size(186, 29);
			// 
			// nudTimeout
			// 
			this.nudTimeout.DecimalPlaces = 0;
			this.nudTimeout.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudTimeout.Location = new System.Drawing.Point(84, 3);
			this.nudTimeout.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.nudTimeout.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudTimeout.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudTimeout.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudTimeout.Name = "nudTimeout";
			this.tableLayoutPanel1.SetRowSpan(this.nudTimeout, 2);
			this.nudTimeout.Size = new System.Drawing.Size(57, 21);
			this.nudTimeout.TabIndex = 3;
			this.nudTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblTimeout
			// 
			this.lblTimeout.AutoSize = true;
			this.lblTimeout.Location = new System.Drawing.Point(3, 5);
			this.lblTimeout.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblTimeout.Name = "lblTimeout";
			this.tableLayoutPanel1.SetRowSpan(this.lblTimeout, 2);
			this.lblTimeout.Size = new System.Drawing.Size(75, 13);
			this.lblTimeout.TabIndex = 0;
			this.lblTimeout.Text = "Script Timeout";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblMs, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTimeout, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.nudTimeout, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(186, 58);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblMs
			// 
			this.lblMs.AutoSize = true;
			this.lblMs.Location = new System.Drawing.Point(147, 5);
			this.lblMs.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblMs.Name = "lblMs";
			this.tableLayoutPanel1.SetRowSpan(this.lblMs, 2);
			this.lblMs.Size = new System.Drawing.Size(20, 13);
			this.lblMs.TabIndex = 4;
			this.lblMs.Text = "ms";
			// 
			// frmSetScriptTimeout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(186, 58);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmSetScriptTimeout";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Script Timeout";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private MesenNumericUpDown nudTimeout;
		private System.Windows.Forms.Label lblTimeout;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblMs;
	}
}