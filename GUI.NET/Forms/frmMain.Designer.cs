using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	partial class frmMain
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
			if(_debugger != null) {
				_debugger.Close();
			}

			ConfigManager.Config.VideoInfo.VideoScale = _regularScale;
			ConfigManager.ApplyChanges();

			StopEmu();
			InteropEmu.Release();
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
			this.menuTimer = new System.Windows.Forms.Timer(this.components);
			this.panelRenderer = new System.Windows.Forms.Panel();
			this.ctrlRenderer = new Mesen.GUI.Controls.ctrlRenderer();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSaveState = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLoadState = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGame = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPause = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
			this.sepFdsDisk = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSwitchDiskSide = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSelectDisk = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEjectDisk = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmulationSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmuSpeedNormal = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuIncreaseSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmuSpeedMaximumSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEmuSpeedTriple = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmuSpeedDouble = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmuSpeedHalf = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmuSpeedQuarter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowFPS = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoScale = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale1x = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale2x = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale3x = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale4x = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScaleCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNoneFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNtscFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAudioConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInput = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegion = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionAuto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionNtsc = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionPal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionDendy = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlay = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStartServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlaySelectController = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlayPlayer1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlayPlayer2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlayPlayer3 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlayPlayer4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNetPlaySpectator = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProfile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMovies = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPlayMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFrom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFromStart = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFromNow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStopMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuTests = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRecordFrom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRecordStart = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRecordNow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRecordMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestRecordTest = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTestStopRecording = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunAllTests = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebugger = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuTakeScreenshot = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.panelRenderer.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuTimer
			// 
			this.menuTimer.Tick += new System.EventHandler(this.menuTimer_Tick);
			// 
			// panelRenderer
			// 
			this.panelRenderer.BackColor = System.Drawing.Color.Black;
			this.panelRenderer.Controls.Add(this.ctrlRenderer);
			this.panelRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelRenderer.Location = new System.Drawing.Point(0, 24);
			this.panelRenderer.Name = "panelRenderer";
			this.panelRenderer.Size = new System.Drawing.Size(304, 218);
			this.panelRenderer.TabIndex = 2;
			this.panelRenderer.Click += new System.EventHandler(this.panelRenderer_Click);
			// 
			// ctrlRenderer
			// 
			this.ctrlRenderer.BackColor = System.Drawing.Color.Black;
			this.ctrlRenderer.Location = new System.Drawing.Point(0, 0);
			this.ctrlRenderer.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlRenderer.Name = "ctrlRenderer";
			this.ctrlRenderer.Size = new System.Drawing.Size(263, 176);
			this.ctrlRenderer.TabIndex = 1;
			this.ctrlRenderer.Enter += new System.EventHandler(this.ctrlRenderer_Enter);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuGame,
            this.mnuOptions,
            this.mnuTools,
            this.mnuHelp});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(304, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			this.menuStrip.MenuActivate += new System.EventHandler(this.menuStrip_MenuActivate);
			this.menuStrip.MenuDeactivate += new System.EventHandler(this.menuStrip_MenuDeactivate);
			this.menuStrip.VisibleChanged += new System.EventHandler(this.menuStrip_VisibleChanged);
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.toolStripMenuItem4,
            this.mnuSaveState,
            this.mnuLoadState,
            this.toolStripMenuItem7,
            this.mnuRecentFiles,
            this.toolStripMenuItem6,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "File";
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = global::Mesen.GUI.Properties.Resources.FolderOpen;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpen.Size = new System.Drawing.Size(146, 22);
			this.mnuOpen.Text = "Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(143, 6);
			// 
			// mnuSaveState
			// 
			this.mnuSaveState.Name = "mnuSaveState";
			this.mnuSaveState.Size = new System.Drawing.Size(146, 22);
			this.mnuSaveState.Text = "Save State";
			this.mnuSaveState.DropDownOpening += new System.EventHandler(this.mnuSaveState_DropDownOpening);
			// 
			// mnuLoadState
			// 
			this.mnuLoadState.Name = "mnuLoadState";
			this.mnuLoadState.Size = new System.Drawing.Size(146, 22);
			this.mnuLoadState.Text = "Load State";
			this.mnuLoadState.DropDownOpening += new System.EventHandler(this.mnuLoadState_DropDownOpening);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(143, 6);
			// 
			// mnuRecentFiles
			// 
			this.mnuRecentFiles.Name = "mnuRecentFiles";
			this.mnuRecentFiles.Size = new System.Drawing.Size(146, 22);
			this.mnuRecentFiles.Text = "Recent Files";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(143, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(146, 22);
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuGame
			// 
			this.mnuGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPause,
            this.mnuReset,
            this.mnuStop,
            this.sepFdsDisk,
            this.mnuSwitchDiskSide,
            this.mnuSelectDisk,
            this.mnuEjectDisk});
			this.mnuGame.Name = "mnuGame";
			this.mnuGame.Size = new System.Drawing.Size(50, 20);
			this.mnuGame.Text = "Game";
			// 
			// mnuPause
			// 
			this.mnuPause.Enabled = false;
			this.mnuPause.Image = global::Mesen.GUI.Properties.Resources.Pause;
			this.mnuPause.Name = "mnuPause";
			this.mnuPause.ShortcutKeyDisplayString = "Esc";
			this.mnuPause.Size = new System.Drawing.Size(200, 22);
			this.mnuPause.Text = "Pause";
			this.mnuPause.Click += new System.EventHandler(this.mnuPause_Click);
			// 
			// mnuReset
			// 
			this.mnuReset.Enabled = false;
			this.mnuReset.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuReset.Name = "mnuReset";
			this.mnuReset.Size = new System.Drawing.Size(200, 22);
			this.mnuReset.Text = "Reset";
			this.mnuReset.Click += new System.EventHandler(this.mnuReset_Click);
			// 
			// mnuStop
			// 
			this.mnuStop.Enabled = false;
			this.mnuStop.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuStop.Name = "mnuStop";
			this.mnuStop.Size = new System.Drawing.Size(200, 22);
			this.mnuStop.Text = "Stop";
			this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
			// 
			// sepFdsDisk
			// 
			this.sepFdsDisk.Name = "sepFdsDisk";
			this.sepFdsDisk.Size = new System.Drawing.Size(197, 6);
			// 
			// mnuSwitchDiskSide
			// 
			this.mnuSwitchDiskSide.Name = "mnuSwitchDiskSide";
			this.mnuSwitchDiskSide.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.mnuSwitchDiskSide.Size = new System.Drawing.Size(200, 22);
			this.mnuSwitchDiskSide.Text = "Switch Disk Side";
			this.mnuSwitchDiskSide.Click += new System.EventHandler(this.mnuSwitchDiskSide_Click);
			// 
			// mnuSelectDisk
			// 
			this.mnuSelectDisk.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.mnuSelectDisk.Name = "mnuSelectDisk";
			this.mnuSelectDisk.Size = new System.Drawing.Size(200, 22);
			this.mnuSelectDisk.Text = "Select Disk";
			// 
			// mnuEjectDisk
			// 
			this.mnuEjectDisk.Image = global::Mesen.GUI.Properties.Resources.Eject;
			this.mnuEjectDisk.Name = "mnuEjectDisk";
			this.mnuEjectDisk.Size = new System.Drawing.Size(200, 22);
			this.mnuEjectDisk.Text = "Eject Disk";
			this.mnuEjectDisk.Click += new System.EventHandler(this.mnuEjectDisk_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEmulationSpeed,
            this.mnuVideoScale,
            this.mnuVideoFilter,
            this.toolStripMenuItem10,
            this.mnuAudioConfig,
            this.mnuInput,
            this.mnuRegion,
            this.mnuVideoConfig,
            this.toolStripMenuItem11,
            this.mnuPreferences});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(61, 20);
			this.mnuOptions.Text = "Options";
			// 
			// mnuEmulationSpeed
			// 
			this.mnuEmulationSpeed.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEmuSpeedNormal,
            this.toolStripMenuItem8,
            this.mnuIncreaseSpeed,
            this.mnuDecreaseSpeed,
            this.mnuEmuSpeedMaximumSpeed,
            this.toolStripMenuItem9,
            this.mnuEmuSpeedTriple,
            this.mnuEmuSpeedDouble,
            this.mnuEmuSpeedHalf,
            this.mnuEmuSpeedQuarter,
            this.toolStripMenuItem14,
            this.mnuShowFPS});
			this.mnuEmulationSpeed.Image = global::Mesen.GUI.Properties.Resources.Speed;
			this.mnuEmulationSpeed.Name = "mnuEmulationSpeed";
			this.mnuEmulationSpeed.Size = new System.Drawing.Size(135, 22);
			this.mnuEmulationSpeed.Text = "Speed";
			// 
			// mnuEmuSpeedNormal
			// 
			this.mnuEmuSpeedNormal.Name = "mnuEmuSpeedNormal";
			this.mnuEmuSpeedNormal.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedNormal.Text = "Normal (100%)";
			this.mnuEmuSpeedNormal.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuIncreaseSpeed
			// 
			this.mnuIncreaseSpeed.Name = "mnuIncreaseSpeed";
			this.mnuIncreaseSpeed.ShortcutKeyDisplayString = "=";
			this.mnuIncreaseSpeed.Size = new System.Drawing.Size(182, 22);
			this.mnuIncreaseSpeed.Text = "Increase Speed";
			this.mnuIncreaseSpeed.Click += new System.EventHandler(this.mnuIncreaseSpeed_Click);
			// 
			// mnuDecreaseSpeed
			// 
			this.mnuDecreaseSpeed.Name = "mnuDecreaseSpeed";
			this.mnuDecreaseSpeed.ShortcutKeyDisplayString = "-";
			this.mnuDecreaseSpeed.Size = new System.Drawing.Size(182, 22);
			this.mnuDecreaseSpeed.Text = "Decrease Speed";
			this.mnuDecreaseSpeed.Click += new System.EventHandler(this.mnuDecreaseSpeed_Click);
			// 
			// mnuEmuSpeedMaximumSpeed
			// 
			this.mnuEmuSpeedMaximumSpeed.Name = "mnuEmuSpeedMaximumSpeed";
			this.mnuEmuSpeedMaximumSpeed.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mnuEmuSpeedMaximumSpeed.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedMaximumSpeed.Text = "Maximum Speed";
			this.mnuEmuSpeedMaximumSpeed.Click += new System.EventHandler(this.mnuEmuSpeedMaximumSpeed_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuEmuSpeedTriple
			// 
			this.mnuEmuSpeedTriple.Name = "mnuEmuSpeedTriple";
			this.mnuEmuSpeedTriple.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedTriple.Tag = "";
			this.mnuEmuSpeedTriple.Text = "Triple (300%)";
			this.mnuEmuSpeedTriple.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedDouble
			// 
			this.mnuEmuSpeedDouble.Name = "mnuEmuSpeedDouble";
			this.mnuEmuSpeedDouble.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedDouble.Text = "Double (200%)";
			this.mnuEmuSpeedDouble.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedHalf
			// 
			this.mnuEmuSpeedHalf.Name = "mnuEmuSpeedHalf";
			this.mnuEmuSpeedHalf.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedHalf.Text = "Half (50%)";
			this.mnuEmuSpeedHalf.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedQuarter
			// 
			this.mnuEmuSpeedQuarter.Name = "mnuEmuSpeedQuarter";
			this.mnuEmuSpeedQuarter.Size = new System.Drawing.Size(182, 22);
			this.mnuEmuSpeedQuarter.Text = "Quarter (25%)";
			this.mnuEmuSpeedQuarter.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuShowFPS
			// 
			this.mnuShowFPS.CheckOnClick = true;
			this.mnuShowFPS.Name = "mnuShowFPS";
			this.mnuShowFPS.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.mnuShowFPS.Size = new System.Drawing.Size(182, 22);
			this.mnuShowFPS.Text = "Show FPS";
			this.mnuShowFPS.Click += new System.EventHandler(this.mnuShowFPS_Click);
			// 
			// mnuVideoScale
			// 
			this.mnuVideoScale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuScale1x,
            this.mnuScale2x,
            this.mnuScale3x,
            this.mnuScale4x,
            this.mnuScaleCustom,
            this.toolStripMenuItem13,
            this.mnuFullscreen});
			this.mnuVideoScale.Image = global::Mesen.GUI.Properties.Resources.Fullscreen;
			this.mnuVideoScale.Name = "mnuVideoScale";
			this.mnuVideoScale.Size = new System.Drawing.Size(135, 22);
			this.mnuVideoScale.Text = "Video Size";
			// 
			// mnuScale1x
			// 
			this.mnuScale1x.Name = "mnuScale1x";
			this.mnuScale1x.Size = new System.Drawing.Size(152, 22);
			this.mnuScale1x.Tag = "1";
			this.mnuScale1x.Text = "1x";
			this.mnuScale1x.Click += new System.EventHandler(this.mnuScale_Click);
			// 
			// mnuScale2x
			// 
			this.mnuScale2x.Name = "mnuScale2x";
			this.mnuScale2x.Size = new System.Drawing.Size(152, 22);
			this.mnuScale2x.Tag = "2";
			this.mnuScale2x.Text = "2x";
			this.mnuScale2x.Click += new System.EventHandler(this.mnuScale_Click);
			// 
			// mnuScale3x
			// 
			this.mnuScale3x.Name = "mnuScale3x";
			this.mnuScale3x.Size = new System.Drawing.Size(152, 22);
			this.mnuScale3x.Tag = "3";
			this.mnuScale3x.Text = "3x";
			this.mnuScale3x.Click += new System.EventHandler(this.mnuScale_Click);
			// 
			// mnuScale4x
			// 
			this.mnuScale4x.Name = "mnuScale4x";
			this.mnuScale4x.Size = new System.Drawing.Size(152, 22);
			this.mnuScale4x.Tag = "4";
			this.mnuScale4x.Text = "4x";
			this.mnuScale4x.Click += new System.EventHandler(this.mnuScale_Click);
			// 
			// mnuScaleCustom
			// 
			this.mnuScaleCustom.Name = "mnuScaleCustom";
			this.mnuScaleCustom.Size = new System.Drawing.Size(152, 22);
			this.mnuScaleCustom.Text = "Custom";
			this.mnuScaleCustom.Click += new System.EventHandler(this.mnuScaleCustom_Click);
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(149, 6);
			// 
			// mnuFullscreen
			// 
			this.mnuFullscreen.Name = "mnuFullscreen";
			this.mnuFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mnuFullscreen.Size = new System.Drawing.Size(152, 22);
			this.mnuFullscreen.Text = "Fullscreen";
			this.mnuFullscreen.Click += new System.EventHandler(this.mnuFullscreen_Click);
			// 
			// mnuVideoFilter
			// 
			this.mnuVideoFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNoneFilter,
            this.mnuNtscFilter});
			this.mnuVideoFilter.Name = "mnuVideoFilter";
			this.mnuVideoFilter.Size = new System.Drawing.Size(135, 22);
			this.mnuVideoFilter.Text = "Video Filter";
			// 
			// mnuNoneFilter
			// 
			this.mnuNoneFilter.Name = "mnuNoneFilter";
			this.mnuNoneFilter.Size = new System.Drawing.Size(104, 22);
			this.mnuNoneFilter.Text = "None";
			this.mnuNoneFilter.Click += new System.EventHandler(this.mnuNoneFilter_Click);
			// 
			// mnuNtscFilter
			// 
			this.mnuNtscFilter.Name = "mnuNtscFilter";
			this.mnuNtscFilter.Size = new System.Drawing.Size(104, 22);
			this.mnuNtscFilter.Text = "NTSC";
			this.mnuNtscFilter.Click += new System.EventHandler(this.mnuNtscFilter_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(132, 6);
			// 
			// mnuAudioConfig
			// 
			this.mnuAudioConfig.Image = global::Mesen.GUI.Properties.Resources.Audio;
			this.mnuAudioConfig.Name = "mnuAudioConfig";
			this.mnuAudioConfig.Size = new System.Drawing.Size(135, 22);
			this.mnuAudioConfig.Text = "Audio";
			this.mnuAudioConfig.Click += new System.EventHandler(this.mnuAudioConfig_Click);
			// 
			// mnuInput
			// 
			this.mnuInput.Image = global::Mesen.GUI.Properties.Resources.Controller;
			this.mnuInput.Name = "mnuInput";
			this.mnuInput.Size = new System.Drawing.Size(135, 22);
			this.mnuInput.Text = "Input";
			this.mnuInput.Click += new System.EventHandler(this.mnuInput_Click);
			// 
			// mnuRegion
			// 
			this.mnuRegion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRegionAuto,
            this.mnuRegionNtsc,
            this.mnuRegionPal,
            this.mnuRegionDendy});
			this.mnuRegion.Image = global::Mesen.GUI.Properties.Resources.Globe;
			this.mnuRegion.Name = "mnuRegion";
			this.mnuRegion.Size = new System.Drawing.Size(135, 22);
			this.mnuRegion.Text = "Region";
			// 
			// mnuRegionAuto
			// 
			this.mnuRegionAuto.Name = "mnuRegionAuto";
			this.mnuRegionAuto.Size = new System.Drawing.Size(108, 22);
			this.mnuRegionAuto.Text = "Auto";
			this.mnuRegionAuto.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuRegionNtsc
			// 
			this.mnuRegionNtsc.Name = "mnuRegionNtsc";
			this.mnuRegionNtsc.Size = new System.Drawing.Size(108, 22);
			this.mnuRegionNtsc.Text = "NTSC";
			this.mnuRegionNtsc.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuRegionPal
			// 
			this.mnuRegionPal.Name = "mnuRegionPal";
			this.mnuRegionPal.Size = new System.Drawing.Size(108, 22);
			this.mnuRegionPal.Text = "PAL";
			this.mnuRegionPal.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuRegionDendy
			// 
			this.mnuRegionDendy.Name = "mnuRegionDendy";
			this.mnuRegionDendy.Size = new System.Drawing.Size(108, 22);
			this.mnuRegionDendy.Text = "Dendy";
			this.mnuRegionDendy.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuVideoConfig
			// 
			this.mnuVideoConfig.Image = global::Mesen.GUI.Properties.Resources.Video;
			this.mnuVideoConfig.Name = "mnuVideoConfig";
			this.mnuVideoConfig.Size = new System.Drawing.Size(135, 22);
			this.mnuVideoConfig.Text = "Video";
			this.mnuVideoConfig.Click += new System.EventHandler(this.mnuVideoConfig_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(132, 6);
			// 
			// mnuPreferences
			// 
			this.mnuPreferences.Image = global::Mesen.GUI.Properties.Resources.Cog;
			this.mnuPreferences.Name = "mnuPreferences";
			this.mnuPreferences.Size = new System.Drawing.Size(135, 22);
			this.mnuPreferences.Text = "Preferences";
			this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNetPlay,
            this.mnuMovies,
            this.mnuCheats,
            this.toolStripMenuItem12,
            this.mnuTests,
            this.mnuDebugger,
            this.toolStripMenuItem1,
            this.mnuTakeScreenshot});
			this.mnuTools.Name = "mnuTools";
			this.mnuTools.Size = new System.Drawing.Size(48, 20);
			this.mnuTools.Text = "Tools";
			// 
			// mnuNetPlay
			// 
			this.mnuNetPlay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStartServer,
            this.mnuConnect,
            this.mnuNetPlaySelectController,
            this.toolStripMenuItem2,
            this.mnuFindServer,
            this.mnuProfile});
			this.mnuNetPlay.Image = global::Mesen.GUI.Properties.Resources.NetPlay;
			this.mnuNetPlay.Name = "mnuNetPlay";
			this.mnuNetPlay.Size = new System.Drawing.Size(185, 22);
			this.mnuNetPlay.Text = "Net Play";
			// 
			// mnuStartServer
			// 
			this.mnuStartServer.Name = "mnuStartServer";
			this.mnuStartServer.Size = new System.Drawing.Size(177, 22);
			this.mnuStartServer.Text = "Start Server";
			this.mnuStartServer.Click += new System.EventHandler(this.mnuStartServer_Click);
			// 
			// mnuConnect
			// 
			this.mnuConnect.Name = "mnuConnect";
			this.mnuConnect.Size = new System.Drawing.Size(177, 22);
			this.mnuConnect.Text = "Connect to Server";
			this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
			// 
			// mnuNetPlaySelectController
			// 
			this.mnuNetPlaySelectController.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNetPlayPlayer1,
            this.mnuNetPlayPlayer2,
            this.mnuNetPlayPlayer3,
            this.mnuNetPlayPlayer4,
            this.toolStripMenuItem3,
            this.mnuNetPlaySpectator});
			this.mnuNetPlaySelectController.Name = "mnuNetPlaySelectController";
			this.mnuNetPlaySelectController.Size = new System.Drawing.Size(177, 22);
			this.mnuNetPlaySelectController.Text = "Select Controller";
			// 
			// mnuNetPlayPlayer1
			// 
			this.mnuNetPlayPlayer1.Name = "mnuNetPlayPlayer1";
			this.mnuNetPlayPlayer1.Size = new System.Drawing.Size(124, 22);
			this.mnuNetPlayPlayer1.Text = "Player 1";
			this.mnuNetPlayPlayer1.Click += new System.EventHandler(this.mnuNetPlayPlayer1_Click);
			// 
			// mnuNetPlayPlayer2
			// 
			this.mnuNetPlayPlayer2.Name = "mnuNetPlayPlayer2";
			this.mnuNetPlayPlayer2.Size = new System.Drawing.Size(124, 22);
			this.mnuNetPlayPlayer2.Text = "Player 2";
			this.mnuNetPlayPlayer2.Click += new System.EventHandler(this.mnuNetPlayPlayer2_Click);
			// 
			// mnuNetPlayPlayer3
			// 
			this.mnuNetPlayPlayer3.Name = "mnuNetPlayPlayer3";
			this.mnuNetPlayPlayer3.Size = new System.Drawing.Size(124, 22);
			this.mnuNetPlayPlayer3.Text = "Player 3";
			this.mnuNetPlayPlayer3.Click += new System.EventHandler(this.mnuNetPlayPlayer3_Click);
			// 
			// mnuNetPlayPlayer4
			// 
			this.mnuNetPlayPlayer4.Name = "mnuNetPlayPlayer4";
			this.mnuNetPlayPlayer4.Size = new System.Drawing.Size(124, 22);
			this.mnuNetPlayPlayer4.Text = "Player 4";
			this.mnuNetPlayPlayer4.Click += new System.EventHandler(this.mnuNetPlayPlayer4_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(121, 6);
			// 
			// mnuNetPlaySpectator
			// 
			this.mnuNetPlaySpectator.Name = "mnuNetPlaySpectator";
			this.mnuNetPlaySpectator.Size = new System.Drawing.Size(124, 22);
			this.mnuNetPlaySpectator.Text = "Spectator";
			this.mnuNetPlaySpectator.Click += new System.EventHandler(this.mnuNetPlaySpectator_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuFindServer
			// 
			this.mnuFindServer.Enabled = false;
			this.mnuFindServer.Name = "mnuFindServer";
			this.mnuFindServer.Size = new System.Drawing.Size(177, 22);
			this.mnuFindServer.Text = "Find Public Server...";
			this.mnuFindServer.Visible = false;
			// 
			// mnuProfile
			// 
			this.mnuProfile.Name = "mnuProfile";
			this.mnuProfile.Size = new System.Drawing.Size(177, 22);
			this.mnuProfile.Text = "Configure Profile";
			this.mnuProfile.Click += new System.EventHandler(this.mnuProfile_Click);
			// 
			// mnuMovies
			// 
			this.mnuMovies.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlayMovie,
            this.mnuRecordFrom,
            this.mnuStopMovie});
			this.mnuMovies.Image = global::Mesen.GUI.Properties.Resources.Movie;
			this.mnuMovies.Name = "mnuMovies";
			this.mnuMovies.Size = new System.Drawing.Size(185, 22);
			this.mnuMovies.Text = "Movies";
			// 
			// mnuPlayMovie
			// 
			this.mnuPlayMovie.Name = "mnuPlayMovie";
			this.mnuPlayMovie.Size = new System.Drawing.Size(149, 22);
			this.mnuPlayMovie.Text = "Play...";
			this.mnuPlayMovie.Click += new System.EventHandler(this.mnuPlayMovie_Click);
			// 
			// mnuRecordFrom
			// 
			this.mnuRecordFrom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRecordFromStart,
            this.mnuRecordFromNow});
			this.mnuRecordFrom.Name = "mnuRecordFrom";
			this.mnuRecordFrom.Size = new System.Drawing.Size(149, 22);
			this.mnuRecordFrom.Text = "Record from...";
			// 
			// mnuRecordFromStart
			// 
			this.mnuRecordFromStart.Name = "mnuRecordFromStart";
			this.mnuRecordFromStart.Size = new System.Drawing.Size(99, 22);
			this.mnuRecordFromStart.Text = "Start";
			this.mnuRecordFromStart.Click += new System.EventHandler(this.mnuRecordFromStart_Click);
			// 
			// mnuRecordFromNow
			// 
			this.mnuRecordFromNow.Name = "mnuRecordFromNow";
			this.mnuRecordFromNow.Size = new System.Drawing.Size(99, 22);
			this.mnuRecordFromNow.Text = "Now";
			this.mnuRecordFromNow.Click += new System.EventHandler(this.mnuRecordFromNow_Click);
			// 
			// mnuStopMovie
			// 
			this.mnuStopMovie.Name = "mnuStopMovie";
			this.mnuStopMovie.Size = new System.Drawing.Size(149, 22);
			this.mnuStopMovie.Text = "Stop";
			this.mnuStopMovie.Click += new System.EventHandler(this.mnuStopMovie_Click);
			// 
			// mnuCheats
			// 
			this.mnuCheats.Name = "mnuCheats";
			this.mnuCheats.Size = new System.Drawing.Size(185, 22);
			this.mnuCheats.Text = "Cheats";
			this.mnuCheats.Click += new System.EventHandler(this.mnuCheats_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(182, 6);
			// 
			// mnuTests
			// 
			this.mnuTests.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTestRun,
            this.mnuTestRecordFrom,
            this.mnuTestStopRecording,
            this.mnuRunAllTests});
			this.mnuTests.Name = "mnuTests";
			this.mnuTests.Size = new System.Drawing.Size(185, 22);
			this.mnuTests.Text = "Tests";
			// 
			// mnuTestRun
			// 
			this.mnuTestRun.Name = "mnuTestRun";
			this.mnuTestRun.Size = new System.Drawing.Size(193, 22);
			this.mnuTestRun.Text = "Run...";
			this.mnuTestRun.Click += new System.EventHandler(this.mnuTestRun_Click);
			// 
			// mnuTestRecordFrom
			// 
			this.mnuTestRecordFrom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTestRecordStart,
            this.mnuTestRecordNow,
            this.mnuTestRecordMovie,
            this.mnuTestRecordTest});
			this.mnuTestRecordFrom.Name = "mnuTestRecordFrom";
			this.mnuTestRecordFrom.Size = new System.Drawing.Size(193, 22);
			this.mnuTestRecordFrom.Text = "Record from...";
			// 
			// mnuTestRecordStart
			// 
			this.mnuTestRecordStart.Name = "mnuTestRecordStart";
			this.mnuTestRecordStart.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.mnuTestRecordStart.Size = new System.Drawing.Size(138, 22);
			this.mnuTestRecordStart.Text = "Start";
			this.mnuTestRecordStart.Click += new System.EventHandler(this.mnuTestRecordStart_Click);
			// 
			// mnuTestRecordNow
			// 
			this.mnuTestRecordNow.Name = "mnuTestRecordNow";
			this.mnuTestRecordNow.Size = new System.Drawing.Size(138, 22);
			this.mnuTestRecordNow.Text = "Now";
			this.mnuTestRecordNow.Click += new System.EventHandler(this.mnuTestRecordNow_Click);
			// 
			// mnuTestRecordMovie
			// 
			this.mnuTestRecordMovie.Name = "mnuTestRecordMovie";
			this.mnuTestRecordMovie.Size = new System.Drawing.Size(138, 22);
			this.mnuTestRecordMovie.Text = "Movie";
			this.mnuTestRecordMovie.Click += new System.EventHandler(this.mnuTestRecordMovie_Click);
			// 
			// mnuTestRecordTest
			// 
			this.mnuTestRecordTest.Name = "mnuTestRecordTest";
			this.mnuTestRecordTest.Size = new System.Drawing.Size(138, 22);
			this.mnuTestRecordTest.Text = "Test";
			this.mnuTestRecordTest.Click += new System.EventHandler(this.mnuTestRecordTest_Click);
			// 
			// mnuTestStopRecording
			// 
			this.mnuTestStopRecording.Name = "mnuTestStopRecording";
			this.mnuTestStopRecording.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.mnuTestStopRecording.Size = new System.Drawing.Size(193, 22);
			this.mnuTestStopRecording.Text = "Stop recording";
			this.mnuTestStopRecording.Click += new System.EventHandler(this.mnuTestStopRecording_Click);
			// 
			// mnuRunAllTests
			// 
			this.mnuRunAllTests.Name = "mnuRunAllTests";
			this.mnuRunAllTests.Size = new System.Drawing.Size(193, 22);
			this.mnuRunAllTests.Text = "Run all tests";
			this.mnuRunAllTests.Click += new System.EventHandler(this.mnuRunAllTests_Click);
			// 
			// mnuDebugger
			// 
			this.mnuDebugger.Name = "mnuDebugger";
			this.mnuDebugger.Size = new System.Drawing.Size(185, 22);
			this.mnuDebugger.Text = "Debugger";
			this.mnuDebugger.Click += new System.EventHandler(this.mnuDebugger_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
			// 
			// mnuTakeScreenshot
			// 
			this.mnuTakeScreenshot.Image = global::Mesen.GUI.Properties.Resources.Camera;
			this.mnuTakeScreenshot.Name = "mnuTakeScreenshot";
			this.mnuTakeScreenshot.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.mnuTakeScreenshot.Size = new System.Drawing.Size(185, 22);
			this.mnuTakeScreenshot.Text = "Take Screenshot";
			this.mnuTakeScreenshot.Click += new System.EventHandler(this.mnuTakeScreenshot_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCheckForUpdates,
            this.toolStripMenuItem5,
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(44, 20);
			this.mnuHelp.Text = "Help";
			// 
			// mnuCheckForUpdates
			// 
			this.mnuCheckForUpdates.Image = global::Mesen.GUI.Properties.Resources.SoftwareUpdate;
			this.mnuCheckForUpdates.Name = "mnuCheckForUpdates";
			this.mnuCheckForUpdates.Size = new System.Drawing.Size(170, 22);
			this.mnuCheckForUpdates.Text = "Check for updates";
			this.mnuCheckForUpdates.Click += new System.EventHandler(this.mnuCheckForUpdates_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(167, 6);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(170, 22);
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// frmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Maroon;
			this.ClientSize = new System.Drawing.Size(304, 242);
			this.Controls.Add(this.panelRenderer);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(320, 280);
			this.Name = "frmMain";
			this.Text = "Mesen";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
			this.Resize += new System.EventHandler(this.frmMain_Resize);
			this.panelRenderer.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuGame;
		private Mesen.GUI.Controls.ctrlRenderer ctrlRenderer;
		private System.Windows.Forms.ToolStripMenuItem mnuPause;
		private System.Windows.Forms.ToolStripMenuItem mnuReset;
		private System.Windows.Forms.ToolStripMenuItem mnuStop;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuEmulationSpeed;
		private System.Windows.Forms.ToolStripMenuItem mnuInput;
		private System.Windows.Forms.ToolStripMenuItem mnuVideoConfig;
		private System.Windows.Forms.ToolStripMenuItem mnuAudioConfig;
		private System.Windows.Forms.ToolStripMenuItem mnuTools;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlay;
		private System.Windows.Forms.ToolStripMenuItem mnuStartServer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuConnect;
		private System.Windows.Forms.ToolStripMenuItem mnuProfile;
		private System.Windows.Forms.ToolStripMenuItem mnuMovies;
		private System.Windows.Forms.ToolStripMenuItem mnuPlayMovie;
		private System.Windows.Forms.ToolStripMenuItem mnuDebugger;
		private System.Windows.Forms.ToolStripMenuItem mnuTakeScreenshot;
		private System.Windows.Forms.ToolStripMenuItem mnuRecordFrom;
		private System.Windows.Forms.ToolStripMenuItem mnuRecordFromStart;
		private System.Windows.Forms.ToolStripMenuItem mnuRecordFromNow;
		private System.Windows.Forms.ToolStripMenuItem mnuStopMovie;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuFindServer;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuCheckForUpdates;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFiles;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveState;
		private System.Windows.Forms.ToolStripMenuItem mnuLoadState;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mnuCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuRegion;
		private System.Windows.Forms.ToolStripMenuItem mnuRegionAuto;
		private System.Windows.Forms.ToolStripMenuItem mnuRegionNtsc;
		private System.Windows.Forms.ToolStripMenuItem mnuRegionPal;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedTriple;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedNormal;
		private System.Windows.Forms.ToolStripMenuItem mnuIncreaseSpeed;
		private System.Windows.Forms.ToolStripMenuItem mnuDecreaseSpeed;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedDouble;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedHalf;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedQuarter;
		private System.Windows.Forms.ToolStripMenuItem mnuEmuSpeedMaximumSpeed;
		private System.Windows.Forms.Timer menuTimer;
		private System.Windows.Forms.ToolStripMenuItem mnuTests;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRun;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRecordFrom;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRecordStart;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRecordNow;
		private System.Windows.Forms.ToolStripMenuItem mnuTestStopRecording;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRecordMovie;
		private System.Windows.Forms.ToolStripMenuItem mnuTestRecordTest;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
		private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
		private System.Windows.Forms.ToolStripMenuItem mnuRunAllTests;
		private System.Windows.Forms.ToolStripMenuItem mnuVideoScale;
		private System.Windows.Forms.ToolStripMenuItem mnuScale1x;
		private System.Windows.Forms.ToolStripMenuItem mnuScale2x;
		private System.Windows.Forms.ToolStripMenuItem mnuScale3x;
		private System.Windows.Forms.ToolStripMenuItem mnuScale4x;
		private System.Windows.Forms.ToolStripMenuItem mnuVideoFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuNoneFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuNtscFilter;
		private System.Windows.Forms.ToolStripSeparator sepFdsDisk;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectDisk;
		private System.Windows.Forms.ToolStripMenuItem mnuEjectDisk;
		private System.Windows.Forms.ToolStripMenuItem mnuSwitchDiskSide;
		private System.Windows.Forms.ToolStripMenuItem mnuRegionDendy;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlaySelectController;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlayPlayer1;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlayPlayer2;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlayPlayer3;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlayPlayer4;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlaySpectator;
		private System.Windows.Forms.Panel panelRenderer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
		private System.Windows.Forms.ToolStripMenuItem mnuScaleCustom;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
		private System.Windows.Forms.ToolStripMenuItem mnuShowFPS;
		private System.Windows.Forms.ToolStripMenuItem mnuFullscreen;
	}
}

