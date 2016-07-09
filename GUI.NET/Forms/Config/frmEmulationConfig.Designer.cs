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
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudEmulationSpeed = new System.Windows.Forms.NumericUpDown();
			this.lblEmuSpeedHint = new System.Windows.Forms.Label();
			this.lblEmulationSpeed = new System.Windows.Forms.Label();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkUseAlternativeMmc3Irq = new System.Windows.Forms.CheckBox();
			this.chkAllowInvalidInput = new System.Windows.Forms.CheckBox();
			this.chkRemoveSpriteLimit = new System.Windows.Forms.CheckBox();
			this.tpgOverclocking = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRateDendy = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValueDendy = new System.Windows.Forms.Label();
			this.lblOverclockWarning = new System.Windows.Forms.Label();
			this.chkOverclockAdjustApu = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRatePal = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValuePal = new System.Windows.Forms.Label();
			this.grpOverclocking = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblClockRate = new System.Windows.Forms.Label();
			this.nudOverclockRate = new System.Windows.Forms.NumericUpDown();
			this.lblClockRatePercent = new System.Windows.Forms.Label();
			this.grpPpuTiming = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.nudExtraScanlinesAfterNmi = new System.Windows.Forms.NumericUpDown();
			this.nudExtraScanlinesBeforeNmi = new System.Windows.Forms.NumericUpDown();
			this.lblExtraScanlinesBeforeNmi = new System.Windows.Forms.Label();
			this.lblExtraScanlinesAfterNmi = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEffectiveClockRate = new System.Windows.Forms.Label();
			this.lblEffectiveClockRateValue = new System.Windows.Forms.Label();
			this.tmrUpdateClockRate = new System.Windows.Forms.Timer(this.components);
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEmulationSpeed)).BeginInit();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tpgOverclocking.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.grpOverclocking.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverclockRate)).BeginInit();
			this.grpPpuTiming.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudExtraScanlinesAfterNmi)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudExtraScanlinesBeforeNmi)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 268);
			this.baseConfigPanel.Size = new System.Drawing.Size(487, 29);
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
			this.tabMain.Size = new System.Drawing.Size(487, 268);
			this.tabMain.TabIndex = 2;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tableLayoutPanel4);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(479, 242);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel6, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblEmulationSpeed, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(473, 236);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.nudEmulationSpeed);
			this.flowLayoutPanel6.Controls.Add(this.lblEmuSpeedHint);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(96, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(377, 26);
			this.flowLayoutPanel6.TabIndex = 11;
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
			this.lblEmuSpeedHint.Size = new System.Drawing.Size(121, 13);
			this.lblEmuSpeedHint.TabIndex = 2;
			this.lblEmuSpeedHint.Text = "%  (0 = Maximum speed)";
			// 
			// lblEmulationSpeed
			// 
			this.lblEmulationSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEmulationSpeed.AutoSize = true;
			this.lblEmulationSpeed.Location = new System.Drawing.Point(3, 6);
			this.lblEmulationSpeed.Name = "lblEmulationSpeed";
			this.lblEmulationSpeed.Size = new System.Drawing.Size(90, 13);
			this.lblEmulationSpeed.TabIndex = 12;
			this.lblEmulationSpeed.Text = "Emulation Speed:";
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel1);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(479, 242);
			this.tpgAdvanced.TabIndex = 1;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkUseAlternativeMmc3Irq, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkAllowInvalidInput, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkRemoveSpriteLimit, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 236);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkUseAlternativeMmc3Irq
			// 
			this.chkUseAlternativeMmc3Irq.AutoSize = true;
			this.chkUseAlternativeMmc3Irq.Location = new System.Drawing.Point(3, 49);
			this.chkUseAlternativeMmc3Irq.Name = "chkUseAlternativeMmc3Irq";
			this.chkUseAlternativeMmc3Irq.Size = new System.Drawing.Size(197, 17);
			this.chkUseAlternativeMmc3Irq.TabIndex = 0;
			this.chkUseAlternativeMmc3Irq.Text = "Use alternative MMC3 IRQ behavior";
			this.chkUseAlternativeMmc3Irq.UseVisualStyleBackColor = true;
			// 
			// chkAllowInvalidInput
			// 
			this.chkAllowInvalidInput.AutoSize = true;
			this.chkAllowInvalidInput.Location = new System.Drawing.Point(3, 26);
			this.chkAllowInvalidInput.Name = "chkAllowInvalidInput";
			this.chkAllowInvalidInput.Size = new System.Drawing.Size(341, 17);
			this.chkAllowInvalidInput.TabIndex = 1;
			this.chkAllowInvalidInput.Text = "Allow invalid input (e.g Down + Up or Left + Right at the same time)";
			this.chkAllowInvalidInput.UseVisualStyleBackColor = true;
			// 
			// chkRemoveSpriteLimit
			// 
			this.chkRemoveSpriteLimit.AutoSize = true;
			this.chkRemoveSpriteLimit.Location = new System.Drawing.Point(3, 3);
			this.chkRemoveSpriteLimit.Name = "chkRemoveSpriteLimit";
			this.chkRemoveSpriteLimit.Size = new System.Drawing.Size(205, 17);
			this.chkRemoveSpriteLimit.TabIndex = 2;
			this.chkRemoveSpriteLimit.Text = "Remove sprite limit (Reduces flashing)";
			this.chkRemoveSpriteLimit.UseVisualStyleBackColor = true;
			// 
			// tpgOverclocking
			// 
			this.tpgOverclocking.Controls.Add(this.tableLayoutPanel3);
			this.tpgOverclocking.Location = new System.Drawing.Point(4, 22);
			this.tpgOverclocking.Name = "tpgOverclocking";
			this.tpgOverclocking.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOverclocking.Size = new System.Drawing.Size(479, 242);
			this.tpgOverclocking.TabIndex = 2;
			this.tpgOverclocking.Text = "Overclocking";
			this.tpgOverclocking.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 0, 5);
			this.tableLayoutPanel3.Controls.Add(this.lblOverclockWarning, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.chkOverclockAdjustApu, 0, 6);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.grpOverclocking, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpPpuTiming, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 3);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 8;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(473, 236);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.lblEffectiveClockRateDendy);
			this.flowLayoutPanel4.Controls.Add(this.lblEffectiveClockRateValueDendy);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 189);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(473, 20);
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
			// lblOverclockWarning
			// 
			this.lblOverclockWarning.AutoSize = true;
			this.lblOverclockWarning.ForeColor = System.Drawing.Color.Red;
			this.lblOverclockWarning.Location = new System.Drawing.Point(3, 5);
			this.lblOverclockWarning.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.lblOverclockWarning.Name = "lblOverclockWarning";
			this.lblOverclockWarning.Size = new System.Drawing.Size(384, 13);
			this.lblOverclockWarning.TabIndex = 2;
			this.lblOverclockWarning.Text = "WARNING: Overclocking will cause stability issues and may crash some games!";
			// 
			// chkOverclockAdjustApu
			// 
			this.chkOverclockAdjustApu.AutoSize = true;
			this.chkOverclockAdjustApu.Location = new System.Drawing.Point(3, 212);
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
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 172);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(473, 17);
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
			this.grpOverclocking.Location = new System.Drawing.Point(3, 26);
			this.grpOverclocking.Name = "grpOverclocking";
			this.grpOverclocking.Size = new System.Drawing.Size(467, 45);
			this.grpOverclocking.TabIndex = 6;
			this.grpOverclocking.TabStop = false;
			this.grpOverclocking.Text = "Overclocking";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel5, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(461, 26);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Controls.Add(this.lblClockRate);
			this.flowLayoutPanel5.Controls.Add(this.nudOverclockRate);
			this.flowLayoutPanel5.Controls.Add(this.lblClockRatePercent);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(0, 0);
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
			this.lblClockRate.Size = new System.Drawing.Size(107, 13);
			this.lblClockRate.TabIndex = 1;
			this.lblClockRate.Text = "Clock Rate Multiplier:";
			// 
			// nudOverclockRate
			// 
			this.nudOverclockRate.Location = new System.Drawing.Point(110, 3);
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
			this.nudOverclockRate.Validated += new System.EventHandler(this.OverclockConfig_Validated);
			// 
			// lblClockRatePercent
			// 
			this.lblClockRatePercent.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClockRatePercent.AutoSize = true;
			this.lblClockRatePercent.Location = new System.Drawing.Point(156, 6);
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
			this.grpPpuTiming.Location = new System.Drawing.Point(3, 77);
			this.grpPpuTiming.Name = "grpPpuTiming";
			this.grpPpuTiming.Size = new System.Drawing.Size(467, 75);
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
			this.tableLayoutPanel5.Size = new System.Drawing.Size(461, 56);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// nudExtraScanlinesAfterNmi
			// 
			this.nudExtraScanlinesAfterNmi.Location = new System.Drawing.Point(171, 29);
			this.nudExtraScanlinesAfterNmi.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudExtraScanlinesAfterNmi.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudExtraScanlinesAfterNmi.Name = "nudExtraScanlinesAfterNmi";
			this.nudExtraScanlinesAfterNmi.Size = new System.Drawing.Size(46, 20);
			this.nudExtraScanlinesAfterNmi.TabIndex = 3;
			this.nudExtraScanlinesAfterNmi.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudExtraScanlinesAfterNmi.Validated += new System.EventHandler(this.OverclockConfig_Validated);
			// 
			// nudExtraScanlinesBeforeNmi
			// 
			this.nudExtraScanlinesBeforeNmi.Location = new System.Drawing.Point(171, 3);
			this.nudExtraScanlinesBeforeNmi.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.nudExtraScanlinesBeforeNmi.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudExtraScanlinesBeforeNmi.Name = "nudExtraScanlinesBeforeNmi";
			this.nudExtraScanlinesBeforeNmi.Size = new System.Drawing.Size(46, 20);
			this.nudExtraScanlinesBeforeNmi.TabIndex = 2;
			this.nudExtraScanlinesBeforeNmi.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudExtraScanlinesBeforeNmi.Validated += new System.EventHandler(this.OverclockConfig_Validated);
			// 
			// lblExtraScanlinesBeforeNmi
			// 
			this.lblExtraScanlinesBeforeNmi.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExtraScanlinesBeforeNmi.AutoSize = true;
			this.lblExtraScanlinesBeforeNmi.Location = new System.Drawing.Point(3, 6);
			this.lblExtraScanlinesBeforeNmi.Name = "lblExtraScanlinesBeforeNmi";
			this.lblExtraScanlinesBeforeNmi.Size = new System.Drawing.Size(165, 13);
			this.lblExtraScanlinesBeforeNmi.TabIndex = 0;
			this.lblExtraScanlinesBeforeNmi.Text = "Additionnal scanlines before NMI:";
			// 
			// lblExtraScanlinesAfterNmi
			// 
			this.lblExtraScanlinesAfterNmi.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExtraScanlinesAfterNmi.AutoSize = true;
			this.lblExtraScanlinesAfterNmi.Location = new System.Drawing.Point(3, 32);
			this.lblExtraScanlinesAfterNmi.Name = "lblExtraScanlinesAfterNmi";
			this.lblExtraScanlinesAfterNmi.Size = new System.Drawing.Size(156, 13);
			this.lblExtraScanlinesAfterNmi.TabIndex = 1;
			this.lblExtraScanlinesAfterNmi.Text = "Additionnal scanlines after NMI:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblEffectiveClockRate);
			this.flowLayoutPanel2.Controls.Add(this.lblEffectiveClockRateValue);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 155);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(473, 17);
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
			// tmrUpdateClockRate
			// 
			this.tmrUpdateClockRate.Enabled = true;
			this.tmrUpdateClockRate.Tick += new System.EventHandler(this.tmrUpdateClockRate_Tick);
			// 
			// frmEmulationConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 297);
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
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEmulationSpeed)).EndInit();
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tpgOverclocking.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.grpOverclocking.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverclockRate)).EndInit();
			this.grpPpuTiming.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudExtraScanlinesAfterNmi)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudExtraScanlinesBeforeNmi)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkUseAlternativeMmc3Irq;
		private System.Windows.Forms.CheckBox chkAllowInvalidInput;
		private System.Windows.Forms.CheckBox chkRemoveSpriteLimit;
		private System.Windows.Forms.TabPage tpgOverclocking;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox grpOverclocking;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblOverclockWarning;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Label lblClockRate;
		private System.Windows.Forms.NumericUpDown nudOverclockRate;
		private System.Windows.Forms.Label lblClockRatePercent;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.NumericUpDown nudEmulationSpeed;
		private System.Windows.Forms.Label lblEmuSpeedHint;
		private System.Windows.Forms.Label lblEmulationSpeed;
		private System.Windows.Forms.GroupBox grpPpuTiming;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.NumericUpDown nudExtraScanlinesAfterNmi;
		private System.Windows.Forms.NumericUpDown nudExtraScanlinesBeforeNmi;
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
	}
}