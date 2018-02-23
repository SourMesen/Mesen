namespace Mesen.GUI.Debugger
{
	partial class frmScript
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
			this.mnuMain = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNewScript = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentScripts = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowLogWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSaveBeforeRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoReload = new System.Windows.Forms.ToolStripMenuItem();
			this.onStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBlankWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTutorialScript = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoLoadLastScript = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new Mesen.GUI.Controls.ctrlMesenToolStrip();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRun = new System.Windows.Forms.ToolStripButton();
			this.btnStop = new System.Windows.Forms.ToolStripButton();
			this.txtScriptContent = new FastColoredTextBoxNS.FastColoredTextBox();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ctrlSplit = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.tmrLog = new System.Windows.Forms.Timer(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblScriptActive = new System.Windows.Forms.ToolStripStatusLabel();
			this.mnuMain.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtScriptContent)).BeginInit();
			this.contextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplit)).BeginInit();
			this.ctrlSplit.Panel1.SuspendLayout();
			this.ctrlSplit.Panel2.SuspendLayout();
			this.ctrlSplit.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuView,
            this.scriptToolStripMenuItem});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(965, 24);
			this.mnuMain.TabIndex = 0;
			this.mnuMain.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewScript,
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripMenuItem1,
            this.mnuRecentScripts,
            this.toolStripMenuItem2,
            this.mnuClose});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuNewScript
			// 
			this.mnuNewScript.Image = global::Mesen.GUI.Properties.Resources.Script;
			this.mnuNewScript.Name = "mnuNewScript";
			this.mnuNewScript.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuNewScript.Size = new System.Drawing.Size(221, 22);
			this.mnuNewScript.Text = "New Script Window";
			this.mnuNewScript.Click += new System.EventHandler(this.mnuNewScript_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = global::Mesen.GUI.Properties.Resources.FolderOpen;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpen.Size = new System.Drawing.Size(221, 22);
			this.mnuOpen.Text = "Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(221, 22);
			this.mnuSave.Text = "Save";
			this.mnuSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(221, 22);
			this.mnuSaveAs.Text = "Save as...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(218, 6);
			// 
			// mnuRecentScripts
			// 
			this.mnuRecentScripts.Name = "mnuRecentScripts";
			this.mnuRecentScripts.Size = new System.Drawing.Size(221, 22);
			this.mnuRecentScripts.Text = "Recent Scripts";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(218, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(221, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontSizeToolStripMenuItem,
            this.toolStripMenuItem4,
            this.mnuShowLogWindow});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(44, 20);
			this.mnuView.Text = "View";
			this.mnuView.DropDownOpening += new System.EventHandler(this.mnuView_DropDownOpening);
			// 
			// fontSizeToolStripMenuItem
			// 
			this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIncreaseFontSize,
            this.mnuDecreaseFontSize,
            this.mnuResetFontSize});
			this.fontSizeToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.Font;
			this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
			this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
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
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(170, 6);
			// 
			// mnuShowLogWindow
			// 
			this.mnuShowLogWindow.CheckOnClick = true;
			this.mnuShowLogWindow.Name = "mnuShowLogWindow";
			this.mnuShowLogWindow.Size = new System.Drawing.Size(173, 22);
			this.mnuShowLogWindow.Text = "Show Log Window";
			this.mnuShowLogWindow.Click += new System.EventHandler(this.mnuShowLogWindow_Click);
			// 
			// scriptToolStripMenuItem
			// 
			this.scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRun,
            this.mnuStop,
            this.toolStripMenuItem3,
            this.mnuSaveBeforeRun,
            this.mnuAutoReload,
            this.onStartupToolStripMenuItem});
			this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
			this.scriptToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.scriptToolStripMenuItem.Text = "Script";
			// 
			// mnuRun
			// 
			this.mnuRun.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.mnuRun.Name = "mnuRun";
			this.mnuRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRun.Size = new System.Drawing.Size(258, 22);
			this.mnuRun.Text = "Run";
			this.mnuRun.Click += new System.EventHandler(this.mnuRun_Click);
			// 
			// mnuStop
			// 
			this.mnuStop.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuStop.Name = "mnuStop";
			this.mnuStop.ShortcutKeyDisplayString = "Esc";
			this.mnuStop.Size = new System.Drawing.Size(258, 22);
			this.mnuStop.Text = "Stop";
			this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuSaveBeforeRun
			// 
			this.mnuSaveBeforeRun.CheckOnClick = true;
			this.mnuSaveBeforeRun.Name = "mnuSaveBeforeRun";
			this.mnuSaveBeforeRun.Size = new System.Drawing.Size(258, 22);
			this.mnuSaveBeforeRun.Text = "Auto-save before running";
			// 
			// mnuAutoReload
			// 
			this.mnuAutoReload.CheckOnClick = true;
			this.mnuAutoReload.Name = "mnuAutoReload";
			this.mnuAutoReload.Size = new System.Drawing.Size(258, 22);
			this.mnuAutoReload.Text = "Auto-reload when file changes";
			// 
			// onStartupToolStripMenuItem
			// 
			this.onStartupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBlankWindow,
            this.mnuTutorialScript,
            this.mnuAutoLoadLastScript});
			this.onStartupToolStripMenuItem.Name = "onStartupToolStripMenuItem";
			this.onStartupToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
			this.onStartupToolStripMenuItem.Text = "When opening the script window...";
			// 
			// mnuBlankWindow
			// 
			this.mnuBlankWindow.Name = "mnuBlankWindow";
			this.mnuBlankWindow.Size = new System.Drawing.Size(227, 22);
			this.mnuBlankWindow.Text = "Display a blank code window";
			this.mnuBlankWindow.Click += new System.EventHandler(this.mnuBlankWindow_Click);
			// 
			// mnuTutorialScript
			// 
			this.mnuTutorialScript.Name = "mnuTutorialScript";
			this.mnuTutorialScript.Size = new System.Drawing.Size(227, 22);
			this.mnuTutorialScript.Text = "Display the tutorial script";
			this.mnuTutorialScript.Click += new System.EventHandler(this.mnuTutorialScript_Click);
			// 
			// mnuAutoLoadLastScript
			// 
			this.mnuAutoLoadLastScript.Name = "mnuAutoLoadLastScript";
			this.mnuAutoLoadLastScript.Size = new System.Drawing.Size(227, 22);
			this.mnuAutoLoadLastScript.Text = "Load the last script loaded";
			this.mnuAutoLoadLastScript.Click += new System.EventHandler(this.mnuAutoLoadLastScript_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnRun,
            this.btnStop});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStrip1.Size = new System.Drawing.Size(965, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = global::Mesen.GUI.Properties.Resources.FolderOpen;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "Open (Ctrl-O)";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "Save (Ctrl-S)";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnRun
			// 
			this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRun.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(23, 22);
			this.btnRun.Text = "Run (F5)";
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// btnStop
			// 
			this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnStop.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(23, 22);
			this.btnStop.Text = "Stop";
			this.btnStop.ToolTipText = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// txtScriptContent
			// 
			this.txtScriptContent.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.txtScriptContent.AutoIndentChars = false;
			this.txtScriptContent.AutoIndentCharsPatterns = "\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>.+)\n";
			this.txtScriptContent.AutoIndentExistingLines = false;
			this.txtScriptContent.AutoScrollMinSize = new System.Drawing.Size(43, 14);
			this.txtScriptContent.BackBrush = null;
			this.txtScriptContent.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
			this.txtScriptContent.CharHeight = 14;
			this.txtScriptContent.CharWidth = 8;
			this.txtScriptContent.CommentPrefix = "--";
			this.txtScriptContent.ContextMenuStrip = this.contextMenu;
			this.txtScriptContent.CurrentLineColor = System.Drawing.Color.Gainsboro;
			this.txtScriptContent.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtScriptContent.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.txtScriptContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtScriptContent.IsReplaceMode = false;
			this.txtScriptContent.Language = FastColoredTextBoxNS.Language.Lua;
			this.txtScriptContent.LeftBracket = '(';
			this.txtScriptContent.LeftBracket2 = '{';
			this.txtScriptContent.Location = new System.Drawing.Point(0, 0);
			this.txtScriptContent.Name = "txtScriptContent";
			this.txtScriptContent.Paddings = new System.Windows.Forms.Padding(0);
			this.txtScriptContent.ReservedCountOfLineNumberChars = 3;
			this.txtScriptContent.RightBracket = ')';
			this.txtScriptContent.RightBracket2 = '}';
			this.txtScriptContent.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.txtScriptContent.Size = new System.Drawing.Size(965, 421);
			this.txtScriptContent.TabIndex = 3;
			this.txtScriptContent.TabLength = 2;
			this.txtScriptContent.Zoom = 100;
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopy,
            this.mnuCut,
            this.mnuPaste,
            this.toolStripMenuItem5,
            this.mnuSelectAll});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(165, 98);
			// 
			// mnuCopy
			// 
			this.mnuCopy.Name = "mnuCopy";
			this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.mnuCopy.Size = new System.Drawing.Size(164, 22);
			this.mnuCopy.Text = "Copy";
			this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
			// 
			// mnuCut
			// 
			this.mnuCut.Name = "mnuCut";
			this.mnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.mnuCut.Size = new System.Drawing.Size(164, 22);
			this.mnuCut.Text = "Cut";
			this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
			// 
			// mnuPaste
			// 
			this.mnuPaste.Name = "mnuPaste";
			this.mnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.mnuPaste.Size = new System.Drawing.Size(164, 22);
			this.mnuPaste.Text = "Paste";
			this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(161, 6);
			// 
			// mnuSelectAll
			// 
			this.mnuSelectAll.Name = "mnuSelectAll";
			this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.mnuSelectAll.Size = new System.Drawing.Size(164, 22);
			this.mnuSelectAll.Text = "Select All";
			this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
			// 
			// ctrlSplit
			// 
			this.ctrlSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.ctrlSplit.Location = new System.Drawing.Point(0, 49);
			this.ctrlSplit.Name = "ctrlSplit";
			this.ctrlSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// ctrlSplit.Panel1
			// 
			this.ctrlSplit.Panel1.Controls.Add(this.txtScriptContent);
			this.ctrlSplit.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.ctrlSplit.Panel1MinSize = 200;
			// 
			// ctrlSplit.Panel2
			// 
			this.ctrlSplit.Panel2.Controls.Add(this.txtLog);
			this.ctrlSplit.Panel2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.ctrlSplit.Size = new System.Drawing.Size(965, 516);
			this.ctrlSplit.SplitterDistance = 423;
			this.ctrlSplit.TabIndex = 4;
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 2);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(965, 87);
			this.txtLog.TabIndex = 0;
			// 
			// tmrLog
			// 
			this.tmrLog.Enabled = true;
			this.tmrLog.Interval = 200;
			this.tmrLog.Tick += new System.EventHandler(this.tmrLog_Tick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblScriptActive});
			this.statusStrip1.Location = new System.Drawing.Point(0, 565);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(965, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(950, 17);
			this.toolStripStatusLabel1.Spring = true;
			// 
			// lblScriptActive
			// 
			this.lblScriptActive.ForeColor = System.Drawing.Color.Green;
			this.lblScriptActive.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.lblScriptActive.Name = "lblScriptActive";
			this.lblScriptActive.Size = new System.Drawing.Size(109, 17);
			this.lblScriptActive.Text = "Script is running";
			this.lblScriptActive.Visible = false;
			// 
			// frmScript
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(965, 587);
			this.Controls.Add(this.ctrlSplit);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.mnuMain);
			this.Controls.Add(this.statusStrip1);
			this.MainMenuStrip = this.mnuMain;
			this.MinimumSize = new System.Drawing.Size(441, 444);
			this.Name = "frmScript";
			this.Text = "Script Window";
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtScriptContent)).EndInit();
			this.contextMenu.ResumeLayout(false);
			this.ctrlSplit.Panel1.ResumeLayout(false);
			this.ctrlSplit.Panel2.ResumeLayout(false);
			this.ctrlSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplit)).EndInit();
			this.ctrlSplit.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Mesen.GUI.Controls.ctrlMesenMenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private Mesen.GUI.Controls.ctrlMesenToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnRun;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuRun;
		private FastColoredTextBoxNS.FastColoredTextBox txtScriptContent;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentScripts;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuStop;
		private System.Windows.Forms.ToolStripButton btnStop;
		private GUI.Controls.ctrlSplitContainer ctrlSplit;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.Timer tmrLog;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel lblScriptActive;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveBeforeRun;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoReload;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuShowLogWindow;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopy;
		private System.Windows.Forms.ToolStripMenuItem mnuCut;
		private System.Windows.Forms.ToolStripMenuItem mnuPaste;
		private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuIncreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuDecreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuResetFontSize;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuNewScript;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
		private System.Windows.Forms.ToolStripMenuItem onStartupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuBlankWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuTutorialScript;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoLoadLastScript;
	}
}