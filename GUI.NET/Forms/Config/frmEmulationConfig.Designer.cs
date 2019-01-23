﻿using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	partial class frmEmulationConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmulationConfig));
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudTurboSpeed = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblTurboSpeedHint = new System.Windows.Forms.Label();
			this.lblTurboSpeed = new System.Windows.Forms.Label();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudEmulationSpeed = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblEmuSpeedHint = new System.Windows.Forms.Label();
			this.lblEmulationSpeed = new System.Windows.Forms.Label();
			this.lblRewindSpeed = new System.Windows.Forms.Label();
			this.flowLayoutPanel10 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudRewindSpeed = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblRewindSpeedHint = new System.Windows.Forms.Label();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblMiscSettings = new System.Windows.Forms.Label();
			this.chkMapperRandomPowerOnState = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkEnableOamDecay = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblRamPowerOnState = new System.Windows.Forms.Label();
			this.cboRamPowerOnState = new System.Windows.Forms.ComboBox();
			this.chkDisablePaletteRead = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkDisableOamAddrBug = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkDisablePpuReset = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkDisablePpu2004Reads = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkUseNes101Hvc101Behavior = new System.Windows.Forms.CheckBox();
			this.chkAllowInvalidInput = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkUseAlternativeMmc3Irq = new System.Windows.Forms.CheckBox();
			this.lblDeveloperSettings = new System.Windows.Forms.Label();
			this.tpgOverclocking = new System.Windows.Forms.TabPage();
			this.picHint = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblOverclockHint = new Mesen.GUI.Controls.ctrlAutoGrowLabel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRateDendy = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValueDendy = new System.Windows.Forms.Label();
			this.chkOverclockAdjustApu = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRatePal = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValuePal = new System.Windows.Forms.Label();
			this.grpOverclocking = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblClockRate = new System.Windows.Forms.Label();
			this.nudOverclockRate = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblClockRatePercent = new System.Windows.Forms.Label();
			this.grpPpuTiming = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.nudExtraScanlinesAfterNmi = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.nudExtraScanlinesBeforeNmi = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblExtraScanlinesBeforeNmi = new System.Windows.Forms.Label();
			this.lblExtraScanlinesAfterNmi = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRate = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValue = new System.Windows.Forms.Label();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkShowLagCounter = new System.Windows.Forms.CheckBox();
			this.btnResetLagCounter = new System.Windows.Forms.Button();
			this.tmrUpdateClockRate = new System.Windows.Forms.Timer(this.components);
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel9.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel10.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel8.SuspendLayout();
			this.tpgOverclocking.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.grpOverclocking.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpPpuTiming.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 321);
			this.baseConfigPanel.Size = new System.Drawing.Size(533, 29);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Controls.Add(this.tpgOverclocking);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(533, 321);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tableLayoutPanel4);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(525, 295);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel9, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblTurboSpeed, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel6, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblEmulationSpeed, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblRewindSpeed, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel10, 1, 2);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(519, 289);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// flowLayoutPanel9
			// 
			this.flowLayoutPanel9.AutoSize = true;
			this.flowLayoutPanel9.Controls.Add(this.nudTurboSpeed);
			this.flowLayoutPanel9.Controls.Add(this.lblTurboSpeedHint);
			this.flowLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel9.Location = new System.Drawing.Point(111, 27);
			this.flowLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel9.Name = "flowLayoutPanel9";
			this.flowLayoutPanel9.Size = new System.Drawing.Size(408, 27);
			this.flowLayoutPanel9.TabIndex = 14;
			// 
			// nudTurboSpeed
			// 
			this.nudTurboSpeed.DecimalPlaces = 0;
			this.nudTurboSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudTurboSpeed.Location = new System.Drawing.Point(3, 3);
			this.nudTurboSpeed.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudTurboSpeed.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudTurboSpeed.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudTurboSpeed.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudTurboSpeed.Name = "nudTurboSpeed";
			this.nudTurboSpeed.Size = new System.Drawing.Size(48, 21);
			this.nudTurboSpeed.TabIndex = 1;
			this.nudTurboSpeed.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblTurboSpeedHint
			// 
			this.lblTurboSpeedHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTurboSpeedHint.AutoSize = true;
			this.lblTurboSpeedHint.Location = new System.Drawing.Point(57, 7);
			this.lblTurboSpeedHint.Name = "lblTurboSpeedHint";
			this.lblTurboSpeedHint.Size = new System.Drawing.Size(121, 13);
			this.lblTurboSpeedHint.TabIndex = 2;
			this.lblTurboSpeedHint.Text = "%  (0 = Maximum speed)";
			// 
			// lblTurboSpeed
			// 
			this.lblTurboSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTurboSpeed.AutoSize = true;
			this.lblTurboSpeed.Location = new System.Drawing.Point(3, 34);
			this.lblTurboSpeed.Name = "lblTurboSpeed";
			this.lblTurboSpeed.Size = new System.Drawing.Size(105, 13);
			this.lblTurboSpeed.TabIndex = 13;
			this.lblTurboSpeed.Text = "Fast Forward Speed:";
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.nudEmulationSpeed);
			this.flowLayoutPanel6.Controls.Add(this.lblEmuSpeedHint);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(111, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(408, 27);
			this.flowLayoutPanel6.TabIndex = 11;
			// 
			// nudEmulationSpeed
			// 
			this.nudEmulationSpeed.DecimalPlaces = 0;
			this.nudEmulationSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudEmulationSpeed.Location = new System.Drawing.Point(3, 3);
			this.nudEmulationSpeed.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudEmulationSpeed.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudEmulationSpeed.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudEmulationSpeed.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudEmulationSpeed.Name = "nudEmulationSpeed";
			this.nudEmulationSpeed.Size = new System.Drawing.Size(48, 21);
			this.nudEmulationSpeed.TabIndex = 1;
			this.nudEmulationSpeed.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblEmuSpeedHint
			// 
			this.lblEmuSpeedHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEmuSpeedHint.AutoSize = true;
			this.lblEmuSpeedHint.Location = new System.Drawing.Point(57, 7);
			this.lblEmuSpeedHint.Name = "lblEmuSpeedHint";
			this.lblEmuSpeedHint.Size = new System.Drawing.Size(121, 13);
			this.lblEmuSpeedHint.TabIndex = 2;
			this.lblEmuSpeedHint.Text = "%  (0 = Maximum speed)";
			// 
			// lblEmulationSpeed
			// 
			this.lblEmulationSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEmulationSpeed.AutoSize = true;
			this.lblEmulationSpeed.Location = new System.Drawing.Point(3, 7);
			this.lblEmulationSpeed.Name = "lblEmulationSpeed";
			this.lblEmulationSpeed.Size = new System.Drawing.Size(90, 13);
			this.lblEmulationSpeed.TabIndex = 12;
			this.lblEmulationSpeed.Text = "Emulation Speed:";
			// 
			// lblRewindSpeed
			// 
			this.lblRewindSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRewindSpeed.AutoSize = true;
			this.lblRewindSpeed.Location = new System.Drawing.Point(3, 61);
			this.lblRewindSpeed.Name = "lblRewindSpeed";
			this.lblRewindSpeed.Size = new System.Drawing.Size(80, 13);
			this.lblRewindSpeed.TabIndex = 15;
			this.lblRewindSpeed.Text = "Rewind Speed:";
			// 
			// flowLayoutPanel10
			// 
			this.flowLayoutPanel10.AutoSize = true;
			this.flowLayoutPanel10.Controls.Add(this.nudRewindSpeed);
			this.flowLayoutPanel10.Controls.Add(this.lblRewindSpeedHint);
			this.flowLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel10.Location = new System.Drawing.Point(111, 54);
			this.flowLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel10.Name = "flowLayoutPanel10";
			this.flowLayoutPanel10.Size = new System.Drawing.Size(408, 27);
			this.flowLayoutPanel10.TabIndex = 16;
			// 
			// nudRewindSpeed
			// 
			this.nudRewindSpeed.DecimalPlaces = 0;
			this.nudRewindSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudRewindSpeed.Location = new System.Drawing.Point(3, 3);
			this.nudRewindSpeed.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudRewindSpeed.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudRewindSpeed.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudRewindSpeed.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudRewindSpeed.Name = "nudRewindSpeed";
			this.nudRewindSpeed.Size = new System.Drawing.Size(48, 21);
			this.nudRewindSpeed.TabIndex = 1;
			this.nudRewindSpeed.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblRewindSpeedHint
			// 
			this.lblRewindSpeedHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRewindSpeedHint.AutoSize = true;
			this.lblRewindSpeedHint.Location = new System.Drawing.Point(57, 7);
			this.lblRewindSpeedHint.Name = "lblRewindSpeedHint";
			this.lblRewindSpeedHint.Size = new System.Drawing.Size(121, 13);
			this.lblRewindSpeedHint.TabIndex = 2;
			this.lblRewindSpeedHint.Text = "%  (0 = Maximum speed)";
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel1);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(525, 295);
			this.tpgAdvanced.TabIndex = 1;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblMiscSettings, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.chkMapperRandomPowerOnState, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkEnableOamDecay, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel8, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.chkDisablePaletteRead, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.chkDisableOamAddrBug, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.chkDisablePpuReset, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.chkDisablePpu2004Reads, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.chkUseNes101Hvc101Behavior, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.chkAllowInvalidInput, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.chkUseAlternativeMmc3Irq, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblDeveloperSettings, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 13;
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
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(519, 289);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblMiscSettings
			// 
			this.lblMiscSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMiscSettings.AutoSize = true;
			this.lblMiscSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblMiscSettings.Location = new System.Drawing.Point(0, 98);
			this.lblMiscSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
			this.lblMiscSettings.Name = "lblMiscSettings";
			this.lblMiscSettings.Size = new System.Drawing.Size(115, 13);
			this.lblMiscSettings.TabIndex = 35;
			this.lblMiscSettings.Text = "Miscellaneous Settings";
			// 
			// chkMapperRandomPowerOnState
			// 
			this.chkMapperRandomPowerOnState.AutoSize = true;
			this.chkMapperRandomPowerOnState.Checked = false;
			this.chkMapperRandomPowerOnState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkMapperRandomPowerOnState.Location = new System.Drawing.Point(10, 43);
			this.chkMapperRandomPowerOnState.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkMapperRandomPowerOnState.MinimumSize = new System.Drawing.Size(0, 23);
			this.chkMapperRandomPowerOnState.Name = "chkMapperRandomPowerOnState";
			this.chkMapperRandomPowerOnState.Size = new System.Drawing.Size(509, 23);
			this.chkMapperRandomPowerOnState.TabIndex = 11;
			this.chkMapperRandomPowerOnState.Text = "Randomize power-on state for mappers";
			// 
			// chkEnableOamDecay
			// 
			this.chkEnableOamDecay.Checked = false;
			this.chkEnableOamDecay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkEnableOamDecay.Location = new System.Drawing.Point(10, 20);
			this.chkEnableOamDecay.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkEnableOamDecay.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkEnableOamDecay.Name = "chkEnableOamDecay";
			this.chkEnableOamDecay.Size = new System.Drawing.Size(509, 23);
			this.chkEnableOamDecay.TabIndex = 9;
			this.chkEnableOamDecay.Text = "Enable OAM RAM decay";
			// 
			// flowLayoutPanel8
			// 
			this.flowLayoutPanel8.Controls.Add(this.lblRamPowerOnState);
			this.flowLayoutPanel8.Controls.Add(this.cboRamPowerOnState);
			this.flowLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel8.Location = new System.Drawing.Point(7, 66);
			this.flowLayoutPanel8.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
			this.flowLayoutPanel8.Name = "flowLayoutPanel8";
			this.flowLayoutPanel8.Size = new System.Drawing.Size(512, 27);
			this.flowLayoutPanel8.TabIndex = 3;
			// 
			// lblRamPowerOnState
			// 
			this.lblRamPowerOnState.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRamPowerOnState.AutoSize = true;
			this.lblRamPowerOnState.Location = new System.Drawing.Point(3, 7);
			this.lblRamPowerOnState.Name = "lblRamPowerOnState";
			this.lblRamPowerOnState.Size = new System.Drawing.Size(159, 13);
			this.lblRamPowerOnState.TabIndex = 0;
			this.lblRamPowerOnState.Text = "Default power on state for RAM:";
			// 
			// cboRamPowerOnState
			// 
			this.cboRamPowerOnState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRamPowerOnState.FormattingEnabled = true;
			this.cboRamPowerOnState.Location = new System.Drawing.Point(168, 3);
			this.cboRamPowerOnState.Name = "cboRamPowerOnState";
			this.cboRamPowerOnState.Size = new System.Drawing.Size(176, 21);
			this.cboRamPowerOnState.TabIndex = 1;
			// 
			// chkDisablePaletteRead
			// 
			this.chkDisablePaletteRead.Checked = false;
			this.chkDisablePaletteRead.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisablePaletteRead.Location = new System.Drawing.Point(10, 228);
			this.chkDisablePaletteRead.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkDisablePaletteRead.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisablePaletteRead.Name = "chkDisablePaletteRead";
			this.chkDisablePaletteRead.Size = new System.Drawing.Size(509, 23);
			this.chkDisablePaletteRead.TabIndex = 6;
			this.chkDisablePaletteRead.Text = "Disable PPU palette reads";
			// 
			// chkDisableOamAddrBug
			// 
			this.chkDisableOamAddrBug.Checked = false;
			this.chkDisableOamAddrBug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisableOamAddrBug.Location = new System.Drawing.Point(10, 182);
			this.chkDisableOamAddrBug.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkDisableOamAddrBug.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisableOamAddrBug.Name = "chkDisableOamAddrBug";
			this.chkDisableOamAddrBug.Size = new System.Drawing.Size(509, 23);
			this.chkDisableOamAddrBug.TabIndex = 5;
			this.chkDisableOamAddrBug.Text = "Disable PPU OAMADDR bug emulation";
			// 
			// chkDisablePpuReset
			// 
			this.chkDisablePpuReset.Checked = false;
			this.chkDisablePpuReset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisablePpuReset.Location = new System.Drawing.Point(10, 159);
			this.chkDisablePpuReset.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkDisablePpuReset.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisablePpuReset.Name = "chkDisablePpuReset";
			this.chkDisablePpuReset.Size = new System.Drawing.Size(509, 23);
			this.chkDisablePpuReset.TabIndex = 7;
			this.chkDisablePpuReset.Text = "Do not reset PPU when resetting console (Famicom behavior)";
			// 
			// chkDisablePpu2004Reads
			// 
			this.chkDisablePpu2004Reads.Checked = false;
			this.chkDisablePpu2004Reads.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisablePpu2004Reads.Location = new System.Drawing.Point(10, 205);
			this.chkDisablePpu2004Reads.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkDisablePpu2004Reads.MinimumSize = new System.Drawing.Size(0, 21);
			this.chkDisablePpu2004Reads.Name = "chkDisablePpu2004Reads";
			this.chkDisablePpu2004Reads.Size = new System.Drawing.Size(509, 23);
			this.chkDisablePpu2004Reads.TabIndex = 4;
			this.chkDisablePpu2004Reads.Text = "Disable PPU $2004 reads (Famicom behavior)";
			// 
			// chkUseNes101Hvc101Behavior
			// 
			this.chkUseNes101Hvc101Behavior.AutoSize = true;
			this.chkUseNes101Hvc101Behavior.Location = new System.Drawing.Point(13, 139);
			this.chkUseNes101Hvc101Behavior.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkUseNes101Hvc101Behavior.Name = "chkUseNes101Hvc101Behavior";
			this.chkUseNes101Hvc101Behavior.Size = new System.Drawing.Size(292, 17);
			this.chkUseNes101Hvc101Behavior.TabIndex = 8;
			this.chkUseNes101Hvc101Behavior.Text = "Use NES/HVC-101 (Top-loader / AV Famicom) behavior";
			this.chkUseNes101Hvc101Behavior.UseVisualStyleBackColor = true;
			// 
			// chkAllowInvalidInput
			// 
			this.chkAllowInvalidInput.AutoSize = true;
			this.chkAllowInvalidInput.Checked = false;
			this.chkAllowInvalidInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkAllowInvalidInput.Location = new System.Drawing.Point(10, 251);
			this.chkAllowInvalidInput.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.chkAllowInvalidInput.MinimumSize = new System.Drawing.Size(0, 23);
			this.chkAllowInvalidInput.Name = "chkAllowInvalidInput";
			this.chkAllowInvalidInput.Size = new System.Drawing.Size(509, 23);
			this.chkAllowInvalidInput.TabIndex = 1;
			this.chkAllowInvalidInput.Text = "Allow invalid input (e.g Down + Up or Left + Right at the same time)";
			// 
			// chkUseAlternativeMmc3Irq
			// 
			this.chkUseAlternativeMmc3Irq.AutoSize = true;
			this.chkUseAlternativeMmc3Irq.Location = new System.Drawing.Point(13, 116);
			this.chkUseAlternativeMmc3Irq.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
			this.chkUseAlternativeMmc3Irq.Name = "chkUseAlternativeMmc3Irq";
			this.chkUseAlternativeMmc3Irq.Size = new System.Drawing.Size(197, 17);
			this.chkUseAlternativeMmc3Irq.TabIndex = 0;
			this.chkUseAlternativeMmc3Irq.Text = "Use alternative MMC3 IRQ behavior";
			this.chkUseAlternativeMmc3Irq.UseVisualStyleBackColor = true;
			// 
			// lblDeveloperSettings
			// 
			this.lblDeveloperSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblDeveloperSettings.AutoSize = true;
			this.lblDeveloperSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblDeveloperSettings.Location = new System.Drawing.Point(0, 5);
			this.lblDeveloperSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
			this.lblDeveloperSettings.Name = "lblDeveloperSettings";
			this.lblDeveloperSettings.Size = new System.Drawing.Size(284, 13);
			this.lblDeveloperSettings.TabIndex = 33;
			this.lblDeveloperSettings.Text = "Recommended for developers (homebrew / ROM hacking)";
			// 
			// tpgOverclocking
			// 
			this.tpgOverclocking.Controls.Add(this.picHint);
			this.tpgOverclocking.Controls.Add(this.tableLayoutPanel3);
			this.tpgOverclocking.Location = new System.Drawing.Point(4, 22);
			this.tpgOverclocking.Name = "tpgOverclocking";
			this.tpgOverclocking.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverclocking.Size = new System.Drawing.Size(525, 295);
			this.tpgOverclocking.TabIndex = 2;
			this.tpgOverclocking.Text = "Overclocking";
			this.tpgOverclocking.UseVisualStyleBackColor = true;
			// 
			// picHint
			// 
			this.picHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picHint.BackgroundImage = global::Mesen.GUI.Properties.Resources.Help;
			this.picHint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picHint.Location = new System.Drawing.Point(12, 15);
			this.picHint.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picHint.Name = "picHint";
			this.picHint.Size = new System.Drawing.Size(16, 16);
			this.picHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picHint.TabIndex = 0;
			this.picHint.TabStop = false;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.lblOverclockHint, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 0, 5);
			this.tableLayoutPanel3.Controls.Add(this.chkOverclockAdjustApu, 0, 6);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.grpOverclocking, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.grpPpuTiming, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel7, 0, 7);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 8;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(519, 289);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// lblOverclockHint
			// 
			this.lblOverclockHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblOverclockHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblOverclockHint.Location = new System.Drawing.Point(3, 0);
			this.lblOverclockHint.Name = "lblOverclockHint";
			this.lblOverclockHint.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
			this.lblOverclockHint.Size = new System.Drawing.Size(513, 41);
			this.lblOverclockHint.TabIndex = 1;
			this.lblOverclockHint.Text = resources.GetString("lblOverclockHint.Text");
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.lblEffectiveClockRateDendy);
			this.flowLayoutPanel4.Controls.Add(this.lblEffectiveClockRateValueDendy);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 203);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(519, 20);
			this.flowLayoutPanel4.TabIndex = 11;
			// 
			// lblEffectiveClockRateDendy
			// 
			this.lblEffectiveClockRateDendy.AutoSize = true;
			this.lblEffectiveClockRateDendy.Location = new System.Drawing.Point(3, 0);
			this.lblEffectiveClockRateDendy.Name = "lblEffectiveClockRateDendy";
			this.lblEffectiveClockRateDendy.Size = new System.Drawing.Size(148, 13);
			this.lblEffectiveClockRateDendy.TabIndex = 0;
			this.lblEffectiveClockRateDendy.Text = "Effective Clock Rate (Dendy):";
			// 
			// lblEffectiveClockRateValueDendy
			// 
			this.lblEffectiveClockRateValueDendy.AutoSize = true;
			this.lblEffectiveClockRateValueDendy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEffectiveClockRateValueDendy.Location = new System.Drawing.Point(157, 0);
			this.lblEffectiveClockRateValueDendy.Name = "lblEffectiveClockRateValueDendy";
			this.lblEffectiveClockRateValueDendy.Size = new System.Drawing.Size(37, 13);
			this.lblEffectiveClockRateValueDendy.TabIndex = 1;
			this.lblEffectiveClockRateValueDendy.Text = "100%";
			// 
			// chkOverclockAdjustApu
			// 
			this.chkOverclockAdjustApu.AutoSize = true;
			this.chkOverclockAdjustApu.Location = new System.Drawing.Point(3, 226);
			this.chkOverclockAdjustApu.Name = "chkOverclockAdjustApu";
			this.chkOverclockAdjustApu.Size = new System.Drawing.Size(401, 17);
			this.chkOverclockAdjustApu.TabIndex = 10;
			this.chkOverclockAdjustApu.Text = "Do not overclock APU (prevents sound pitch changes caused by overclocking)";
			this.chkOverclockAdjustApu.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.lblEffectiveClockRatePal);
			this.flowLayoutPanel3.Controls.Add(this.lblEffectiveClockRateValuePal);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 186);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(519, 17);
			this.flowLayoutPanel3.TabIndex = 9;
			// 
			// lblEffectiveClockRatePal
			// 
			this.lblEffectiveClockRatePal.AutoSize = true;
			this.lblEffectiveClockRatePal.Location = new System.Drawing.Point(3, 0);
			this.lblEffectiveClockRatePal.Name = "lblEffectiveClockRatePal";
			this.lblEffectiveClockRatePal.Size = new System.Drawing.Size(137, 13);
			this.lblEffectiveClockRatePal.TabIndex = 0;
			this.lblEffectiveClockRatePal.Text = "Effective Clock Rate (PAL):";
			// 
			// lblEffectiveClockRateValuePal
			// 
			this.lblEffectiveClockRateValuePal.AutoSize = true;
			this.lblEffectiveClockRateValuePal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEffectiveClockRateValuePal.Location = new System.Drawing.Point(146, 0);
			this.lblEffectiveClockRateValuePal.Name = "lblEffectiveClockRateValuePal";
			this.lblEffectiveClockRateValuePal.Size = new System.Drawing.Size(37, 13);
			this.lblEffectiveClockRateValuePal.TabIndex = 1;
			this.lblEffectiveClockRateValuePal.Text = "100%";
			// 
			// grpOverclocking
			// 
			this.grpOverclocking.Controls.Add(this.tableLayoutPanel2);
			this.grpOverclocking.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpOverclocking.Location = new System.Drawing.Point(3, 121);
			this.grpOverclocking.Name = "grpOverclocking";
			this.grpOverclocking.Size = new System.Drawing.Size(513, 45);
			this.grpOverclocking.TabIndex = 6;
			this.grpOverclocking.TabStop = false;
			this.grpOverclocking.Text = "Overclocking";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblClockRate, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.nudOverclockRate, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblClockRatePercent, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(507, 26);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// lblClockRate
			// 
			this.lblClockRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClockRate.AutoSize = true;
			this.lblClockRate.Location = new System.Drawing.Point(3, 7);
			this.lblClockRate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblClockRate.Name = "lblClockRate";
			this.lblClockRate.Size = new System.Drawing.Size(107, 13);
			this.lblClockRate.TabIndex = 1;
			this.lblClockRate.Text = "Clock Rate Multiplier:";
			// 
			// nudOverclockRate
			// 
			this.nudOverclockRate.DecimalPlaces = 0;
			this.nudOverclockRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverclockRate.Location = new System.Drawing.Point(110, 3);
			this.nudOverclockRate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudOverclockRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudOverclockRate.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudOverclockRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudOverclockRate.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudOverclockRate.Name = "nudOverclockRate";
			this.nudOverclockRate.Size = new System.Drawing.Size(46, 21);
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
			this.lblClockRatePercent.Location = new System.Drawing.Point(156, 7);
			this.lblClockRatePercent.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblClockRatePercent.Name = "lblClockRatePercent";
			this.lblClockRatePercent.Size = new System.Drawing.Size(90, 13);
			this.lblClockRatePercent.TabIndex = 1;
			this.lblClockRatePercent.Text = "% (Default: 100%)";
			// 
			// grpPpuTiming
			// 
			this.grpPpuTiming.Controls.Add(this.tableLayoutPanel5);
			this.grpPpuTiming.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpPpuTiming.Location = new System.Drawing.Point(3, 44);
			this.grpPpuTiming.Name = "grpPpuTiming";
			this.grpPpuTiming.Size = new System.Drawing.Size(513, 71);
			this.grpPpuTiming.TabIndex = 7;
			this.grpPpuTiming.TabStop = false;
			this.grpPpuTiming.Text = "PPU Vertical Blank Configuration";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.nudExtraScanlinesAfterNmi, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.nudExtraScanlinesBeforeNmi, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblExtraScanlinesBeforeNmi, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblExtraScanlinesAfterNmi, 0, 1);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 3;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(507, 52);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// nudExtraScanlinesAfterNmi
			// 
			this.nudExtraScanlinesAfterNmi.DecimalPlaces = 0;
			this.nudExtraScanlinesAfterNmi.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudExtraScanlinesAfterNmi.Location = new System.Drawing.Point(165, 30);
			this.nudExtraScanlinesAfterNmi.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudExtraScanlinesAfterNmi.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudExtraScanlinesAfterNmi.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudExtraScanlinesAfterNmi.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudExtraScanlinesAfterNmi.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudExtraScanlinesAfterNmi.Name = "nudExtraScanlinesAfterNmi";
			this.nudExtraScanlinesAfterNmi.Size = new System.Drawing.Size(46, 21);
			this.nudExtraScanlinesAfterNmi.TabIndex = 3;
			this.nudExtraScanlinesAfterNmi.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// nudExtraScanlinesBeforeNmi
			// 
			this.nudExtraScanlinesBeforeNmi.DecimalPlaces = 0;
			this.nudExtraScanlinesBeforeNmi.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudExtraScanlinesBeforeNmi.Location = new System.Drawing.Point(165, 3);
			this.nudExtraScanlinesBeforeNmi.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudExtraScanlinesBeforeNmi.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudExtraScanlinesBeforeNmi.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudExtraScanlinesBeforeNmi.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudExtraScanlinesBeforeNmi.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudExtraScanlinesBeforeNmi.Name = "nudExtraScanlinesBeforeNmi";
			this.nudExtraScanlinesBeforeNmi.Size = new System.Drawing.Size(46, 21);
			this.nudExtraScanlinesBeforeNmi.TabIndex = 2;
			this.nudExtraScanlinesBeforeNmi.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblExtraScanlinesBeforeNmi
			// 
			this.lblExtraScanlinesBeforeNmi.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExtraScanlinesBeforeNmi.AutoSize = true;
			this.lblExtraScanlinesBeforeNmi.Location = new System.Drawing.Point(3, 7);
			this.lblExtraScanlinesBeforeNmi.Name = "lblExtraScanlinesBeforeNmi";
			this.lblExtraScanlinesBeforeNmi.Size = new System.Drawing.Size(159, 13);
			this.lblExtraScanlinesBeforeNmi.TabIndex = 0;
			this.lblExtraScanlinesBeforeNmi.Text = "Additional scanlines before NMI:";
			// 
			// lblExtraScanlinesAfterNmi
			// 
			this.lblExtraScanlinesAfterNmi.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExtraScanlinesAfterNmi.AutoSize = true;
			this.lblExtraScanlinesAfterNmi.Location = new System.Drawing.Point(3, 34);
			this.lblExtraScanlinesAfterNmi.Name = "lblExtraScanlinesAfterNmi";
			this.lblExtraScanlinesAfterNmi.Size = new System.Drawing.Size(150, 13);
			this.lblExtraScanlinesAfterNmi.TabIndex = 1;
			this.lblExtraScanlinesAfterNmi.Text = "Additional scanlines after NMI:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblEffectiveClockRate);
			this.flowLayoutPanel2.Controls.Add(this.lblEffectiveClockRateValue);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 169);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(519, 17);
			this.flowLayoutPanel2.TabIndex = 8;
			// 
			// lblEffectiveClockRate
			// 
			this.lblEffectiveClockRate.AutoSize = true;
			this.lblEffectiveClockRate.Location = new System.Drawing.Point(3, 0);
			this.lblEffectiveClockRate.Name = "lblEffectiveClockRate";
			this.lblEffectiveClockRate.Size = new System.Drawing.Size(146, 13);
			this.lblEffectiveClockRate.TabIndex = 0;
			this.lblEffectiveClockRate.Text = "Effective Clock Rate (NTSC):";
			// 
			// lblEffectiveClockRateValue
			// 
			this.lblEffectiveClockRateValue.AutoSize = true;
			this.lblEffectiveClockRateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEffectiveClockRateValue.Location = new System.Drawing.Point(155, 0);
			this.lblEffectiveClockRateValue.Name = "lblEffectiveClockRateValue";
			this.lblEffectiveClockRateValue.Size = new System.Drawing.Size(37, 13);
			this.lblEffectiveClockRateValue.TabIndex = 1;
			this.lblEffectiveClockRateValue.Text = "100%";
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.Controls.Add(this.chkShowLagCounter);
			this.flowLayoutPanel7.Controls.Add(this.btnResetLagCounter);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 246);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(519, 43);
			this.flowLayoutPanel7.TabIndex = 12;
			// 
			// chkShowLagCounter
			// 
			this.chkShowLagCounter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkShowLagCounter.AutoSize = true;
			this.chkShowLagCounter.Location = new System.Drawing.Point(3, 6);
			this.chkShowLagCounter.Name = "chkShowLagCounter";
			this.chkShowLagCounter.Size = new System.Drawing.Size(114, 17);
			this.chkShowLagCounter.TabIndex = 13;
			this.chkShowLagCounter.Text = "Show Lag Counter";
			this.chkShowLagCounter.UseVisualStyleBackColor = true;
			// 
			// btnResetLagCounter
			// 
			this.btnResetLagCounter.AutoSize = true;
			this.btnResetLagCounter.Location = new System.Drawing.Point(123, 3);
			this.btnResetLagCounter.Name = "btnResetLagCounter";
			this.btnResetLagCounter.Size = new System.Drawing.Size(85, 23);
			this.btnResetLagCounter.TabIndex = 14;
			this.btnResetLagCounter.Text = "Reset Counter";
			this.btnResetLagCounter.UseVisualStyleBackColor = true;
			this.btnResetLagCounter.Click += new System.EventHandler(this.btnResetLagCounter_Click);
			// 
			// tmrUpdateClockRate
			// 
			this.tmrUpdateClockRate.Enabled = true;
			this.tmrUpdateClockRate.Tick += new System.EventHandler(this.tmrUpdateClockRate_Tick);
			// 
			// frmEmulationConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(533, 350);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(503, 322);
			this.Name = "frmEmulationConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Emulation Settings";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgGeneral.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.flowLayoutPanel9.ResumeLayout(false);
			this.flowLayoutPanel9.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.flowLayoutPanel10.ResumeLayout(false);
			this.flowLayoutPanel10.PerformLayout();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel8.ResumeLayout(false);
			this.flowLayoutPanel8.PerformLayout();
			this.tpgOverclocking.ResumeLayout(false);
			this.tpgOverclocking.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.grpOverclocking.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpPpuTiming.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkUseAlternativeMmc3Irq;
		private ctrlRiskyOption chkAllowInvalidInput;
		private System.Windows.Forms.TabPage tpgOverclocking;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox grpOverclocking;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblClockRate;
		private MesenNumericUpDown nudOverclockRate;
		private System.Windows.Forms.Label lblClockRatePercent;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private MesenNumericUpDown nudEmulationSpeed;
		private System.Windows.Forms.Label lblEmuSpeedHint;
		private System.Windows.Forms.Label lblEmulationSpeed;
		private System.Windows.Forms.GroupBox grpPpuTiming;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private MesenNumericUpDown nudExtraScanlinesAfterNmi;
		private MesenNumericUpDown nudExtraScanlinesBeforeNmi;
		private System.Windows.Forms.Label lblExtraScanlinesBeforeNmi;
		private System.Windows.Forms.Label lblExtraScanlinesAfterNmi;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblEffectiveClockRate;
		private System.Windows.Forms.Label lblEffectiveClockRateValue;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label lblEffectiveClockRatePal;
		private System.Windows.Forms.Timer tmrUpdateClockRate;
		private System.Windows.Forms.CheckBox chkOverclockAdjustApu;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblEffectiveClockRateDendy;
		private System.Windows.Forms.Label lblEffectiveClockRateValueDendy;
		private System.Windows.Forms.Label lblEffectiveClockRateValuePal;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.CheckBox chkShowLagCounter;
		private System.Windows.Forms.Button btnResetLagCounter;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
		private System.Windows.Forms.Label lblRamPowerOnState;
		private System.Windows.Forms.ComboBox cboRamPowerOnState;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
		private MesenNumericUpDown nudTurboSpeed;
		private System.Windows.Forms.Label lblTurboSpeed;
		private System.Windows.Forms.Label lblTurboSpeedHint;
		private ctrlRiskyOption chkDisablePpu2004Reads;
		private ctrlRiskyOption chkDisableOamAddrBug;
		private ctrlRiskyOption chkDisablePaletteRead;
		private ctrlRiskyOption chkDisablePpuReset;
		private Mesen.GUI.Controls.ctrlRiskyOption chkEnableOamDecay;
		private System.Windows.Forms.Label lblRewindSpeed;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel10;
		private MesenNumericUpDown nudRewindSpeed;
		private System.Windows.Forms.Label lblRewindSpeedHint;
		private ctrlRiskyOption chkMapperRandomPowerOnState;
		private System.Windows.Forms.PictureBox picHint;
		private Mesen.GUI.Controls.ctrlAutoGrowLabel lblOverclockHint;
		private System.Windows.Forms.CheckBox chkUseNes101Hvc101Behavior;
		private System.Windows.Forms.Label lblDeveloperSettings;
		private System.Windows.Forms.Label lblMiscSettings;
	}
}