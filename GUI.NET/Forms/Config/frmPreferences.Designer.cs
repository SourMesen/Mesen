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
			this.chkSingleInstance = new System.Windows.Forms.CheckBox();
			this.chkAutomaticallyCheckForUpdates = new System.Windows.Forms.CheckBox();
			this.chkPauseOnMovieEnd = new System.Windows.Forms.CheckBox();
			this.chkAllowBackgroundInput = new System.Windows.Forms.CheckBox();
			this.chkPauseWhenInBackground = new System.Windows.Forms.CheckBox();
			this.chkAutoLoadIps = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOpenMesenFolder = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDisplayLanguage = new System.Windows.Forms.Label();
			this.cboDisplayLanguage = new System.Windows.Forms.ComboBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
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
			this.nudAutoSave = new System.Windows.Forms.NumericUpDown();
			this.lblAutoSave = new System.Windows.Forms.Label();
			this.tpgNsf = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkNsfAutoDetectSilence = new System.Windows.Forms.CheckBox();
			this.nudNsfAutoDetectSilenceDelay = new System.Windows.Forms.NumericUpDown();
			this.lblNsfMillisecondsOfSilence = new System.Windows.Forms.Label();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkNsfMoveToNextTrackAfterTime = new System.Windows.Forms.CheckBox();
			this.nudNsfMoveToNextTrackTime = new System.Windows.Forms.NumericUpDown();
			this.lblNsfSeconds = new System.Windows.Forms.Label();
			this.chkNsfDisableApuIrqs = new System.Windows.Forms.CheckBox();
			this.tpgFileAssociations = new System.Windows.Forms.TabPage();
			this.grpFileAssociations = new System.Windows.Forms.GroupBox();
			this.tlpFileFormat = new System.Windows.Forms.TableLayoutPanel();
			this.chkNsfeFormat = new System.Windows.Forms.CheckBox();
			this.chkNesFormat = new System.Windows.Forms.CheckBox();
			this.chkFdsFormat = new System.Windows.Forms.CheckBox();
			this.chkMmoFormat = new System.Windows.Forms.CheckBox();
			this.chkMstFormat = new System.Windows.Forms.CheckBox();
			this.chkNsfFormat = new System.Windows.Forms.CheckBox();
			this.chkUnfFormat = new System.Windows.Forms.CheckBox();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDisableGameDatabase = new System.Windows.Forms.CheckBox();
			this.chkFdsAutoLoadDisk = new System.Windows.Forms.CheckBox();
			this.chkFdsFastForwardOnLoad = new System.Windows.Forms.CheckBox();
			this.tmrSyncDateTime = new System.Windows.Forms.Timer(this.components);
			this.tpgShortcuts = new System.Windows.Forms.TabPage();
			this.ctrlEmulatorShortcuts = new Mesen.GUI.Forms.Config.ctrlEmulatorShortcuts();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
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
			((System.ComponentModel.ISupportInitialize)(this.nudAutoSave)).BeginInit();
			this.tpgNsf.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNsfAutoDetectSilenceDelay)).BeginInit();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNsfMoveToNextTrackTime)).BeginInit();
			this.tpgFileAssociations.SuspendLayout();
			this.grpFileAssociations.SuspendLayout();
			this.tlpFileFormat.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tpgShortcuts.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 369);
			this.baseConfigPanel.Size = new System.Drawing.Size(487, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.chkSingleInstance, 0, 2);
			this.tlpMain.Controls.Add(this.chkAutomaticallyCheckForUpdates, 0, 1);
			this.tlpMain.Controls.Add(this.chkPauseOnMovieEnd, 0, 6);
			this.tlpMain.Controls.Add(this.chkAllowBackgroundInput, 0, 5);
			this.tlpMain.Controls.Add(this.chkPauseWhenInBackground, 0, 4);
			this.tlpMain.Controls.Add(this.chkAutoLoadIps, 0, 3);
			this.tlpMain.Controls.Add(this.flowLayoutPanel6, 0, 1);
			this.tlpMain.Controls.Add(this.btnOpenMesenFolder, 0, 9);
			this.tlpMain.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(3, 3);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 10;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(473, 337);
			this.tlpMain.TabIndex = 1;
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
			// chkPauseOnMovieEnd
			// 
			this.chkPauseOnMovieEnd.AutoSize = true;
			this.chkPauseOnMovieEnd.Location = new System.Drawing.Point(3, 144);
			this.chkPauseOnMovieEnd.Name = "chkPauseOnMovieEnd";
			this.chkPauseOnMovieEnd.Size = new System.Drawing.Size(199, 17);
			this.chkPauseOnMovieEnd.TabIndex = 15;
			this.chkPauseOnMovieEnd.Text = "Pause when a movie finishes playing";
			this.chkPauseOnMovieEnd.UseVisualStyleBackColor = true;
			// 
			// chkAllowBackgroundInput
			// 
			this.chkAllowBackgroundInput.AutoSize = true;
			this.chkAllowBackgroundInput.Location = new System.Drawing.Point(3, 121);
			this.chkAllowBackgroundInput.Name = "chkAllowBackgroundInput";
			this.chkAllowBackgroundInput.Size = new System.Drawing.Size(177, 17);
			this.chkAllowBackgroundInput.TabIndex = 14;
			this.chkAllowBackgroundInput.Text = "Allow input when in background";
			this.chkAllowBackgroundInput.UseVisualStyleBackColor = true;
			// 
			// chkPauseWhenInBackground
			// 
			this.chkPauseWhenInBackground.AutoSize = true;
			this.chkPauseWhenInBackground.Location = new System.Drawing.Point(3, 98);
			this.chkPauseWhenInBackground.Name = "chkPauseWhenInBackground";
			this.chkPauseWhenInBackground.Size = new System.Drawing.Size(204, 17);
			this.chkPauseWhenInBackground.TabIndex = 13;
			this.chkPauseWhenInBackground.Text = "Pause emulation when in background";
			this.chkPauseWhenInBackground.UseVisualStyleBackColor = true;
			this.chkPauseWhenInBackground.CheckedChanged += new System.EventHandler(this.chkPauseWhenInBackground_CheckedChanged);
			// 
			// chkAutoLoadIps
			// 
			this.chkAutoLoadIps.AutoSize = true;
			this.chkAutoLoadIps.Location = new System.Drawing.Point(3, 75);
			this.chkAutoLoadIps.Name = "chkAutoLoadIps";
			this.chkAutoLoadIps.Size = new System.Drawing.Size(132, 17);
			this.chkAutoLoadIps.TabIndex = 9;
			this.chkAutoLoadIps.Text = "Auto-load IPS patches";
			this.chkAutoLoadIps.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 26);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(473, 1);
			this.flowLayoutPanel6.TabIndex = 10;
			// 
			// btnOpenMesenFolder
			// 
			this.btnOpenMesenFolder.AutoSize = true;
			this.btnOpenMesenFolder.Location = new System.Drawing.Point(3, 311);
			this.btnOpenMesenFolder.Name = "btnOpenMesenFolder";
			this.btnOpenMesenFolder.Size = new System.Drawing.Size(117, 23);
			this.btnOpenMesenFolder.TabIndex = 16;
			this.btnOpenMesenFolder.Text = "Open Mesen Folder";
			this.btnOpenMesenFolder.UseVisualStyleBackColor = true;
			this.btnOpenMesenFolder.Click += new System.EventHandler(this.btnOpenMesenFolder_Click);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblDisplayLanguage);
			this.flowLayoutPanel2.Controls.Add(this.cboDisplayLanguage);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(473, 26);
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
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgShortcuts);
			this.tabMain.Controls.Add(this.tpgSaveData);
			this.tabMain.Controls.Add(this.tpgNsf);
			this.tabMain.Controls.Add(this.tpgFileAssociations);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(487, 369);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tlpMain);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(479, 343);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgSaveData
			// 
			this.tpgSaveData.Controls.Add(this.tableLayoutPanel3);
			this.tpgSaveData.Location = new System.Drawing.Point(4, 22);
			this.tpgSaveData.Name = "tpgSaveData";
			this.tpgSaveData.Padding = new System.Windows.Forms.Padding(3);
			this.tpgSaveData.Size = new System.Drawing.Size(479, 343);
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
			this.tableLayoutPanel3.Size = new System.Drawing.Size(473, 337);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// grpCloudSaves
			// 
			this.grpCloudSaves.Controls.Add(this.tlpCloudSaves);
			this.grpCloudSaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCloudSaves.Location = new System.Drawing.Point(3, 76);
			this.grpCloudSaves.Name = "grpCloudSaves";
			this.grpCloudSaves.Size = new System.Drawing.Size(467, 258);
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
			this.tlpCloudSaves.Size = new System.Drawing.Size(461, 239);
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
			this.tlpCloudSaveDesc.Size = new System.Drawing.Size(461, 100);
			this.tlpCloudSaveDesc.TabIndex = 0;
			// 
			// lblGoogleDriveIntegration
			// 
			this.lblGoogleDriveIntegration.AutoSize = true;
			this.lblGoogleDriveIntegration.Location = new System.Drawing.Point(3, 0);
			this.lblGoogleDriveIntegration.Name = "lblGoogleDriveIntegration";
			this.lblGoogleDriveIntegration.Size = new System.Drawing.Size(455, 52);
			this.lblGoogleDriveIntegration.TabIndex = 0;
			this.lblGoogleDriveIntegration.Text = resources.GetString("lblGoogleDriveIntegration.Text");
			this.lblGoogleDriveIntegration.UseWaitCursor = true;
			// 
			// btnEnableIntegration
			// 
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
			this.tlpCloudSaveEnabled.Size = new System.Drawing.Size(461, 139);
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
			this.flowLayoutPanel3.Size = new System.Drawing.Size(461, 22);
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
			this.grpAutomaticSaves.Size = new System.Drawing.Size(467, 67);
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
			this.tableLayoutPanel4.Size = new System.Drawing.Size(461, 48);
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
			this.flpAutoSave.Size = new System.Drawing.Size(461, 23);
			this.flpAutoSave.TabIndex = 0;
			// 
			// chkAutoSave
			// 
			this.chkAutoSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAutoSave.AutoSize = true;
			this.chkAutoSave.Location = new System.Drawing.Point(3, 4);
			this.chkAutoSave.Name = "chkAutoSave";
			this.chkAutoSave.Size = new System.Drawing.Size(211, 17);
			this.chkAutoSave.TabIndex = 0;
			this.chkAutoSave.Text = "Automatically create a save state every";
			this.chkAutoSave.UseVisualStyleBackColor = true;
			this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);
			// 
			// nudAutoSave
			// 
			this.nudAutoSave.Location = new System.Drawing.Point(220, 3);
			this.nudAutoSave.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
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
			this.lblAutoSave.Location = new System.Drawing.Point(268, 6);
			this.lblAutoSave.Name = "lblAutoSave";
			this.lblAutoSave.Size = new System.Drawing.Size(99, 13);
			this.lblAutoSave.TabIndex = 2;
			this.lblAutoSave.Text = "minutes (F6 to load)";
			// 
			// tpgNsf
			// 
			this.tpgNsf.Controls.Add(this.tableLayoutPanel2);
			this.tpgNsf.Location = new System.Drawing.Point(4, 22);
			this.tpgNsf.Name = "tpgNsf";
			this.tpgNsf.Padding = new System.Windows.Forms.Padding(3);
			this.tpgNsf.Size = new System.Drawing.Size(479, 343);
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(473, 337);
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
			this.flowLayoutPanel7.Size = new System.Drawing.Size(473, 24);
			this.flowLayoutPanel7.TabIndex = 5;
			// 
			// chkNsfAutoDetectSilence
			// 
			this.chkNsfAutoDetectSilence.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkNsfAutoDetectSilence.AutoSize = true;
			this.chkNsfAutoDetectSilence.Location = new System.Drawing.Point(3, 4);
			this.chkNsfAutoDetectSilence.Name = "chkNsfAutoDetectSilence";
			this.chkNsfAutoDetectSilence.Size = new System.Drawing.Size(139, 17);
			this.chkNsfAutoDetectSilence.TabIndex = 1;
			this.chkNsfAutoDetectSilence.Text = "Move to next track after";
			this.chkNsfAutoDetectSilence.UseVisualStyleBackColor = true;
			// 
			// nudNsfAutoDetectSilenceDelay
			// 
			this.nudNsfAutoDetectSilenceDelay.Location = new System.Drawing.Point(145, 3);
			this.nudNsfAutoDetectSilenceDelay.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.nudNsfAutoDetectSilenceDelay.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
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
			this.lblNsfMillisecondsOfSilence.Location = new System.Drawing.Point(205, 6);
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
			this.flowLayoutPanel5.Size = new System.Drawing.Size(473, 24);
			this.flowLayoutPanel5.TabIndex = 4;
			// 
			// chkNsfMoveToNextTrackAfterTime
			// 
			this.chkNsfMoveToNextTrackAfterTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkNsfMoveToNextTrackAfterTime.AutoSize = true;
			this.chkNsfMoveToNextTrackAfterTime.Location = new System.Drawing.Point(3, 4);
			this.chkNsfMoveToNextTrackAfterTime.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.chkNsfMoveToNextTrackAfterTime.Name = "chkNsfMoveToNextTrackAfterTime";
			this.chkNsfMoveToNextTrackAfterTime.Size = new System.Drawing.Size(126, 17);
			this.chkNsfMoveToNextTrackAfterTime.TabIndex = 2;
			this.chkNsfMoveToNextTrackAfterTime.Text = "Limit track run time to";
			this.chkNsfMoveToNextTrackAfterTime.UseVisualStyleBackColor = true;
			// 
			// nudNsfMoveToNextTrackTime
			// 
			this.nudNsfMoveToNextTrackTime.Location = new System.Drawing.Point(129, 3);
			this.nudNsfMoveToNextTrackTime.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.nudNsfMoveToNextTrackTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.nudNsfMoveToNextTrackTime.Name = "nudNsfMoveToNextTrackTime";
			this.nudNsfMoveToNextTrackTime.Size = new System.Drawing.Size(44, 20);
			this.nudNsfMoveToNextTrackTime.TabIndex = 3;
			// 
			// lblNsfSeconds
			// 
			this.lblNsfSeconds.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNsfSeconds.AutoSize = true;
			this.lblNsfSeconds.Location = new System.Drawing.Point(176, 6);
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
			// tpgFileAssociations
			// 
			this.tpgFileAssociations.Controls.Add(this.grpFileAssociations);
			this.tpgFileAssociations.Location = new System.Drawing.Point(4, 22);
			this.tpgFileAssociations.Name = "tpgFileAssociations";
			this.tpgFileAssociations.Padding = new System.Windows.Forms.Padding(3);
			this.tpgFileAssociations.Size = new System.Drawing.Size(479, 343);
			this.tpgFileAssociations.TabIndex = 2;
			this.tpgFileAssociations.Text = "File Associations";
			this.tpgFileAssociations.UseVisualStyleBackColor = true;
			// 
			// grpFileAssociations
			// 
			this.grpFileAssociations.Controls.Add(this.tlpFileFormat);
			this.grpFileAssociations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFileAssociations.Location = new System.Drawing.Point(3, 3);
			this.grpFileAssociations.Name = "grpFileAssociations";
			this.grpFileAssociations.Size = new System.Drawing.Size(473, 337);
			this.grpFileAssociations.TabIndex = 12;
			this.grpFileAssociations.TabStop = false;
			this.grpFileAssociations.Text = "File Associations";
			// 
			// tlpFileFormat
			// 
			this.tlpFileFormat.ColumnCount = 2;
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpFileFormat.Controls.Add(this.chkNsfeFormat, 0, 4);
			this.tlpFileFormat.Controls.Add(this.chkNesFormat, 0, 0);
			this.tlpFileFormat.Controls.Add(this.chkFdsFormat, 0, 1);
			this.tlpFileFormat.Controls.Add(this.chkMmoFormat, 1, 0);
			this.tlpFileFormat.Controls.Add(this.chkMstFormat, 1, 1);
			this.tlpFileFormat.Controls.Add(this.chkNsfFormat, 0, 3);
			this.tlpFileFormat.Controls.Add(this.chkUnfFormat, 0, 2);
			this.tlpFileFormat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpFileFormat.Location = new System.Drawing.Point(3, 16);
			this.tlpFileFormat.Name = "tlpFileFormat";
			this.tlpFileFormat.RowCount = 5;
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.Size = new System.Drawing.Size(467, 318);
			this.tlpFileFormat.TabIndex = 0;
			// 
			// chkNsfeFormat
			// 
			this.chkNsfeFormat.AutoSize = true;
			this.chkNsfeFormat.Location = new System.Drawing.Point(3, 95);
			this.chkNsfeFormat.Name = "chkNsfeFormat";
			this.chkNsfeFormat.Size = new System.Drawing.Size(226, 17);
			this.chkNsfeFormat.TabIndex = 15;
			this.chkNsfeFormat.Text = ".NSFE (Nintendo Sound Format Extended)";
			this.chkNsfeFormat.UseVisualStyleBackColor = true;
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
			// chkMmoFormat
			// 
			this.chkMmoFormat.AutoSize = true;
			this.chkMmoFormat.Location = new System.Drawing.Point(236, 3);
			this.chkMmoFormat.Name = "chkMmoFormat";
			this.chkMmoFormat.Size = new System.Drawing.Size(133, 17);
			this.chkMmoFormat.TabIndex = 11;
			this.chkMmoFormat.Text = ".MMO (Mesen Movies)";
			this.chkMmoFormat.UseVisualStyleBackColor = true;
			// 
			// chkMstFormat
			// 
			this.chkMstFormat.AutoSize = true;
			this.chkMstFormat.Enabled = false;
			this.chkMstFormat.Location = new System.Drawing.Point(236, 26);
			this.chkMstFormat.Name = "chkMstFormat";
			this.chkMstFormat.Size = new System.Drawing.Size(144, 17);
			this.chkMstFormat.TabIndex = 13;
			this.chkMstFormat.Text = ".MST (Mesen Savestate)";
			this.chkMstFormat.UseVisualStyleBackColor = true;
			// 
			// chkNsfFormat
			// 
			this.chkNsfFormat.AutoSize = true;
			this.chkNsfFormat.Location = new System.Drawing.Point(3, 72);
			this.chkNsfFormat.Name = "chkNsfFormat";
			this.chkNsfFormat.Size = new System.Drawing.Size(171, 17);
			this.chkNsfFormat.TabIndex = 14;
			this.chkNsfFormat.Text = ".NSF (Nintendo Sound Format)";
			this.chkNsfFormat.UseVisualStyleBackColor = true;
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
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel1);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(479, 343);
			this.tpgAdvanced.TabIndex = 1;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkDisableGameDatabase, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsAutoLoadDisk, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsFastForwardOnLoad, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 250);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkDisableGameDatabase
			// 
			this.chkDisableGameDatabase.AutoSize = true;
			this.chkDisableGameDatabase.Location = new System.Drawing.Point(3, 3);
			this.chkDisableGameDatabase.Name = "chkDisableGameDatabase";
			this.chkDisableGameDatabase.Size = new System.Drawing.Size(170, 17);
			this.chkDisableGameDatabase.TabIndex = 6;
			this.chkDisableGameDatabase.Text = "Disable built-in game database";
			this.chkDisableGameDatabase.UseVisualStyleBackColor = true;
			// 
			// chkFdsAutoLoadDisk
			// 
			this.chkFdsAutoLoadDisk.AutoSize = true;
			this.chkFdsAutoLoadDisk.Location = new System.Drawing.Point(3, 26);
			this.chkFdsAutoLoadDisk.Name = "chkFdsAutoLoadDisk";
			this.chkFdsAutoLoadDisk.Size = new System.Drawing.Size(303, 17);
			this.chkFdsAutoLoadDisk.TabIndex = 3;
			this.chkFdsAutoLoadDisk.Text = "Automatically insert disk 1 side A when starting FDS games";
			this.chkFdsAutoLoadDisk.UseVisualStyleBackColor = true;
			// 
			// chkFdsFastForwardOnLoad
			// 
			this.chkFdsFastForwardOnLoad.AutoSize = true;
			this.chkFdsFastForwardOnLoad.Location = new System.Drawing.Point(3, 49);
			this.chkFdsFastForwardOnLoad.Name = "chkFdsFastForwardOnLoad";
			this.chkFdsFastForwardOnLoad.Size = new System.Drawing.Size(342, 17);
			this.chkFdsFastForwardOnLoad.TabIndex = 4;
			this.chkFdsFastForwardOnLoad.Text = "Automatically fast forward FDS games when disk or BIOS is loading";
			this.chkFdsFastForwardOnLoad.UseVisualStyleBackColor = true;
			// 
			// tmrSyncDateTime
			// 
			this.tmrSyncDateTime.Enabled = true;
			this.tmrSyncDateTime.Tick += new System.EventHandler(this.tmrSyncDateTime_Tick);
			// 
			// tpgShortcuts
			// 
			this.tpgShortcuts.Controls.Add(this.ctrlEmulatorShortcuts);
			this.tpgShortcuts.Location = new System.Drawing.Point(4, 22);
			this.tpgShortcuts.Name = "tpgShortcuts";
			this.tpgShortcuts.Padding = new System.Windows.Forms.Padding(3);
			this.tpgShortcuts.Size = new System.Drawing.Size(479, 343);
			this.tpgShortcuts.TabIndex = 7;
			this.tpgShortcuts.Text = "Shortcut Keys";
			this.tpgShortcuts.UseVisualStyleBackColor = true;
			// 
			// ctrlEmulatorShortcuts
			// 
			this.ctrlEmulatorShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEmulatorShortcuts.Location = new System.Drawing.Point(3, 3);
			this.ctrlEmulatorShortcuts.Name = "ctrlEmulatorShortcuts";
			this.ctrlEmulatorShortcuts.Size = new System.Drawing.Size(473, 337);
			this.ctrlEmulatorShortcuts.TabIndex = 0;
			// 
			// frmPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 398);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(503, 322);
			this.Name = "frmPreferences";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preferences";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
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
			((System.ComponentModel.ISupportInitialize)(this.nudAutoSave)).EndInit();
			this.tpgNsf.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNsfAutoDetectSilenceDelay)).EndInit();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNsfMoveToNextTrackTime)).EndInit();
			this.tpgFileAssociations.ResumeLayout(false);
			this.grpFileAssociations.ResumeLayout(false);
			this.tlpFileFormat.ResumeLayout(false);
			this.tlpFileFormat.PerformLayout();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tpgShortcuts.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkAutoLoadIps;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.CheckBox chkSingleInstance;
		private System.Windows.Forms.CheckBox chkPauseWhenInBackground;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgFileAssociations;
		private System.Windows.Forms.GroupBox grpFileAssociations;
		private System.Windows.Forms.TableLayoutPanel tlpFileFormat;
		private System.Windows.Forms.CheckBox chkNesFormat;
		private System.Windows.Forms.CheckBox chkFdsFormat;
		private System.Windows.Forms.CheckBox chkMmoFormat;
		private System.Windows.Forms.CheckBox chkMstFormat;
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
		private System.Windows.Forms.CheckBox chkNsfeFormat;
		private System.Windows.Forms.CheckBox chkNsfFormat;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TabPage tpgNsf;
		private System.Windows.Forms.CheckBox chkFdsFastForwardOnLoad;
		private System.Windows.Forms.CheckBox chkFdsAutoLoadDisk;
		private System.Windows.Forms.CheckBox chkDisableGameDatabase;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.CheckBox chkNsfMoveToNextTrackAfterTime;
		private System.Windows.Forms.NumericUpDown nudNsfMoveToNextTrackTime;
		private System.Windows.Forms.Label lblNsfSeconds;
		private System.Windows.Forms.CheckBox chkNsfAutoDetectSilence;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.NumericUpDown nudNsfAutoDetectSilenceDelay;
		private System.Windows.Forms.Label lblNsfMillisecondsOfSilence;
		private System.Windows.Forms.CheckBox chkNsfDisableApuIrqs;
		private System.Windows.Forms.CheckBox chkUnfFormat;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox grpCloudSaves;
		private System.Windows.Forms.GroupBox grpAutomaticSaves;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flpAutoSave;
		private System.Windows.Forms.CheckBox chkAutoSave;
		private System.Windows.Forms.NumericUpDown nudAutoSave;
		private System.Windows.Forms.Label lblAutoSave;
		private System.Windows.Forms.CheckBox chkAutoSaveNotify;
		private System.Windows.Forms.TabPage tpgShortcuts;
		private ctrlEmulatorShortcuts ctrlEmulatorShortcuts;
	}
}