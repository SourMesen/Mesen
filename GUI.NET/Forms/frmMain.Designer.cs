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
			this.ctrlLoading = new Mesen.GUI.Controls.ctrlLoadingRom();
			this.panelInfo = new System.Windows.Forms.Panel();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.ctrlRecentGames = new Mesen.GUI.Controls.ctrlRecentGames();
			this.ctrlNsfPlayer = new Mesen.GUI.Controls.ctrlNsfPlayer();
			this.ctrlRenderer = new Mesen.GUI.Controls.ctrlRenderer();
			this.menuStrip = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSaveState = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLoadState = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLoadLastSession = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGame = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPause = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPowerCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPowerOff = new System.Windows.Forms.ToolStripMenuItem();
			this.sepFdsDisk = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSwitchDiskSide = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSelectDisk = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEjectDisk = new System.Windows.Forms.ToolStripMenuItem();
			this.sepVsSystem = new System.Windows.Forms.ToolStripSeparator();
			this.mnuVsGameConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInsertCoin1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInsertCoin2 = new System.Windows.Forms.ToolStripMenuItem();
			this.sepBarcode = new System.Windows.Forms.ToolStripSeparator();
			this.mnuInputBarcode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTapeRecorder = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLoadTapeFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStartRecordTapeFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStopRecordTapeFile = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mnuScale5x = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale6x = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNoneFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNtscFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNtscBisqwitQuarterFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNtscBisqwitHalfFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNtscBisqwitFullFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuXBRZ2xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXBRZ3xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXBRZ4xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXBRZ5xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXBRZ6xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHQ2xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHQ3xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHQ4xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuScale2xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale3xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScale4xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.mnu2xSaiFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSuper2xSaiFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSuperEagleFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPrescale2xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrescale3xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrescale4xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrescale6xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrescale8xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrescale10xFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBilinearInterpolation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegion = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionAuto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionNtsc = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionPal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionDendy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAudioConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInput = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEmulationConfig = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mnuNetPlayPlayer5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNetPlaySpectator = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProfile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMovies = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPlayMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStopMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSoundRecorder = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWaveRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWaveStop = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoRecorder = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAviRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAviStop = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mnuRunAllGameTests = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunAutomaticTest = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebugger = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLogWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHdPackEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRandomGame = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTakeScreenshot = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebug = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuApuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAssembler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebugDebugger = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEventViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPpuViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuScriptWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTraceLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditHeader = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOnlineHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelpWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuReportBug = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuInstallHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.panelRenderer.SuspendLayout();
			this.panelInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
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
			this.panelRenderer.Controls.Add(this.ctrlLoading);
			this.panelRenderer.Controls.Add(this.panelInfo);
			this.panelRenderer.Controls.Add(this.ctrlRecentGames);
			this.panelRenderer.Controls.Add(this.ctrlNsfPlayer);
			this.panelRenderer.Controls.Add(this.ctrlRenderer);
			this.panelRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelRenderer.Location = new System.Drawing.Point(0, 24);
			this.panelRenderer.Name = "panelRenderer";
			this.panelRenderer.Size = new System.Drawing.Size(430, 309);
			this.panelRenderer.TabIndex = 2;
			this.panelRenderer.Click += new System.EventHandler(this.panelRenderer_Click);
			this.panelRenderer.DoubleClick += new System.EventHandler(this.ctrlRenderer_DoubleClick);
			this.panelRenderer.MouseLeave += new System.EventHandler(this.panelRenderer_MouseLeave);
			this.panelRenderer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseMove);
			// 
			// ctrlLoading
			// 
			this.ctrlLoading.BackColor = System.Drawing.Color.Black;
			this.ctrlLoading.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLoading.Location = new System.Drawing.Point(0, 0);
			this.ctrlLoading.Name = "ctrlLoading";
			this.ctrlLoading.Size = new System.Drawing.Size(430, 309);
			this.ctrlLoading.TabIndex = 4;
			this.ctrlLoading.Visible = false;
			// 
			// panelInfo
			// 
			this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.panelInfo.BackColor = System.Drawing.Color.Transparent;
			this.panelInfo.Controls.Add(this.picIcon);
			this.panelInfo.Controls.Add(this.lblVersion);
			this.panelInfo.Location = new System.Drawing.Point(359, 283);
			this.panelInfo.Name = "panelInfo";
			this.panelInfo.Size = new System.Drawing.Size(71, 26);
			this.panelInfo.TabIndex = 6;
			// 
			// picIcon
			// 
			this.picIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.picIcon.BackgroundImage = global::Mesen.GUI.Properties.Resources.MesenIconSmall;
			this.picIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picIcon.Location = new System.Drawing.Point(50, 5);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(16, 16);
			this.picIcon.TabIndex = 5;
			this.picIcon.TabStop = false;
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.AutoSize = true;
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.ForeColor = System.Drawing.Color.White;
			this.lblVersion.Location = new System.Drawing.Point(4, 7);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(0, 13);
			this.lblVersion.TabIndex = 6;
			// 
			// ctrlRecentGames
			// 
			this.ctrlRecentGames.BackColor = System.Drawing.Color.Transparent;
			this.ctrlRecentGames.Location = new System.Drawing.Point(0, 0);
			this.ctrlRecentGames.Name = "ctrlRecentGames";
			this.ctrlRecentGames.Size = new System.Drawing.Size(430, 309);
			this.ctrlRecentGames.TabIndex = 7;
			this.ctrlRecentGames.Visible = false;
			this.ctrlRecentGames.OnRecentGameLoaded += new Mesen.GUI.Controls.ctrlRecentGames.RecentGameLoadedHandler(this.ctrlRecentGames_OnRecentGameLoaded);
			this.ctrlRecentGames.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseMove);
			this.ctrlRecentGames.DoubleClick += new System.EventHandler(this.ctrlRenderer_DoubleClick);
			// 
			// ctrlNsfPlayer
			// 
			this.ctrlNsfPlayer.BackColor = System.Drawing.Color.Black;
			this.ctrlNsfPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNsfPlayer.Location = new System.Drawing.Point(0, 0);
			this.ctrlNsfPlayer.Name = "ctrlNsfPlayer";
			this.ctrlNsfPlayer.Size = new System.Drawing.Size(430, 309);
			this.ctrlNsfPlayer.TabIndex = 2;
			this.ctrlNsfPlayer.Visible = false;
			this.ctrlNsfPlayer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlNsfPlayer_MouseMove);
			// 
			// ctrlRenderer
			// 
			this.ctrlRenderer.BackColor = System.Drawing.Color.Black;
			this.ctrlRenderer.Location = new System.Drawing.Point(0, 0);
			this.ctrlRenderer.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlRenderer.Name = "ctrlRenderer";
			this.ctrlRenderer.Size = new System.Drawing.Size(150, 90);
			this.ctrlRenderer.TabIndex = 1;
			this.ctrlRenderer.DoubleClick += new System.EventHandler(this.ctrlRenderer_DoubleClick);
			this.ctrlRenderer.Enter += new System.EventHandler(this.ctrlRenderer_Enter);
			this.ctrlRenderer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseClick);
			this.ctrlRenderer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseMove);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuGame,
            this.mnuOptions,
            this.mnuTools,
            this.mnuDebug,
            this.mnuHelp});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(430, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.toolStripMenuItem4,
            this.mnuSaveState,
            this.mnuLoadState,
            this.mnuLoadLastSession,
            this.toolStripMenuItem7,
            this.mnuRecentFiles,
            this.toolStripMenuItem6,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.ShortcutKeyDisplayString = "";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "File";
			this.mnuFile.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
			this.mnuFile.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = global::Mesen.GUI.Properties.Resources.FolderOpen;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.Size = new System.Drawing.Size(166, 22);
			this.mnuOpen.Text = "Open";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuSaveState
			// 
			this.mnuSaveState.Name = "mnuSaveState";
			this.mnuSaveState.Size = new System.Drawing.Size(166, 22);
			this.mnuSaveState.Text = "Save State";
			this.mnuSaveState.DropDownOpening += new System.EventHandler(this.mnuSaveState_DropDownOpening);
			// 
			// mnuLoadState
			// 
			this.mnuLoadState.Name = "mnuLoadState";
			this.mnuLoadState.Size = new System.Drawing.Size(166, 22);
			this.mnuLoadState.Text = "Load State";
			this.mnuLoadState.DropDownOpening += new System.EventHandler(this.mnuLoadState_DropDownOpening);
			// 
			// mnuLoadLastSession
			// 
			this.mnuLoadLastSession.Name = "mnuLoadLastSession";
			this.mnuLoadLastSession.Size = new System.Drawing.Size(166, 22);
			this.mnuLoadLastSession.Text = "Load Last Session";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuRecentFiles
			// 
			this.mnuRecentFiles.Name = "mnuRecentFiles";
			this.mnuRecentFiles.Size = new System.Drawing.Size(166, 22);
			this.mnuRecentFiles.Text = "Recent Files";
			this.mnuRecentFiles.DropDownOpening += new System.EventHandler(this.mnuRecentFiles_DropDownOpening);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(166, 22);
			this.mnuExit.Text = "Exit";
			// 
			// mnuGame
			// 
			this.mnuGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPause,
            this.mnuReset,
            this.mnuPowerCycle,
            this.toolStripMenuItem24,
            this.mnuPowerOff,
            this.sepFdsDisk,
            this.mnuSwitchDiskSide,
            this.mnuSelectDisk,
            this.mnuEjectDisk,
            this.sepVsSystem,
            this.mnuVsGameConfig,
            this.mnuInsertCoin1,
            this.mnuInsertCoin2,
            this.sepBarcode,
            this.mnuInputBarcode,
            this.mnuTapeRecorder});
			this.mnuGame.Name = "mnuGame";
			this.mnuGame.Size = new System.Drawing.Size(50, 20);
			this.mnuGame.Text = "Game";
			this.mnuGame.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuGame.DropDownOpening += new System.EventHandler(this.mnuGame_DropDownOpening);
			this.mnuGame.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
			// 
			// mnuPause
			// 
			this.mnuPause.Enabled = false;
			this.mnuPause.Image = global::Mesen.GUI.Properties.Resources.Pause;
			this.mnuPause.Name = "mnuPause";
			this.mnuPause.Size = new System.Drawing.Size(182, 22);
			this.mnuPause.Text = "Pause";
			// 
			// mnuReset
			// 
			this.mnuReset.Enabled = false;
			this.mnuReset.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuReset.Name = "mnuReset";
			this.mnuReset.Size = new System.Drawing.Size(182, 22);
			this.mnuReset.Text = "Reset";
			// 
			// mnuPowerCycle
			// 
			this.mnuPowerCycle.Enabled = false;
			this.mnuPowerCycle.Image = global::Mesen.GUI.Properties.Resources.PowerCycle;
			this.mnuPowerCycle.Name = "mnuPowerCycle";
			this.mnuPowerCycle.Size = new System.Drawing.Size(182, 22);
			this.mnuPowerCycle.Text = "Power Cycle";
			// 
			// toolStripMenuItem24
			// 
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			this.toolStripMenuItem24.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuPowerOff
			// 
			this.mnuPowerOff.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuPowerOff.Name = "mnuPowerOff";
			this.mnuPowerOff.Size = new System.Drawing.Size(182, 22);
			this.mnuPowerOff.Text = "Power Off";
			// 
			// sepFdsDisk
			// 
			this.sepFdsDisk.Name = "sepFdsDisk";
			this.sepFdsDisk.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuSwitchDiskSide
			// 
			this.mnuSwitchDiskSide.Name = "mnuSwitchDiskSide";
			this.mnuSwitchDiskSide.Size = new System.Drawing.Size(182, 22);
			this.mnuSwitchDiskSide.Text = "Switch Disk Side";
			// 
			// mnuSelectDisk
			// 
			this.mnuSelectDisk.Image = global::Mesen.GUI.Properties.Resources.Floppy;
			this.mnuSelectDisk.Name = "mnuSelectDisk";
			this.mnuSelectDisk.Size = new System.Drawing.Size(182, 22);
			this.mnuSelectDisk.Text = "Select Disk";
			// 
			// mnuEjectDisk
			// 
			this.mnuEjectDisk.Image = global::Mesen.GUI.Properties.Resources.Eject;
			this.mnuEjectDisk.Name = "mnuEjectDisk";
			this.mnuEjectDisk.Size = new System.Drawing.Size(182, 22);
			this.mnuEjectDisk.Text = "Eject Disk";
			// 
			// sepVsSystem
			// 
			this.sepVsSystem.Name = "sepVsSystem";
			this.sepVsSystem.Size = new System.Drawing.Size(179, 6);
			this.sepVsSystem.Visible = false;
			// 
			// mnuVsGameConfig
			// 
			this.mnuVsGameConfig.Image = global::Mesen.GUI.Properties.Resources.DipSwitches;
			this.mnuVsGameConfig.Name = "mnuVsGameConfig";
			this.mnuVsGameConfig.Size = new System.Drawing.Size(182, 22);
			this.mnuVsGameConfig.Text = "Game Configuration";
			this.mnuVsGameConfig.Click += new System.EventHandler(this.mnuVsGameConfig_Click);
			// 
			// mnuInsertCoin1
			// 
			this.mnuInsertCoin1.Image = global::Mesen.GUI.Properties.Resources.coins;
			this.mnuInsertCoin1.Name = "mnuInsertCoin1";
			this.mnuInsertCoin1.Size = new System.Drawing.Size(182, 22);
			this.mnuInsertCoin1.Text = "Insert Coin (1)";
			this.mnuInsertCoin1.Visible = false;
			// 
			// mnuInsertCoin2
			// 
			this.mnuInsertCoin2.Image = global::Mesen.GUI.Properties.Resources.coins;
			this.mnuInsertCoin2.Name = "mnuInsertCoin2";
			this.mnuInsertCoin2.Size = new System.Drawing.Size(182, 22);
			this.mnuInsertCoin2.Text = "Insert Coin (2)";
			this.mnuInsertCoin2.Visible = false;
			// 
			// sepBarcode
			// 
			this.sepBarcode.Name = "sepBarcode";
			this.sepBarcode.Size = new System.Drawing.Size(179, 6);
			this.sepBarcode.Visible = false;
			// 
			// mnuInputBarcode
			// 
			this.mnuInputBarcode.Image = global::Mesen.GUI.Properties.Resources.Barcode;
			this.mnuInputBarcode.Name = "mnuInputBarcode";
			this.mnuInputBarcode.Size = new System.Drawing.Size(182, 22);
			this.mnuInputBarcode.Text = "Input Barcode...";
			this.mnuInputBarcode.Visible = false;
			// 
			// mnuTapeRecorder
			// 
			this.mnuTapeRecorder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLoadTapeFile,
            this.mnuStartRecordTapeFile,
            this.mnuStopRecordTapeFile});
			this.mnuTapeRecorder.Image = global::Mesen.GUI.Properties.Resources.Tape;
			this.mnuTapeRecorder.Name = "mnuTapeRecorder";
			this.mnuTapeRecorder.Size = new System.Drawing.Size(182, 22);
			this.mnuTapeRecorder.Text = "Tape Recorder";
			// 
			// mnuLoadTapeFile
			// 
			this.mnuLoadTapeFile.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuLoadTapeFile.Name = "mnuLoadTapeFile";
			this.mnuLoadTapeFile.Size = new System.Drawing.Size(157, 22);
			this.mnuLoadTapeFile.Text = "Load from file...";
			this.mnuLoadTapeFile.Click += new System.EventHandler(this.mnuLoadTapeFile_Click);
			// 
			// mnuStartRecordTapeFile
			// 
			this.mnuStartRecordTapeFile.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuStartRecordTapeFile.Name = "mnuStartRecordTapeFile";
			this.mnuStartRecordTapeFile.Size = new System.Drawing.Size(157, 22);
			this.mnuStartRecordTapeFile.Text = "Record to file...";
			this.mnuStartRecordTapeFile.Click += new System.EventHandler(this.mnuStartRecordTapeFile_Click);
			// 
			// mnuStopRecordTapeFile
			// 
			this.mnuStopRecordTapeFile.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuStopRecordTapeFile.Name = "mnuStopRecordTapeFile";
			this.mnuStopRecordTapeFile.Size = new System.Drawing.Size(157, 22);
			this.mnuStopRecordTapeFile.Text = "Stop recording";
			this.mnuStopRecordTapeFile.Click += new System.EventHandler(this.mnuStopRecordTapeFile_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEmulationSpeed,
            this.mnuVideoScale,
            this.mnuVideoFilter,
            this.mnuRegion,
            this.toolStripMenuItem10,
            this.mnuAudioConfig,
            this.mnuInput,
            this.mnuVideoConfig,
            this.mnuEmulationConfig,
            this.toolStripMenuItem11,
            this.mnuPreferences});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(61, 20);
			this.mnuOptions.Text = "Options";
			this.mnuOptions.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuOptions.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
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
			this.mnuEmulationSpeed.DropDownOpening += new System.EventHandler(this.mnuEmulationSpeed_DropDownOpening);
			// 
			// mnuEmuSpeedNormal
			// 
			this.mnuEmuSpeedNormal.Name = "mnuEmuSpeedNormal";
			this.mnuEmuSpeedNormal.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedNormal.Text = "Normal (100%)";
			this.mnuEmuSpeedNormal.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(160, 6);
			// 
			// mnuIncreaseSpeed
			// 
			this.mnuIncreaseSpeed.Name = "mnuIncreaseSpeed";
			this.mnuIncreaseSpeed.Size = new System.Drawing.Size(163, 22);
			this.mnuIncreaseSpeed.Text = "Increase Speed";
			// 
			// mnuDecreaseSpeed
			// 
			this.mnuDecreaseSpeed.Name = "mnuDecreaseSpeed";
			this.mnuDecreaseSpeed.Size = new System.Drawing.Size(163, 22);
			this.mnuDecreaseSpeed.Text = "Decrease Speed";
			// 
			// mnuEmuSpeedMaximumSpeed
			// 
			this.mnuEmuSpeedMaximumSpeed.Name = "mnuEmuSpeedMaximumSpeed";
			this.mnuEmuSpeedMaximumSpeed.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedMaximumSpeed.Text = "Maximum Speed";
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(160, 6);
			// 
			// mnuEmuSpeedTriple
			// 
			this.mnuEmuSpeedTriple.Name = "mnuEmuSpeedTriple";
			this.mnuEmuSpeedTriple.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedTriple.Tag = "";
			this.mnuEmuSpeedTriple.Text = "Triple (300%)";
			this.mnuEmuSpeedTriple.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedDouble
			// 
			this.mnuEmuSpeedDouble.Name = "mnuEmuSpeedDouble";
			this.mnuEmuSpeedDouble.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedDouble.Text = "Double (200%)";
			this.mnuEmuSpeedDouble.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedHalf
			// 
			this.mnuEmuSpeedHalf.Name = "mnuEmuSpeedHalf";
			this.mnuEmuSpeedHalf.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedHalf.Text = "Half (50%)";
			this.mnuEmuSpeedHalf.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// mnuEmuSpeedQuarter
			// 
			this.mnuEmuSpeedQuarter.Name = "mnuEmuSpeedQuarter";
			this.mnuEmuSpeedQuarter.Size = new System.Drawing.Size(163, 22);
			this.mnuEmuSpeedQuarter.Text = "Quarter (25%)";
			this.mnuEmuSpeedQuarter.Click += new System.EventHandler(this.mnuEmulationSpeedOption_Click);
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(160, 6);
			// 
			// mnuShowFPS
			// 
			this.mnuShowFPS.CheckOnClick = true;
			this.mnuShowFPS.Name = "mnuShowFPS";
			this.mnuShowFPS.Size = new System.Drawing.Size(163, 22);
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
            this.mnuScale5x,
            this.mnuScale6x,
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
			this.mnuScale1x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale1x.Text = "1x";
			// 
			// mnuScale2x
			// 
			this.mnuScale2x.Name = "mnuScale2x";
			this.mnuScale2x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale2x.Text = "2x";
			// 
			// mnuScale3x
			// 
			this.mnuScale3x.Name = "mnuScale3x";
			this.mnuScale3x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale3x.Text = "3x";
			// 
			// mnuScale4x
			// 
			this.mnuScale4x.Name = "mnuScale4x";
			this.mnuScale4x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale4x.Text = "4x";
			// 
			// mnuScale5x
			// 
			this.mnuScale5x.Name = "mnuScale5x";
			this.mnuScale5x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale5x.Text = "5x";
			// 
			// mnuScale6x
			// 
			this.mnuScale6x.Name = "mnuScale6x";
			this.mnuScale6x.Size = new System.Drawing.Size(127, 22);
			this.mnuScale6x.Text = "6x";
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(124, 6);
			// 
			// mnuFullscreen
			// 
			this.mnuFullscreen.Name = "mnuFullscreen";
			this.mnuFullscreen.Size = new System.Drawing.Size(127, 22);
			this.mnuFullscreen.Text = "Fullscreen";
			// 
			// mnuVideoFilter
			// 
			this.mnuVideoFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNoneFilter,
            this.toolStripMenuItem21,
            this.mnuNtscFilter,
            this.mnuNtscBisqwitQuarterFilter,
            this.mnuNtscBisqwitHalfFilter,
            this.mnuNtscBisqwitFullFilter,
            this.toolStripMenuItem15,
            this.mnuXBRZ2xFilter,
            this.mnuXBRZ3xFilter,
            this.mnuXBRZ4xFilter,
            this.mnuXBRZ5xFilter,
            this.mnuXBRZ6xFilter,
            this.toolStripMenuItem16,
            this.mnuHQ2xFilter,
            this.mnuHQ3xFilter,
            this.mnuHQ4xFilter,
            this.toolStripMenuItem17,
            this.mnuScale2xFilter,
            this.mnuScale3xFilter,
            this.mnuScale4xFilter,
            this.toolStripMenuItem23,
            this.mnu2xSaiFilter,
            this.mnuSuper2xSaiFilter,
            this.mnuSuperEagleFilter,
            this.toolStripMenuItem18,
            this.mnuPrescale2xFilter,
            this.mnuPrescale3xFilter,
            this.mnuPrescale4xFilter,
            this.mnuPrescale6xFilter,
            this.mnuPrescale8xFilter,
            this.mnuPrescale10xFilter,
            this.toolStripMenuItem19,
            this.mnuBilinearInterpolation});
			this.mnuVideoFilter.Image = global::Mesen.GUI.Properties.Resources.VideoFilter;
			this.mnuVideoFilter.Name = "mnuVideoFilter";
			this.mnuVideoFilter.Size = new System.Drawing.Size(135, 22);
			this.mnuVideoFilter.Text = "Video Filter";
			// 
			// mnuNoneFilter
			// 
			this.mnuNoneFilter.Name = "mnuNoneFilter";
			this.mnuNoneFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuNoneFilter.Text = "None";
			this.mnuNoneFilter.Click += new System.EventHandler(this.mnuNoneFilter_Click);
			// 
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuNtscFilter
			// 
			this.mnuNtscFilter.Name = "mnuNtscFilter";
			this.mnuNtscFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuNtscFilter.Text = "NTSC";
			this.mnuNtscFilter.Click += new System.EventHandler(this.mnuNtscFilter_Click);
			// 
			// mnuNtscBisqwitQuarterFilter
			// 
			this.mnuNtscBisqwitQuarterFilter.Name = "mnuNtscBisqwitQuarterFilter";
			this.mnuNtscBisqwitQuarterFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuNtscBisqwitQuarterFilter.Text = "NTSC 2x (Bisqwit)";
			this.mnuNtscBisqwitQuarterFilter.Click += new System.EventHandler(this.mnuNtscBisqwitQuarterFilter_Click);
			// 
			// mnuNtscBisqwitHalfFilter
			// 
			this.mnuNtscBisqwitHalfFilter.Name = "mnuNtscBisqwitHalfFilter";
			this.mnuNtscBisqwitHalfFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuNtscBisqwitHalfFilter.Text = "NTSC 4x (Bisqwit)";
			this.mnuNtscBisqwitHalfFilter.Click += new System.EventHandler(this.mnuNtscBisqwitHalfFilter_Click);
			// 
			// mnuNtscBisqwitFullFilter
			// 
			this.mnuNtscBisqwitFullFilter.Name = "mnuNtscBisqwitFullFilter";
			this.mnuNtscBisqwitFullFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuNtscBisqwitFullFilter.Text = "NTSC 8x (Bisqwit)";
			this.mnuNtscBisqwitFullFilter.Click += new System.EventHandler(this.mnuNtscBisqwitFullFilter_Click);
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuXBRZ2xFilter
			// 
			this.mnuXBRZ2xFilter.Name = "mnuXBRZ2xFilter";
			this.mnuXBRZ2xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuXBRZ2xFilter.Text = "xBRZ 2x";
			this.mnuXBRZ2xFilter.Click += new System.EventHandler(this.mnuXBRZ2xFilter_Click);
			// 
			// mnuXBRZ3xFilter
			// 
			this.mnuXBRZ3xFilter.Name = "mnuXBRZ3xFilter";
			this.mnuXBRZ3xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuXBRZ3xFilter.Text = "xBRZ 3x";
			this.mnuXBRZ3xFilter.Click += new System.EventHandler(this.mnuXBRZ3xFilter_Click);
			// 
			// mnuXBRZ4xFilter
			// 
			this.mnuXBRZ4xFilter.Name = "mnuXBRZ4xFilter";
			this.mnuXBRZ4xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuXBRZ4xFilter.Text = "xBRZ 4x";
			this.mnuXBRZ4xFilter.Click += new System.EventHandler(this.mnuXBRZ4xFilter_Click);
			// 
			// mnuXBRZ5xFilter
			// 
			this.mnuXBRZ5xFilter.Name = "mnuXBRZ5xFilter";
			this.mnuXBRZ5xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuXBRZ5xFilter.Text = "xBRZ 5x";
			this.mnuXBRZ5xFilter.Click += new System.EventHandler(this.mnuXBRZ5xFilter_Click);
			// 
			// mnuXBRZ6xFilter
			// 
			this.mnuXBRZ6xFilter.Name = "mnuXBRZ6xFilter";
			this.mnuXBRZ6xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuXBRZ6xFilter.Text = "xBRZ 6x";
			this.mnuXBRZ6xFilter.Click += new System.EventHandler(this.mnuXBRZ6xFilter_Click);
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuHQ2xFilter
			// 
			this.mnuHQ2xFilter.Name = "mnuHQ2xFilter";
			this.mnuHQ2xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuHQ2xFilter.Text = "HQ 2x";
			this.mnuHQ2xFilter.Click += new System.EventHandler(this.mnuHQ2xFilter_Click);
			// 
			// mnuHQ3xFilter
			// 
			this.mnuHQ3xFilter.Name = "mnuHQ3xFilter";
			this.mnuHQ3xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuHQ3xFilter.Text = "HQ 3x";
			this.mnuHQ3xFilter.Click += new System.EventHandler(this.mnuHQ3xFilter_Click);
			// 
			// mnuHQ4xFilter
			// 
			this.mnuHQ4xFilter.Name = "mnuHQ4xFilter";
			this.mnuHQ4xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuHQ4xFilter.Text = "HQ 4x";
			this.mnuHQ4xFilter.Click += new System.EventHandler(this.mnuHQ4xFilter_Click);
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuScale2xFilter
			// 
			this.mnuScale2xFilter.Name = "mnuScale2xFilter";
			this.mnuScale2xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuScale2xFilter.Text = "Scale2x";
			this.mnuScale2xFilter.Click += new System.EventHandler(this.mnuScale2xFilter_Click);
			// 
			// mnuScale3xFilter
			// 
			this.mnuScale3xFilter.Name = "mnuScale3xFilter";
			this.mnuScale3xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuScale3xFilter.Text = "Scale3x";
			this.mnuScale3xFilter.Click += new System.EventHandler(this.mnuScale3xFilter_Click);
			// 
			// mnuScale4xFilter
			// 
			this.mnuScale4xFilter.Name = "mnuScale4xFilter";
			this.mnuScale4xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuScale4xFilter.Text = "Scale4x";
			this.mnuScale4xFilter.Click += new System.EventHandler(this.mnuScale4xFilter_Click);
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(203, 6);
			// 
			// mnu2xSaiFilter
			// 
			this.mnu2xSaiFilter.Name = "mnu2xSaiFilter";
			this.mnu2xSaiFilter.Size = new System.Drawing.Size(206, 22);
			this.mnu2xSaiFilter.Text = "2xSai";
			this.mnu2xSaiFilter.Click += new System.EventHandler(this.mnu2xSaiFilter_Click);
			// 
			// mnuSuper2xSaiFilter
			// 
			this.mnuSuper2xSaiFilter.Name = "mnuSuper2xSaiFilter";
			this.mnuSuper2xSaiFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuSuper2xSaiFilter.Text = "Super2xSai";
			this.mnuSuper2xSaiFilter.Click += new System.EventHandler(this.mnuSuper2xSaiFilter_Click);
			// 
			// mnuSuperEagleFilter
			// 
			this.mnuSuperEagleFilter.Name = "mnuSuperEagleFilter";
			this.mnuSuperEagleFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuSuperEagleFilter.Text = "SuperEagle";
			this.mnuSuperEagleFilter.Click += new System.EventHandler(this.mnuSuperEagleFilter_Click);
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			this.toolStripMenuItem18.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuPrescale2xFilter
			// 
			this.mnuPrescale2xFilter.Name = "mnuPrescale2xFilter";
			this.mnuPrescale2xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale2xFilter.Text = "Prescale 2x";
			this.mnuPrescale2xFilter.Click += new System.EventHandler(this.mnuPrescale2xFilter_Click);
			// 
			// mnuPrescale3xFilter
			// 
			this.mnuPrescale3xFilter.Name = "mnuPrescale3xFilter";
			this.mnuPrescale3xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale3xFilter.Text = "Prescale 3x";
			this.mnuPrescale3xFilter.Click += new System.EventHandler(this.mnuPrescale3xFilter_Click);
			// 
			// mnuPrescale4xFilter
			// 
			this.mnuPrescale4xFilter.Name = "mnuPrescale4xFilter";
			this.mnuPrescale4xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale4xFilter.Text = "Prescale 4x";
			this.mnuPrescale4xFilter.Click += new System.EventHandler(this.mnuPrescale4xFilter_Click);
			// 
			// mnuPrescale6xFilter
			// 
			this.mnuPrescale6xFilter.Name = "mnuPrescale6xFilter";
			this.mnuPrescale6xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale6xFilter.Text = "Prescale 6x";
			this.mnuPrescale6xFilter.Click += new System.EventHandler(this.mnuPrescale6xFilter_Click);
			// 
			// mnuPrescale8xFilter
			// 
			this.mnuPrescale8xFilter.Name = "mnuPrescale8xFilter";
			this.mnuPrescale8xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale8xFilter.Text = "Prescale 8x";
			this.mnuPrescale8xFilter.Click += new System.EventHandler(this.mnuPrescale8xFilter_Click);
			// 
			// mnuPrescale10xFilter
			// 
			this.mnuPrescale10xFilter.Name = "mnuPrescale10xFilter";
			this.mnuPrescale10xFilter.Size = new System.Drawing.Size(206, 22);
			this.mnuPrescale10xFilter.Text = "Prescale 10x";
			this.mnuPrescale10xFilter.Click += new System.EventHandler(this.mnuPrescale10xFilter_Click);
			// 
			// toolStripMenuItem19
			// 
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			this.toolStripMenuItem19.Size = new System.Drawing.Size(203, 6);
			// 
			// mnuBilinearInterpolation
			// 
			this.mnuBilinearInterpolation.CheckOnClick = true;
			this.mnuBilinearInterpolation.Name = "mnuBilinearInterpolation";
			this.mnuBilinearInterpolation.Size = new System.Drawing.Size(206, 22);
			this.mnuBilinearInterpolation.Text = "Use Bilinear Interpolation";
			this.mnuBilinearInterpolation.Click += new System.EventHandler(this.mnuBilinearInterpolation_Click);
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
			// mnuVideoConfig
			// 
			this.mnuVideoConfig.Image = global::Mesen.GUI.Properties.Resources.Video;
			this.mnuVideoConfig.Name = "mnuVideoConfig";
			this.mnuVideoConfig.Size = new System.Drawing.Size(135, 22);
			this.mnuVideoConfig.Text = "Video";
			this.mnuVideoConfig.Click += new System.EventHandler(this.mnuVideoConfig_Click);
			// 
			// mnuEmulationConfig
			// 
			this.mnuEmulationConfig.Image = global::Mesen.GUI.Properties.Resources.DipSwitches;
			this.mnuEmulationConfig.Name = "mnuEmulationConfig";
			this.mnuEmulationConfig.Size = new System.Drawing.Size(135, 22);
			this.mnuEmulationConfig.Text = "Emulation";
			this.mnuEmulationConfig.Click += new System.EventHandler(this.mnuEmulationConfig_Click);
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
            this.toolStripMenuItem22,
            this.mnuSoundRecorder,
            this.mnuVideoRecorder,
            this.toolStripMenuItem12,
            this.mnuTests,
            this.mnuDebugger,
            this.mnuLogWindow,
            this.toolStripMenuItem27,
            this.mnuInstallHdPack,
            this.mnuHdPackEditor,
            this.toolStripMenuItem1,
            this.mnuRandomGame,
            this.mnuTakeScreenshot});
			this.mnuTools.Name = "mnuTools";
			this.mnuTools.Size = new System.Drawing.Size(47, 20);
			this.mnuTools.Text = "Tools";
			this.mnuTools.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuTools.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
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
			this.mnuNetPlay.Size = new System.Drawing.Size(182, 22);
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
            this.mnuNetPlayPlayer5,
            this.toolStripMenuItem3,
            this.mnuNetPlaySpectator});
			this.mnuNetPlaySelectController.Name = "mnuNetPlaySelectController";
			this.mnuNetPlaySelectController.Size = new System.Drawing.Size(177, 22);
			this.mnuNetPlaySelectController.Text = "Select Controller";
			// 
			// mnuNetPlayPlayer1
			// 
			this.mnuNetPlayPlayer1.Name = "mnuNetPlayPlayer1";
			this.mnuNetPlayPlayer1.Size = new System.Drawing.Size(165, 22);
			this.mnuNetPlayPlayer1.Text = "Player 1";
			this.mnuNetPlayPlayer1.Click += new System.EventHandler(this.mnuNetPlayPlayer1_Click);
			// 
			// mnuNetPlayPlayer2
			// 
			this.mnuNetPlayPlayer2.Name = "mnuNetPlayPlayer2";
			this.mnuNetPlayPlayer2.Size = new System.Drawing.Size(165, 22);
			this.mnuNetPlayPlayer2.Text = "Player 2";
			this.mnuNetPlayPlayer2.Click += new System.EventHandler(this.mnuNetPlayPlayer2_Click);
			// 
			// mnuNetPlayPlayer3
			// 
			this.mnuNetPlayPlayer3.Name = "mnuNetPlayPlayer3";
			this.mnuNetPlayPlayer3.Size = new System.Drawing.Size(165, 22);
			this.mnuNetPlayPlayer3.Text = "Player 3";
			this.mnuNetPlayPlayer3.Click += new System.EventHandler(this.mnuNetPlayPlayer3_Click);
			// 
			// mnuNetPlayPlayer4
			// 
			this.mnuNetPlayPlayer4.Name = "mnuNetPlayPlayer4";
			this.mnuNetPlayPlayer4.Size = new System.Drawing.Size(165, 22);
			this.mnuNetPlayPlayer4.Text = "Player 4";
			this.mnuNetPlayPlayer4.Click += new System.EventHandler(this.mnuNetPlayPlayer4_Click);
			// 
			// mnuNetPlayPlayer5
			// 
			this.mnuNetPlayPlayer5.Name = "mnuNetPlayPlayer5";
			this.mnuNetPlayPlayer5.Size = new System.Drawing.Size(165, 22);
			this.mnuNetPlayPlayer5.Text = "Expansion Device";
			this.mnuNetPlayPlayer5.Click += new System.EventHandler(this.mnuNetPlayPlayer5_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(162, 6);
			// 
			// mnuNetPlaySpectator
			// 
			this.mnuNetPlaySpectator.Name = "mnuNetPlaySpectator";
			this.mnuNetPlaySpectator.Size = new System.Drawing.Size(165, 22);
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
            this.mnuRecordMovie,
            this.mnuStopMovie});
			this.mnuMovies.Image = global::Mesen.GUI.Properties.Resources.Movie;
			this.mnuMovies.Name = "mnuMovies";
			this.mnuMovies.Size = new System.Drawing.Size(182, 22);
			this.mnuMovies.Text = "Movies";
			// 
			// mnuPlayMovie
			// 
			this.mnuPlayMovie.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.mnuPlayMovie.Name = "mnuPlayMovie";
			this.mnuPlayMovie.Size = new System.Drawing.Size(120, 22);
			this.mnuPlayMovie.Text = "Play...";
			this.mnuPlayMovie.Click += new System.EventHandler(this.mnuPlayMovie_Click);
			// 
			// mnuRecordMovie
			// 
			this.mnuRecordMovie.Image = global::Mesen.GUI.Properties.Resources.Record;
			this.mnuRecordMovie.Name = "mnuRecordMovie";
			this.mnuRecordMovie.Size = new System.Drawing.Size(120, 22);
			this.mnuRecordMovie.Text = "Record...";
			this.mnuRecordMovie.Click += new System.EventHandler(this.mnuRecordMovie_Click);
			// 
			// mnuStopMovie
			// 
			this.mnuStopMovie.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuStopMovie.Name = "mnuStopMovie";
			this.mnuStopMovie.Size = new System.Drawing.Size(120, 22);
			this.mnuStopMovie.Text = "Stop";
			this.mnuStopMovie.Click += new System.EventHandler(this.mnuStopMovie_Click);
			// 
			// mnuCheats
			// 
			this.mnuCheats.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuCheats.Name = "mnuCheats";
			this.mnuCheats.Size = new System.Drawing.Size(182, 22);
			this.mnuCheats.Text = "Cheats";
			this.mnuCheats.Click += new System.EventHandler(this.mnuCheats_Click);
			// 
			// toolStripMenuItem22
			// 
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			this.toolStripMenuItem22.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuSoundRecorder
			// 
			this.mnuSoundRecorder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWaveRecord,
            this.mnuWaveStop});
			this.mnuSoundRecorder.Image = global::Mesen.GUI.Properties.Resources.Microphone;
			this.mnuSoundRecorder.Name = "mnuSoundRecorder";
			this.mnuSoundRecorder.Size = new System.Drawing.Size(182, 22);
			this.mnuSoundRecorder.Text = "Sound Recorder";
			// 
			// mnuWaveRecord
			// 
			this.mnuWaveRecord.Image = global::Mesen.GUI.Properties.Resources.Record;
			this.mnuWaveRecord.Name = "mnuWaveRecord";
			this.mnuWaveRecord.Size = new System.Drawing.Size(155, 22);
			this.mnuWaveRecord.Text = "Record...";
			this.mnuWaveRecord.Click += new System.EventHandler(this.mnuWaveRecord_Click);
			// 
			// mnuWaveStop
			// 
			this.mnuWaveStop.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuWaveStop.Name = "mnuWaveStop";
			this.mnuWaveStop.Size = new System.Drawing.Size(155, 22);
			this.mnuWaveStop.Text = "Stop Recording";
			this.mnuWaveStop.Click += new System.EventHandler(this.mnuWaveStop_Click);
			// 
			// mnuVideoRecorder
			// 
			this.mnuVideoRecorder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAviRecord,
            this.mnuAviStop});
			this.mnuVideoRecorder.Image = global::Mesen.GUI.Properties.Resources.VideoRecorder;
			this.mnuVideoRecorder.Name = "mnuVideoRecorder";
			this.mnuVideoRecorder.Size = new System.Drawing.Size(182, 22);
			this.mnuVideoRecorder.Text = "Video Recorder";
			// 
			// mnuAviRecord
			// 
			this.mnuAviRecord.Image = global::Mesen.GUI.Properties.Resources.Record;
			this.mnuAviRecord.Name = "mnuAviRecord";
			this.mnuAviRecord.Size = new System.Drawing.Size(155, 22);
			this.mnuAviRecord.Text = "Record...";
			this.mnuAviRecord.Click += new System.EventHandler(this.mnuAviRecord_Click);
			// 
			// mnuAviStop
			// 
			this.mnuAviStop.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.mnuAviStop.Name = "mnuAviStop";
			this.mnuAviStop.Size = new System.Drawing.Size(155, 22);
			this.mnuAviStop.Text = "Stop Recording";
			this.mnuAviStop.Click += new System.EventHandler(this.mnuAviStop_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuTests
			// 
			this.mnuTests.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTestRun,
            this.mnuTestRecordFrom,
            this.mnuTestStopRecording,
            this.mnuRunAllTests,
            this.mnuRunAllGameTests,
            this.mnuRunAutomaticTest});
			this.mnuTests.Name = "mnuTests";
			this.mnuTests.Size = new System.Drawing.Size(182, 22);
			this.mnuTests.Text = "Tests";
			// 
			// mnuTestRun
			// 
			this.mnuTestRun.Name = "mnuTestRun";
			this.mnuTestRun.Size = new System.Drawing.Size(174, 22);
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
			this.mnuTestRecordFrom.Size = new System.Drawing.Size(174, 22);
			this.mnuTestRecordFrom.Text = "Record from...";
			// 
			// mnuTestRecordStart
			// 
			this.mnuTestRecordStart.Name = "mnuTestRecordStart";
			this.mnuTestRecordStart.Size = new System.Drawing.Size(107, 22);
			this.mnuTestRecordStart.Text = "Start";
			this.mnuTestRecordStart.Click += new System.EventHandler(this.mnuTestRecordStart_Click);
			// 
			// mnuTestRecordNow
			// 
			this.mnuTestRecordNow.Name = "mnuTestRecordNow";
			this.mnuTestRecordNow.Size = new System.Drawing.Size(107, 22);
			this.mnuTestRecordNow.Text = "Now";
			this.mnuTestRecordNow.Click += new System.EventHandler(this.mnuTestRecordNow_Click);
			// 
			// mnuTestRecordMovie
			// 
			this.mnuTestRecordMovie.Name = "mnuTestRecordMovie";
			this.mnuTestRecordMovie.Size = new System.Drawing.Size(107, 22);
			this.mnuTestRecordMovie.Text = "Movie";
			this.mnuTestRecordMovie.Click += new System.EventHandler(this.mnuTestRecordMovie_Click);
			// 
			// mnuTestRecordTest
			// 
			this.mnuTestRecordTest.Name = "mnuTestRecordTest";
			this.mnuTestRecordTest.Size = new System.Drawing.Size(107, 22);
			this.mnuTestRecordTest.Text = "Test";
			this.mnuTestRecordTest.Click += new System.EventHandler(this.mnuTestRecordTest_Click);
			// 
			// mnuTestStopRecording
			// 
			this.mnuTestStopRecording.Name = "mnuTestStopRecording";
			this.mnuTestStopRecording.Size = new System.Drawing.Size(174, 22);
			this.mnuTestStopRecording.Text = "Stop recording";
			this.mnuTestStopRecording.Click += new System.EventHandler(this.mnuTestStopRecording_Click);
			// 
			// mnuRunAllTests
			// 
			this.mnuRunAllTests.Name = "mnuRunAllTests";
			this.mnuRunAllTests.Size = new System.Drawing.Size(174, 22);
			this.mnuRunAllTests.Text = "Run all tests";
			this.mnuRunAllTests.Click += new System.EventHandler(this.mnuRunAllTests_Click);
			// 
			// mnuRunAllGameTests
			// 
			this.mnuRunAllGameTests.Name = "mnuRunAllGameTests";
			this.mnuRunAllGameTests.Size = new System.Drawing.Size(174, 22);
			this.mnuRunAllGameTests.Text = "Run all game tests";
			this.mnuRunAllGameTests.Click += new System.EventHandler(this.mnuRunAllGameTests_Click);
			// 
			// mnuRunAutomaticTest
			// 
			this.mnuRunAutomaticTest.Name = "mnuRunAutomaticTest";
			this.mnuRunAutomaticTest.Size = new System.Drawing.Size(174, 22);
			this.mnuRunAutomaticTest.Text = "Run automatic test";
			this.mnuRunAutomaticTest.Click += new System.EventHandler(this.mnuRunAutomaticTest_Click);
			// 
			// mnuDebugger
			// 
			this.mnuDebugger.Image = global::Mesen.GUI.Properties.Resources.Bug;
			this.mnuDebugger.Name = "mnuDebugger";
			this.mnuDebugger.Size = new System.Drawing.Size(182, 22);
			this.mnuDebugger.Text = "Debugger";
			this.mnuDebugger.Click += new System.EventHandler(this.mnuDebugger_Click);
			// 
			// mnuLogWindow
			// 
			this.mnuLogWindow.Image = global::Mesen.GUI.Properties.Resources.LogWindow;
			this.mnuLogWindow.Name = "mnuLogWindow";
			this.mnuLogWindow.Size = new System.Drawing.Size(182, 22);
			this.mnuLogWindow.Text = "Log Window";
			this.mnuLogWindow.Click += new System.EventHandler(this.mnuLogWindow_Click);
			// 
			// mnuHdPackEditor
			// 
			this.mnuHdPackEditor.Image = global::Mesen.GUI.Properties.Resources.HdPack;
			this.mnuHdPackEditor.Name = "mnuHdPackEditor";
			this.mnuHdPackEditor.Size = new System.Drawing.Size(182, 22);
			this.mnuHdPackEditor.Text = "HD Pack Builder";
			this.mnuHdPackEditor.Click += new System.EventHandler(this.mnuHdPackEditor_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuRandomGame
			// 
			this.mnuRandomGame.Image = global::Mesen.GUI.Properties.Resources.Dice;
			this.mnuRandomGame.Name = "mnuRandomGame";
			this.mnuRandomGame.Size = new System.Drawing.Size(182, 22);
			this.mnuRandomGame.Text = "Load Random Game";
			// 
			// mnuTakeScreenshot
			// 
			this.mnuTakeScreenshot.Image = global::Mesen.GUI.Properties.Resources.Camera;
			this.mnuTakeScreenshot.Name = "mnuTakeScreenshot";
			this.mnuTakeScreenshot.Size = new System.Drawing.Size(182, 22);
			this.mnuTakeScreenshot.Text = "Take Screenshot";
			// 
			// mnuDebug
			// 
			this.mnuDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApuViewer,
            this.mnuAssembler,
            this.mnuDebugDebugger,
            this.mnuEventViewer,
            this.mnuMemoryViewer,
            this.mnuPpuViewer,
            this.mnuScriptWindow,
            this.mnuTraceLogger,
            this.toolStripMenuItem25,
            this.mnuEditHeader});
			this.mnuDebug.Name = "mnuDebug";
			this.mnuDebug.Size = new System.Drawing.Size(54, 20);
			this.mnuDebug.Text = "Debug";
			this.mnuDebug.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuDebug.DropDownOpening += new System.EventHandler(this.mnuDebug_DropDownOpening);
			this.mnuDebug.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
			// 
			// mnuApuViewer
			// 
			this.mnuApuViewer.Image = global::Mesen.GUI.Properties.Resources.Audio;
			this.mnuApuViewer.Name = "mnuApuViewer";
			this.mnuApuViewer.Size = new System.Drawing.Size(162, 22);
			this.mnuApuViewer.Text = "APU Viewer";
			this.mnuApuViewer.Click += new System.EventHandler(this.mnuApuViewer_Click);
			// 
			// mnuAssembler
			// 
			this.mnuAssembler.Image = global::Mesen.GUI.Properties.Resources.Chip;
			this.mnuAssembler.Name = "mnuAssembler";
			this.mnuAssembler.Size = new System.Drawing.Size(162, 22);
			this.mnuAssembler.Text = "Assembler";
			this.mnuAssembler.Click += new System.EventHandler(this.mnuAssembler_Click);
			// 
			// mnuDebugDebugger
			// 
			this.mnuDebugDebugger.Image = global::Mesen.GUI.Properties.Resources.Bug;
			this.mnuDebugDebugger.Name = "mnuDebugDebugger";
			this.mnuDebugDebugger.Size = new System.Drawing.Size(162, 22);
			this.mnuDebugDebugger.Text = "Debugger";
			this.mnuDebugDebugger.Click += new System.EventHandler(this.mnuDebugDebugger_Click);
			// 
			// mnuEventViewer
			// 
			this.mnuEventViewer.Image = global::Mesen.GUI.Properties.Resources.NesEventViewer;
			this.mnuEventViewer.Name = "mnuEventViewer";
			this.mnuEventViewer.Size = new System.Drawing.Size(162, 22);
			this.mnuEventViewer.Text = "Event Viewer";
			this.mnuEventViewer.Click += new System.EventHandler(this.mnuEventViewer_Click);
			// 
			// mnuMemoryViewer
			// 
			this.mnuMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuMemoryViewer.Name = "mnuMemoryViewer";
			this.mnuMemoryViewer.Size = new System.Drawing.Size(162, 22);
			this.mnuMemoryViewer.Text = "Memory Tools";
			this.mnuMemoryViewer.Click += new System.EventHandler(this.mnuMemoryViewer_Click);
			// 
			// mnuPpuViewer
			// 
			this.mnuPpuViewer.Image = global::Mesen.GUI.Properties.Resources.Video;
			this.mnuPpuViewer.Name = "mnuPpuViewer";
			this.mnuPpuViewer.Size = new System.Drawing.Size(162, 22);
			this.mnuPpuViewer.Text = "PPU Viewer";
			this.mnuPpuViewer.Click += new System.EventHandler(this.mnuPpuViewer_Click);
			// 
			// mnuScriptWindow
			// 
			this.mnuScriptWindow.Image = global::Mesen.GUI.Properties.Resources.Script;
			this.mnuScriptWindow.Name = "mnuScriptWindow";
			this.mnuScriptWindow.Size = new System.Drawing.Size(162, 22);
			this.mnuScriptWindow.Text = "Script Window";
			this.mnuScriptWindow.Click += new System.EventHandler(this.mnuScriptWindow_Click);
			// 
			// mnuTraceLogger
			// 
			this.mnuTraceLogger.Image = global::Mesen.GUI.Properties.Resources.LogWindow;
			this.mnuTraceLogger.Name = "mnuTraceLogger";
			this.mnuTraceLogger.Size = new System.Drawing.Size(162, 22);
			this.mnuTraceLogger.Text = "Trace Logger";
			this.mnuTraceLogger.Click += new System.EventHandler(this.mnuTraceLogger_Click);
			// 
			// toolStripMenuItem25
			// 
			this.toolStripMenuItem25.Name = "toolStripMenuItem25";
			this.toolStripMenuItem25.Size = new System.Drawing.Size(159, 6);
			// 
			// mnuEditHeader
			// 
			this.mnuEditHeader.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditHeader.Name = "mnuEditHeader";
			this.mnuEditHeader.Size = new System.Drawing.Size(162, 22);
			this.mnuEditHeader.Text = "Edit iNES Header";
			this.mnuEditHeader.Click += new System.EventHandler(this.mnuEditHeader_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOnlineHelp,
            this.mnuHelpWindow,
            this.toolStripMenuItem26,
            this.mnuCheckForUpdates,
            this.toolStripMenuItem20,
            this.mnuReportBug,
            this.toolStripMenuItem5,
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(44, 20);
			this.mnuHelp.Text = "Help";
			this.mnuHelp.DropDownClosed += new System.EventHandler(this.mnu_DropDownClosed);
			this.mnuHelp.DropDownOpened += new System.EventHandler(this.mnu_DropDownOpened);
			// 
			// mnuOnlineHelp
			// 
			this.mnuOnlineHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.mnuOnlineHelp.Name = "mnuOnlineHelp";
			this.mnuOnlineHelp.Size = new System.Drawing.Size(198, 22);
			this.mnuOnlineHelp.Text = "Online Help";
			this.mnuOnlineHelp.Click += new System.EventHandler(this.mnuOnlineHelp_Click);
			// 
			// mnuHelpWindow
			// 
			this.mnuHelpWindow.Image = global::Mesen.GUI.Properties.Resources.CommandLine;
			this.mnuHelpWindow.Name = "mnuHelpWindow";
			this.mnuHelpWindow.Size = new System.Drawing.Size(198, 22);
			this.mnuHelpWindow.Text = "Command-line options";
			this.mnuHelpWindow.Click += new System.EventHandler(this.mnuHelpWindow_Click);
			// 
			// toolStripMenuItem26
			// 
			this.toolStripMenuItem26.Name = "toolStripMenuItem26";
			this.toolStripMenuItem26.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuCheckForUpdates
			// 
			this.mnuCheckForUpdates.Image = global::Mesen.GUI.Properties.Resources.SoftwareUpdate;
			this.mnuCheckForUpdates.Name = "mnuCheckForUpdates";
			this.mnuCheckForUpdates.Size = new System.Drawing.Size(198, 22);
			this.mnuCheckForUpdates.Text = "Check for updates";
			this.mnuCheckForUpdates.Click += new System.EventHandler(this.mnuCheckForUpdates_Click);
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuReportBug
			// 
			this.mnuReportBug.Image = global::Mesen.GUI.Properties.Resources.Comment;
			this.mnuReportBug.Name = "mnuReportBug";
			this.mnuReportBug.Size = new System.Drawing.Size(198, 22);
			this.mnuReportBug.Text = "Report a bug";
			this.mnuReportBug.Click += new System.EventHandler(this.mnuReportBug_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Image = global::Mesen.GUI.Properties.Resources.Exclamation;
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(198, 22);
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// toolStripMenuItem27
			// 
			this.toolStripMenuItem27.Name = "toolStripMenuItem27";
			this.toolStripMenuItem27.Size = new System.Drawing.Size(179, 6);
			// 
			// mnuInstallHdPack
			// 
			this.mnuInstallHdPack.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuInstallHdPack.Name = "mnuInstallHdPack";
			this.mnuInstallHdPack.Size = new System.Drawing.Size(182, 22);
			this.mnuInstallHdPack.Text = "Install HD Pack";
			this.mnuInstallHdPack.Click += new System.EventHandler(this.mnuInstallHdPack_Click);
			// 
			// frmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(430, 333);
			this.Controls.Add(this.panelRenderer);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(340, 230);
			this.Name = "frmMain";
			this.Text = "Mesen";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
			this.Resize += new System.EventHandler(this.frmMain_Resize);
			this.panelRenderer.ResumeLayout(false);
			this.panelInfo.ResumeLayout(false);
			this.panelInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Mesen.GUI.Controls.ctrlMesenMenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuGame;
		private Mesen.GUI.Controls.ctrlRenderer ctrlRenderer;
		private System.Windows.Forms.ToolStripMenuItem mnuPause;
		private System.Windows.Forms.ToolStripMenuItem mnuReset;
		private System.Windows.Forms.ToolStripMenuItem mnuPowerCycle;
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
		private System.Windows.Forms.ToolStripMenuItem mnuRecordMovie;
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
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
		private System.Windows.Forms.ToolStripMenuItem mnuShowFPS;
		private System.Windows.Forms.ToolStripMenuItem mnuFullscreen;
		private System.Windows.Forms.ToolStripSeparator sepVsSystem;
		private System.Windows.Forms.ToolStripMenuItem mnuInsertCoin1;
		private System.Windows.Forms.ToolStripMenuItem mnuVsGameConfig;
		private System.Windows.Forms.ToolStripMenuItem mnuInsertCoin2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
		private System.Windows.Forms.ToolStripMenuItem mnuXBRZ2xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuXBRZ3xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuXBRZ4xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuXBRZ5xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuXBRZ6xFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
		private System.Windows.Forms.ToolStripMenuItem mnuHQ2xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuHQ3xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuHQ4xFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
		private System.Windows.Forms.ToolStripMenuItem mnuScale2xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuScale3xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuScale4xFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
		private System.Windows.Forms.ToolStripMenuItem mnu2xSaiFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuSuper2xSaiFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuSuperEagleFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem19;
		private System.Windows.Forms.ToolStripMenuItem mnuBilinearInterpolation;
		private System.Windows.Forms.ToolStripMenuItem mnuSoundRecorder;
		private System.Windows.Forms.ToolStripMenuItem mnuWaveRecord;
		private System.Windows.Forms.ToolStripMenuItem mnuWaveStop;
		private System.Windows.Forms.ToolStripMenuItem mnuScale5x;
		private System.Windows.Forms.ToolStripMenuItem mnuScale6x;
		private System.Windows.Forms.ToolStripMenuItem mnuLogWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuEmulationConfig;
		private Controls.ctrlNsfPlayer ctrlNsfPlayer;
		private Controls.ctrlLoadingRom ctrlLoading;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale4xFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem23;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale2xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale3xFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem20;
		private System.Windows.Forms.ToolStripMenuItem mnuReportBug;
		private System.Windows.Forms.ToolStripMenuItem mnuRunAllGameTests;
		private System.Windows.Forms.ToolStripMenuItem mnuRandomGame;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
		private System.Windows.Forms.ToolStripMenuItem mnuNtscBisqwitHalfFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuNtscBisqwitFullFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuNtscBisqwitQuarterFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem22;
		private System.Windows.Forms.ToolStripMenuItem mnuVideoRecorder;
		private System.Windows.Forms.ToolStripMenuItem mnuAviRecord;
		private System.Windows.Forms.ToolStripMenuItem mnuAviStop;
		private System.Windows.Forms.ToolStripMenuItem mnuHelpWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale6xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale8xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuPrescale10xFilter;
		private System.Windows.Forms.ToolStripMenuItem mnuRunAutomaticTest;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Panel panelInfo;
		private System.Windows.Forms.Label lblVersion;
		private Controls.ctrlRecentGames ctrlRecentGames;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
		private System.Windows.Forms.ToolStripMenuItem mnuPowerOff;
		private System.Windows.Forms.ToolStripMenuItem mnuHdPackEditor;
		private System.Windows.Forms.ToolStripMenuItem mnuDebug;
		private System.Windows.Forms.ToolStripMenuItem mnuAssembler;
		private System.Windows.Forms.ToolStripMenuItem mnuDebugDebugger;
		private System.Windows.Forms.ToolStripMenuItem mnuMemoryViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuPpuViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuTraceLogger;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem25;
		private System.Windows.Forms.ToolStripMenuItem mnuEditHeader;
		private System.Windows.Forms.ToolStripMenuItem mnuScriptWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuOnlineHelp;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem26;
		private System.Windows.Forms.ToolStripSeparator sepBarcode;
		private System.Windows.Forms.ToolStripMenuItem mnuInputBarcode;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlayPlayer5;
		private System.Windows.Forms.ToolStripMenuItem mnuTapeRecorder;
		private System.Windows.Forms.ToolStripMenuItem mnuLoadTapeFile;
		private System.Windows.Forms.ToolStripMenuItem mnuStartRecordTapeFile;
		private System.Windows.Forms.ToolStripMenuItem mnuStopRecordTapeFile;
		private System.Windows.Forms.ToolStripMenuItem mnuApuViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuEventViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuLoadLastSession;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem27;
		private System.Windows.Forms.ToolStripMenuItem mnuInstallHdPack;
	}
}

