using Mesen.GUI.Debugger.Controls;

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
			this.tmrCdlRatios = new System.Windows.Forms.Timer(this.components);
			this.splitContainer = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.ctrlSplitContainerTop = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlDebuggerCode = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.ctrlConsoleStatus = new Mesen.GUI.Debugger.ctrlConsoleStatus();
			this.ctrlDebuggerCodeSplit = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.tlpFunctionLabelLists = new System.Windows.Forms.TableLayoutPanel();
			this.grpLabels = new System.Windows.Forms.GroupBox();
			this.ctrlLabelList = new Mesen.GUI.Debugger.Controls.ctrlLabelList();
			this.grpFunctions = new System.Windows.Forms.GroupBox();
			this.ctrlFunctionList = new Mesen.GUI.Debugger.Controls.ctrlFunctionList();
			this.picWatchHelp = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.grpWatch = new System.Windows.Forms.GroupBox();
			this.ctrlWatch = new Mesen.GUI.Debugger.ctrlWatch();
			this.grpBreakpoints = new System.Windows.Forms.GroupBox();
			this.ctrlBreakpoints = new Mesen.GUI.Debugger.Controls.ctrlBreakpoints();
			this.grpCallstack = new System.Windows.Forms.GroupBox();
			this.ctrlCallstack = new Mesen.GUI.Debugger.Controls.ctrlCallstack();
			this.menuStrip = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveRom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveRomAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAsIps = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRevertChanges = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuResetWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoLoadDbgFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoLoadCdlFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableDefaultLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCode = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuContinue = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepInto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOver = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepBack = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableEnableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRunPpuCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunScanline = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunOneFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakIn = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindAllOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToNmiHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToProgramCount = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassemblyOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassemble = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleVerifiedCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleVerifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleUnidentifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowDisassembledCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowVerifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowUnidentifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuDisplayOpCodesInLowerCase = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowEffectiveAddresses = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowMemoryValues = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnUnofficialOpcodes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnBrk = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnCrash = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnDebuggerFocus = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBringToFrontOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBringToFrontOnPause = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuConfigureColors = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSplitView = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSelectFont = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowToolbar = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCpuMemoryMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowPpuMemoryMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowFunctionLabelLists = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowBottomPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowCodePreview = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowOpCodeTooltips = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPpuPartialDraw = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuShowPreviousFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshWatchWhileRunning = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuApuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAssembler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEventViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScriptWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTraceLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditHeader = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCodeDataLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.autoLoadsaveCDLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuLoadCdlFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAsCdlFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetCdlLog = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCdlGenerateRom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCdlStripUnusedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCdlStripUsedData = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblPrgAnalysis = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblPrgAnalysisResult = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblChrAnalysis = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblChrAnalysisResult = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCyclesElapsedCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCyclesElapsed = new System.Windows.Forms.ToolStripStatusLabel();
			this.ctrlPpuMemoryMapping = new Mesen.GUI.Debugger.Controls.ctrlMemoryMapping();
			this.ctrlCpuMemoryMapping = new Mesen.GUI.Debugger.Controls.ctrlMemoryMapping();
			this.tsToolbar = new Mesen.GUI.Controls.ctrlMesenToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplitContainerTop)).BeginInit();
			this.ctrlSplitContainerTop.Panel1.SuspendLayout();
			this.ctrlSplitContainerTop.Panel2.SuspendLayout();
			this.ctrlSplitContainerTop.SuspendLayout();
			this.tlpTop.SuspendLayout();
			this.tlpFunctionLabelLists.SuspendLayout();
			this.grpLabels.SuspendLayout();
			this.grpFunctions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).BeginInit();
			this.tableLayoutPanel10.SuspendLayout();
			this.grpWatch.SuspendLayout();
			this.grpBreakpoints.SuspendLayout();
			this.grpCallstack.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrCdlRatios
			// 
			this.tmrCdlRatios.Interval = 300;
			this.tmrCdlRatios.Tick += new System.EventHandler(this.tmrCdlRatios_Tick);
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
			this.splitContainer.Panel1.Controls.Add(this.ctrlSplitContainerTop);
			this.splitContainer.Panel1MinSize = 400;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.picWatchHelp);
			this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel10);
			this.splitContainer.Panel2MinSize = 100;
			this.splitContainer.Size = new System.Drawing.Size(1172, 573);
			this.splitContainer.SplitterDistance = 413;
			this.splitContainer.SplitterWidth = 7;
			this.splitContainer.TabIndex = 1;
			this.splitContainer.TabStop = false;
			this.splitContainer.PanelCollapsed += new System.EventHandler(this.splitContainer_PanelCollapsed);
			this.splitContainer.PanelExpanded += new System.EventHandler(this.splitContainer_PanelExpanded);
			// 
			// ctrlSplitContainerTop
			// 
			this.ctrlSplitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSplitContainerTop.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.ctrlSplitContainerTop.Location = new System.Drawing.Point(0, 0);
			this.ctrlSplitContainerTop.Name = "ctrlSplitContainerTop";
			// 
			// ctrlSplitContainerTop.Panel1
			// 
			this.ctrlSplitContainerTop.Panel1.Controls.Add(this.tlpTop);
			this.ctrlSplitContainerTop.Panel1MinSize = 750;
			// 
			// ctrlSplitContainerTop.Panel2
			// 
			this.ctrlSplitContainerTop.Panel2.Controls.Add(this.tlpFunctionLabelLists);
			this.ctrlSplitContainerTop.Panel2MinSize = 150;
			this.ctrlSplitContainerTop.Size = new System.Drawing.Size(1172, 413);
			this.ctrlSplitContainerTop.SplitterDistance = 750;
			this.ctrlSplitContainerTop.SplitterWidth = 7;
			this.ctrlSplitContainerTop.TabIndex = 3;
			this.ctrlSplitContainerTop.PanelCollapsed += new System.EventHandler(this.ctrlSplitContainerTop_PanelCollapsed);
			this.ctrlSplitContainerTop.PanelExpanded += new System.EventHandler(this.ctrlSplitContainerTop_PanelExpanded);
			// 
			// tlpTop
			// 
			this.tlpTop.ColumnCount = 4;
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTop.Controls.Add(this.ctrlDebuggerCode, 0, 0);
			this.tlpTop.Controls.Add(this.ctrlConsoleStatus, 2, 0);
			this.tlpTop.Controls.Add(this.ctrlDebuggerCodeSplit, 1, 0);
			this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTop.Location = new System.Drawing.Point(0, 0);
			this.tlpTop.Name = "tlpTop";
			this.tlpTop.RowCount = 1;
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.Size = new System.Drawing.Size(750, 413);
			this.tlpTop.TabIndex = 2;
			// 
			// ctrlDebuggerCode
			// 
			this.ctrlDebuggerCode.Code = null;
			this.ctrlDebuggerCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCode.HideSelection = false;
			this.ctrlDebuggerCode.Location = new System.Drawing.Point(3, 3);
			this.ctrlDebuggerCode.Name = "ctrlDebuggerCode";
			this.ctrlDebuggerCode.ShowMemoryValues = false;
			this.ctrlDebuggerCode.ShowScrollbars = true;
			this.ctrlDebuggerCode.Size = new System.Drawing.Size(286, 407);
			this.ctrlDebuggerCode.TabIndex = 2;
			this.ctrlDebuggerCode.TextZoom = 100;
			this.ctrlDebuggerCode.OnEditCode += new Mesen.GUI.Debugger.ctrlDebuggerCode.AssemblerEventHandler(this.ctrlDebuggerCode_OnEditCode);
			this.ctrlDebuggerCode.OnSetNextStatement += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnSetNextStatement);
			this.ctrlDebuggerCode.OnScrollToAddress += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnScrollToAddress);
			this.ctrlDebuggerCode.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// ctrlConsoleStatus
			// 
			this.ctrlConsoleStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlConsoleStatus.Location = new System.Drawing.Point(292, 0);
			this.ctrlConsoleStatus.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlConsoleStatus.Name = "ctrlConsoleStatus";
			this.ctrlConsoleStatus.Size = new System.Drawing.Size(458, 413);
			this.ctrlConsoleStatus.TabIndex = 3;
			this.ctrlConsoleStatus.OnGotoLocation += new System.EventHandler(this.ctrlConsoleStatus_OnGotoLocation);
			// 
			// ctrlDebuggerCodeSplit
			// 
			this.ctrlDebuggerCodeSplit.Code = null;
			this.ctrlDebuggerCodeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCodeSplit.HideSelection = false;
			this.ctrlDebuggerCodeSplit.Location = new System.Drawing.Point(295, 3);
			this.ctrlDebuggerCodeSplit.Name = "ctrlDebuggerCodeSplit";
			this.ctrlDebuggerCodeSplit.ShowMemoryValues = false;
			this.ctrlDebuggerCodeSplit.ShowScrollbars = true;
			this.ctrlDebuggerCodeSplit.Size = new System.Drawing.Size(1, 407);
			this.ctrlDebuggerCodeSplit.TabIndex = 4;
			this.ctrlDebuggerCodeSplit.TextZoom = 100;
			this.ctrlDebuggerCodeSplit.Visible = false;
			this.ctrlDebuggerCodeSplit.OnEditCode += new Mesen.GUI.Debugger.ctrlDebuggerCode.AssemblerEventHandler(this.ctrlDebuggerCode_OnEditCode);
			this.ctrlDebuggerCodeSplit.OnSetNextStatement += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnSetNextStatement);
			this.ctrlDebuggerCodeSplit.OnScrollToAddress += new Mesen.GUI.Debugger.ctrlDebuggerCode.AddressEventHandler(this.ctrlDebuggerCode_OnScrollToAddress);
			this.ctrlDebuggerCodeSplit.Enter += new System.EventHandler(this.ctrlDebuggerCodeSplit_Enter);
			// 
			// tlpFunctionLabelLists
			// 
			this.tlpFunctionLabelLists.ColumnCount = 1;
			this.tlpFunctionLabelLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFunctionLabelLists.Controls.Add(this.grpLabels, 0, 1);
			this.tlpFunctionLabelLists.Controls.Add(this.grpFunctions, 0, 0);
			this.tlpFunctionLabelLists.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpFunctionLabelLists.Location = new System.Drawing.Point(0, 0);
			this.tlpFunctionLabelLists.Margin = new System.Windows.Forms.Padding(0);
			this.tlpFunctionLabelLists.Name = "tlpFunctionLabelLists";
			this.tlpFunctionLabelLists.RowCount = 2;
			this.tlpFunctionLabelLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFunctionLabelLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFunctionLabelLists.Size = new System.Drawing.Size(415, 413);
			this.tlpFunctionLabelLists.TabIndex = 5;
			// 
			// grpLabels
			// 
			this.grpLabels.Controls.Add(this.ctrlLabelList);
			this.grpLabels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpLabels.Location = new System.Drawing.Point(3, 209);
			this.grpLabels.Name = "grpLabels";
			this.grpLabels.Size = new System.Drawing.Size(409, 201);
			this.grpLabels.TabIndex = 6;
			this.grpLabels.TabStop = false;
			this.grpLabels.Text = "Labels";
			// 
			// ctrlLabelList
			// 
			this.ctrlLabelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLabelList.Location = new System.Drawing.Point(3, 16);
			this.ctrlLabelList.Name = "ctrlLabelList";
			this.ctrlLabelList.Size = new System.Drawing.Size(403, 182);
			this.ctrlLabelList.TabIndex = 0;
			this.ctrlLabelList.OnFindOccurrence += new System.EventHandler(this.ctrlLabelList_OnFindOccurrence);
			this.ctrlLabelList.OnLabelSelected += new System.EventHandler(this.ctrlLabelList_OnLabelSelected);
			// 
			// grpFunctions
			// 
			this.grpFunctions.Controls.Add(this.ctrlFunctionList);
			this.grpFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFunctions.Location = new System.Drawing.Point(3, 3);
			this.grpFunctions.Name = "grpFunctions";
			this.grpFunctions.Size = new System.Drawing.Size(409, 200);
			this.grpFunctions.TabIndex = 5;
			this.grpFunctions.TabStop = false;
			this.grpFunctions.Text = "Functions";
			// 
			// ctrlFunctionList
			// 
			this.ctrlFunctionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFunctionList.Location = new System.Drawing.Point(3, 16);
			this.ctrlFunctionList.Name = "ctrlFunctionList";
			this.ctrlFunctionList.Size = new System.Drawing.Size(403, 181);
			this.ctrlFunctionList.TabIndex = 0;
			this.ctrlFunctionList.OnFindOccurrence += new System.EventHandler(this.ctrlFunctionList_OnFindOccurrence);
			this.ctrlFunctionList.OnFunctionSelected += new System.EventHandler(this.ctrlFunctionList_OnFunctionSelected);
			// 
			// picWatchHelp
			// 
			this.picWatchHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picWatchHelp.Location = new System.Drawing.Point(50, 2);
			this.picWatchHelp.Name = "picWatchHelp";
			this.picWatchHelp.Size = new System.Drawing.Size(16, 16);
			this.picWatchHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWatchHelp.TabIndex = 1;
			this.picWatchHelp.TabStop = false;
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 3;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel10.Controls.Add(this.grpWatch, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.grpBreakpoints, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.grpCallstack, 2, 0);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 3;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.Size = new System.Drawing.Size(1172, 153);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// grpWatch
			// 
			this.grpWatch.Controls.Add(this.ctrlWatch);
			this.grpWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpWatch.Location = new System.Drawing.Point(3, 3);
			this.grpWatch.Name = "grpWatch";
			this.grpWatch.Size = new System.Drawing.Size(384, 147);
			this.grpWatch.TabIndex = 2;
			this.grpWatch.TabStop = false;
			this.grpWatch.Text = "Watch";
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(3, 16);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(378, 128);
			this.ctrlWatch.TabIndex = 0;
			// 
			// grpBreakpoints
			// 
			this.grpBreakpoints.Controls.Add(this.ctrlBreakpoints);
			this.grpBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpBreakpoints.Location = new System.Drawing.Point(393, 3);
			this.grpBreakpoints.Name = "grpBreakpoints";
			this.grpBreakpoints.Size = new System.Drawing.Size(384, 147);
			this.grpBreakpoints.TabIndex = 3;
			this.grpBreakpoints.TabStop = false;
			this.grpBreakpoints.Text = "Breakpoints";
			// 
			// ctrlBreakpoints
			// 
			this.ctrlBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlBreakpoints.Location = new System.Drawing.Point(3, 16);
			this.ctrlBreakpoints.Name = "ctrlBreakpoints";
			this.ctrlBreakpoints.Size = new System.Drawing.Size(378, 128);
			this.ctrlBreakpoints.TabIndex = 0;
			this.ctrlBreakpoints.BreakpointNavigation += new System.EventHandler(this.ctrlBreakpoints_BreakpointNavigation);
			// 
			// grpCallstack
			// 
			this.grpCallstack.Controls.Add(this.ctrlCallstack);
			this.grpCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCallstack.Location = new System.Drawing.Point(783, 3);
			this.grpCallstack.Name = "grpCallstack";
			this.grpCallstack.Size = new System.Drawing.Size(386, 147);
			this.grpCallstack.TabIndex = 4;
			this.grpCallstack.TabStop = false;
			this.grpCallstack.Text = "Call Stack";
			// 
			// ctrlCallstack
			// 
			this.ctrlCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCallstack.Location = new System.Drawing.Point(3, 16);
			this.ctrlCallstack.Name = "ctrlCallstack";
			this.ctrlCallstack.Size = new System.Drawing.Size(380, 128);
			this.ctrlCallstack.TabIndex = 0;
			this.ctrlCallstack.FunctionSelected += new System.EventHandler(this.ctrlCallstack_FunctionSelected);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuCode,
            this.debugToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.mnuOptions,
            this.toolsToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1172, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveRom,
            this.mnuSaveRomAs,
            this.mnuSaveAsIps,
            this.mnuRevertChanges,
            this.toolStripMenuItem14,
            this.mnuWorkspace,
            this.toolStripMenuItem3,
            this.mnuClose});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "File";
			this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
			// 
			// mnuSaveRom
			// 
			this.mnuSaveRom.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.mnuSaveRom.Name = "mnuSaveRom";
			this.mnuSaveRom.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSaveRom.Size = new System.Drawing.Size(208, 22);
			this.mnuSaveRom.Text = "Save ROM";
			this.mnuSaveRom.Click += new System.EventHandler(this.mnuSaveRom_Click);
			// 
			// mnuSaveRomAs
			// 
			this.mnuSaveRomAs.Name = "mnuSaveRomAs";
			this.mnuSaveRomAs.Size = new System.Drawing.Size(208, 22);
			this.mnuSaveRomAs.Text = "Save ROM as...";
			this.mnuSaveRomAs.Click += new System.EventHandler(this.mnuSaveRomAs_Click);
			// 
			// mnuSaveAsIps
			// 
			this.mnuSaveAsIps.Name = "mnuSaveAsIps";
			this.mnuSaveAsIps.Size = new System.Drawing.Size(208, 22);
			this.mnuSaveAsIps.Text = "Save edits as IPS";
			this.mnuSaveAsIps.Click += new System.EventHandler(this.mnuSaveAsIps_Click);
			// 
			// mnuRevertChanges
			// 
			this.mnuRevertChanges.Image = global::Mesen.GUI.Properties.Resources.Undo;
			this.mnuRevertChanges.Name = "mnuRevertChanges";
			this.mnuRevertChanges.Size = new System.Drawing.Size(208, 22);
			this.mnuRevertChanges.Text = "Revert PRG/CHR changes";
			this.mnuRevertChanges.Click += new System.EventHandler(this.mnuRevertChanges_Click);
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(205, 6);
			// 
			// mnuWorkspace
			// 
			this.mnuWorkspace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImportLabels,
            this.mnuExportLabels,
            this.toolStripMenuItem16,
            this.mnuResetWorkspace,
            this.toolStripMenuItem10,
            this.mnuAutoLoadDbgFiles,
            this.mnuAutoLoadCdlFiles,
            this.mnuDisableDefaultLabels});
			this.mnuWorkspace.Name = "mnuWorkspace";
			this.mnuWorkspace.Size = new System.Drawing.Size(208, 22);
			this.mnuWorkspace.Text = "Workspace";
			// 
			// mnuImportLabels
			// 
			this.mnuImportLabels.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuImportLabels.Name = "mnuImportLabels";
			this.mnuImportLabels.Size = new System.Drawing.Size(207, 22);
			this.mnuImportLabels.Text = "Import Labels";
			this.mnuImportLabels.Click += new System.EventHandler(this.mnuImportLabels_Click);
			// 
			// mnuExportLabels
			// 
			this.mnuExportLabels.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportLabels.Name = "mnuExportLabels";
			this.mnuExportLabels.Size = new System.Drawing.Size(207, 22);
			this.mnuExportLabels.Text = "Export Labels";
			this.mnuExportLabels.Click += new System.EventHandler(this.mnuExportLabels_Click);
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(204, 6);
			// 
			// mnuResetWorkspace
			// 
			this.mnuResetWorkspace.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuResetWorkspace.Name = "mnuResetWorkspace";
			this.mnuResetWorkspace.Size = new System.Drawing.Size(207, 22);
			this.mnuResetWorkspace.Text = "Reset Workspace";
			this.mnuResetWorkspace.Click += new System.EventHandler(this.mnuResetWorkspace_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(204, 6);
			// 
			// mnuAutoLoadDbgFiles
			// 
			this.mnuAutoLoadDbgFiles.CheckOnClick = true;
			this.mnuAutoLoadDbgFiles.Name = "mnuAutoLoadDbgFiles";
			this.mnuAutoLoadDbgFiles.Size = new System.Drawing.Size(207, 22);
			this.mnuAutoLoadDbgFiles.Text = "Auto-load DBG/MLB files";
			this.mnuAutoLoadDbgFiles.Click += new System.EventHandler(this.mnuAutoLoadDbgFiles_Click);
			// 
			// mnuAutoLoadCdlFiles
			// 
			this.mnuAutoLoadCdlFiles.CheckOnClick = true;
			this.mnuAutoLoadCdlFiles.Name = "mnuAutoLoadCdlFiles";
			this.mnuAutoLoadCdlFiles.Size = new System.Drawing.Size(207, 22);
			this.mnuAutoLoadCdlFiles.Text = "Auto-load CDL files";
			this.mnuAutoLoadCdlFiles.Click += new System.EventHandler(this.mnuAutoLoadCdlFiles_Click);
			// 
			// mnuDisableDefaultLabels
			// 
			this.mnuDisableDefaultLabels.CheckOnClick = true;
			this.mnuDisableDefaultLabels.Name = "mnuDisableDefaultLabels";
			this.mnuDisableDefaultLabels.Size = new System.Drawing.Size(207, 22);
			this.mnuDisableDefaultLabels.Text = "Disable default labels";
			this.mnuDisableDefaultLabels.Click += new System.EventHandler(this.mnuDisableDefaultLabels_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(208, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuCode
			// 
			this.mnuCode.Name = "mnuCode";
			this.mnuCode.Size = new System.Drawing.Size(47, 20);
			this.mnuCode.Text = "Code";
			this.mnuCode.DropDownClosed += new System.EventHandler(this.mnuCode_DropDownClosed);
			this.mnuCode.DropDownOpening += new System.EventHandler(this.mnuCode_DropDownOpening);
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContinue,
            this.mnuBreak,
            this.mnuStepInto,
            this.mnuStepOver,
            this.mnuStepOut,
            this.mnuStepBack,
            this.toolStripMenuItem1,
            this.mnuToggleBreakpoint,
            this.mnuDisableEnableBreakpoint,
            this.toolStripMenuItem2,
            this.mnuRunPpuCycle,
            this.mnuRunScanline,
            this.mnuRunOneFrame,
            this.toolStripMenuItem8,
            this.mnuBreakIn});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// mnuContinue
			// 
			this.mnuContinue.Enabled = false;
			this.mnuContinue.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.mnuContinue.Name = "mnuContinue";
			this.mnuContinue.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuContinue.Size = new System.Drawing.Size(258, 22);
			this.mnuContinue.Text = "Continue";
			this.mnuContinue.Click += new System.EventHandler(this.mnuContinue_Click);
			// 
			// mnuBreak
			// 
			this.mnuBreak.Enabled = false;
			this.mnuBreak.Image = global::Mesen.GUI.Properties.Resources.Pause;
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
			this.mnuStepInto.Image = global::Mesen.GUI.Properties.Resources.StepInto;
			this.mnuStepInto.Name = "mnuStepInto";
			this.mnuStepInto.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mnuStepInto.Size = new System.Drawing.Size(258, 22);
			this.mnuStepInto.Text = "Step Into";
			this.mnuStepInto.Click += new System.EventHandler(this.mnuStepInto_Click);
			// 
			// mnuStepOver
			// 
			this.mnuStepOver.Image = global::Mesen.GUI.Properties.Resources.StepOver;
			this.mnuStepOver.Name = "mnuStepOver";
			this.mnuStepOver.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.mnuStepOver.Size = new System.Drawing.Size(258, 22);
			this.mnuStepOver.Text = "Step Over";
			this.mnuStepOver.Click += new System.EventHandler(this.mnuStepOver_Click);
			// 
			// mnuStepOut
			// 
			this.mnuStepOut.Image = global::Mesen.GUI.Properties.Resources.StepOut;
			this.mnuStepOut.Name = "mnuStepOut";
			this.mnuStepOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
			this.mnuStepOut.Size = new System.Drawing.Size(258, 22);
			this.mnuStepOut.Text = "Step Out";
			this.mnuStepOut.Click += new System.EventHandler(this.mnuStepOut_Click);
			// 
			// mnuStepBack
			// 
			this.mnuStepBack.Image = global::Mesen.GUI.Properties.Resources.StepBack;
			this.mnuStepBack.Name = "mnuStepBack";
			this.mnuStepBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F10)));
			this.mnuStepBack.Size = new System.Drawing.Size(258, 22);
			this.mnuStepBack.Text = "Step Back";
			this.mnuStepBack.Click += new System.EventHandler(this.mnuStepBack_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Breakpoint;
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(258, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			this.mnuToggleBreakpoint.Click += new System.EventHandler(this.mnuToggleBreakpoint_Click);
			// 
			// mnuDisableEnableBreakpoint
			// 
			this.mnuDisableEnableBreakpoint.Image = global::Mesen.GUI.Properties.Resources.BreakpointDisabled;
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
			// mnuRunPpuCycle
			// 
			this.mnuRunPpuCycle.Image = global::Mesen.GUI.Properties.Resources.RunPpuCycle;
			this.mnuRunPpuCycle.Name = "mnuRunPpuCycle";
			this.mnuRunPpuCycle.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.mnuRunPpuCycle.Size = new System.Drawing.Size(258, 22);
			this.mnuRunPpuCycle.Text = "Run one PPU cycle";
			this.mnuRunPpuCycle.Click += new System.EventHandler(this.mnuRunPpuCycle_Click);
			// 
			// mnuRunScanline
			// 
			this.mnuRunScanline.Image = global::Mesen.GUI.Properties.Resources.RunPpuScanline;
			this.mnuRunScanline.Name = "mnuRunScanline";
			this.mnuRunScanline.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.mnuRunScanline.Size = new System.Drawing.Size(258, 22);
			this.mnuRunScanline.Text = "Run one scanline";
			this.mnuRunScanline.Click += new System.EventHandler(this.mnuRunScanline_Click);
			// 
			// mnuRunOneFrame
			// 
			this.mnuRunOneFrame.Image = global::Mesen.GUI.Properties.Resources.RunPpuFrame;
			this.mnuRunOneFrame.Name = "mnuRunOneFrame";
			this.mnuRunOneFrame.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.mnuRunOneFrame.Size = new System.Drawing.Size(258, 22);
			this.mnuRunOneFrame.Text = "Run one frame";
			this.mnuRunOneFrame.Click += new System.EventHandler(this.mnuRunOneFrame_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuBreakIn
			// 
			this.mnuBreakIn.Name = "mnuBreakIn";
			this.mnuBreakIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.mnuBreakIn.Size = new System.Drawing.Size(258, 22);
			this.mnuBreakIn.Text = "Break in...";
			this.mnuBreakIn.Click += new System.EventHandler(this.mnuBreakIn_Click);
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFind,
            this.mnuFindNext,
            this.mnuFindPrev,
            this.toolStripMenuItem9,
            this.mnuFindAllOccurrences,
            this.mnuGoTo});
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.searchToolStripMenuItem.Text = "Search";
			// 
			// mnuFind
			// 
			this.mnuFind.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFind.Name = "mnuFind";
			this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.mnuFind.Size = new System.Drawing.Size(255, 22);
			this.mnuFind.Text = "Find...";
			this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
			// 
			// mnuFindNext
			// 
			this.mnuFindNext.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuFindNext.Name = "mnuFindNext";
			this.mnuFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.mnuFindNext.Size = new System.Drawing.Size(255, 22);
			this.mnuFindNext.Text = "Find Next";
			this.mnuFindNext.Click += new System.EventHandler(this.mnuFindNext_Click);
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Image = global::Mesen.GUI.Properties.Resources.PreviousArrow;
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.mnuFindPrev.Size = new System.Drawing.Size(255, 22);
			this.mnuFindPrev.Text = "Find Previous";
			this.mnuFindPrev.Click += new System.EventHandler(this.mnuFindPrev_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(252, 6);
			// 
			// mnuFindAllOccurrences
			// 
			this.mnuFindAllOccurrences.Name = "mnuFindAllOccurrences";
			this.mnuFindAllOccurrences.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
			this.mnuFindAllOccurrences.Size = new System.Drawing.Size(255, 22);
			this.mnuFindAllOccurrences.Text = "Find All Occurrences";
			this.mnuFindAllOccurrences.Click += new System.EventHandler(this.mnuFindAllOccurrences_Click);
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToAddress,
            this.toolStripMenuItem22,
            this.mnuGoToIrqHandler,
            this.mnuGoToNmiHandler,
            this.mnuGoToResetHandler,
            this.toolStripMenuItem23,
            this.mnuGoToProgramCount});
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.Size = new System.Drawing.Size(255, 22);
			this.mnuGoTo.Text = "Go To...";
			// 
			// mnuGoToAddress
			// 
			this.mnuGoToAddress.Name = "mnuGoToAddress";
			this.mnuGoToAddress.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.mnuGoToAddress.Size = new System.Drawing.Size(201, 22);
			this.mnuGoToAddress.Text = "Address";
			this.mnuGoToAddress.Click += new System.EventHandler(this.mnuGoToAddress_Click);
			// 
			// toolStripMenuItem22
			// 
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			this.toolStripMenuItem22.Size = new System.Drawing.Size(198, 6);
			// 
			// mnuGoToIrqHandler
			// 
			this.mnuGoToIrqHandler.Name = "mnuGoToIrqHandler";
			this.mnuGoToIrqHandler.Size = new System.Drawing.Size(201, 22);
			this.mnuGoToIrqHandler.Text = "IRQ Handler";
			this.mnuGoToIrqHandler.Click += new System.EventHandler(this.mnuGoToIrqHandler_Click);
			// 
			// mnuGoToNmiHandler
			// 
			this.mnuGoToNmiHandler.Name = "mnuGoToNmiHandler";
			this.mnuGoToNmiHandler.Size = new System.Drawing.Size(201, 22);
			this.mnuGoToNmiHandler.Text = "NMI Handler";
			this.mnuGoToNmiHandler.Click += new System.EventHandler(this.mnuGoToNmiHandler_Click);
			// 
			// mnuGoToResetHandler
			// 
			this.mnuGoToResetHandler.Name = "mnuGoToResetHandler";
			this.mnuGoToResetHandler.Size = new System.Drawing.Size(201, 22);
			this.mnuGoToResetHandler.Text = "Reset Handler";
			this.mnuGoToResetHandler.Click += new System.EventHandler(this.mnuGoToResetHandler_Click);
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(198, 6);
			// 
			// mnuGoToProgramCount
			// 
			this.mnuGoToProgramCount.Name = "mnuGoToProgramCount";
			this.mnuGoToProgramCount.ShortcutKeyDisplayString = "Alt+*";
			this.mnuGoToProgramCount.Size = new System.Drawing.Size(201, 22);
			this.mnuGoToProgramCount.Text = "Program Counter";
			this.mnuGoToProgramCount.Click += new System.EventHandler(this.mnuGoToProgramCount_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassemblyOptions,
            this.mnuBreakOptions,
            this.toolStripMenuItem12,
            this.mnuConfigureColors,
            this.mnuSplitView,
            this.fontSizeToolStripMenuItem,
            this.toolStripMenuItem11,
            this.mnuShowToolbar,
            this.mnuShowCpuMemoryMapping,
            this.mnuShowPpuMemoryMapping,
            this.mnuShowFunctionLabelLists,
            this.mnuShowBottomPanel,
            this.toolStripMenuItem18,
            this.mnuShowCodePreview,
            this.mnuShowOpCodeTooltips,
            this.toolStripMenuItem6,
            this.mnuPpuPartialDraw,
            this.mnuPpuShowPreviousFrame,
            this.toolStripMenuItem19,
            this.mnuRefreshWatchWhileRunning});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(61, 20);
			this.mnuOptions.Text = "Options";
			// 
			// mnuDisassemblyOptions
			// 
			this.mnuDisassemblyOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassemble,
            this.mnuShow,
            this.toolStripMenuItem7,
            this.mnuDisplayOpCodesInLowerCase,
            this.mnuShowEffectiveAddresses,
            this.mnuShowMemoryValues});
			this.mnuDisassemblyOptions.Name = "mnuDisassemblyOptions";
			this.mnuDisassemblyOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuDisassemblyOptions.Text = "Disassembly Options";
			// 
			// mnuDisassemble
			// 
			this.mnuDisassemble.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassembleVerifiedCode,
            this.mnuDisassembleVerifiedData,
            this.mnuDisassembleUnidentifiedData});
			this.mnuDisassemble.Name = "mnuDisassemble";
			this.mnuDisassemble.Size = new System.Drawing.Size(236, 22);
			this.mnuDisassemble.Text = "Disassemble...";
			// 
			// mnuDisassembleVerifiedCode
			// 
			this.mnuDisassembleVerifiedCode.Checked = true;
			this.mnuDisassembleVerifiedCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuDisassembleVerifiedCode.Enabled = false;
			this.mnuDisassembleVerifiedCode.Name = "mnuDisassembleVerifiedCode";
			this.mnuDisassembleVerifiedCode.Size = new System.Drawing.Size(250, 22);
			this.mnuDisassembleVerifiedCode.Text = "Verified Code";
			// 
			// mnuDisassembleVerifiedData
			// 
			this.mnuDisassembleVerifiedData.CheckOnClick = true;
			this.mnuDisassembleVerifiedData.Name = "mnuDisassembleVerifiedData";
			this.mnuDisassembleVerifiedData.Size = new System.Drawing.Size(250, 22);
			this.mnuDisassembleVerifiedData.Text = "Verified Data (not recommended)";
			this.mnuDisassembleVerifiedData.Click += new System.EventHandler(this.mnuDisassembleVerifiedData_Click);
			// 
			// mnuDisassembleUnidentifiedData
			// 
			this.mnuDisassembleUnidentifiedData.CheckOnClick = true;
			this.mnuDisassembleUnidentifiedData.Name = "mnuDisassembleUnidentifiedData";
			this.mnuDisassembleUnidentifiedData.Size = new System.Drawing.Size(250, 22);
			this.mnuDisassembleUnidentifiedData.Text = "Unidentified Code/Data";
			this.mnuDisassembleUnidentifiedData.Click += new System.EventHandler(this.mnuDisassembleUnidentifiedData_Click);
			// 
			// mnuShow
			// 
			this.mnuShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowDisassembledCode,
            this.mnuShowVerifiedData,
            this.mnuShowUnidentifiedData});
			this.mnuShow.Name = "mnuShow";
			this.mnuShow.Size = new System.Drawing.Size(236, 22);
			this.mnuShow.Text = "Show...";
			// 
			// mnuShowDisassembledCode
			// 
			this.mnuShowDisassembledCode.Checked = true;
			this.mnuShowDisassembledCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuShowDisassembledCode.Enabled = false;
			this.mnuShowDisassembledCode.Name = "mnuShowDisassembledCode";
			this.mnuShowDisassembledCode.Size = new System.Drawing.Size(235, 22);
			this.mnuShowDisassembledCode.Text = "Disassembled Code";
			// 
			// mnuShowVerifiedData
			// 
			this.mnuShowVerifiedData.CheckOnClick = true;
			this.mnuShowVerifiedData.Image = global::Mesen.GUI.Properties.Resources.VerifiedData;
			this.mnuShowVerifiedData.Name = "mnuShowVerifiedData";
			this.mnuShowVerifiedData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
			this.mnuShowVerifiedData.Size = new System.Drawing.Size(235, 22);
			this.mnuShowVerifiedData.Text = "Verified Data";
			this.mnuShowVerifiedData.Click += new System.EventHandler(this.mnuShowVerifiedData_Click);
			// 
			// mnuShowUnidentifiedData
			// 
			this.mnuShowUnidentifiedData.CheckOnClick = true;
			this.mnuShowUnidentifiedData.Image = global::Mesen.GUI.Properties.Resources.UnidentifiedData;
			this.mnuShowUnidentifiedData.Name = "mnuShowUnidentifiedData";
			this.mnuShowUnidentifiedData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
			this.mnuShowUnidentifiedData.Size = new System.Drawing.Size(235, 22);
			this.mnuShowUnidentifiedData.Text = "Unidentified Code/Data";
			this.mnuShowUnidentifiedData.Click += new System.EventHandler(this.mnuShowUnidentifiedData_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(233, 6);
			// 
			// mnuDisplayOpCodesInLowerCase
			// 
			this.mnuDisplayOpCodesInLowerCase.CheckOnClick = true;
			this.mnuDisplayOpCodesInLowerCase.Name = "mnuDisplayOpCodesInLowerCase";
			this.mnuDisplayOpCodesInLowerCase.Size = new System.Drawing.Size(236, 22);
			this.mnuDisplayOpCodesInLowerCase.Text = "Display OP codes in lower case";
			this.mnuDisplayOpCodesInLowerCase.Click += new System.EventHandler(this.mnuDisplayOpCodesInLowerCase_Click);
			// 
			// mnuShowEffectiveAddresses
			// 
			this.mnuShowEffectiveAddresses.CheckOnClick = true;
			this.mnuShowEffectiveAddresses.Name = "mnuShowEffectiveAddresses";
			this.mnuShowEffectiveAddresses.Size = new System.Drawing.Size(236, 22);
			this.mnuShowEffectiveAddresses.Text = "Show Effective Addresses";
			this.mnuShowEffectiveAddresses.Click += new System.EventHandler(this.mnuShowEffectiveAddresses_Click);
			// 
			// mnuShowMemoryValues
			// 
			this.mnuShowMemoryValues.CheckOnClick = true;
			this.mnuShowMemoryValues.Name = "mnuShowMemoryValues";
			this.mnuShowMemoryValues.Size = new System.Drawing.Size(236, 22);
			this.mnuShowMemoryValues.Text = "Show Memory Values";
			this.mnuShowMemoryValues.Click += new System.EventHandler(this.mnuShowMemoryValues_Click);
			// 
			// mnuBreakOptions
			// 
			this.mnuBreakOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBreakOnReset,
            this.mnuBreakOnUnofficialOpcodes,
            this.mnuBreakOnBrk,
            this.mnuBreakOnCrash,
            this.toolStripMenuItem15,
            this.mnuBreakOnOpen,
            this.mnuBreakOnDebuggerFocus,
            this.toolStripMenuItem20,
            this.mnuBringToFrontOnBreak,
            this.mnuBringToFrontOnPause});
			this.mnuBreakOptions.Name = "mnuBreakOptions";
			this.mnuBreakOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuBreakOptions.Text = "Break Options";
			// 
			// mnuBreakOnReset
			// 
			this.mnuBreakOnReset.CheckOnClick = true;
			this.mnuBreakOnReset.Name = "mnuBreakOnReset";
			this.mnuBreakOnReset.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnReset.Text = "Break on power/reset";
			this.mnuBreakOnReset.Click += new System.EventHandler(this.mnuBreakOnReset_Click);
			// 
			// mnuBreakOnUnofficialOpcodes
			// 
			this.mnuBreakOnUnofficialOpcodes.CheckOnClick = true;
			this.mnuBreakOnUnofficialOpcodes.Name = "mnuBreakOnUnofficialOpcodes";
			this.mnuBreakOnUnofficialOpcodes.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnUnofficialOpcodes.Text = "Break on unofficial opcodes";
			this.mnuBreakOnUnofficialOpcodes.Click += new System.EventHandler(this.mnuBreakOnUnofficialOpcodes_Click);
			// 
			// mnuBreakOnBrk
			// 
			this.mnuBreakOnBrk.CheckOnClick = true;
			this.mnuBreakOnBrk.Name = "mnuBreakOnBrk";
			this.mnuBreakOnBrk.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnBrk.Text = "Break on BRK";
			this.mnuBreakOnBrk.Click += new System.EventHandler(this.mnuBreakOnBrk_Click);
			// 
			// mnuBreakOnCrash
			// 
			this.mnuBreakOnCrash.CheckOnClick = true;
			this.mnuBreakOnCrash.Name = "mnuBreakOnCrash";
			this.mnuBreakOnCrash.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnCrash.Text = "Break on CPU crash";
			this.mnuBreakOnCrash.Click += new System.EventHandler(this.mnuBreakOnCrash_Click);
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(247, 6);
			// 
			// mnuBreakOnOpen
			// 
			this.mnuBreakOnOpen.CheckOnClick = true;
			this.mnuBreakOnOpen.Name = "mnuBreakOnOpen";
			this.mnuBreakOnOpen.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnOpen.Text = "Break when debugger is opened";
			this.mnuBreakOnOpen.Click += new System.EventHandler(this.mnuBreakOnOpen_Click);
			// 
			// mnuBreakOnDebuggerFocus
			// 
			this.mnuBreakOnDebuggerFocus.CheckOnClick = true;
			this.mnuBreakOnDebuggerFocus.Name = "mnuBreakOnDebuggerFocus";
			this.mnuBreakOnDebuggerFocus.Size = new System.Drawing.Size(250, 22);
			this.mnuBreakOnDebuggerFocus.Text = "Break on debugger focus";
			this.mnuBreakOnDebuggerFocus.Click += new System.EventHandler(this.mnuBreakOnDebuggerFocus_Click);
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Size = new System.Drawing.Size(247, 6);
			// 
			// mnuBringToFrontOnBreak
			// 
			this.mnuBringToFrontOnBreak.CheckOnClick = true;
			this.mnuBringToFrontOnBreak.Name = "mnuBringToFrontOnBreak";
			this.mnuBringToFrontOnBreak.Size = new System.Drawing.Size(250, 22);
			this.mnuBringToFrontOnBreak.Text = "Bring debugger to front on break";
			this.mnuBringToFrontOnBreak.Click += new System.EventHandler(this.mnuBringToFrontOnBreak_Click);
			// 
			// mnuBringToFrontOnPause
			// 
			this.mnuBringToFrontOnPause.CheckOnClick = true;
			this.mnuBringToFrontOnPause.Name = "mnuBringToFrontOnPause";
			this.mnuBringToFrontOnPause.Size = new System.Drawing.Size(250, 22);
			this.mnuBringToFrontOnPause.Text = "Bring debugger to front on pause";
			this.mnuBringToFrontOnPause.Click += new System.EventHandler(this.mnuBringToFrontOnPause_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuConfigureColors
			// 
			this.mnuConfigureColors.Image = global::Mesen.GUI.Properties.Resources.PipetteSmall;
			this.mnuConfigureColors.Name = "mnuConfigureColors";
			this.mnuConfigureColors.Size = new System.Drawing.Size(266, 22);
			this.mnuConfigureColors.Text = "Configure Colors";
			this.mnuConfigureColors.Click += new System.EventHandler(this.mnuConfigureColors_Click);
			// 
			// mnuSplitView
			// 
			this.mnuSplitView.CheckOnClick = true;
			this.mnuSplitView.Image = global::Mesen.GUI.Properties.Resources.SplitView;
			this.mnuSplitView.Name = "mnuSplitView";
			this.mnuSplitView.Size = new System.Drawing.Size(266, 22);
			this.mnuSplitView.Text = "Split View";
			this.mnuSplitView.Click += new System.EventHandler(this.mnuSplitView_Click);
			// 
			// fontSizeToolStripMenuItem
			// 
			this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIncreaseFontSize,
            this.mnuDecreaseFontSize,
            this.mnuResetFontSize,
            this.toolStripMenuItem21,
            this.mnuSelectFont});
			this.fontSizeToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.Font;
			this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
			this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
			this.fontSizeToolStripMenuItem.Text = "Font Options";
			// 
			// mnuIncreaseFontSize
			// 
			this.mnuIncreaseFontSize.Name = "mnuIncreaseFontSize";
			this.mnuIncreaseFontSize.ShortcutKeyDisplayString = "Ctrl++";
			this.mnuIncreaseFontSize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.mnuIncreaseFontSize.Size = new System.Drawing.Size(197, 22);
			this.mnuIncreaseFontSize.Text = "Increase Size";
			this.mnuIncreaseFontSize.Click += new System.EventHandler(this.mnuIncreaseFontSize_Click);
			// 
			// mnuDecreaseFontSize
			// 
			this.mnuDecreaseFontSize.Name = "mnuDecreaseFontSize";
			this.mnuDecreaseFontSize.ShortcutKeyDisplayString = "Ctrl+-";
			this.mnuDecreaseFontSize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.mnuDecreaseFontSize.Size = new System.Drawing.Size(197, 22);
			this.mnuDecreaseFontSize.Text = "Decrease Size";
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
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(194, 6);
			// 
			// mnuSelectFont
			// 
			this.mnuSelectFont.Name = "mnuSelectFont";
			this.mnuSelectFont.Size = new System.Drawing.Size(197, 22);
			this.mnuSelectFont.Text = "Select Font...";
			this.mnuSelectFont.Click += new System.EventHandler(this.mnuSelectFont_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuShowToolbar
			// 
			this.mnuShowToolbar.CheckOnClick = true;
			this.mnuShowToolbar.Name = "mnuShowToolbar";
			this.mnuShowToolbar.Size = new System.Drawing.Size(266, 22);
			this.mnuShowToolbar.Text = "Show Toolbar";
			this.mnuShowToolbar.Click += new System.EventHandler(this.mnuShowToolbar_Click);
			// 
			// mnuShowCpuMemoryMapping
			// 
			this.mnuShowCpuMemoryMapping.CheckOnClick = true;
			this.mnuShowCpuMemoryMapping.Name = "mnuShowCpuMemoryMapping";
			this.mnuShowCpuMemoryMapping.Size = new System.Drawing.Size(266, 22);
			this.mnuShowCpuMemoryMapping.Text = "Show CPU Memory Mapping";
			this.mnuShowCpuMemoryMapping.Click += new System.EventHandler(this.mnuShowCpuMemoryMapping_Click);
			// 
			// mnuShowPpuMemoryMapping
			// 
			this.mnuShowPpuMemoryMapping.CheckOnClick = true;
			this.mnuShowPpuMemoryMapping.Name = "mnuShowPpuMemoryMapping";
			this.mnuShowPpuMemoryMapping.Size = new System.Drawing.Size(266, 22);
			this.mnuShowPpuMemoryMapping.Text = "Show PPU Memory Mapping";
			this.mnuShowPpuMemoryMapping.Click += new System.EventHandler(this.mnuShowPpuMemoryMapping_Click);
			// 
			// mnuShowFunctionLabelLists
			// 
			this.mnuShowFunctionLabelLists.CheckOnClick = true;
			this.mnuShowFunctionLabelLists.Name = "mnuShowFunctionLabelLists";
			this.mnuShowFunctionLabelLists.Size = new System.Drawing.Size(266, 22);
			this.mnuShowFunctionLabelLists.Text = "Show Function/Label Lists";
			this.mnuShowFunctionLabelLists.Click += new System.EventHandler(this.mnuShowFunctionLabelLists_Click);
			// 
			// mnuShowBottomPanel
			// 
			this.mnuShowBottomPanel.CheckOnClick = true;
			this.mnuShowBottomPanel.Name = "mnuShowBottomPanel";
			this.mnuShowBottomPanel.Size = new System.Drawing.Size(266, 22);
			this.mnuShowBottomPanel.Text = "Show Watch/Breakpoints/Callstack";
			this.mnuShowBottomPanel.Click += new System.EventHandler(this.mnuShowBottomPanel_Click);
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			this.toolStripMenuItem18.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuShowCodePreview
			// 
			this.mnuShowCodePreview.CheckOnClick = true;
			this.mnuShowCodePreview.Name = "mnuShowCodePreview";
			this.mnuShowCodePreview.Size = new System.Drawing.Size(266, 22);
			this.mnuShowCodePreview.Text = "Show Code Preview in Tooltips";
			this.mnuShowCodePreview.CheckedChanged += new System.EventHandler(this.mnuShowCodePreview_CheckedChanged);
			// 
			// mnuShowOpCodeTooltips
			// 
			this.mnuShowOpCodeTooltips.CheckOnClick = true;
			this.mnuShowOpCodeTooltips.Name = "mnuShowOpCodeTooltips";
			this.mnuShowOpCodeTooltips.Size = new System.Drawing.Size(266, 22);
			this.mnuShowOpCodeTooltips.Text = "Show OP Code Info Tooltips";
			this.mnuShowOpCodeTooltips.CheckedChanged += new System.EventHandler(this.mnuShowOpCodeTooltips_CheckedChanged);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuPpuPartialDraw
			// 
			this.mnuPpuPartialDraw.CheckOnClick = true;
			this.mnuPpuPartialDraw.Name = "mnuPpuPartialDraw";
			this.mnuPpuPartialDraw.Size = new System.Drawing.Size(266, 22);
			this.mnuPpuPartialDraw.Text = "Draw Partial Frame";
			this.mnuPpuPartialDraw.Click += new System.EventHandler(this.mnuPpuPartialDraw_Click);
			// 
			// mnuPpuShowPreviousFrame
			// 
			this.mnuPpuShowPreviousFrame.CheckOnClick = true;
			this.mnuPpuShowPreviousFrame.Name = "mnuPpuShowPreviousFrame";
			this.mnuPpuShowPreviousFrame.Size = new System.Drawing.Size(266, 22);
			this.mnuPpuShowPreviousFrame.Text = "Show previous frame behind current";
			this.mnuPpuShowPreviousFrame.Click += new System.EventHandler(this.mnuShowPreviousFrame_Click);
			// 
			// toolStripMenuItem19
			// 
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			this.toolStripMenuItem19.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuRefreshWatchWhileRunning
			// 
			this.mnuRefreshWatchWhileRunning.CheckOnClick = true;
			this.mnuRefreshWatchWhileRunning.Name = "mnuRefreshWatchWhileRunning";
			this.mnuRefreshWatchWhileRunning.Size = new System.Drawing.Size(266, 22);
			this.mnuRefreshWatchWhileRunning.Text = "Refresh watch while running";
			this.mnuRefreshWatchWhileRunning.Click += new System.EventHandler(this.mnuRefreshWatchWhileRunning_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApuViewer,
            this.mnuAssembler,
            this.mnuMemoryViewer,
            this.mnuEventViewer,
            this.mnuPpuViewer,
            this.mnuScriptWindow,
            this.mnuTraceLogger,
            this.toolStripMenuItem13,
            this.mnuEditHeader,
            this.toolStripMenuItem17,
            this.mnuCodeDataLogger});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// mnuApuViewer
			// 
			this.mnuApuViewer.Image = global::Mesen.GUI.Properties.Resources.Audio;
			this.mnuApuViewer.Name = "mnuApuViewer";
			this.mnuApuViewer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
			this.mnuApuViewer.Size = new System.Drawing.Size(205, 22);
			this.mnuApuViewer.Text = "APU Viewer";
			this.mnuApuViewer.Click += new System.EventHandler(this.mnuApuViewer_Click);
			// 
			// mnuAssembler
			// 
			this.mnuAssembler.Image = global::Mesen.GUI.Properties.Resources.Chip;
			this.mnuAssembler.Name = "mnuAssembler";
			this.mnuAssembler.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.mnuAssembler.Size = new System.Drawing.Size(205, 22);
			this.mnuAssembler.Text = "Assembler";
			this.mnuAssembler.Click += new System.EventHandler(this.mnuAssembler_Click);
			// 
			// mnuMemoryViewer
			// 
			this.mnuMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuMemoryViewer.Name = "mnuMemoryViewer";
			this.mnuMemoryViewer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
			this.mnuMemoryViewer.Size = new System.Drawing.Size(205, 22);
			this.mnuMemoryViewer.Text = "Memory Tools";
			this.mnuMemoryViewer.Click += new System.EventHandler(this.mnuMemoryViewer_Click);
			// 
			// mnuEventViewer
			// 
			this.mnuEventViewer.Image = global::Mesen.GUI.Properties.Resources.NesEventViewer;
			this.mnuEventViewer.Name = "mnuEventViewer";
			this.mnuEventViewer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.mnuEventViewer.Size = new System.Drawing.Size(205, 22);
			this.mnuEventViewer.Text = "NES Event Viewer";
			this.mnuEventViewer.Click += new System.EventHandler(this.mnuEventViewer_Click);
			// 
			// mnuPpuViewer
			// 
			this.mnuPpuViewer.Image = global::Mesen.GUI.Properties.Resources.Video;
			this.mnuPpuViewer.Name = "mnuPpuViewer";
			this.mnuPpuViewer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.mnuPpuViewer.Size = new System.Drawing.Size(205, 22);
			this.mnuPpuViewer.Text = "PPU Viewer";
			this.mnuPpuViewer.Click += new System.EventHandler(this.mnuNametableViewer_Click);
			// 
			// mnuScriptWindow
			// 
			this.mnuScriptWindow.Image = global::Mesen.GUI.Properties.Resources.Script;
			this.mnuScriptWindow.Name = "mnuScriptWindow";
			this.mnuScriptWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
			this.mnuScriptWindow.Size = new System.Drawing.Size(205, 22);
			this.mnuScriptWindow.Text = "Script Window";
			this.mnuScriptWindow.Click += new System.EventHandler(this.mnuScriptWindow_Click);
			// 
			// mnuTraceLogger
			// 
			this.mnuTraceLogger.Image = global::Mesen.GUI.Properties.Resources.LogWindow;
			this.mnuTraceLogger.Name = "mnuTraceLogger";
			this.mnuTraceLogger.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuTraceLogger.Size = new System.Drawing.Size(205, 22);
			this.mnuTraceLogger.Text = "Trace Logger";
			this.mnuTraceLogger.Click += new System.EventHandler(this.mnuTraceLogger_Click);
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(202, 6);
			// 
			// mnuEditHeader
			// 
			this.mnuEditHeader.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditHeader.Name = "mnuEditHeader";
			this.mnuEditHeader.Size = new System.Drawing.Size(205, 22);
			this.mnuEditHeader.Text = "Edit iNES Header";
			this.mnuEditHeader.Click += new System.EventHandler(this.mnuEditHeader_Click);
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(202, 6);
			// 
			// mnuCodeDataLogger
			// 
			this.mnuCodeDataLogger.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLoadsaveCDLFileToolStripMenuItem,
            this.toolStripMenuItem4,
            this.mnuLoadCdlFile,
            this.mnuSaveAsCdlFile,
            this.mnuResetCdlLog,
            this.toolStripMenuItem5,
            this.mnuCdlGenerateRom});
			this.mnuCodeDataLogger.Name = "mnuCodeDataLogger";
			this.mnuCodeDataLogger.Size = new System.Drawing.Size(205, 22);
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
			this.mnuLoadCdlFile.Image = global::Mesen.GUI.Properties.Resources.FolderOpen;
			this.mnuLoadCdlFile.Name = "mnuLoadCdlFile";
			this.mnuLoadCdlFile.Size = new System.Drawing.Size(193, 22);
			this.mnuLoadCdlFile.Text = "Load CDL file...";
			this.mnuLoadCdlFile.Click += new System.EventHandler(this.mnuLoadCdlFile_Click);
			// 
			// mnuSaveAsCdlFile
			// 
			this.mnuSaveAsCdlFile.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.mnuSaveAsCdlFile.Name = "mnuSaveAsCdlFile";
			this.mnuSaveAsCdlFile.Size = new System.Drawing.Size(193, 22);
			this.mnuSaveAsCdlFile.Text = "Save as CDL file...";
			this.mnuSaveAsCdlFile.Click += new System.EventHandler(this.mnuSaveAsCdlFile_Click);
			// 
			// mnuResetCdlLog
			// 
			this.mnuResetCdlLog.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuResetCdlLog.Name = "mnuResetCdlLog";
			this.mnuResetCdlLog.Size = new System.Drawing.Size(193, 22);
			this.mnuResetCdlLog.Text = "Reset log";
			this.mnuResetCdlLog.Click += new System.EventHandler(this.mnuResetCdlLog_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuCdlGenerateRom
			// 
			this.mnuCdlGenerateRom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCdlStripUnusedData,
            this.mnuCdlStripUsedData});
			this.mnuCdlGenerateRom.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCdlGenerateRom.Name = "mnuCdlGenerateRom";
			this.mnuCdlGenerateRom.Size = new System.Drawing.Size(193, 22);
			this.mnuCdlGenerateRom.Text = "Generate ROM";
			// 
			// mnuCdlStripUnusedData
			// 
			this.mnuCdlStripUnusedData.Name = "mnuCdlStripUnusedData";
			this.mnuCdlStripUnusedData.Size = new System.Drawing.Size(166, 22);
			this.mnuCdlStripUnusedData.Text = "Strip unused data";
			this.mnuCdlStripUnusedData.Click += new System.EventHandler(this.mnuCdlStripUnusedData_Click);
			// 
			// mnuCdlStripUsedData
			// 
			this.mnuCdlStripUsedData.Name = "mnuCdlStripUsedData";
			this.mnuCdlStripUsedData.Size = new System.Drawing.Size(166, 22);
			this.mnuCdlStripUsedData.Text = "Strip used data";
			this.mnuCdlStripUsedData.Click += new System.EventHandler(this.mnuCdlStripUsedData_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPrgAnalysis,
            this.lblPrgAnalysisResult,
            this.lblChrAnalysis,
            this.lblChrAnalysisResult,
            this.toolStripStatusLabel1,
            this.lblCyclesElapsedCount,
            this.lblCyclesElapsed});
			this.statusStrip.Location = new System.Drawing.Point(0, 663);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1172, 24);
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
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(437, 19);
			this.toolStripStatusLabel1.Spring = true;
			// 
			// lblCyclesElapsedCount
			// 
			this.lblCyclesElapsedCount.Name = "lblCyclesElapsedCount";
			this.lblCyclesElapsedCount.Size = new System.Drawing.Size(13, 19);
			this.lblCyclesElapsedCount.Text = "0";
			// 
			// lblCyclesElapsed
			// 
			this.lblCyclesElapsed.Margin = new System.Windows.Forms.Padding(-3, 3, 0, 2);
			this.lblCyclesElapsed.Name = "lblCyclesElapsed";
			this.lblCyclesElapsed.Size = new System.Drawing.Size(82, 19);
			this.lblCyclesElapsed.Text = "cycles elapsed";
			// 
			// ctrlPpuMemoryMapping
			// 
			this.ctrlPpuMemoryMapping.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ctrlPpuMemoryMapping.Location = new System.Drawing.Point(0, 630);
			this.ctrlPpuMemoryMapping.Name = "ctrlPpuMemoryMapping";
			this.ctrlPpuMemoryMapping.Size = new System.Drawing.Size(1172, 33);
			this.ctrlPpuMemoryMapping.TabIndex = 5;
			this.ctrlPpuMemoryMapping.Text = "ctrlMemoryMapping1";
			this.ctrlPpuMemoryMapping.Visible = false;
			// 
			// ctrlCpuMemoryMapping
			// 
			this.ctrlCpuMemoryMapping.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ctrlCpuMemoryMapping.Location = new System.Drawing.Point(0, 597);
			this.ctrlCpuMemoryMapping.Name = "ctrlCpuMemoryMapping";
			this.ctrlCpuMemoryMapping.Size = new System.Drawing.Size(1172, 33);
			this.ctrlCpuMemoryMapping.TabIndex = 4;
			this.ctrlCpuMemoryMapping.Text = "ctrlMemoryMapping1";
			this.ctrlCpuMemoryMapping.Visible = false;
			// 
			// tsToolbar
			// 
			this.tsToolbar.Location = new System.Drawing.Point(0, 24);
			this.tsToolbar.Name = "tsToolbar";
			this.tsToolbar.Size = new System.Drawing.Size(1172, 25);
			this.tsToolbar.TabIndex = 6;
			this.tsToolbar.Text = "toolStrip1";
			this.tsToolbar.Visible = false;
			// 
			// frmDebugger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1172, 687);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.ctrlCpuMemoryMapping);
			this.Controls.Add(this.ctrlPpuMemoryMapping);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.tsToolbar);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(1000, 725);
			this.Name = "frmDebugger";
			this.Text = "Debugger";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ctrlSplitContainerTop.Panel1.ResumeLayout(false);
			this.ctrlSplitContainerTop.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplitContainerTop)).EndInit();
			this.ctrlSplitContainerTop.ResumeLayout(false);
			this.tlpTop.ResumeLayout(false);
			this.tlpFunctionLabelLists.ResumeLayout(false);
			this.grpLabels.ResumeLayout(false);
			this.grpFunctions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).EndInit();
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

		private Mesen.GUI.Controls.ctrlSplitContainer splitContainer;
		private System.Windows.Forms.TableLayoutPanel tlpTop;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.GroupBox grpBreakpoints;
		private System.Windows.Forms.GroupBox grpWatch;
		private Mesen.GUI.Controls.ctrlMesenMenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
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
		private ctrlConsoleStatus ctrlConsoleStatus;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private ctrlDebuggerCode ctrlDebuggerCodeSplit;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
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
		private System.Windows.Forms.ToolStripMenuItem mnuCdlGenerateRom;
		private System.Windows.Forms.ToolStripMenuItem mnuCdlStripUsedData;
		private System.Windows.Forms.ToolStripMenuItem mnuCdlStripUnusedData;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableEnableBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToAddress;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToIrqHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToNmiHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToResetHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuTraceLogger;
		private System.Windows.Forms.ToolStripMenuItem mnuRunPpuCycle;
		private System.Windows.Forms.ToolStripMenuItem mnuPpuPartialDraw;
		private System.Windows.Forms.ToolStripMenuItem mnuRunScanline;
		private System.Windows.Forms.PictureBox picWatchHelp;
		private Controls.ctrlMemoryMapping ctrlCpuMemoryMapping;
		private Controls.ctrlMemoryMapping ctrlPpuMemoryMapping;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCpuMemoryMapping;
		private System.Windows.Forms.ToolStripMenuItem mnuShowPpuMemoryMapping;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.TableLayoutPanel tlpFunctionLabelLists;
		private System.Windows.Forms.GroupBox grpLabels;
		private System.Windows.Forms.GroupBox grpFunctions;
		private Controls.ctrlFunctionList ctrlFunctionList;
		private Controls.ctrlLabelList ctrlLabelList;
		private System.Windows.Forms.ToolStripMenuItem mnuShowFunctionLabelLists;
		private System.Windows.Forms.ToolStripMenuItem mnuWorkspace;
		private System.Windows.Forms.ToolStripMenuItem mnuResetWorkspace;
		private System.Windows.Forms.ToolStripMenuItem mnuImportLabels;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakIn;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
		private System.Windows.Forms.ToolStripMenuItem mnuFindAllOccurrences;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoLoadDbgFiles;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableDefaultLabels;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnReset;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassemblyOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuShowEffectiveAddresses;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassemble;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassembleVerifiedCode;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassembleVerifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassembleUnidentifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuDisplayOpCodesInLowerCase;
		private ctrlWatch ctrlWatch;
		private GUI.Controls.ctrlSplitContainer ctrlSplitContainerTop;
		private System.Windows.Forms.ToolStripMenuItem mnuShowBottomPanel;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnUnofficialOpcodes;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnBrk;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveRomAs;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
		private System.Windows.Forms.ToolStripMenuItem mnuAssembler;
		private System.Windows.Forms.ToolStripMenuItem mnuCode;
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshWatchWhileRunning;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnDebuggerFocus;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
		private System.Windows.Forms.ToolStripMenuItem mnuStepBack;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel lblCyclesElapsedCount;
		private System.Windows.Forms.ToolStripStatusLabel lblCyclesElapsed;
		private System.Windows.Forms.ToolStripMenuItem mnuExportLabels;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAsIps;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoLoadCdlFiles;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveRom;
		private System.Windows.Forms.ToolStripMenuItem mnuEditHeader;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
		private System.Windows.Forms.ToolStripMenuItem mnuRevertChanges;
		private System.Windows.Forms.ToolStripMenuItem mnuScriptWindow;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuApuViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCodePreview;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
		private System.Windows.Forms.ToolStripMenuItem mnuShowOpCodeTooltips;
		private System.Windows.Forms.ToolStripMenuItem mnuShow;
		private System.Windows.Forms.ToolStripMenuItem mnuShowDisassembledCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowVerifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuShowUnidentifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuConfigureColors;
		private Mesen.GUI.Controls.ctrlMesenToolStrip tsToolbar;
		private System.Windows.Forms.ToolStripMenuItem mnuShowToolbar;
		private System.Windows.Forms.ToolStripMenuItem mnuPpuShowPreviousFrame;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem19;
		private System.Windows.Forms.ToolStripMenuItem mnuEventViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnCrash;
		private System.Windows.Forms.ToolStripMenuItem mnuShowMemoryValues;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem20;
		private System.Windows.Forms.ToolStripMenuItem mnuBringToFrontOnPause;
		private System.Windows.Forms.ToolStripMenuItem mnuBringToFrontOnBreak;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectFont;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem22;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem23;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToProgramCount;
	}
}