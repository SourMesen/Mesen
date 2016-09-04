namespace Mesen.GUI.Debugger
{
	partial class frmTraceLogger
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
			this.btnOpenTrace = new System.Windows.Forms.Button();
			this.btnStartLogging = new System.Windows.Forms.Button();
			this.btnStopLogging = new System.Windows.Forms.Button();
			this.grpLogOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkShowCpuCycles = new System.Windows.Forms.CheckBox();
			this.chkShowPpuCycles = new System.Windows.Forms.CheckBox();
			this.chkShowRegisters = new System.Windows.Forms.CheckBox();
			this.chkShowPpuScanline = new System.Windows.Forms.CheckBox();
			this.chkShowFrameCount = new System.Windows.Forms.CheckBox();
			this.chkShowExtraInfo = new System.Windows.Forms.CheckBox();
			this.chkShowByteCode = new System.Windows.Forms.CheckBox();
			this.chkIndentCode = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpLogOptions.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.btnOpenTrace, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnStartLogging, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnStopLogging, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpLogOptions, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkIndentCode, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(339, 150);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnOpenTrace
			// 
			this.btnOpenTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenTrace.Enabled = false;
			this.btnOpenTrace.Location = new System.Drawing.Point(241, 3);
			this.btnOpenTrace.Name = "btnOpenTrace";
			this.btnOpenTrace.Size = new System.Drawing.Size(95, 23);
			this.btnOpenTrace.TabIndex = 2;
			this.btnOpenTrace.Text = "Open Trace File";
			this.btnOpenTrace.UseVisualStyleBackColor = true;
			this.btnOpenTrace.Click += new System.EventHandler(this.btnOpenTrace_Click);
			// 
			// btnStartLogging
			// 
			this.btnStartLogging.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnStartLogging.Location = new System.Drawing.Point(3, 3);
			this.btnStartLogging.Name = "btnStartLogging";
			this.btnStartLogging.Size = new System.Drawing.Size(95, 23);
			this.btnStartLogging.TabIndex = 0;
			this.btnStartLogging.Text = "Start Logging";
			this.btnStartLogging.UseVisualStyleBackColor = true;
			this.btnStartLogging.Click += new System.EventHandler(this.btnStartLogging_Click);
			// 
			// btnStopLogging
			// 
			this.btnStopLogging.Enabled = false;
			this.btnStopLogging.Location = new System.Drawing.Point(104, 3);
			this.btnStopLogging.Name = "btnStopLogging";
			this.btnStopLogging.Size = new System.Drawing.Size(95, 23);
			this.btnStopLogging.TabIndex = 1;
			this.btnStopLogging.Text = "Stop Logging";
			this.btnStopLogging.UseVisualStyleBackColor = true;
			this.btnStopLogging.Click += new System.EventHandler(this.btnStopLogging_Click);
			// 
			// grpLogOptions
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.grpLogOptions, 3);
			this.grpLogOptions.Controls.Add(this.tableLayoutPanel2);
			this.grpLogOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpLogOptions.Location = new System.Drawing.Point(3, 32);
			this.grpLogOptions.Name = "grpLogOptions";
			this.grpLogOptions.Size = new System.Drawing.Size(333, 90);
			this.grpLogOptions.TabIndex = 3;
			this.grpLogOptions.TabStop = false;
			this.grpLogOptions.Text = "Log Contents";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.Controls.Add(this.chkShowCpuCycles, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuCycles, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowRegisters, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuScanline, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowFrameCount, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowExtraInfo, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkShowByteCode, 2, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(327, 71);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkShowCpuCycles
			// 
			this.chkShowCpuCycles.AutoSize = true;
			this.chkShowCpuCycles.Checked = true;
			this.chkShowCpuCycles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowCpuCycles.Location = new System.Drawing.Point(112, 3);
			this.chkShowCpuCycles.Name = "chkShowCpuCycles";
			this.chkShowCpuCycles.Size = new System.Drawing.Size(82, 17);
			this.chkShowCpuCycles.TabIndex = 3;
			this.chkShowCpuCycles.Text = "CPU Cycles";
			this.chkShowCpuCycles.UseVisualStyleBackColor = true;
			// 
			// chkShowPpuCycles
			// 
			this.chkShowPpuCycles.AutoSize = true;
			this.chkShowPpuCycles.Checked = true;
			this.chkShowPpuCycles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPpuCycles.Location = new System.Drawing.Point(3, 26);
			this.chkShowPpuCycles.Name = "chkShowPpuCycles";
			this.chkShowPpuCycles.Size = new System.Drawing.Size(77, 17);
			this.chkShowPpuCycles.TabIndex = 5;
			this.chkShowPpuCycles.Text = "PPU Cycle";
			this.chkShowPpuCycles.UseVisualStyleBackColor = true;
			// 
			// chkShowRegisters
			// 
			this.chkShowRegisters.AutoSize = true;
			this.chkShowRegisters.Checked = true;
			this.chkShowRegisters.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowRegisters.Location = new System.Drawing.Point(3, 3);
			this.chkShowRegisters.Name = "chkShowRegisters";
			this.chkShowRegisters.Size = new System.Drawing.Size(70, 17);
			this.chkShowRegisters.TabIndex = 2;
			this.chkShowRegisters.Text = "Registers";
			this.chkShowRegisters.UseVisualStyleBackColor = true;
			// 
			// chkShowPpuScanline
			// 
			this.chkShowPpuScanline.AutoSize = true;
			this.chkShowPpuScanline.Checked = true;
			this.chkShowPpuScanline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPpuScanline.Location = new System.Drawing.Point(112, 26);
			this.chkShowPpuScanline.Name = "chkShowPpuScanline";
			this.chkShowPpuScanline.Size = new System.Drawing.Size(92, 17);
			this.chkShowPpuScanline.TabIndex = 6;
			this.chkShowPpuScanline.Text = "PPU Scanline";
			this.chkShowPpuScanline.UseVisualStyleBackColor = true;
			// 
			// chkShowFrameCount
			// 
			this.chkShowFrameCount.AutoSize = true;
			this.chkShowFrameCount.Location = new System.Drawing.Point(221, 26);
			this.chkShowFrameCount.Name = "chkShowFrameCount";
			this.chkShowFrameCount.Size = new System.Drawing.Size(86, 17);
			this.chkShowFrameCount.TabIndex = 7;
			this.chkShowFrameCount.Text = "Frame Count";
			this.chkShowFrameCount.UseVisualStyleBackColor = true;
			// 
			// chkShowExtraInfo
			// 
			this.chkShowExtraInfo.AutoSize = true;
			this.chkShowExtraInfo.Checked = true;
			this.chkShowExtraInfo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowExtraInfo, 2);
			this.chkShowExtraInfo.Location = new System.Drawing.Point(3, 49);
			this.chkShowExtraInfo.Name = "chkShowExtraInfo";
			this.chkShowExtraInfo.Size = new System.Drawing.Size(204, 17);
			this.chkShowExtraInfo.TabIndex = 9;
			this.chkShowExtraInfo.Text = "Additional information (IRQ, NMI, etc.)";
			this.chkShowExtraInfo.UseVisualStyleBackColor = true;
			// 
			// chkShowByteCode
			// 
			this.chkShowByteCode.AutoSize = true;
			this.chkShowByteCode.Checked = true;
			this.chkShowByteCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowByteCode.Location = new System.Drawing.Point(221, 49);
			this.chkShowByteCode.Name = "chkShowByteCode";
			this.chkShowByteCode.Size = new System.Drawing.Size(75, 17);
			this.chkShowByteCode.TabIndex = 4;
			this.chkShowByteCode.Text = "Byte Code";
			this.chkShowByteCode.UseVisualStyleBackColor = true;
			// 
			// chkIndentCode
			// 
			this.chkIndentCode.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkIndentCode, 3);
			this.chkIndentCode.Location = new System.Drawing.Point(3, 128);
			this.chkIndentCode.Name = "chkIndentCode";
			this.chkIndentCode.Size = new System.Drawing.Size(194, 17);
			this.chkIndentCode.TabIndex = 8;
			this.chkIndentCode.Text = "Indent code based on stack pointer";
			this.chkIndentCode.UseVisualStyleBackColor = true;
			// 
			// frmTraceLogger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 150);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmTraceLogger";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trace Logger";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpLogOptions.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnStopLogging;
		private System.Windows.Forms.Button btnStartLogging;
		private System.Windows.Forms.GroupBox grpLogOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkShowCpuCycles;
		private System.Windows.Forms.CheckBox chkShowRegisters;
		private System.Windows.Forms.CheckBox chkShowFrameCount;
		private System.Windows.Forms.CheckBox chkShowPpuScanline;
		private System.Windows.Forms.CheckBox chkShowPpuCycles;
		private System.Windows.Forms.CheckBox chkShowByteCode;
		private System.Windows.Forms.CheckBox chkShowExtraInfo;
		private System.Windows.Forms.CheckBox chkIndentCode;
		private System.Windows.Forms.Button btnOpenTrace;
	}
}