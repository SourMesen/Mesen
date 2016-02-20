namespace Mesen.GUI.Forms.Config
{
	partial class frmAudioConfig
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
			this.grpVolume = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.trkDmcVol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkNoiseVol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkTriangleVol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkSquare2Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkSquare1Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkMaster = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkFdsVol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkMmc5Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkVrc6Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkVrc7Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkNamco163Vol = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkSunsoft5b = new Mesen.GUI.Controls.ctrlTrackbar();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkMuteSoundInBackground = new System.Windows.Forms.CheckBox();
			this.chkReduceSoundInBackground = new System.Windows.Forms.CheckBox();
			this.chkEnableAudio = new System.Windows.Forms.CheckBox();
			this.lblSampleRate = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.nudLatency = new System.Windows.Forms.NumericUpDown();
			this.lblLatencyMs = new System.Windows.Forms.Label();
			this.lblAudioLatency = new System.Windows.Forms.Label();
			this.cboSampleRate = new System.Windows.Forms.ComboBox();
			this.lblAudioDevice = new System.Windows.Forms.Label();
			this.cboAudioDevice = new System.Windows.Forms.ComboBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgVolume = new System.Windows.Forms.TabPage();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.chkSwapDutyCycles = new System.Windows.Forms.CheckBox();
			this.baseConfigPanel.SuspendLayout();
			this.grpVolume.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLatency)).BeginInit();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgVolume.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 375);
			this.baseConfigPanel.Size = new System.Drawing.Size(477, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// grpVolume
			// 
			this.grpVolume.Controls.Add(this.tableLayoutPanel1);
			this.grpVolume.Location = new System.Drawing.Point(3, 6);
			this.grpVolume.Name = "grpVolume";
			this.grpVolume.Size = new System.Drawing.Size(462, 341);
			this.grpVolume.TabIndex = 2;
			this.grpVolume.TabStop = false;
			this.grpVolume.Text = "Volume";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.Controls.Add(this.trkDmcVol, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkNoiseVol, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkTriangleVol, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkSquare2Vol, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkSquare1Vol, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkMaster, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkFdsVol, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.trkMmc5Vol, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.trkVrc6Vol, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.trkVrc7Vol, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.trkNamco163Vol, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.trkSunsoft5b, 5, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(456, 322);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// trkDmcVol
			// 
			this.trkDmcVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkDmcVol.Text = "DMC";
			this.trkDmcVol.Location = new System.Drawing.Point(384, 0);
			this.trkDmcVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkDmcVol.Maximum = 100;
			this.trkDmcVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkDmcVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkDmcVol.Name = "trkDmcVol";
			this.trkDmcVol.Size = new System.Drawing.Size(63, 160);
			this.trkDmcVol.TabIndex = 16;
			this.trkDmcVol.Value = 50;
			// 
			// trkNoiseVol
			// 
			this.trkNoiseVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNoiseVol.Text = "Noise";
			this.trkNoiseVol.Location = new System.Drawing.Point(306, 0);
			this.trkNoiseVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkNoiseVol.Maximum = 100;
			this.trkNoiseVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.Name = "trkNoiseVol";
			this.trkNoiseVol.Size = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.TabIndex = 15;
			this.trkNoiseVol.Value = 50;
			// 
			// trkTriangleVol
			// 
			this.trkTriangleVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkTriangleVol.Text = "Triangle";
			this.trkTriangleVol.Location = new System.Drawing.Point(231, 0);
			this.trkTriangleVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkTriangleVol.Maximum = 100;
			this.trkTriangleVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.Name = "trkTriangleVol";
			this.trkTriangleVol.Size = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.TabIndex = 14;
			this.trkTriangleVol.Value = 50;
			// 
			// trkSquare2Vol
			// 
			this.trkSquare2Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare2Vol.Text = "Square 2";
			this.trkSquare2Vol.Location = new System.Drawing.Point(156, 0);
			this.trkSquare2Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare2Vol.Maximum = 100;
			this.trkSquare2Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.Name = "trkSquare2Vol";
			this.trkSquare2Vol.Size = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.TabIndex = 13;
			this.trkSquare2Vol.Value = 50;
			// 
			// trkSquare1Vol
			// 
			this.trkSquare1Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare1Vol.Text = "Square 1";
			this.trkSquare1Vol.Location = new System.Drawing.Point(81, 0);
			this.trkSquare1Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare1Vol.Maximum = 100;
			this.trkSquare1Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.Name = "trkSquare1Vol";
			this.trkSquare1Vol.Size = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.TabIndex = 12;
			this.trkSquare1Vol.Value = 50;
			// 
			// trkMaster
			// 
			this.trkMaster.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkMaster.Text = "Master";
			this.trkMaster.Location = new System.Drawing.Point(6, 0);
			this.trkMaster.Margin = new System.Windows.Forms.Padding(0);
			this.trkMaster.Maximum = 100;
			this.trkMaster.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.Name = "trkMaster";
			this.trkMaster.Size = new System.Drawing.Size(63, 160);
			this.trkMaster.TabIndex = 11;
			this.trkMaster.Value = 50;
			// 
			// trkFdsVol
			// 
			this.trkFdsVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkFdsVol.Text = "FDS";
			this.trkFdsVol.Location = new System.Drawing.Point(6, 160);
			this.trkFdsVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkFdsVol.Maximum = 100;
			this.trkFdsVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkFdsVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkFdsVol.Name = "trkFdsVol";
			this.trkFdsVol.Size = new System.Drawing.Size(63, 160);
			this.trkFdsVol.TabIndex = 17;
			this.trkFdsVol.Value = 50;
			// 
			// trkMmc5Vol
			// 
			this.trkMmc5Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkMmc5Vol.Text = "MMC5";
			this.trkMmc5Vol.Enabled = false;
			this.trkMmc5Vol.Location = new System.Drawing.Point(81, 160);
			this.trkMmc5Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkMmc5Vol.Maximum = 100;
			this.trkMmc5Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.Name = "trkMmc5Vol";
			this.trkMmc5Vol.Size = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.TabIndex = 18;
			this.trkMmc5Vol.Value = 50;
			// 
			// trkVrc6Vol
			// 
			this.trkVrc6Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc6Vol.Text = "VRC6";
			this.trkVrc6Vol.Enabled = false;
			this.trkVrc6Vol.Location = new System.Drawing.Point(156, 160);
			this.trkVrc6Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc6Vol.Maximum = 100;
			this.trkVrc6Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.Name = "trkVrc6Vol";
			this.trkVrc6Vol.Size = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.TabIndex = 19;
			this.trkVrc6Vol.Value = 50;
			// 
			// trkVrc7Vol
			// 
			this.trkVrc7Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc7Vol.Text = "VRC7";
			this.trkVrc7Vol.Enabled = false;
			this.trkVrc7Vol.Location = new System.Drawing.Point(231, 160);
			this.trkVrc7Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc7Vol.Maximum = 100;
			this.trkVrc7Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.Name = "trkVrc7Vol";
			this.trkVrc7Vol.Size = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.TabIndex = 20;
			this.trkVrc7Vol.Value = 50;
			// 
			// trkNamco163Vol
			// 
			this.trkNamco163Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNamco163Vol.Text = "Namco";
			this.trkNamco163Vol.Enabled = false;
			this.trkNamco163Vol.Location = new System.Drawing.Point(306, 160);
			this.trkNamco163Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkNamco163Vol.Maximum = 100;
			this.trkNamco163Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.Name = "trkNamco163Vol";
			this.trkNamco163Vol.Size = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.TabIndex = 21;
			this.trkNamco163Vol.Value = 50;
			// 
			// trkSunsoft5b
			// 
			this.trkSunsoft5b.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSunsoft5b.Text = "Sunsoft";
			this.trkSunsoft5b.Enabled = false;
			this.trkSunsoft5b.Location = new System.Drawing.Point(384, 160);
			this.trkSunsoft5b.Margin = new System.Windows.Forms.Padding(0);
			this.trkSunsoft5b.Maximum = 100;
			this.trkSunsoft5b.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.Name = "trkSunsoft5b";
			this.trkSunsoft5b.Size = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.TabIndex = 22;
			this.trkSunsoft5b.Value = 50;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.chkMuteSoundInBackground, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkReduceSoundInBackground, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.chkEnableAudio, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSampleRate, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblAudioLatency, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.cboSampleRate, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblAudioDevice, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.cboAudioDevice, 1, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 6;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(463, 343);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// chkMuteSoundInBackground
			// 
			this.chkMuteSoundInBackground.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkMuteSoundInBackground, 2);
			this.chkMuteSoundInBackground.Location = new System.Drawing.Point(3, 107);
			this.chkMuteSoundInBackground.Name = "chkMuteSoundInBackground";
			this.chkMuteSoundInBackground.Size = new System.Drawing.Size(182, 17);
			this.chkMuteSoundInBackground.TabIndex = 14;
			this.chkMuteSoundInBackground.Text = "Mute sound when in background";
			this.chkMuteSoundInBackground.UseVisualStyleBackColor = true;
			this.chkMuteSoundInBackground.CheckedChanged += new System.EventHandler(this.chkMuteWhenInBackground_CheckedChanged);
			// 
			// chkReduceSoundInBackground
			// 
			this.chkReduceSoundInBackground.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkReduceSoundInBackground, 2);
			this.chkReduceSoundInBackground.Location = new System.Drawing.Point(3, 130);
			this.chkReduceSoundInBackground.Name = "chkReduceSoundInBackground";
			this.chkReduceSoundInBackground.Size = new System.Drawing.Size(201, 17);
			this.chkReduceSoundInBackground.TabIndex = 13;
			this.chkReduceSoundInBackground.Text = "Reduce volume when in background";
			this.chkReduceSoundInBackground.UseVisualStyleBackColor = true;
			// 
			// chkEnableAudio
			// 
			this.chkEnableAudio.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkEnableAudio, 2);
			this.chkEnableAudio.Location = new System.Drawing.Point(6, 6);
			this.chkEnableAudio.Margin = new System.Windows.Forms.Padding(6, 6, 6, 3);
			this.chkEnableAudio.Name = "chkEnableAudio";
			this.chkEnableAudio.Size = new System.Drawing.Size(89, 17);
			this.chkEnableAudio.TabIndex = 3;
			this.chkEnableAudio.Text = "Enable Audio";
			this.chkEnableAudio.UseVisualStyleBackColor = true;
			// 
			// lblSampleRate
			// 
			this.lblSampleRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSampleRate.AutoSize = true;
			this.lblSampleRate.Location = new System.Drawing.Point(3, 60);
			this.lblSampleRate.Name = "lblSampleRate";
			this.lblSampleRate.Size = new System.Drawing.Size(71, 13);
			this.lblSampleRate.TabIndex = 0;
			this.lblSampleRate.Text = "Sample Rate:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.nudLatency);
			this.flowLayoutPanel2.Controls.Add(this.lblLatencyMs);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(77, 80);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(78, 24);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// nudLatency
			// 
			this.nudLatency.Location = new System.Drawing.Point(3, 3);
			this.nudLatency.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.nudLatency.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudLatency.Name = "nudLatency";
			this.nudLatency.Size = new System.Drawing.Size(45, 20);
			this.nudLatency.TabIndex = 1;
			this.nudLatency.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblLatencyMs
			// 
			this.lblLatencyMs.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLatencyMs.AutoSize = true;
			this.lblLatencyMs.Location = new System.Drawing.Point(54, 6);
			this.lblLatencyMs.Name = "lblLatencyMs";
			this.lblLatencyMs.Size = new System.Drawing.Size(20, 13);
			this.lblLatencyMs.TabIndex = 2;
			this.lblLatencyMs.Text = "ms";
			// 
			// lblAudioLatency
			// 
			this.lblAudioLatency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAudioLatency.AutoSize = true;
			this.lblAudioLatency.Location = new System.Drawing.Point(3, 85);
			this.lblAudioLatency.Name = "lblAudioLatency";
			this.lblAudioLatency.Size = new System.Drawing.Size(48, 13);
			this.lblAudioLatency.TabIndex = 0;
			this.lblAudioLatency.Text = "Latency:";
			// 
			// cboSampleRate
			// 
			this.cboSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSampleRate.FormattingEnabled = true;
			this.cboSampleRate.Items.AddRange(new object[] {
            "11,025 Hz",
            "22,050 Hz",
            "44,100 Hz",
            "48,000 Hz"});
			this.cboSampleRate.Location = new System.Drawing.Point(80, 56);
			this.cboSampleRate.Name = "cboSampleRate";
			this.cboSampleRate.Size = new System.Drawing.Size(75, 21);
			this.cboSampleRate.TabIndex = 5;
			// 
			// lblAudioDevice
			// 
			this.lblAudioDevice.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAudioDevice.AutoSize = true;
			this.lblAudioDevice.Location = new System.Drawing.Point(3, 33);
			this.lblAudioDevice.Name = "lblAudioDevice";
			this.lblAudioDevice.Size = new System.Drawing.Size(44, 13);
			this.lblAudioDevice.TabIndex = 6;
			this.lblAudioDevice.Text = "Device:";
			// 
			// cboAudioDevice
			// 
			this.cboAudioDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAudioDevice.FormattingEnabled = true;
			this.cboAudioDevice.Location = new System.Drawing.Point(80, 29);
			this.cboAudioDevice.Name = "cboAudioDevice";
			this.cboAudioDevice.Size = new System.Drawing.Size(209, 21);
			this.cboAudioDevice.TabIndex = 7;
			// 
			// btnReset
			// 
			this.btnReset.AutoSize = true;
			this.btnReset.Location = new System.Drawing.Point(6, 3);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(99, 23);
			this.btnReset.TabIndex = 3;
			this.btnReset.Text = "Reset to Defaults";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgGeneral);
			this.tabMain.Controls.Add(this.tpgVolume);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(477, 375);
			this.tabMain.TabIndex = 4;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tableLayoutPanel2);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(469, 349);
			this.tpgGeneral.TabIndex = 0;
			this.tpgGeneral.Text = "General";
			this.tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// tpgVolume
			// 
			this.tpgVolume.Controls.Add(this.grpVolume);
			this.tpgVolume.Location = new System.Drawing.Point(4, 22);
			this.tpgVolume.Name = "tpgVolume";
			this.tpgVolume.Padding = new System.Windows.Forms.Padding(3);
			this.tpgVolume.Size = new System.Drawing.Size(469, 349);
			this.tpgVolume.TabIndex = 1;
			this.tpgVolume.Text = "Volume";
			this.tpgVolume.UseVisualStyleBackColor = true;
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel3);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(469, 349);
			this.tpgAdvanced.TabIndex = 2;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.chkSwapDutyCycles, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(463, 343);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// chkSwapDutyCycles
			// 
			this.chkSwapDutyCycles.AutoSize = true;
			this.chkSwapDutyCycles.Location = new System.Drawing.Point(3, 3);
			this.chkSwapDutyCycles.Name = "chkSwapDutyCycles";
			this.chkSwapDutyCycles.Size = new System.Drawing.Size(282, 17);
			this.chkSwapDutyCycles.TabIndex = 0;
			this.chkSwapDutyCycles.Text = "Swap square channels duty cycles (Mimics old clones)";
			this.chkSwapDutyCycles.UseVisualStyleBackColor = true;
			// 
			// frmAudioConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(477, 404);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAudioConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Audio Options";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.baseConfigPanel.PerformLayout();
			this.grpVolume.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLatency)).EndInit();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgVolume.ResumeLayout(false);
			this.tpgAdvanced.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpVolume;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkEnableAudio;
		private System.Windows.Forms.Label lblAudioLatency;
		private System.Windows.Forms.NumericUpDown nudLatency;
		private System.Windows.Forms.Label lblLatencyMs;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label lblSampleRate;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.ComboBox cboSampleRate;
		private System.Windows.Forms.Label lblAudioDevice;
		private System.Windows.Forms.ComboBox cboAudioDevice;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Controls.ctrlTrackbar trkDmcVol;
		private Controls.ctrlTrackbar trkNoiseVol;
		private Controls.ctrlTrackbar trkTriangleVol;
		private Controls.ctrlTrackbar trkSquare2Vol;
		private Controls.ctrlTrackbar trkSquare1Vol;
		private Controls.ctrlTrackbar trkMaster;
		private Controls.ctrlTrackbar trkFdsVol;
		private Controls.ctrlTrackbar trkMmc5Vol;
		private Controls.ctrlTrackbar trkVrc6Vol;
		private Controls.ctrlTrackbar trkVrc7Vol;
		private Controls.ctrlTrackbar trkNamco163Vol;
		private Controls.ctrlTrackbar trkSunsoft5b;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgVolume;
		private System.Windows.Forms.CheckBox chkMuteSoundInBackground;
		private System.Windows.Forms.CheckBox chkReduceSoundInBackground;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox chkSwapDutyCycles;
	}
}