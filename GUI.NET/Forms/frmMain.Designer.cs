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
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLimitFPS = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowFPS = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAudioConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInput = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegion = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionAuto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionNtsc = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRegionPal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVideoConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNetPlay = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStartServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStopServer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuProfile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMovies = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPlayMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFrom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFromStart = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecordFromNow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStopMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebugger = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTakeScreenshot = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.dxViewer = new Mesen.GUI.Controls.DXViewer();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
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
			this.menuStrip.Size = new System.Drawing.Size(365, 24);
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
            this.mnuStop});
			this.mnuGame.Name = "mnuGame";
			this.mnuGame.Size = new System.Drawing.Size(50, 20);
			this.mnuGame.Text = "Game";
			// 
			// mnuPause
			// 
			this.mnuPause.Enabled = false;
			this.mnuPause.Name = "mnuPause";
			this.mnuPause.ShortcutKeyDisplayString = "Esc";
			this.mnuPause.Size = new System.Drawing.Size(129, 22);
			this.mnuPause.Text = "Pause";
			this.mnuPause.Click += new System.EventHandler(this.mnuPause_Click);
			// 
			// mnuReset
			// 
			this.mnuReset.Enabled = false;
			this.mnuReset.Name = "mnuReset";
			this.mnuReset.Size = new System.Drawing.Size(129, 22);
			this.mnuReset.Text = "Reset";
			this.mnuReset.Click += new System.EventHandler(this.mnuReset_Click);
			// 
			// mnuStop
			// 
			this.mnuStop.Enabled = false;
			this.mnuStop.Name = "mnuStop";
			this.mnuStop.Size = new System.Drawing.Size(129, 22);
			this.mnuStop.Text = "Stop";
			this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLimitFPS,
            this.mnuShowFPS,
            this.toolStripMenuItem1,
            this.mnuAudioConfig,
            this.mnuInput,
            this.mnuRegion,
            this.mnuVideoConfig});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(61, 20);
			this.mnuOptions.Text = "Options";
			// 
			// mnuLimitFPS
			// 
			this.mnuLimitFPS.CheckOnClick = true;
			this.mnuLimitFPS.Name = "mnuLimitFPS";
			this.mnuLimitFPS.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mnuLimitFPS.Size = new System.Drawing.Size(150, 22);
			this.mnuLimitFPS.Text = "Limit FPS";
			this.mnuLimitFPS.Click += new System.EventHandler(this.mnuLimitFPS_Click);
			// 
			// mnuShowFPS
			// 
			this.mnuShowFPS.CheckOnClick = true;
			this.mnuShowFPS.Name = "mnuShowFPS";
			this.mnuShowFPS.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.mnuShowFPS.Size = new System.Drawing.Size(150, 22);
			this.mnuShowFPS.Text = "Show FPS";
			this.mnuShowFPS.Click += new System.EventHandler(this.mnuShowFPS_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
			// 
			// mnuAudioConfig
			// 
			this.mnuAudioConfig.Name = "mnuAudioConfig";
			this.mnuAudioConfig.Size = new System.Drawing.Size(150, 22);
			this.mnuAudioConfig.Text = "Audio";
			this.mnuAudioConfig.Click += new System.EventHandler(this.mnuAudioConfig_Click);
			// 
			// mnuInput
			// 
			this.mnuInput.Name = "mnuInput";
			this.mnuInput.Size = new System.Drawing.Size(150, 22);
			this.mnuInput.Text = "Input";
			this.mnuInput.Click += new System.EventHandler(this.mnuInput_Click);
			// 
			// mnuRegion
			// 
			this.mnuRegion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRegionAuto,
            this.mnuRegionNtsc,
            this.mnuRegionPal});
			this.mnuRegion.Name = "mnuRegion";
			this.mnuRegion.Size = new System.Drawing.Size(150, 22);
			this.mnuRegion.Text = "Region";
			// 
			// mnuRegionAuto
			// 
			this.mnuRegionAuto.Name = "mnuRegionAuto";
			this.mnuRegionAuto.Size = new System.Drawing.Size(104, 22);
			this.mnuRegionAuto.Text = "Auto";
			this.mnuRegionAuto.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuRegionNtsc
			// 
			this.mnuRegionNtsc.Name = "mnuRegionNtsc";
			this.mnuRegionNtsc.Size = new System.Drawing.Size(104, 22);
			this.mnuRegionNtsc.Text = "NTSC";
			this.mnuRegionNtsc.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuRegionPal
			// 
			this.mnuRegionPal.Name = "mnuRegionPal";
			this.mnuRegionPal.Size = new System.Drawing.Size(104, 22);
			this.mnuRegionPal.Text = "PAL";
			this.mnuRegionPal.Click += new System.EventHandler(this.mnuRegion_Click);
			// 
			// mnuVideoConfig
			// 
			this.mnuVideoConfig.Name = "mnuVideoConfig";
			this.mnuVideoConfig.Size = new System.Drawing.Size(150, 22);
			this.mnuVideoConfig.Text = "Video";
			this.mnuVideoConfig.Click += new System.EventHandler(this.mnuVideoConfig_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNetPlay,
            this.mnuCheats,
            this.mnuMovies,
            this.mnuDebugger,
            this.mnuTakeScreenshot});
			this.mnuTools.Name = "mnuTools";
			this.mnuTools.Size = new System.Drawing.Size(48, 20);
			this.mnuTools.Text = "Tools";
			// 
			// mnuNetPlay
			// 
			this.mnuNetPlay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStartServer,
            this.mnuStopServer,
            this.toolStripMenuItem2,
            this.mnuFindServer,
            this.mnuConnect,
            this.mnuDisconnect,
            this.toolStripMenuItem3,
            this.mnuProfile});
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
			// mnuStopServer
			// 
			this.mnuStopServer.Name = "mnuStopServer";
			this.mnuStopServer.Size = new System.Drawing.Size(177, 22);
			this.mnuStopServer.Text = "Stop Server";
			this.mnuStopServer.Click += new System.EventHandler(this.mnuStopServer_Click);
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
			// 
			// mnuConnect
			// 
			this.mnuConnect.Name = "mnuConnect";
			this.mnuConnect.Size = new System.Drawing.Size(177, 22);
			this.mnuConnect.Text = "Connect...";
			this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
			// 
			// mnuDisconnect
			// 
			this.mnuDisconnect.Name = "mnuDisconnect";
			this.mnuDisconnect.Size = new System.Drawing.Size(177, 22);
			this.mnuDisconnect.Text = "Disconnect";
			this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuProfile
			// 
			this.mnuProfile.Name = "mnuProfile";
			this.mnuProfile.Size = new System.Drawing.Size(177, 22);
			this.mnuProfile.Text = "Configure Profile";
			this.mnuProfile.Click += new System.EventHandler(this.mnuProfile_Click);
			// 
			// mnuCheats
			// 
			this.mnuCheats.Name = "mnuCheats";
			this.mnuCheats.Size = new System.Drawing.Size(185, 22);
			this.mnuCheats.Text = "Cheats";
			this.mnuCheats.Click += new System.EventHandler(this.mnuCheats_Click);
			// 
			// mnuMovies
			// 
			this.mnuMovies.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlayMovie,
            this.mnuRecordFrom,
            this.mnuStopMovie});
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
			// mnuDebugger
			// 
			this.mnuDebugger.Name = "mnuDebugger";
			this.mnuDebugger.Size = new System.Drawing.Size(185, 22);
			this.mnuDebugger.Text = "Debugger";
			this.mnuDebugger.Click += new System.EventHandler(this.mnuDebugger_Click);
			// 
			// mnuTakeScreenshot
			// 
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
			this.mnuCheckForUpdates.Enabled = false;
			this.mnuCheckForUpdates.Name = "mnuCheckForUpdates";
			this.mnuCheckForUpdates.Size = new System.Drawing.Size(170, 22);
			this.mnuCheckForUpdates.Text = "Check for updates";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(167, 6);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Enabled = false;
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(170, 22);
			this.mnuAbout.Text = "About";
			// 
			// dxViewer
			// 
			this.dxViewer.BackColor = System.Drawing.Color.Black;
			this.dxViewer.Location = new System.Drawing.Point(0, 24);
			this.dxViewer.Margin = new System.Windows.Forms.Padding(0);
			this.dxViewer.Name = "dxViewer";
			this.dxViewer.Size = new System.Drawing.Size(1024, 960);
			this.dxViewer.TabIndex = 1;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(365, 272);
			this.Controls.Add(this.dxViewer);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "frmMain";
			this.Text = "Mesen";
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
		private Mesen.GUI.Controls.DXViewer dxViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuPause;
		private System.Windows.Forms.ToolStripMenuItem mnuReset;
		private System.Windows.Forms.ToolStripMenuItem mnuStop;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuLimitFPS;
		private System.Windows.Forms.ToolStripMenuItem mnuShowFPS;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuInput;
		private System.Windows.Forms.ToolStripMenuItem mnuVideoConfig;
		private System.Windows.Forms.ToolStripMenuItem mnuAudioConfig;
		private System.Windows.Forms.ToolStripMenuItem mnuTools;
		private System.Windows.Forms.ToolStripMenuItem mnuNetPlay;
		private System.Windows.Forms.ToolStripMenuItem mnuStartServer;
		private System.Windows.Forms.ToolStripMenuItem mnuStopServer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuConnect;
		private System.Windows.Forms.ToolStripMenuItem mnuDisconnect;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
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
	}
}

