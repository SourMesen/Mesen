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
			this.chkDisableScreensaver = new System.Windows.Forms.CheckBox();
			this.btnOpenMesenFolder = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDisplayLanguage = new System.Windows.Forms.Label();
			this.cboDisplayLanguage = new System.Windows.Forms.ComboBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgCloudSave = new System.Windows.Forms.TabPage();
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
			this.tpgFileAssociations = new System.Windows.Forms.TabPage();
			this.grpFileAssociations = new System.Windows.Forms.GroupBox();
			this.tlpFileFormat = new System.Windows.Forms.TableLayoutPanel();
			this.chkNesFormat = new System.Windows.Forms.CheckBox();
			this.chkFdsFormat = new System.Windows.Forms.CheckBox();
			this.chkMmoFormat = new System.Windows.Forms.CheckBox();
			this.chkMstFormat = new System.Windows.Forms.CheckBox();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkUseAlternativeMmc3Irq = new System.Windows.Forms.CheckBox();
			this.chkAllowInvalidInput = new System.Windows.Forms.CheckBox();
			this.chkRemoveSpriteLimit = new System.Windows.Forms.CheckBox();
			this.chkFdsAutoLoadDisk = new System.Windows.Forms.CheckBox();
			this.chkFdsFastForwardOnLoad = new System.Windows.Forms.CheckBox();
			this.grpOverclocking = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblOverclockWarning = new System.Windows.Forms.Label();
			this.chkOverclockAdjustApu = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblClockRate = new System.Windows.Forms.Label();
			this.nudOverclockRate = new System.Windows.Forms.NumericUpDown();
			this.lblClockRatePercent = new System.Windows.Forms.Label();
			this.tmrSyncDateTime = new System.Windows.Forms.Timer(this.components);
			this.chkDisableGameDatabase = new System.Windows.Forms.CheckBox();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgCloudSave.SuspendLayout();
			this.tlpCloudSaves.SuspendLayout();
			this.tlpCloudSaveDesc.SuspendLayout();
			this.tlpCloudSaveEnabled.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOK)).BeginInit();
			this.flowLayoutPanel4.SuspendLayout();
			this.tpgFileAssociations.SuspendLayout();
			this.grpFileAssociations.SuspendLayout();
			this.tlpFileFormat.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpOverclocking.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverclockRate)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 282);
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
			this.tlpMain.Controls.Add(this.chkDisableScreensaver, 0, 7);
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
			this.tlpMain.Size = new System.Drawing.Size(473, 250);
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
			// chkDisableScreensaver
			// 
			this.chkDisableScreensaver.AutoSize = true;
			this.chkDisableScreensaver.Enabled = false;
			this.chkDisableScreensaver.Location = new System.Drawing.Point(3, 167);
			this.chkDisableScreensaver.Name = "chkDisableScreensaver";
			this.chkDisableScreensaver.Size = new System.Drawing.Size(245, 17);
			this.chkDisableScreensaver.TabIndex = 11;
			this.chkDisableScreensaver.Text = "Disable screensaver while emulation is running";
			this.chkDisableScreensaver.UseVisualStyleBackColor = true;
			this.chkDisableScreensaver.Visible = false;
			// 
			// btnOpenMesenFolder
			// 
			this.btnOpenMesenFolder.AutoSize = true;
			this.btnOpenMesenFolder.Location = new System.Drawing.Point(3, 224);
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
			this.tabMain.Controls.Add(this.tpgCloudSave);
			this.tabMain.Controls.Add(this.tpgFileAssociations);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(487, 282);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tlpMain);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(479, 256);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgCloudSave
			// 
			this.tpgCloudSave.Controls.Add(this.tlpCloudSaves);
			this.tpgCloudSave.Location = new System.Drawing.Point(4, 22);
			this.tpgCloudSave.Name = "tpgCloudSave";
			this.tpgCloudSave.Padding = new System.Windows.Forms.Padding(3);
			this.tpgCloudSave.Size = new System.Drawing.Size(479, 229);
			this.tpgCloudSave.TabIndex = 3;
			this.tpgCloudSave.Text = "Cloud Saves";
			this.tpgCloudSave.UseVisualStyleBackColor = true;
			// 
			// tlpCloudSaves
			// 
			this.tlpCloudSaves.ColumnCount = 1;
			this.tlpCloudSaves.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaves.Controls.Add(this.tlpCloudSaveDesc, 0, 0);
			this.tlpCloudSaves.Controls.Add(this.tlpCloudSaveEnabled, 0, 1);
			this.tlpCloudSaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpCloudSaves.Location = new System.Drawing.Point(3, 3);
			this.tlpCloudSaves.Name = "tlpCloudSaves";
			this.tlpCloudSaves.RowCount = 2;
			this.tlpCloudSaves.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaves.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCloudSaves.Size = new System.Drawing.Size(473, 223);
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
			this.tlpCloudSaveDesc.Size = new System.Drawing.Size(473, 100);
			this.tlpCloudSaveDesc.TabIndex = 0;
			// 
			// lblGoogleDriveIntegration
			// 
			this.lblGoogleDriveIntegration.AutoSize = true;
			this.lblGoogleDriveIntegration.Location = new System.Drawing.Point(3, 0);
			this.lblGoogleDriveIntegration.Name = "lblGoogleDriveIntegration";
			this.lblGoogleDriveIntegration.Size = new System.Drawing.Size(467, 52);
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
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCloudSaveEnabled.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpCloudSaveEnabled.Size = new System.Drawing.Size(473, 123);
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
			this.flowLayoutPanel3.Size = new System.Drawing.Size(473, 22);
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
			// tpgFileAssociations
			// 
			this.tpgFileAssociations.Controls.Add(this.grpFileAssociations);
			this.tpgFileAssociations.Location = new System.Drawing.Point(4, 22);
			this.tpgFileAssociations.Name = "tpgFileAssociations";
			this.tpgFileAssociations.Padding = new System.Windows.Forms.Padding(3);
			this.tpgFileAssociations.Size = new System.Drawing.Size(479, 229);
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
			this.grpFileAssociations.Size = new System.Drawing.Size(473, 223);
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
			this.tlpFileFormat.Controls.Add(this.chkMmoFormat, 1, 0);
			this.tlpFileFormat.Controls.Add(this.chkMstFormat, 1, 1);
			this.tlpFileFormat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpFileFormat.Location = new System.Drawing.Point(3, 16);
			this.tlpFileFormat.Name = "tlpFileFormat";
			this.tlpFileFormat.RowCount = 4;
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpFileFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpFileFormat.Size = new System.Drawing.Size(467, 204);
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
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel1);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(479, 256);
			this.tpgAdvanced.TabIndex = 1;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkDisableGameDatabase, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkUseAlternativeMmc3Irq, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkAllowInvalidInput, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkRemoveSpriteLimit, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsAutoLoadDisk, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.chkFdsFastForwardOnLoad, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.grpOverclocking, 0, 6);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 250);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkUseAlternativeMmc3Irq
			// 
			this.chkUseAlternativeMmc3Irq.AutoSize = true;
			this.chkUseAlternativeMmc3Irq.Location = new System.Drawing.Point(3, 26);
			this.chkUseAlternativeMmc3Irq.Name = "chkUseAlternativeMmc3Irq";
			this.chkUseAlternativeMmc3Irq.Size = new System.Drawing.Size(197, 17);
			this.chkUseAlternativeMmc3Irq.TabIndex = 0;
			this.chkUseAlternativeMmc3Irq.Text = "Use alternative MMC3 IRQ behavior";
			this.chkUseAlternativeMmc3Irq.UseVisualStyleBackColor = true;
			// 
			// chkAllowInvalidInput
			// 
			this.chkAllowInvalidInput.AutoSize = true;
			this.chkAllowInvalidInput.Location = new System.Drawing.Point(3, 49);
			this.chkAllowInvalidInput.Name = "chkAllowInvalidInput";
			this.chkAllowInvalidInput.Size = new System.Drawing.Size(341, 17);
			this.chkAllowInvalidInput.TabIndex = 1;
			this.chkAllowInvalidInput.Text = "Allow invalid input (e.g Down + Up or Left + Right at the same time)";
			this.chkAllowInvalidInput.UseVisualStyleBackColor = true;
			// 
			// chkRemoveSpriteLimit
			// 
			this.chkRemoveSpriteLimit.AutoSize = true;
			this.chkRemoveSpriteLimit.Location = new System.Drawing.Point(3, 72);
			this.chkRemoveSpriteLimit.Name = "chkRemoveSpriteLimit";
			this.chkRemoveSpriteLimit.Size = new System.Drawing.Size(205, 17);
			this.chkRemoveSpriteLimit.TabIndex = 2;
			this.chkRemoveSpriteLimit.Text = "Remove sprite limit (Reduces flashing)";
			this.chkRemoveSpriteLimit.UseVisualStyleBackColor = true;
			// 
			// chkFdsAutoLoadDisk
			// 
			this.chkFdsAutoLoadDisk.AutoSize = true;
			this.chkFdsAutoLoadDisk.Location = new System.Drawing.Point(3, 95);
			this.chkFdsAutoLoadDisk.Name = "chkFdsAutoLoadDisk";
			this.chkFdsAutoLoadDisk.Size = new System.Drawing.Size(303, 17);
			this.chkFdsAutoLoadDisk.TabIndex = 3;
			this.chkFdsAutoLoadDisk.Text = "Automatically insert disk 1 side A when starting FDS games";
			this.chkFdsAutoLoadDisk.UseVisualStyleBackColor = true;
			// 
			// chkFdsFastForwardOnLoad
			// 
			this.chkFdsFastForwardOnLoad.AutoSize = true;
			this.chkFdsFastForwardOnLoad.Location = new System.Drawing.Point(3, 118);
			this.chkFdsFastForwardOnLoad.Name = "chkFdsFastForwardOnLoad";
			this.chkFdsFastForwardOnLoad.Size = new System.Drawing.Size(342, 17);
			this.chkFdsFastForwardOnLoad.TabIndex = 4;
			this.chkFdsFastForwardOnLoad.Text = "Automatically fast forward FDS games when disk or BIOS is loading";
			this.chkFdsFastForwardOnLoad.UseVisualStyleBackColor = true;
			// 
			// grpOverclocking
			// 
			this.grpOverclocking.Controls.Add(this.tableLayoutPanel2);
			this.grpOverclocking.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpOverclocking.Location = new System.Drawing.Point(3, 141);
			this.grpOverclocking.Name = "grpOverclocking";
			this.grpOverclocking.Size = new System.Drawing.Size(467, 106);
			this.grpOverclocking.TabIndex = 5;
			this.grpOverclocking.TabStop = false;
			this.grpOverclocking.Text = "Overclocking";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblOverclockWarning, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkOverclockAdjustApu, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel5, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(461, 87);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// lblOverclockWarning
			// 
			this.lblOverclockWarning.AutoSize = true;
			this.lblOverclockWarning.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblOverclockWarning.ForeColor = System.Drawing.Color.Red;
			this.lblOverclockWarning.Location = new System.Drawing.Point(3, 5);
			this.lblOverclockWarning.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.lblOverclockWarning.Name = "lblOverclockWarning";
			this.lblOverclockWarning.Size = new System.Drawing.Size(455, 13);
			this.lblOverclockWarning.TabIndex = 2;
			this.lblOverclockWarning.Text = "WARNING: Overclocking will cause stability issues and may crash some games!";
			// 
			// chkOverclockAdjustApu
			// 
			this.chkOverclockAdjustApu.AutoSize = true;
			this.chkOverclockAdjustApu.Location = new System.Drawing.Point(3, 51);
			this.chkOverclockAdjustApu.Name = "chkOverclockAdjustApu";
			this.chkOverclockAdjustApu.Size = new System.Drawing.Size(401, 17);
			this.chkOverclockAdjustApu.TabIndex = 1;
			this.chkOverclockAdjustApu.Text = "Do not overclock APU (prevents sound pitch changes caused by overclocking)";
			this.chkOverclockAdjustApu.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Controls.Add(this.lblClockRate);
			this.flowLayoutPanel5.Controls.Add(this.nudOverclockRate);
			this.flowLayoutPanel5.Controls.Add(this.lblClockRatePercent);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(0, 23);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(461, 25);
			this.flowLayoutPanel5.TabIndex = 1;
			// 
			// lblClockRate
			// 
			this.lblClockRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClockRate.AutoSize = true;
			this.lblClockRate.Location = new System.Drawing.Point(3, 6);
			this.lblClockRate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblClockRate.Name = "lblClockRate";
			this.lblClockRate.Size = new System.Drawing.Size(63, 13);
			this.lblClockRate.TabIndex = 1;
			this.lblClockRate.Text = "Clock Rate:";
			// 
			// nudOverclockRate
			// 
			this.nudOverclockRate.Location = new System.Drawing.Point(66, 3);
			this.nudOverclockRate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudOverclockRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudOverclockRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverclockRate.Name = "nudOverclockRate";
			this.nudOverclockRate.Size = new System.Drawing.Size(46, 20);
			this.nudOverclockRate.TabIndex = 1;
			this.nudOverclockRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblClockRatePercent
			// 
			this.lblClockRatePercent.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClockRatePercent.AutoSize = true;
			this.lblClockRatePercent.Location = new System.Drawing.Point(112, 6);
			this.lblClockRatePercent.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblClockRatePercent.Name = "lblClockRatePercent";
			this.lblClockRatePercent.Size = new System.Drawing.Size(15, 13);
			this.lblClockRatePercent.TabIndex = 1;
			this.lblClockRatePercent.Text = "%";
			// 
			// tmrSyncDateTime
			// 
			this.tmrSyncDateTime.Enabled = true;
			this.tmrSyncDateTime.Tick += new System.EventHandler(this.tmrSyncDateTime_Tick);
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
			// frmPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 311);
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
			this.tpgCloudSave.ResumeLayout(false);
			this.tlpCloudSaves.ResumeLayout(false);
			this.tlpCloudSaveDesc.ResumeLayout(false);
			this.tlpCloudSaveDesc.PerformLayout();
			this.tlpCloudSaveEnabled.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOK)).EndInit();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.tpgFileAssociations.ResumeLayout(false);
			this.grpFileAssociations.ResumeLayout(false);
			this.tlpFileFormat.ResumeLayout(false);
			this.tlpFileFormat.PerformLayout();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpOverclocking.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverclockRate)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkAutoLoadIps;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.CheckBox chkSingleInstance;
		private System.Windows.Forms.CheckBox chkPauseWhenInBackground;
		private System.Windows.Forms.CheckBox chkDisableScreensaver;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgFileAssociations;
		private System.Windows.Forms.GroupBox grpFileAssociations;
		private System.Windows.Forms.TableLayoutPanel tlpFileFormat;
		private System.Windows.Forms.CheckBox chkNesFormat;
		private System.Windows.Forms.CheckBox chkFdsFormat;
		private System.Windows.Forms.CheckBox chkMmoFormat;
		private System.Windows.Forms.CheckBox chkMstFormat;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkUseAlternativeMmc3Irq;
		private System.Windows.Forms.CheckBox chkAllowInvalidInput;
		private System.Windows.Forms.CheckBox chkRemoveSpriteLimit;
		private System.Windows.Forms.CheckBox chkFdsAutoLoadDisk;
		private System.Windows.Forms.CheckBox chkFdsFastForwardOnLoad;
		private System.Windows.Forms.CheckBox chkAllowBackgroundInput;
		private System.Windows.Forms.CheckBox chkPauseOnMovieEnd;
		private System.Windows.Forms.Button btnOpenMesenFolder;
		private System.Windows.Forms.CheckBox chkAutomaticallyCheckForUpdates;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblDisplayLanguage;
		private System.Windows.Forms.ComboBox cboDisplayLanguage;
		private System.Windows.Forms.TabPage tpgCloudSave;
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
		private System.Windows.Forms.GroupBox grpOverclocking;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblOverclockWarning;
		private System.Windows.Forms.CheckBox chkOverclockAdjustApu;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Label lblClockRate;
		private System.Windows.Forms.NumericUpDown nudOverclockRate;
		private System.Windows.Forms.Label lblClockRatePercent;
		private System.Windows.Forms.CheckBox chkDisableGameDatabase;
	}
}