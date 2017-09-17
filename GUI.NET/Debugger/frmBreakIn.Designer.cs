using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	partial class frmBreakIn
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
			this.lblBreakIn = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.radCpuInstructions = new System.Windows.Forms.RadioButton();
			this.radPpuCycles = new System.Windows.Forms.RadioButton();
			this.nudCount = new MesenNumericUpDown();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 48);
			this.baseConfigPanel.Size = new System.Drawing.Size(240, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblBreakIn, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.nudCount, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 77);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblBreakIn
			// 
			this.lblBreakIn.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBreakIn.AutoSize = true;
			this.lblBreakIn.Location = new System.Drawing.Point(3, 16);
			this.lblBreakIn.Name = "lblBreakIn";
			this.lblBreakIn.Size = new System.Drawing.Size(49, 13);
			this.lblBreakIn.TabIndex = 0;
			this.lblBreakIn.Text = "Break in:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.radCpuInstructions);
			this.flowLayoutPanel2.Controls.Add(this.radPpuCycles);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(131, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(109, 45);
			this.flowLayoutPanel2.TabIndex = 2;
			// 
			// radCpuInstructions
			// 
			this.radCpuInstructions.AutoSize = true;
			this.radCpuInstructions.Checked = true;
			this.radCpuInstructions.Location = new System.Drawing.Point(3, 3);
			this.radCpuInstructions.Name = "radCpuInstructions";
			this.radCpuInstructions.Size = new System.Drawing.Size(104, 17);
			this.radCpuInstructions.TabIndex = 0;
			this.radCpuInstructions.TabStop = true;
			this.radCpuInstructions.Text = "CPU Instructions";
			this.radCpuInstructions.UseVisualStyleBackColor = true;
			// 
			// radPpuCycles
			// 
			this.radPpuCycles.AutoSize = true;
			this.radPpuCycles.Location = new System.Drawing.Point(3, 26);
			this.radPpuCycles.Name = "radPpuCycles";
			this.radPpuCycles.Size = new System.Drawing.Size(81, 17);
			this.radPpuCycles.TabIndex = 1;
			this.radPpuCycles.Text = "PPU Cycles";
			this.radPpuCycles.UseVisualStyleBackColor = true;
			// 
			// nudCount
			// 
			this.nudCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudCount.Location = new System.Drawing.Point(58, 12);
			this.nudCount.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
			this.nudCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudCount.Name = "nudCount";
			this.nudCount.Size = new System.Drawing.Size(70, 20);
			this.nudCount.TabIndex = 3;
			this.nudCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// frmBreakIn
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(240, 77);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmBreakIn";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Break In...";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblBreakIn;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.RadioButton radCpuInstructions;
		private System.Windows.Forms.RadioButton radPpuCycles;
		private MesenNumericUpDown nudCount;
	}
}