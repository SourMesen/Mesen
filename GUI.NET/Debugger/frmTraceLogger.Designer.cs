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
			this.chkShowMemoryValues = new System.Windows.Forms.CheckBox();
			this.chkShowRegisters = new System.Windows.Forms.CheckBox();
			this.chkIndentCode = new System.Windows.Forms.CheckBox();
			this.chkUseLabels = new System.Windows.Forms.CheckBox();
			this.chkShowByteCode = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkShowCpuCycles = new System.Windows.Forms.CheckBox();
			this.chkShowPpuCycles = new System.Windows.Forms.CheckBox();
			this.chkShowPpuScanline = new System.Windows.Forms.CheckBox();
			this.chkShowFrameCount = new System.Windows.Forms.CheckBox();
			this.chkShowEffectiveAddresses = new System.Windows.Forms.CheckBox();
			this.cboStatusFlagFormat = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.picExpressionWarning = new System.Windows.Forms.PictureBox();
			this.lblCondition = new System.Windows.Forms.Label();
			this.txtCondition = new System.Windows.Forms.TextBox();
			this.chkShowExtraInfo = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpExecutionLog = new System.Windows.Forms.GroupBox();
			this.txtTraceLog = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
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
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picExpressionWarning)).BeginInit();
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
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 274);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(781, 181);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnOpenTrace
			// 
			this.btnOpenTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenTrace.Enabled = false;
			this.btnOpenTrace.Location = new System.Drawing.Point(683, 3);
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
			this.grpLogOptions.Size = new System.Drawing.Size(775, 146);
			this.grpLogOptions.TabIndex = 3;
			this.grpLogOptions.TabStop = false;
			this.grpLogOptions.Text = "Log Options";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.chkShowMemoryValues, 3, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowRegisters, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkIndentCode, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.chkUseLabels, 3, 5);
			this.tableLayoutPanel2.Controls.Add(this.chkShowByteCode, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkShowCpuCycles, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuCycles, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuScanline, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowFrameCount, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowEffectiveAddresses, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.cboStatusFlagFormat, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkShowExtraInfo, 3, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 6;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(769, 127);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkShowMemoryValues
			// 
			this.chkShowMemoryValues.AutoSize = true;
			this.chkShowMemoryValues.Checked = true;
			this.chkShowMemoryValues.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowMemoryValues.Location = new System.Drawing.Point(332, 26);
			this.chkShowMemoryValues.Name = "chkShowMemoryValues";
			this.chkShowMemoryValues.Size = new System.Drawing.Size(128, 17);
			this.chkShowMemoryValues.TabIndex = 17;
			this.chkShowMemoryValues.Text = "Show Memory Values";
			this.chkShowMemoryValues.UseVisualStyleBackColor = true;
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
			// chkIndentCode
			// 
			this.chkIndentCode.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkIndentCode, 3);
			this.chkIndentCode.Location = new System.Drawing.Point(3, 107);
			this.chkIndentCode.Name = "chkIndentCode";
			this.chkIndentCode.Size = new System.Drawing.Size(194, 17);
			this.chkIndentCode.TabIndex = 8;
			this.chkIndentCode.Text = "Indent code based on stack pointer";
			this.chkIndentCode.UseVisualStyleBackColor = true;
			// 
			// chkUseLabels
			// 
			this.chkUseLabels.AutoSize = true;
			this.chkUseLabels.Location = new System.Drawing.Point(332, 107);
			this.chkUseLabels.Name = "chkUseLabels";
			this.chkUseLabels.Size = new System.Drawing.Size(79, 17);
			this.chkUseLabels.TabIndex = 11;
			this.chkUseLabels.Text = "Use Labels";
			this.chkUseLabels.UseVisualStyleBackColor = true;
			// 
			// chkShowByteCode
			// 
			this.chkShowByteCode.AutoSize = true;
			this.chkShowByteCode.Checked = true;
			this.chkShowByteCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowByteCode.Location = new System.Drawing.Point(3, 26);
			this.chkShowByteCode.Name = "chkShowByteCode";
			this.chkShowByteCode.Size = new System.Drawing.Size(75, 17);
			this.chkShowByteCode.TabIndex = 4;
			this.chkShowByteCode.Text = "Byte Code";
			this.chkShowByteCode.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Status Flag Format:";
			// 
			// chkShowCpuCycles
			// 
			this.chkShowCpuCycles.AutoSize = true;
			this.chkShowCpuCycles.Checked = true;
			this.chkShowCpuCycles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowCpuCycles.Location = new System.Drawing.Point(107, 3);
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
			this.chkShowPpuCycles.Location = new System.Drawing.Point(234, 3);
			this.chkShowPpuCycles.Name = "chkShowPpuCycles";
			this.chkShowPpuCycles.Size = new System.Drawing.Size(77, 17);
			this.chkShowPpuCycles.TabIndex = 5;
			this.chkShowPpuCycles.Text = "PPU Cycle";
			this.chkShowPpuCycles.UseVisualStyleBackColor = true;
			// 
			// chkShowPpuScanline
			// 
			this.chkShowPpuScanline.AutoSize = true;
			this.chkShowPpuScanline.Checked = true;
			this.chkShowPpuScanline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPpuScanline.Location = new System.Drawing.Point(234, 26);
			this.chkShowPpuScanline.Name = "chkShowPpuScanline";
			this.chkShowPpuScanline.Size = new System.Drawing.Size(92, 17);
			this.chkShowPpuScanline.TabIndex = 6;
			this.chkShowPpuScanline.Text = "PPU Scanline";
			this.chkShowPpuScanline.UseVisualStyleBackColor = true;
			// 
			// chkShowFrameCount
			// 
			this.chkShowFrameCount.AutoSize = true;
			this.chkShowFrameCount.Location = new System.Drawing.Point(107, 26);
			this.chkShowFrameCount.Name = "chkShowFrameCount";
			this.chkShowFrameCount.Size = new System.Drawing.Size(86, 17);
			this.chkShowFrameCount.TabIndex = 7;
			this.chkShowFrameCount.Text = "Frame Count";
			this.chkShowFrameCount.UseVisualStyleBackColor = true;
			// 
			// chkShowEffectiveAddresses
			// 
			this.chkShowEffectiveAddresses.AutoSize = true;
			this.chkShowEffectiveAddresses.Checked = true;
			this.chkShowEffectiveAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowEffectiveAddresses.Location = new System.Drawing.Point(332, 3);
			this.chkShowEffectiveAddresses.Name = "chkShowEffectiveAddresses";
			this.chkShowEffectiveAddresses.Size = new System.Drawing.Size(150, 17);
			this.chkShowEffectiveAddresses.TabIndex = 10;
			this.chkShowEffectiveAddresses.Text = "Show Effective Addresses";
			this.chkShowEffectiveAddresses.UseVisualStyleBackColor = true;
			// 
			// cboStatusFlagFormat
			// 
			this.cboStatusFlagFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStatusFlagFormat.FormattingEnabled = true;
			this.cboStatusFlagFormat.Location = new System.Drawing.Point(107, 49);
			this.cboStatusFlagFormat.Name = "cboStatusFlagFormat";
			this.cboStatusFlagFormat.Size = new System.Drawing.Size(121, 21);
			this.cboStatusFlagFormat.TabIndex = 13;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 4;
			this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel4, 4);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.picHelp, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.picExpressionWarning, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblCondition, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.txtCondition, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 73);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(769, 25);
			this.tableLayoutPanel4.TabIndex = 16;
			// 
			// picHelp
			// 
			this.picHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picHelp.Location = new System.Drawing.Point(748, 5);
			this.picHelp.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picHelp.Name = "picHelp";
			this.picHelp.Size = new System.Drawing.Size(18, 17);
			this.picHelp.TabIndex = 17;
			this.picHelp.TabStop = false;
			// 
			// picExpressionWarning
			// 
			this.picExpressionWarning.Image = global::Mesen.GUI.Properties.Resources.Warning;
			this.picExpressionWarning.Location = new System.Drawing.Point(724, 5);
			this.picExpressionWarning.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picExpressionWarning.Name = "picExpressionWarning";
			this.picExpressionWarning.Size = new System.Drawing.Size(18, 17);
			this.picExpressionWarning.TabIndex = 16;
			this.picExpressionWarning.TabStop = false;
			this.picExpressionWarning.Visible = false;
			// 
			// lblCondition
			// 
			this.lblCondition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCondition.AutoSize = true;
			this.lblCondition.Location = new System.Drawing.Point(3, 6);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(54, 13);
			this.lblCondition.TabIndex = 14;
			this.lblCondition.Text = "Condition:";
			// 
			// txtCondition
			// 
			this.txtCondition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCondition.Location = new System.Drawing.Point(63, 3);
			this.txtCondition.Name = "txtCondition";
			this.txtCondition.Size = new System.Drawing.Size(655, 20);
			this.txtCondition.TabIndex = 15;
			// 
			// chkShowExtraInfo
			// 
			this.chkShowExtraInfo.AutoSize = true;
			this.chkShowExtraInfo.Checked = true;
			this.chkShowExtraInfo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowExtraInfo.Location = new System.Drawing.Point(332, 49);
			this.chkShowExtraInfo.Name = "chkShowExtraInfo";
			this.chkShowExtraInfo.Size = new System.Drawing.Size(204, 17);
			this.chkShowExtraInfo.TabIndex = 9;
			this.chkShowExtraInfo.Text = "Additional information (IRQ, NMI, etc.)";
			this.chkShowExtraInfo.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpExecutionLog, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 24);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(787, 458);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// grpExecutionLog
			// 
			this.grpExecutionLog.Controls.Add(this.txtTraceLog);
			this.grpExecutionLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpExecutionLog.Location = new System.Drawing.Point(3, 3);
			this.grpExecutionLog.Name = "grpExecutionLog";
			this.grpExecutionLog.Size = new System.Drawing.Size(781, 265);
			this.grpExecutionLog.TabIndex = 2;
			this.grpExecutionLog.TabStop = false;
			this.grpExecutionLog.Text = "Execution Log";
			// 
			// txtTraceLog
			// 
			this.txtTraceLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTraceLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTraceLog.FontSize = 13F;
			this.txtTraceLog.HideSelection = false;
			this.txtTraceLog.Location = new System.Drawing.Point(3, 16);
			this.txtTraceLog.Name = "txtTraceLog";
			this.txtTraceLog.ShowContentNotes = false;
			this.txtTraceLog.ShowLineNumberNotes = false;
			this.txtTraceLog.ShowMemoryValues = false;
			this.txtTraceLog.ShowScrollbars = true;
			this.txtTraceLog.ShowSingleContentLineNotes = true;
			this.txtTraceLog.ShowSingleLineLineNumberNotes = false;
			this.txtTraceLog.Size = new System.Drawing.Size(775, 246);
			this.txtTraceLog.TabIndex = 0;
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
			this.menuStrip1.Size = new System.Drawing.Size(787, 24);
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
			this.logLinesToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.logLinesToolStripMenuItem.Text = "Line Count";
			// 
			// mnu100Lines
			// 
			this.mnu100Lines.Name = "mnu100Lines";
			this.mnu100Lines.Size = new System.Drawing.Size(104, 22);
			this.mnu100Lines.Text = "100";
			this.mnu100Lines.Click += new System.EventHandler(this.mnu100Lines_Click);
			// 
			// mnu1000Lines
			// 
			this.mnu1000Lines.Checked = true;
			this.mnu1000Lines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnu1000Lines.Name = "mnu1000Lines";
			this.mnu1000Lines.Size = new System.Drawing.Size(104, 22);
			this.mnu1000Lines.Text = "1000";
			this.mnu1000Lines.Click += new System.EventHandler(this.mnu1000Lines_Click);
			// 
			// mnu10000Lines
			// 
			this.mnu10000Lines.Name = "mnu10000Lines";
			this.mnu10000Lines.Size = new System.Drawing.Size(104, 22);
			this.mnu10000Lines.Text = "10000";
			this.mnu10000Lines.Click += new System.EventHandler(this.mnu10000Lines_Click);
			// 
			// mnu30000Lines
			// 
			this.mnu30000Lines.Name = "mnu30000Lines";
			this.mnu30000Lines.Size = new System.Drawing.Size(104, 22);
			this.mnu30000Lines.Text = "30000";
			this.mnu30000Lines.Click += new System.EventHandler(this.mnu30000Lines_Click);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(141, 22);
			this.mnuAutoRefresh.Text = "Auto-refresh";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(138, 6);
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefresh.Size = new System.Drawing.Size(141, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// frmTraceLogger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(787, 482);
			this.Controls.Add(this.tableLayoutPanel3);
			this.Controls.Add(this.menuStrip1);
			this.MinimumSize = new System.Drawing.Size(669, 448);
			this.Name = "frmTraceLogger";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trace Logger";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpLogOptions.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picExpressionWarning)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpExecutionLog.ResumeLayout(false);
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
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboStatusFlagFormat;
		private System.Windows.Forms.Label lblCondition;
		private System.Windows.Forms.TextBox txtCondition;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.PictureBox picExpressionWarning;
		private System.Windows.Forms.PictureBox picHelp;
		private ctrlScrollableTextbox txtTraceLog;
		private System.Windows.Forms.CheckBox chkShowMemoryValues;
	}
}