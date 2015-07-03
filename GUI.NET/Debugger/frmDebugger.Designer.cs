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
			this.grpBreakpoints = new System.Windows.Forms.GroupBox();
			this.lstBreakpoints = new System.Windows.Forms.ListView();
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuBreakpoints = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.grpWatch = new System.Windows.Forms.GroupBox();
			this.ctrlWatch = new Mesen.GUI.Debugger.ctrlWatch();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSplitView = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuContinue = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepInto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOver = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOut = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRunOneFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.tlpTop.SuspendLayout();
			this.contextMenuCode.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.grpBreakpoints.SuspendLayout();
			this.contextMenuBreakpoints.SuspendLayout();
			this.grpWatch.SuspendLayout();
			this.menuStrip.SuspendLayout();
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
			this.splitContainer.Size = new System.Drawing.Size(984, 588);
			this.splitContainer.SplitterDistance = 422;
			this.splitContainer.TabIndex = 1;
			// 
			// tlpTop
			// 
			this.tlpTop.ColumnCount = 3;
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTop.Controls.Add(this.ctrlDebuggerCode, 0, 0);
			this.tlpTop.Controls.Add(this.ctrlConsoleStatus, 2, 0);
			this.tlpTop.Controls.Add(this.ctrlDebuggerCodeSplit, 1, 0);
			this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTop.Location = new System.Drawing.Point(0, 0);
			this.tlpTop.Name = "tlpTop";
			this.tlpTop.RowCount = 1;
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.Size = new System.Drawing.Size(984, 422);
			this.tlpTop.TabIndex = 2;
			// 
			// ctrlDebuggerCode
			// 
			this.ctrlDebuggerCode.Code = null;
			this.ctrlDebuggerCode.ContextMenuStrip = this.contextMenuCode;
			this.ctrlDebuggerCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCode.Location = new System.Drawing.Point(3, 3);
			this.ctrlDebuggerCode.Name = "ctrlDebuggerCode";
			this.ctrlDebuggerCode.Size = new System.Drawing.Size(270, 416);
			this.ctrlDebuggerCode.TabIndex = 2;
			this.ctrlDebuggerCode.OnWatchAdded += new Mesen.GUI.Debugger.ctrlDebuggerCode.WatchAddedEventHandler(this.ctrlDebuggerCode_OnWatchAdded);
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
			this.ctrlDebuggerCodeSplit.Location = new System.Drawing.Point(279, 3);
			this.ctrlDebuggerCodeSplit.Name = "ctrlDebuggerCodeSplit";
			this.ctrlDebuggerCodeSplit.Size = new System.Drawing.Size(270, 416);
			this.ctrlDebuggerCodeSplit.TabIndex = 4;
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 2;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.Controls.Add(this.grpBreakpoints, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.grpWatch, 0, 0);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 1;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 162F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 162F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(984, 162);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// grpBreakpoints
			// 
			this.grpBreakpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpBreakpoints.Controls.Add(this.lstBreakpoints);
			this.grpBreakpoints.Location = new System.Drawing.Point(495, 3);
			this.grpBreakpoints.Name = "grpBreakpoints";
			this.grpBreakpoints.Size = new System.Drawing.Size(486, 156);
			this.grpBreakpoints.TabIndex = 3;
			this.grpBreakpoints.TabStop = false;
			this.grpBreakpoints.Text = "Breakpoints";
			// 
			// lstBreakpoints
			// 
			this.lstBreakpoints.CheckBoxes = true;
			this.lstBreakpoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
			this.lstBreakpoints.ContextMenuStrip = this.contextMenuBreakpoints;
			this.lstBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstBreakpoints.FullRowSelect = true;
			this.lstBreakpoints.GridLines = true;
			this.lstBreakpoints.LabelEdit = true;
			this.lstBreakpoints.Location = new System.Drawing.Point(3, 16);
			this.lstBreakpoints.Name = "lstBreakpoints";
			this.lstBreakpoints.Size = new System.Drawing.Size(480, 137);
			this.lstBreakpoints.TabIndex = 5;
			this.lstBreakpoints.UseCompatibleStateImageBehavior = false;
			this.lstBreakpoints.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Address";
			this.columnHeader3.Width = 59;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Breakpoint Type";
			this.columnHeader4.Width = 100;
			// 
			// contextMenuBreakpoints
			// 
			this.contextMenuBreakpoints.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemoveBreakpoint,
            this.mnuDisableBreakpoint,
            this.mnuAddBreakpoint});
			this.contextMenuBreakpoints.Name = "contextMenuWatch";
			this.contextMenuBreakpoints.Size = new System.Drawing.Size(173, 70);
			// 
			// mnuRemoveBreakpoint
			// 
			this.mnuRemoveBreakpoint.Name = "mnuRemoveBreakpoint";
			this.mnuRemoveBreakpoint.Size = new System.Drawing.Size(172, 22);
			this.mnuRemoveBreakpoint.Text = "Remove";
			// 
			// mnuDisableBreakpoint
			// 
			this.mnuDisableBreakpoint.Name = "mnuDisableBreakpoint";
			this.mnuDisableBreakpoint.Size = new System.Drawing.Size(172, 22);
			this.mnuDisableBreakpoint.Text = "Disable Breakpoint";
			// 
			// mnuAddBreakpoint
			// 
			this.mnuAddBreakpoint.Name = "mnuAddBreakpoint";
			this.mnuAddBreakpoint.Size = new System.Drawing.Size(172, 22);
			this.mnuAddBreakpoint.Text = "Add...";
			// 
			// grpWatch
			// 
			this.grpWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpWatch.Controls.Add(this.ctrlWatch);
			this.grpWatch.Location = new System.Drawing.Point(3, 3);
			this.grpWatch.Name = "grpWatch";
			this.grpWatch.Size = new System.Drawing.Size(486, 156);
			this.grpWatch.TabIndex = 2;
			this.grpWatch.TabStop = false;
			this.grpWatch.Text = "Watch";
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(3, 16);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(480, 137);
			this.ctrlWatch.TabIndex = 0;
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuView,
            this.debugToolStripMenuItem,
            this.searchToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(984, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClose});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuClose
			// 
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(103, 22);
			this.mnuClose.Text = "Close";
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSplitView});
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
			this.mnuContinue.Size = new System.Drawing.Size(190, 22);
			this.mnuContinue.Text = "Continue";
			this.mnuContinue.Click += new System.EventHandler(this.mnuContinue_Click);
			// 
			// mnuBreak
			// 
			this.mnuBreak.Name = "mnuBreak";
			this.mnuBreak.ShortcutKeyDisplayString = "Ctrl+Alt+Break";
			this.mnuBreak.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Cancel)));
			this.mnuBreak.Size = new System.Drawing.Size(190, 22);
			this.mnuBreak.Text = "Break";
			this.mnuBreak.Click += new System.EventHandler(this.mnuBreak_Click);
			// 
			// mnuStepInto
			// 
			this.mnuStepInto.Name = "mnuStepInto";
			this.mnuStepInto.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mnuStepInto.Size = new System.Drawing.Size(190, 22);
			this.mnuStepInto.Text = "Step Into";
			this.mnuStepInto.Click += new System.EventHandler(this.mnuStepInto_Click);
			// 
			// mnuStepOver
			// 
			this.mnuStepOver.Name = "mnuStepOver";
			this.mnuStepOver.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.mnuStepOver.Size = new System.Drawing.Size(190, 22);
			this.mnuStepOver.Text = "Step Over";
			this.mnuStepOver.Click += new System.EventHandler(this.mnuStepOver_Click);
			// 
			// mnuStepOut
			// 
			this.mnuStepOut.Name = "mnuStepOut";
			this.mnuStepOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
			this.mnuStepOut.Size = new System.Drawing.Size(190, 22);
			this.mnuStepOut.Text = "Step Out";
			this.mnuStepOut.Click += new System.EventHandler(this.mnuStepOut_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(187, 6);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(190, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			this.mnuToggleBreakpoint.Click += new System.EventHandler(this.mnuToggleBreakpoint_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(187, 6);
			// 
			// mnuRunOneFrame
			// 
			this.mnuRunOneFrame.Name = "mnuRunOneFrame";
			this.mnuRunOneFrame.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.mnuRunOneFrame.Size = new System.Drawing.Size(190, 22);
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
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.mnuFindPrev.Size = new System.Drawing.Size(196, 22);
			this.mnuFindPrev.Text = "Find Previous";
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.mnuGoTo.Size = new System.Drawing.Size(196, 22);
			this.mnuGoTo.Text = "Go to...";
			// 
			// frmDebugger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 612);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.menuStrip);
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
			this.grpBreakpoints.ResumeLayout(false);
			this.contextMenuBreakpoints.ResumeLayout(false);
			this.grpWatch.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TableLayoutPanel tlpTop;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.GroupBox grpBreakpoints;
		private System.Windows.Forms.ListView lstBreakpoints;
		private System.Windows.Forms.GroupBox grpWatch;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuAddBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuContinue;
		private System.Windows.Forms.ToolStripMenuItem mnuBreak;
		private System.Windows.Forms.ToolStripMenuItem mnuStepInto;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOver;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOut;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuFind;
		private System.Windows.Forms.ToolStripMenuItem mnuFindNext;
		private System.Windows.Forms.ToolStripMenuItem mnuFindPrev;
		private System.Windows.Forms.ToolStripMenuItem mnuGoTo;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
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
	}
}