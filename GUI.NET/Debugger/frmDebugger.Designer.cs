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
			this.splitContainer = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.ctrlSplitContainerTop = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ctrlSourceViewer = new Mesen.GUI.Debugger.Controls.ctrlSourceViewer();
			this.ctrlDebuggerCode = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.panel2 = new System.Windows.Forms.Panel();
			this.ctrlSourceViewerSplit = new Mesen.GUI.Debugger.Controls.ctrlSourceViewer();
			this.ctrlDebuggerCodeSplit = new Mesen.GUI.Debugger.ctrlDebuggerCode();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlConsoleStatus = new Mesen.GUI.Debugger.ctrlConsoleStatus();
			this.tlpVerticalLayout = new System.Windows.Forms.TableLayoutPanel();
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
			this.mnuImportSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuResetWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetLabels = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPowerCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableEnableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRunCpuCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunPpuCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunScanline = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunOneFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakIn = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOn = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSearch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindAllOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassemblyOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassemble = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleVerifiedCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleVerifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleUnidentifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowVerifiedCode = new System.Windows.Forms.ToolStripMenuItem();
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
			this.sepBreakNsfOptions = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnInit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnPlay = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnBusConflict = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnDecayedOamRead = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnPpu2006ScrollGlitch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnUninitMemoryRead = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnDebuggerFocus = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBringToFrontOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBringToFrontOnPause = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEnableSubInstructionBreakpoints = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowToolbar = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCpuMemoryMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowPpuMemoryMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowFunctionLabelLists = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowBottomPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTooltipOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCodePreview = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowOpCodeTooltips = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuOnlyShowTooltipOnShift = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyAddresses = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyByteCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyComments = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSelectFont = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuConfigureColors = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSplitView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUseVerticalLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoCreateJumpLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHidePauseIcon = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuPartialDraw = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuShowPreviousFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowBreakNotifications = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowInstructionProgression = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowSelectionLength = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAlwaysScrollToCenter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefreshWhileRunning = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuConfigureExternalEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuApuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAssembler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEventViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProfiler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScriptWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTextHooker = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTraceLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWatchWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.pPUViewerCompactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenNametableViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenChrViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenSpriteViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenPaletteViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditHeader = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripSeparator();
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
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
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
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.HidePanel2 = false;
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
			this.splitContainer.Size = new System.Drawing.Size(1075, 570);
			this.splitContainer.SplitterDistance = 407;
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
			this.ctrlSplitContainerTop.HidePanel2 = false;
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
			this.ctrlSplitContainerTop.Size = new System.Drawing.Size(1075, 407);
			this.ctrlSplitContainerTop.SplitterDistance = 821;
			this.ctrlSplitContainerTop.SplitterWidth = 7;
			this.ctrlSplitContainerTop.TabIndex = 3;
			this.ctrlSplitContainerTop.PanelCollapsed += new System.EventHandler(this.ctrlSplitContainerTop_PanelCollapsed);
			this.ctrlSplitContainerTop.PanelExpanded += new System.EventHandler(this.ctrlSplitContainerTop_PanelExpanded);
			// 
			// tlpTop
			// 
			this.tlpTop.ColumnCount = 3;
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTop.Controls.Add(this.panel1, 0, 0);
			this.tlpTop.Controls.Add(this.panel2, 1, 0);
			this.tlpTop.Controls.Add(this.tableLayoutPanel1, 2, 0);
			this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTop.Location = new System.Drawing.Point(0, 0);
			this.tlpTop.Name = "tlpTop";
			this.tlpTop.RowCount = 1;
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 407F));
			this.tlpTop.Size = new System.Drawing.Size(821, 407);
			this.tlpTop.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.ctrlSourceViewer);
			this.panel1.Controls.Add(this.ctrlDebuggerCode);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(357, 407);
			this.panel1.TabIndex = 5;
			// 
			// ctrlSourceViewer
			// 
			this.ctrlSourceViewer.CurrentFile = null;
			this.ctrlSourceViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSourceViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlSourceViewer.Name = "ctrlSourceViewer";
			this.ctrlSourceViewer.Size = new System.Drawing.Size(357, 407);
			this.ctrlSourceViewer.SymbolProvider = null;
			this.ctrlSourceViewer.TabIndex = 7;
			this.ctrlSourceViewer.Visible = false;
			this.ctrlSourceViewer.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// ctrlDebuggerCode
			// 
			this.ctrlDebuggerCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCode.HideSelection = false;
			this.ctrlDebuggerCode.Location = new System.Drawing.Point(0, 0);
			this.ctrlDebuggerCode.Name = "ctrlDebuggerCode";
			this.ctrlDebuggerCode.ShowMemoryValues = false;
			this.ctrlDebuggerCode.Size = new System.Drawing.Size(357, 407);
			this.ctrlDebuggerCode.SymbolProvider = null;
			this.ctrlDebuggerCode.TabIndex = 2;
			this.ctrlDebuggerCode.OnEditCode += new Mesen.GUI.Debugger.ctrlDebuggerCode.AssemblerEventHandler(this.ctrlDebuggerCode_OnEditCode);
			this.ctrlDebuggerCode.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.ctrlSourceViewerSplit);
			this.panel2.Controls.Add(this.ctrlDebuggerCodeSplit);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(363, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1, 407);
			this.panel2.TabIndex = 6;
			// 
			// ctrlSourceViewerSplit
			// 
			this.ctrlSourceViewerSplit.CurrentFile = null;
			this.ctrlSourceViewerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSourceViewerSplit.Location = new System.Drawing.Point(0, 0);
			this.ctrlSourceViewerSplit.Name = "ctrlSourceViewerSplit";
			this.ctrlSourceViewerSplit.Size = new System.Drawing.Size(1, 407);
			this.ctrlSourceViewerSplit.SymbolProvider = null;
			this.ctrlSourceViewerSplit.TabIndex = 8;
			this.ctrlSourceViewerSplit.Visible = false;
			this.ctrlSourceViewerSplit.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// ctrlDebuggerCodeSplit
			// 
			this.ctrlDebuggerCodeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDebuggerCodeSplit.HideSelection = false;
			this.ctrlDebuggerCodeSplit.Location = new System.Drawing.Point(0, 0);
			this.ctrlDebuggerCodeSplit.Name = "ctrlDebuggerCodeSplit";
			this.ctrlDebuggerCodeSplit.ShowMemoryValues = false;
			this.ctrlDebuggerCodeSplit.Size = new System.Drawing.Size(1, 407);
			this.ctrlDebuggerCodeSplit.SymbolProvider = null;
			this.ctrlDebuggerCodeSplit.TabIndex = 4;
			this.ctrlDebuggerCodeSplit.Visible = false;
			this.ctrlDebuggerCodeSplit.OnEditCode += new Mesen.GUI.Debugger.ctrlDebuggerCode.AssemblerEventHandler(this.ctrlDebuggerCode_OnEditCode);
			this.ctrlDebuggerCodeSplit.Enter += new System.EventHandler(this.ctrlDebuggerCode_Enter);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.ctrlConsoleStatus, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tlpVerticalLayout, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(363, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(458, 407);
			this.tableLayoutPanel1.TabIndex = 7;
			// 
			// ctrlConsoleStatus
			// 
			this.ctrlConsoleStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlConsoleStatus.Location = new System.Drawing.Point(0, 0);
			this.ctrlConsoleStatus.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlConsoleStatus.Name = "ctrlConsoleStatus";
			this.ctrlConsoleStatus.Size = new System.Drawing.Size(458, 400);
			this.ctrlConsoleStatus.TabIndex = 3;
			this.ctrlConsoleStatus.OnGotoLocation += new System.EventHandler(this.ctrlConsoleStatus_OnGotoLocation);
			// 
			// tlpVerticalLayout
			// 
			this.tlpVerticalLayout.ColumnCount = 2;
			this.tlpVerticalLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpVerticalLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpVerticalLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpVerticalLayout.Location = new System.Drawing.Point(0, 400);
			this.tlpVerticalLayout.Margin = new System.Windows.Forms.Padding(0);
			this.tlpVerticalLayout.Name = "tlpVerticalLayout";
			this.tlpVerticalLayout.RowCount = 1;
			this.tlpVerticalLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpVerticalLayout.Size = new System.Drawing.Size(458, 7);
			this.tlpVerticalLayout.TabIndex = 4;
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
			this.tlpFunctionLabelLists.Size = new System.Drawing.Size(247, 407);
			this.tlpFunctionLabelLists.TabIndex = 5;
			// 
			// grpLabels
			// 
			this.grpLabels.Controls.Add(this.ctrlLabelList);
			this.grpLabels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpLabels.Location = new System.Drawing.Point(3, 206);
			this.grpLabels.Name = "grpLabels";
			this.grpLabels.Size = new System.Drawing.Size(241, 198);
			this.grpLabels.TabIndex = 6;
			this.grpLabels.TabStop = false;
			this.grpLabels.Text = "Labels";
			// 
			// ctrlLabelList
			// 
			this.ctrlLabelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLabelList.Location = new System.Drawing.Point(3, 16);
			this.ctrlLabelList.Name = "ctrlLabelList";
			this.ctrlLabelList.Size = new System.Drawing.Size(235, 179);
			this.ctrlLabelList.TabIndex = 0;
			this.ctrlLabelList.OnFindOccurrence += new System.EventHandler(this.ctrlLabelList_OnFindOccurrence);
			this.ctrlLabelList.OnLabelSelected += new Mesen.GUI.Debugger.GoToDestinationEventHandler(this.ctrlLabelList_OnLabelSelected);
			// 
			// grpFunctions
			// 
			this.grpFunctions.Controls.Add(this.ctrlFunctionList);
			this.grpFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFunctions.Location = new System.Drawing.Point(3, 3);
			this.grpFunctions.Name = "grpFunctions";
			this.grpFunctions.Size = new System.Drawing.Size(241, 197);
			this.grpFunctions.TabIndex = 5;
			this.grpFunctions.TabStop = false;
			this.grpFunctions.Text = "Functions";
			// 
			// ctrlFunctionList
			// 
			this.ctrlFunctionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFunctionList.Location = new System.Drawing.Point(3, 16);
			this.ctrlFunctionList.Name = "ctrlFunctionList";
			this.ctrlFunctionList.Size = new System.Drawing.Size(235, 178);
			this.ctrlFunctionList.TabIndex = 0;
			this.ctrlFunctionList.OnFindOccurrence += new System.EventHandler(this.ctrlFunctionList_OnFindOccurrence);
			this.ctrlFunctionList.OnFunctionSelected += new Mesen.GUI.Debugger.GoToDestinationEventHandler(this.ctrlFunctionList_OnFunctionSelected);
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
			this.tableLayoutPanel10.Size = new System.Drawing.Size(1075, 156);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// grpWatch
			// 
			this.grpWatch.Controls.Add(this.ctrlWatch);
			this.grpWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpWatch.Location = new System.Drawing.Point(3, 3);
			this.grpWatch.Name = "grpWatch";
			this.grpWatch.Size = new System.Drawing.Size(352, 150);
			this.grpWatch.TabIndex = 2;
			this.grpWatch.TabStop = false;
			this.grpWatch.Text = "Watch";
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(3, 16);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(346, 131);
			this.ctrlWatch.TabIndex = 0;
			// 
			// grpBreakpoints
			// 
			this.grpBreakpoints.Controls.Add(this.ctrlBreakpoints);
			this.grpBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpBreakpoints.Location = new System.Drawing.Point(361, 3);
			this.grpBreakpoints.Name = "grpBreakpoints";
			this.grpBreakpoints.Size = new System.Drawing.Size(352, 150);
			this.grpBreakpoints.TabIndex = 3;
			this.grpBreakpoints.TabStop = false;
			this.grpBreakpoints.Text = "Breakpoints";
			// 
			// ctrlBreakpoints
			// 
			this.ctrlBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlBreakpoints.Location = new System.Drawing.Point(3, 16);
			this.ctrlBreakpoints.Name = "ctrlBreakpoints";
			this.ctrlBreakpoints.Size = new System.Drawing.Size(346, 131);
			this.ctrlBreakpoints.TabIndex = 0;
			this.ctrlBreakpoints.BreakpointNavigation += new System.EventHandler(this.ctrlBreakpoints_BreakpointNavigation);
			// 
			// grpCallstack
			// 
			this.grpCallstack.Controls.Add(this.ctrlCallstack);
			this.grpCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCallstack.Location = new System.Drawing.Point(719, 3);
			this.grpCallstack.Name = "grpCallstack";
			this.grpCallstack.Size = new System.Drawing.Size(353, 150);
			this.grpCallstack.TabIndex = 4;
			this.grpCallstack.TabStop = false;
			this.grpCallstack.Text = "Call Stack";
			// 
			// ctrlCallstack
			// 
			this.ctrlCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCallstack.Location = new System.Drawing.Point(3, 16);
			this.ctrlCallstack.Name = "ctrlCallstack";
			this.ctrlCallstack.Size = new System.Drawing.Size(347, 131);
			this.ctrlCallstack.TabIndex = 0;
			this.ctrlCallstack.FunctionSelected += new System.EventHandler(this.ctrlCallstack_FunctionSelected);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuCode,
            this.debugToolStripMenuItem,
            this.mnuSearch,
            this.mnuOptions,
            this.toolsToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1075, 24);
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
            this.mnuImportSettings,
            this.toolStripMenuItem16,
            this.mnuResetWorkspace,
            this.mnuResetLabels,
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
			// mnuImportSettings
			// 
			this.mnuImportSettings.Image = global::Mesen.GUI.Properties.Resources.Cog;
			this.mnuImportSettings.Name = "mnuImportSettings";
			this.mnuImportSettings.Size = new System.Drawing.Size(207, 22);
			this.mnuImportSettings.Text = "Import Settings";
			this.mnuImportSettings.Click += new System.EventHandler(this.mnuImportSettings_Click);
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
			// mnuResetLabels
			// 
			this.mnuResetLabels.Name = "mnuResetLabels";
			this.mnuResetLabels.Size = new System.Drawing.Size(207, 22);
			this.mnuResetLabels.Text = "Reset Labels";
			this.mnuResetLabels.Click += new System.EventHandler(this.mnuResetLabels_Click);
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
            this.mnuReset,
            this.mnuPowerCycle,
            this.toolStripMenuItem24,
            this.mnuToggleBreakpoint,
            this.mnuDisableEnableBreakpoint,
            this.toolStripMenuItem2,
            this.mnuRunCpuCycle,
            this.mnuRunPpuCycle,
            this.mnuRunScanline,
            this.mnuRunOneFrame,
            this.toolStripMenuItem8,
            this.mnuBreakIn,
            this.mnuBreakOn});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// mnuContinue
			// 
			this.mnuContinue.Enabled = false;
			this.mnuContinue.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.mnuContinue.Name = "mnuContinue";
			this.mnuContinue.Size = new System.Drawing.Size(212, 22);
			this.mnuContinue.Text = "Continue";
			this.mnuContinue.Click += new System.EventHandler(this.mnuContinue_Click);
			// 
			// mnuBreak
			// 
			this.mnuBreak.Enabled = false;
			this.mnuBreak.Image = global::Mesen.GUI.Properties.Resources.Pause;
			this.mnuBreak.Name = "mnuBreak";
			this.mnuBreak.ShortcutKeyDisplayString = "";
			this.mnuBreak.Size = new System.Drawing.Size(212, 22);
			this.mnuBreak.Text = "Break";
			this.mnuBreak.Click += new System.EventHandler(this.mnuBreak_Click);
			// 
			// mnuStepInto
			// 
			this.mnuStepInto.Image = global::Mesen.GUI.Properties.Resources.StepInto;
			this.mnuStepInto.Name = "mnuStepInto";
			this.mnuStepInto.Size = new System.Drawing.Size(212, 22);
			this.mnuStepInto.Text = "Step Into";
			this.mnuStepInto.Click += new System.EventHandler(this.mnuStepInto_Click);
			// 
			// mnuStepOver
			// 
			this.mnuStepOver.Image = global::Mesen.GUI.Properties.Resources.StepOver;
			this.mnuStepOver.Name = "mnuStepOver";
			this.mnuStepOver.Size = new System.Drawing.Size(212, 22);
			this.mnuStepOver.Text = "Step Over";
			this.mnuStepOver.Click += new System.EventHandler(this.mnuStepOver_Click);
			// 
			// mnuStepOut
			// 
			this.mnuStepOut.Image = global::Mesen.GUI.Properties.Resources.StepOut;
			this.mnuStepOut.Name = "mnuStepOut";
			this.mnuStepOut.Size = new System.Drawing.Size(212, 22);
			this.mnuStepOut.Text = "Step Out";
			this.mnuStepOut.Click += new System.EventHandler(this.mnuStepOut_Click);
			// 
			// mnuStepBack
			// 
			this.mnuStepBack.Image = global::Mesen.GUI.Properties.Resources.StepBack;
			this.mnuStepBack.Name = "mnuStepBack";
			this.mnuStepBack.Size = new System.Drawing.Size(212, 22);
			this.mnuStepBack.Text = "Step Back";
			this.mnuStepBack.Click += new System.EventHandler(this.mnuStepBack_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuReset
			// 
			this.mnuReset.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuReset.Name = "mnuReset";
			this.mnuReset.Size = new System.Drawing.Size(212, 22);
			this.mnuReset.Text = "Reset";
			this.mnuReset.Click += new System.EventHandler(this.mnuReset_Click);
			// 
			// mnuPowerCycle
			// 
			this.mnuPowerCycle.Image = global::Mesen.GUI.Properties.Resources.PowerCycle;
			this.mnuPowerCycle.Name = "mnuPowerCycle";
			this.mnuPowerCycle.Size = new System.Drawing.Size(212, 22);
			this.mnuPowerCycle.Text = "Power Cycle";
			this.mnuPowerCycle.Click += new System.EventHandler(this.mnuPowerCycle_Click);
			// 
			// toolStripMenuItem24
			// 
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			this.toolStripMenuItem24.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Breakpoint;
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(212, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			this.mnuToggleBreakpoint.Click += new System.EventHandler(this.mnuToggleBreakpoint_Click);
			// 
			// mnuDisableEnableBreakpoint
			// 
			this.mnuDisableEnableBreakpoint.Image = global::Mesen.GUI.Properties.Resources.BreakpointDisabled;
			this.mnuDisableEnableBreakpoint.Name = "mnuDisableEnableBreakpoint";
			this.mnuDisableEnableBreakpoint.Size = new System.Drawing.Size(212, 22);
			this.mnuDisableEnableBreakpoint.Text = "Disable/Enable Breakpoint";
			this.mnuDisableEnableBreakpoint.Click += new System.EventHandler(this.mnuDisableEnableBreakpoint_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuRunCpuCycle
			// 
			this.mnuRunCpuCycle.Image = global::Mesen.GUI.Properties.Resources.JumpTarget;
			this.mnuRunCpuCycle.Name = "mnuRunCpuCycle";
			this.mnuRunCpuCycle.Size = new System.Drawing.Size(212, 22);
			this.mnuRunCpuCycle.Text = "Run one CPU cycle";
			this.mnuRunCpuCycle.Click += new System.EventHandler(this.mnuRunCpuCycle_Click);
			// 
			// mnuRunPpuCycle
			// 
			this.mnuRunPpuCycle.Image = global::Mesen.GUI.Properties.Resources.RunPpuCycle;
			this.mnuRunPpuCycle.Name = "mnuRunPpuCycle";
			this.mnuRunPpuCycle.Size = new System.Drawing.Size(212, 22);
			this.mnuRunPpuCycle.Text = "Run one PPU cycle";
			this.mnuRunPpuCycle.Click += new System.EventHandler(this.mnuRunPpuCycle_Click);
			// 
			// mnuRunScanline
			// 
			this.mnuRunScanline.Image = global::Mesen.GUI.Properties.Resources.RunPpuScanline;
			this.mnuRunScanline.Name = "mnuRunScanline";
			this.mnuRunScanline.Size = new System.Drawing.Size(212, 22);
			this.mnuRunScanline.Text = "Run one scanline";
			this.mnuRunScanline.Click += new System.EventHandler(this.mnuRunScanline_Click);
			// 
			// mnuRunOneFrame
			// 
			this.mnuRunOneFrame.Image = global::Mesen.GUI.Properties.Resources.RunPpuFrame;
			this.mnuRunOneFrame.Name = "mnuRunOneFrame";
			this.mnuRunOneFrame.Size = new System.Drawing.Size(212, 22);
			this.mnuRunOneFrame.Text = "Run one frame";
			this.mnuRunOneFrame.Click += new System.EventHandler(this.mnuRunOneFrame_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuBreakIn
			// 
			this.mnuBreakIn.Name = "mnuBreakIn";
			this.mnuBreakIn.Size = new System.Drawing.Size(212, 22);
			this.mnuBreakIn.Text = "Break in...";
			this.mnuBreakIn.Click += new System.EventHandler(this.mnuBreakIn_Click);
			// 
			// mnuBreakOn
			// 
			this.mnuBreakOn.Name = "mnuBreakOn";
			this.mnuBreakOn.Size = new System.Drawing.Size(212, 22);
			this.mnuBreakOn.Text = "Break on...";
			this.mnuBreakOn.Click += new System.EventHandler(this.mnuBreakOn_Click);
			// 
			// mnuSearch
			// 
			this.mnuSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToAll,
            this.mnuGoToAddress,
            this.mnuGoTo,
            this.toolStripMenuItem29,
            this.mnuFind,
            this.mnuFindNext,
            this.mnuFindPrev,
            this.toolStripMenuItem9,
            this.mnuFindAllOccurrences});
			this.mnuSearch.Name = "mnuSearch";
			this.mnuSearch.Size = new System.Drawing.Size(54, 20);
			this.mnuSearch.Text = "Search";
			this.mnuSearch.DropDownOpening += new System.EventHandler(this.mnuSearch_DropDownOpening);
			// 
			// mnuGoToAll
			// 
			this.mnuGoToAll.Name = "mnuGoToAll";
			this.mnuGoToAll.Size = new System.Drawing.Size(183, 22);
			this.mnuGoToAll.Text = "Go to All";
			this.mnuGoToAll.Click += new System.EventHandler(this.mnuGoToAll_Click);
			// 
			// mnuGoToAddress
			// 
			this.mnuGoToAddress.Name = "mnuGoToAddress";
			this.mnuGoToAddress.Size = new System.Drawing.Size(183, 22);
			this.mnuGoToAddress.Text = "Go to Address";
			this.mnuGoToAddress.Click += new System.EventHandler(this.mnuGoToAddress_Click);
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.Size = new System.Drawing.Size(183, 22);
			this.mnuGoTo.Text = "Go to...";
			// 
			// toolStripMenuItem29
			// 
			this.toolStripMenuItem29.Name = "toolStripMenuItem29";
			this.toolStripMenuItem29.Size = new System.Drawing.Size(180, 6);
			// 
			// mnuFind
			// 
			this.mnuFind.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFind.Name = "mnuFind";
			this.mnuFind.Size = new System.Drawing.Size(183, 22);
			this.mnuFind.Text = "Find...";
			this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
			// 
			// mnuFindNext
			// 
			this.mnuFindNext.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuFindNext.Name = "mnuFindNext";
			this.mnuFindNext.Size = new System.Drawing.Size(183, 22);
			this.mnuFindNext.Text = "Find Next";
			this.mnuFindNext.Click += new System.EventHandler(this.mnuFindNext_Click);
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Image = global::Mesen.GUI.Properties.Resources.PreviousArrow;
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.Size = new System.Drawing.Size(183, 22);
			this.mnuFindPrev.Text = "Find Previous";
			this.mnuFindPrev.Click += new System.EventHandler(this.mnuFindPrev_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(180, 6);
			// 
			// mnuFindAllOccurrences
			// 
			this.mnuFindAllOccurrences.Name = "mnuFindAllOccurrences";
			this.mnuFindAllOccurrences.Size = new System.Drawing.Size(183, 22);
			this.mnuFindAllOccurrences.Text = "Find All Occurrences";
			this.mnuFindAllOccurrences.Click += new System.EventHandler(this.mnuFindAllOccurrences_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassemblyOptions,
            this.mnuBreakOptions,
            this.toolStripMenuItem12,
            this.mnuShowOptions,
            this.mnuTooltipOptions,
            this.mnuCopyOptions,
            this.fontSizeToolStripMenuItem,
            this.mnuConfigureColors,
            this.toolStripSeparator1,
            this.mnuSplitView,
            this.mnuUseVerticalLayout,
            this.toolStripMenuItem11,
            this.mnuAutoCreateJumpLabels,
            this.toolStripMenuItem25,
            this.mnuHidePauseIcon,
            this.mnuPpuPartialDraw,
            this.mnuPpuShowPreviousFrame,
            this.toolStripMenuItem19,
            this.mnuShowBreakNotifications,
            this.mnuShowInstructionProgression,
            this.mnuShowSelectionLength,
            this.toolStripMenuItem27,
            this.mnuAlwaysScrollToCenter,
            this.mnuRefreshWhileRunning,
            this.toolStripMenuItem6,
            this.mnuConfigureExternalEditor,
            this.mnuPreferences});
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
            this.mnuShowVerifiedCode,
            this.mnuShowVerifiedData,
            this.mnuShowUnidentifiedData});
			this.mnuShow.Name = "mnuShow";
			this.mnuShow.Size = new System.Drawing.Size(236, 22);
			this.mnuShow.Text = "Show...";
			// 
			// mnuShowVerifiedCode
			// 
			this.mnuShowVerifiedCode.Checked = true;
			this.mnuShowVerifiedCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuShowVerifiedCode.Enabled = false;
			this.mnuShowVerifiedCode.Name = "mnuShowVerifiedCode";
			this.mnuShowVerifiedCode.Size = new System.Drawing.Size(199, 22);
			this.mnuShowVerifiedCode.Text = "Verified Code";
			// 
			// mnuShowVerifiedData
			// 
			this.mnuShowVerifiedData.CheckOnClick = true;
			this.mnuShowVerifiedData.Image = global::Mesen.GUI.Properties.Resources.VerifiedData;
			this.mnuShowVerifiedData.Name = "mnuShowVerifiedData";
			this.mnuShowVerifiedData.Size = new System.Drawing.Size(199, 22);
			this.mnuShowVerifiedData.Text = "Verified Data";
			this.mnuShowVerifiedData.Click += new System.EventHandler(this.mnuShowVerifiedData_Click);
			// 
			// mnuShowUnidentifiedData
			// 
			this.mnuShowUnidentifiedData.CheckOnClick = true;
			this.mnuShowUnidentifiedData.Image = global::Mesen.GUI.Properties.Resources.UnidentifiedData;
			this.mnuShowUnidentifiedData.Name = "mnuShowUnidentifiedData";
			this.mnuShowUnidentifiedData.Size = new System.Drawing.Size(199, 22);
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
            this.sepBreakNsfOptions,
            this.mnuBreakOnInit,
            this.mnuBreakOnPlay,
            this.toolStripMenuItem26,
            this.mnuBreakOnBusConflict,
            this.mnuBreakOnDecayedOamRead,
            this.mnuBreakOnPpu2006ScrollGlitch,
            this.mnuBreakOnUninitMemoryRead,
            this.toolStripMenuItem15,
            this.mnuBreakOnOpen,
            this.mnuBreakOnDebuggerFocus,
            this.toolStripMenuItem20,
            this.mnuBringToFrontOnBreak,
            this.mnuBringToFrontOnPause,
            this.toolStripMenuItem28,
            this.mnuEnableSubInstructionBreakpoints});
			this.mnuBreakOptions.Name = "mnuBreakOptions";
			this.mnuBreakOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuBreakOptions.Text = "Break Options";
			this.mnuBreakOptions.DropDownOpening += new System.EventHandler(this.mnuBreakOptions_DropDownOpening);
			// 
			// mnuBreakOnReset
			// 
			this.mnuBreakOnReset.CheckOnClick = true;
			this.mnuBreakOnReset.Name = "mnuBreakOnReset";
			this.mnuBreakOnReset.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnReset.Text = "Break on power/reset";
			this.mnuBreakOnReset.Click += new System.EventHandler(this.mnuBreakOnReset_Click);
			// 
			// mnuBreakOnUnofficialOpcodes
			// 
			this.mnuBreakOnUnofficialOpcodes.CheckOnClick = true;
			this.mnuBreakOnUnofficialOpcodes.Name = "mnuBreakOnUnofficialOpcodes";
			this.mnuBreakOnUnofficialOpcodes.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnUnofficialOpcodes.Text = "Break on unofficial opcodes";
			this.mnuBreakOnUnofficialOpcodes.Click += new System.EventHandler(this.mnuBreakOnUnofficialOpcodes_Click);
			// 
			// mnuBreakOnBrk
			// 
			this.mnuBreakOnBrk.CheckOnClick = true;
			this.mnuBreakOnBrk.Name = "mnuBreakOnBrk";
			this.mnuBreakOnBrk.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnBrk.Text = "Break on BRK";
			this.mnuBreakOnBrk.Click += new System.EventHandler(this.mnuBreakOnBrk_Click);
			// 
			// mnuBreakOnCrash
			// 
			this.mnuBreakOnCrash.CheckOnClick = true;
			this.mnuBreakOnCrash.Name = "mnuBreakOnCrash";
			this.mnuBreakOnCrash.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnCrash.Text = "Break on CPU crash";
			this.mnuBreakOnCrash.Click += new System.EventHandler(this.mnuBreakOnCrash_Click);
			// 
			// sepBreakNsfOptions
			// 
			this.sepBreakNsfOptions.Name = "sepBreakNsfOptions";
			this.sepBreakNsfOptions.Size = new System.Drawing.Size(258, 6);
			// 
			// mnuBreakOnInit
			// 
			this.mnuBreakOnInit.CheckOnClick = true;
			this.mnuBreakOnInit.Name = "mnuBreakOnInit";
			this.mnuBreakOnInit.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnInit.Text = "Break on Init (NSF)";
			this.mnuBreakOnInit.Click += new System.EventHandler(this.mnuBreakOnInit_Click);
			// 
			// mnuBreakOnPlay
			// 
			this.mnuBreakOnPlay.CheckOnClick = true;
			this.mnuBreakOnPlay.Name = "mnuBreakOnPlay";
			this.mnuBreakOnPlay.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnPlay.Text = "Break on Play (NSF)";
			this.mnuBreakOnPlay.Click += new System.EventHandler(this.mnuBreakOnPlay_Click);
			// 
			// toolStripMenuItem26
			// 
			this.toolStripMenuItem26.Name = "toolStripMenuItem26";
			this.toolStripMenuItem26.Size = new System.Drawing.Size(258, 6);
			// 
			// mnuBreakOnBusConflict
			// 
			this.mnuBreakOnBusConflict.CheckOnClick = true;
			this.mnuBreakOnBusConflict.Name = "mnuBreakOnBusConflict";
			this.mnuBreakOnBusConflict.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnBusConflict.Text = "Break on bus conflict";
			this.mnuBreakOnBusConflict.Click += new System.EventHandler(this.mnuBreakOnBusConflict_Click);
			// 
			// mnuBreakOnDecayedOamRead
			// 
			this.mnuBreakOnDecayedOamRead.CheckOnClick = true;
			this.mnuBreakOnDecayedOamRead.Name = "mnuBreakOnDecayedOamRead";
			this.mnuBreakOnDecayedOamRead.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnDecayedOamRead.Text = "Break on decayed OAM read";
			this.mnuBreakOnDecayedOamRead.Click += new System.EventHandler(this.mnuBreakOnDecayedOamRead_Click);
			// 
			// mnuBreakOnPpu2006ScrollGlitch
			// 
			this.mnuBreakOnPpu2006ScrollGlitch.CheckOnClick = true;
			this.mnuBreakOnPpu2006ScrollGlitch.Name = "mnuBreakOnPpu2006ScrollGlitch";
			this.mnuBreakOnPpu2006ScrollGlitch.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnPpu2006ScrollGlitch.Text = "Break on PPU $2006 scroll glitch";
			this.mnuBreakOnPpu2006ScrollGlitch.Click += new System.EventHandler(this.mnuBreakOnPpu2006ScrollGlitch_Click);
			// 
			// mnuBreakOnUninitMemoryRead
			// 
			this.mnuBreakOnUninitMemoryRead.CheckOnClick = true;
			this.mnuBreakOnUninitMemoryRead.Name = "mnuBreakOnUninitMemoryRead";
			this.mnuBreakOnUninitMemoryRead.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnUninitMemoryRead.Text = "Break on uninitialized memory read";
			this.mnuBreakOnUninitMemoryRead.Click += new System.EventHandler(this.mnuBreakOnUninitMemoryRead_Click);
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(258, 6);
			// 
			// mnuBreakOnOpen
			// 
			this.mnuBreakOnOpen.CheckOnClick = true;
			this.mnuBreakOnOpen.Name = "mnuBreakOnOpen";
			this.mnuBreakOnOpen.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnOpen.Text = "Break when debugger is opened";
			this.mnuBreakOnOpen.Click += new System.EventHandler(this.mnuBreakOnOpen_Click);
			// 
			// mnuBreakOnDebuggerFocus
			// 
			this.mnuBreakOnDebuggerFocus.CheckOnClick = true;
			this.mnuBreakOnDebuggerFocus.Name = "mnuBreakOnDebuggerFocus";
			this.mnuBreakOnDebuggerFocus.Size = new System.Drawing.Size(261, 22);
			this.mnuBreakOnDebuggerFocus.Text = "Break on debugger focus";
			this.mnuBreakOnDebuggerFocus.Click += new System.EventHandler(this.mnuBreakOnDebuggerFocus_Click);
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Size = new System.Drawing.Size(258, 6);
			// 
			// mnuBringToFrontOnBreak
			// 
			this.mnuBringToFrontOnBreak.CheckOnClick = true;
			this.mnuBringToFrontOnBreak.Name = "mnuBringToFrontOnBreak";
			this.mnuBringToFrontOnBreak.Size = new System.Drawing.Size(261, 22);
			this.mnuBringToFrontOnBreak.Text = "Bring debugger to front on break";
			this.mnuBringToFrontOnBreak.Click += new System.EventHandler(this.mnuBringToFrontOnBreak_Click);
			// 
			// mnuBringToFrontOnPause
			// 
			this.mnuBringToFrontOnPause.CheckOnClick = true;
			this.mnuBringToFrontOnPause.Name = "mnuBringToFrontOnPause";
			this.mnuBringToFrontOnPause.Size = new System.Drawing.Size(261, 22);
			this.mnuBringToFrontOnPause.Text = "Bring debugger to front on pause";
			this.mnuBringToFrontOnPause.Click += new System.EventHandler(this.mnuBringToFrontOnPause_Click);
			// 
			// toolStripMenuItem28
			// 
			this.toolStripMenuItem28.Name = "toolStripMenuItem28";
			this.toolStripMenuItem28.Size = new System.Drawing.Size(258, 6);
			// 
			// mnuEnableSubInstructionBreakpoints
			// 
			this.mnuEnableSubInstructionBreakpoints.CheckOnClick = true;
			this.mnuEnableSubInstructionBreakpoints.Name = "mnuEnableSubInstructionBreakpoints";
			this.mnuEnableSubInstructionBreakpoints.Size = new System.Drawing.Size(261, 22);
			this.mnuEnableSubInstructionBreakpoints.Text = "Enable sub-instruction breakpoints";
			this.mnuEnableSubInstructionBreakpoints.Click += new System.EventHandler(this.mnuBreakOnFirstCycle_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuShowOptions
			// 
			this.mnuShowOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowToolbar,
            this.mnuShowCpuMemoryMapping,
            this.mnuShowPpuMemoryMapping,
            this.mnuShowFunctionLabelLists,
            this.mnuShowBottomPanel});
			this.mnuShowOptions.Image = global::Mesen.GUI.Properties.Resources.DipSwitches;
			this.mnuShowOptions.Name = "mnuShowOptions";
			this.mnuShowOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuShowOptions.Text = "Show...";
			// 
			// mnuShowToolbar
			// 
			this.mnuShowToolbar.CheckOnClick = true;
			this.mnuShowToolbar.Name = "mnuShowToolbar";
			this.mnuShowToolbar.Size = new System.Drawing.Size(263, 22);
			this.mnuShowToolbar.Text = "Show Toolbar";
			this.mnuShowToolbar.Click += new System.EventHandler(this.mnuShowToolbar_Click);
			// 
			// mnuShowCpuMemoryMapping
			// 
			this.mnuShowCpuMemoryMapping.CheckOnClick = true;
			this.mnuShowCpuMemoryMapping.Name = "mnuShowCpuMemoryMapping";
			this.mnuShowCpuMemoryMapping.Size = new System.Drawing.Size(263, 22);
			this.mnuShowCpuMemoryMapping.Text = "Show CPU Memory Mapping";
			this.mnuShowCpuMemoryMapping.Click += new System.EventHandler(this.mnuShowCpuMemoryMapping_Click);
			// 
			// mnuShowPpuMemoryMapping
			// 
			this.mnuShowPpuMemoryMapping.CheckOnClick = true;
			this.mnuShowPpuMemoryMapping.Name = "mnuShowPpuMemoryMapping";
			this.mnuShowPpuMemoryMapping.Size = new System.Drawing.Size(263, 22);
			this.mnuShowPpuMemoryMapping.Text = "Show PPU Memory Mapping";
			this.mnuShowPpuMemoryMapping.Click += new System.EventHandler(this.mnuShowPpuMemoryMapping_Click);
			// 
			// mnuShowFunctionLabelLists
			// 
			this.mnuShowFunctionLabelLists.CheckOnClick = true;
			this.mnuShowFunctionLabelLists.Name = "mnuShowFunctionLabelLists";
			this.mnuShowFunctionLabelLists.Size = new System.Drawing.Size(263, 22);
			this.mnuShowFunctionLabelLists.Text = "Show Function/Label Lists";
			this.mnuShowFunctionLabelLists.Click += new System.EventHandler(this.mnuShowFunctionLabelLists_Click);
			// 
			// mnuShowBottomPanel
			// 
			this.mnuShowBottomPanel.CheckOnClick = true;
			this.mnuShowBottomPanel.Name = "mnuShowBottomPanel";
			this.mnuShowBottomPanel.Size = new System.Drawing.Size(263, 22);
			this.mnuShowBottomPanel.Text = "Show Watch/Breakpoints/Call Stack";
			this.mnuShowBottomPanel.Click += new System.EventHandler(this.mnuShowBottomPanel_Click);
			// 
			// mnuTooltipOptions
			// 
			this.mnuTooltipOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowCodePreview,
            this.mnuShowOpCodeTooltips,
            this.toolStripMenuItem18,
            this.mnuOnlyShowTooltipOnShift});
			this.mnuTooltipOptions.Image = global::Mesen.GUI.Properties.Resources.Comment;
			this.mnuTooltipOptions.Name = "mnuTooltipOptions";
			this.mnuTooltipOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuTooltipOptions.Text = "Tooltip Options";
			// 
			// mnuShowCodePreview
			// 
			this.mnuShowCodePreview.CheckOnClick = true;
			this.mnuShowCodePreview.Name = "mnuShowCodePreview";
			this.mnuShowCodePreview.Size = new System.Drawing.Size(307, 22);
			this.mnuShowCodePreview.Text = "Show Code Preview in Tooltips";
			this.mnuShowCodePreview.CheckedChanged += new System.EventHandler(this.mnuShowCodePreview_CheckedChanged);
			// 
			// mnuShowOpCodeTooltips
			// 
			this.mnuShowOpCodeTooltips.CheckOnClick = true;
			this.mnuShowOpCodeTooltips.Name = "mnuShowOpCodeTooltips";
			this.mnuShowOpCodeTooltips.Size = new System.Drawing.Size(307, 22);
			this.mnuShowOpCodeTooltips.Text = "Show OP Code Info Tooltips";
			this.mnuShowOpCodeTooltips.CheckedChanged += new System.EventHandler(this.mnuShowOpCodeTooltips_CheckedChanged);
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			this.toolStripMenuItem18.Size = new System.Drawing.Size(304, 6);
			// 
			// mnuOnlyShowTooltipOnShift
			// 
			this.mnuOnlyShowTooltipOnShift.CheckOnClick = true;
			this.mnuOnlyShowTooltipOnShift.Name = "mnuOnlyShowTooltipOnShift";
			this.mnuOnlyShowTooltipOnShift.Size = new System.Drawing.Size(307, 22);
			this.mnuOnlyShowTooltipOnShift.Text = "Only show tooltips when Shift key is pressed";
			this.mnuOnlyShowTooltipOnShift.CheckedChanged += new System.EventHandler(this.mnuTooltipShowOnShift_CheckedChanged);
			// 
			// mnuCopyOptions
			// 
			this.mnuCopyOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyAddresses,
            this.mnuCopyByteCode,
            this.mnuCopyComments});
			this.mnuCopyOptions.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCopyOptions.Name = "mnuCopyOptions";
			this.mnuCopyOptions.Size = new System.Drawing.Size(266, 22);
			this.mnuCopyOptions.Text = "Copy Options";
			// 
			// mnuCopyAddresses
			// 
			this.mnuCopyAddresses.CheckOnClick = true;
			this.mnuCopyAddresses.Name = "mnuCopyAddresses";
			this.mnuCopyAddresses.Size = new System.Drawing.Size(164, 22);
			this.mnuCopyAddresses.Text = "Copy Addresses";
			this.mnuCopyAddresses.Click += new System.EventHandler(this.mnuCopyAddresses_Click);
			// 
			// mnuCopyByteCode
			// 
			this.mnuCopyByteCode.CheckOnClick = true;
			this.mnuCopyByteCode.Name = "mnuCopyByteCode";
			this.mnuCopyByteCode.Size = new System.Drawing.Size(164, 22);
			this.mnuCopyByteCode.Text = "Copy Byte Code";
			this.mnuCopyByteCode.Click += new System.EventHandler(this.mnuCopyByteCode_Click);
			// 
			// mnuCopyComments
			// 
			this.mnuCopyComments.CheckOnClick = true;
			this.mnuCopyComments.Name = "mnuCopyComments";
			this.mnuCopyComments.Size = new System.Drawing.Size(164, 22);
			this.mnuCopyComments.Text = "Copy Comments";
			this.mnuCopyComments.Click += new System.EventHandler(this.mnuCopyComments_Click);
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
			this.mnuIncreaseFontSize.ShortcutKeyDisplayString = "";
			this.mnuIncreaseFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuIncreaseFontSize.Text = "Increase Size";
			this.mnuIncreaseFontSize.Click += new System.EventHandler(this.mnuIncreaseFontSize_Click);
			// 
			// mnuDecreaseFontSize
			// 
			this.mnuDecreaseFontSize.Name = "mnuDecreaseFontSize";
			this.mnuDecreaseFontSize.ShortcutKeyDisplayString = "";
			this.mnuDecreaseFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuDecreaseFontSize.Text = "Decrease Size";
			this.mnuDecreaseFontSize.Click += new System.EventHandler(this.mnuDecreaseFontSize_Click);
			// 
			// mnuResetFontSize
			// 
			this.mnuResetFontSize.Name = "mnuResetFontSize";
			this.mnuResetFontSize.ShortcutKeyDisplayString = "";
			this.mnuResetFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuResetFontSize.Text = "Reset to Default";
			this.mnuResetFontSize.Click += new System.EventHandler(this.mnuResetFontSize_Click);
			// 
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(154, 6);
			// 
			// mnuSelectFont
			// 
			this.mnuSelectFont.Name = "mnuSelectFont";
			this.mnuSelectFont.Size = new System.Drawing.Size(157, 22);
			this.mnuSelectFont.Text = "Select Font...";
			this.mnuSelectFont.Click += new System.EventHandler(this.mnuSelectFont_Click);
			// 
			// mnuConfigureColors
			// 
			this.mnuConfigureColors.Image = global::Mesen.GUI.Properties.Resources.PipetteSmall;
			this.mnuConfigureColors.Name = "mnuConfigureColors";
			this.mnuConfigureColors.Size = new System.Drawing.Size(266, 22);
			this.mnuConfigureColors.Text = "Configure Colors";
			this.mnuConfigureColors.Click += new System.EventHandler(this.mnuConfigureColors_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(263, 6);
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
			// mnuUseVerticalLayout
			// 
			this.mnuUseVerticalLayout.CheckOnClick = true;
			this.mnuUseVerticalLayout.Image = global::Mesen.GUI.Properties.Resources.VerticalLayout;
			this.mnuUseVerticalLayout.Name = "mnuUseVerticalLayout";
			this.mnuUseVerticalLayout.Size = new System.Drawing.Size(266, 22);
			this.mnuUseVerticalLayout.Text = "Use Vertical Layout";
			this.mnuUseVerticalLayout.CheckedChanged += new System.EventHandler(this.mnuUseVerticalLayout_CheckedChanged);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuAutoCreateJumpLabels
			// 
			this.mnuAutoCreateJumpLabels.CheckOnClick = true;
			this.mnuAutoCreateJumpLabels.Name = "mnuAutoCreateJumpLabels";
			this.mnuAutoCreateJumpLabels.Size = new System.Drawing.Size(266, 22);
			this.mnuAutoCreateJumpLabels.Text = "Auto-create jump labels";
			this.mnuAutoCreateJumpLabels.Click += new System.EventHandler(this.mnuAutoCreateJumpLabels_Click);
			// 
			// toolStripMenuItem25
			// 
			this.toolStripMenuItem25.Name = "toolStripMenuItem25";
			this.toolStripMenuItem25.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuHidePauseIcon
			// 
			this.mnuHidePauseIcon.CheckOnClick = true;
			this.mnuHidePauseIcon.Name = "mnuHidePauseIcon";
			this.mnuHidePauseIcon.Size = new System.Drawing.Size(266, 22);
			this.mnuHidePauseIcon.Text = "Hide Pause Icon";
			this.mnuHidePauseIcon.Click += new System.EventHandler(this.mnuHidePauseIcon_Click);
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
			// mnuShowBreakNotifications
			// 
			this.mnuShowBreakNotifications.CheckOnClick = true;
			this.mnuShowBreakNotifications.Name = "mnuShowBreakNotifications";
			this.mnuShowBreakNotifications.Size = new System.Drawing.Size(266, 22);
			this.mnuShowBreakNotifications.Text = "Show break notifications";
			this.mnuShowBreakNotifications.Click += new System.EventHandler(this.mnuShowBreakNotifications_Click);
			// 
			// mnuShowInstructionProgression
			// 
			this.mnuShowInstructionProgression.CheckOnClick = true;
			this.mnuShowInstructionProgression.Name = "mnuShowInstructionProgression";
			this.mnuShowInstructionProgression.Size = new System.Drawing.Size(266, 22);
			this.mnuShowInstructionProgression.Text = "Show instruction progression";
			this.mnuShowInstructionProgression.Click += new System.EventHandler(this.mnuShowInstructionProgression_Click);
			// 
			// mnuShowSelectionLength
			// 
			this.mnuShowSelectionLength.CheckOnClick = true;
			this.mnuShowSelectionLength.Name = "mnuShowSelectionLength";
			this.mnuShowSelectionLength.Size = new System.Drawing.Size(266, 22);
			this.mnuShowSelectionLength.Text = "Show selection length";
			this.mnuShowSelectionLength.Click += new System.EventHandler(this.mnuShowSelectionLength_Click);
			// 
			// toolStripMenuItem27
			// 
			this.toolStripMenuItem27.Name = "toolStripMenuItem27";
			this.toolStripMenuItem27.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuAlwaysScrollToCenter
			// 
			this.mnuAlwaysScrollToCenter.CheckOnClick = true;
			this.mnuAlwaysScrollToCenter.Name = "mnuAlwaysScrollToCenter";
			this.mnuAlwaysScrollToCenter.Size = new System.Drawing.Size(266, 22);
			this.mnuAlwaysScrollToCenter.Text = "Keep active statement in the center";
			this.mnuAlwaysScrollToCenter.Click += new System.EventHandler(this.mnuAlwaysScrollToCenter_Click);
			// 
			// mnuRefreshWhileRunning
			// 
			this.mnuRefreshWhileRunning.CheckOnClick = true;
			this.mnuRefreshWhileRunning.Name = "mnuRefreshWhileRunning";
			this.mnuRefreshWhileRunning.Size = new System.Drawing.Size(266, 22);
			this.mnuRefreshWhileRunning.Text = "Refresh UI while running";
			this.mnuRefreshWhileRunning.Click += new System.EventHandler(this.mnuRefreshWhileRunning_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(263, 6);
			// 
			// mnuConfigureExternalEditor
			// 
			this.mnuConfigureExternalEditor.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuConfigureExternalEditor.Name = "mnuConfigureExternalEditor";
			this.mnuConfigureExternalEditor.Size = new System.Drawing.Size(266, 22);
			this.mnuConfigureExternalEditor.Text = "Configure external code editor...";
			this.mnuConfigureExternalEditor.Click += new System.EventHandler(this.mnuConfigureExternalEditor_Click);
			// 
			// mnuPreferences
			// 
			this.mnuPreferences.Image = global::Mesen.GUI.Properties.Resources.Cog;
			this.mnuPreferences.Name = "mnuPreferences";
			this.mnuPreferences.Size = new System.Drawing.Size(266, 22);
			this.mnuPreferences.Text = "Configure shortcut keys...";
			this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApuViewer,
            this.mnuAssembler,
            this.mnuEventViewer,
            this.mnuMemoryViewer,
            this.mnuProfiler,
            this.mnuPpuViewer,
            this.mnuScriptWindow,
            this.mnuTextHooker,
            this.mnuTraceLogger,
            this.mnuWatchWindow,
            this.toolStripMenuItem13,
            this.pPUViewerCompactToolStripMenuItem,
            this.toolStripMenuItem17,
            this.mnuEditHeader,
            this.toolStripMenuItem30,
            this.mnuCodeDataLogger});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// mnuApuViewer
			// 
			this.mnuApuViewer.Image = global::Mesen.GUI.Properties.Resources.Audio;
			this.mnuApuViewer.Name = "mnuApuViewer";
			this.mnuApuViewer.Size = new System.Drawing.Size(194, 22);
			this.mnuApuViewer.Text = "APU Viewer";
			this.mnuApuViewer.Click += new System.EventHandler(this.mnuApuViewer_Click);
			// 
			// mnuAssembler
			// 
			this.mnuAssembler.Image = global::Mesen.GUI.Properties.Resources.Chip;
			this.mnuAssembler.Name = "mnuAssembler";
			this.mnuAssembler.Size = new System.Drawing.Size(194, 22);
			this.mnuAssembler.Text = "Assembler";
			this.mnuAssembler.Click += new System.EventHandler(this.mnuAssembler_Click);
			// 
			// mnuEventViewer
			// 
			this.mnuEventViewer.Image = global::Mesen.GUI.Properties.Resources.NesEventViewer;
			this.mnuEventViewer.Name = "mnuEventViewer";
			this.mnuEventViewer.Size = new System.Drawing.Size(194, 22);
			this.mnuEventViewer.Text = "Event Viewer";
			this.mnuEventViewer.Click += new System.EventHandler(this.mnuEventViewer_Click);
			// 
			// mnuMemoryViewer
			// 
			this.mnuMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuMemoryViewer.Name = "mnuMemoryViewer";
			this.mnuMemoryViewer.Size = new System.Drawing.Size(194, 22);
			this.mnuMemoryViewer.Text = "Memory Tools";
			this.mnuMemoryViewer.Click += new System.EventHandler(this.mnuMemoryViewer_Click);
			// 
			// mnuProfiler
			// 
			this.mnuProfiler.Image = global::Mesen.GUI.Properties.Resources.Speed;
			this.mnuProfiler.Name = "mnuProfiler";
			this.mnuProfiler.Size = new System.Drawing.Size(194, 22);
			this.mnuProfiler.Text = "Performance Profiler";
			this.mnuProfiler.Click += new System.EventHandler(this.mnuProfiler_Click);
			// 
			// mnuPpuViewer
			// 
			this.mnuPpuViewer.Image = global::Mesen.GUI.Properties.Resources.Video;
			this.mnuPpuViewer.Name = "mnuPpuViewer";
			this.mnuPpuViewer.Size = new System.Drawing.Size(194, 22);
			this.mnuPpuViewer.Text = "PPU Viewer";
			this.mnuPpuViewer.Click += new System.EventHandler(this.mnuNametableViewer_Click);
			// 
			// mnuScriptWindow
			// 
			this.mnuScriptWindow.Image = global::Mesen.GUI.Properties.Resources.Script;
			this.mnuScriptWindow.Name = "mnuScriptWindow";
			this.mnuScriptWindow.Size = new System.Drawing.Size(194, 22);
			this.mnuScriptWindow.Text = "Script Window";
			this.mnuScriptWindow.Click += new System.EventHandler(this.mnuScriptWindow_Click);
			// 
			// mnuTextHooker
			// 
			this.mnuTextHooker.Image = global::Mesen.GUI.Properties.Resources.Font;
			this.mnuTextHooker.Name = "mnuTextHooker";
			this.mnuTextHooker.Size = new System.Drawing.Size(194, 22);
			this.mnuTextHooker.Text = "Text Hooker";
			this.mnuTextHooker.Click += new System.EventHandler(this.mnuTextHooker_Click);
			// 
			// mnuTraceLogger
			// 
			this.mnuTraceLogger.Image = global::Mesen.GUI.Properties.Resources.LogWindow;
			this.mnuTraceLogger.Name = "mnuTraceLogger";
			this.mnuTraceLogger.Size = new System.Drawing.Size(194, 22);
			this.mnuTraceLogger.Text = "Trace Logger";
			this.mnuTraceLogger.Click += new System.EventHandler(this.mnuTraceLogger_Click);
			// 
			// mnuWatchWindow
			// 
			this.mnuWatchWindow.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuWatchWindow.Name = "mnuWatchWindow";
			this.mnuWatchWindow.Size = new System.Drawing.Size(194, 22);
			this.mnuWatchWindow.Text = "Watch Window";
			this.mnuWatchWindow.Click += new System.EventHandler(this.mnuWatchWindow_Click);
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(191, 6);
			// 
			// pPUViewerCompactToolStripMenuItem
			// 
			this.pPUViewerCompactToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenNametableViewer,
            this.mnuOpenChrViewer,
            this.mnuOpenSpriteViewer,
            this.mnuOpenPaletteViewer});
			this.pPUViewerCompactToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.VideoFilter;
			this.pPUViewerCompactToolStripMenuItem.Name = "pPUViewerCompactToolStripMenuItem";
			this.pPUViewerCompactToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.pPUViewerCompactToolStripMenuItem.Text = "PPU Viewer (Compact)";
			// 
			// mnuOpenNametableViewer
			// 
			this.mnuOpenNametableViewer.Name = "mnuOpenNametableViewer";
			this.mnuOpenNametableViewer.Size = new System.Drawing.Size(170, 22);
			this.mnuOpenNametableViewer.Text = "Nametable Viewer";
			this.mnuOpenNametableViewer.Click += new System.EventHandler(this.mnuOpenNametableViewer_Click);
			// 
			// mnuOpenChrViewer
			// 
			this.mnuOpenChrViewer.Name = "mnuOpenChrViewer";
			this.mnuOpenChrViewer.Size = new System.Drawing.Size(170, 22);
			this.mnuOpenChrViewer.Text = "CHR Viewer";
			this.mnuOpenChrViewer.Click += new System.EventHandler(this.mnuOpenChrViewer_Click);
			// 
			// mnuOpenSpriteViewer
			// 
			this.mnuOpenSpriteViewer.Name = "mnuOpenSpriteViewer";
			this.mnuOpenSpriteViewer.Size = new System.Drawing.Size(170, 22);
			this.mnuOpenSpriteViewer.Text = "Sprite Viewer";
			this.mnuOpenSpriteViewer.Click += new System.EventHandler(this.mnuOpenSpriteViewer_Click);
			// 
			// mnuOpenPaletteViewer
			// 
			this.mnuOpenPaletteViewer.Name = "mnuOpenPaletteViewer";
			this.mnuOpenPaletteViewer.Size = new System.Drawing.Size(170, 22);
			this.mnuOpenPaletteViewer.Text = "Palette Viewer";
			this.mnuOpenPaletteViewer.Click += new System.EventHandler(this.mnuOpenPaletteViewer_Click);
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(191, 6);
			// 
			// mnuEditHeader
			// 
			this.mnuEditHeader.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditHeader.Name = "mnuEditHeader";
			this.mnuEditHeader.Size = new System.Drawing.Size(194, 22);
			this.mnuEditHeader.Text = "Edit iNES Header";
			this.mnuEditHeader.Click += new System.EventHandler(this.mnuEditHeader_Click);
			// 
			// toolStripMenuItem30
			// 
			this.toolStripMenuItem30.Name = "toolStripMenuItem30";
			this.toolStripMenuItem30.Size = new System.Drawing.Size(191, 6);
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
			this.mnuCodeDataLogger.Size = new System.Drawing.Size(194, 22);
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
			this.statusStrip.Location = new System.Drawing.Point(0, 660);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1075, 24);
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
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(340, 19);
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
			this.ctrlPpuMemoryMapping.Location = new System.Drawing.Point(0, 627);
			this.ctrlPpuMemoryMapping.Name = "ctrlPpuMemoryMapping";
			this.ctrlPpuMemoryMapping.Size = new System.Drawing.Size(1075, 33);
			this.ctrlPpuMemoryMapping.TabIndex = 5;
			this.ctrlPpuMemoryMapping.Text = "ctrlMemoryMapping1";
			this.ctrlPpuMemoryMapping.Visible = false;
			// 
			// ctrlCpuMemoryMapping
			// 
			this.ctrlCpuMemoryMapping.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ctrlCpuMemoryMapping.Location = new System.Drawing.Point(0, 594);
			this.ctrlCpuMemoryMapping.Name = "ctrlCpuMemoryMapping";
			this.ctrlCpuMemoryMapping.Size = new System.Drawing.Size(1075, 33);
			this.ctrlCpuMemoryMapping.TabIndex = 4;
			this.ctrlCpuMemoryMapping.Text = "ctrlMemoryMapping1";
			this.ctrlCpuMemoryMapping.Visible = false;
			// 
			// tsToolbar
			// 
			this.tsToolbar.Location = new System.Drawing.Point(0, 24);
			this.tsToolbar.Name = "tsToolbar";
			this.tsToolbar.Size = new System.Drawing.Size(1075, 25);
			this.tsToolbar.TabIndex = 6;
			this.tsToolbar.Text = "toolStrip1";
			this.tsToolbar.Visible = false;
			// 
			// frmDebugger
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1075, 684);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.ctrlCpuMemoryMapping);
			this.Controls.Add(this.ctrlPpuMemoryMapping);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.tsToolbar);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(850, 685);
			this.Name = "frmDebugger";
			this.Text = "Debugger";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmDebugger_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmDebugger_DragEnter);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ctrlSplitContainerTop.Panel1.ResumeLayout(false);
			this.ctrlSplitContainerTop.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplitContainerTop)).EndInit();
			this.ctrlSplitContainerTop.ResumeLayout(false);
			this.tlpTop.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripMenuItem mnuSearch;
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
		private System.Windows.Forms.ToolStripMenuItem mnuTraceLogger;
		private System.Windows.Forms.ToolStripMenuItem mnuRunPpuCycle;
		private System.Windows.Forms.ToolStripMenuItem mnuPpuPartialDraw;
		private System.Windows.Forms.ToolStripMenuItem mnuRunScanline;
		private System.Windows.Forms.PictureBox picWatchHelp;
		private Controls.ctrlMemoryMapping ctrlCpuMemoryMapping;
		private Controls.ctrlMemoryMapping ctrlPpuMemoryMapping;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCpuMemoryMapping;
		private System.Windows.Forms.ToolStripMenuItem mnuShowPpuMemoryMapping;
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
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshWhileRunning;
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
		private System.Windows.Forms.ToolStripMenuItem mnuShowOpCodeTooltips;
		private System.Windows.Forms.ToolStripMenuItem mnuShow;
		private System.Windows.Forms.ToolStripMenuItem mnuShowVerifiedCode;
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
		private ctrlSourceViewer ctrlSourceViewer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private ctrlSourceViewer ctrlSourceViewerSplit;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectFont;
		private System.Windows.Forms.ToolStripMenuItem mnuHidePauseIcon;
		private System.Windows.Forms.ToolStripMenuItem mnuResetLabels;
		private System.Windows.Forms.ToolStripMenuItem mnuShowOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyAddresses;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyByteCode;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOn;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyComments;
		private System.Windows.Forms.ToolStripMenuItem mnuImportSettings;
		private System.Windows.Forms.ToolStripMenuItem mnuReset;
		private System.Windows.Forms.ToolStripMenuItem mnuPowerCycle;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
		private System.Windows.Forms.ToolStripMenuItem mnuTooltipOptions;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
		private System.Windows.Forms.ToolStripMenuItem mnuOnlyShowTooltipOnShift;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tlpVerticalLayout;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mnuUseVerticalLayout;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnUninitMemoryRead;
		private System.Windows.Forms.ToolStripMenuItem mnuTextHooker;
		private System.Windows.Forms.ToolStripSeparator sepBreakNsfOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnDecayedOamRead;
		private System.Windows.Forms.ToolStripMenuItem mnuAlwaysScrollToCenter;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnInit;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnPlay;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem26;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoCreateJumpLabels;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem25;
		private System.Windows.Forms.ToolStripMenuItem mnuShowBreakNotifications;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInstructionProgression;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem27;
		private System.Windows.Forms.ToolStripMenuItem mnuEnableSubInstructionBreakpoints;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem28;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToAll;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem29;
		private System.Windows.Forms.ToolStripMenuItem mnuConfigureExternalEditor;
		private System.Windows.Forms.ToolStripMenuItem pPUViewerCompactToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenChrViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenNametableViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenSpriteViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenPaletteViewer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem30;
		private System.Windows.Forms.ToolStripMenuItem mnuProfiler;
		private System.Windows.Forms.ToolStripMenuItem mnuRunCpuCycle;
		private System.Windows.Forms.ToolStripMenuItem mnuShowSelectionLength;
		private System.Windows.Forms.ToolStripMenuItem mnuWatchWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnPpu2006ScrollGlitch;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnBusConflict;
	  private System.Windows.Forms.ToolStripMenuItem mnuGoToAddress;
   }
}