using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	partial class frmPreferences
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreferences));
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.chkDeveloperMode = new System.Windows.Forms.CheckBox();
			this.lblPauseBackgroundSettings = new System.Windows.Forms.Label();
			this.chkSingleInstance = new System.Windows.Forms.CheckBox();
			this.chkAutomaticallyCheckForUpdates = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDisplayLanguage = new System.Windows.Forms.Label();
			this.cboDisplayLanguage = new System.Windows.Forms.ComboBox();
			this.lblMiscSettings = new System.Windows.Forms.Label();
			this.chkAutoLoadIps = new System.Windows.Forms.CheckBox();
			this.chkDisplayMovieIcons = new System.Windows.Forms.CheckBox();
			this.chkAutoHideMenu = new System.Windows.Forms.CheckBox();
			this.chkHidePauseOverlay = new System.Windows.Forms.CheckBox();
			this.chkAllowBackgroundInput = new System.Windows.Forms.CheckBox();
			this.chkPauseWhenInBackground = new System.Windows.Forms.CheckBox();
			this.chkPauseOnMovieEnd = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.btnOpenMesenFolder = new System.Windows.Forms.Button();
			this.btnResetSettings = new System.Windows.Forms.Button();
			this.chkConfirmExitResetPower = new System.Windows.Forms.CheckBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgShortcuts = new System.Windows.Forms.TabPage();
			this.ctrlEmulatorShortcuts = new Mesen.GUI.Forms.Config.ctrlEmulatorShortcuts();
			this.tpgSaveData = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpCloudSaves = new System.Windows.Forms.GroupBox();
			this.tlpCloudSaves = new System.Windows.Forms.TableLayoutPanel();
			this.tlpCloudSaveDesc = new System.Windows.Forms.TableLayoutPanel();
			this.lblGoogleDriveIntegration = new System.Windows.Forms.Label();
			this.btnEnableIntegration = new System.Windows.Forms.Button();
			this.tlpCloudSaveEnabled = new System.Windows.Forms.TableLayoutPanel();
			this.btnDisableIntegration = new System.Windows.Forms.Button();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.picOK = new System.Windows.Forms.PictureBox();
			this.lblIntegrationOK = new System.Windows.Forms.Label();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblLastSync = new System.Windows.Forms.Label();
			this.lblLastSyncDateTime = new System.Windows.Forms.Label();
			this.btnResync = new System.Windows.Forms.Button();
			this.grpAutomaticSaves = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.chkAutoSaveNotify = new System.Windows.Forms.CheckBox();
			this.flpAutoSave = new System.Windows.Forms.FlowLayoutPanel();
			this.chkAutoSave = new System.Windows.Forms.CheckBox();
			this.nudAutoSave = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblAutoSave = new System.Windows.Forms.Label();
			this.tpgNsf = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkNsfAutoDetectSilence = new System.Windows.Forms.CheckBox();
			this.nudNsfAutoDetectSilenceDelay = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblNsfMillisecondsOfSilence = new System.Windows.Forms.Label();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkNsfMoveToNextTrackAfterTime = new System.Windows.Forms.CheckBox();
			this.nudNsfMoveToNextTrackTime = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblNsfSeconds = new System.Windows.Forms.Label();
			this.chkNsfDisableApuIrqs = new System.Windows.Forms.CheckBox();
			this.tpgFiles = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.grpPathOverrides = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.psGame = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.chkGameOverride = new System.Windows.Forms.CheckBox();
			this.psWave = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.psMovies = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.psSaveData = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.psSaveStates = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.psScreenshots = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.psAvi = new Mesen.GUI.Forms.Config.ctrlPathSelection();
			this.chkAviOverride = new System.Windows.Forms.CheckBox();
			this.chkScreenshotsOverride = new System.Windows.Forms.CheckBox();
			this.chkSaveDataOverride = new System.Windows.Forms.CheckBox();
			this.chkWaveOverride = new System.Windows.Forms.CheckBox();
			this.chkSaveStatesOverride = new System.Windows.Forms.CheckBox();
			this.chkMoviesOverride = new System.Windows.Forms.CheckBox();
			this.grpFileAssociations = new System.Windows.Forms.GroupBox();
			this.tlpFileFormat = new System.Windows.Forms.TableLayoutPanel();
			this.chkNesFormat = new System.Windows.Forms.CheckBox();
			this.chkFdsFormat = new System.Windows.Forms.CheckBox();
			this.chkUnfFormat = new System.Windows.Forms.CheckBox();
			this.chkMmoFormat = new System.Windows.Forms.CheckBox();
			this.chkNsfFormat = new System.Windows.Forms.CheckBox();
			this.chkMstFormat = new System.Windows.Forms.CheckBox();
			this.grpDataStorageLocation = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.radStorageDocuments = new System.Windows.Forms.RadioButton();
			this.radStoragePortable = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLocation = new System.Windows.Forms.Label();
			this.lblDataLocation = new System.Windows.Forms.Label();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
			this.chkDisableGameSelectionScreen = new System.Windows.Forms.CheckBox();
			this.chkGameSelectionScreenResetGame = new System.Windows.Forms.CheckBox();
			this.chkDisableGameDatabase = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkFdsAutoLoadDisk = new System.Windows.Forms.CheckBox();
			this.chkFdsFastForwardOnLoad = new System.Windows.Forms.CheckBox();
			this.chkDisplayTitleBarInfo = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblRewind = new System.Windows.Forms.Label();
			this.nudRewindBufferSize = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblRewindMinutes = new System.Windows.Forms.Label();
			this.chkFdsAutoInsertDisk = new System.Windows.Forms.CheckBox();
			this.chkShowGameTimer = new System.Windows.Forms.CheckBox();
			this.chkShowFrameCounter = new System.Windows.Forms.CheckBox();
			this.chkShowVsConfigOnLoad = new System.Windows.Forms.CheckBox();
			this.chkDisableOsd = new System.Windows.Forms.CheckBox();
			this.lblFdsSettings = new System.Windows.Forms.Label();
			this.lblUiDisplaySettings = new System.Windows.Forms.Label();
			this.lblGameSelectionScreenSettings = new System.Windows.Forms.Label();
			this.tmrSyncDateTime = new System.Windows.Forms.Timer(this.components);
			this.chkShowFullPathInRecents = new System.Windows.Forms.CheckBox();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgShortcuts.SuspendLayout();
			this.tpgSaveData.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpCloudSaves.SuspendLayout();
			this.tlpCloudSaves.SuspendLayout();
			this.tlpCloudSaveDesc.SuspendLayout();
			this.tlpCloudSaveEnabled.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOK)).BeginInit();
			this.flowLayoutPanel4.SuspendLayout();
			this.grpAutomaticSaves.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flpAutoSave.SuspendLayout();
			this.tpgNsf.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.tpgFiles.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.grpPathOverrides.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.grpFileAssociations.SuspendLayout();
			this.tlpFileFormat.SuspendLayout();
			this.grpDataStorageLocation.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 419);
			this.baseConfigPanel.Size = new System.Drawing.Size(497, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.chkDeveloperMode, 0, 13);
			this.tlpMain.Controls.Add(this.lblPauseBackgroundSettings, 0, 3);
			this.tlpMain.Controls.Add(this.chkSingleInstance, 0, 2);
			this.tlpMain.Controls.Add(this.chkAutomaticallyCheckForUpdates, 0, 1);
			this.tlpMain.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tlpMain.Controls.Add(this.lblMiscSettings, 0, 8);
			this.tlpMain.Controls.Add(this.chkAutoLoadIps, 0, 9);
			this.tlpMain.Controls.Add(this.chkDisplayMovieIcons, 0, 12);
			this.tlpMain.Controls.Add(this.chkAutoHideMenu, 0, 10);
			this.tlpMain.Controls.Add(this.chkHidePauseOverlay, 0, 4);
			this.tlpMain.Controls.Add(this.chkAllowBackgroundInput, 0, 7);
			this.tlpMain.Controls.Add(this.chkPauseWhenInBackground, 0, 6);
			this.tlpMain.Controls.Add(this.chkPauseOnMovieEnd, 0, 5);
			this.tlpMain.Controls.Add(this.tableLayoutPanel5, 0, 15);
			this.tlpMain.Controls.Add(this.chkConfirmExitResetPower, 0, 11);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(3, 3);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 16;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(483, 387);
			this.tlpMain.TabIndex = 1;
			// 
			// chkDeveloperMode
			// 
			this.chkDeveloperMode.AutoSize = true;
			this.chkDeveloperMode.Location = new System.Drawing.Point(13, 299);
			this.chkDeveloperMode.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkDeveloperMode.Name = "chkDeveloperMode";
			this.chkDeveloperMode.Size = new System.Drawing.Size(138, 17);
			this.chkDeveloperMode.TabIndex = 26;
			this.chkDeveloperMode.Text = "Enable developer mode";
			this.chkDeveloperMode.UseVisualStyleBackColor = true;
			// 
			// lblPauseBackgroundSettings
			// 
			this.lblPauseBackgroundSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPauseBackgroundSettings.AutoSize = true;
			this.lblPauseBackgroundSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblPauseBackgroundSettings.Location = new System.Drawing.Point(0, 79);
			this.lblPauseBackgroundSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblPauseBackgroundSettings.Name = "lblPauseBackgroundSettings";
			this.lblPauseBackgroundSettings.Size = new System.Drawing.Size(141, 13);
			this.lblPauseBackgroundSettings.TabIndex = 23;
			this.lblPauseBackgroundSettings.Text = "Pause/Background Settings";
			// 
			// chkSingleInstance
			// 
			this.chkSingleInstance.AutoSize = true;
			this.chkSingleInstance.Location = new System.Drawing.Point(3, 52);
			this.chkSingleInstance.Name = "chkSingleInstance";
			this.chkSingleInstance.Size = new System.Drawing.Size(228, 17);
			this.chkSingleInstance.TabIndex = 11;
			this.chkSingleInstance.Text = "Only allow one instance of Mesen at a time";
			this.chkSingleInstance.UseVisualStyleBackColor = true;
			// 
			// chkAutomaticallyCheckForUpdates
			// 
			this.chkAutomaticallyCheckForUpdates.AutoSize = true;
			this.chkAutomaticallyCheckForUpdates.Location = new System.Drawing.Point(3, 29);
			this.chkAutomaticallyCheckForUpdates.Name = "chkAutomaticallyCheckForUpdates";
			this.chkAutomaticallyCheckForUpdates.Size = new System.Drawing.Size(177, 17);
			this.chkAutomaticallyCheckForUpdates.TabIndex = 17;
			this.chkAutomaticallyCheckForUpdates.Text = "Automatically check for updates";
			this.chkAutomaticallyCheckForUpdates.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblDisplayLanguage);
			this.flowLayoutPanel2.Controls.Add(this.cboDisplayLanguage);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(483, 26);
			this.flowLayoutPanel2.TabIndex = 18;
			// 
			// lblDisplayLanguage
			// 
			this.lblDisplayLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDisplayLanguage.AutoSize = true;
			this.lblDisplayLanguage.Location = new System.Drawing.Point(3, 7);
			this.lblDisplayLanguage.Name = "lblDisplayLanguage";
			this.lblDisplayLanguage.Size = new System.Drawing.Size(95, 13);
			this.lblDisplayLanguage.TabIndex = 0;
			this.lblDisplayLanguage.Text = "Display Language:";
			// 
			// cboDisplayLanguage
			// 
			this.cboDisplayLanguage.FormattingEnabled = true;
			this.cboDisplayLanguage.Location = new System.Drawing.Point(104, 3);
			this.cboDisplayLanguage.Name = "cboDisplayLanguage";
			this.cboDisplayLanguage.Size = new System.Drawing.Size(206, 21);
			this.cboDisplayLanguage.TabIndex = 1;
			// 
			// lblMiscSettings
			// 
			this.lblMiscSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMiscSettings.AutoSize = true;
			this.lblMiscSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblMiscSettings.Location = new System.Drawing.Point(0, 191);
			this.lblMiscSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblMiscSettings.Name = "lblMiscSettings";
			this.lblMiscSettings.Size = new System.Drawing.Size(73, 13);
			this.lblMiscSettings.TabIndex = 22;
			this.lblMiscSettings.Text = "Misc. Settings";
			// 
			// chkAutoLoadIps
			// 
			this.chkAutoLoadIps.AutoSize = true;
			this.chkAutoLoadIps.Location = new System.Drawing.Point(13, 207);
			this.chkAutoLoadIps.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkAutoLoadIps.Name = "chkAutoLoadIps";
			this.chkAutoLoadIps.Size = new System.Drawing.Size(198, 17);
			this.chkAutoLoadIps.TabIndex = 9;
			this.chkAutoLoadIps.Text = "Automatically load IPS/BPS patches";
			this.chkAutoLoadIps.UseVisualStyleBackColor = true;
			// 
			// chkDisplayMovieIcons
			// 
			this.chkDisplayMovieIcons.AutoSize = true;
			this.chkDisplayMovieIcons.Location = new System.Drawing.Point(13, 276);
			this.chkDisplayMovieIcons.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkDisplayMovieIcons.Name = "chkDisplayMovieIcons";
			this.chkDisplayMovieIcons.Size = new System.Drawing.Size(304, 17);
			this.chkDisplayMovieIcons.TabIndex = 19;
			this.chkDisplayMovieIcons.Text = "Display play/record icon when playing or recording a movie";
			this.chkDisplayMovieIcons.UseVisualStyleBackColor = true;
			// 
			// chkAutoHideMenu
			// 
			this.chkAutoHideMenu.AutoSize = true;
			this.chkAutoHideMenu.Location = new System.Drawing.Point(13, 230);
			this.chkAutoHideMenu.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkAutoHideMenu.Name = "chkAutoHideMenu";
			this.chkAutoHideMenu.Size = new System.Drawing.Size(158, 17);
			this.chkAutoHideMenu.TabIndex = 21;
			this.chkAutoHideMenu.Text = "Automatically hide menu bar";
			this.chkAutoHideMenu.UseVisualStyleBackColor = true;
			// 
			// chkHidePauseOverlay
			// 
			this.chkHidePauseOverlay.AutoSize = true;
			this.chkHidePauseOverlay.Location = new System.Drawing.Point(13, 95);
			this.chkHidePauseOverlay.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkHidePauseOverlay.Name = "chkHidePauseOverlay";
			this.chkHidePauseOverlay.Size = new System.Drawing.Size(133, 17);
			this.chkHidePauseOverlay.TabIndex = 20;
			this.chkHidePauseOverlay.Text = "Hide the pause screen";
			this.chkHidePauseOverlay.UseVisualStyleBackColor = true;
			// 
			// chkAllowBackgroundInput
			// 
			this.chkAllowBackgroundInput.AutoSize = true;
			this.chkAllowBackgroundInput.Location = new System.Drawing.Point(13, 164);
			this.chkAllowBackgroundInput.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkAllowBackgroundInput.Name = "chkAllowBackgroundInput";
			this.chkAllowBackgroundInput.Size = new System.Drawing.Size(177, 17);
			this.chkAllowBackgroundInput.TabIndex = 14;
			this.chkAllowBackgroundInput.Text = "Allow input when in background";
			this.chkAllowBackgroundInput.UseVisualStyleBackColor = true;
			// 
			// chkPauseWhenInBackground
			// 
			this.chkPauseWhenInBackground.AutoSize = true;
			this.chkPauseWhenInBackground.Location = new System.Drawing.Point(13, 141);
			this.chkPauseWhenInBackground.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkPauseWhenInBackground.Name = "chkPauseWhenInBackground";
			this.chkPauseWhenInBackground.Size = new System.Drawing.Size(204, 17);
			this.chkPauseWhenInBackground.TabIndex = 13;
			this.chkPauseWhenInBackground.Text = "Pause emulation when in background";
			this.chkPauseWhenInBackground.UseVisualStyleBackColor = true;
			this.chkPauseWhenInBackground.CheckedChanged += new System.EventHandler(this.chkPauseWhenInBackground_CheckedChanged);
			// 
			// chkPauseOnMovieEnd
			// 
			this.chkPauseOnMovieEnd.AutoSize = true;
			this.chkPauseOnMovieEnd.Location = new System.Drawing.Point(13, 118);
			this.chkPauseOnMovieEnd.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkPauseOnMovieEnd.Name = "chkPauseOnMovieEnd";
			this.chkPauseOnMovieEnd.Size = new System.Drawing.Size(199, 17);
			this.chkPauseOnMovieEnd.TabIndex = 15;
			this.chkPauseOnMovieEnd.Text = "Pause when a movie finishes playing";
			this.chkPauseOnMovieEnd.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.btnOpenMesenFolder, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.btnResetSettings, 2, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 358);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(483, 29);
			this.tableLayoutPanel5.TabIndex = 24;
			// 
			// btnOpenMesenFolder
			// 
			this.btnOpenMesenFolder.AutoSize = true;
			this.btnOpenMesenFolder.Location = new System.Drawing.Point(3, 3);
			this.btnOpenMesenFolder.Name = "btnOpenMesenFolder";
			this.btnOpenMesenFolder.Size = new System.Drawing.Size(110, 23);
			this.btnOpenMesenFolder.TabIndex = 16;
			this.btnOpenMesenFolder.Text = "Open Mesen Folder";
			this.btnOpenMesenFolder.UseVisualStyleBackColor = true;
			this.btnOpenMesenFolder.Click += new System.EventHandler(this.btnOpenMesenFolder_Click);
			// 
			// btnResetSettings
			// 
			this.btnResetSettings.AutoSize = true;
			this.btnResetSettings.Location = new System.Drawing.Point(380, 3);
			this.btnResetSettings.Name = "btnResetSettings";
			this.btnResetSettings.Size = new System.Drawing.Size(100, 23);
			this.btnResetSettings.TabIndex = 17;
			this.btnResetSettings.Text = "Reset All Settings";
			this.btnResetSettings.UseVisualStyleBackColor = true;
			this.btnResetSettings.Click += new System.EventHandler(this.btnResetSettings_Click);
			// 
			// chkConfirmExitResetPower
			// 
			this.chkConfirmExitResetPower.AutoSize = true;
			this.chkConfirmExitResetPower.Location = new System.Drawing.Point(13, 253);
			this.chkConfirmExitResetPower.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkConfirmExitResetPower.Name = "chkConfirmExitResetPower";
			this.chkConfirmExitResetPower.Size = new System.Drawing.Size(293, 17);
			this.chkConfirmExitResetPower.TabIndex = 25;
			this.chkConfirmExitResetPower.Text = "Display confirmation dialog before reset/power cycle/exit";
			this.chkConfirmExitResetPower.UseVisualStyleBackColor = true;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgShortcuts);
			this.tabMain.Controls.Add(this.tpgSaveData);
			this.tabMain.Controls.Add(this.tpgNsf);
			this.tabMain.Controls.Add(this.tpgFiles);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(497, 448);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tlpMain);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(489, 393);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgShortcuts
			// 
			this.tpgShortcuts.Controls.Add(this.ctrlEmulatorShortcuts);
			this.tpgShortcuts.Location = new System.Drawing.Point(4, 22);
			this.tpgShortcuts.Name = "tpgShortcuts";
			this.tpgShortcuts.Padding = new System.Windows.Forms.Padding(3);
			this.tpgShortcuts.Size = new System.Drawing.Size(489, 393);
			this.tpgShortcuts.TabIndex = 7;
			this.tpgShortcuts.Text = "Shortcut Keys";
			this.tpgShortcuts.UseVisualStyleBackColor = true;
			// 
			// ctrlEmulatorShortcuts
			// 
			this.ctrlEmulatorShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEmulatorShortcuts.Location = new System.Drawing.Point(3, 3);
			this.ctrlEmulatorShortcuts.Name = "ctrlEmulatorShortcuts";
			this.ctrlEmulatorShortcuts.Size = new System.Drawing.Size(483, 387);
			this.ctrlEmulatorShortcuts.TabIndex = 0;
			// 
			// tpgSaveData
			// 
			this.tpgSaveData.Controls.Add(this.tableLayoutPanel3);
			this.tpgSaveData.Location = new System.Drawing.Point(4, 22);
			this.tpgSaveData.Name = "tpgSaveData";
			this.tpgSaveData.Padding = new System.Windows.Forms.Padding(3);
			this.tpgSaveData.Size = new System.Drawing.Size(489, 393);
			this.tpgSaveData.TabIndex = 3;
			this.tpgSaveData.Text = "Save Data";
			this.tpgSaveData.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.grpCloudSaves, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpAutomaticSaves, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(483, 387);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// grpCloudSaves
			// 
			this.grpCloudSaves.Controls.Add(this.tlpCloudSaves);
			this.grpCloudSaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCloudSaves.Location = new System.Drawing.Point(3, 76);
			this.grpCloudSaves.Name = "grpCloudSaves";
			this.grpCloudSaves.Size = new System.Drawing.Size(477, 308);
			this.grpCloudSaves.TabIndex = 2;
			this.grpCloudSaves.TabStop = false;
			this.grpCloudSaves.Text = "Cloud Saves";
			// 
			// tlpCloudSaves
			// 
			this.tlpCloudSaves.ColumnCount = 1;
			this.tlpCloudSaves.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaves.Controls.Add(this.tlpCloudSaveDesc, 0, 0);
			this.tlpCloudSaves.Controls.Add(this.tlpCloudSaveEnabled, 0, 1);
			this.tlpCloudSaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpCloudSaves.Location = new System.Drawing.Point(3, 16);
			this.tlpCloudSaves.Name = "tlpCloudSaves";
			this.tlpCloudSaves.RowCount = 2;
			this.tlpCloudSaves.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaves.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaves.Size = new System.Drawing.Size(471, 289);
			this.tlpCloudSaves.TabIndex = 0;
			// 
			// tlpCloudSaveDesc
			// 
			this.tlpCloudSaveDesc.ColumnCount = 1;
			this.tlpCloudSaveDesc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaveDesc.Controls.Add(this.lblGoogleDriveIntegration, 0, 0);
			this.tlpCloudSaveDesc.Controls.Add(this.btnEnableIntegration, 0, 1);
			this.tlpCloudSaveDesc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpCloudSaveDesc.Location = new System.Drawing.Point(0, 0);
			this.tlpCloudSaveDesc.Margin = new System.Windows.Forms.Padding(0);
			this.tlpCloudSaveDesc.Name = "tlpCloudSaveDesc";
			this.tlpCloudSaveDesc.RowCount = 2;
			this.tlpCloudSaveDesc.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaveDesc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaveDesc.Size = new System.Drawing.Size(471, 100);
			this.tlpCloudSaveDesc.TabIndex = 0;
			// 
			// lblGoogleDriveIntegration
			// 
			this.lblGoogleDriveIntegration.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblGoogleDriveIntegration.Location = new System.Drawing.Point(3, 0);
			this.lblGoogleDriveIntegration.Name = "lblGoogleDriveIntegration";
			this.lblGoogleDriveIntegration.Size = new System.Drawing.Size(465, 52);
			this.lblGoogleDriveIntegration.TabIndex = 0;
			this.lblGoogleDriveIntegration.Text = resources.GetString("lblGoogleDriveIntegration.Text");
			this.lblGoogleDriveIntegration.UseWaitCursor = true;
			// 
			// btnEnableIntegration
			// 
			this.btnEnableIntegration.AutoSize = true;
			this.btnEnableIntegration.Location = new System.Drawing.Point(3, 55);
			this.btnEnableIntegration.Name = "btnEnableIntegration";
			this.btnEnableIntegration.Size = new System.Drawing.Size(172, 23);
			this.btnEnableIntegration.TabIndex = 1;
			this.btnEnableIntegration.Text = "Enable Google Drive Integration";
			this.btnEnableIntegration.UseVisualStyleBackColor = true;
			this.btnEnableIntegration.Click += new System.EventHandler(this.btnEnableIntegration_Click);
			// 
			// tlpCloudSaveEnabled
			// 
			this.tlpCloudSaveEnabled.ColumnCount = 1;
			this.tlpCloudSaveEnabled.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaveEnabled.Controls.Add(this.btnDisableIntegration, 0, 2);
			this.tlpCloudSaveEnabled.Controls.Add(this.flowLayoutPanel3, 0, 0);
			this.tlpCloudSaveEnabled.Controls.Add(this.flowLayoutPanel4, 0, 1);
			this.tlpCloudSaveEnabled.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpCloudSaveEnabled.Location = new System.Drawing.Point(0, 100);
			this.tlpCloudSaveEnabled.Margin = new System.Windows.Forms.Padding(0);
			this.tlpCloudSaveEnabled.Name = "tlpCloudSaveEnabled";
			this.tlpCloudSaveEnabled.RowCount = 4;
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaveEnabled.Size = new System.Drawing.Size(471, 237);
			this.tlpCloudSaveEnabled.TabIndex = 1;
			// 
			// btnDisableIntegration
			// 
			this.btnDisableIntegration.Location = new System.Drawing.Point(3, 53);
			this.btnDisableIntegration.Name = "btnDisableIntegration";
			this.btnDisableIntegration.Size = new System.Drawing.Size(172, 23);
			this.btnDisableIntegration.TabIndex = 2;
			this.btnDisableIntegration.Text = "Disable Google Drive Integration";
			this.btnDisableIntegration.UseVisualStyleBackColor = true;
			this.btnDisableIntegration.Click += new System.EventHandler(this.btnDisableIntegration_Click);
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.picOK);
			this.flowLayoutPanel3.Controls.Add(this.lblIntegrationOK);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(471, 22);
			this.flowLayoutPanel3.TabIndex = 1;
			// 
			// picOK
			// 
			this.picOK.Image = global::Mesen.GUI.Properties.Resources.Accept;
			this.picOK.Location = new System.Drawing.Point(3, 3);
			this.picOK.Name = "picOK";
			this.picOK.Size = new System.Drawing.Size(16, 16);
			this.picOK.TabIndex = 0;
			this.picOK.TabStop = false;
			// 
			// lblIntegrationOK
			// 
			this.lblIntegrationOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblIntegrationOK.AutoSize = true;
			this.lblIntegrationOK.Location = new System.Drawing.Point(25, 4);
			this.lblIntegrationOK.Name = "lblIntegrationOK";
			this.lblIntegrationOK.Size = new System.Drawing.Size(166, 13);
			this.lblIntegrationOK.TabIndex = 1;
			this.lblIntegrationOK.Text = "Google Drive integration is active.";
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.lblLastSync);
			this.flowLayoutPanel4.Controls.Add(this.lblLastSyncDateTime);
			this.flowLayoutPanel4.Controls.Add(this.btnResync);
			this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 22);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(444, 28);
			this.flowLayoutPanel4.TabIndex = 3;
			// 
			// lblLastSync
			// 
			this.lblLastSync.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLastSync.AutoSize = true;
			this.lblLastSync.Location = new System.Drawing.Point(3, 9);
			this.lblLastSync.Name = "lblLastSync";
			this.lblLastSync.Size = new System.Drawing.Size(57, 13);
			this.lblLastSync.TabIndex = 0;
			this.lblLastSync.Text = "Last Sync:";
			// 
			// lblLastSyncDateTime
			// 
			this.lblLastSyncDateTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLastSyncDateTime.AutoSize = true;
			this.lblLastSyncDateTime.Location = new System.Drawing.Point(66, 9);
			this.lblLastSyncDateTime.Name = "lblLastSyncDateTime";
			this.lblLastSyncDateTime.Size = new System.Drawing.Size(114, 13);
			this.lblLastSyncDateTime.TabIndex = 1;
			this.lblLastSyncDateTime.Text = "9999/01/01 12:00 PM";
			// 
			// btnResync
			// 
			this.btnResync.AutoSize = true;
			this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnResync.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.btnResync.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnResync.Location = new System.Drawing.Point(186, 3);
			this.btnResync.MinimumSize = new System.Drawing.Size(0, 25);
			this.btnResync.Name = "btnResync";
			this.btnResync.Size = new System.Drawing.Size(69, 25);
			this.btnResync.TabIndex = 4;
			this.btnResync.Text = "Resync";
			this.btnResync.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnResync.UseVisualStyleBackColor = true;
			this.btnResync.Click += new System.EventHandler(this.btnResync_Click);
			// 
			// grpAutomaticSaves
			// 
			this.grpAutomaticSaves.Controls.Add(this.tableLayoutPanel4);
			this.grpAutomaticSaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpAutomaticSaves.Location = new System.Drawing.Point(3, 3);
			this.grpAutomaticSaves.Name = "grpAutomaticSaves";
			this.grpAutomaticSaves.Size = new System.Drawing.Size(477, 67);
			this.grpAutomaticSaves.TabIndex = 3;
			this.grpAutomaticSaves.TabStop = false;
			this.grpAutomaticSaves.Text = "Automatic Save States";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.chkAutoSaveNotify, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.flpAutoSave, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 3;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(471, 48);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// chkAutoSaveNotify
			// 
			this.chkAutoSaveNotify.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAutoSaveNotify.AutoSize = true;
			this.chkAutoSaveNotify.Location = new System.Drawing.Point(15, 27);
			this.chkAutoSaveNotify.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
			this.chkAutoSaveNotify.Name = "chkAutoSaveNotify";
			this.chkAutoSaveNotify.Size = new System.Drawing.Size(240, 17);
			this.chkAutoSaveNotify.TabIndex = 1;
			this.chkAutoSaveNotify.Text = "Notify when an automatic save state is saved";
			this.chkAutoSaveNotify.UseVisualStyleBackColor = true;
			// 
			// flpAutoSave
			// 
			this.flpAutoSave.Controls.Add(this.chkAutoSave);
			this.flpAutoSave.Controls.Add(this.nudAutoSave);
			this.flpAutoSave.Controls.Add(this.lblAutoSave);
			this.flpAutoSave.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpAutoSave.Location = new System.Drawing.Point(0, 0);
			this.flpAutoSave.Margin = new System.Windows.Forms.Padding(0);
			this.flpAutoSave.Name = "flpAutoSave";
			this.flpAutoSave.Size = new System.Drawing.Size(471, 23);
			this.flpAutoSave.TabIndex = 0;
			// 
			// chkAutoSave
			// 
			this.chkAutoSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAutoSave.AutoSize = true;
			this.chkAutoSave.Location = new System.Drawing.Point(3, 3);
			this.chkAutoSave.Name = "chkAutoSave";
			this.chkAutoSave.Size = new System.Drawing.Size(211, 17);
			this.chkAutoSave.TabIndex = 0;
			this.chkAutoSave.Text = "Automatically create a save state every";
			this.chkAutoSave.UseVisualStyleBackColor = true;
			this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);
			// 
			// nudAutoSave
			// 
			this.nudAutoSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudAutoSave.DecimalPlaces = 0;
			this.nudAutoSave.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAutoSave.Location = new System.Drawing.Point(217, 1);
			this.nudAutoSave.Margin = new System.Windows.Forms.Padding(0);
			this.nudAutoSave.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.nudAutoSave.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudAutoSave.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAutoSave.Name = "nudAutoSave";
			this.nudAutoSave.Size = new System.Drawing.Size(42, 20);
			this.nudAutoSave.TabIndex = 1;
			this.nudAutoSave.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// lblAutoSave
			// 
			this.lblAutoSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAutoSave.AutoSize = true;
			this.lblAutoSave.Location = new System.Drawing.Point(262, 5);
			this.lblAutoSave.Name = "lblAutoSave";
			this.lblAutoSave.Size = new System.Drawing.Size(99, 13);
			this.lblAutoSave.TabIndex = 2;
			this.lblAutoSave.Text = "minutes (F8 to load)";
			// 
			// tpgNsf
			// 
			this.tpgNsf.Controls.Add(this.tableLayoutPanel2);
			this.tpgNsf.Location = new System.Drawing.Point(4, 22);
			this.tpgNsf.Name = "tpgNsf";
			this.tpgNsf.Padding = new System.Windows.Forms.Padding(3);
			this.tpgNsf.Size = new System.Drawing.Size(489, 393);
			this.tpgNsf.TabIndex = 4;
			this.tpgNsf.Text = "NSF";
			this.tpgNsf.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel7, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel5, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkNsfDisableApuIrqs, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(483, 387);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.Controls.Add(this.chkNsfAutoDetectSilence);
			this.flowLayoutPanel7.Controls.Add(this.nudNsfAutoDetectSilenceDelay);
			this.flowLayoutPanel7.Controls.Add(this.lblNsfMillisecondsOfSilence);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(483, 24);
			this.flowLayoutPanel7.TabIndex = 5;
			// 
			// chkNsfAutoDetectSilence
			// 
			this.chkNsfAutoDetectSilence.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkNsfAutoDetectSilence.AutoSize = true;
			this.chkNsfAutoDetectSilence.Location = new System.Drawing.Point(3, 3);
			this.chkNsfAutoDetectSilence.Name = "chkNsfAutoDetectSilence";
			this.chkNsfAutoDetectSilence.Size = new System.Drawing.Size(139, 17);
			this.chkNsfAutoDetectSilence.TabIndex = 1;
			this.chkNsfAutoDetectSilence.Text = "Move to next track after";
			this.chkNsfAutoDetectSilence.UseVisualStyleBackColor = true;
			// 
			// nudNsfAutoDetectSilenceDelay
			// 
			this.nudNsfAutoDetectSilenceDelay.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudNsfAutoDetectSilenceDelay.DecimalPlaces = 0;
			this.nudNsfAutoDetectSilenceDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudNsfAutoDetectSilenceDelay.Location = new System.Drawing.Point(145, 1);
			this.nudNsfAutoDetectSilenceDelay.Margin = new System.Windows.Forms.Padding(0);
			this.nudNsfAutoDetectSilenceDelay.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.nudNsfAutoDetectSilenceDelay.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudNsfAutoDetectSilenceDelay.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.nudNsfAutoDetectSilenceDelay.Name = "nudNsfAutoDetectSilenceDelay";
			this.nudNsfAutoDetectSilenceDelay.Size = new System.Drawing.Size(57, 20);
			this.nudNsfAutoDetectSilenceDelay.TabIndex = 3;
			this.nudNsfAutoDetectSilenceDelay.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			// 
			// lblNsfMillisecondsOfSilence
			// 
			this.lblNsfMillisecondsOfSilence.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNsfMillisecondsOfSilence.AutoSize = true;
			this.lblNsfMillisecondsOfSilence.Location = new System.Drawing.Point(202, 5);
			this.lblNsfMillisecondsOfSilence.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblNsfMillisecondsOfSilence.Name = "lblNsfMillisecondsOfSilence";
			this.lblNsfMillisecondsOfSilence.Size = new System.Drawing.Size(111, 13);
			this.lblNsfMillisecondsOfSilence.TabIndex = 4;
			this.lblNsfMillisecondsOfSilence.Text = "milliseconds of silence";
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Controls.Add(this.chkNsfMoveToNextTrackAfterTime);
			this.flowLayoutPanel5.Controls.Add(this.nudNsfMoveToNextTrackTime);
			this.flowLayoutPanel5.Controls.Add(this.lblNsfSeconds);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(0, 24);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(483, 24);
			this.flowLayoutPanel5.TabIndex = 4;
			// 
			// chkNsfMoveToNextTrackAfterTime
			// 
			this.chkNsfMoveToNextTrackAfterTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkNsfMoveToNextTrackAfterTime.AutoSize = true;
			this.chkNsfMoveToNextTrackAfterTime.Location = new System.Drawing.Point(3, 3);
			this.chkNsfMoveToNextTrackAfterTime.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.chkNsfMoveToNextTrackAfterTime.Name = "chkNsfMoveToNextTrackAfterTime";
			this.chkNsfMoveToNextTrackAfterTime.Size = new System.Drawing.Size(126, 17);
			this.chkNsfMoveToNextTrackAfterTime.TabIndex = 2;
			this.chkNsfMoveToNextTrackAfterTime.Text = "Limit track run time to";
			this.chkNsfMoveToNextTrackAfterTime.UseVisualStyleBackColor = true;
			// 
			// nudNsfMoveToNextTrackTime
			// 
			this.nudNsfMoveToNextTrackTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudNsfMoveToNextTrackTime.DecimalPlaces = 0;
			this.nudNsfMoveToNextTrackTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudNsfMoveToNextTrackTime.Location = new System.Drawing.Point(129, 1);
			this.nudNsfMoveToNextTrackTime.Margin = new System.Windows.Forms.Padding(0);
			this.nudNsfMoveToNextTrackTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.nudNsfMoveToNextTrackTime.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudNsfMoveToNextTrackTime.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudNsfMoveToNextTrackTime.Name = "nudNsfMoveToNextTrackTime";
			this.nudNsfMoveToNextTrackTime.Size = new System.Drawing.Size(44, 20);
			this.nudNsfMoveToNextTrackTime.TabIndex = 3;
			this.nudNsfMoveToNextTrackTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// lblNsfSeconds
			// 
			this.lblNsfSeconds.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNsfSeconds.AutoSize = true;
			this.lblNsfSeconds.Location = new System.Drawing.Point(173, 5);
			this.lblNsfSeconds.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblNsfSeconds.Name = "lblNsfSeconds";
			this.lblNsfSeconds.Size = new System.Drawing.Size(47, 13);
			this.lblNsfSeconds.TabIndex = 4;
			this.lblNsfSeconds.Text = "seconds";
			// 
			// chkNsfDisableApuIrqs
			// 
			this.chkNsfDisableApuIrqs.AutoSize = true;
			this.chkNsfDisableApuIrqs.Location = new System.Drawing.Point(3, 51);
			this.chkNsfDisableApuIrqs.Name = "chkNsfDisableApuIrqs";
			this.chkNsfDisableApuIrqs.Size = new System.Drawing.Size(194, 17);
			this.chkNsfDisableApuIrqs.TabIndex = 6;
			this.chkNsfDisableApuIrqs.Text = "Disable APU IRQs (Recommended)";
			this.chkNsfDisableApuIrqs.UseVisualStyleBackColor = true;
			// 
			// tpgFiles
			// 
			this.tpgFiles.Controls.Add(this.tableLayoutPanel6);
			this.tpgFiles.Location = new System.Drawing.Point(4, 22);
			this.tpgFiles.Name = "tpgFiles";
			this.tpgFiles.Padding = new System.Windows.Forms.Padding(3);
			this.tpgFiles.Size = new System.Drawing.Size(489, 393);
			this.tpgFiles.TabIndex = 2;
			this.tpgFiles.Text = "Folders/Files";
			this.tpgFiles.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add(this.grpPathOverrides, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.grpFileAssociations, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.grpDataStorageLocation, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 4;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(483, 387);
			this.tableLayoutPanel6.TabIndex = 13;
			// 
			// grpPathOverrides
			// 
			this.grpPathOverrides.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpPathOverrides.Controls.Add(this.tableLayoutPanel10);
			this.grpPathOverrides.Location = new System.Drawing.Point(3, 198);
			this.grpPathOverrides.Name = "grpPathOverrides";
			this.grpPathOverrides.Size = new System.Drawing.Size(477, 186);
			this.grpPathOverrides.TabIndex = 15;
			this.grpPathOverrides.TabStop = false;
			this.grpPathOverrides.Text = "Folder Overrides";
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 2;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Controls.Add(this.psGame, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.chkGameOverride, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.psWave, 1, 2);
			this.tableLayoutPanel10.Controls.Add(this.psMovies, 1, 3);
			this.tableLayoutPanel10.Controls.Add(this.psSaveData, 1, 4);
			this.tableLayoutPanel10.Controls.Add(this.psSaveStates, 1, 5);
			this.tableLayoutPanel10.Controls.Add(this.psScreenshots, 1, 6);
			this.tableLayoutPanel10.Controls.Add(this.psAvi, 1, 7);
			this.tableLayoutPanel10.Controls.Add(this.chkAviOverride, 0, 7);
			this.tableLayoutPanel10.Controls.Add(this.chkScreenshotsOverride, 0, 6);
			this.tableLayoutPanel10.Controls.Add(this.chkSaveDataOverride, 0, 4);
			this.tableLayoutPanel10.Controls.Add(this.chkWaveOverride, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.chkSaveStatesOverride, 0, 5);
			this.tableLayoutPanel10.Controls.Add(this.chkMoviesOverride, 0, 3);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 9;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(471, 167);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// psGame
			// 
			this.psGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psGame.DisabledText = "";
			this.psGame.Location = new System.Drawing.Point(94, 0);
			this.psGame.Margin = new System.Windows.Forms.Padding(0);
			this.psGame.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psGame.MinimumSize = new System.Drawing.Size(0, 20);
			this.psGame.Name = "psGame";
			this.psGame.Size = new System.Drawing.Size(377, 20);
			this.psGame.TabIndex = 13;
			// 
			// chkGameOverride
			// 
			this.chkGameOverride.AutoSize = true;
			this.chkGameOverride.Location = new System.Drawing.Point(3, 3);
			this.chkGameOverride.Name = "chkGameOverride";
			this.chkGameOverride.Size = new System.Drawing.Size(62, 17);
			this.chkGameOverride.TabIndex = 12;
			this.chkGameOverride.Text = "Games:";
			this.chkGameOverride.UseVisualStyleBackColor = true;
			this.chkGameOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// psWave
			// 
			this.psWave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psWave.DisabledText = "";
			this.psWave.Location = new System.Drawing.Point(94, 28);
			this.psWave.Margin = new System.Windows.Forms.Padding(0);
			this.psWave.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psWave.MinimumSize = new System.Drawing.Size(0, 20);
			this.psWave.Name = "psWave";
			this.psWave.Size = new System.Drawing.Size(377, 20);
			this.psWave.TabIndex = 6;
			// 
			// psMovies
			// 
			this.psMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psMovies.DisabledText = "";
			this.psMovies.Location = new System.Drawing.Point(94, 51);
			this.psMovies.Margin = new System.Windows.Forms.Padding(0);
			this.psMovies.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psMovies.MinimumSize = new System.Drawing.Size(0, 20);
			this.psMovies.Name = "psMovies";
			this.psMovies.Size = new System.Drawing.Size(377, 20);
			this.psMovies.TabIndex = 7;
			// 
			// psSaveData
			// 
			this.psSaveData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psSaveData.DisabledText = "";
			this.psSaveData.Location = new System.Drawing.Point(94, 74);
			this.psSaveData.Margin = new System.Windows.Forms.Padding(0);
			this.psSaveData.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psSaveData.MinimumSize = new System.Drawing.Size(0, 20);
			this.psSaveData.Name = "psSaveData";
			this.psSaveData.Size = new System.Drawing.Size(377, 20);
			this.psSaveData.TabIndex = 8;
			// 
			// psSaveStates
			// 
			this.psSaveStates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psSaveStates.DisabledText = "";
			this.psSaveStates.Location = new System.Drawing.Point(94, 97);
			this.psSaveStates.Margin = new System.Windows.Forms.Padding(0);
			this.psSaveStates.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psSaveStates.MinimumSize = new System.Drawing.Size(0, 20);
			this.psSaveStates.Name = "psSaveStates";
			this.psSaveStates.Size = new System.Drawing.Size(377, 20);
			this.psSaveStates.TabIndex = 9;
			// 
			// psScreenshots
			// 
			this.psScreenshots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psScreenshots.DisabledText = "";
			this.psScreenshots.Location = new System.Drawing.Point(94, 120);
			this.psScreenshots.Margin = new System.Windows.Forms.Padding(0);
			this.psScreenshots.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psScreenshots.MinimumSize = new System.Drawing.Size(0, 20);
			this.psScreenshots.Name = "psScreenshots";
			this.psScreenshots.Size = new System.Drawing.Size(377, 20);
			this.psScreenshots.TabIndex = 10;
			// 
			// psAvi
			// 
			this.psAvi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.psAvi.DisabledText = "";
			this.psAvi.Location = new System.Drawing.Point(94, 143);
			this.psAvi.Margin = new System.Windows.Forms.Padding(0);
			this.psAvi.MaximumSize = new System.Drawing.Size(1000, 20);
			this.psAvi.MinimumSize = new System.Drawing.Size(0, 20);
			this.psAvi.Name = "psAvi";
			this.psAvi.Size = new System.Drawing.Size(377, 20);
			this.psAvi.TabIndex = 11;
			// 
			// chkAviOverride
			// 
			this.chkAviOverride.AutoSize = true;
			this.chkAviOverride.Location = new System.Drawing.Point(3, 146);
			this.chkAviOverride.Name = "chkAviOverride";
			this.chkAviOverride.Size = new System.Drawing.Size(61, 17);
			this.chkAviOverride.TabIndex = 4;
			this.chkAviOverride.Text = "Videos:";
			this.chkAviOverride.UseVisualStyleBackColor = true;
			this.chkAviOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkScreenshotsOverride
			// 
			this.chkScreenshotsOverride.AutoSize = true;
			this.chkScreenshotsOverride.Location = new System.Drawing.Point(3, 123);
			this.chkScreenshotsOverride.Name = "chkScreenshotsOverride";
			this.chkScreenshotsOverride.Size = new System.Drawing.Size(88, 17);
			this.chkScreenshotsOverride.TabIndex = 3;
			this.chkScreenshotsOverride.Text = "Screenshots:";
			this.chkScreenshotsOverride.UseVisualStyleBackColor = true;
			this.chkScreenshotsOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkSaveDataOverride
			// 
			this.chkSaveDataOverride.AutoSize = true;
			this.chkSaveDataOverride.Location = new System.Drawing.Point(3, 77);
			this.chkSaveDataOverride.Name = "chkSaveDataOverride";
			this.chkSaveDataOverride.Size = new System.Drawing.Size(80, 17);
			this.chkSaveDataOverride.TabIndex = 0;
			this.chkSaveDataOverride.Text = "Save Data:";
			this.chkSaveDataOverride.UseVisualStyleBackColor = true;
			this.chkSaveDataOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkWaveOverride
			// 
			this.chkWaveOverride.AutoSize = true;
			this.chkWaveOverride.Location = new System.Drawing.Point(3, 31);
			this.chkWaveOverride.Name = "chkWaveOverride";
			this.chkWaveOverride.Size = new System.Drawing.Size(56, 17);
			this.chkWaveOverride.TabIndex = 5;
			this.chkWaveOverride.Text = "Audio:";
			this.chkWaveOverride.UseVisualStyleBackColor = true;
			this.chkWaveOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkSaveStatesOverride
			// 
			this.chkSaveStatesOverride.AutoSize = true;
			this.chkSaveStatesOverride.Location = new System.Drawing.Point(3, 100);
			this.chkSaveStatesOverride.Name = "chkSaveStatesOverride";
			this.chkSaveStatesOverride.Size = new System.Drawing.Size(87, 17);
			this.chkSaveStatesOverride.TabIndex = 2;
			this.chkSaveStatesOverride.Text = "Save States:";
			this.chkSaveStatesOverride.UseVisualStyleBackColor = true;
			this.chkSaveStatesOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkMoviesOverride
			// 
			this.chkMoviesOverride.AutoSize = true;
			this.chkMoviesOverride.Location = new System.Drawing.Point(3, 54);
			this.chkMoviesOverride.Name = "chkMoviesOverride";
			this.chkMoviesOverride.Size = new System.Drawing.Size(63, 17);
			this.chkMoviesOverride.TabIndex = 1;
			this.chkMoviesOverride.Text = "Movies:";
			this.chkMoviesOverride.UseVisualStyleBackColor = true;
			this.chkMoviesOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// grpFileAssociations
			// 
			this.grpFileAssociations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpFileAssociations.Controls.Add(this.tlpFileFormat);
			this.grpFileAssociations.Location = new System.Drawing.Point(3, 3);
			this.grpFileAssociations.Name = "grpFileAssociations";
			this.grpFileAssociations.Size = new System.Drawing.Size(477, 86);
			this.grpFileAssociations.TabIndex = 12;
			this.grpFileAssociations.TabStop = false;
			this.grpFileAssociations.Text = "File Associations";
			// 
			// tlpFileFormat
			// 
			this.tlpFileFormat.ColumnCount = 2;
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.Controls.Add(this.chkNesFormat, 0, 0);
			this.tlpFileFormat.Controls.Add(this.chkFdsFormat, 0, 1);
			this.tlpFileFormat.Controls.Add(this.chkUnfFormat, 0, 2);
			this.tlpFileFormat.Controls.Add(this.chkMmoFormat, 1, 2);
			this.tlpFileFormat.Controls.Add(this.chkNsfFormat, 1, 0);
			this.tlpFileFormat.Controls.Add(this.chkMstFormat, 1, 1);
			this.tlpFileFormat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpFileFormat.Location = new System.Drawing.Point(3, 16);
			this.tlpFileFormat.Name = "tlpFileFormat";
			this.tlpFileFormat.RowCount = 5;
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.Size = new System.Drawing.Size(471, 67);
			this.tlpFileFormat.TabIndex = 0;
			// 
			// chkNesFormat
			// 
			this.chkNesFormat.AutoSize = true;
			this.chkNesFormat.Location = new System.Drawing.Point(3, 3);
			this.chkNesFormat.Name = "chkNesFormat";
			this.chkNesFormat.Size = new System.Drawing.Size(51, 17);
			this.chkNesFormat.TabIndex = 10;
			this.chkNesFormat.Text = ".NES";
			this.chkNesFormat.UseVisualStyleBackColor = true;
			// 
			// chkFdsFormat
			// 
			this.chkFdsFormat.AutoSize = true;
			this.chkFdsFormat.Location = new System.Drawing.Point(3, 26);
			this.chkFdsFormat.Name = "chkFdsFormat";
			this.chkFdsFormat.Size = new System.Drawing.Size(162, 17);
			this.chkFdsFormat.TabIndex = 12;
			this.chkFdsFormat.Text = ".FDS (Famicom Disk System)";
			this.chkFdsFormat.UseVisualStyleBackColor = true;
			// 
			// chkUnfFormat
			// 
			this.chkUnfFormat.AutoSize = true;
			this.chkUnfFormat.Location = new System.Drawing.Point(3, 49);
			this.chkUnfFormat.Name = "chkUnfFormat";
			this.chkUnfFormat.Size = new System.Drawing.Size(85, 17);
			this.chkUnfFormat.TabIndex = 16;
			this.chkUnfFormat.Text = ".UNF (UNIF)";
			this.chkUnfFormat.UseVisualStyleBackColor = true;
			// 
			// chkMmoFormat
			// 
			this.chkMmoFormat.AutoSize = true;
			this.chkMmoFormat.Location = new System.Drawing.Point(238, 49);
			this.chkMmoFormat.Name = "chkMmoFormat";
			this.chkMmoFormat.Size = new System.Drawing.Size(133, 17);
			this.chkMmoFormat.TabIndex = 11;
			this.chkMmoFormat.Text = ".MMO (Mesen Movies)";
			this.chkMmoFormat.UseVisualStyleBackColor = true;
			// 
			// chkNsfFormat
			// 
			this.chkNsfFormat.AutoSize = true;
			this.chkNsfFormat.Location = new System.Drawing.Point(238, 3);
			this.chkNsfFormat.Name = "chkNsfFormat";
			this.chkNsfFormat.Size = new System.Drawing.Size(207, 17);
			this.chkNsfFormat.TabIndex = 14;
			this.chkNsfFormat.Text = ".NSF/.NSFE (Nintendo Sound Format)";
			this.chkNsfFormat.UseVisualStyleBackColor = true;
			// 
			// chkMstFormat
			// 
			this.chkMstFormat.AutoSize = true;
			this.chkMstFormat.Location = new System.Drawing.Point(238, 26);
			this.chkMstFormat.Name = "chkMstFormat";
			this.chkMstFormat.Size = new System.Drawing.Size(149, 17);
			this.chkMstFormat.TabIndex = 15;
			this.chkMstFormat.Text = ".MST (Mesen Save State)";
			this.chkMstFormat.UseVisualStyleBackColor = true;
			// 
			// grpDataStorageLocation
			// 
			this.grpDataStorageLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDataStorageLocation.Controls.Add(this.tableLayoutPanel7);
			this.grpDataStorageLocation.Location = new System.Drawing.Point(3, 95);
			this.grpDataStorageLocation.Name = "grpDataStorageLocation";
			this.grpDataStorageLocation.Size = new System.Drawing.Size(477, 97);
			this.grpDataStorageLocation.TabIndex = 14;
			this.grpDataStorageLocation.TabStop = false;
			this.grpDataStorageLocation.Text = "Data Storage Location";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.ColumnCount = 1;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel9, 0, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 3;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(471, 78);
			this.tableLayoutPanel7.TabIndex = 13;
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 1;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.radStorageDocuments, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.radStoragePortable, 0, 1);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 2;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(465, 47);
			this.tableLayoutPanel8.TabIndex = 0;
			// 
			// radStorageDocuments
			// 
			this.radStorageDocuments.AutoSize = true;
			this.radStorageDocuments.Checked = true;
			this.radStorageDocuments.Cursor = System.Windows.Forms.Cursors.Default;
			this.radStorageDocuments.Location = new System.Drawing.Point(3, 3);
			this.radStorageDocuments.Name = "radStorageDocuments";
			this.radStorageDocuments.Size = new System.Drawing.Size(197, 17);
			this.radStorageDocuments.TabIndex = 0;
			this.radStorageDocuments.TabStop = true;
			this.radStorageDocuments.Text = "Store Mesen\'s data in my user profile";
			this.radStorageDocuments.UseVisualStyleBackColor = true;
			this.radStorageDocuments.CheckedChanged += new System.EventHandler(this.radStorageDocuments_CheckedChanged);
			// 
			// radStoragePortable
			// 
			this.radStoragePortable.AutoSize = true;
			this.radStoragePortable.Cursor = System.Windows.Forms.Cursors.Default;
			this.radStoragePortable.Location = new System.Drawing.Point(3, 26);
			this.radStoragePortable.Name = "radStoragePortable";
			this.radStoragePortable.Size = new System.Drawing.Size(288, 17);
			this.radStoragePortable.TabIndex = 1;
			this.radStoragePortable.Text = "Store Mesen\'s data in the same folder as the application";
			this.radStoragePortable.UseVisualStyleBackColor = true;
			this.radStoragePortable.CheckedChanged += new System.EventHandler(this.radStorageDocuments_CheckedChanged);
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.ColumnCount = 2;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Controls.Add(this.lblLocation, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.lblDataLocation, 0, 0);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 56);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.tableLayoutPanel9.RowCount = 1;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(465, 18);
			this.tableLayoutPanel9.TabIndex = 39;
			// 
			// lblLocation
			// 
			this.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLocation.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblLocation.Location = new System.Drawing.Point(48, 5);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(414, 13);
			this.lblLocation.TabIndex = 1;
			this.lblLocation.Text = "....";
			// 
			// lblDataLocation
			// 
			this.lblDataLocation.AutoSize = true;
			this.lblDataLocation.Location = new System.Drawing.Point(3, 5);
			this.lblDataLocation.Name = "lblDataLocation";
			this.lblDataLocation.Size = new System.Drawing.Size(39, 13);
			this.lblDataLocation.TabIndex = 0;
			this.lblDataLocation.Text = "Folder:";
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel1);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(489, 422);
			this.tpgAdvanced.TabIndex = 1;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkShowFullPathInRecents, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.chkAlwaysOnTop, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.chkDisableGameSelectionScreen, 0, 15);
			this.tableLayoutPanel1.Controls.Add(this.chkGameSelectionScreenResetGame, 0, 14);
			this.tableLayoutPanel1.Controls.Add(this.chkDisableGameDatabase, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsAutoLoadDisk, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsFastForwardOnLoad, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.chkDisplayTitleBarInfo, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel6, 0, 16);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsAutoInsertDisk, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.chkShowGameTimer, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.chkShowFrameCounter, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.chkShowVsConfigOnLoad, 0, 12);
			this.tableLayoutPanel1.Controls.Add(this.chkDisableOsd, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.lblFdsSettings, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblUiDisplaySettings, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblGameSelectionScreenSettings, 0, 13);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 18;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(483, 416);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkAlwaysOnTop
			// 
			this.chkAlwaysOnTop.AutoSize = true;
			this.chkAlwaysOnTop.Location = new System.Drawing.Point(13, 135);
			this.chkAlwaysOnTop.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
			this.chkAlwaysOnTop.Size = new System.Drawing.Size(210, 17);
			this.chkAlwaysOnTop.TabIndex = 29;
			this.chkAlwaysOnTop.Text = "Always display on top of other windows";
			this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
			// 
			// chkDisableGameSelectionScreen
			// 
			this.chkDisableGameSelectionScreen.AutoSize = true;
			this.chkDisableGameSelectionScreen.Location = new System.Drawing.Point(13, 339);
			this.chkDisableGameSelectionScreen.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkDisableGameSelectionScreen.Name = "chkDisableGameSelectionScreen";
			this.chkDisableGameSelectionScreen.Size = new System.Drawing.Size(170, 17);
			this.chkDisableGameSelectionScreen.TabIndex = 28;
			this.chkDisableGameSelectionScreen.Text = "Disable game selection screen";
			this.chkDisableGameSelectionScreen.UseVisualStyleBackColor = true;
			// 
			// chkGameSelectionScreenResetGame
			// 
			this.chkGameSelectionScreenResetGame.AutoSize = true;
			this.chkGameSelectionScreenResetGame.Location = new System.Drawing.Point(13, 316);
			this.chkGameSelectionScreenResetGame.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkGameSelectionScreenResetGame.Name = "chkGameSelectionScreenResetGame";
			this.chkGameSelectionScreenResetGame.Size = new System.Drawing.Size(388, 17);
			this.chkGameSelectionScreenResetGame.TabIndex = 27;
			this.chkGameSelectionScreenResetGame.Text = "Start game from power-on instead of resuming the previous gameplay session";
			this.chkGameSelectionScreenResetGame.UseVisualStyleBackColor = true;
			// 
			// chkDisableGameDatabase
			// 
			this.chkDisableGameDatabase.Checked = false;
			this.chkDisableGameDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisableGameDatabase.Location = new System.Drawing.Point(0, 0);
			this.chkDisableGameDatabase.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisableGameDatabase.Name = "chkDisableGameDatabase";
			this.chkDisableGameDatabase.Size = new System.Drawing.Size(483, 23);
			this.chkDisableGameDatabase.TabIndex = 6;
			this.chkDisableGameDatabase.Text = "Disable built-in game database";
			// 
			// chkFdsAutoLoadDisk
			// 
			this.chkFdsAutoLoadDisk.AutoSize = true;
			this.chkFdsAutoLoadDisk.Location = new System.Drawing.Point(13, 46);
			this.chkFdsAutoLoadDisk.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkFdsAutoLoadDisk.Name = "chkFdsAutoLoadDisk";
			this.chkFdsAutoLoadDisk.Size = new System.Drawing.Size(303, 17);
			this.chkFdsAutoLoadDisk.TabIndex = 3;
			this.chkFdsAutoLoadDisk.Text = "Automatically insert disk 1 side A when starting FDS games";
			this.chkFdsAutoLoadDisk.UseVisualStyleBackColor = true;
			// 
			// chkFdsFastForwardOnLoad
			// 
			this.chkFdsFastForwardOnLoad.AutoSize = true;
			this.chkFdsFastForwardOnLoad.Location = new System.Drawing.Point(13, 69);
			this.chkFdsFastForwardOnLoad.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkFdsFastForwardOnLoad.Name = "chkFdsFastForwardOnLoad";
			this.chkFdsFastForwardOnLoad.Size = new System.Drawing.Size(342, 17);
			this.chkFdsFastForwardOnLoad.TabIndex = 4;
			this.chkFdsFastForwardOnLoad.Text = "Automatically fast forward FDS games when disk or BIOS is loading";
			this.chkFdsFastForwardOnLoad.UseVisualStyleBackColor = true;
			// 
			// chkDisplayTitleBarInfo
			// 
			this.chkDisplayTitleBarInfo.AutoSize = true;
			this.chkDisplayTitleBarInfo.Location = new System.Drawing.Point(13, 181);
			this.chkDisplayTitleBarInfo.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkDisplayTitleBarInfo.Name = "chkDisplayTitleBarInfo";
			this.chkDisplayTitleBarInfo.Size = new System.Drawing.Size(210, 17);
			this.chkDisplayTitleBarInfo.TabIndex = 8;
			this.chkDisplayTitleBarInfo.Text = "Display additional information in title bar";
			this.chkDisplayTitleBarInfo.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.Controls.Add(this.lblRewind);
			this.flowLayoutPanel6.Controls.Add(this.nudRewindBufferSize);
			this.flowLayoutPanel6.Controls.Add(this.lblRewindMinutes);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 362);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(483, 23);
			this.flowLayoutPanel6.TabIndex = 9;
			// 
			// lblRewind
			// 
			this.lblRewind.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRewind.AutoSize = true;
			this.lblRewind.Location = new System.Drawing.Point(3, 3);
			this.lblRewind.Name = "lblRewind";
			this.lblRewind.Size = new System.Drawing.Size(142, 13);
			this.lblRewind.TabIndex = 3;
			this.lblRewind.Text = "Keep rewind data for the last";
			// 
			// nudRewindBufferSize
			// 
			this.nudRewindBufferSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudRewindBufferSize.DecimalPlaces = 0;
			this.nudRewindBufferSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudRewindBufferSize.Location = new System.Drawing.Point(148, 0);
			this.nudRewindBufferSize.Margin = new System.Windows.Forms.Padding(0);
			this.nudRewindBufferSize.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
			this.nudRewindBufferSize.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudRewindBufferSize.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudRewindBufferSize.Name = "nudRewindBufferSize";
			this.nudRewindBufferSize.Size = new System.Drawing.Size(42, 20);
			this.nudRewindBufferSize.TabIndex = 1;
			this.nudRewindBufferSize.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			// 
			// lblRewindMinutes
			// 
			this.lblRewindMinutes.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRewindMinutes.AutoSize = true;
			this.lblRewindMinutes.Location = new System.Drawing.Point(193, 3);
			this.lblRewindMinutes.Name = "lblRewindMinutes";
			this.lblRewindMinutes.Size = new System.Drawing.Size(175, 13);
			this.lblRewindMinutes.TabIndex = 2;
			this.lblRewindMinutes.Text = "minutes (Memory Usage ≈1MB/min)";
			// 
			// chkFdsAutoInsertDisk
			// 
			this.chkFdsAutoInsertDisk.AutoSize = true;
			this.chkFdsAutoInsertDisk.Location = new System.Drawing.Point(13, 92);
			this.chkFdsAutoInsertDisk.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkFdsAutoInsertDisk.Name = "chkFdsAutoInsertDisk";
			this.chkFdsAutoInsertDisk.Size = new System.Drawing.Size(221, 17);
			this.chkFdsAutoInsertDisk.TabIndex = 10;
			this.chkFdsAutoInsertDisk.Text = "Automatically switch disks for FDS games";
			this.chkFdsAutoInsertDisk.UseVisualStyleBackColor = true;
			// 
			// chkShowGameTimer
			// 
			this.chkShowGameTimer.AutoSize = true;
			this.chkShowGameTimer.Location = new System.Drawing.Point(13, 250);
			this.chkShowGameTimer.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkShowGameTimer.Name = "chkShowGameTimer";
			this.chkShowGameTimer.Size = new System.Drawing.Size(107, 17);
			this.chkShowGameTimer.TabIndex = 11;
			this.chkShowGameTimer.Text = "Show game timer";
			this.chkShowGameTimer.UseVisualStyleBackColor = true;
			// 
			// chkShowFrameCounter
			// 
			this.chkShowFrameCounter.AutoSize = true;
			this.chkShowFrameCounter.Location = new System.Drawing.Point(13, 227);
			this.chkShowFrameCounter.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkShowFrameCounter.Name = "chkShowFrameCounter";
			this.chkShowFrameCounter.Size = new System.Drawing.Size(121, 17);
			this.chkShowFrameCounter.TabIndex = 12;
			this.chkShowFrameCounter.Text = "Show frame counter";
			this.chkShowFrameCounter.UseVisualStyleBackColor = true;
			// 
			// chkShowVsConfigOnLoad
			// 
			this.chkShowVsConfigOnLoad.AutoSize = true;
			this.chkShowVsConfigOnLoad.Location = new System.Drawing.Point(13, 273);
			this.chkShowVsConfigOnLoad.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkShowVsConfigOnLoad.Name = "chkShowVsConfigOnLoad";
			this.chkShowVsConfigOnLoad.Size = new System.Drawing.Size(331, 17);
			this.chkShowVsConfigOnLoad.TabIndex = 13;
			this.chkShowVsConfigOnLoad.Text = "Show game configuration dialog when loading VS System games";
			this.chkShowVsConfigOnLoad.UseVisualStyleBackColor = true;
			// 
			// chkDisableOsd
			// 
			this.chkDisableOsd.AutoSize = true;
			this.chkDisableOsd.Location = new System.Drawing.Point(13, 158);
			this.chkDisableOsd.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkDisableOsd.Name = "chkDisableOsd";
			this.chkDisableOsd.Size = new System.Drawing.Size(178, 17);
			this.chkDisableOsd.TabIndex = 14;
			this.chkDisableOsd.Text = "Disable on-screen display (OSD)";
			this.chkDisableOsd.UseVisualStyleBackColor = true;
			// 
			// lblFdsSettings
			// 
			this.lblFdsSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFdsSettings.AutoSize = true;
			this.lblFdsSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblFdsSettings.Location = new System.Drawing.Point(0, 30);
			this.lblFdsSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblFdsSettings.Name = "lblFdsSettings";
			this.lblFdsSettings.Size = new System.Drawing.Size(151, 13);
			this.lblFdsSettings.TabIndex = 24;
			this.lblFdsSettings.Text = "Famicom Disk System Settings";
			// 
			// lblUiDisplaySettings
			// 
			this.lblUiDisplaySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblUiDisplaySettings.AutoSize = true;
			this.lblUiDisplaySettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblUiDisplaySettings.Location = new System.Drawing.Point(0, 119);
			this.lblUiDisplaySettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblUiDisplaySettings.Name = "lblUiDisplaySettings";
			this.lblUiDisplaySettings.Size = new System.Drawing.Size(96, 13);
			this.lblUiDisplaySettings.TabIndex = 25;
			this.lblUiDisplaySettings.Text = "UI Display Settings";
			// 
			// lblGameSelectionScreenSettings
			// 
			this.lblGameSelectionScreenSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblGameSelectionScreenSettings.AutoSize = true;
			this.lblGameSelectionScreenSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblGameSelectionScreenSettings.Location = new System.Drawing.Point(0, 300);
			this.lblGameSelectionScreenSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblGameSelectionScreenSettings.Name = "lblGameSelectionScreenSettings";
			this.lblGameSelectionScreenSettings.Size = new System.Drawing.Size(160, 13);
			this.lblGameSelectionScreenSettings.TabIndex = 26;
			this.lblGameSelectionScreenSettings.Text = "Game Selection Screen Settings";
			// 
			// tmrSyncDateTime
			// 
			this.tmrSyncDateTime.Enabled = true;
			this.tmrSyncDateTime.Tick += new System.EventHandler(this.tmrSyncDateTime_Tick);
			// 
			// chkShowFullPathInRecents
			// 
			this.chkShowFullPathInRecents.AutoSize = true;
			this.chkShowFullPathInRecents.Location = new System.Drawing.Point(13, 204);
			this.chkShowFullPathInRecents.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkShowFullPathInRecents.Name = "chkShowFullPathInRecents";
			this.chkShowFullPathInRecents.Size = new System.Drawing.Size(184, 17);
			this.chkShowFullPathInRecents.TabIndex = 30;
			this.chkShowFullPathInRecents.Text = "Show full file path in recent file list";
			this.chkShowFullPathInRecents.UseVisualStyleBackColor = true;
			// 
			// frmPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 448);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(513, 322);
			this.Name = "frmPreferences";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgShortcuts.ResumeLayout(false);
			this.tpgSaveData.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpCloudSaves.ResumeLayout(false);
			this.tlpCloudSaves.ResumeLayout(false);
			this.tlpCloudSaveDesc.ResumeLayout(false);
			this.tlpCloudSaveDesc.PerformLayout();
			this.tlpCloudSaveEnabled.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOK)).EndInit();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.grpAutomaticSaves.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.flpAutoSave.ResumeLayout(false);
			this.flpAutoSave.PerformLayout();
			this.tpgNsf.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.tpgFiles.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.grpPathOverrides.ResumeLayout(false);
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.grpFileAssociations.ResumeLayout(false);
			this.tlpFileFormat.ResumeLayout(false);
			this.tlpFileFormat.PerformLayout();
			this.grpDataStorageLocation.ResumeLayout(false);
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkAutoLoadIps;
		private System.Windows.Forms.CheckBox chkSingleInstance;
		private System.Windows.Forms.CheckBox chkPauseWhenInBackground;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgFiles;
		private System.Windows.Forms.GroupBox grpFileAssociations;
		private System.Windows.Forms.TableLayoutPanel tlpFileFormat;
		private System.Windows.Forms.CheckBox chkNesFormat;
		private System.Windows.Forms.CheckBox chkFdsFormat;
		private System.Windows.Forms.CheckBox chkMmoFormat;
		private System.Windows.Forms.CheckBox chkAllowBackgroundInput;
		private System.Windows.Forms.CheckBox chkPauseOnMovieEnd;
		private System.Windows.Forms.Button btnOpenMesenFolder;
		private System.Windows.Forms.CheckBox chkAutomaticallyCheckForUpdates;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblDisplayLanguage;
		private System.Windows.Forms.ComboBox cboDisplayLanguage;
		private System.Windows.Forms.TabPage tpgSaveData;
		private System.Windows.Forms.TableLayoutPanel tlpCloudSaves;
		private System.Windows.Forms.TableLayoutPanel tlpCloudSaveDesc;
		private System.Windows.Forms.Label lblGoogleDriveIntegration;
		private System.Windows.Forms.Button btnEnableIntegration;
		private System.Windows.Forms.TableLayoutPanel tlpCloudSaveEnabled;
		private System.Windows.Forms.Button btnDisableIntegration;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.PictureBox picOK;
		private System.Windows.Forms.Label lblIntegrationOK;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblLastSync;
		private System.Windows.Forms.Label lblLastSyncDateTime;
		private System.Windows.Forms.Timer tmrSyncDateTime;
		private System.Windows.Forms.Button btnResync;
		private System.Windows.Forms.CheckBox chkMstFormat;
		private System.Windows.Forms.CheckBox chkNsfFormat;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TabPage tpgNsf;
		private ctrlRiskyOption chkDisableGameDatabase;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.CheckBox chkNsfMoveToNextTrackAfterTime;
		private MesenNumericUpDown nudNsfMoveToNextTrackTime;
		private System.Windows.Forms.Label lblNsfSeconds;
		private System.Windows.Forms.CheckBox chkNsfAutoDetectSilence;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private MesenNumericUpDown nudNsfAutoDetectSilenceDelay;
		private System.Windows.Forms.Label lblNsfMillisecondsOfSilence;
		private System.Windows.Forms.CheckBox chkNsfDisableApuIrqs;
		private System.Windows.Forms.CheckBox chkUnfFormat;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox grpCloudSaves;
		private System.Windows.Forms.GroupBox grpAutomaticSaves;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flpAutoSave;
		private System.Windows.Forms.CheckBox chkAutoSave;
		private MesenNumericUpDown nudAutoSave;
		private System.Windows.Forms.Label lblAutoSave;
		private System.Windows.Forms.CheckBox chkAutoSaveNotify;
		private System.Windows.Forms.TabPage tpgShortcuts;
		private ctrlEmulatorShortcuts ctrlEmulatorShortcuts;
		private System.Windows.Forms.CheckBox chkHidePauseOverlay;
		private System.Windows.Forms.CheckBox chkDisplayMovieIcons;
		private System.Windows.Forms.CheckBox chkAutoHideMenu;
		private System.Windows.Forms.CheckBox chkDisplayTitleBarInfo;
		private System.Windows.Forms.Label lblPauseBackgroundSettings;
		private System.Windows.Forms.Label lblMiscSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkFdsAutoLoadDisk;
		private System.Windows.Forms.CheckBox chkFdsFastForwardOnLoad;
		private System.Windows.Forms.CheckBox chkFdsAutoInsertDisk;
		private System.Windows.Forms.CheckBox chkShowGameTimer;
		private System.Windows.Forms.CheckBox chkShowFrameCounter;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button btnResetSettings;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblRewind;
		private MesenNumericUpDown nudRewindBufferSize;
		private System.Windows.Forms.Label lblRewindMinutes;
		private System.Windows.Forms.CheckBox chkShowVsConfigOnLoad;
		private System.Windows.Forms.CheckBox chkDisableOsd;
		private System.Windows.Forms.CheckBox chkDisableGameSelectionScreen;
		private System.Windows.Forms.CheckBox chkGameSelectionScreenResetGame;
		private System.Windows.Forms.Label lblFdsSettings;
		private System.Windows.Forms.Label lblUiDisplaySettings;
		private System.Windows.Forms.Label lblGameSelectionScreenSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.GroupBox grpDataStorageLocation;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.RadioButton radStorageDocuments;
		private System.Windows.Forms.RadioButton radStoragePortable;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.Label lblDataLocation;
		private System.Windows.Forms.GroupBox grpPathOverrides;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.CheckBox chkSaveStatesOverride;
		private System.Windows.Forms.CheckBox chkSaveDataOverride;
		private System.Windows.Forms.CheckBox chkMoviesOverride;
		private System.Windows.Forms.CheckBox chkScreenshotsOverride;
		private System.Windows.Forms.CheckBox chkAviOverride;
		private System.Windows.Forms.CheckBox chkWaveOverride;
		private ctrlPathSelection psWave;
		private ctrlPathSelection psMovies;
		private ctrlPathSelection psSaveData;
		private ctrlPathSelection psSaveStates;
		private ctrlPathSelection psScreenshots;
		private ctrlPathSelection psAvi;
		private System.Windows.Forms.CheckBox chkConfirmExitResetPower;
		private System.Windows.Forms.CheckBox chkDeveloperMode;
		private ctrlPathSelection psGame;
		private System.Windows.Forms.CheckBox chkGameOverride;
		private System.Windows.Forms.CheckBox chkAlwaysOnTop;
		private System.Windows.Forms.CheckBox chkShowFullPathInRecents;
	}
}