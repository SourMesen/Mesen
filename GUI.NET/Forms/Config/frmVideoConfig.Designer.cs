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
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.lblVideoScale = new System.Windows.Forms.Label();
			this.lblVideoFilter = new System.Windows.Forms.Label();
			this.chkVerticalSync = new System.Windows.Forms.CheckBox();
			this.cboAspectRatio = new System.Windows.Forms.ComboBox();
			this.lblDisplayRatio = new System.Windows.Forms.Label();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudEmulationSpeed = new System.Windows.Forms.NumericUpDown();
			this.lblEmuSpeedHint = new System.Windows.Forms.Label();
			this.lblEmulationSpeed = new System.Windows.Forms.Label();
			this.chkShowFps = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkUseHdPacks = new System.Windows.Forms.CheckBox();
			this.picHdNesTooltip = new System.Windows.Forms.PictureBox();
			this.nudScale = new System.Windows.Forms.NumericUpDown();
			this.cboFilter = new System.Windows.Forms.ComboBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgOverscan = new System.Windows.Forms.TabPage();
			this.grpCropping = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.picOverscan = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblLeft = new System.Windows.Forms.Label();
			this.nudOverscanLeft = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblTop = new System.Windows.Forms.Label();
			this.nudOverscanTop = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblBottom = new System.Windows.Forms.Label();
			this.nudOverscanBottom = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblRight = new System.Windows.Forms.Label();
			this.nudOverscanRight = new System.Windows.Forms.NumericUpDown();
			this.tpgPalette = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.picPalette = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnResetPalette = new System.Windows.Forms.Button();
			this.btnLoadPalFile = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEmulationSpeed)).BeginInit();
			this.flowLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHdNesTooltip)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgOverscan.SuspendLayout();
			this.grpCropping.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).BeginInit();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanLeft)).BeginInit();
			this.flowLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanTop)).BeginInit();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanBottom)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanRight)).BeginInit();
			this.tpgPalette.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 368);
			this.baseConfigPanel.Size = new System.Drawing.Size(515, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.lblVideoScale, 0, 0);
			this.tlpMain.Controls.Add(this.lblVideoFilter, 0, 1);
			this.tlpMain.Controls.Add(this.chkVerticalSync, 0, 4);
			this.tlpMain.Controls.Add(this.cboAspectRatio, 1, 2);
			this.tlpMain.Controls.Add(this.lblDisplayRatio, 0, 2);
			this.tlpMain.Controls.Add(this.flowLayoutPanel6, 1, 6);
			this.tlpMain.Controls.Add(this.lblEmulationSpeed, 0, 6);
			this.tlpMain.Controls.Add(this.chkShowFps, 0, 5);
			this.tlpMain.Controls.Add(this.flowLayoutPanel7, 0, 3);
			this.tlpMain.Controls.Add(this.nudScale, 1, 0);
			this.tlpMain.Controls.Add(this.cboFilter, 1, 1);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(3, 3);
			this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 8;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.Size = new System.Drawing.Size(501, 336);
			this.tlpMain.TabIndex = 1;
			// 
			// lblVideoScale
			// 
			this.lblVideoScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVideoScale.AutoSize = true;
			this.lblVideoScale.Location = new System.Drawing.Point(3, 6);
			this.lblVideoScale.Name = "lblVideoScale";
			this.lblVideoScale.Size = new System.Drawing.Size(37, 13);
			this.lblVideoScale.TabIndex = 11;
			this.lblVideoScale.Text = "Scale:";
			// 
			// lblVideoFilter
			// 
			this.lblVideoFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVideoFilter.AutoSize = true;
			this.lblVideoFilter.Location = new System.Drawing.Point(3, 33);
			this.lblVideoFilter.Name = "lblVideoFilter";
			this.lblVideoFilter.Size = new System.Drawing.Size(32, 13);
			this.lblVideoFilter.TabIndex = 12;
			this.lblVideoFilter.Text = "Filter:";
			// 
			// chkVerticalSync
			// 
			this.chkVerticalSync.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkVerticalSync.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkVerticalSync, 2);
			this.chkVerticalSync.Location = new System.Drawing.Point(3, 106);
			this.chkVerticalSync.Name = "chkVerticalSync";
			this.chkVerticalSync.Size = new System.Drawing.Size(121, 17);
			this.chkVerticalSync.TabIndex = 15;
			this.chkVerticalSync.Text = "Enable vertical sync";
			this.chkVerticalSync.UseVisualStyleBackColor = true;
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
			this.cboAspectRatio.Location = new System.Drawing.Point(99, 56);
			this.cboAspectRatio.Name = "cboAspectRatio";
			this.cboAspectRatio.Size = new System.Drawing.Size(121, 21);
			this.cboAspectRatio.TabIndex = 16;
			// 
			// lblDisplayRatio
			// 
			this.lblDisplayRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDisplayRatio.AutoSize = true;
			this.lblDisplayRatio.Location = new System.Drawing.Point(3, 60);
			this.lblDisplayRatio.Name = "lblDisplayRatio";
			this.lblDisplayRatio.Size = new System.Drawing.Size(71, 13);
			this.lblDisplayRatio.TabIndex = 17;
			this.lblDisplayRatio.Text = "Aspect Ratio:";
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.nudEmulationSpeed);
			this.flowLayoutPanel6.Controls.Add(this.lblEmuSpeedHint);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(96, 149);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(405, 26);
			this.flowLayoutPanel6.TabIndex = 10;
			// 
			// nudEmulationSpeed
			// 
			this.nudEmulationSpeed.Location = new System.Drawing.Point(3, 3);
			this.nudEmulationSpeed.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.nudEmulationSpeed.Name = "nudEmulationSpeed";
			this.nudEmulationSpeed.Size = new System.Drawing.Size(48, 20);
			this.nudEmulationSpeed.TabIndex = 1;
			// 
			// lblEmuSpeedHint
			// 
			this.lblEmuSpeedHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEmuSpeedHint.AutoSize = true;
			this.lblEmuSpeedHint.Location = new System.Drawing.Point(57, 6);
			this.lblEmuSpeedHint.Name = "lblEmuSpeedHint";
			this.lblEmuSpeedHint.Size = new System.Drawing.Size(107, 13);
			this.lblEmuSpeedHint.TabIndex = 2;
			this.lblEmuSpeedHint.Text = "(0 = Maximum speed)";
			// 
			// lblEmulationSpeed
			// 
			this.lblEmulationSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEmulationSpeed.AutoSize = true;
			this.lblEmulationSpeed.Location = new System.Drawing.Point(3, 155);
			this.lblEmulationSpeed.Name = "lblEmulationSpeed";
			this.lblEmulationSpeed.Size = new System.Drawing.Size(90, 13);
			this.lblEmulationSpeed.TabIndex = 0;
			this.lblEmulationSpeed.Text = "Emulation Speed:";
			// 
			// chkShowFps
			// 
			this.chkShowFps.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkShowFps.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkShowFps, 2);
			this.chkShowFps.Location = new System.Drawing.Point(3, 129);
			this.chkShowFps.Name = "chkShowFps";
			this.chkShowFps.Size = new System.Drawing.Size(76, 17);
			this.chkShowFps.TabIndex = 9;
			this.chkShowFps.Text = "Show FPS";
			this.chkShowFps.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel7
			// 
			this.tlpMain.SetColumnSpan(this.flowLayoutPanel7, 2);
			this.flowLayoutPanel7.Controls.Add(this.chkUseHdPacks);
			this.flowLayoutPanel7.Controls.Add(this.picHdNesTooltip);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 80);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(501, 23);
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
			this.picHdNesTooltip.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picHdNesTooltip.Location = new System.Drawing.Point(143, 3);
			this.picHdNesTooltip.Name = "picHdNesTooltip";
			this.picHdNesTooltip.Size = new System.Drawing.Size(17, 17);
			this.picHdNesTooltip.TabIndex = 21;
			this.picHdNesTooltip.TabStop = false;
			// 
			// nudScale
			// 
			this.nudScale.DecimalPlaces = 2;
			this.nudScale.Location = new System.Drawing.Point(99, 3);
			this.nudScale.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudScale.Name = "nudScale";
			this.nudScale.Size = new System.Drawing.Size(48, 20);
			this.nudScale.TabIndex = 21;
			// 
			// cboFilter
			// 
			this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFilter.FormattingEnabled = true;
			this.cboFilter.Items.AddRange(new object[] {
            "None",
            "NTSC"});
			this.cboFilter.Location = new System.Drawing.Point(99, 29);
			this.cboFilter.Name = "cboFilter";
			this.cboFilter.Size = new System.Drawing.Size(76, 21);
			this.cboFilter.TabIndex = 14;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgOverscan);
			this.tabMain.Controls.Add(this.tpgPalette);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(515, 368);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tlpMain);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(507, 342);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgOverscan
			// 
			this.tpgOverscan.Controls.Add(this.grpCropping);
			this.tpgOverscan.Location = new System.Drawing.Point(4, 22);
			this.tpgOverscan.Name = "tpgOverscan";
			this.tpgOverscan.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverscan.Size = new System.Drawing.Size(507, 342);
			this.tpgOverscan.TabIndex = 1;
			this.tpgOverscan.Text = "Overscan";
			this.tpgOverscan.UseVisualStyleBackColor = true;
			// 
			// grpCropping
			// 
			this.grpCropping.Controls.Add(this.tableLayoutPanel1);
			this.grpCropping.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCropping.Location = new System.Drawing.Point(3, 3);
			this.grpCropping.Name = "grpCropping";
			this.grpCropping.Size = new System.Drawing.Size(501, 336);
			this.grpCropping.TabIndex = 8;
			this.grpCropping.TabStop = false;
			this.grpCropping.Text = "Video Cropping";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.picOverscan, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel5, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(65, 8);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(369, 327);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// picOverscan
			// 
			this.picOverscan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picOverscan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picOverscan.Location = new System.Drawing.Point(56, 43);
			this.picOverscan.Name = "picOverscan";
			this.picOverscan.Size = new System.Drawing.Size(257, 241);
			this.picOverscan.TabIndex = 1;
			this.picOverscan.TabStop = false;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.flowLayoutPanel3.Controls.Add(this.lblLeft);
			this.flowLayoutPanel3.Controls.Add(this.nudOverscanLeft);
			this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 143);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel3.TabIndex = 1;
			// 
			// lblLeft
			// 
			this.lblLeft.AutoSize = true;
			this.lblLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblLeft.Location = new System.Drawing.Point(3, 0);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(50, 13);
			this.lblLeft.TabIndex = 0;
			this.lblLeft.Text = "Left";
			this.lblLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanLeft
			// 
			this.nudOverscanLeft.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanLeft.Name = "nudOverscanLeft";
			this.nudOverscanLeft.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanLeft.TabIndex = 2;
			this.nudOverscanLeft.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel4.Controls.Add(this.lblTop);
			this.flowLayoutPanel4.Controls.Add(this.nudOverscanTop);
			this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(158, 0);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel4.TabIndex = 2;
			// 
			// lblTop
			// 
			this.lblTop.AutoSize = true;
			this.lblTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTop.Location = new System.Drawing.Point(3, 0);
			this.lblTop.Name = "lblTop";
			this.lblTop.Size = new System.Drawing.Size(50, 13);
			this.lblTop.TabIndex = 0;
			this.lblTop.Text = "Top";
			this.lblTop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanTop
			// 
			this.nudOverscanTop.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanTop.Name = "nudOverscanTop";
			this.nudOverscanTop.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanTop.TabIndex = 2;
			this.nudOverscanTop.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel5.Controls.Add(this.lblBottom);
			this.flowLayoutPanel5.Controls.Add(this.nudOverscanBottom);
			this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(158, 287);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel5.TabIndex = 3;
			// 
			// lblBottom
			// 
			this.lblBottom.AutoSize = true;
			this.lblBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblBottom.Location = new System.Drawing.Point(3, 0);
			this.lblBottom.Name = "lblBottom";
			this.lblBottom.Size = new System.Drawing.Size(50, 13);
			this.lblBottom.TabIndex = 0;
			this.lblBottom.Text = "Bottom";
			this.lblBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanBottom
			// 
			this.nudOverscanBottom.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanBottom.Name = "nudOverscanBottom";
			this.nudOverscanBottom.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanBottom.TabIndex = 2;
			this.nudOverscanBottom.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.flowLayoutPanel2.Controls.Add(this.lblRight);
			this.flowLayoutPanel2.Controls.Add(this.nudOverscanRight);
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(316, 143);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel2.TabIndex = 0;
			// 
			// lblRight
			// 
			this.lblRight.AutoSize = true;
			this.lblRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblRight.Location = new System.Drawing.Point(3, 0);
			this.lblRight.Name = "lblRight";
			this.lblRight.Size = new System.Drawing.Size(50, 13);
			this.lblRight.TabIndex = 0;
			this.lblRight.Text = "Right";
			this.lblRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanRight
			// 
			this.nudOverscanRight.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanRight.Name = "nudOverscanRight";
			this.nudOverscanRight.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanRight.TabIndex = 1;
			this.nudOverscanRight.ValueChanged += new System.EventHandler(this.nudOverscan_ValueChanged);
			// 
			// tpgPalette
			// 
			this.tpgPalette.Controls.Add(this.tableLayoutPanel3);
			this.tpgPalette.Location = new System.Drawing.Point(4, 22);
			this.tpgPalette.Name = "tpgPalette";
			this.tpgPalette.Padding = new System.Windows.Forms.Padding(3);
			this.tpgPalette.Size = new System.Drawing.Size(507, 342);
			this.tpgPalette.TabIndex = 2;
			this.tpgPalette.Text = "Palette";
			this.tpgPalette.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.picPalette, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(501, 336);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// picPalette
			// 
			this.picPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPalette.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPalette.Location = new System.Drawing.Point(1, 1);
			this.picPalette.Margin = new System.Windows.Forms.Padding(1);
			this.picPalette.Name = "picPalette";
			this.picPalette.Size = new System.Drawing.Size(386, 98);
			this.picPalette.TabIndex = 0;
			this.picPalette.TabStop = false;
			this.picPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.btnResetPalette, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.btnLoadPalFile, 0, 0);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(388, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(113, 73);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// btnResetPalette
			// 
			this.btnResetPalette.Location = new System.Drawing.Point(3, 32);
			this.btnResetPalette.Name = "btnResetPalette";
			this.btnResetPalette.Size = new System.Drawing.Size(106, 22);
			this.btnResetPalette.TabIndex = 1;
			this.btnResetPalette.Text = "Reset to Defaults";
			this.btnResetPalette.UseVisualStyleBackColor = true;
			this.btnResetPalette.Click += new System.EventHandler(this.btnResetPalette_Click);
			// 
			// btnLoadPalFile
			// 
			this.btnLoadPalFile.AutoSize = true;
			this.btnLoadPalFile.Location = new System.Drawing.Point(3, 3);
			this.btnLoadPalFile.Name = "btnLoadPalFile";
			this.btnLoadPalFile.Size = new System.Drawing.Size(106, 23);
			this.btnLoadPalFile.TabIndex = 0;
			this.btnLoadPalFile.Text = "Load Palette File";
			this.btnLoadPalFile.UseVisualStyleBackColor = true;
			this.btnLoadPalFile.Click += new System.EventHandler(this.btnLoadPalFile_Click);
			// 
			// frmVideoConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 397);
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
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEmulationSpeed)).EndInit();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHdNesTooltip)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgOverscan.ResumeLayout(false);
			this.grpCropping.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).EndInit();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanLeft)).EndInit();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanTop)).EndInit();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanBottom)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanRight)).EndInit();
			this.tpgPalette.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkShowFps;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblEmulationSpeed;
		private System.Windows.Forms.NumericUpDown nudEmulationSpeed;
		private System.Windows.Forms.Label lblEmuSpeedHint;
		private System.Windows.Forms.Label lblVideoScale;
		private System.Windows.Forms.Label lblVideoFilter;
		private System.Windows.Forms.ComboBox cboFilter;
		private System.Windows.Forms.CheckBox chkVerticalSync;
		private System.Windows.Forms.ComboBox cboAspectRatio;
		private System.Windows.Forms.Label lblDisplayRatio;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgOverscan;
		private System.Windows.Forms.GroupBox grpCropping;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox picOverscan;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label lblLeft;
		private System.Windows.Forms.NumericUpDown nudOverscanLeft;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblTop;
		private System.Windows.Forms.NumericUpDown nudOverscanTop;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Label lblBottom;
		private System.Windows.Forms.NumericUpDown nudOverscanBottom;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblRight;
		private System.Windows.Forms.NumericUpDown nudOverscanRight;
		private System.Windows.Forms.TabPage tpgPalette;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.CheckBox chkUseHdPacks;
		private System.Windows.Forms.PictureBox picHdNesTooltip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox picPalette;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnResetPalette;
		private System.Windows.Forms.Button btnLoadPalFile;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.NumericUpDown nudScale;
	}
}