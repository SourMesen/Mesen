namespace Mesen.GUI.Debugger
{
	partial class frmMemoryViewer
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
			if(this._notifListener != null) {
				this._notifListener.Dispose();
				this._notifListener = null;
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
			this.ctrlHexViewer = new Mesen.GUI.Debugger.Controls.ctrlHexViewer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblViewMemoryType = new System.Windows.Forms.Label();
			this.cboMemoryType = new System.Windows.Forms.ComboBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuLoadTblFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetTblMappings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightExecution = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightWrites = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHightlightReads = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.fadeSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFadeSlow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFadeNormal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFadeFast = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFadeNever = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCustomFadeSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.dataTypeHighlightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightLabelledBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHighlightCodeBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightDataBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightDmcDataBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHighlightChrDrawnBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHighlightChrReadBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.fadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideUnusedBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideReadBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideWrittenBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideExecutedBytes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuConfigureColors = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCharacters = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowLabelInfoOnMouseOver = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnImport = new System.Windows.Forms.ToolStripButton();
			this.btnExport = new System.Windows.Forms.ToolStripButton();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgMemoryViewer = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tpgAccessCounters = new System.Windows.Forms.TabPage();
			this.ctrlMemoryAccessCounters = new Mesen.GUI.Debugger.Controls.ctrlMemoryAccessCounters();
			this.tpgProfiler = new System.Windows.Forms.TabPage();
			this.ctrlProfiler = new Mesen.GUI.Debugger.Controls.ctrlProfiler();
			this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
			this.flowLayoutPanel1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgMemoryViewer.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tpgAccessCounters.SuspendLayout();
			this.tpgProfiler.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctrlHexViewer
			// 
			this.ctrlHexViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlHexViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlHexViewer.Name = "ctrlHexViewer";
			this.ctrlHexViewer.Size = new System.Drawing.Size(606, 318);
			this.ctrlHexViewer.TabIndex = 0;
			this.ctrlHexViewer.RequiredWidthChanged += new System.EventHandler(this.ctrlHexViewer_RequiredWidthChanged);
			this.ctrlHexViewer.InitializeContextMenu += new System.EventHandler(this.ctrlHexViewer_InitializeContextMenu);
			this.ctrlHexViewer.ByteChanged += new Be.Windows.Forms.DynamicByteProvider.ByteChangedHandler(this.ctrlHexViewer_ByteChanged);
			this.ctrlHexViewer.ByteMouseHover += new Mesen.GUI.Debugger.Controls.ctrlHexViewer.ByteMouseHoverHandler(this.ctrlHexViewer_ByteMouseHover);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.lblViewMemoryType);
			this.flowLayoutPanel1.Controls.Add(this.cboMemoryType);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(166, 27);
			this.flowLayoutPanel1.TabIndex = 1;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// lblViewMemoryType
			// 
			this.lblViewMemoryType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblViewMemoryType.AutoSize = true;
			this.lblViewMemoryType.Location = new System.Drawing.Point(3, 7);
			this.lblViewMemoryType.Name = "lblViewMemoryType";
			this.lblViewMemoryType.Size = new System.Drawing.Size(33, 13);
			this.lblViewMemoryType.TabIndex = 0;
			this.lblViewMemoryType.Text = "View:";
			// 
			// cboMemoryType
			// 
			this.cboMemoryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMemoryType.FormattingEnabled = true;
			this.cboMemoryType.Items.AddRange(new object[] {
            "CPU Memory",
            "PPU Memory",
            "Palette Memory",
            "OAM Memory",
            "Secondary OAM Memory",
            "PRG ROM",
            "CHR ROM",
            "CHR RAM",
            "Work RAM",
            "Save RAM",
            "NES RAM"});
			this.cboMemoryType.Location = new System.Drawing.Point(42, 3);
			this.cboMemoryType.Name = "cboMemoryType";
			this.cboMemoryType.Size = new System.Drawing.Size(121, 21);
			this.cboMemoryType.TabIndex = 1;
			this.cboMemoryType.SelectedIndexChanged += new System.EventHandler(this.cboMemoryType_SelectedIndexChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuView,
            this.toolStripMenuItem1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(614, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImport,
            this.mnuExport,
            this.toolStripMenuItem3,
            this.mnuLoadTblFile,
            this.mnuResetTblMappings,
            this.toolStripMenuItem4,
            this.mnuClose});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuImport
			// 
			this.mnuImport.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuImport.Name = "mnuImport";
			this.mnuImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuImport.Size = new System.Drawing.Size(181, 22);
			this.mnuImport.Text = "Import";
			this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
			// 
			// mnuExport
			// 
			this.mnuExport.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExport.Name = "mnuExport";
			this.mnuExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuExport.Size = new System.Drawing.Size(181, 22);
			this.mnuExport.Text = "Export";
			this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuLoadTblFile
			// 
			this.mnuLoadTblFile.Name = "mnuLoadTblFile";
			this.mnuLoadTblFile.Size = new System.Drawing.Size(181, 22);
			this.mnuLoadTblFile.Text = "Load TBL file";
			this.mnuLoadTblFile.Click += new System.EventHandler(this.mnuLoadTblFile_Click);
			// 
			// mnuResetTblMappings
			// 
			this.mnuResetTblMappings.Name = "mnuResetTblMappings";
			this.mnuResetTblMappings.Size = new System.Drawing.Size(181, 22);
			this.mnuResetTblMappings.Text = "Reset TBL mappings";
			this.mnuResetTblMappings.Click += new System.EventHandler(this.mnuResetTblMappings_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(181, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highlightToolStripMenuItem,
            this.dataTypeHighlightingToolStripMenuItem,
            this.fadeToolStripMenuItem,
            this.toolStripMenuItem5,
            this.mnuConfigureColors,
            this.fontSizeToolStripMenuItem,
            this.mnuRefresh,
            this.toolStripMenuItem2,
            this.mnuAutoRefresh,
            this.mnuShowCharacters,
            this.mnuShowLabelInfoOnMouseOver});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(44, 20);
			this.mnuView.Text = "View";
			// 
			// highlightToolStripMenuItem
			// 
			this.highlightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHighlightExecution,
            this.mnuHighlightWrites,
            this.mnuHightlightReads,
            this.toolStripMenuItem6,
            this.fadeSpeedToolStripMenuItem});
			this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
			this.highlightToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.highlightToolStripMenuItem.Text = "Memory Access Highlighting";
			// 
			// mnuHighlightExecution
			// 
			this.mnuHighlightExecution.CheckOnClick = true;
			this.mnuHighlightExecution.Name = "mnuHighlightExecution";
			this.mnuHighlightExecution.Size = new System.Drawing.Size(133, 22);
			this.mnuHighlightExecution.Text = "Execution";
			this.mnuHighlightExecution.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// mnuHighlightWrites
			// 
			this.mnuHighlightWrites.CheckOnClick = true;
			this.mnuHighlightWrites.Name = "mnuHighlightWrites";
			this.mnuHighlightWrites.Size = new System.Drawing.Size(133, 22);
			this.mnuHighlightWrites.Text = "Writes";
			this.mnuHighlightWrites.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// mnuHightlightReads
			// 
			this.mnuHightlightReads.CheckOnClick = true;
			this.mnuHightlightReads.Name = "mnuHightlightReads";
			this.mnuHightlightReads.Size = new System.Drawing.Size(133, 22);
			this.mnuHightlightReads.Text = "Reads";
			this.mnuHightlightReads.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(130, 6);
			// 
			// fadeSpeedToolStripMenuItem
			// 
			this.fadeSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFadeSlow,
            this.mnuFadeNormal,
            this.mnuFadeFast,
            this.mnuFadeNever,
            this.toolStripMenuItem7,
            this.mnuCustomFadeSpeed});
			this.fadeSpeedToolStripMenuItem.Name = "fadeSpeedToolStripMenuItem";
			this.fadeSpeedToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.fadeSpeedToolStripMenuItem.Text = "Fade speed";
			// 
			// mnuFadeSlow
			// 
			this.mnuFadeSlow.Name = "mnuFadeSlow";
			this.mnuFadeSlow.Size = new System.Drawing.Size(136, 22);
			this.mnuFadeSlow.Text = "Slow";
			this.mnuFadeSlow.Click += new System.EventHandler(this.mnuFadeSpeed_Click);
			// 
			// mnuFadeNormal
			// 
			this.mnuFadeNormal.Checked = true;
			this.mnuFadeNormal.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuFadeNormal.Name = "mnuFadeNormal";
			this.mnuFadeNormal.Size = new System.Drawing.Size(136, 22);
			this.mnuFadeNormal.Text = "Normal";
			this.mnuFadeNormal.Click += new System.EventHandler(this.mnuFadeSpeed_Click);
			// 
			// mnuFadeFast
			// 
			this.mnuFadeFast.Name = "mnuFadeFast";
			this.mnuFadeFast.Size = new System.Drawing.Size(136, 22);
			this.mnuFadeFast.Text = "Fast";
			this.mnuFadeFast.Click += new System.EventHandler(this.mnuFadeSpeed_Click);
			// 
			// mnuFadeNever
			// 
			this.mnuFadeNever.Name = "mnuFadeNever";
			this.mnuFadeNever.Size = new System.Drawing.Size(136, 22);
			this.mnuFadeNever.Text = "Do not fade";
			this.mnuFadeNever.Click += new System.EventHandler(this.mnuFadeSpeed_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(133, 6);
			// 
			// mnuCustomFadeSpeed
			// 
			this.mnuCustomFadeSpeed.Name = "mnuCustomFadeSpeed";
			this.mnuCustomFadeSpeed.Size = new System.Drawing.Size(136, 22);
			this.mnuCustomFadeSpeed.Text = "Custom...";
			this.mnuCustomFadeSpeed.Click += new System.EventHandler(this.mnuCustomFadeSpeed_Click);
			// 
			// dataTypeHighlightingToolStripMenuItem
			// 
			this.dataTypeHighlightingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHighlightLabelledBytes,
            this.toolStripMenuItem8,
            this.mnuHighlightCodeBytes,
            this.mnuHighlightDataBytes,
            this.mnuHighlightDmcDataBytes,
            this.toolStripMenuItem11,
            this.mnuHighlightChrDrawnBytes,
            this.mnuHighlightChrReadBytes});
			this.dataTypeHighlightingToolStripMenuItem.Name = "dataTypeHighlightingToolStripMenuItem";
			this.dataTypeHighlightingToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.dataTypeHighlightingToolStripMenuItem.Text = "Data Type Highlighting";
			// 
			// mnuHighlightLabelledBytes
			// 
			this.mnuHighlightLabelledBytes.CheckOnClick = true;
			this.mnuHighlightLabelledBytes.Name = "mnuHighlightLabelledBytes";
			this.mnuHighlightLabelledBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightLabelledBytes.Text = "Labelled bytes";
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(233, 6);
			// 
			// mnuHighlightCodeBytes
			// 
			this.mnuHighlightCodeBytes.CheckOnClick = true;
			this.mnuHighlightCodeBytes.Name = "mnuHighlightCodeBytes";
			this.mnuHighlightCodeBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightCodeBytes.Text = "Code bytes (PRG ROM)";
			// 
			// mnuHighlightDataBytes
			// 
			this.mnuHighlightDataBytes.CheckOnClick = true;
			this.mnuHighlightDataBytes.Name = "mnuHighlightDataBytes";
			this.mnuHighlightDataBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightDataBytes.Text = "Data bytes (PRG ROM)";
			// 
			// mnuHighlightDmcDataBytes
			// 
			this.mnuHighlightDmcDataBytes.CheckOnClick = true;
			this.mnuHighlightDmcDataBytes.Name = "mnuHighlightDmcDataBytes";
			this.mnuHighlightDmcDataBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightDmcDataBytes.Text = "DMC sample bytes (PRG ROM)";
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(233, 6);
			// 
			// mnuHighlightChrDrawnBytes
			// 
			this.mnuHighlightChrDrawnBytes.CheckOnClick = true;
			this.mnuHighlightChrDrawnBytes.Name = "mnuHighlightChrDrawnBytes";
			this.mnuHighlightChrDrawnBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightChrDrawnBytes.Text = "Drawn bytes (CHR ROM)";
			// 
			// mnuHighlightChrReadBytes
			// 
			this.mnuHighlightChrReadBytes.CheckOnClick = true;
			this.mnuHighlightChrReadBytes.Name = "mnuHighlightChrReadBytes";
			this.mnuHighlightChrReadBytes.Size = new System.Drawing.Size(236, 22);
			this.mnuHighlightChrReadBytes.Text = "Read bytes (CHR ROM)";
			// 
			// fadeToolStripMenuItem
			// 
			this.fadeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHideUnusedBytes,
            this.mnuHideReadBytes,
            this.mnuHideWrittenBytes,
            this.mnuHideExecutedBytes});
			this.fadeToolStripMenuItem.Name = "fadeToolStripMenuItem";
			this.fadeToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.fadeToolStripMenuItem.Text = "De-emphasize";
			// 
			// mnuHideUnusedBytes
			// 
			this.mnuHideUnusedBytes.CheckOnClick = true;
			this.mnuHideUnusedBytes.Name = "mnuHideUnusedBytes";
			this.mnuHideUnusedBytes.Size = new System.Drawing.Size(152, 22);
			this.mnuHideUnusedBytes.Text = "Unused bytes";
			this.mnuHideUnusedBytes.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// mnuHideReadBytes
			// 
			this.mnuHideReadBytes.CheckOnClick = true;
			this.mnuHideReadBytes.Name = "mnuHideReadBytes";
			this.mnuHideReadBytes.Size = new System.Drawing.Size(152, 22);
			this.mnuHideReadBytes.Text = "Read bytes";
			this.mnuHideReadBytes.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// mnuHideWrittenBytes
			// 
			this.mnuHideWrittenBytes.CheckOnClick = true;
			this.mnuHideWrittenBytes.Name = "mnuHideWrittenBytes";
			this.mnuHideWrittenBytes.Size = new System.Drawing.Size(152, 22);
			this.mnuHideWrittenBytes.Text = "Written bytes";
			this.mnuHideWrittenBytes.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// mnuHideExecutedBytes
			// 
			this.mnuHideExecutedBytes.CheckOnClick = true;
			this.mnuHideExecutedBytes.Name = "mnuHideExecutedBytes";
			this.mnuHideExecutedBytes.Size = new System.Drawing.Size(152, 22);
			this.mnuHideExecutedBytes.Text = "Executed bytes";
			this.mnuHideExecutedBytes.Click += new System.EventHandler(this.mnuColorProviderOptions_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(245, 6);
			// 
			// mnuConfigureColors
			// 
			this.mnuConfigureColors.Image = global::Mesen.GUI.Properties.Resources.PipetteSmall;
			this.mnuConfigureColors.Name = "mnuConfigureColors";
			this.mnuConfigureColors.Size = new System.Drawing.Size(248, 22);
			this.mnuConfigureColors.Text = "Configure Colors";
			this.mnuConfigureColors.Click += new System.EventHandler(this.mnuConfigureColors_Click);
			// 
			// fontSizeToolStripMenuItem
			// 
			this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIncreaseFontSize,
            this.mnuDecreaseFontSize,
            this.mnuResetFontSize});
			this.fontSizeToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.Font;
			this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
			this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
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
			// mnuRefresh
			// 
			this.mnuRefresh.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefresh.Size = new System.Drawing.Size(248, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(245, 6);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(248, 22);
			this.mnuAutoRefresh.Text = "Auto-refresh";
			this.mnuAutoRefresh.Click += new System.EventHandler(this.mnuAutoRefresh_Click);
			// 
			// mnuShowCharacters
			// 
			this.mnuShowCharacters.Checked = true;
			this.mnuShowCharacters.CheckOnClick = true;
			this.mnuShowCharacters.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuShowCharacters.Name = "mnuShowCharacters";
			this.mnuShowCharacters.Size = new System.Drawing.Size(248, 22);
			this.mnuShowCharacters.Text = "Show characters";
			// 
			// mnuShowLabelInfoOnMouseOver
			// 
			this.mnuShowLabelInfoOnMouseOver.CheckOnClick = true;
			this.mnuShowLabelInfoOnMouseOver.Name = "mnuShowLabelInfoOnMouseOver";
			this.mnuShowLabelInfoOnMouseOver.Size = new System.Drawing.Size(248, 22);
			this.mnuShowLabelInfoOnMouseOver.Text = "Show label tooltip on mouseover";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFind,
            this.mnuFindNext,
            this.mnuFindPrev,
            this.mnuGoTo});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(54, 20);
			this.toolStripMenuItem1.Text = "Search";
			// 
			// mnuFind
			// 
			this.mnuFind.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFind.Name = "mnuFind";
			this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.mnuFind.Size = new System.Drawing.Size(196, 22);
			this.mnuFind.Text = "Find...";
			this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
			// 
			// mnuFindNext
			// 
			this.mnuFindNext.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuFindNext.Name = "mnuFindNext";
			this.mnuFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.mnuFindNext.Size = new System.Drawing.Size(196, 22);
			this.mnuFindNext.Text = "Find Next";
			this.mnuFindNext.Click += new System.EventHandler(this.mnuFindNext_Click);
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Image = global::Mesen.GUI.Properties.Resources.PreviousArrow;
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.mnuFindPrev.Size = new System.Drawing.Size(196, 22);
			this.mnuFindPrev.Text = "Find Previous";
			this.mnuFindPrev.Click += new System.EventHandler(this.mnuFindPrev_Click);
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.mnuGoTo.Size = new System.Drawing.Size(196, 22);
			this.mnuGoTo.Text = "Go To...";
			this.mnuGoTo.Click += new System.EventHandler(this.mnuGoTo_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImport,
            this.btnExport});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(606, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnImport
			// 
			this.btnImport.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(63, 22);
			this.btnImport.Text = "Import";
			this.btnImport.Click += new System.EventHandler(this.mnuImport_Click);
			// 
			// btnExport
			// 
			this.btnExport.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(60, 22);
			this.btnExport.Text = "Export";
			this.btnExport.Click += new System.EventHandler(this.mnuExport_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgMemoryViewer);
			this.tabMain.Controls.Add(this.tpgAccessCounters);
			this.tabMain.Controls.Add(this.tpgProfiler);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 24);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(614, 369);
			this.tabMain.TabIndex = 4;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tpgMemoryViewer
			// 
			this.tpgMemoryViewer.Controls.Add(this.panel1);
			this.tpgMemoryViewer.Controls.Add(this.toolStrip1);
			this.tpgMemoryViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgMemoryViewer.Name = "tpgMemoryViewer";
			this.tpgMemoryViewer.Size = new System.Drawing.Size(606, 343);
			this.tpgMemoryViewer.TabIndex = 0;
			this.tpgMemoryViewer.Text = "Memory Viewer";
			this.tpgMemoryViewer.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.flowLayoutPanel1);
			this.panel1.Controls.Add(this.ctrlHexViewer);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(606, 318);
			this.panel1.TabIndex = 4;
			// 
			// tpgAccessCounters
			// 
			this.tpgAccessCounters.Controls.Add(this.ctrlMemoryAccessCounters);
			this.tpgAccessCounters.Location = new System.Drawing.Point(4, 22);
			this.tpgAccessCounters.Name = "tpgAccessCounters";
			this.tpgAccessCounters.Size = new System.Drawing.Size(606, 343);
			this.tpgAccessCounters.TabIndex = 1;
			this.tpgAccessCounters.Text = "Access Counters";
			this.tpgAccessCounters.UseVisualStyleBackColor = true;
			// 
			// ctrlMemoryAccessCounters
			// 
			this.ctrlMemoryAccessCounters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlMemoryAccessCounters.Location = new System.Drawing.Point(0, 0);
			this.ctrlMemoryAccessCounters.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlMemoryAccessCounters.Name = "ctrlMemoryAccessCounters";
			this.ctrlMemoryAccessCounters.Size = new System.Drawing.Size(606, 343);
			this.ctrlMemoryAccessCounters.TabIndex = 0;
			// 
			// tpgProfiler
			// 
			this.tpgProfiler.Controls.Add(this.ctrlProfiler);
			this.tpgProfiler.Location = new System.Drawing.Point(4, 22);
			this.tpgProfiler.Name = "tpgProfiler";
			this.tpgProfiler.Size = new System.Drawing.Size(606, 343);
			this.tpgProfiler.TabIndex = 2;
			this.tpgProfiler.Text = "Profiler";
			this.tpgProfiler.UseVisualStyleBackColor = true;
			// 
			// ctrlProfiler
			// 
			this.ctrlProfiler.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlProfiler.Location = new System.Drawing.Point(0, 0);
			this.ctrlProfiler.Name = "ctrlProfiler";
			this.ctrlProfiler.Size = new System.Drawing.Size(606, 343);
			this.ctrlProfiler.TabIndex = 0;
			// 
			// tmrRefresh
			// 
			this.tmrRefresh.Enabled = true;
			this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
			// 
			// frmMemoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(614, 393);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(429, 337);
			this.Name = "frmMemoryViewer";
			this.Text = "Memory Tools";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgMemoryViewer.ResumeLayout(false);
			this.tpgMemoryViewer.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tpgAccessCounters.ResumeLayout(false);
			this.tpgProfiler.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.ctrlHexViewer ctrlHexViewer;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblViewMemoryType;
		private System.Windows.Forms.ComboBox cboMemoryType;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuFind;
		private System.Windows.Forms.ToolStripMenuItem mnuFindNext;
		private System.Windows.Forms.ToolStripMenuItem mnuFindPrev;
		private System.Windows.Forms.ToolStripMenuItem mnuGoTo;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
		private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuIncreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuDecreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuResetFontSize;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefresh;
		private System.Windows.Forms.ToolStripMenuItem mnuImport;
		private System.Windows.Forms.ToolStripMenuItem mnuExport;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnImport;
		private System.Windows.Forms.ToolStripButton btnExport;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgMemoryViewer;
		private System.Windows.Forms.TabPage tpgAccessCounters;
		private Controls.ctrlMemoryAccessCounters ctrlMemoryAccessCounters;
		private System.Windows.Forms.Timer tmrRefresh;
		private System.Windows.Forms.TabPage tpgProfiler;
		private Controls.ctrlProfiler ctrlProfiler;
		private System.Windows.Forms.ToolStripMenuItem mnuLoadTblFile;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCharacters;
		private System.Windows.Forms.ToolStripMenuItem mnuResetTblMappings;
		private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuHightlightReads;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightWrites;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightExecution;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem fadeSpeedToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuFadeSlow;
		private System.Windows.Forms.ToolStripMenuItem mnuFadeNormal;
		private System.Windows.Forms.ToolStripMenuItem mnuFadeFast;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mnuCustomFadeSpeed;
		private System.Windows.Forms.ToolStripMenuItem fadeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuHideUnusedBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuHideReadBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuHideWrittenBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuHideExecutedBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuFadeNever;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem dataTypeHighlightingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightCodeBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightDataBytes;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightChrDrawnBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightChrReadBytes;
		private System.Windows.Forms.ToolStripMenuItem mnuConfigureColors;
		private System.Windows.Forms.ToolStripMenuItem mnuShowLabelInfoOnMouseOver;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightLabelledBytes;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem mnuHighlightDmcDataBytes;
	}
}