using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	partial class frmVideoConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVideoConfig));
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.chkUseExclusiveFullscreen = new System.Windows.Forms.CheckBox();
			this.lblVideoScale = new System.Windows.Forms.Label();
			this.chkVerticalSync = new System.Windows.Forms.CheckBox();
			this.lblDisplayRatio = new System.Windows.Forms.Label();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkUseHdPacks = new System.Windows.Forms.CheckBox();
			this.picHdNesTooltip = new System.Windows.Forms.PictureBox();
			this.nudScale = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.cboAspectRatio = new System.Windows.Forms.ComboBox();
			this.lblCustomRatio = new System.Windows.Forms.Label();
			this.nudCustomRatio = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.chkFullscreenForceIntegerScale = new System.Windows.Forms.CheckBox();
			this.chkShowFps = new System.Windows.Forms.CheckBox();
			this.chkIntegerFpsMode = new System.Windows.Forms.CheckBox();
			this.flpRefreshRate = new System.Windows.Forms.FlowLayoutPanel();
			this.lblRequestedRefreshRate = new System.Windows.Forms.Label();
			this.cboRefreshRate = new System.Windows.Forms.ComboBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgPicture = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSelectPreset = new System.Windows.Forms.Button();
			this.btnResetPictureSettings = new System.Windows.Forms.Button();
			this.grpNtscFilter = new System.Windows.Forms.GroupBox();
			this.tlpNtscFilter2 = new System.Windows.Forms.TableLayoutPanel();
			this.trkYFilterLength = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkIFilterLength = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkQFilterLength = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.tlpNtscFilter1 = new System.Windows.Forms.TableLayoutPanel();
			this.trkArtifacts = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkBleed = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkFringing = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkGamma = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkResolution = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkSharpness = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.chkMergeFields = new System.Windows.Forms.CheckBox();
			this.chkVerticalBlend = new System.Windows.Forms.CheckBox();
			this.grpCommon = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.chkBilinearInterpolation = new System.Windows.Forms.CheckBox();
			this.trkBrightness = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkContrast = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkHue = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkSaturation = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.grpScanlines = new System.Windows.Forms.GroupBox();
			this.trkScanlines = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.cboFilter = new System.Windows.Forms.ComboBox();
			this.lblVideoFilter = new System.Windows.Forms.Label();
			this.tpgOverscan = new System.Windows.Forms.TabPage();
			this.tabOverscan = new System.Windows.Forms.TabControl();
			this.tpgOverscanGlobal = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.picOverscan = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.nudOverscanTop = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblTop = new System.Windows.Forms.Label();
			this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
			this.nudOverscanBottom = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblBottom = new System.Windows.Forms.Label();
			this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
			this.nudOverscanRight = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblRight = new System.Windows.Forms.Label();
			this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
			this.nudOverscanLeft = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblLeft = new System.Windows.Forms.Label();
			this.tpgOverscanGameSpecific = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.picGameSpecificOverscan = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
			this.nudGameSpecificOverscanTop = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblGameSpecificOverscanTop = new System.Windows.Forms.Label();
			this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
			this.nudGameSpecificOverscanBottom = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblGameSpecificOverscanBottom = new System.Windows.Forms.Label();
			this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
			this.nudGameSpecificOverscanRight = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblGameSpecificOverscanRight = new System.Windows.Forms.Label();
			this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
			this.nudGameSpecificOverscanLeft = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblGameSpecificOverscanLeft = new System.Windows.Forms.Label();
			this.chkEnableGameSpecificOverscan = new System.Windows.Forms.CheckBox();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.tpgPalette = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnExportPalette = new System.Windows.Forms.Button();
			this.btnSelectPalette = new System.Windows.Forms.Button();
			this.btnLoadPalFile = new System.Windows.Forms.Button();
			this.chkShowColorIndexes = new System.Windows.Forms.CheckBox();
			this.chkUseCustomVsPalette = new System.Windows.Forms.CheckBox();
			this.ctrlPaletteDisplay = new Mesen.GUI.Debugger.ctrlPaletteDisplay();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDisableBackground = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkDisableSprites = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkForceBackgroundFirstColumn = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkForceSpritesFirstColumn = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.lblScreenRotation = new System.Windows.Forms.Label();
			this.cboScreenRotation = new System.Windows.Forms.ComboBox();
			this.contextPicturePresets = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuPresetComposite = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPresetSVideo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPresetRgb = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPresetMonochrome = new System.Windows.Forms.ToolStripMenuItem();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.contextPaletteList = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuDefaultPalette = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPaletteCompositeDirect = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteNesClassic = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteNestopiaRgb = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteOriginalHardware = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPalettePvmStyle = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteSonyCxa2025As = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteUnsaturated = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaletteYuv = new System.Windows.Forms.ToolStripMenuItem();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHdNesTooltip)).BeginInit();
			this.flowLayoutPanel6.SuspendLayout();
			this.flpRefreshRate.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgPicture.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.grpNtscFilter.SuspendLayout();
			this.tlpNtscFilter2.SuspendLayout();
			this.tlpNtscFilter1.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.grpCommon.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.grpScanlines.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tpgOverscan.SuspendLayout();
			this.tabOverscan.SuspendLayout();
			this.tpgOverscanGlobal.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).BeginInit();
			this.tableLayoutPanel11.SuspendLayout();
			this.tableLayoutPanel12.SuspendLayout();
			this.tableLayoutPanel13.SuspendLayout();
			this.tableLayoutPanel14.SuspendLayout();
			this.tpgOverscanGameSpecific.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picGameSpecificOverscan)).BeginInit();
			this.tableLayoutPanel15.SuspendLayout();
			this.tableLayoutPanel16.SuspendLayout();
			this.tableLayoutPanel17.SuspendLayout();
			this.tableLayoutPanel18.SuspendLayout();
			this.tpgPalette.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.contextPicturePresets.SuspendLayout();
			this.contextPaletteList.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 428);
			this.baseConfigPanel.Size = new System.Drawing.Size(535, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.chkUseExclusiveFullscreen, 0, 4);
			this.tlpMain.Controls.Add(this.lblVideoScale, 0, 0);
			this.tlpMain.Controls.Add(this.chkVerticalSync, 0, 3);
			this.tlpMain.Controls.Add(this.lblDisplayRatio, 0, 1);
			this.tlpMain.Controls.Add(this.flowLayoutPanel7, 0, 7);
			this.tlpMain.Controls.Add(this.nudScale, 1, 0);
			this.tlpMain.Controls.Add(this.flowLayoutPanel6, 1, 1);
			this.tlpMain.Controls.Add(this.chkFullscreenForceIntegerScale, 0, 6);
			this.tlpMain.Controls.Add(this.chkShowFps, 0, 8);
			this.tlpMain.Controls.Add(this.chkIntegerFpsMode, 0, 2);
			this.tlpMain.Controls.Add(this.flpRefreshRate, 0, 5);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(3, 3);
			this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
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
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Size = new System.Drawing.Size(521, 396);
			this.tlpMain.TabIndex = 1;
			// 
			// chkUseExclusiveFullscreen
			// 
			this.chkUseExclusiveFullscreen.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkUseExclusiveFullscreen.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkUseExclusiveFullscreen, 2);
			this.chkUseExclusiveFullscreen.Location = new System.Drawing.Point(3, 96);
			this.chkUseExclusiveFullscreen.Name = "chkUseExclusiveFullscreen";
			this.chkUseExclusiveFullscreen.Size = new System.Drawing.Size(169, 17);
			this.chkUseExclusiveFullscreen.TabIndex = 24;
			this.chkUseExclusiveFullscreen.Text = "Use exclusive fullscreen mode";
			this.chkUseExclusiveFullscreen.UseVisualStyleBackColor = true;
			this.chkUseExclusiveFullscreen.CheckedChanged += new System.EventHandler(this.chkUseExclusiveFullscreen_CheckedChanged);
			// 
			// lblVideoScale
			// 
			this.lblVideoScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVideoScale.AutoSize = true;
			this.lblVideoScale.Location = new System.Drawing.Point(3, 4);
			this.lblVideoScale.Name = "lblVideoScale";
			this.lblVideoScale.Size = new System.Drawing.Size(37, 13);
			this.lblVideoScale.TabIndex = 11;
			this.lblVideoScale.Text = "Scale:";
			// 
			// chkVerticalSync
			// 
			this.chkVerticalSync.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkVerticalSync.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkVerticalSync, 2);
			this.chkVerticalSync.Location = new System.Drawing.Point(3, 73);
			this.chkVerticalSync.Name = "chkVerticalSync";
			this.chkVerticalSync.Size = new System.Drawing.Size(121, 17);
			this.chkVerticalSync.TabIndex = 15;
			this.chkVerticalSync.Text = "Enable vertical sync";
			this.chkVerticalSync.UseVisualStyleBackColor = true;
			// 
			// lblDisplayRatio
			// 
			this.lblDisplayRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDisplayRatio.AutoSize = true;
			this.lblDisplayRatio.Location = new System.Drawing.Point(3, 27);
			this.lblDisplayRatio.Name = "lblDisplayRatio";
			this.lblDisplayRatio.Size = new System.Drawing.Size(71, 13);
			this.lblDisplayRatio.TabIndex = 17;
			this.lblDisplayRatio.Text = "Aspect Ratio:";
			// 
			// flowLayoutPanel7
			// 
			this.tlpMain.SetColumnSpan(this.flowLayoutPanel7, 2);
			this.flowLayoutPanel7.Controls.Add(this.chkUseHdPacks);
			this.flowLayoutPanel7.Controls.Add(this.picHdNesTooltip);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 166);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(521, 23);
			this.flowLayoutPanel7.TabIndex = 20;
			// 
			// chkUseHdPacks
			// 
			this.chkUseHdPacks.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkUseHdPacks.AutoSize = true;
			this.chkUseHdPacks.Location = new System.Drawing.Point(3, 3);
			this.chkUseHdPacks.Name = "chkUseHdPacks";
			this.chkUseHdPacks.Size = new System.Drawing.Size(134, 17);
			this.chkUseHdPacks.TabIndex = 19;
			this.chkUseHdPacks.Text = "Use HDNes HD packs";
			this.chkUseHdPacks.UseVisualStyleBackColor = true;
			// 
			// picHdNesTooltip
			// 
			this.picHdNesTooltip.BackgroundImage = global::Mesen.GUI.Properties.Resources.Help;
			this.picHdNesTooltip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picHdNesTooltip.Location = new System.Drawing.Point(143, 3);
			this.picHdNesTooltip.Name = "picHdNesTooltip";
			this.picHdNesTooltip.Size = new System.Drawing.Size(17, 17);
			this.picHdNesTooltip.TabIndex = 21;
			this.picHdNesTooltip.TabStop = false;
			// 
			// nudScale
			// 
			this.nudScale.DecimalPlaces = 2;
			this.nudScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudScale.Location = new System.Drawing.Point(77, 0);
			this.nudScale.Margin = new System.Windows.Forms.Padding(0);
			this.nudScale.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudScale.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudScale.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.nudScale.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudScale.Name = "nudScale";
			this.nudScale.Size = new System.Drawing.Size(48, 21);
			this.nudScale.TabIndex = 21;
			this.nudScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.Controls.Add(this.cboAspectRatio);
			this.flowLayoutPanel6.Controls.Add(this.lblCustomRatio);
			this.flowLayoutPanel6.Controls.Add(this.nudCustomRatio);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(77, 21);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(444, 26);
			this.flowLayoutPanel6.TabIndex = 22;
			// 
			// cboAspectRatio
			// 
			this.cboAspectRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAspectRatio.FormattingEnabled = true;
			this.cboAspectRatio.Items.AddRange(new object[] {
            "Auto",
            "NTSC (8:7)",
            "PAL (18:13)",
            "Standard (4:3)",
            "Widescreen (16:9)"});
			this.cboAspectRatio.Location = new System.Drawing.Point(3, 3);
			this.cboAspectRatio.Name = "cboAspectRatio";
			this.cboAspectRatio.Size = new System.Drawing.Size(197, 21);
			this.cboAspectRatio.TabIndex = 16;
			this.cboAspectRatio.SelectedIndexChanged += new System.EventHandler(this.cboAspectRatio_SelectedIndexChanged);
			// 
			// lblCustomRatio
			// 
			this.lblCustomRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCustomRatio.AutoSize = true;
			this.lblCustomRatio.Location = new System.Drawing.Point(206, 7);
			this.lblCustomRatio.Name = "lblCustomRatio";
			this.lblCustomRatio.Size = new System.Drawing.Size(76, 13);
			this.lblCustomRatio.TabIndex = 17;
			this.lblCustomRatio.Text = "Custom Ratio: ";
			this.lblCustomRatio.Visible = false;
			// 
			// nudCustomRatio
			// 
			this.nudCustomRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudCustomRatio.DecimalPlaces = 3;
			this.nudCustomRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudCustomRatio.Location = new System.Drawing.Point(285, 3);
			this.nudCustomRatio.Margin = new System.Windows.Forms.Padding(0);
			this.nudCustomRatio.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudCustomRatio.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudCustomRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudCustomRatio.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudCustomRatio.Name = "nudCustomRatio";
			this.nudCustomRatio.Size = new System.Drawing.Size(48, 21);
			this.nudCustomRatio.TabIndex = 22;
			this.nudCustomRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudCustomRatio.Visible = false;
			// 
			// chkFullscreenForceIntegerScale
			// 
			this.chkFullscreenForceIntegerScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkFullscreenForceIntegerScale.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkFullscreenForceIntegerScale, 2);
			this.chkFullscreenForceIntegerScale.Location = new System.Drawing.Point(3, 146);
			this.chkFullscreenForceIntegerScale.Name = "chkFullscreenForceIntegerScale";
			this.chkFullscreenForceIntegerScale.Size = new System.Drawing.Size(289, 17);
			this.chkFullscreenForceIntegerScale.TabIndex = 23;
			this.chkFullscreenForceIntegerScale.Text = "Use integer scale values when entering fullscreen mode";
			this.chkFullscreenForceIntegerScale.UseVisualStyleBackColor = true;
			// 
			// chkShowFps
			// 
			this.chkShowFps.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkShowFps.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkShowFps, 2);
			this.chkShowFps.Location = new System.Drawing.Point(3, 192);
			this.chkShowFps.Name = "chkShowFps";
			this.chkShowFps.Size = new System.Drawing.Size(76, 17);
			this.chkShowFps.TabIndex = 9;
			this.chkShowFps.Text = "Show FPS";
			this.chkShowFps.UseVisualStyleBackColor = true;
			// 
			// chkIntegerFpsMode
			// 
			this.chkIntegerFpsMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkIntegerFpsMode.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkIntegerFpsMode, 2);
			this.chkIntegerFpsMode.Location = new System.Drawing.Point(3, 50);
			this.chkIntegerFpsMode.Name = "chkIntegerFpsMode";
			this.chkIntegerFpsMode.Size = new System.Drawing.Size(308, 17);
			this.chkIntegerFpsMode.TabIndex = 24;
			this.chkIntegerFpsMode.Text = "Enable integer FPS mode (e.g: run at 60 fps instead of 60.1)";
			this.chkIntegerFpsMode.UseVisualStyleBackColor = true;
			// 
			// flpRefreshRate
			// 
			this.tlpMain.SetColumnSpan(this.flpRefreshRate, 2);
			this.flpRefreshRate.Controls.Add(this.lblRequestedRefreshRate);
			this.flpRefreshRate.Controls.Add(this.cboRefreshRate);
			this.flpRefreshRate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpRefreshRate.Location = new System.Drawing.Point(30, 116);
			this.flpRefreshRate.Margin = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.flpRefreshRate.Name = "flpRefreshRate";
			this.flpRefreshRate.Size = new System.Drawing.Size(491, 27);
			this.flpRefreshRate.TabIndex = 26;
			this.flpRefreshRate.Visible = false;
			// 
			// lblRequestedRefreshRate
			// 
			this.lblRequestedRefreshRate.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblRequestedRefreshRate.AutoSize = true;
			this.lblRequestedRefreshRate.Location = new System.Drawing.Point(3, 7);
			this.lblRequestedRefreshRate.Name = "lblRequestedRefreshRate";
			this.lblRequestedRefreshRate.Size = new System.Drawing.Size(128, 13);
			this.lblRequestedRefreshRate.TabIndex = 17;
			this.lblRequestedRefreshRate.Text = "Requested Refresh Rate:";
			// 
			// cboRefreshRate
			// 
			this.cboRefreshRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRefreshRate.FormattingEnabled = true;
			this.cboRefreshRate.Items.AddRange(new object[] {
            "Auto",
            "NTSC (8:7)",
            "PAL (18:13)",
            "Standard (4:3)",
            "Widescreen (16:9)"});
			this.cboRefreshRate.Location = new System.Drawing.Point(137, 3);
			this.cboRefreshRate.Name = "cboRefreshRate";
			this.cboRefreshRate.Size = new System.Drawing.Size(68, 21);
			this.cboRefreshRate.TabIndex = 25;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgPicture);
			this.tabMain.Controls.Add(this.tpgOverscan);
			this.tabMain.Controls.Add(this.tpgPalette);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(535, 428);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tlpMain);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(527, 402);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgPicture
			// 
			this.tpgPicture.Controls.Add(this.tableLayoutPanel5);
			this.tpgPicture.Location = new System.Drawing.Point(4, 22);
			this.tpgPicture.Name = "tpgPicture";
			this.tpgPicture.Padding = new System.Windows.Forms.Padding(3);
			this.tpgPicture.Size = new System.Drawing.Size(527, 402);
			this.tpgPicture.TabIndex = 3;
			this.tpgPicture.Text = "Picture";
			this.tpgPicture.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 3);
			this.tableLayoutPanel5.Controls.Add(this.grpNtscFilter, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.grpCommon, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.grpScanlines, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel8, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 4;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(521, 396);
			this.tableLayoutPanel5.TabIndex = 5;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.92308F));
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.07692F));
			this.tableLayoutPanel7.Controls.Add(this.btnSelectPreset, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.btnResetPictureSettings, 0, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 341);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 1;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(260, 55);
			this.tableLayoutPanel7.TabIndex = 3;
			// 
			// btnSelectPreset
			// 
			this.btnSelectPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectPreset.AutoSize = true;
			this.btnSelectPreset.Image = global::Mesen.GUI.Properties.Resources.DownArrow;
			this.btnSelectPreset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSelectPreset.Location = new System.Drawing.Point(158, 29);
			this.btnSelectPreset.Name = "btnSelectPreset";
			this.btnSelectPreset.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.btnSelectPreset.Size = new System.Drawing.Size(99, 23);
			this.btnSelectPreset.TabIndex = 3;
			this.btnSelectPreset.Text = "Select Preset...";
			this.btnSelectPreset.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnSelectPreset.UseVisualStyleBackColor = true;
			this.btnSelectPreset.Click += new System.EventHandler(this.btnSelectPreset_Click);
			// 
			// btnResetPictureSettings
			// 
			this.btnResetPictureSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnResetPictureSettings.AutoSize = true;
			this.btnResetPictureSettings.Location = new System.Drawing.Point(3, 29);
			this.btnResetPictureSettings.Name = "btnResetPictureSettings";
			this.btnResetPictureSettings.Size = new System.Drawing.Size(75, 23);
			this.btnResetPictureSettings.TabIndex = 3;
			this.btnResetPictureSettings.Text = "Reset";
			this.btnResetPictureSettings.UseVisualStyleBackColor = true;
			this.btnResetPictureSettings.Click += new System.EventHandler(this.btnResetPictureSettings_Click);
			// 
			// grpNtscFilter
			// 
			this.grpNtscFilter.Controls.Add(this.tlpNtscFilter2);
			this.grpNtscFilter.Controls.Add(this.tlpNtscFilter1);
			this.grpNtscFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpNtscFilter.Location = new System.Drawing.Point(262, 27);
			this.grpNtscFilter.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.grpNtscFilter.Name = "grpNtscFilter";
			this.tableLayoutPanel5.SetRowSpan(this.grpNtscFilter, 3);
			this.grpNtscFilter.Size = new System.Drawing.Size(259, 369);
			this.grpNtscFilter.TabIndex = 4;
			this.grpNtscFilter.TabStop = false;
			this.grpNtscFilter.Text = "NTSC Filter";
			// 
			// tlpNtscFilter2
			// 
			this.tlpNtscFilter2.ColumnCount = 1;
			this.tlpNtscFilter2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpNtscFilter2.Controls.Add(this.trkYFilterLength, 0, 0);
			this.tlpNtscFilter2.Controls.Add(this.trkIFilterLength, 0, 1);
			this.tlpNtscFilter2.Controls.Add(this.trkQFilterLength, 0, 2);
			this.tlpNtscFilter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpNtscFilter2.Location = new System.Drawing.Point(3, 16);
			this.tlpNtscFilter2.Margin = new System.Windows.Forms.Padding(0);
			this.tlpNtscFilter2.Name = "tlpNtscFilter2";
			this.tlpNtscFilter2.RowCount = 7;
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpNtscFilter2.Size = new System.Drawing.Size(253, 298);
			this.tlpNtscFilter2.TabIndex = 6;
			// 
			// trkYFilterLength
			// 
			this.trkYFilterLength.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkYFilterLength.Location = new System.Drawing.Point(0, 0);
			this.trkYFilterLength.Margin = new System.Windows.Forms.Padding(0);
			this.trkYFilterLength.Maximum = 400;
			this.trkYFilterLength.MaximumSize = new System.Drawing.Size(0, 60);
			this.trkYFilterLength.Minimum = -50;
			this.trkYFilterLength.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkYFilterLength.Name = "trkYFilterLength";
			this.trkYFilterLength.Size = new System.Drawing.Size(253, 50);
			this.trkYFilterLength.TabIndex = 24;
			this.trkYFilterLength.Text = "Y Filter (Horizontal Blur)";
			this.trkYFilterLength.Value = 0;
			// 
			// trkIFilterLength
			// 
			this.trkIFilterLength.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkIFilterLength.Location = new System.Drawing.Point(0, 50);
			this.trkIFilterLength.Margin = new System.Windows.Forms.Padding(0);
			this.trkIFilterLength.Maximum = 400;
			this.trkIFilterLength.MaximumSize = new System.Drawing.Size(400, 55);
			this.trkIFilterLength.Minimum = 0;
			this.trkIFilterLength.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkIFilterLength.Name = "trkIFilterLength";
			this.trkIFilterLength.Size = new System.Drawing.Size(253, 50);
			this.trkIFilterLength.TabIndex = 25;
			this.trkIFilterLength.Text = "I Filter (Horizontal Bleed)";
			this.trkIFilterLength.Value = 0;
			// 
			// trkQFilterLength
			// 
			this.trkQFilterLength.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkQFilterLength.Location = new System.Drawing.Point(0, 100);
			this.trkQFilterLength.Margin = new System.Windows.Forms.Padding(0);
			this.trkQFilterLength.Maximum = 400;
			this.trkQFilterLength.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkQFilterLength.Minimum = 0;
			this.trkQFilterLength.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkQFilterLength.Name = "trkQFilterLength";
			this.trkQFilterLength.Size = new System.Drawing.Size(253, 50);
			this.trkQFilterLength.TabIndex = 26;
			this.trkQFilterLength.Text = "Q Filter (Horizontal Bleed)";
			this.trkQFilterLength.Value = 0;
			// 
			// tlpNtscFilter1
			// 
			this.tlpNtscFilter1.ColumnCount = 1;
			this.tlpNtscFilter1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpNtscFilter1.Controls.Add(this.trkArtifacts, 0, 0);
			this.tlpNtscFilter1.Controls.Add(this.trkBleed, 0, 1);
			this.tlpNtscFilter1.Controls.Add(this.trkFringing, 0, 2);
			this.tlpNtscFilter1.Controls.Add(this.trkGamma, 0, 3);
			this.tlpNtscFilter1.Controls.Add(this.trkResolution, 0, 4);
			this.tlpNtscFilter1.Controls.Add(this.trkSharpness, 0, 5);
			this.tlpNtscFilter1.Controls.Add(this.tableLayoutPanel6, 0, 6);
			this.tlpNtscFilter1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpNtscFilter1.Location = new System.Drawing.Point(3, 16);
			this.tlpNtscFilter1.Margin = new System.Windows.Forms.Padding(0);
			this.tlpNtscFilter1.Name = "tlpNtscFilter1";
			this.tlpNtscFilter1.RowCount = 7;
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNtscFilter1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpNtscFilter1.Size = new System.Drawing.Size(253, 350);
			this.tlpNtscFilter1.TabIndex = 5;
			// 
			// trkArtifacts
			// 
			this.trkArtifacts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkArtifacts.Location = new System.Drawing.Point(0, 0);
			this.trkArtifacts.Margin = new System.Windows.Forms.Padding(0);
			this.trkArtifacts.Maximum = 100;
			this.trkArtifacts.MaximumSize = new System.Drawing.Size(0, 60);
			this.trkArtifacts.Minimum = -100;
			this.trkArtifacts.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkArtifacts.Name = "trkArtifacts";
			this.trkArtifacts.Size = new System.Drawing.Size(253, 50);
			this.trkArtifacts.TabIndex = 24;
			this.trkArtifacts.Text = "Artifacts";
			this.trkArtifacts.Value = 0;
			// 
			// trkBleed
			// 
			this.trkBleed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBleed.Location = new System.Drawing.Point(0, 50);
			this.trkBleed.Margin = new System.Windows.Forms.Padding(0);
			this.trkBleed.Maximum = 100;
			this.trkBleed.MaximumSize = new System.Drawing.Size(400, 55);
			this.trkBleed.Minimum = -100;
			this.trkBleed.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkBleed.Name = "trkBleed";
			this.trkBleed.Size = new System.Drawing.Size(253, 50);
			this.trkBleed.TabIndex = 25;
			this.trkBleed.Text = "Bleed";
			this.trkBleed.Value = 0;
			// 
			// trkFringing
			// 
			this.trkFringing.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkFringing.Location = new System.Drawing.Point(0, 100);
			this.trkFringing.Margin = new System.Windows.Forms.Padding(0);
			this.trkFringing.Maximum = 100;
			this.trkFringing.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkFringing.Minimum = -100;
			this.trkFringing.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkFringing.Name = "trkFringing";
			this.trkFringing.Size = new System.Drawing.Size(253, 50);
			this.trkFringing.TabIndex = 26;
			this.trkFringing.Text = "Fringing";
			this.trkFringing.Value = 0;
			// 
			// trkGamma
			// 
			this.trkGamma.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkGamma.Location = new System.Drawing.Point(0, 150);
			this.trkGamma.Margin = new System.Windows.Forms.Padding(0);
			this.trkGamma.Maximum = 100;
			this.trkGamma.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkGamma.Minimum = -100;
			this.trkGamma.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkGamma.Name = "trkGamma";
			this.trkGamma.Size = new System.Drawing.Size(253, 50);
			this.trkGamma.TabIndex = 27;
			this.trkGamma.Text = "Gamma";
			this.trkGamma.Value = 0;
			// 
			// trkResolution
			// 
			this.trkResolution.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkResolution.Location = new System.Drawing.Point(0, 200);
			this.trkResolution.Margin = new System.Windows.Forms.Padding(0);
			this.trkResolution.Maximum = 100;
			this.trkResolution.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkResolution.Minimum = -100;
			this.trkResolution.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkResolution.Name = "trkResolution";
			this.trkResolution.Size = new System.Drawing.Size(253, 50);
			this.trkResolution.TabIndex = 28;
			this.trkResolution.Text = "Resolution";
			this.trkResolution.Value = 0;
			// 
			// trkSharpness
			// 
			this.trkSharpness.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkSharpness.Location = new System.Drawing.Point(0, 250);
			this.trkSharpness.Margin = new System.Windows.Forms.Padding(0);
			this.trkSharpness.Maximum = 100;
			this.trkSharpness.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkSharpness.Minimum = -100;
			this.trkSharpness.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkSharpness.Name = "trkSharpness";
			this.trkSharpness.Size = new System.Drawing.Size(253, 50);
			this.trkSharpness.TabIndex = 29;
			this.trkSharpness.Text = "Sharpness";
			this.trkSharpness.Value = 0;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add(this.chkMergeFields, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.chkVerticalBlend, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 300);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(253, 50);
			this.tableLayoutPanel6.TabIndex = 30;
			// 
			// chkMergeFields
			// 
			this.chkMergeFields.AutoSize = true;
			this.chkMergeFields.Location = new System.Drawing.Point(3, 3);
			this.chkMergeFields.Name = "chkMergeFields";
			this.chkMergeFields.Size = new System.Drawing.Size(86, 17);
			this.chkMergeFields.TabIndex = 30;
			this.chkMergeFields.Text = "Merge Fields";
			this.chkMergeFields.UseVisualStyleBackColor = true;
			// 
			// chkVerticalBlend
			// 
			this.chkVerticalBlend.AutoSize = true;
			this.chkVerticalBlend.Location = new System.Drawing.Point(3, 28);
			this.chkVerticalBlend.Name = "chkVerticalBlend";
			this.chkVerticalBlend.Size = new System.Drawing.Size(134, 17);
			this.chkVerticalBlend.TabIndex = 31;
			this.chkVerticalBlend.Text = "Apply Vertical Blending";
			this.chkVerticalBlend.UseVisualStyleBackColor = true;
			// 
			// grpCommon
			// 
			this.grpCommon.Controls.Add(this.tableLayoutPanel4);
			this.grpCommon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCommon.Location = new System.Drawing.Point(0, 27);
			this.grpCommon.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.grpCommon.Name = "grpCommon";
			this.grpCommon.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.grpCommon.Size = new System.Drawing.Size(258, 242);
			this.grpCommon.TabIndex = 3;
			this.grpCommon.TabStop = false;
			this.grpCommon.Text = "Common Settings";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Controls.Add(this.chkBilinearInterpolation, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.trkBrightness, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.trkContrast, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.trkHue, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.trkSaturation, 0, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 15);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 5;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(252, 225);
			this.tableLayoutPanel4.TabIndex = 4;
			// 
			// chkBilinearInterpolation
			// 
			this.chkBilinearInterpolation.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.chkBilinearInterpolation, 2);
			this.chkBilinearInterpolation.Location = new System.Drawing.Point(3, 203);
			this.chkBilinearInterpolation.Name = "chkBilinearInterpolation";
			this.chkBilinearInterpolation.Size = new System.Drawing.Size(206, 17);
			this.chkBilinearInterpolation.TabIndex = 28;
			this.chkBilinearInterpolation.Text = "Use bilinear interpolation when scaling";
			this.chkBilinearInterpolation.UseVisualStyleBackColor = true;
			// 
			// trkBrightness
			// 
			this.trkBrightness.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBrightness.Location = new System.Drawing.Point(0, 0);
			this.trkBrightness.Margin = new System.Windows.Forms.Padding(0);
			this.trkBrightness.Maximum = 100;
			this.trkBrightness.MaximumSize = new System.Drawing.Size(0, 60);
			this.trkBrightness.Minimum = -100;
			this.trkBrightness.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkBrightness.Name = "trkBrightness";
			this.trkBrightness.Size = new System.Drawing.Size(252, 50);
			this.trkBrightness.TabIndex = 24;
			this.trkBrightness.Text = "Brightness";
			this.trkBrightness.Value = 0;
			// 
			// trkContrast
			// 
			this.trkContrast.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkContrast.Location = new System.Drawing.Point(0, 50);
			this.trkContrast.Margin = new System.Windows.Forms.Padding(0);
			this.trkContrast.Maximum = 100;
			this.trkContrast.MaximumSize = new System.Drawing.Size(400, 55);
			this.trkContrast.Minimum = -100;
			this.trkContrast.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkContrast.Name = "trkContrast";
			this.trkContrast.Size = new System.Drawing.Size(252, 50);
			this.trkContrast.TabIndex = 25;
			this.trkContrast.Text = "Contrast";
			this.trkContrast.Value = 0;
			// 
			// trkHue
			// 
			this.trkHue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkHue.Location = new System.Drawing.Point(0, 100);
			this.trkHue.Margin = new System.Windows.Forms.Padding(0);
			this.trkHue.Maximum = 100;
			this.trkHue.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkHue.Minimum = -100;
			this.trkHue.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkHue.Name = "trkHue";
			this.trkHue.Size = new System.Drawing.Size(252, 50);
			this.trkHue.TabIndex = 26;
			this.trkHue.Text = "Hue";
			this.trkHue.Value = 0;
			// 
			// trkSaturation
			// 
			this.trkSaturation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkSaturation.Location = new System.Drawing.Point(0, 150);
			this.trkSaturation.Margin = new System.Windows.Forms.Padding(0);
			this.trkSaturation.Maximum = 100;
			this.trkSaturation.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkSaturation.Minimum = -100;
			this.trkSaturation.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkSaturation.Name = "trkSaturation";
			this.trkSaturation.Size = new System.Drawing.Size(252, 50);
			this.trkSaturation.TabIndex = 27;
			this.trkSaturation.Text = "Saturation";
			this.trkSaturation.Value = 0;
			// 
			// grpScanlines
			// 
			this.grpScanlines.Controls.Add(this.trkScanlines);
			this.grpScanlines.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpScanlines.Location = new System.Drawing.Point(0, 269);
			this.grpScanlines.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.grpScanlines.Name = "grpScanlines";
			this.grpScanlines.Size = new System.Drawing.Size(258, 72);
			this.grpScanlines.TabIndex = 5;
			this.grpScanlines.TabStop = false;
			this.grpScanlines.Text = "Scanlines";
			// 
			// trkScanlines
			// 
			this.trkScanlines.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkScanlines.Location = new System.Drawing.Point(3, 16);
			this.trkScanlines.Margin = new System.Windows.Forms.Padding(0);
			this.trkScanlines.Maximum = 100;
			this.trkScanlines.MaximumSize = new System.Drawing.Size(0, 41);
			this.trkScanlines.Minimum = 0;
			this.trkScanlines.MinimumSize = new System.Drawing.Size(206, 50);
			this.trkScanlines.Name = "trkScanlines";
			this.trkScanlines.Size = new System.Drawing.Size(252, 50);
			this.trkScanlines.TabIndex = 28;
			this.trkScanlines.Text = "Scanlines";
			this.trkScanlines.Value = 0;
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel5.SetColumnSpan(this.tableLayoutPanel8, 2);
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.cboFilter, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.lblVideoFilter, 0, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 1;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(521, 27);
			this.tableLayoutPanel8.TabIndex = 6;
			// 
			// cboFilter
			// 
			this.cboFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFilter.FormattingEnabled = true;
			this.cboFilter.Items.AddRange(new object[] {
            "None",
            "NTSC"});
			this.cboFilter.Location = new System.Drawing.Point(41, 3);
			this.cboFilter.Name = "cboFilter";
			this.cboFilter.Size = new System.Drawing.Size(477, 21);
			this.cboFilter.TabIndex = 15;
			// 
			// lblVideoFilter
			// 
			this.lblVideoFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVideoFilter.AutoSize = true;
			this.lblVideoFilter.Location = new System.Drawing.Point(3, 7);
			this.lblVideoFilter.Name = "lblVideoFilter";
			this.lblVideoFilter.Size = new System.Drawing.Size(32, 13);
			this.lblVideoFilter.TabIndex = 13;
			this.lblVideoFilter.Text = "Filter:";
			// 
			// tpgOverscan
			// 
			this.tpgOverscan.Controls.Add(this.tabOverscan);
			this.tpgOverscan.Location = new System.Drawing.Point(4, 22);
			this.tpgOverscan.Name = "tpgOverscan";
			this.tpgOverscan.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverscan.Size = new System.Drawing.Size(527, 402);
			this.tpgOverscan.TabIndex = 1;
			this.tpgOverscan.Text = "Overscan";
			this.tpgOverscan.UseVisualStyleBackColor = true;
			// 
			// tabOverscan
			// 
			this.tabOverscan.Controls.Add(this.tpgOverscanGlobal);
			this.tabOverscan.Controls.Add(this.tpgOverscanGameSpecific);
			this.tabOverscan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabOverscan.ImageList = this.imageList;
			this.tabOverscan.Location = new System.Drawing.Point(3, 3);
			this.tabOverscan.Name = "tabOverscan";
			this.tabOverscan.SelectedIndex = 0;
			this.tabOverscan.Size = new System.Drawing.Size(521, 396);
			this.tabOverscan.TabIndex = 1;
			// 
			// tpgOverscanGlobal
			// 
			this.tpgOverscanGlobal.Controls.Add(this.tableLayoutPanel1);
			this.tpgOverscanGlobal.Location = new System.Drawing.Point(4, 23);
			this.tpgOverscanGlobal.Name = "tpgOverscanGlobal";
			this.tpgOverscanGlobal.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverscanGlobal.Size = new System.Drawing.Size(513, 369);
			this.tpgOverscanGlobal.TabIndex = 0;
			this.tpgOverscanGlobal.Text = "Global";
			this.tpgOverscanGlobal.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 262F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.picOverscan, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel11, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel12, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel13, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel14, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 246F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(507, 363);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// picOverscan
			// 
			this.picOverscan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picOverscan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picOverscan.Location = new System.Drawing.Point(125, 61);
			this.picOverscan.Name = "picOverscan";
			this.picOverscan.Size = new System.Drawing.Size(256, 240);
			this.picOverscan.TabIndex = 1;
			this.picOverscan.TabStop = false;
			// 
			// tableLayoutPanel11
			// 
			this.tableLayoutPanel11.ColumnCount = 1;
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Controls.Add(this.nudOverscanTop, 0, 1);
			this.tableLayoutPanel11.Controls.Add(this.lblTop, 0, 0);
			this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel11.Location = new System.Drawing.Point(122, 0);
			this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			this.tableLayoutPanel11.RowCount = 2;
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel11.Size = new System.Drawing.Size(262, 58);
			this.tableLayoutPanel11.TabIndex = 4;
			// 
			// nudOverscanTop
			// 
			this.nudOverscanTop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.nudOverscanTop.DecimalPlaces = 0;
			this.nudOverscanTop.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverscanTop.Location = new System.Drawing.Point(110, 37);
			this.nudOverscanTop.Margin = new System.Windows.Forms.Padding(0);
			this.nudOverscanTop.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudOverscanTop.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudOverscanTop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanTop.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudOverscanTop.Name = "nudOverscanTop";
			this.nudOverscanTop.Size = new System.Drawing.Size(41, 21);
			this.nudOverscanTop.TabIndex = 2;
			this.nudOverscanTop.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanTop.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// lblTop
			// 
			this.lblTop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblTop.AutoSize = true;
			this.lblTop.Location = new System.Drawing.Point(118, 24);
			this.lblTop.Name = "lblTop";
			this.lblTop.Size = new System.Drawing.Size(26, 13);
			this.lblTop.TabIndex = 0;
			this.lblTop.Text = "Top";
			this.lblTop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel12
			// 
			this.tableLayoutPanel12.ColumnCount = 1;
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel12.Controls.Add(this.nudOverscanBottom, 0, 1);
			this.tableLayoutPanel12.Controls.Add(this.lblBottom, 0, 0);
			this.tableLayoutPanel12.Location = new System.Drawing.Point(122, 304);
			this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel12.Name = "tableLayoutPanel12";
			this.tableLayoutPanel12.RowCount = 2;
			this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel12.Size = new System.Drawing.Size(262, 58);
			this.tableLayoutPanel12.TabIndex = 5;
			// 
			// nudOverscanBottom
			// 
			this.nudOverscanBottom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudOverscanBottom.DecimalPlaces = 0;
			this.nudOverscanBottom.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverscanBottom.Location = new System.Drawing.Point(110, 13);
			this.nudOverscanBottom.Margin = new System.Windows.Forms.Padding(0);
			this.nudOverscanBottom.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudOverscanBottom.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudOverscanBottom.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanBottom.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudOverscanBottom.Name = "nudOverscanBottom";
			this.nudOverscanBottom.Size = new System.Drawing.Size(41, 21);
			this.nudOverscanBottom.TabIndex = 2;
			this.nudOverscanBottom.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanBottom.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// lblBottom
			// 
			this.lblBottom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblBottom.AutoSize = true;
			this.lblBottom.Location = new System.Drawing.Point(111, 0);
			this.lblBottom.Name = "lblBottom";
			this.lblBottom.Size = new System.Drawing.Size(40, 13);
			this.lblBottom.TabIndex = 0;
			this.lblBottom.Text = "Bottom";
			this.lblBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel13
			// 
			this.tableLayoutPanel13.ColumnCount = 2;
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel13.Controls.Add(this.nudOverscanRight, 0, 2);
			this.tableLayoutPanel13.Controls.Add(this.lblRight, 0, 1);
			this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel13.Location = new System.Drawing.Point(384, 58);
			this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel13.Name = "tableLayoutPanel13";
			this.tableLayoutPanel13.RowCount = 4;
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel13.Size = new System.Drawing.Size(123, 246);
			this.tableLayoutPanel13.TabIndex = 6;
			// 
			// nudOverscanRight
			// 
			this.nudOverscanRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudOverscanRight.DecimalPlaces = 0;
			this.nudOverscanRight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverscanRight.Location = new System.Drawing.Point(0, 119);
			this.nudOverscanRight.Margin = new System.Windows.Forms.Padding(0);
			this.nudOverscanRight.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudOverscanRight.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudOverscanRight.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanRight.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudOverscanRight.Name = "nudOverscanRight";
			this.nudOverscanRight.Size = new System.Drawing.Size(41, 21);
			this.nudOverscanRight.TabIndex = 1;
			this.nudOverscanRight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanRight.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// lblRight
			// 
			this.lblRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblRight.AutoSize = true;
			this.lblRight.Location = new System.Drawing.Point(4, 106);
			this.lblRight.Name = "lblRight";
			this.lblRight.Size = new System.Drawing.Size(32, 13);
			this.lblRight.TabIndex = 0;
			this.lblRight.Text = "Right";
			this.lblRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel14
			// 
			this.tableLayoutPanel14.ColumnCount = 2;
			this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel14.Controls.Add(this.nudOverscanLeft, 1, 2);
			this.tableLayoutPanel14.Controls.Add(this.lblLeft, 1, 1);
			this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 58);
			this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel14.Name = "tableLayoutPanel14";
			this.tableLayoutPanel14.RowCount = 4;
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel14.Size = new System.Drawing.Size(122, 246);
			this.tableLayoutPanel14.TabIndex = 7;
			// 
			// nudOverscanLeft
			// 
			this.nudOverscanLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudOverscanLeft.DecimalPlaces = 0;
			this.nudOverscanLeft.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverscanLeft.Location = new System.Drawing.Point(81, 119);
			this.nudOverscanLeft.Margin = new System.Windows.Forms.Padding(0);
			this.nudOverscanLeft.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudOverscanLeft.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudOverscanLeft.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanLeft.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudOverscanLeft.Name = "nudOverscanLeft";
			this.nudOverscanLeft.Size = new System.Drawing.Size(41, 21);
			this.nudOverscanLeft.TabIndex = 2;
			this.nudOverscanLeft.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudOverscanLeft.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// lblLeft
			// 
			this.lblLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblLeft.AutoSize = true;
			this.lblLeft.Location = new System.Drawing.Point(89, 106);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(25, 13);
			this.lblLeft.TabIndex = 0;
			this.lblLeft.Text = "Left";
			this.lblLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tpgOverscanGameSpecific
			// 
			this.tpgOverscanGameSpecific.Controls.Add(this.groupBox1);
			this.tpgOverscanGameSpecific.Location = new System.Drawing.Point(4, 23);
			this.tpgOverscanGameSpecific.Name = "tpgOverscanGameSpecific";
			this.tpgOverscanGameSpecific.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverscanGameSpecific.Size = new System.Drawing.Size(513, 369);
			this.tpgOverscanGameSpecific.TabIndex = 1;
			this.tpgOverscanGameSpecific.Text = "Game-Specific";
			this.tpgOverscanGameSpecific.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tableLayoutPanel10);
			this.groupBox1.Controls.Add(this.chkEnableGameSpecificOverscan);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.groupBox1.Size = new System.Drawing.Size(507, 363);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 3;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 262F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.Controls.Add(this.picGameSpecificOverscan, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel15, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel16, 1, 2);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel17, 2, 1);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel18, 0, 1);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 18);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 3;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 246F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(501, 342);
			this.tableLayoutPanel10.TabIndex = 1;
			// 
			// picGameSpecificOverscan
			// 
			this.picGameSpecificOverscan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picGameSpecificOverscan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picGameSpecificOverscan.Location = new System.Drawing.Point(122, 51);
			this.picGameSpecificOverscan.Name = "picGameSpecificOverscan";
			this.picGameSpecificOverscan.Size = new System.Drawing.Size(256, 240);
			this.picGameSpecificOverscan.TabIndex = 1;
			this.picGameSpecificOverscan.TabStop = false;
			// 
			// tableLayoutPanel15
			// 
			this.tableLayoutPanel15.ColumnCount = 1;
			this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel15.Controls.Add(this.nudGameSpecificOverscanTop, 0, 1);
			this.tableLayoutPanel15.Controls.Add(this.lblGameSpecificOverscanTop, 0, 0);
			this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel15.Location = new System.Drawing.Point(119, 0);
			this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel15.Name = "tableLayoutPanel15";
			this.tableLayoutPanel15.RowCount = 2;
			this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel15.Size = new System.Drawing.Size(262, 48);
			this.tableLayoutPanel15.TabIndex = 4;
			// 
			// nudGameSpecificOverscanTop
			// 
			this.nudGameSpecificOverscanTop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.nudGameSpecificOverscanTop.DecimalPlaces = 0;
			this.nudGameSpecificOverscanTop.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudGameSpecificOverscanTop.Location = new System.Drawing.Point(110, 27);
			this.nudGameSpecificOverscanTop.Margin = new System.Windows.Forms.Padding(0);
			this.nudGameSpecificOverscanTop.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudGameSpecificOverscanTop.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudGameSpecificOverscanTop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudGameSpecificOverscanTop.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudGameSpecificOverscanTop.Name = "nudGameSpecificOverscanTop";
			this.nudGameSpecificOverscanTop.Size = new System.Drawing.Size(41, 21);
			this.nudGameSpecificOverscanTop.TabIndex = 2;
			this.nudGameSpecificOverscanTop.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblGameSpecificOverscanTop
			// 
			this.lblGameSpecificOverscanTop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblGameSpecificOverscanTop.AutoSize = true;
			this.lblGameSpecificOverscanTop.Location = new System.Drawing.Point(118, 14);
			this.lblGameSpecificOverscanTop.Name = "lblGameSpecificOverscanTop";
			this.lblGameSpecificOverscanTop.Size = new System.Drawing.Size(26, 13);
			this.lblGameSpecificOverscanTop.TabIndex = 0;
			this.lblGameSpecificOverscanTop.Text = "Top";
			this.lblGameSpecificOverscanTop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel16
			// 
			this.tableLayoutPanel16.ColumnCount = 1;
			this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel16.Controls.Add(this.nudGameSpecificOverscanBottom, 0, 1);
			this.tableLayoutPanel16.Controls.Add(this.lblGameSpecificOverscanBottom, 0, 0);
			this.tableLayoutPanel16.Location = new System.Drawing.Point(119, 294);
			this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel16.Name = "tableLayoutPanel16";
			this.tableLayoutPanel16.RowCount = 2;
			this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel16.Size = new System.Drawing.Size(262, 48);
			this.tableLayoutPanel16.TabIndex = 5;
			// 
			// nudGameSpecificOverscanBottom
			// 
			this.nudGameSpecificOverscanBottom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudGameSpecificOverscanBottom.DecimalPlaces = 0;
			this.nudGameSpecificOverscanBottom.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudGameSpecificOverscanBottom.Location = new System.Drawing.Point(110, 13);
			this.nudGameSpecificOverscanBottom.Margin = new System.Windows.Forms.Padding(0);
			this.nudGameSpecificOverscanBottom.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudGameSpecificOverscanBottom.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudGameSpecificOverscanBottom.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudGameSpecificOverscanBottom.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudGameSpecificOverscanBottom.Name = "nudGameSpecificOverscanBottom";
			this.nudGameSpecificOverscanBottom.Size = new System.Drawing.Size(41, 21);
			this.nudGameSpecificOverscanBottom.TabIndex = 2;
			this.nudGameSpecificOverscanBottom.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblGameSpecificOverscanBottom
			// 
			this.lblGameSpecificOverscanBottom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblGameSpecificOverscanBottom.AutoSize = true;
			this.lblGameSpecificOverscanBottom.Location = new System.Drawing.Point(111, 0);
			this.lblGameSpecificOverscanBottom.Name = "lblGameSpecificOverscanBottom";
			this.lblGameSpecificOverscanBottom.Size = new System.Drawing.Size(40, 13);
			this.lblGameSpecificOverscanBottom.TabIndex = 0;
			this.lblGameSpecificOverscanBottom.Text = "Bottom";
			this.lblGameSpecificOverscanBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel17
			// 
			this.tableLayoutPanel17.ColumnCount = 2;
			this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel17.Controls.Add(this.nudGameSpecificOverscanRight, 0, 2);
			this.tableLayoutPanel17.Controls.Add(this.lblGameSpecificOverscanRight, 0, 1);
			this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel17.Location = new System.Drawing.Point(381, 48);
			this.tableLayoutPanel17.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel17.Name = "tableLayoutPanel17";
			this.tableLayoutPanel17.RowCount = 4;
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel17.Size = new System.Drawing.Size(120, 246);
			this.tableLayoutPanel17.TabIndex = 6;
			// 
			// nudGameSpecificOverscanRight
			// 
			this.nudGameSpecificOverscanRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudGameSpecificOverscanRight.DecimalPlaces = 0;
			this.nudGameSpecificOverscanRight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudGameSpecificOverscanRight.Location = new System.Drawing.Point(0, 119);
			this.nudGameSpecificOverscanRight.Margin = new System.Windows.Forms.Padding(0);
			this.nudGameSpecificOverscanRight.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudGameSpecificOverscanRight.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudGameSpecificOverscanRight.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudGameSpecificOverscanRight.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudGameSpecificOverscanRight.Name = "nudGameSpecificOverscanRight";
			this.nudGameSpecificOverscanRight.Size = new System.Drawing.Size(41, 21);
			this.nudGameSpecificOverscanRight.TabIndex = 1;
			this.nudGameSpecificOverscanRight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblGameSpecificOverscanRight
			// 
			this.lblGameSpecificOverscanRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblGameSpecificOverscanRight.AutoSize = true;
			this.lblGameSpecificOverscanRight.Location = new System.Drawing.Point(4, 106);
			this.lblGameSpecificOverscanRight.Name = "lblGameSpecificOverscanRight";
			this.lblGameSpecificOverscanRight.Size = new System.Drawing.Size(32, 13);
			this.lblGameSpecificOverscanRight.TabIndex = 0;
			this.lblGameSpecificOverscanRight.Text = "Right";
			this.lblGameSpecificOverscanRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel18
			// 
			this.tableLayoutPanel18.ColumnCount = 2;
			this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel18.Controls.Add(this.nudGameSpecificOverscanLeft, 1, 2);
			this.tableLayoutPanel18.Controls.Add(this.lblGameSpecificOverscanLeft, 1, 1);
			this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel18.Location = new System.Drawing.Point(0, 48);
			this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel18.Name = "tableLayoutPanel18";
			this.tableLayoutPanel18.RowCount = 4;
			this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel18.Size = new System.Drawing.Size(119, 246);
			this.tableLayoutPanel18.TabIndex = 7;
			// 
			// nudGameSpecificOverscanLeft
			// 
			this.nudGameSpecificOverscanLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.nudGameSpecificOverscanLeft.DecimalPlaces = 0;
			this.nudGameSpecificOverscanLeft.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudGameSpecificOverscanLeft.Location = new System.Drawing.Point(78, 119);
			this.nudGameSpecificOverscanLeft.Margin = new System.Windows.Forms.Padding(0);
			this.nudGameSpecificOverscanLeft.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudGameSpecificOverscanLeft.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudGameSpecificOverscanLeft.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudGameSpecificOverscanLeft.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudGameSpecificOverscanLeft.Name = "nudGameSpecificOverscanLeft";
			this.nudGameSpecificOverscanLeft.Size = new System.Drawing.Size(41, 21);
			this.nudGameSpecificOverscanLeft.TabIndex = 2;
			this.nudGameSpecificOverscanLeft.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblGameSpecificOverscanLeft
			// 
			this.lblGameSpecificOverscanLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblGameSpecificOverscanLeft.AutoSize = true;
			this.lblGameSpecificOverscanLeft.Location = new System.Drawing.Point(86, 106);
			this.lblGameSpecificOverscanLeft.Name = "lblGameSpecificOverscanLeft";
			this.lblGameSpecificOverscanLeft.Size = new System.Drawing.Size(25, 13);
			this.lblGameSpecificOverscanLeft.TabIndex = 0;
			this.lblGameSpecificOverscanLeft.Text = "Left";
			this.lblGameSpecificOverscanLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// chkEnableGameSpecificOverscan
			// 
			this.chkEnableGameSpecificOverscan.AutoSize = true;
			this.chkEnableGameSpecificOverscan.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.chkEnableGameSpecificOverscan.Location = new System.Drawing.Point(6, -1);
			this.chkEnableGameSpecificOverscan.Name = "chkEnableGameSpecificOverscan";
			this.chkEnableGameSpecificOverscan.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.chkEnableGameSpecificOverscan.Size = new System.Drawing.Size(218, 17);
			this.chkEnableGameSpecificOverscan.TabIndex = 0;
			this.chkEnableGameSpecificOverscan.Text = "Enable game-specific overscan settings";
			this.chkEnableGameSpecificOverscan.UseVisualStyleBackColor = false;
			this.chkEnableGameSpecificOverscan.CheckedChanged += new System.EventHandler(this.chkEnableGameSpecificOverscan_CheckedChanged);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Exclamation.png");
			// 
			// tpgPalette
			// 
			this.tpgPalette.Controls.Add(this.tableLayoutPanel3);
			this.tpgPalette.Location = new System.Drawing.Point(4, 22);
			this.tpgPalette.Name = "tpgPalette";
			this.tpgPalette.Padding = new System.Windows.Forms.Padding(3);
			this.tpgPalette.Size = new System.Drawing.Size(527, 402);
			this.tpgPalette.TabIndex = 2;
			this.tpgPalette.Text = "Palette";
			this.tpgPalette.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.chkUseCustomVsPalette, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.ctrlPaletteDisplay, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(521, 396);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.btnExportPalette, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.btnSelectPalette, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnLoadPalFile, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowColorIndexes, 0, 3);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(344, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(181, 344);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// btnExportPalette
			// 
			this.btnExportPalette.AutoSize = true;
			this.btnExportPalette.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnExportPalette.Location = new System.Drawing.Point(3, 61);
			this.btnExportPalette.Name = "btnExportPalette";
			this.btnExportPalette.Size = new System.Drawing.Size(175, 23);
			this.btnExportPalette.TabIndex = 3;
			this.btnExportPalette.Text = "Export Palette";
			this.btnExportPalette.UseVisualStyleBackColor = true;
			this.btnExportPalette.Click += new System.EventHandler(this.btnExportPalette_Click);
			// 
			// btnSelectPalette
			// 
			this.btnSelectPalette.AutoSize = true;
			this.btnSelectPalette.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnSelectPalette.Image = global::Mesen.GUI.Properties.Resources.DownArrow;
			this.btnSelectPalette.Location = new System.Drawing.Point(3, 3);
			this.btnSelectPalette.Name = "btnSelectPalette";
			this.btnSelectPalette.Size = new System.Drawing.Size(175, 23);
			this.btnSelectPalette.TabIndex = 2;
			this.btnSelectPalette.Text = "Load Preset Palette...";
			this.btnSelectPalette.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnSelectPalette.UseVisualStyleBackColor = true;
			this.btnSelectPalette.Click += new System.EventHandler(this.btnSelectPalette_Click);
			// 
			// btnLoadPalFile
			// 
			this.btnLoadPalFile.AutoSize = true;
			this.btnLoadPalFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnLoadPalFile.Location = new System.Drawing.Point(3, 32);
			this.btnLoadPalFile.Name = "btnLoadPalFile";
			this.btnLoadPalFile.Size = new System.Drawing.Size(175, 23);
			this.btnLoadPalFile.TabIndex = 0;
			this.btnLoadPalFile.Text = "Load Palette File";
			this.btnLoadPalFile.UseVisualStyleBackColor = true;
			this.btnLoadPalFile.Click += new System.EventHandler(this.btnLoadPalFile_Click);
			// 
			// chkShowColorIndexes
			// 
			this.chkShowColorIndexes.AutoSize = true;
			this.chkShowColorIndexes.Location = new System.Drawing.Point(3, 90);
			this.chkShowColorIndexes.Name = "chkShowColorIndexes";
			this.chkShowColorIndexes.Size = new System.Drawing.Size(118, 17);
			this.chkShowColorIndexes.TabIndex = 4;
			this.chkShowColorIndexes.Text = "Show color indexes";
			this.chkShowColorIndexes.UseVisualStyleBackColor = true;
			this.chkShowColorIndexes.CheckedChanged += new System.EventHandler(this.chkShowColorIndexes_CheckedChanged);
			// 
			// chkUseCustomVsPalette
			// 
			this.chkUseCustomVsPalette.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.chkUseCustomVsPalette, 2);
			this.chkUseCustomVsPalette.Location = new System.Drawing.Point(3, 347);
			this.chkUseCustomVsPalette.Name = "chkUseCustomVsPalette";
			this.chkUseCustomVsPalette.Size = new System.Drawing.Size(202, 17);
			this.chkUseCustomVsPalette.TabIndex = 2;
			this.chkUseCustomVsPalette.Text = "Use this palette for VS System games";
			this.chkUseCustomVsPalette.UseVisualStyleBackColor = true;
			// 
			// ctrlPaletteDisplay
			// 
			this.ctrlPaletteDisplay.Location = new System.Drawing.Point(3, 3);
			this.ctrlPaletteDisplay.Name = "ctrlPaletteDisplay";
			this.ctrlPaletteDisplay.Size = new System.Drawing.Size(338, 338);
			this.ctrlPaletteDisplay.TabIndex = 3;
			this.ctrlPaletteDisplay.ColorClick += new Mesen.GUI.Debugger.ctrlPaletteDisplay.PaletteClickHandler(this.ctrlPaletteDisplay_ColorClick);
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel9);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(527, 402);
			this.tpgAdvanced.TabIndex = 4;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.ColumnCount = 2;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Controls.Add(this.chkDisableBackground, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.chkDisableSprites, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.chkForceBackgroundFirstColumn, 0, 3);
			this.tableLayoutPanel9.Controls.Add(this.chkForceSpritesFirstColumn, 0, 4);
			this.tableLayoutPanel9.Controls.Add(this.lblScreenRotation, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.cboScreenRotation, 1, 0);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 6;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(521, 396);
			this.tableLayoutPanel9.TabIndex = 0;
			// 
			// chkDisableBackground
			// 
			this.chkDisableBackground.Checked = false;
			this.tableLayoutPanel9.SetColumnSpan(this.chkDisableBackground, 2);
			this.chkDisableBackground.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisableBackground.Location = new System.Drawing.Point(0, 27);
			this.chkDisableBackground.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisableBackground.Name = "chkDisableBackground";
			this.chkDisableBackground.Size = new System.Drawing.Size(521, 23);
			this.chkDisableBackground.TabIndex = 0;
			this.chkDisableBackground.Text = "Disable background";
			// 
			// chkDisableSprites
			// 
			this.chkDisableSprites.Checked = false;
			this.tableLayoutPanel9.SetColumnSpan(this.chkDisableSprites, 2);
			this.chkDisableSprites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisableSprites.Location = new System.Drawing.Point(0, 50);
			this.chkDisableSprites.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisableSprites.Name = "chkDisableSprites";
			this.chkDisableSprites.Size = new System.Drawing.Size(521, 23);
			this.chkDisableSprites.TabIndex = 1;
			this.chkDisableSprites.Text = "Disable sprites";
			// 
			// chkForceBackgroundFirstColumn
			// 
			this.chkForceBackgroundFirstColumn.Checked = false;
			this.tableLayoutPanel9.SetColumnSpan(this.chkForceBackgroundFirstColumn, 2);
			this.chkForceBackgroundFirstColumn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkForceBackgroundFirstColumn.Location = new System.Drawing.Point(0, 73);
			this.chkForceBackgroundFirstColumn.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkForceBackgroundFirstColumn.Name = "chkForceBackgroundFirstColumn";
			this.chkForceBackgroundFirstColumn.Size = new System.Drawing.Size(521, 23);
			this.chkForceBackgroundFirstColumn.TabIndex = 2;
			this.chkForceBackgroundFirstColumn.Text = "Force background display in first column";
			// 
			// chkForceSpritesFirstColumn
			// 
			this.chkForceSpritesFirstColumn.Checked = false;
			this.tableLayoutPanel9.SetColumnSpan(this.chkForceSpritesFirstColumn, 2);
			this.chkForceSpritesFirstColumn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkForceSpritesFirstColumn.Location = new System.Drawing.Point(0, 96);
			this.chkForceSpritesFirstColumn.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkForceSpritesFirstColumn.Name = "chkForceSpritesFirstColumn";
			this.chkForceSpritesFirstColumn.Size = new System.Drawing.Size(521, 23);
			this.chkForceSpritesFirstColumn.TabIndex = 3;
			this.chkForceSpritesFirstColumn.Text = "Force sprite display in first column";
			// 
			// lblScreenRotation
			// 
			this.lblScreenRotation.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblScreenRotation.AutoSize = true;
			this.lblScreenRotation.Location = new System.Drawing.Point(3, 7);
			this.lblScreenRotation.Name = "lblScreenRotation";
			this.lblScreenRotation.Size = new System.Drawing.Size(87, 13);
			this.lblScreenRotation.TabIndex = 4;
			this.lblScreenRotation.Text = "Screen Rotation:";
			// 
			// cboScreenRotation
			// 
			this.cboScreenRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboScreenRotation.FormattingEnabled = true;
			this.cboScreenRotation.Location = new System.Drawing.Point(96, 3);
			this.cboScreenRotation.Name = "cboScreenRotation";
			this.cboScreenRotation.Size = new System.Drawing.Size(77, 21);
			this.cboScreenRotation.TabIndex = 5;
			// 
			// contextPicturePresets
			// 
			this.contextPicturePresets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPresetComposite,
            this.mnuPresetSVideo,
            this.mnuPresetRgb,
            this.mnuPresetMonochrome});
			this.contextPicturePresets.Name = "contextPicturePresets";
			this.contextPicturePresets.Size = new System.Drawing.Size(148, 92);
			// 
			// mnuPresetComposite
			// 
			this.mnuPresetComposite.Name = "mnuPresetComposite";
			this.mnuPresetComposite.Size = new System.Drawing.Size(147, 22);
			this.mnuPresetComposite.Text = "Composite";
			this.mnuPresetComposite.Click += new System.EventHandler(this.mnuPresetComposite_Click);
			// 
			// mnuPresetSVideo
			// 
			this.mnuPresetSVideo.Name = "mnuPresetSVideo";
			this.mnuPresetSVideo.Size = new System.Drawing.Size(147, 22);
			this.mnuPresetSVideo.Text = "S-Video";
			this.mnuPresetSVideo.Click += new System.EventHandler(this.mnuPresetSVideo_Click);
			// 
			// mnuPresetRgb
			// 
			this.mnuPresetRgb.Name = "mnuPresetRgb";
			this.mnuPresetRgb.Size = new System.Drawing.Size(147, 22);
			this.mnuPresetRgb.Text = "RGB";
			this.mnuPresetRgb.Click += new System.EventHandler(this.mnuPresetRgb_Click);
			// 
			// mnuPresetMonochrome
			// 
			this.mnuPresetMonochrome.Name = "mnuPresetMonochrome";
			this.mnuPresetMonochrome.Size = new System.Drawing.Size(147, 22);
			this.mnuPresetMonochrome.Text = "Monochrome";
			this.mnuPresetMonochrome.Click += new System.EventHandler(this.mnuPresetMonochrome_Click);
			// 
			// contextPaletteList
			// 
			this.contextPaletteList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDefaultPalette,
            this.toolStripMenuItem1,
            this.mnuPaletteCompositeDirect,
            this.mnuPaletteNesClassic,
            this.mnuPaletteNestopiaRgb,
            this.mnuPaletteOriginalHardware,
            this.mnuPalettePvmStyle,
            this.mnuPaletteSonyCxa2025As,
            this.mnuPaletteUnsaturated,
            this.mnuPaletteYuv});
			this.contextPaletteList.Name = "contextPicturePresets";
			this.contextPaletteList.Size = new System.Drawing.Size(255, 208);
			this.contextPaletteList.Opening += new System.ComponentModel.CancelEventHandler(this.contextPaletteList_Opening);
			// 
			// mnuDefaultPalette
			// 
			this.mnuDefaultPalette.Name = "mnuDefaultPalette";
			this.mnuDefaultPalette.Size = new System.Drawing.Size(254, 22);
			this.mnuDefaultPalette.Text = "Default (NTSC)";
			this.mnuDefaultPalette.Click += new System.EventHandler(this.mnuDefaultPalette_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
			// 
			// mnuPaletteCompositeDirect
			// 
			this.mnuPaletteCompositeDirect.Name = "mnuPaletteCompositeDirect";
			this.mnuPaletteCompositeDirect.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteCompositeDirect.Text = "Composite Direct (by FirebrandX)";
			this.mnuPaletteCompositeDirect.Click += new System.EventHandler(this.mnuPaletteCompositeDirect_Click);
			// 
			// mnuPaletteNesClassic
			// 
			this.mnuPaletteNesClassic.Name = "mnuPaletteNesClassic";
			this.mnuPaletteNesClassic.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteNesClassic.Text = "NES Classic (by FirebrandX)";
			this.mnuPaletteNesClassic.Click += new System.EventHandler(this.mnuPaletteNesClassic_Click);
			// 
			// mnuPaletteNestopiaRgb
			// 
			this.mnuPaletteNestopiaRgb.Name = "mnuPaletteNestopiaRgb";
			this.mnuPaletteNestopiaRgb.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteNestopiaRgb.Text = "Nestopia (RGB)";
			this.mnuPaletteNestopiaRgb.Click += new System.EventHandler(this.mnuPaletteNestopiaRgb_Click);
			// 
			// mnuPaletteOriginalHardware
			// 
			this.mnuPaletteOriginalHardware.Name = "mnuPaletteOriginalHardware";
			this.mnuPaletteOriginalHardware.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteOriginalHardware.Text = "Original Hardware (by FirebrandX)";
			this.mnuPaletteOriginalHardware.Click += new System.EventHandler(this.mnuPaletteOriginalHardware_Click);
			// 
			// mnuPalettePvmStyle
			// 
			this.mnuPalettePvmStyle.Name = "mnuPalettePvmStyle";
			this.mnuPalettePvmStyle.Size = new System.Drawing.Size(254, 22);
			this.mnuPalettePvmStyle.Text = "PVM Style (by FirebrandX)";
			this.mnuPalettePvmStyle.Click += new System.EventHandler(this.mnuPalettePvmStyle_Click);
			// 
			// mnuPaletteSonyCxa2025As
			// 
			this.mnuPaletteSonyCxa2025As.Name = "mnuPaletteSonyCxa2025As";
			this.mnuPaletteSonyCxa2025As.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteSonyCxa2025As.Text = "Sony CXA2025AS";
			this.mnuPaletteSonyCxa2025As.Click += new System.EventHandler(this.mnuPaletteSonyCxa2025As_Click);
			// 
			// mnuPaletteUnsaturated
			// 
			this.mnuPaletteUnsaturated.Name = "mnuPaletteUnsaturated";
			this.mnuPaletteUnsaturated.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteUnsaturated.Text = "Unsaturated v6 (by FirebrandX)";
			this.mnuPaletteUnsaturated.Click += new System.EventHandler(this.mnuPaletteUnsaturated_Click);
			// 
			// mnuPaletteYuv
			// 
			this.mnuPaletteYuv.Name = "mnuPaletteYuv";
			this.mnuPaletteYuv.Size = new System.Drawing.Size(254, 22);
			this.mnuPaletteYuv.Text = "YUV v3 (by FirebrandX)";
			this.mnuPaletteYuv.Click += new System.EventHandler(this.mnuPaletteYuv_Click);
			// 
			// frmVideoConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 457);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmVideoConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Video Options";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHdNesTooltip)).EndInit();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.flpRefreshRate.ResumeLayout(false);
			this.flpRefreshRate.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgPicture.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.grpNtscFilter.ResumeLayout(false);
			this.tlpNtscFilter2.ResumeLayout(false);
			this.tlpNtscFilter1.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.grpCommon.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.grpScanlines.ResumeLayout(false);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tpgOverscan.ResumeLayout(false);
			this.tabOverscan.ResumeLayout(false);
			this.tpgOverscanGlobal.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).EndInit();
			this.tableLayoutPanel11.ResumeLayout(false);
			this.tableLayoutPanel11.PerformLayout();
			this.tableLayoutPanel12.ResumeLayout(false);
			this.tableLayoutPanel12.PerformLayout();
			this.tableLayoutPanel13.ResumeLayout(false);
			this.tableLayoutPanel13.PerformLayout();
			this.tableLayoutPanel14.ResumeLayout(false);
			this.tableLayoutPanel14.PerformLayout();
			this.tpgOverscanGameSpecific.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picGameSpecificOverscan)).EndInit();
			this.tableLayoutPanel15.ResumeLayout(false);
			this.tableLayoutPanel15.PerformLayout();
			this.tableLayoutPanel16.ResumeLayout(false);
			this.tableLayoutPanel16.PerformLayout();
			this.tableLayoutPanel17.ResumeLayout(false);
			this.tableLayoutPanel17.PerformLayout();
			this.tableLayoutPanel18.ResumeLayout(false);
			this.tableLayoutPanel18.PerformLayout();
			this.tpgPalette.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.contextPicturePresets.ResumeLayout(false);
			this.contextPaletteList.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkShowFps;
		private System.Windows.Forms.Label lblVideoScale;
		private System.Windows.Forms.CheckBox chkVerticalSync;
		private System.Windows.Forms.ComboBox cboAspectRatio;
		private System.Windows.Forms.Label lblDisplayRatio;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgOverscan;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox picOverscan;
		private System.Windows.Forms.Label lblLeft;
		private MesenNumericUpDown nudOverscanLeft;
		private System.Windows.Forms.Label lblTop;
		private MesenNumericUpDown nudOverscanTop;
		private System.Windows.Forms.Label lblBottom;
		private MesenNumericUpDown nudOverscanBottom;
		private System.Windows.Forms.Label lblRight;
		private MesenNumericUpDown nudOverscanRight;
		private System.Windows.Forms.TabPage tpgPalette;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.CheckBox chkUseHdPacks;
		private System.Windows.Forms.PictureBox picHdNesTooltip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnLoadPalFile;
		private System.Windows.Forms.ColorDialog colorDialog;
		private MesenNumericUpDown nudScale;
		private System.Windows.Forms.TabPage tpgPicture;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private Controls.ctrlHorizontalTrackbar trkBrightness;
		private Controls.ctrlHorizontalTrackbar trkContrast;
		private Controls.ctrlHorizontalTrackbar trkHue;
		private Controls.ctrlHorizontalTrackbar trkSaturation;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.GroupBox grpNtscFilter;
		private System.Windows.Forms.TableLayoutPanel tlpNtscFilter1;
		private Controls.ctrlHorizontalTrackbar trkArtifacts;
		private Controls.ctrlHorizontalTrackbar trkBleed;
		private Controls.ctrlHorizontalTrackbar trkFringing;
		private Controls.ctrlHorizontalTrackbar trkGamma;
		private Controls.ctrlHorizontalTrackbar trkResolution;
		private Controls.ctrlHorizontalTrackbar trkSharpness;
		private System.Windows.Forms.GroupBox grpCommon;
		private System.Windows.Forms.CheckBox chkMergeFields;
		private System.Windows.Forms.Button btnResetPictureSettings;
		private System.Windows.Forms.GroupBox grpScanlines;
		private Controls.ctrlHorizontalTrackbar trkScanlines;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Button btnSelectPreset;
		private System.Windows.Forms.ContextMenuStrip contextPicturePresets;
		private System.Windows.Forms.ToolStripMenuItem mnuPresetComposite;
		private System.Windows.Forms.ToolStripMenuItem mnuPresetSVideo;
		private System.Windows.Forms.ToolStripMenuItem mnuPresetRgb;
		private System.Windows.Forms.ToolStripMenuItem mnuPresetMonochrome;
		private System.Windows.Forms.CheckBox chkBilinearInterpolation;
		private System.Windows.Forms.Button btnSelectPalette;
		private System.Windows.Forms.ContextMenuStrip contextPaletteList;
		private System.Windows.Forms.ToolStripMenuItem mnuDefaultPalette;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteUnsaturated;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteYuv;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteNestopiaRgb;
		private System.Windows.Forms.Button btnExportPalette;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.Label lblVideoFilter;
		private System.Windows.Forms.ComboBox cboFilter;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private ctrlRiskyOption chkDisableBackground;
		private ctrlRiskyOption chkDisableSprites;
		private ctrlRiskyOption chkForceBackgroundFirstColumn;
		private ctrlRiskyOption chkForceSpritesFirstColumn;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteNesClassic;
		private System.Windows.Forms.TableLayoutPanel tlpNtscFilter2;
		private Controls.ctrlHorizontalTrackbar trkYFilterLength;
		private Controls.ctrlHorizontalTrackbar trkIFilterLength;
		private Controls.ctrlHorizontalTrackbar trkQFilterLength;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteSonyCxa2025As;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblCustomRatio;
		private MesenNumericUpDown nudCustomRatio;
		private System.Windows.Forms.ToolStripMenuItem mnuPalettePvmStyle;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteOriginalHardware;
		private System.Windows.Forms.ToolStripMenuItem mnuPaletteCompositeDirect;
		private System.Windows.Forms.CheckBox chkUseCustomVsPalette;
		private System.Windows.Forms.CheckBox chkFullscreenForceIntegerScale;
		private System.Windows.Forms.CheckBox chkShowColorIndexes;
		private Debugger.ctrlPaletteDisplay ctrlPaletteDisplay;
		private System.Windows.Forms.CheckBox chkIntegerFpsMode;
		private System.Windows.Forms.Label lblScreenRotation;
		private System.Windows.Forms.ComboBox cboScreenRotation;
		private System.Windows.Forms.CheckBox chkUseExclusiveFullscreen;
		private System.Windows.Forms.Label lblRequestedRefreshRate;
		private System.Windows.Forms.ComboBox cboRefreshRate;
		private System.Windows.Forms.FlowLayoutPanel flpRefreshRate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.CheckBox chkVerticalBlend;
		private System.Windows.Forms.TabControl tabOverscan;
		private System.Windows.Forms.TabPage tpgOverscanGlobal;
		private System.Windows.Forms.TabPage tpgOverscanGameSpecific;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkEnableGameSpecificOverscan;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.PictureBox picGameSpecificOverscan;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
		private MesenNumericUpDown nudGameSpecificOverscanTop;
		private System.Windows.Forms.Label lblGameSpecificOverscanTop;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
		private MesenNumericUpDown nudGameSpecificOverscanBottom;
		private System.Windows.Forms.Label lblGameSpecificOverscanBottom;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
		private MesenNumericUpDown nudGameSpecificOverscanRight;
		private System.Windows.Forms.Label lblGameSpecificOverscanRight;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
		private MesenNumericUpDown nudGameSpecificOverscanLeft;
		private System.Windows.Forms.Label lblGameSpecificOverscanLeft;
	}
}