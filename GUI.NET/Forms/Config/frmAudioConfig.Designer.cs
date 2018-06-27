using Mesen.GUI.Controls;

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
			this.lblVolumeReductionSettings = new System.Windows.Forms.Label();
			this.chkEnableAudio = new System.Windows.Forms.CheckBox();
			this.lblSampleRate = new System.Windows.Forms.Label();
			this.lblAudioLatency = new System.Windows.Forms.Label();
			this.cboSampleRate = new System.Windows.Forms.ComboBox();
			this.lblAudioDevice = new System.Windows.Forms.Label();
			this.cboAudioDevice = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLatencyWarning = new System.Windows.Forms.Label();
			this.picLatencyWarning = new System.Windows.Forms.PictureBox();
			this.lblLatencyMs = new System.Windows.Forms.Label();
			this.nudLatency = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.chkReduceSoundInBackground = new System.Windows.Forms.CheckBox();
			this.chkReduceSoundInFastForward = new System.Windows.Forms.CheckBox();
			this.trkVolumeReduction = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.chkMuteSoundInBackground = new System.Windows.Forms.CheckBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgGeneral = new System.Windows.Forms.TabPage();
			this.tpgVolume = new System.Windows.Forms.TabPage();
			this.tpgPanning = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.trkSquare1Pan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkFdsPan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkSquare2Pan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkMmc5Pan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkTrianglePan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkNoisePan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkDmcPan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkVrc6Pan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkVrc7Pan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkNamcoPan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.trkSunsoftPan = new Mesen.GUI.Controls.ctrlHorizontalTrackbar();
			this.tpgEqualizer = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkEnableEqualizer = new System.Windows.Forms.CheckBox();
			this.tlpEqualizer = new System.Windows.Forms.TableLayoutPanel();
			this.trkBand6Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand5Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand4Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand3Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand2Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand1Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand11Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand12Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand13Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand14Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand15Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand16Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand7Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand8Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand9Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand10Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand17Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand18Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand19Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.trkBand20Gain = new Mesen.GUI.Controls.ctrlTrackbar();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEqualizerPreset = new System.Windows.Forms.Label();
			this.cboEqualizerPreset = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblEqualizerFilterType = new System.Windows.Forms.Label();
			this.cboEqualizerFilterType = new System.Windows.Forms.ComboBox();
			this.tpgEffects = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.grpStereo = new System.Windows.Forms.GroupBox();
			this.tlpStereoFilter = new System.Windows.Forms.TableLayoutPanel();
			this.lblStereoDelayMs = new System.Windows.Forms.Label();
			this.lblStereoPanningAngle = new System.Windows.Forms.Label();
			this.radStereoDisabled = new System.Windows.Forms.RadioButton();
			this.radStereoDelay = new System.Windows.Forms.RadioButton();
			this.radStereoPanning = new System.Windows.Forms.RadioButton();
			this.nudStereoDelay = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.nudStereoPanning = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.grpReverb = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.chkReverbEnabled = new System.Windows.Forms.CheckBox();
			this.lblReverbStrength = new System.Windows.Forms.Label();
			this.lblReverbDelay = new System.Windows.Forms.Label();
			this.trkReverbDelay = new System.Windows.Forms.TrackBar();
			this.trkReverbStrength = new System.Windows.Forms.TrackBar();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkCrossFeedEnabled = new System.Windows.Forms.CheckBox();
			this.nudCrossFeedRatio = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblCrossFeedRatio = new System.Windows.Forms.Label();
			this.tpgAdvanced = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDisableNoiseModeFlag = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkSilenceTriangleHighFreq = new System.Windows.Forms.CheckBox();
			this.chkSwapDutyCycles = new Mesen.GUI.Controls.ctrlRiskyOption();
			this.chkReduceDmcPopping = new System.Windows.Forms.CheckBox();
			this.baseConfigPanel.SuspendLayout();
			this.grpVolume.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLatencyWarning)).BeginInit();
			this.tableLayoutPanel8.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgGeneral.SuspendLayout();
			this.tpgVolume.SuspendLayout();
			this.tpgPanning.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tpgEqualizer.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tlpEqualizer.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.tpgEffects.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.grpStereo.SuspendLayout();
			this.tlpStereoFilter.SuspendLayout();
			this.grpReverb.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkReverbDelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkReverbStrength)).BeginInit();
			this.flowLayoutPanel5.SuspendLayout();
			this.tpgAdvanced.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 373);
			this.baseConfigPanel.Size = new System.Drawing.Size(477, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// grpVolume
			// 
			this.grpVolume.Controls.Add(this.tableLayoutPanel1);
			this.grpVolume.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpVolume.Location = new System.Drawing.Point(3, 3);
			this.grpVolume.Name = "grpVolume";
			this.grpVolume.Size = new System.Drawing.Size(463, 341);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(457, 322);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// trkDmcVol
			// 
			this.trkDmcVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkDmcVol.Location = new System.Drawing.Point(387, 0);
			this.trkDmcVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkDmcVol.Maximum = 100;
			this.trkDmcVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkDmcVol.Minimum = 0;
			this.trkDmcVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkDmcVol.Name = "trkDmcVol";
			this.trkDmcVol.Size = new System.Drawing.Size(63, 160);
			this.trkDmcVol.TabIndex = 16;
			this.trkDmcVol.Text = "DMC";
			this.trkDmcVol.Value = 50;
			// 
			// trkNoiseVol
			// 
			this.trkNoiseVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNoiseVol.Location = new System.Drawing.Point(310, 0);
			this.trkNoiseVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkNoiseVol.Maximum = 100;
			this.trkNoiseVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.Minimum = 0;
			this.trkNoiseVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.Name = "trkNoiseVol";
			this.trkNoiseVol.Size = new System.Drawing.Size(63, 160);
			this.trkNoiseVol.TabIndex = 15;
			this.trkNoiseVol.Text = "Noise";
			this.trkNoiseVol.Value = 50;
			// 
			// trkTriangleVol
			// 
			this.trkTriangleVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkTriangleVol.Location = new System.Drawing.Point(234, 0);
			this.trkTriangleVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkTriangleVol.Maximum = 100;
			this.trkTriangleVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.Minimum = 0;
			this.trkTriangleVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.Name = "trkTriangleVol";
			this.trkTriangleVol.Size = new System.Drawing.Size(63, 160);
			this.trkTriangleVol.TabIndex = 14;
			this.trkTriangleVol.Text = "Triangle";
			this.trkTriangleVol.Value = 50;
			// 
			// trkSquare2Vol
			// 
			this.trkSquare2Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare2Vol.Location = new System.Drawing.Point(158, 0);
			this.trkSquare2Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare2Vol.Maximum = 100;
			this.trkSquare2Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.Minimum = 0;
			this.trkSquare2Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.Name = "trkSquare2Vol";
			this.trkSquare2Vol.Size = new System.Drawing.Size(63, 160);
			this.trkSquare2Vol.TabIndex = 13;
			this.trkSquare2Vol.Text = "Square 2";
			this.trkSquare2Vol.Value = 50;
			// 
			// trkSquare1Vol
			// 
			this.trkSquare1Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare1Vol.Location = new System.Drawing.Point(82, 0);
			this.trkSquare1Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare1Vol.Maximum = 100;
			this.trkSquare1Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.Minimum = 0;
			this.trkSquare1Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.Name = "trkSquare1Vol";
			this.trkSquare1Vol.Size = new System.Drawing.Size(63, 160);
			this.trkSquare1Vol.TabIndex = 12;
			this.trkSquare1Vol.Text = "Square 1";
			this.trkSquare1Vol.Value = 50;
			// 
			// trkMaster
			// 
			this.trkMaster.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkMaster.Location = new System.Drawing.Point(6, 0);
			this.trkMaster.Margin = new System.Windows.Forms.Padding(0);
			this.trkMaster.Maximum = 100;
			this.trkMaster.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.Minimum = 0;
			this.trkMaster.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.Name = "trkMaster";
			this.trkMaster.Size = new System.Drawing.Size(63, 160);
			this.trkMaster.TabIndex = 11;
			this.trkMaster.Text = "Master";
			this.trkMaster.Value = 50;
			// 
			// trkFdsVol
			// 
			this.trkFdsVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkFdsVol.Location = new System.Drawing.Point(6, 160);
			this.trkFdsVol.Margin = new System.Windows.Forms.Padding(0);
			this.trkFdsVol.Maximum = 100;
			this.trkFdsVol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkFdsVol.Minimum = 0;
			this.trkFdsVol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkFdsVol.Name = "trkFdsVol";
			this.trkFdsVol.Size = new System.Drawing.Size(63, 160);
			this.trkFdsVol.TabIndex = 17;
			this.trkFdsVol.Text = "FDS";
			this.trkFdsVol.Value = 50;
			// 
			// trkMmc5Vol
			// 
			this.trkMmc5Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkMmc5Vol.Location = new System.Drawing.Point(82, 160);
			this.trkMmc5Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkMmc5Vol.Maximum = 100;
			this.trkMmc5Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.Minimum = 0;
			this.trkMmc5Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.Name = "trkMmc5Vol";
			this.trkMmc5Vol.Size = new System.Drawing.Size(63, 160);
			this.trkMmc5Vol.TabIndex = 18;
			this.trkMmc5Vol.Text = "MMC5";
			this.trkMmc5Vol.Value = 50;
			// 
			// trkVrc6Vol
			// 
			this.trkVrc6Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc6Vol.Location = new System.Drawing.Point(158, 160);
			this.trkVrc6Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc6Vol.Maximum = 100;
			this.trkVrc6Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.Minimum = 0;
			this.trkVrc6Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.Name = "trkVrc6Vol";
			this.trkVrc6Vol.Size = new System.Drawing.Size(63, 160);
			this.trkVrc6Vol.TabIndex = 19;
			this.trkVrc6Vol.Text = "VRC6";
			this.trkVrc6Vol.Value = 50;
			// 
			// trkVrc7Vol
			// 
			this.trkVrc7Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc7Vol.Location = new System.Drawing.Point(234, 160);
			this.trkVrc7Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc7Vol.Maximum = 100;
			this.trkVrc7Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.Minimum = 0;
			this.trkVrc7Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.Name = "trkVrc7Vol";
			this.trkVrc7Vol.Size = new System.Drawing.Size(63, 160);
			this.trkVrc7Vol.TabIndex = 20;
			this.trkVrc7Vol.Text = "VRC7";
			this.trkVrc7Vol.Value = 50;
			// 
			// trkNamco163Vol
			// 
			this.trkNamco163Vol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNamco163Vol.Location = new System.Drawing.Point(310, 160);
			this.trkNamco163Vol.Margin = new System.Windows.Forms.Padding(0);
			this.trkNamco163Vol.Maximum = 100;
			this.trkNamco163Vol.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.Minimum = 0;
			this.trkNamco163Vol.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.Name = "trkNamco163Vol";
			this.trkNamco163Vol.Size = new System.Drawing.Size(63, 160);
			this.trkNamco163Vol.TabIndex = 21;
			this.trkNamco163Vol.Text = "Namco";
			this.trkNamco163Vol.Value = 50;
			// 
			// trkSunsoft5b
			// 
			this.trkSunsoft5b.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSunsoft5b.Location = new System.Drawing.Point(387, 160);
			this.trkSunsoft5b.Margin = new System.Windows.Forms.Padding(0);
			this.trkSunsoft5b.Maximum = 100;
			this.trkSunsoft5b.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.Minimum = 0;
			this.trkSunsoft5b.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.Name = "trkSunsoft5b";
			this.trkSunsoft5b.Size = new System.Drawing.Size(63, 160);
			this.trkSunsoft5b.TabIndex = 22;
			this.trkSunsoft5b.Text = "Sunsoft";
			this.trkSunsoft5b.Value = 50;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.lblVolumeReductionSettings, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkEnableAudio, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSampleRate, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblAudioLatency, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.cboSampleRate, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblAudioDevice, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.cboAudioDevice, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel7, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 0, 5);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 9;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(463, 341);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// lblVolumeReductionSettings
			// 
			this.lblVolumeReductionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVolumeReductionSettings.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.lblVolumeReductionSettings, 2);
			this.lblVolumeReductionSettings.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblVolumeReductionSettings.Location = new System.Drawing.Point(0, 116);
			this.lblVolumeReductionSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblVolumeReductionSettings.Name = "lblVolumeReductionSettings";
			this.lblVolumeReductionSettings.Size = new System.Drawing.Size(94, 13);
			this.lblVolumeReductionSettings.TabIndex = 24;
			this.lblVolumeReductionSettings.Text = "Volume Reduction";
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
			// lblAudioLatency
			// 
			this.lblAudioLatency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAudioLatency.AutoSize = true;
			this.lblAudioLatency.Location = new System.Drawing.Point(3, 87);
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
            "48,000 Hz",
            "96,000 Hz"});
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
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 4;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add(this.lblLatencyWarning, 3, 0);
			this.tableLayoutPanel7.Controls.Add(this.picLatencyWarning, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.lblLatencyMs, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.nudLatency, 0, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(77, 80);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 1;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(386, 27);
			this.tableLayoutPanel7.TabIndex = 15;
			// 
			// lblLatencyWarning
			// 
			this.lblLatencyWarning.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLatencyWarning.AutoSize = true;
			this.lblLatencyWarning.Location = new System.Drawing.Point(98, 7);
			this.lblLatencyWarning.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblLatencyWarning.Name = "lblLatencyWarning";
			this.lblLatencyWarning.Size = new System.Drawing.Size(192, 13);
			this.lblLatencyWarning.TabIndex = 4;
			this.lblLatencyWarning.Text = "Low values may cause sound problems";
			// 
			// picLatencyWarning
			// 
			this.picLatencyWarning.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picLatencyWarning.BackgroundImage = global::Mesen.GUI.Properties.Resources.Warning;
			this.picLatencyWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picLatencyWarning.Location = new System.Drawing.Point(82, 5);
			this.picLatencyWarning.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this.picLatencyWarning.Name = "picLatencyWarning";
			this.picLatencyWarning.Size = new System.Drawing.Size(16, 16);
			this.picLatencyWarning.TabIndex = 3;
			this.picLatencyWarning.TabStop = false;
			// 
			// lblLatencyMs
			// 
			this.lblLatencyMs.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLatencyMs.AutoSize = true;
			this.lblLatencyMs.Location = new System.Drawing.Point(54, 7);
			this.lblLatencyMs.Name = "lblLatencyMs";
			this.lblLatencyMs.Size = new System.Drawing.Size(20, 13);
			this.lblLatencyMs.TabIndex = 2;
			this.lblLatencyMs.Text = "ms";
			// 
			// nudLatency
			// 
			this.nudLatency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudLatency.DecimalPlaces = 0;
			this.nudLatency.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudLatency.Location = new System.Drawing.Point(3, 3);
			this.nudLatency.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.nudLatency.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudLatency.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.nudLatency.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudLatency.Name = "nudLatency";
			this.nudLatency.Size = new System.Drawing.Size(45, 21);
			this.nudLatency.TabIndex = 1;
			this.nudLatency.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudLatency.ValueChanged += new System.EventHandler(this.nudLatency_ValueChanged);
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel8, 2);
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.chkReduceSoundInBackground, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this.chkReduceSoundInFastForward, 0, 2);
			this.tableLayoutPanel8.Controls.Add(this.trkVolumeReduction, 1, 1);
			this.tableLayoutPanel8.Controls.Add(this.chkMuteSoundInBackground, 0, 0);
			this.tableLayoutPanel8.Location = new System.Drawing.Point(10, 132);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(10, 3, 0, 0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 4;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(453, 81);
			this.tableLayoutPanel8.TabIndex = 25;
			// 
			// chkReduceSoundInBackground
			// 
			this.chkReduceSoundInBackground.AutoSize = true;
			this.chkReduceSoundInBackground.Location = new System.Drawing.Point(3, 26);
			this.chkReduceSoundInBackground.Name = "chkReduceSoundInBackground";
			this.chkReduceSoundInBackground.Size = new System.Drawing.Size(164, 17);
			this.chkReduceSoundInBackground.TabIndex = 13;
			this.chkReduceSoundInBackground.Text = "Reduce when in background";
			this.chkReduceSoundInBackground.UseVisualStyleBackColor = true;
			this.chkReduceSoundInBackground.CheckedChanged += new System.EventHandler(this.chkReduceVolume_CheckedChanged);
			// 
			// chkReduceSoundInFastForward
			// 
			this.chkReduceSoundInFastForward.AutoSize = true;
			this.chkReduceSoundInFastForward.Location = new System.Drawing.Point(3, 49);
			this.chkReduceSoundInFastForward.Name = "chkReduceSoundInFastForward";
			this.chkReduceSoundInFastForward.Size = new System.Drawing.Size(225, 17);
			this.chkReduceSoundInFastForward.TabIndex = 16;
			this.chkReduceSoundInFastForward.Text = "Reduce when fast forwarding or rewinding";
			this.chkReduceSoundInFastForward.UseVisualStyleBackColor = true;
			this.chkReduceSoundInFastForward.CheckedChanged += new System.EventHandler(this.chkReduceVolume_CheckedChanged);
			// 
			// trkVolumeReduction
			// 
			this.trkVolumeReduction.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkVolumeReduction.Enabled = false;
			this.trkVolumeReduction.Location = new System.Drawing.Point(231, 23);
			this.trkVolumeReduction.Margin = new System.Windows.Forms.Padding(0);
			this.trkVolumeReduction.Maximum = 100;
			this.trkVolumeReduction.MaximumSize = new System.Drawing.Size(400, 55);
			this.trkVolumeReduction.Minimum = 0;
			this.trkVolumeReduction.MinimumSize = new System.Drawing.Size(150, 55);
			this.trkVolumeReduction.Name = "trkVolumeReduction";
			this.tableLayoutPanel8.SetRowSpan(this.trkVolumeReduction, 2);
			this.trkVolumeReduction.Size = new System.Drawing.Size(222, 55);
			this.trkVolumeReduction.TabIndex = 17;
			this.trkVolumeReduction.Text = "Volume Reduction";
			this.trkVolumeReduction.Value = 50;
			// 
			// chkMuteSoundInBackground
			// 
			this.chkMuteSoundInBackground.AutoSize = true;
			this.chkMuteSoundInBackground.Location = new System.Drawing.Point(3, 3);
			this.chkMuteSoundInBackground.Name = "chkMuteSoundInBackground";
			this.chkMuteSoundInBackground.Size = new System.Drawing.Size(182, 17);
			this.chkMuteSoundInBackground.TabIndex = 18;
			this.chkMuteSoundInBackground.Text = "Mute sound when in background";
			this.chkMuteSoundInBackground.UseVisualStyleBackColor = true;
			this.chkMuteSoundInBackground.CheckedChanged += new System.EventHandler(this.chkMuteWhenInBackground_CheckedChanged);
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
			this.tabMain.Controls.Add(this.tpgPanning);
			this.tabMain.Controls.Add(this.tpgEqualizer);
			this.tabMain.Controls.Add(this.tpgEffects);
			this.tabMain.Controls.Add(this.tpgAdvanced);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(477, 373);
			this.tabMain.TabIndex = 4;
			// 
			// tpgGeneral
			// 
			this.tpgGeneral.Controls.Add(this.tableLayoutPanel2);
			this.tpgGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneral.Name = "tpgGeneral";
			this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpgGeneral.Size = new System.Drawing.Size(469, 347);
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
			this.tpgVolume.Size = new System.Drawing.Size(469, 347);
			this.tpgVolume.TabIndex = 1;
			this.tpgVolume.Text = "Volume";
			this.tpgVolume.UseVisualStyleBackColor = true;
			// 
			// tpgPanning
			// 
			this.tpgPanning.Controls.Add(this.tableLayoutPanel6);
			this.tpgPanning.Location = new System.Drawing.Point(4, 22);
			this.tpgPanning.Name = "tpgPanning";
			this.tpgPanning.Padding = new System.Windows.Forms.Padding(3);
			this.tpgPanning.Size = new System.Drawing.Size(469, 347);
			this.tpgPanning.TabIndex = 4;
			this.tpgPanning.Text = "Panning";
			this.tpgPanning.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Controls.Add(this.trkSquare1Pan, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.trkFdsPan, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.trkSquare2Pan, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.trkMmc5Pan, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.trkTrianglePan, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.trkNoisePan, 0, 3);
			this.tableLayoutPanel6.Controls.Add(this.trkDmcPan, 0, 4);
			this.tableLayoutPanel6.Controls.Add(this.trkVrc6Pan, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.trkVrc7Pan, 1, 3);
			this.tableLayoutPanel6.Controls.Add(this.trkNamcoPan, 1, 4);
			this.tableLayoutPanel6.Controls.Add(this.trkSunsoftPan, 1, 5);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 7;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(463, 341);
			this.tableLayoutPanel6.TabIndex = 3;
			// 
			// trkSquare1Pan
			// 
			this.trkSquare1Pan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare1Pan.Location = new System.Drawing.Point(12, 0);
			this.trkSquare1Pan.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare1Pan.Maximum = 100;
			this.trkSquare1Pan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare1Pan.Minimum = -100;
			this.trkSquare1Pan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkSquare1Pan.Name = "trkSquare1Pan";
			this.trkSquare1Pan.Size = new System.Drawing.Size(206, 55);
			this.trkSquare1Pan.TabIndex = 12;
			this.trkSquare1Pan.Text = "Square 1";
			this.trkSquare1Pan.Value = 0;
			// 
			// trkFdsPan
			// 
			this.trkFdsPan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkFdsPan.Location = new System.Drawing.Point(244, 0);
			this.trkFdsPan.Margin = new System.Windows.Forms.Padding(0);
			this.trkFdsPan.Maximum = 100;
			this.trkFdsPan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkFdsPan.Minimum = -100;
			this.trkFdsPan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkFdsPan.Name = "trkFdsPan";
			this.trkFdsPan.Size = new System.Drawing.Size(206, 55);
			this.trkFdsPan.TabIndex = 17;
			this.trkFdsPan.Text = "FDS";
			this.trkFdsPan.Value = 0;
			// 
			// trkSquare2Pan
			// 
			this.trkSquare2Pan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSquare2Pan.Location = new System.Drawing.Point(12, 55);
			this.trkSquare2Pan.Margin = new System.Windows.Forms.Padding(0);
			this.trkSquare2Pan.Maximum = 100;
			this.trkSquare2Pan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSquare2Pan.Minimum = -100;
			this.trkSquare2Pan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkSquare2Pan.Name = "trkSquare2Pan";
			this.trkSquare2Pan.Size = new System.Drawing.Size(206, 55);
			this.trkSquare2Pan.TabIndex = 13;
			this.trkSquare2Pan.Text = "Square 2";
			this.trkSquare2Pan.Value = 0;
			// 
			// trkMmc5Pan
			// 
			this.trkMmc5Pan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkMmc5Pan.Location = new System.Drawing.Point(244, 55);
			this.trkMmc5Pan.Margin = new System.Windows.Forms.Padding(0);
			this.trkMmc5Pan.Maximum = 100;
			this.trkMmc5Pan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMmc5Pan.Minimum = -100;
			this.trkMmc5Pan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkMmc5Pan.Name = "trkMmc5Pan";
			this.trkMmc5Pan.Size = new System.Drawing.Size(206, 55);
			this.trkMmc5Pan.TabIndex = 18;
			this.trkMmc5Pan.Text = "MMC5";
			this.trkMmc5Pan.Value = 0;
			// 
			// trkTrianglePan
			// 
			this.trkTrianglePan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkTrianglePan.Location = new System.Drawing.Point(12, 110);
			this.trkTrianglePan.Margin = new System.Windows.Forms.Padding(0);
			this.trkTrianglePan.Maximum = 100;
			this.trkTrianglePan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkTrianglePan.Minimum = -100;
			this.trkTrianglePan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkTrianglePan.Name = "trkTrianglePan";
			this.trkTrianglePan.Size = new System.Drawing.Size(206, 55);
			this.trkTrianglePan.TabIndex = 14;
			this.trkTrianglePan.Text = "Triangle";
			this.trkTrianglePan.Value = 0;
			// 
			// trkNoisePan
			// 
			this.trkNoisePan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNoisePan.Location = new System.Drawing.Point(12, 165);
			this.trkNoisePan.Margin = new System.Windows.Forms.Padding(0);
			this.trkNoisePan.Maximum = 100;
			this.trkNoisePan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNoisePan.Minimum = -100;
			this.trkNoisePan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkNoisePan.Name = "trkNoisePan";
			this.trkNoisePan.Size = new System.Drawing.Size(206, 55);
			this.trkNoisePan.TabIndex = 15;
			this.trkNoisePan.Text = "Noise";
			this.trkNoisePan.Value = 0;
			// 
			// trkDmcPan
			// 
			this.trkDmcPan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkDmcPan.Location = new System.Drawing.Point(12, 220);
			this.trkDmcPan.Margin = new System.Windows.Forms.Padding(0);
			this.trkDmcPan.Maximum = 100;
			this.trkDmcPan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkDmcPan.Minimum = -100;
			this.trkDmcPan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkDmcPan.Name = "trkDmcPan";
			this.trkDmcPan.Size = new System.Drawing.Size(206, 55);
			this.trkDmcPan.TabIndex = 16;
			this.trkDmcPan.Text = "DMC";
			this.trkDmcPan.Value = 0;
			// 
			// trkVrc6Pan
			// 
			this.trkVrc6Pan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc6Pan.Location = new System.Drawing.Point(244, 110);
			this.trkVrc6Pan.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc6Pan.Maximum = 100;
			this.trkVrc6Pan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc6Pan.Minimum = -100;
			this.trkVrc6Pan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkVrc6Pan.Name = "trkVrc6Pan";
			this.trkVrc6Pan.Size = new System.Drawing.Size(206, 55);
			this.trkVrc6Pan.TabIndex = 19;
			this.trkVrc6Pan.Text = "VRC6";
			this.trkVrc6Pan.Value = 0;
			// 
			// trkVrc7Pan
			// 
			this.trkVrc7Pan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkVrc7Pan.Location = new System.Drawing.Point(244, 165);
			this.trkVrc7Pan.Margin = new System.Windows.Forms.Padding(0);
			this.trkVrc7Pan.Maximum = 100;
			this.trkVrc7Pan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkVrc7Pan.Minimum = -100;
			this.trkVrc7Pan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkVrc7Pan.Name = "trkVrc7Pan";
			this.trkVrc7Pan.Size = new System.Drawing.Size(206, 55);
			this.trkVrc7Pan.TabIndex = 20;
			this.trkVrc7Pan.Text = "VRC7";
			this.trkVrc7Pan.Value = 0;
			// 
			// trkNamcoPan
			// 
			this.trkNamcoPan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkNamcoPan.Location = new System.Drawing.Point(244, 220);
			this.trkNamcoPan.Margin = new System.Windows.Forms.Padding(0);
			this.trkNamcoPan.Maximum = 100;
			this.trkNamcoPan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkNamcoPan.Minimum = -100;
			this.trkNamcoPan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkNamcoPan.Name = "trkNamcoPan";
			this.trkNamcoPan.Size = new System.Drawing.Size(206, 55);
			this.trkNamcoPan.TabIndex = 21;
			this.trkNamcoPan.Text = "Namco";
			this.trkNamcoPan.Value = 0;
			// 
			// trkSunsoftPan
			// 
			this.trkSunsoftPan.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkSunsoftPan.Location = new System.Drawing.Point(244, 275);
			this.trkSunsoftPan.Margin = new System.Windows.Forms.Padding(0);
			this.trkSunsoftPan.Maximum = 100;
			this.trkSunsoftPan.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkSunsoftPan.Minimum = -100;
			this.trkSunsoftPan.MinimumSize = new System.Drawing.Size(206, 55);
			this.trkSunsoftPan.Name = "trkSunsoftPan";
			this.trkSunsoftPan.Size = new System.Drawing.Size(206, 55);
			this.trkSunsoftPan.TabIndex = 22;
			this.trkSunsoftPan.Text = "Sunsoft";
			this.trkSunsoftPan.Value = 0;
			// 
			// tpgEqualizer
			// 
			this.tpgEqualizer.Controls.Add(this.groupBox1);
			this.tpgEqualizer.Location = new System.Drawing.Point(4, 22);
			this.tpgEqualizer.Name = "tpgEqualizer";
			this.tpgEqualizer.Padding = new System.Windows.Forms.Padding(3);
			this.tpgEqualizer.Size = new System.Drawing.Size(469, 347);
			this.tpgEqualizer.TabIndex = 5;
			this.tpgEqualizer.Text = "Equalizer";
			this.tpgEqualizer.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkEnableEqualizer);
			this.groupBox1.Controls.Add(this.tlpEqualizer);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(463, 341);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// chkEnableEqualizer
			// 
			this.chkEnableEqualizer.AutoSize = true;
			this.chkEnableEqualizer.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.chkEnableEqualizer.Location = new System.Drawing.Point(7, 0);
			this.chkEnableEqualizer.Name = "chkEnableEqualizer";
			this.chkEnableEqualizer.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.chkEnableEqualizer.Size = new System.Drawing.Size(110, 17);
			this.chkEnableEqualizer.TabIndex = 5;
			this.chkEnableEqualizer.Text = "Enable Equalizer";
			this.chkEnableEqualizer.UseVisualStyleBackColor = false;
			this.chkEnableEqualizer.CheckedChanged += new System.EventHandler(this.chkEnableEqualizer_CheckedChanged);
			// 
			// tlpEqualizer
			// 
			this.tlpEqualizer.ColumnCount = 10;
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tlpEqualizer.Controls.Add(this.trkBand6Gain, 5, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand5Gain, 4, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand4Gain, 3, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand3Gain, 2, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand2Gain, 1, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand1Gain, 0, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand11Gain, 0, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand12Gain, 1, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand13Gain, 2, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand14Gain, 3, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand15Gain, 4, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand16Gain, 5, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand7Gain, 6, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand8Gain, 7, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand9Gain, 8, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand10Gain, 9, 1);
			this.tlpEqualizer.Controls.Add(this.trkBand17Gain, 6, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand18Gain, 7, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand19Gain, 8, 2);
			this.tlpEqualizer.Controls.Add(this.trkBand20Gain, 9, 2);
			this.tlpEqualizer.Controls.Add(this.flowLayoutPanel6, 0, 0);
			this.tlpEqualizer.Controls.Add(this.flowLayoutPanel7, 6, 0);
			this.tlpEqualizer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpEqualizer.Location = new System.Drawing.Point(3, 16);
			this.tlpEqualizer.Name = "tlpEqualizer";
			this.tlpEqualizer.RowCount = 4;
			this.tlpEqualizer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpEqualizer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpEqualizer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpEqualizer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpEqualizer.Size = new System.Drawing.Size(457, 322);
			this.tlpEqualizer.TabIndex = 3;
			// 
			// trkBand6Gain
			// 
			this.trkBand6Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand6Gain.Location = new System.Drawing.Point(225, 23);
			this.trkBand6Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand6Gain.Maximum = 200;
			this.trkBand6Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand6Gain.Minimum = -200;
			this.trkBand6Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand6Gain.Name = "trkBand6Gain";
			this.trkBand6Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand6Gain.TabIndex = 16;
			this.trkBand6Gain.Text = "225 Hz";
			this.trkBand6Gain.Value = 50;
			this.trkBand6Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand5Gain
			// 
			this.trkBand5Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand5Gain.Location = new System.Drawing.Point(180, 23);
			this.trkBand5Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand5Gain.Maximum = 200;
			this.trkBand5Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand5Gain.Minimum = -200;
			this.trkBand5Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand5Gain.Name = "trkBand5Gain";
			this.trkBand5Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand5Gain.TabIndex = 15;
			this.trkBand5Gain.Text = "160 Hz";
			this.trkBand5Gain.Value = 50;
			this.trkBand5Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand4Gain
			// 
			this.trkBand4Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand4Gain.Location = new System.Drawing.Point(135, 23);
			this.trkBand4Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand4Gain.Maximum = 200;
			this.trkBand4Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand4Gain.Minimum = -200;
			this.trkBand4Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand4Gain.Name = "trkBand4Gain";
			this.trkBand4Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand4Gain.TabIndex = 14;
			this.trkBand4Gain.Text = "113 Hz";
			this.trkBand4Gain.Value = 50;
			this.trkBand4Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand3Gain
			// 
			this.trkBand3Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand3Gain.Location = new System.Drawing.Point(90, 23);
			this.trkBand3Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand3Gain.Maximum = 200;
			this.trkBand3Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand3Gain.Minimum = -200;
			this.trkBand3Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand3Gain.Name = "trkBand3Gain";
			this.trkBand3Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand3Gain.TabIndex = 13;
			this.trkBand3Gain.Text = "80 Hz";
			this.trkBand3Gain.Value = 50;
			this.trkBand3Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand2Gain
			// 
			this.trkBand2Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand2Gain.Location = new System.Drawing.Point(45, 23);
			this.trkBand2Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand2Gain.Maximum = 200;
			this.trkBand2Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand2Gain.Minimum = -200;
			this.trkBand2Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand2Gain.Name = "trkBand2Gain";
			this.trkBand2Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand2Gain.TabIndex = 12;
			this.trkBand2Gain.Text = "56 Hz";
			this.trkBand2Gain.Value = 50;
			this.trkBand2Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand1Gain
			// 
			this.trkBand1Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand1Gain.Location = new System.Drawing.Point(0, 23);
			this.trkBand1Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand1Gain.Maximum = 200;
			this.trkBand1Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand1Gain.Minimum = -200;
			this.trkBand1Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand1Gain.Name = "trkBand1Gain";
			this.trkBand1Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand1Gain.TabIndex = 11;
			this.trkBand1Gain.Text = "40 Hz";
			this.trkBand1Gain.Value = 50;
			this.trkBand1Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand11Gain
			// 
			this.trkBand11Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand11Gain.Location = new System.Drawing.Point(0, 183);
			this.trkBand11Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand11Gain.Maximum = 200;
			this.trkBand11Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand11Gain.Minimum = -200;
			this.trkBand11Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand11Gain.Name = "trkBand11Gain";
			this.trkBand11Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand11Gain.TabIndex = 17;
			this.trkBand11Gain.Text = "1.0 kHz";
			this.trkBand11Gain.Value = 50;
			this.trkBand11Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand12Gain
			// 
			this.trkBand12Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand12Gain.Location = new System.Drawing.Point(45, 183);
			this.trkBand12Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand12Gain.Maximum = 200;
			this.trkBand12Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand12Gain.Minimum = -200;
			this.trkBand12Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand12Gain.Name = "trkBand12Gain";
			this.trkBand12Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand12Gain.TabIndex = 18;
			this.trkBand12Gain.Text = "2.0 kHz";
			this.trkBand12Gain.Value = 50;
			this.trkBand12Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand13Gain
			// 
			this.trkBand13Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand13Gain.Location = new System.Drawing.Point(90, 183);
			this.trkBand13Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand13Gain.Maximum = 200;
			this.trkBand13Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand13Gain.Minimum = -200;
			this.trkBand13Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand13Gain.Name = "trkBand13Gain";
			this.trkBand13Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand13Gain.TabIndex = 19;
			this.trkBand13Gain.Text = "3.0 kHz";
			this.trkBand13Gain.Value = 50;
			this.trkBand13Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand14Gain
			// 
			this.trkBand14Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand14Gain.Location = new System.Drawing.Point(135, 183);
			this.trkBand14Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand14Gain.Maximum = 200;
			this.trkBand14Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand14Gain.Minimum = -200;
			this.trkBand14Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand14Gain.Name = "trkBand14Gain";
			this.trkBand14Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand14Gain.TabIndex = 20;
			this.trkBand14Gain.Text = "4.0 kHz";
			this.trkBand14Gain.Value = 50;
			this.trkBand14Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand15Gain
			// 
			this.trkBand15Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand15Gain.Location = new System.Drawing.Point(180, 183);
			this.trkBand15Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand15Gain.Maximum = 200;
			this.trkBand15Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand15Gain.Minimum = -200;
			this.trkBand15Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand15Gain.Name = "trkBand15Gain";
			this.trkBand15Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand15Gain.TabIndex = 21;
			this.trkBand15Gain.Text = "5.0 kHz";
			this.trkBand15Gain.Value = 50;
			this.trkBand15Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand16Gain
			// 
			this.trkBand16Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand16Gain.Location = new System.Drawing.Point(225, 183);
			this.trkBand16Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand16Gain.Maximum = 200;
			this.trkBand16Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand16Gain.Minimum = -200;
			this.trkBand16Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand16Gain.Name = "trkBand16Gain";
			this.trkBand16Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand16Gain.TabIndex = 22;
			this.trkBand16Gain.Text = "6.0 kHz";
			this.trkBand16Gain.Value = 50;
			this.trkBand16Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand7Gain
			// 
			this.trkBand7Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand7Gain.Location = new System.Drawing.Point(270, 23);
			this.trkBand7Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand7Gain.Maximum = 200;
			this.trkBand7Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand7Gain.Minimum = -200;
			this.trkBand7Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand7Gain.Name = "trkBand7Gain";
			this.trkBand7Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand7Gain.TabIndex = 23;
			this.trkBand7Gain.Text = "320 Hz";
			this.trkBand7Gain.Value = 50;
			this.trkBand7Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand8Gain
			// 
			this.trkBand8Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand8Gain.Location = new System.Drawing.Point(315, 23);
			this.trkBand8Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand8Gain.Maximum = 200;
			this.trkBand8Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand8Gain.Minimum = -200;
			this.trkBand8Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand8Gain.Name = "trkBand8Gain";
			this.trkBand8Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand8Gain.TabIndex = 24;
			this.trkBand8Gain.Text = "450 Hz";
			this.trkBand8Gain.Value = 50;
			this.trkBand8Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand9Gain
			// 
			this.trkBand9Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand9Gain.Location = new System.Drawing.Point(360, 23);
			this.trkBand9Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand9Gain.Maximum = 200;
			this.trkBand9Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand9Gain.Minimum = -200;
			this.trkBand9Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand9Gain.Name = "trkBand9Gain";
			this.trkBand9Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand9Gain.TabIndex = 25;
			this.trkBand9Gain.Text = "600 Hz";
			this.trkBand9Gain.Value = 50;
			this.trkBand9Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand10Gain
			// 
			this.trkBand10Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand10Gain.Location = new System.Drawing.Point(405, 23);
			this.trkBand10Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand10Gain.Maximum = 200;
			this.trkBand10Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand10Gain.Minimum = -200;
			this.trkBand10Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand10Gain.Name = "trkBand10Gain";
			this.trkBand10Gain.Size = new System.Drawing.Size(52, 160);
			this.trkBand10Gain.TabIndex = 26;
			this.trkBand10Gain.Text = "750 Hz";
			this.trkBand10Gain.Value = 50;
			this.trkBand10Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand17Gain
			// 
			this.trkBand17Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand17Gain.Location = new System.Drawing.Point(270, 183);
			this.trkBand17Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand17Gain.Maximum = 200;
			this.trkBand17Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand17Gain.Minimum = -200;
			this.trkBand17Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand17Gain.Name = "trkBand17Gain";
			this.trkBand17Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand17Gain.TabIndex = 27;
			this.trkBand17Gain.Text = "7.0 kHz";
			this.trkBand17Gain.Value = 50;
			this.trkBand17Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand18Gain
			// 
			this.trkBand18Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand18Gain.Location = new System.Drawing.Point(315, 183);
			this.trkBand18Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand18Gain.Maximum = 200;
			this.trkBand18Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand18Gain.Minimum = -200;
			this.trkBand18Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand18Gain.Name = "trkBand18Gain";
			this.trkBand18Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand18Gain.TabIndex = 28;
			this.trkBand18Gain.Text = "10.0 kHz";
			this.trkBand18Gain.Value = 50;
			this.trkBand18Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand19Gain
			// 
			this.trkBand19Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand19Gain.Location = new System.Drawing.Point(360, 183);
			this.trkBand19Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand19Gain.Maximum = 200;
			this.trkBand19Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand19Gain.Minimum = -200;
			this.trkBand19Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand19Gain.Name = "trkBand19Gain";
			this.trkBand19Gain.Size = new System.Drawing.Size(45, 160);
			this.trkBand19Gain.TabIndex = 29;
			this.trkBand19Gain.Text = "12.5 kHz";
			this.trkBand19Gain.Value = 50;
			this.trkBand19Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// trkBand20Gain
			// 
			this.trkBand20Gain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkBand20Gain.Location = new System.Drawing.Point(405, 183);
			this.trkBand20Gain.Margin = new System.Windows.Forms.Padding(0);
			this.trkBand20Gain.Maximum = 200;
			this.trkBand20Gain.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkBand20Gain.Minimum = -200;
			this.trkBand20Gain.MinimumSize = new System.Drawing.Size(34, 160);
			this.trkBand20Gain.Name = "trkBand20Gain";
			this.trkBand20Gain.Size = new System.Drawing.Size(52, 160);
			this.trkBand20Gain.TabIndex = 30;
			this.trkBand20Gain.Text = "15 kHz";
			this.trkBand20Gain.Value = 50;
			this.trkBand20Gain.ValueChanged += new System.EventHandler(this.trkBandGain_ValueChanged);
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.tlpEqualizer.SetColumnSpan(this.flowLayoutPanel6, 5);
			this.flowLayoutPanel6.Controls.Add(this.lblEqualizerPreset);
			this.flowLayoutPanel6.Controls.Add(this.cboEqualizerPreset);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 2);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(167, 21);
			this.flowLayoutPanel6.TabIndex = 33;
			this.flowLayoutPanel6.Visible = false;
			// 
			// lblEqualizerPreset
			// 
			this.lblEqualizerPreset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEqualizerPreset.AutoSize = true;
			this.lblEqualizerPreset.Location = new System.Drawing.Point(3, 4);
			this.lblEqualizerPreset.Name = "lblEqualizerPreset";
			this.lblEqualizerPreset.Size = new System.Drawing.Size(40, 13);
			this.lblEqualizerPreset.TabIndex = 32;
			this.lblEqualizerPreset.Text = "Preset:";
			// 
			// cboEqualizerPreset
			// 
			this.cboEqualizerPreset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboEqualizerPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEqualizerPreset.FormattingEnabled = true;
			this.cboEqualizerPreset.Location = new System.Drawing.Point(46, 0);
			this.cboEqualizerPreset.Margin = new System.Windows.Forms.Padding(0);
			this.cboEqualizerPreset.Name = "cboEqualizerPreset";
			this.cboEqualizerPreset.Size = new System.Drawing.Size(121, 21);
			this.cboEqualizerPreset.TabIndex = 33;
			this.cboEqualizerPreset.SelectedIndexChanged += new System.EventHandler(this.cboEqualizerPreset_SelectedIndexChanged);
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel7.AutoSize = true;
			this.tlpEqualizer.SetColumnSpan(this.flowLayoutPanel7, 4);
			this.flowLayoutPanel7.Controls.Add(this.lblEqualizerFilterType);
			this.flowLayoutPanel7.Controls.Add(this.cboEqualizerFilterType);
			this.flowLayoutPanel7.Location = new System.Drawing.Point(298, 2);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(159, 21);
			this.flowLayoutPanel7.TabIndex = 34;
			this.flowLayoutPanel7.Visible = false;
			// 
			// lblEqualizerFilterType
			// 
			this.lblEqualizerFilterType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEqualizerFilterType.AutoSize = true;
			this.lblEqualizerFilterType.Location = new System.Drawing.Point(3, 4);
			this.lblEqualizerFilterType.Name = "lblEqualizerFilterType";
			this.lblEqualizerFilterType.Size = new System.Drawing.Size(32, 13);
			this.lblEqualizerFilterType.TabIndex = 31;
			this.lblEqualizerFilterType.Text = "Filter:";
			// 
			// cboEqualizerFilterType
			// 
			this.cboEqualizerFilterType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboEqualizerFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEqualizerFilterType.FormattingEnabled = true;
			this.cboEqualizerFilterType.Location = new System.Drawing.Point(38, 0);
			this.cboEqualizerFilterType.Margin = new System.Windows.Forms.Padding(0);
			this.cboEqualizerFilterType.Name = "cboEqualizerFilterType";
			this.cboEqualizerFilterType.Size = new System.Drawing.Size(121, 21);
			this.cboEqualizerFilterType.TabIndex = 32;
			// 
			// tpgEffects
			// 
			this.tpgEffects.Controls.Add(this.tableLayoutPanel4);
			this.tpgEffects.Location = new System.Drawing.Point(4, 22);
			this.tpgEffects.Name = "tpgEffects";
			this.tpgEffects.Padding = new System.Windows.Forms.Padding(3);
			this.tpgEffects.Size = new System.Drawing.Size(469, 347);
			this.tpgEffects.TabIndex = 3;
			this.tpgEffects.Text = "Effects";
			this.tpgEffects.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.grpStereo, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.grpReverb, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel5, 0, 2);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(463, 341);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// grpStereo
			// 
			this.grpStereo.Controls.Add(this.tlpStereoFilter);
			this.grpStereo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpStereo.Location = new System.Drawing.Point(3, 3);
			this.grpStereo.Name = "grpStereo";
			this.grpStereo.Size = new System.Drawing.Size(457, 95);
			this.grpStereo.TabIndex = 0;
			this.grpStereo.TabStop = false;
			this.grpStereo.Text = "Stereo";
			// 
			// tlpStereoFilter
			// 
			this.tlpStereoFilter.ColumnCount = 3;
			this.tlpStereoFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpStereoFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpStereoFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpStereoFilter.Controls.Add(this.lblStereoDelayMs, 2, 1);
			this.tlpStereoFilter.Controls.Add(this.lblStereoPanningAngle, 2, 2);
			this.tlpStereoFilter.Controls.Add(this.radStereoDisabled, 0, 0);
			this.tlpStereoFilter.Controls.Add(this.radStereoDelay, 0, 1);
			this.tlpStereoFilter.Controls.Add(this.radStereoPanning, 0, 2);
			this.tlpStereoFilter.Controls.Add(this.nudStereoDelay, 1, 1);
			this.tlpStereoFilter.Controls.Add(this.nudStereoPanning, 1, 2);
			this.tlpStereoFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpStereoFilter.Location = new System.Drawing.Point(3, 16);
			this.tlpStereoFilter.Name = "tlpStereoFilter";
			this.tlpStereoFilter.RowCount = 4;
			this.tlpStereoFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStereoFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStereoFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStereoFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpStereoFilter.Size = new System.Drawing.Size(451, 76);
			this.tlpStereoFilter.TabIndex = 0;
			// 
			// lblStereoDelayMs
			// 
			this.lblStereoDelayMs.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblStereoDelayMs.AutoSize = true;
			this.lblStereoDelayMs.Location = new System.Drawing.Point(120, 28);
			this.lblStereoDelayMs.Name = "lblStereoDelayMs";
			this.lblStereoDelayMs.Size = new System.Drawing.Size(20, 13);
			this.lblStereoDelayMs.TabIndex = 1;
			this.lblStereoDelayMs.Text = "ms";
			// 
			// lblStereoPanningAngle
			// 
			this.lblStereoPanningAngle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblStereoPanningAngle.AutoSize = true;
			this.lblStereoPanningAngle.Location = new System.Drawing.Point(120, 51);
			this.lblStereoPanningAngle.Name = "lblStereoPanningAngle";
			this.lblStereoPanningAngle.Size = new System.Drawing.Size(92, 13);
			this.lblStereoPanningAngle.TabIndex = 1;
			this.lblStereoPanningAngle.Text = "(Angle in degrees)";
			// 
			// radStereoDisabled
			// 
			this.radStereoDisabled.AutoSize = true;
			this.radStereoDisabled.Checked = true;
			this.radStereoDisabled.Location = new System.Drawing.Point(3, 3);
			this.radStereoDisabled.Name = "radStereoDisabled";
			this.radStereoDisabled.Size = new System.Drawing.Size(66, 17);
			this.radStereoDisabled.TabIndex = 1;
			this.radStereoDisabled.TabStop = true;
			this.radStereoDisabled.Tag = "None";
			this.radStereoDisabled.Text = "Disabled";
			this.radStereoDisabled.UseVisualStyleBackColor = true;
			// 
			// radStereoDelay
			// 
			this.radStereoDelay.AutoSize = true;
			this.radStereoDelay.Location = new System.Drawing.Point(3, 26);
			this.radStereoDelay.Name = "radStereoDelay";
			this.radStereoDelay.Size = new System.Drawing.Size(52, 17);
			this.radStereoDelay.TabIndex = 2;
			this.radStereoDelay.TabStop = true;
			this.radStereoDelay.Tag = "Delay";
			this.radStereoDelay.Text = "Delay";
			this.radStereoDelay.UseVisualStyleBackColor = true;
			// 
			// radStereoPanning
			// 
			this.radStereoPanning.AutoSize = true;
			this.radStereoPanning.Location = new System.Drawing.Point(3, 49);
			this.radStereoPanning.Name = "radStereoPanning";
			this.radStereoPanning.Size = new System.Drawing.Size(64, 17);
			this.radStereoPanning.TabIndex = 3;
			this.radStereoPanning.TabStop = true;
			this.radStereoPanning.Tag = "Panning";
			this.radStereoPanning.Text = "Panning";
			this.radStereoPanning.UseVisualStyleBackColor = true;
			// 
			// nudStereoDelay
			// 
			this.nudStereoDelay.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudStereoDelay.DecimalPlaces = 0;
			this.nudStereoDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudStereoDelay.Location = new System.Drawing.Point(72, 24);
			this.nudStereoDelay.Margin = new System.Windows.Forms.Padding(0);
			this.nudStereoDelay.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudStereoDelay.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudStereoDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudStereoDelay.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudStereoDelay.Name = "nudStereoDelay";
			this.nudStereoDelay.Size = new System.Drawing.Size(45, 21);
			this.nudStereoDelay.TabIndex = 1;
			this.nudStereoDelay.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// nudStereoPanning
			// 
			this.nudStereoPanning.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudStereoPanning.DecimalPlaces = 0;
			this.nudStereoPanning.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudStereoPanning.Location = new System.Drawing.Point(72, 47);
			this.nudStereoPanning.Margin = new System.Windows.Forms.Padding(0);
			this.nudStereoPanning.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.nudStereoPanning.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudStereoPanning.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.nudStereoPanning.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudStereoPanning.Name = "nudStereoPanning";
			this.nudStereoPanning.Size = new System.Drawing.Size(45, 21);
			this.nudStereoPanning.TabIndex = 1;
			this.nudStereoPanning.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// grpReverb
			// 
			this.grpReverb.Controls.Add(this.tableLayoutPanel5);
			this.grpReverb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpReverb.Location = new System.Drawing.Point(3, 104);
			this.grpReverb.Name = "grpReverb";
			this.grpReverb.Size = new System.Drawing.Size(457, 106);
			this.grpReverb.TabIndex = 1;
			this.grpReverb.TabStop = false;
			this.grpReverb.Text = "Reverb";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add(this.chkReverbEnabled, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblReverbStrength, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.lblReverbDelay, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.trkReverbDelay, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.trkReverbStrength, 1, 1);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 4;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(451, 87);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// chkReverbEnabled
			// 
			this.chkReverbEnabled.AutoSize = true;
			this.tableLayoutPanel5.SetColumnSpan(this.chkReverbEnabled, 2);
			this.chkReverbEnabled.Location = new System.Drawing.Point(3, 3);
			this.chkReverbEnabled.Name = "chkReverbEnabled";
			this.chkReverbEnabled.Size = new System.Drawing.Size(97, 17);
			this.chkReverbEnabled.TabIndex = 0;
			this.chkReverbEnabled.Text = "Enable Reverb";
			this.chkReverbEnabled.UseVisualStyleBackColor = true;
			// 
			// lblReverbStrength
			// 
			this.lblReverbStrength.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblReverbStrength.AutoSize = true;
			this.lblReverbStrength.Location = new System.Drawing.Point(3, 32);
			this.lblReverbStrength.Name = "lblReverbStrength";
			this.lblReverbStrength.Size = new System.Drawing.Size(50, 13);
			this.lblReverbStrength.TabIndex = 2;
			this.lblReverbStrength.Text = "Strength:";
			// 
			// lblReverbDelay
			// 
			this.lblReverbDelay.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblReverbDelay.AutoSize = true;
			this.lblReverbDelay.Location = new System.Drawing.Point(3, 64);
			this.lblReverbDelay.Name = "lblReverbDelay";
			this.lblReverbDelay.Size = new System.Drawing.Size(37, 13);
			this.lblReverbDelay.TabIndex = 3;
			this.lblReverbDelay.Text = "Delay:";
			// 
			// trkReverbDelay
			// 
			this.trkReverbDelay.Location = new System.Drawing.Point(59, 58);
			this.trkReverbDelay.Maximum = 30;
			this.trkReverbDelay.Minimum = 1;
			this.trkReverbDelay.Name = "trkReverbDelay";
			this.trkReverbDelay.Size = new System.Drawing.Size(104, 26);
			this.trkReverbDelay.TabIndex = 4;
			this.trkReverbDelay.TickFrequency = 3;
			this.trkReverbDelay.Value = 1;
			// 
			// trkReverbStrength
			// 
			this.trkReverbStrength.Location = new System.Drawing.Point(59, 26);
			this.trkReverbStrength.Minimum = 1;
			this.trkReverbStrength.Name = "trkReverbStrength";
			this.trkReverbStrength.Size = new System.Drawing.Size(104, 26);
			this.trkReverbStrength.TabIndex = 1;
			this.trkReverbStrength.Value = 1;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.Controls.Add(this.chkCrossFeedEnabled);
			this.flowLayoutPanel5.Controls.Add(this.nudCrossFeedRatio);
			this.flowLayoutPanel5.Controls.Add(this.lblCrossFeedRatio);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(6, 213);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(457, 25);
			this.flowLayoutPanel5.TabIndex = 6;
			// 
			// chkCrossFeedEnabled
			// 
			this.chkCrossFeedEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkCrossFeedEnabled.AutoSize = true;
			this.chkCrossFeedEnabled.Location = new System.Drawing.Point(3, 5);
			this.chkCrossFeedEnabled.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.chkCrossFeedEnabled.Name = "chkCrossFeedEnabled";
			this.chkCrossFeedEnabled.Size = new System.Drawing.Size(112, 17);
			this.chkCrossFeedEnabled.TabIndex = 1;
			this.chkCrossFeedEnabled.Text = "Enable Crossfeed:";
			this.chkCrossFeedEnabled.UseVisualStyleBackColor = true;
			// 
			// nudCrossFeedRatio
			// 
			this.nudCrossFeedRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudCrossFeedRatio.DecimalPlaces = 0;
			this.nudCrossFeedRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudCrossFeedRatio.Location = new System.Drawing.Point(118, 2);
			this.nudCrossFeedRatio.Margin = new System.Windows.Forms.Padding(0);
			this.nudCrossFeedRatio.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudCrossFeedRatio.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudCrossFeedRatio.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudCrossFeedRatio.MinimumSize = new System.Drawing.Size(0, 21);
			this.nudCrossFeedRatio.Name = "nudCrossFeedRatio";
			this.nudCrossFeedRatio.Size = new System.Drawing.Size(42, 21);
			this.nudCrossFeedRatio.TabIndex = 2;
			this.nudCrossFeedRatio.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblCrossFeedRatio
			// 
			this.lblCrossFeedRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCrossFeedRatio.AutoSize = true;
			this.lblCrossFeedRatio.Location = new System.Drawing.Point(163, 6);
			this.lblCrossFeedRatio.Name = "lblCrossFeedRatio";
			this.lblCrossFeedRatio.Size = new System.Drawing.Size(15, 13);
			this.lblCrossFeedRatio.TabIndex = 3;
			this.lblCrossFeedRatio.Text = "%";
			// 
			// tpgAdvanced
			// 
			this.tpgAdvanced.Controls.Add(this.tableLayoutPanel3);
			this.tpgAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tpgAdvanced.Name = "tpgAdvanced";
			this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAdvanced.Size = new System.Drawing.Size(469, 347);
			this.tpgAdvanced.TabIndex = 2;
			this.tpgAdvanced.Text = "Advanced";
			this.tpgAdvanced.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.chkDisableNoiseModeFlag, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.chkSilenceTriangleHighFreq, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.chkSwapDutyCycles, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.chkReduceDmcPopping, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(463, 341);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// chkDisableNoiseModeFlag
			// 
			this.chkDisableNoiseModeFlag.Checked = false;
			this.chkDisableNoiseModeFlag.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDisableNoiseModeFlag.Location = new System.Drawing.Point(0, 70);
			this.chkDisableNoiseModeFlag.Name = "chkDisableNoiseModeFlag";
			this.chkDisableNoiseModeFlag.Size = new System.Drawing.Size(463, 23);
			this.chkDisableNoiseModeFlag.TabIndex = 3;
			this.chkDisableNoiseModeFlag.Text = "Disable noise channel mode flag";
			// 
			// chkSilenceTriangleHighFreq
			// 
			this.chkSilenceTriangleHighFreq.AutoSize = true;
			this.chkSilenceTriangleHighFreq.Location = new System.Drawing.Point(3, 3);
			this.chkSilenceTriangleHighFreq.Name = "chkSilenceTriangleHighFreq";
			this.chkSilenceTriangleHighFreq.Size = new System.Drawing.Size(337, 17);
			this.chkSilenceTriangleHighFreq.TabIndex = 1;
			this.chkSilenceTriangleHighFreq.Text = "Mute ultrasonic frequencies on triangle channel (reduces popping)";
			// 
			// chkSwapDutyCycles
			// 
			this.chkSwapDutyCycles.Checked = false;
			this.chkSwapDutyCycles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkSwapDutyCycles.Location = new System.Drawing.Point(0, 46);
			this.chkSwapDutyCycles.Name = "chkSwapDutyCycles";
			this.chkSwapDutyCycles.Size = new System.Drawing.Size(463, 24);
			this.chkSwapDutyCycles.TabIndex = 0;
			this.chkSwapDutyCycles.Text = "Swap square channels duty cycles (Mimics old clones)";
			// 
			// chkReduceDmcPopping
			// 
			this.chkReduceDmcPopping.AutoSize = true;
			this.chkReduceDmcPopping.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkReduceDmcPopping.Location = new System.Drawing.Point(3, 26);
			this.chkReduceDmcPopping.Name = "chkReduceDmcPopping";
			this.chkReduceDmcPopping.Size = new System.Drawing.Size(243, 17);
			this.chkReduceDmcPopping.TabIndex = 2;
			this.chkReduceDmcPopping.Text = "Reduce popping sounds on the DMC channel";
			this.chkReduceDmcPopping.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkReduceDmcPopping.UseVisualStyleBackColor = true;
			// 
			// frmAudioConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(477, 402);
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
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLatencyWarning)).EndInit();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgGeneral.ResumeLayout(false);
			this.tpgVolume.ResumeLayout(false);
			this.tpgPanning.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tpgEqualizer.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tlpEqualizer.ResumeLayout(false);
			this.tlpEqualizer.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.tpgEffects.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.grpStereo.ResumeLayout(false);
			this.tlpStereoFilter.ResumeLayout(false);
			this.tlpStereoFilter.PerformLayout();
			this.grpReverb.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkReverbDelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkReverbStrength)).EndInit();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
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
		private MesenNumericUpDown nudLatency;
		private System.Windows.Forms.Label lblLatencyMs;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label lblSampleRate;
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
		private System.Windows.Forms.CheckBox chkReduceSoundInBackground;
		private System.Windows.Forms.TabPage tpgAdvanced;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private ctrlRiskyOption chkSwapDutyCycles;
		private System.Windows.Forms.TabPage tpgEffects;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.GroupBox grpStereo;
		private System.Windows.Forms.TableLayoutPanel tlpStereoFilter;
		private MesenNumericUpDown nudStereoDelay;
		private System.Windows.Forms.Label lblStereoDelayMs;
		private System.Windows.Forms.RadioButton radStereoDisabled;
		private System.Windows.Forms.RadioButton radStereoDelay;
		private System.Windows.Forms.RadioButton radStereoPanning;
		private MesenNumericUpDown nudStereoPanning;
		private System.Windows.Forms.Label lblStereoPanningAngle;
		private System.Windows.Forms.GroupBox grpReverb;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.CheckBox chkReverbEnabled;
		private System.Windows.Forms.Label lblReverbStrength;
		private System.Windows.Forms.Label lblReverbDelay;
		private System.Windows.Forms.TrackBar trkReverbDelay;
		private System.Windows.Forms.TrackBar trkReverbStrength;
		private System.Windows.Forms.CheckBox chkSilenceTriangleHighFreq;
		private System.Windows.Forms.CheckBox chkReduceDmcPopping;
		private System.Windows.Forms.PictureBox picLatencyWarning;
		private System.Windows.Forms.Label lblLatencyWarning;
		private System.Windows.Forms.TabPage tpgPanning;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private Controls.ctrlHorizontalTrackbar trkDmcPan;
		private Controls.ctrlHorizontalTrackbar trkNoisePan;
		private Controls.ctrlHorizontalTrackbar trkSquare2Pan;
		private Controls.ctrlHorizontalTrackbar trkFdsPan;
		private Controls.ctrlHorizontalTrackbar trkMmc5Pan;
		private Controls.ctrlHorizontalTrackbar trkVrc6Pan;
		private Controls.ctrlHorizontalTrackbar trkVrc7Pan;
		private Controls.ctrlHorizontalTrackbar trkNamcoPan;
		private Controls.ctrlHorizontalTrackbar trkSunsoftPan;
		private Controls.ctrlHorizontalTrackbar trkSquare1Pan;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.CheckBox chkCrossFeedEnabled;
		private MesenNumericUpDown nudCrossFeedRatio;
		private System.Windows.Forms.Label lblCrossFeedRatio;
		private Controls.ctrlHorizontalTrackbar trkTrianglePan;
		private ctrlRiskyOption chkDisableNoiseModeFlag;
		private System.Windows.Forms.TabPage tpgEqualizer;
		private System.Windows.Forms.TableLayoutPanel tlpEqualizer;
		private ctrlTrackbar trkBand6Gain;
		private ctrlTrackbar trkBand5Gain;
		private ctrlTrackbar trkBand4Gain;
		private ctrlTrackbar trkBand3Gain;
		private ctrlTrackbar trkBand2Gain;
		private ctrlTrackbar trkBand1Gain;
		private ctrlTrackbar trkBand11Gain;
		private ctrlTrackbar trkBand12Gain;
		private ctrlTrackbar trkBand13Gain;
		private ctrlTrackbar trkBand14Gain;
		private ctrlTrackbar trkBand15Gain;
		private ctrlTrackbar trkBand16Gain;
		private ctrlTrackbar trkBand7Gain;
		private ctrlTrackbar trkBand8Gain;
		private ctrlTrackbar trkBand9Gain;
		private ctrlTrackbar trkBand10Gain;
		private ctrlTrackbar trkBand17Gain;
		private ctrlTrackbar trkBand18Gain;
		private ctrlTrackbar trkBand19Gain;
		private ctrlTrackbar trkBand20Gain;
		private System.Windows.Forms.Label lblEqualizerFilterType;
		private System.Windows.Forms.ComboBox cboEqualizerFilterType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkEnableEqualizer;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblEqualizerPreset;
		private System.Windows.Forms.ComboBox cboEqualizerPreset;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.CheckBox chkReduceSoundInFastForward;
		private System.Windows.Forms.Label lblVolumeReductionSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private ctrlHorizontalTrackbar trkVolumeReduction;
		private System.Windows.Forms.CheckBox chkMuteSoundInBackground;
	}
}