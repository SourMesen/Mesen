namespace Mesen.GUI.Debugger
{
	partial class frmDebugger
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
			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlDebuggerCode = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.contextMenuCode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSetNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.ctrlConsoleStatus = new Mesen.GUI.Debugger.ctrlConsoleStatus();
			this.ctrlDebuggerCodeSplit = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.grpWatch = new System.Windows.Forms.GroupBox();
			this.ctrlWatch = new Mesen.GUI.Debugger.ctrlWatch();
			this.grpBreakpoints = new System.Windows.Forms.GroupBox();
			this.ctrlBreakpoints = new Mesen.GUI.Debugger.Controls.ctrlBreakpoints();
			this.grpCallstack = new System.Windows.Forms.GroupBox();
			this.ctrlCallstack = new Mesen.GUI.Debugger.Controls.ctrlCallstack();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSplitView = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuContinue = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepInto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOver = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOut = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableEnableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRunOneFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToNmiHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCodeDataLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.autoLoadsaveCDLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuLoadCdlFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAsCdlFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetCdlLog = new System.Windows.Forms.ToolStripMenuItem();
			this.generateROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveStrippedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveUnusedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTraceLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblPrgAnalysis = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblPrgAnalysisResult = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblChrAnalysis = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblChrAnalysisResult = new System.Windows.Forms.ToolStripStatusLabel();
			this.tmrCdlRatios = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.tlpTop.SuspendLayout();
			this.contextMenuCode.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.grpWatch.SuspendLayout();
			this.grpBreakpoints.SuspendLayout();
			this.grpCallstack.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.Location = new System.Drawing.Point(0, 24);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.tlpTop);
			this.splitContainer.Panel1MinSize = 375;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel10);
			this.splitContainer.Panel2MinSize = 50;
			this.splitContainer.Size = new System.Drawing.Size(984, 564);
			this.splitContainer.SplitterDistance = 412;
			this.splitContainer.TabIndex = 1;
			// 
			// tlpTop
			// 
			this.tlpTop.ColumnCount = 3;
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTop.Controls.Add(this.ctrlDebuggerCode, 0, 0);
			this.tlpTop.Controls.Add(this.ctrlConsoleStatus, 2, 0);
			this.tlpTop.Controls.Add(this.ctrlDebuggerCodeSplit, 1, 0);
			this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTop.Location = new System.Drawing.Point(0, 0);
			this.tlpTop.Name = "tlpTop";
			this.tlpTop.RowCount = 1;
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.Size = new System.Drawing.Size(984, 412);
			this.tlpTop.TabIndex = 2;
			// 
			// ctrlDebuggerCode
			// 
			this.ctrlDebuggerCode.Code = null;
			this.ctrlDebuggerCode.ContextMenuStrip = this.contextMenuCode;
			this.ctrlDebuggerCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCode.Location = new System.Drawing.Point(3, 3);
			this.ctrlDebuggerCode.Name = "ctrlDebuggerCode";
			this.ctrlDebuggerCode.Size = new System.Drawing.Size(546, 406);
			this.ctrlDebuggerCode.TabIndex = 2;
			this.ctrlDebuggerCode.OnWatchAdded += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnWatchAdded);
			this.ctrlDebuggerCode.OnSetNextStatement += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnSetNextStatement);
			this.ctrlDebuggerCode.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// contextMenuCode
			// 
			this.contextMenuCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNextStatement,
            this.mnuSetNextStatement});
			this.contextMenuCode.Name = "contextMenuWatch";
			this.contextMenuCode.Size = new System.Drawing.Size(259, 48);
			// 
			// mnuShowNextStatement
			// 
			this.mnuShowNextStatement.Name = "mnuShowNextStatement";
			this.mnuShowNextStatement.ShortcutKeyDisplayString = "Alt+*";
			this.mnuShowNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Multiply)));
			this.mnuShowNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuShowNextStatement.Text = "Show Next Statement";
			// 
			// mnuSetNextStatement
			// 
			this.mnuSetNextStatement.Name = "mnuSetNextStatement";
			this.mnuSetNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F10)));
			this.mnuSetNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuSetNextStatement.Text = "Set Next Statement";
			// 
			// ctrlConsoleStatus
			// 
			this.ctrlConsoleStatus.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctrlConsoleStatus.Location = new System.Drawing.Point(552, 0);
			this.ctrlConsoleStatus.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlConsoleStatus.Name = "ctrlConsoleStatus";
			this.ctrlConsoleStatus.Size = new System.Drawing.Size(432, 362);
			this.ctrlConsoleStatus.TabIndex = 3;
			// 
			// ctrlDebuggerCodeSplit
			// 
			this.ctrlDebuggerCodeSplit.Code = null;
			this.ctrlDebuggerCodeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCodeSplit.Location = new System.Drawing.Point(555, 3);
			this.ctrlDebuggerCodeSplit.Name = "ctrlDebuggerCodeSplit";
			this.ctrlDebuggerCodeSplit.Size = new System.Drawing.Size(1, 406);
			this.ctrlDebuggerCodeSplit.TabIndex = 4;
			this.ctrlDebuggerCodeSplit.Visible = false;
			this.ctrlDebuggerCodeSplit.OnWatchAdded += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnWatchAdded);
			this.ctrlDebuggerCodeSplit.OnSetNextStatement += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnSetNextStatement);
			this.ctrlDebuggerCodeSplit.Enter += new System.EventHandler(this.ctrlDebuggerCodeSplit_Enter);
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 3;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.Controls.Add(this.grpWatch, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.grpBreakpoints, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.grpCallstack, 2, 0);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 1;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(984, 148);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// grpWatch
			// 
			this.grpWatch.Controls.Add(this.ctrlWatch);
			this.grpWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpWatch.Location = new System.Drawing.Point(3, 3);
			this.grpWatch.Name = "grpWatch";
			this.grpWatch.Size = new System.Drawing.Size(322, 142);
			this.grpWatch.TabIndex = 2;
			this.grpWatch.TabStop = false;
			this.grpWatch.Text = "Watch";
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(3, 16);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(316, 123);
			this.ctrlWatch.TabIndex = 0;
			// 
			// grpBreakpoints
			// 
			this.grpBreakpoints.Controls.Add(this.ctrlBreakpoints);
			this.grpBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpBreakpoints.Location = new System.Drawing.Point(331, 3);
			this.grpBreakpoints.Name = "grpBreakpoints";
			this.grpBreakpoints.Size = new System.Drawing.Size(322, 142);
			this.grpBreakpoints.TabIndex = 3;
			this.grpBreakpoints.TabStop = false;
			this.grpBreakpoints.Text = "Breakpoints";
			// 
			// ctrlBreakpoints
			// 
			this.ctrlBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlBreakpoints.Location = new System.Drawing.Point(3, 16);
			this.ctrlBreakpoints.Name = "ctrlBreakpoints";
			this.ctrlBreakpoints.Size = new System.Drawing.Size(316, 123);
			this.ctrlBreakpoints.TabIndex = 0;
			this.ctrlBreakpoints.BreakpointNavigation += new System.EventHandler(this.ctrlBreakpoints_BreakpointNavigation);
			// 
			// grpCallstack
			// 
			this.grpCallstack.Controls.Add(this.ctrlCallstack);
			this.grpCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCallstack.Location = new System.Drawing.Point(659, 3);
			this.grpCallstack.Name = "grpCallstack";
			this.grpCallstack.Size = new System.Drawing.Size(322, 142);
			this.grpCallstack.TabIndex = 4;
			this.grpCallstack.TabStop = false;
			this.grpCallstack.Text = "Callstack";
			// 
			// ctrlCallstack
			// 
			this.ctrlCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCallstack.Location = new System.Drawing.Point(3, 16);
			this.ctrlCallstack.Name = "ctrlCallstack";
			this.ctrlCallstack.Size = new System.Drawing.Size(316, 123);
			this.ctrlCallstack.TabIndex = 0;
			this.ctrlCallstack.FunctionSelected += new System.EventHandler(this.ctrlCallstack_FunctionSelected);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuView,
            this.debugToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(984, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.mnuClose});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(100, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(103, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSplitView,
            this.fontSizeToolStripMenuItem});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(44, 20);
			this.mnuView.Text = "View";
			// 
			// mnuSplitView
			// 
			this.mnuSplitView.CheckOnClick = true;
			this.mnuSplitView.Name = "mnuSplitView";
			this.mnuSplitView.Size = new System.Drawing.Size(125, 22);
			this.mnuSplitView.Text = "Split View";
			this.mnuSplitView.Click += new System.EventHandler(this.mnuSplitView_Click);
			// 
			// fontSizeToolStripMenuItem
			// 
			this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIncreaseFontSize,
            this.mnuDecreaseFontSize,
            this.mnuResetFontSize});
			this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
			this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.fontSizeToolStripMenuItem.Text = "Text Size";
			// 
			// mnuIncreaseFontSize
			// 
			this.mnuIncreaseFontSize.Name = "mnuIncreaseFontSize";
			this.mnuIncreaseFontSize.ShortcutKeyDisplayString = "Ctrl++";
			this.mnuIncreaseFontSize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.mnuIncreaseFontSize.Size = new System.Drawing.Size(197, 22);
			this.mnuIncreaseFontSize.Text = "Increase";
			this.mnuIncreaseFontSize.Click += new System.EventHandler(this.mnuIncreaseFontSize_Click);
			// 
			// mnuDecreaseFontSize
			// 
			this.mnuDecreaseFontSize.Name = "mnuDecreaseFontSize";
			this.mnuDecreaseFontSize.ShortcutKeyDisplayString = "Ctrl+-";
			this.mnuDecreaseFontSize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.mnuDecreaseFontSize.Size = new System.Drawing.Size(197, 22);
			this.mnuDecreaseFontSize.Text = "Decrease";
			this.mnuDecreaseFontSize.Click += new System.EventHandler(this.mnuDecreaseFontSize_Click);
			// 
			// mnuResetFontSize
			// 
			this.mnuResetFontSize.Name = "mnuResetFontSize";
			this.mnuResetFontSize.ShortcutKeyDisplayString = "Ctrl+0";
			this.mnuResetFontSize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.mnuResetFontSize.Size = new System.Drawing.Size(197, 22);
			this.mnuResetFontSize.Text = "Reset to Default";
			this.mnuResetFontSize.Click += new System.EventHandler(this.mnuResetFontSize_Click);
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContinue,
            this.mnuBreak,
            this.mnuStepInto,
            this.mnuStepOver,
            this.mnuStepOut,
            this.toolStripMenuItem1,
            this.mnuToggleBreakpoint,
            this.mnuDisableEnableBreakpoint,
            this.toolStripMenuItem2,
            this.mnuRunOneFrame});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// mnuContinue
			// 
			this.mnuContinue.Name = "mnuContinue";
			this.mnuContinue.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuContinue.Size = new System.Drawing.Size(258, 22);
			this.mnuContinue.Text = "Continue";
			this.mnuContinue.Click += new System.EventHandler(this.mnuContinue_Click);
			// 
			// mnuBreak
			// 
			this.mnuBreak.Name = "mnuBreak";
			this.mnuBreak.ShortcutKeyDisplayString = "Ctrl+Alt+Break";
			this.mnuBreak.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Cancel)));
			this.mnuBreak.Size = new System.Drawing.Size(258, 22);
			this.mnuBreak.Text = "Break";
			this.mnuBreak.Click += new System.EventHandler(this.mnuBreak_Click);
			// 
			// mnuStepInto
			// 
			this.mnuStepInto.Name = "mnuStepInto";
			this.mnuStepInto.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mnuStepInto.Size = new System.Drawing.Size(258, 22);
			this.mnuStepInto.Text = "Step Into";
			this.mnuStepInto.Click += new System.EventHandler(this.mnuStepInto_Click);
			// 
			// mnuStepOver
			// 
			this.mnuStepOver.Name = "mnuStepOver";
			this.mnuStepOver.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.mnuStepOver.Size = new System.Drawing.Size(258, 22);
			this.mnuStepOver.Text = "Step Over";
			this.mnuStepOver.Click += new System.EventHandler(this.mnuStepOver_Click);
			// 
			// mnuStepOut
			// 
			this.mnuStepOut.Name = "mnuStepOut";
			this.mnuStepOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
			this.mnuStepOut.Size = new System.Drawing.Size(258, 22);
			this.mnuStepOut.Text = "Step Out";
			this.mnuStepOut.Click += new System.EventHandler(this.mnuStepOut_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(258, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			this.mnuToggleBreakpoint.Click += new System.EventHandler(this.mnuToggleBreakpoint_Click);
			// 
			// mnuDisableEnableBreakpoint
			// 
			this.mnuDisableEnableBreakpoint.Name = "mnuDisableEnableBreakpoint";
			this.mnuDisableEnableBreakpoint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9)));
			this.mnuDisableEnableBreakpoint.Size = new System.Drawing.Size(258, 22);
			this.mnuDisableEnableBreakpoint.Text = "Disable/Enable Breakpoint";
			this.mnuDisableEnableBreakpoint.Click += new System.EventHandler(this.mnuDisableEnableBreakpoint_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuRunOneFrame
			// 
			this.mnuRunOneFrame.Name = "mnuRunOneFrame";
			this.mnuRunOneFrame.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.mnuRunOneFrame.Size = new System.Drawing.Size(258, 22);
			this.mnuRunOneFrame.Text = "Run one frame";
			this.mnuRunOneFrame.Click += new System.EventHandler(this.mnuRunOneFrame_Click);
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFind,
            this.mnuFindNext,
            this.mnuFindPrev,
            this.mnuGoTo});
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.searchToolStripMenuItem.Text = "Search";
			// 
			// mnuFind
			// 
			this.mnuFind.Name = "mnuFind";
			this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.mnuFind.Size = new System.Drawing.Size(196, 22);
			this.mnuFind.Text = "Find...";
			this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
			// 
			// mnuFindNext
			// 
			this.mnuFindNext.Name = "mnuFindNext";
			this.mnuFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.mnuFindNext.Size = new System.Drawing.Size(196, 22);
			this.mnuFindNext.Text = "Find Next";
			this.mnuFindNext.Click += new System.EventHandler(this.mnuFindNext_Click);
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.mnuFindPrev.Size = new System.Drawing.Size(196, 22);
			this.mnuFindPrev.Text = "Find Previous";
			this.mnuFindPrev.Click += new System.EventHandler(this.mnuFindPrev_Click);
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToAddress,
            this.mnuGoToIrqHandler,
            this.mnuGoToNmiHandler,
            this.mnuGoToResetHandler});
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.Size = new System.Drawing.Size(196, 22);
			this.mnuGoTo.Text = "Go To...";
			// 
			// mnuGoToAddress
			// 
			this.mnuGoToAddress.Name = "mnuGoToAddress";
			this.mnuGoToAddress.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.mnuGoToAddress.Size = new System.Drawing.Size(158, 22);
			this.mnuGoToAddress.Text = "Address";
			this.mnuGoToAddress.Click += new System.EventHandler(this.mnuGoToAddress_Click);
			// 
			// mnuGoToIrqHandler
			// 
			this.mnuGoToIrqHandler.Name = "mnuGoToIrqHandler";
			this.mnuGoToIrqHandler.Size = new System.Drawing.Size(158, 22);
			this.mnuGoToIrqHandler.Text = "IRQ Handler";
			this.mnuGoToIrqHandler.Click += new System.EventHandler(this.mnuGoToIrqHandler_Click);
			// 
			// mnuGoToNmiHandler
			// 
			this.mnuGoToNmiHandler.Name = "mnuGoToNmiHandler";
			this.mnuGoToNmiHandler.Size = new System.Drawing.Size(158, 22);
			this.mnuGoToNmiHandler.Text = "NMI Handler";
			this.mnuGoToNmiHandler.Click += new System.EventHandler(this.mnuGoToNmiHandler_Click);
			// 
			// mnuGoToResetHandler
			// 
			this.mnuGoToResetHandler.Name = "mnuGoToResetHandler";
			this.mnuGoToResetHandler.Size = new System.Drawing.Size(158, 22);
			this.mnuGoToResetHandler.Text = "Reset Handler";
			this.mnuGoToResetHandler.Click += new System.EventHandler(this.mnuGoToResetHandler_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPpuViewer,
            this.mnuMemoryViewer,
            this.mnuCodeDataLogger,
            this.mnuTraceLogger});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// mnuPpuViewer
			// 
			this.mnuPpuViewer.Name = "mnuPpuViewer";
			this.mnuPpuViewer.Size = new System.Drawing.Size(171, 22);
			this.mnuPpuViewer.Text = "PPU Viewer";
			this.mnuPpuViewer.Click += new System.EventHandler(this.mnuNametableViewer_Click);
			// 
			// mnuMemoryViewer
			// 
			this.mnuMemoryViewer.Name = "mnuMemoryViewer";
			this.mnuMemoryViewer.Size = new System.Drawing.Size(171, 22);
			this.mnuMemoryViewer.Text = "Memory Viewer";
			this.mnuMemoryViewer.Click += new System.EventHandler(this.mnuMemoryViewer_Click);
			// 
			// mnuCodeDataLogger
			// 
			this.mnuCodeDataLogger.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLoadsaveCDLFileToolStripMenuItem,
            this.toolStripMenuItem4,
            this.mnuLoadCdlFile,
            this.mnuSaveAsCdlFile,
            this.mnuResetCdlLog,
            this.generateROMToolStripMenuItem});
			this.mnuCodeDataLogger.Name = "mnuCodeDataLogger";
			this.mnuCodeDataLogger.Size = new System.Drawing.Size(171, 22);
			this.mnuCodeDataLogger.Text = "Code/Data Logger";
			// 
			// autoLoadsaveCDLFileToolStripMenuItem
			// 
			this.autoLoadsaveCDLFileToolStripMenuItem.Checked = true;
			this.autoLoadsaveCDLFileToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoLoadsaveCDLFileToolStripMenuItem.Enabled = false;
			this.autoLoadsaveCDLFileToolStripMenuItem.Name = "autoLoadsaveCDLFileToolStripMenuItem";
			this.autoLoadsaveCDLFileToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.autoLoadsaveCDLFileToolStripMenuItem.Text = "Auto load/save log file";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuLoadCdlFile
			// 
			this.mnuLoadCdlFile.Name = "mnuLoadCdlFile";
			this.mnuLoadCdlFile.Size = new System.Drawing.Size(193, 22);
			this.mnuLoadCdlFile.Text = "Load CDL file...";
			this.mnuLoadCdlFile.Click += new System.EventHandler(this.mnuLoadCdlFile_Click);
			// 
			// mnuSaveAsCdlFile
			// 
			this.mnuSaveAsCdlFile.Name = "mnuSaveAsCdlFile";
			this.mnuSaveAsCdlFile.Size = new System.Drawing.Size(193, 22);
			this.mnuSaveAsCdlFile.Text = "Save as CDL file...";
			this.mnuSaveAsCdlFile.Click += new System.EventHandler(this.mnuSaveAsCdlFile_Click);
			// 
			// mnuResetCdlLog
			// 
			this.mnuResetCdlLog.Name = "mnuResetCdlLog";
			this.mnuResetCdlLog.Size = new System.Drawing.Size(193, 22);
			this.mnuResetCdlLog.Text = "Reset log";
			this.mnuResetCdlLog.Click += new System.EventHandler(this.mnuResetCdlLog_Click);
			// 
			// generateROMToolStripMenuItem
			// 
			this.generateROMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveStrippedDataToolStripMenuItem,
            this.saveUnusedDataToolStripMenuItem});
			this.generateROMToolStripMenuItem.Enabled = false;
			this.generateROMToolStripMenuItem.Name = "generateROMToolStripMenuItem";
			this.generateROMToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.generateROMToolStripMenuItem.Text = "Generate ROM";
			// 
			// saveStrippedDataToolStripMenuItem
			// 
			this.saveStrippedDataToolStripMenuItem.Name = "saveStrippedDataToolStripMenuItem";
			this.saveStrippedDataToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.saveStrippedDataToolStripMenuItem.Text = "Save stripped data";
			// 
			// saveUnusedDataToolStripMenuItem
			// 
			this.saveUnusedDataToolStripMenuItem.Name = "saveUnusedDataToolStripMenuItem";
			this.saveUnusedDataToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.saveUnusedDataToolStripMenuItem.Text = "Save unused data";
			// 
			// mnuTraceLogger
			// 
			this.mnuTraceLogger.Name = "mnuTraceLogger";
			this.mnuTraceLogger.Size = new System.Drawing.Size(171, 22);
			this.mnuTraceLogger.Text = "Trace Logger";
			this.mnuTraceLogger.Click += new System.EventHandler(this.mnuTraceLogger_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPrgAnalysis,
            this.lblPrgAnalysisResult,
            this.lblChrAnalysis,
            this.lblChrAnalysisResult});
			this.statusStrip.Location = new System.Drawing.Point(0, 588);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(984, 24);
			this.statusStrip.TabIndex = 3;
			this.statusStrip.Text = "statusStrip1";
			// 
			// lblPrgAnalysis
			// 
			this.lblPrgAnalysis.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.lblPrgAnalysis.Name = "lblPrgAnalysis";
			this.lblPrgAnalysis.Size = new System.Drawing.Size(76, 19);
			this.lblPrgAnalysis.Text = "PRG analysis:";
			// 
			// lblPrgAnalysisResult
			// 
			this.lblPrgAnalysisResult.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.lblPrgAnalysisResult.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.lblPrgAnalysisResult.Name = "lblPrgAnalysisResult";
			this.lblPrgAnalysisResult.Size = new System.Drawing.Size(235, 19);
			this.lblPrgAnalysisResult.Text = "xx% (Code: xx%, Data: xx%, Unknown: xx%)";
			// 
			// lblChrAnalysis
			// 
			this.lblChrAnalysis.Name = "lblChrAnalysis";
			this.lblChrAnalysis.Size = new System.Drawing.Size(78, 19);
			this.lblChrAnalysis.Text = "CHR analysis:";
			// 
			// lblChrAnalysisResult
			// 
			this.lblChrAnalysisResult.Name = "lblChrAnalysisResult";
			this.lblChrAnalysisResult.Size = new System.Drawing.Size(239, 19);
			this.lblChrAnalysisResult.Text = "xx% (Drawn: xx%, Read: xx%, Unknown: xx%)";
			// 
			// tmrCdlRatios
			// 
			this.tmrCdlRatios.Interval = 300;
			this.tmrCdlRatios.Tick += new System.EventHandler(this.tmrCdlRatios_Tick);
			// 
			// frmDebugger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 612);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.statusStrip);
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(1000, 650);
			this.Name = "frmDebugger";
			this.Text = "Debugger";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDebugger_FormClosed);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.tlpTop.ResumeLayout(false);
			this.contextMenuCode.ResumeLayout(false);
			this.tableLayoutPanel10.ResumeLayout(false);
			this.grpWatch.ResumeLayout(false);
			this.grpBreakpoints.ResumeLayout(false);
			this.grpCallstack.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TableLayoutPanel tlpTop;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.GroupBox grpBreakpoints;
		private System.Windows.Forms.GroupBox grpWatch;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuContinue;
		private System.Windows.Forms.ToolStripMenuItem mnuBreak;
		private System.Windows.Forms.ToolStripMenuItem mnuStepInto;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOver;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOut;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuFind;
		private System.Windows.Forms.ToolStripMenuItem mnuFindNext;
		private System.Windows.Forms.ToolStripMenuItem mnuFindPrev;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuToggleBreakpoint;
		private ctrlDebuggerCode ctrlDebuggerCode;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuRunOneFrame;
		private System.Windows.Forms.ContextMenuStrip contextMenuCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowNextStatement;
		private System.Windows.Forms.ToolStripMenuItem mnuSetNextStatement;
		private ctrlWatch ctrlWatch;
		private ctrlConsoleStatus ctrlConsoleStatus;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private ctrlDebuggerCode ctrlDebuggerCodeSplit;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuSplitView;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuPpuViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuMemoryViewer;
		private Controls.ctrlBreakpoints ctrlBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem mnuGoTo;
		private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuIncreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuDecreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuResetFontSize;
		private System.Windows.Forms.GroupBox grpCallstack;
		private Controls.ctrlCallstack ctrlCallstack;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuCodeDataLogger;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblPrgAnalysis;
		private System.Windows.Forms.ToolStripStatusLabel lblPrgAnalysisResult;
		private System.Windows.Forms.Timer tmrCdlRatios;
		private System.Windows.Forms.ToolStripStatusLabel lblChrAnalysis;
		private System.Windows.Forms.ToolStripStatusLabel lblChrAnalysisResult;
		private System.Windows.Forms.ToolStripMenuItem autoLoadsaveCDLFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuLoadCdlFile;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAsCdlFile;
		private System.Windows.Forms.ToolStripMenuItem mnuResetCdlLog;
		private System.Windows.Forms.ToolStripMenuItem generateROMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveStrippedDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveUnusedDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableEnableBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToAddress;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToIrqHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToNmiHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToResetHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuTraceLogger;
	}
}