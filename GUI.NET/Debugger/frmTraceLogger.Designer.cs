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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnOpenTrace = new System.Windows.Forms.Button();
			this.btnStartLogging = new System.Windows.Forms.Button();
			this.btnStopLogging = new System.Windows.Forms.Button();
			this.grpLogOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkShowEffectiveAddresses = new System.Windows.Forms.CheckBox();
			this.chkShowPpuCycles = new System.Windows.Forms.CheckBox();
			this.chkShowRegisters = new System.Windows.Forms.CheckBox();
			this.chkShowExtraInfo = new System.Windows.Forms.CheckBox();
			this.chkIndentCode = new System.Windows.Forms.CheckBox();
			this.chkShowByteCode = new System.Windows.Forms.CheckBox();
			this.chkShowFrameCount = new System.Windows.Forms.CheckBox();
			this.chkShowPpuScanline = new System.Windows.Forms.CheckBox();
			this.chkShowCpuCycles = new System.Windows.Forms.CheckBox();
			this.chkUseLabels = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpExecutionLog = new System.Windows.Forms.GroupBox();
			this.txtTraceLog = new System.Windows.Forms.TextBox();
			this.tmrUpdateLog = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnu100Lines = new System.Windows.Forms.ToolStripMenuItem();
			this.mnu1000Lines = new System.Windows.Forms.ToolStripMenuItem();
			this.mnu10000Lines = new System.Windows.Forms.ToolStripMenuItem();
			this.mnu30000Lines = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpLogOptions.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpExecutionLog.SuspendLayout();
			this.menuStrip1.SuspendLayout();
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 314);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(442, 206);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnOpenTrace
			// 
			this.btnOpenTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenTrace.Enabled = false;
			this.btnOpenTrace.Location = new System.Drawing.Point(344, 3);
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
			this.grpLogOptions.Location = new System.Drawing.Point(3, 32);
			this.grpLogOptions.Name = "grpLogOptions";
			this.grpLogOptions.Size = new System.Drawing.Size(436, 168);
			this.grpLogOptions.TabIndex = 3;
			this.grpLogOptions.TabStop = false;
			this.grpLogOptions.Text = "Log Options";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel2.Controls.Add(this.chkShowEffectiveAddresses, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuCycles, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowRegisters, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowExtraInfo, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkIndentCode, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.chkShowByteCode, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkShowFrameCount, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuScanline, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowCpuCycles, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkUseLabels, 2, 6);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 7;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(430, 149);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkShowEffectiveAddresses
			// 
			this.chkShowEffectiveAddresses.AutoSize = true;
			this.chkShowEffectiveAddresses.Checked = true;
			this.chkShowEffectiveAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowEffectiveAddresses, 2);
			this.chkShowEffectiveAddresses.Location = new System.Drawing.Point(3, 72);
			this.chkShowEffectiveAddresses.Name = "chkShowEffectiveAddresses";
			this.chkShowEffectiveAddresses.Size = new System.Drawing.Size(150, 17);
			this.chkShowEffectiveAddresses.TabIndex = 10;
			this.chkShowEffectiveAddresses.Text = "Show Effective Addresses";
			this.chkShowEffectiveAddresses.UseVisualStyleBackColor = true;
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
			// chkShowExtraInfo
			// 
			this.chkShowExtraInfo.AutoSize = true;
			this.chkShowExtraInfo.Checked = true;
			this.chkShowExtraInfo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowExtraInfo, 2);
			this.chkShowExtraInfo.Location = new System.Drawing.Point(3, 95);
			this.chkShowExtraInfo.Name = "chkShowExtraInfo";
			this.chkShowExtraInfo.Size = new System.Drawing.Size(204, 17);
			this.chkShowExtraInfo.TabIndex = 9;
			this.chkShowExtraInfo.Text = "Additional information (IRQ, NMI, etc.)";
			this.chkShowExtraInfo.UseVisualStyleBackColor = true;
			// 
			// chkIndentCode
			// 
			this.chkIndentCode.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkIndentCode, 2);
			this.chkIndentCode.Location = new System.Drawing.Point(3, 129);
			this.chkIndentCode.Name = "chkIndentCode";
			this.chkIndentCode.Size = new System.Drawing.Size(194, 17);
			this.chkIndentCode.TabIndex = 8;
			this.chkIndentCode.Text = "Indent code based on stack pointer";
			this.chkIndentCode.UseVisualStyleBackColor = true;
			// 
			// chkShowByteCode
			// 
			this.chkShowByteCode.AutoSize = true;
			this.chkShowByteCode.Checked = true;
			this.chkShowByteCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowByteCode.Location = new System.Drawing.Point(3, 49);
			this.chkShowByteCode.Name = "chkShowByteCode";
			this.chkShowByteCode.Size = new System.Drawing.Size(75, 17);
			this.chkShowByteCode.TabIndex = 4;
			this.chkShowByteCode.Text = "Byte Code";
			this.chkShowByteCode.UseVisualStyleBackColor = true;
			// 
			// chkShowFrameCount
			// 
			this.chkShowFrameCount.AutoSize = true;
			this.chkShowFrameCount.Location = new System.Drawing.Point(289, 49);
			this.chkShowFrameCount.Name = "chkShowFrameCount";
			this.chkShowFrameCount.Size = new System.Drawing.Size(86, 17);
			this.chkShowFrameCount.TabIndex = 7;
			this.chkShowFrameCount.Text = "Frame Count";
			this.chkShowFrameCount.UseVisualStyleBackColor = true;
			// 
			// chkShowPpuScanline
			// 
			this.chkShowPpuScanline.AutoSize = true;
			this.chkShowPpuScanline.Checked = true;
			this.chkShowPpuScanline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPpuScanline.Location = new System.Drawing.Point(289, 26);
			this.chkShowPpuScanline.Name = "chkShowPpuScanline";
			this.chkShowPpuScanline.Size = new System.Drawing.Size(92, 17);
			this.chkShowPpuScanline.TabIndex = 6;
			this.chkShowPpuScanline.Text = "PPU Scanline";
			this.chkShowPpuScanline.UseVisualStyleBackColor = true;
			// 
			// chkShowCpuCycles
			// 
			this.chkShowCpuCycles.AutoSize = true;
			this.chkShowCpuCycles.Checked = true;
			this.chkShowCpuCycles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowCpuCycles.Location = new System.Drawing.Point(289, 3);
			this.chkShowCpuCycles.Name = "chkShowCpuCycles";
			this.chkShowCpuCycles.Size = new System.Drawing.Size(82, 17);
			this.chkShowCpuCycles.TabIndex = 3;
			this.chkShowCpuCycles.Text = "CPU Cycles";
			this.chkShowCpuCycles.UseVisualStyleBackColor = true;
			// 
			// chkUseLabels
			// 
			this.chkUseLabels.AutoSize = true;
			this.chkUseLabels.Location = new System.Drawing.Point(289, 129);
			this.chkUseLabels.Name = "chkUseLabels";
			this.chkUseLabels.Size = new System.Drawing.Size(79, 17);
			this.chkUseLabels.TabIndex = 11;
			this.chkUseLabels.Text = "Use Labels";
			this.chkUseLabels.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpExecutionLog, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 24);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(808, 523);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// grpExecutionLog
			// 
			this.tableLayoutPanel3.SetColumnSpan(this.grpExecutionLog, 2);
			this.grpExecutionLog.Controls.Add(this.txtTraceLog);
			this.grpExecutionLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpExecutionLog.Location = new System.Drawing.Point(3, 3);
			this.grpExecutionLog.Name = "grpExecutionLog";
			this.grpExecutionLog.Size = new System.Drawing.Size(802, 305);
			this.grpExecutionLog.TabIndex = 2;
			this.grpExecutionLog.TabStop = false;
			this.grpExecutionLog.Text = "Execution Log";
			// 
			// txtTraceLog
			// 
			this.txtTraceLog.BackColor = System.Drawing.SystemColors.Window;
			this.txtTraceLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTraceLog.Location = new System.Drawing.Point(3, 16);
			this.txtTraceLog.Multiline = true;
			this.txtTraceLog.Name = "txtTraceLog";
			this.txtTraceLog.ReadOnly = true;
			this.txtTraceLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtTraceLog.Size = new System.Drawing.Size(796, 286);
			this.txtTraceLog.TabIndex = 1;
			// 
			// tmrUpdateLog
			// 
			this.tmrUpdateLog.Interval = 150;
			this.tmrUpdateLog.Tick += new System.EventHandler(this.tmrUpdateLog_Tick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(808, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logLinesToolStripMenuItem,
            this.mnuAutoRefresh,
            this.toolStripMenuItem1,
            this.mnuRefresh});
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.showToolStripMenuItem.Text = "View";
			// 
			// logLinesToolStripMenuItem
			// 
			this.logLinesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu100Lines,
            this.mnu1000Lines,
            this.mnu10000Lines,
            this.mnu30000Lines});
			this.logLinesToolStripMenuItem.Name = "logLinesToolStripMenuItem";
			this.logLinesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.logLinesToolStripMenuItem.Text = "Line Count";
			// 
			// mnu100Lines
			// 
			this.mnu100Lines.Name = "mnu100Lines";
			this.mnu100Lines.Size = new System.Drawing.Size(152, 22);
			this.mnu100Lines.Text = "100";
			this.mnu100Lines.Click += new System.EventHandler(this.mnu100Lines_Click);
			// 
			// mnu1000Lines
			// 
			this.mnu1000Lines.Checked = true;
			this.mnu1000Lines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnu1000Lines.Name = "mnu1000Lines";
			this.mnu1000Lines.Size = new System.Drawing.Size(152, 22);
			this.mnu1000Lines.Text = "1000";
			this.mnu1000Lines.Click += new System.EventHandler(this.mnu1000Lines_Click);
			// 
			// mnu10000Lines
			// 
			this.mnu10000Lines.Name = "mnu10000Lines";
			this.mnu10000Lines.Size = new System.Drawing.Size(152, 22);
			this.mnu10000Lines.Text = "10000";
			this.mnu10000Lines.Click += new System.EventHandler(this.mnu10000Lines_Click);
			// 
			// mnu30000Lines
			// 
			this.mnu30000Lines.Name = "mnu30000Lines";
			this.mnu30000Lines.Size = new System.Drawing.Size(152, 22);
			this.mnu30000Lines.Text = "30000";
			this.mnu30000Lines.Click += new System.EventHandler(this.mnu30000Lines_Click);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(152, 22);
			this.mnuAutoRefresh.Text = "Auto-refresh";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefresh.Size = new System.Drawing.Size(152, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// frmTraceLogger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(808, 547);
			this.Controls.Add(this.tableLayoutPanel3);
			this.Controls.Add(this.menuStrip1);
			this.Name = "frmTraceLogger";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trace Logger";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpLogOptions.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpExecutionLog.ResumeLayout(false);
			this.grpExecutionLog.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.CheckBox chkShowEffectiveAddresses;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TextBox txtTraceLog;
		private System.Windows.Forms.Timer tmrUpdateLog;
		private System.Windows.Forms.GroupBox grpExecutionLog;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefresh;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
		private System.Windows.Forms.ToolStripMenuItem mnu100Lines;
		private System.Windows.Forms.ToolStripMenuItem mnu1000Lines;
		private System.Windows.Forms.ToolStripMenuItem mnu10000Lines;
		private System.Windows.Forms.ToolStripMenuItem mnu30000Lines;
		private System.Windows.Forms.CheckBox chkUseLabels;
	}
}