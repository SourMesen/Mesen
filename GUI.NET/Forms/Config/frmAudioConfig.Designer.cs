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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkEnableAudio = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblAudioLatency = new System.Windows.Forms.Label();
			this.nudLatency = new System.Windows.Forms.NumericUpDown();
			this.lblLatencyMs = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.baseConfigPanel.SuspendLayout();
			this.grpVolume.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLatency)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 245);
			this.baseConfigPanel.Size = new System.Drawing.Size(470, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// grpVolume
			// 
			this.grpVolume.Controls.Add(this.tableLayoutPanel1);
			this.grpVolume.Location = new System.Drawing.Point(3, 29);
			this.grpVolume.Name = "grpVolume";
			this.grpVolume.Size = new System.Drawing.Size(462, 185);
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(451, 160);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// trkDmcVol
			// 
			this.trkDmcVol.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.trkDmcVol.Caption = "DMC";
			this.trkDmcVol.Location = new System.Drawing.Point(381, 0);
			this.trkDmcVol.Margin = new System.Windows.Forms.Padding(0);
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
			this.trkNoiseVol.Caption = "Noise";
			this.trkNoiseVol.Location = new System.Drawing.Point(306, 0);
			this.trkNoiseVol.Margin = new System.Windows.Forms.Padding(0);
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
			this.trkTriangleVol.Caption = "Triangle";
			this.trkTriangleVol.Location = new System.Drawing.Point(231, 0);
			this.trkTriangleVol.Margin = new System.Windows.Forms.Padding(0);
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
			this.trkSquare2Vol.Caption = "Square 2";
			this.trkSquare2Vol.Location = new System.Drawing.Point(156, 0);
			this.trkSquare2Vol.Margin = new System.Windows.Forms.Padding(0);
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
			this.trkSquare1Vol.Caption = "Square 1";
			this.trkSquare1Vol.Location = new System.Drawing.Point(81, 0);
			this.trkSquare1Vol.Margin = new System.Windows.Forms.Padding(0);
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
			this.trkMaster.Caption = "Master";
			this.trkMaster.Location = new System.Drawing.Point(6, 0);
			this.trkMaster.Margin = new System.Windows.Forms.Padding(0);
			this.trkMaster.MaximumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.MinimumSize = new System.Drawing.Size(63, 160);
			this.trkMaster.Name = "trkMaster";
			this.trkMaster.Size = new System.Drawing.Size(63, 160);
			this.trkMaster.TabIndex = 11;
			this.trkMaster.Value = 50;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.grpVolume, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkEnableAudio, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 274);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// chkEnableAudio
			// 
			this.chkEnableAudio.AutoSize = true;
			this.chkEnableAudio.Location = new System.Drawing.Point(6, 6);
			this.chkEnableAudio.Margin = new System.Windows.Forms.Padding(6, 6, 6, 3);
			this.chkEnableAudio.Name = "chkEnableAudio";
			this.chkEnableAudio.Size = new System.Drawing.Size(89, 17);
			this.chkEnableAudio.TabIndex = 3;
			this.chkEnableAudio.Text = "Enable Audio";
			this.chkEnableAudio.UseVisualStyleBackColor = true;
			this.chkEnableAudio.CheckedChanged += new System.EventHandler(this.AudioConfig_ValueChanged);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblAudioLatency);
			this.flowLayoutPanel2.Controls.Add(this.nudLatency);
			this.flowLayoutPanel2.Controls.Add(this.lblLatencyMs);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 220);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 24);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// lblAudioLatency
			// 
			this.lblAudioLatency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAudioLatency.AutoSize = true;
			this.lblAudioLatency.Location = new System.Drawing.Point(3, 6);
			this.lblAudioLatency.Name = "lblAudioLatency";
			this.lblAudioLatency.Size = new System.Drawing.Size(48, 13);
			this.lblAudioLatency.TabIndex = 0;
			this.lblAudioLatency.Text = "Latency:";
			// 
			// nudLatency
			// 
			this.nudLatency.Location = new System.Drawing.Point(57, 3);
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
			this.lblLatencyMs.Location = new System.Drawing.Point(108, 6);
			this.lblLatencyMs.Name = "lblLatencyMs";
			this.lblLatencyMs.Size = new System.Drawing.Size(20, 13);
			this.lblLatencyMs.TabIndex = 2;
			this.lblLatencyMs.Text = "ms";
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
			// frmAudioConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 274);
			this.Controls.Add(this.tableLayoutPanel2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAudioConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Audio Options";
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.baseConfigPanel.PerformLayout();
			this.grpVolume.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLatency)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpVolume;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkEnableAudio;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblAudioLatency;
		private System.Windows.Forms.NumericUpDown nudLatency;
		private System.Windows.Forms.Label lblLatencyMs;
		private System.Windows.Forms.Button btnReset;
		private Controls.ctrlTrackbar trkMaster;
		private Controls.ctrlTrackbar trkDmcVol;
		private Controls.ctrlTrackbar trkNoiseVol;
		private Controls.ctrlTrackbar trkTriangleVol;
		private Controls.ctrlTrackbar trkSquare2Vol;
		private Controls.ctrlTrackbar trkSquare1Vol;

	}
}