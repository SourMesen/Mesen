namespace Mesen.GUI.Debugger
{
	partial class frmApuViewer
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grpChannelControl = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkFds = new System.Windows.Forms.CheckBox();
			this.chkSquare1 = new System.Windows.Forms.CheckBox();
			this.chkSquare2 = new System.Windows.Forms.CheckBox();
			this.chkTriangle = new System.Windows.Forms.CheckBox();
			this.chkNoise = new System.Windows.Forms.CheckBox();
			this.chkMmc5 = new System.Windows.Forms.CheckBox();
			this.chkNamco = new System.Windows.Forms.CheckBox();
			this.chkVrc7 = new System.Windows.Forms.CheckBox();
			this.chkDmc = new System.Windows.Forms.CheckBox();
			this.chkSunsoft = new System.Windows.Forms.CheckBox();
			this.chkVrc6 = new System.Windows.Forms.CheckBox();
			this.grpSquare1 = new System.Windows.Forms.GroupBox();
			this.ctrlSquareInfo1 = new Mesen.GUI.Debugger.Controls.ctrlSquareInfo();
			this.grpSquare2 = new System.Windows.Forms.GroupBox();
			this.ctrlSquareInfo2 = new Mesen.GUI.Debugger.Controls.ctrlSquareInfo();
			this.grpTriangle = new System.Windows.Forms.GroupBox();
			this.ctrlTriangleInfo = new Mesen.GUI.Debugger.Controls.ctrlTriangleInfo();
			this.grpNoise = new System.Windows.Forms.GroupBox();
			this.ctrlNoiseInfo = new Mesen.GUI.Debugger.Controls.ctrlNoiseInfo();
			this.grpDmc = new System.Windows.Forms.GroupBox();
			this.ctrlDmcInfo = new Mesen.GUI.Debugger.Controls.ctrlDmcInfo();
			this.grpFrameCounter = new System.Windows.Forms.GroupBox();
			this.ctrlFrameCounterInfo = new Mesen.GUI.Debugger.Controls.ctrlFrameCounterInfo();
			this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.grpChannelControl.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpSquare1.SuspendLayout();
			this.grpSquare2.SuspendLayout();
			this.grpTriangle.SuspendLayout();
			this.grpNoise.SuspendLayout();
			this.grpDmc.SuspendLayout();
			this.grpFrameCounter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.grpChannelControl, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.grpSquare1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpSquare2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.grpTriangle, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpNoise, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.grpDmc, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.grpFrameCounter, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(987, 533);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// grpChannelControl
			// 
			this.grpChannelControl.Controls.Add(this.tableLayoutPanel2);
			this.grpChannelControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpChannelControl.Location = new System.Drawing.Point(712, 408);
			this.grpChannelControl.Name = "grpChannelControl";
			this.grpChannelControl.Size = new System.Drawing.Size(270, 121);
			this.grpChannelControl.TabIndex = 8;
			this.grpChannelControl.TabStop = false;
			this.grpChannelControl.Text = "Channel Control (uncheck to mute)";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel2.Controls.Add(this.chkFds, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkSquare1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkSquare2, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkTriangle, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkNoise, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkMmc5, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkNamco, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkVrc7, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkDmc, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkSunsoft, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkVrc6, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(264, 102);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkFds
			// 
			this.chkFds.AutoSize = true;
			this.chkFds.Checked = true;
			this.chkFds.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFds.Location = new System.Drawing.Point(90, 0);
			this.chkFds.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkFds.Name = "chkFds";
			this.chkFds.Size = new System.Drawing.Size(47, 17);
			this.chkFds.TabIndex = 5;
			this.chkFds.Text = "FDS";
			this.chkFds.UseVisualStyleBackColor = true;
			this.chkFds.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkSquare1
			// 
			this.chkSquare1.AutoSize = true;
			this.chkSquare1.Checked = true;
			this.chkSquare1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSquare1.Location = new System.Drawing.Point(3, 0);
			this.chkSquare1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkSquare1.Name = "chkSquare1";
			this.chkSquare1.Size = new System.Drawing.Size(69, 17);
			this.chkSquare1.TabIndex = 0;
			this.chkSquare1.Text = "Square 1";
			this.chkSquare1.UseVisualStyleBackColor = true;
			this.chkSquare1.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkSquare2
			// 
			this.chkSquare2.AutoSize = true;
			this.chkSquare2.Checked = true;
			this.chkSquare2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSquare2.Location = new System.Drawing.Point(3, 20);
			this.chkSquare2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkSquare2.Name = "chkSquare2";
			this.chkSquare2.Size = new System.Drawing.Size(69, 17);
			this.chkSquare2.TabIndex = 1;
			this.chkSquare2.Text = "Square 2";
			this.chkSquare2.UseVisualStyleBackColor = true;
			this.chkSquare2.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkTriangle
			// 
			this.chkTriangle.AutoSize = true;
			this.chkTriangle.Checked = true;
			this.chkTriangle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTriangle.Location = new System.Drawing.Point(3, 40);
			this.chkTriangle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkTriangle.Name = "chkTriangle";
			this.chkTriangle.Size = new System.Drawing.Size(64, 17);
			this.chkTriangle.TabIndex = 2;
			this.chkTriangle.Text = "Triangle";
			this.chkTriangle.UseVisualStyleBackColor = true;
			this.chkTriangle.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkNoise
			// 
			this.chkNoise.AutoSize = true;
			this.chkNoise.Checked = true;
			this.chkNoise.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNoise.Location = new System.Drawing.Point(3, 60);
			this.chkNoise.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkNoise.Name = "chkNoise";
			this.chkNoise.Size = new System.Drawing.Size(53, 17);
			this.chkNoise.TabIndex = 3;
			this.chkNoise.Text = "Noise";
			this.chkNoise.UseVisualStyleBackColor = true;
			this.chkNoise.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkMmc5
			// 
			this.chkMmc5.AutoSize = true;
			this.chkMmc5.Checked = true;
			this.chkMmc5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMmc5.Location = new System.Drawing.Point(90, 20);
			this.chkMmc5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkMmc5.Name = "chkMmc5";
			this.chkMmc5.Size = new System.Drawing.Size(57, 17);
			this.chkMmc5.TabIndex = 9;
			this.chkMmc5.Text = "MMC5";
			this.chkMmc5.UseVisualStyleBackColor = true;
			this.chkMmc5.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkNamco
			// 
			this.chkNamco.AutoSize = true;
			this.chkNamco.Checked = true;
			this.chkNamco.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNamco.Location = new System.Drawing.Point(90, 40);
			this.chkNamco.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkNamco.Name = "chkNamco";
			this.chkNamco.Size = new System.Drawing.Size(60, 17);
			this.chkNamco.TabIndex = 6;
			this.chkNamco.Text = "Namco";
			this.chkNamco.UseVisualStyleBackColor = true;
			this.chkNamco.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkVrc7
			// 
			this.chkVrc7.AutoSize = true;
			this.chkVrc7.Checked = true;
			this.chkVrc7.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVrc7.Location = new System.Drawing.Point(178, 20);
			this.chkVrc7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkVrc7.Name = "chkVrc7";
			this.chkVrc7.Size = new System.Drawing.Size(54, 17);
			this.chkVrc7.TabIndex = 8;
			this.chkVrc7.Text = "VRC7";
			this.chkVrc7.UseVisualStyleBackColor = true;
			this.chkVrc7.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkDmc
			// 
			this.chkDmc.AutoSize = true;
			this.chkDmc.Checked = true;
			this.chkDmc.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDmc.Location = new System.Drawing.Point(3, 80);
			this.chkDmc.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkDmc.Name = "chkDmc";
			this.chkDmc.Size = new System.Drawing.Size(50, 17);
			this.chkDmc.TabIndex = 4;
			this.chkDmc.Text = "DMC";
			this.chkDmc.UseVisualStyleBackColor = true;
			this.chkDmc.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkSunsoft
			// 
			this.chkSunsoft.AutoSize = true;
			this.chkSunsoft.Checked = true;
			this.chkSunsoft.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSunsoft.Location = new System.Drawing.Point(90, 60);
			this.chkSunsoft.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkSunsoft.Name = "chkSunsoft";
			this.chkSunsoft.Size = new System.Drawing.Size(62, 17);
			this.chkSunsoft.TabIndex = 10;
			this.chkSunsoft.Text = "Sunsoft";
			this.chkSunsoft.UseVisualStyleBackColor = true;
			this.chkSunsoft.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// chkVrc6
			// 
			this.chkVrc6.AutoSize = true;
			this.chkVrc6.Checked = true;
			this.chkVrc6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVrc6.Location = new System.Drawing.Point(178, 0);
			this.chkVrc6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.chkVrc6.Name = "chkVrc6";
			this.chkVrc6.Size = new System.Drawing.Size(54, 17);
			this.chkVrc6.TabIndex = 7;
			this.chkVrc6.Text = "VRC6";
			this.chkVrc6.UseVisualStyleBackColor = true;
			this.chkVrc6.CheckedChanged += new System.EventHandler(this.chkSoundChannel_CheckedChanged);
			// 
			// grpSquare1
			// 
			this.grpSquare1.Controls.Add(this.ctrlSquareInfo1);
			this.grpSquare1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSquare1.Location = new System.Drawing.Point(3, 3);
			this.grpSquare1.Name = "grpSquare1";
			this.grpSquare1.Size = new System.Drawing.Size(547, 196);
			this.grpSquare1.TabIndex = 3;
			this.grpSquare1.TabStop = false;
			this.grpSquare1.Text = "Square 1";
			// 
			// ctrlSquareInfo1
			// 
			this.ctrlSquareInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSquareInfo1.Location = new System.Drawing.Point(3, 16);
			this.ctrlSquareInfo1.Name = "ctrlSquareInfo1";
			this.ctrlSquareInfo1.Size = new System.Drawing.Size(541, 177);
			this.ctrlSquareInfo1.TabIndex = 1;
			// 
			// grpSquare2
			// 
			this.grpSquare2.Controls.Add(this.ctrlSquareInfo2);
			this.grpSquare2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSquare2.Location = new System.Drawing.Point(3, 205);
			this.grpSquare2.Name = "grpSquare2";
			this.grpSquare2.Size = new System.Drawing.Size(547, 197);
			this.grpSquare2.TabIndex = 2;
			this.grpSquare2.TabStop = false;
			this.grpSquare2.Text = "Square 2";
			// 
			// ctrlSquareInfo2
			// 
			this.ctrlSquareInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSquareInfo2.Location = new System.Drawing.Point(3, 16);
			this.ctrlSquareInfo2.Name = "ctrlSquareInfo2";
			this.ctrlSquareInfo2.Size = new System.Drawing.Size(541, 178);
			this.ctrlSquareInfo2.TabIndex = 1;
			// 
			// grpTriangle
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.grpTriangle, 2);
			this.grpTriangle.Controls.Add(this.ctrlTriangleInfo);
			this.grpTriangle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpTriangle.Location = new System.Drawing.Point(556, 3);
			this.grpTriangle.Name = "grpTriangle";
			this.grpTriangle.Size = new System.Drawing.Size(426, 196);
			this.grpTriangle.TabIndex = 4;
			this.grpTriangle.TabStop = false;
			this.grpTriangle.Text = "Triangle";
			// 
			// ctrlTriangleInfo
			// 
			this.ctrlTriangleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlTriangleInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlTriangleInfo.Name = "ctrlTriangleInfo";
			this.ctrlTriangleInfo.Size = new System.Drawing.Size(420, 177);
			this.ctrlTriangleInfo.TabIndex = 0;
			// 
			// grpNoise
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.grpNoise, 2);
			this.grpNoise.Controls.Add(this.ctrlNoiseInfo);
			this.grpNoise.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpNoise.Location = new System.Drawing.Point(556, 205);
			this.grpNoise.Name = "grpNoise";
			this.grpNoise.Size = new System.Drawing.Size(426, 197);
			this.grpNoise.TabIndex = 5;
			this.grpNoise.TabStop = false;
			this.grpNoise.Text = "Noise";
			// 
			// ctrlNoiseInfo
			// 
			this.ctrlNoiseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNoiseInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlNoiseInfo.Name = "ctrlNoiseInfo";
			this.ctrlNoiseInfo.Size = new System.Drawing.Size(420, 178);
			this.ctrlNoiseInfo.TabIndex = 0;
			// 
			// grpDmc
			// 
			this.grpDmc.Controls.Add(this.ctrlDmcInfo);
			this.grpDmc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpDmc.Location = new System.Drawing.Point(3, 408);
			this.grpDmc.Name = "grpDmc";
			this.grpDmc.Size = new System.Drawing.Size(547, 121);
			this.grpDmc.TabIndex = 6;
			this.grpDmc.TabStop = false;
			this.grpDmc.Text = "DMC";
			// 
			// ctrlDmcInfo
			// 
			this.ctrlDmcInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDmcInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlDmcInfo.Name = "ctrlDmcInfo";
			this.ctrlDmcInfo.Size = new System.Drawing.Size(541, 102);
			this.ctrlDmcInfo.TabIndex = 0;
			// 
			// grpFrameCounter
			// 
			this.grpFrameCounter.Controls.Add(this.ctrlFrameCounterInfo);
			this.grpFrameCounter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFrameCounter.Location = new System.Drawing.Point(556, 408);
			this.grpFrameCounter.Name = "grpFrameCounter";
			this.grpFrameCounter.Size = new System.Drawing.Size(150, 121);
			this.grpFrameCounter.TabIndex = 7;
			this.grpFrameCounter.TabStop = false;
			this.grpFrameCounter.Text = "Frame Counter";
			// 
			// ctrlFrameCounterInfo
			// 
			this.ctrlFrameCounterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFrameCounterInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlFrameCounterInfo.Name = "ctrlFrameCounterInfo";
			this.ctrlFrameCounterInfo.Size = new System.Drawing.Size(144, 102);
			this.ctrlFrameCounterInfo.TabIndex = 0;
			// 
			// tmrUpdate
			// 
			this.tmrUpdate.Interval = 67;
			this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
			// 
			// frmApuViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(987, 533);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmApuViewer";
			this.Text = "APU Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpChannelControl.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpSquare1.ResumeLayout(false);
			this.grpSquare2.ResumeLayout(false);
			this.grpTriangle.ResumeLayout(false);
			this.grpNoise.ResumeLayout(false);
			this.grpDmc.ResumeLayout(false);
			this.grpFrameCounter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Timer tmrUpdate;
		private System.Windows.Forms.GroupBox grpSquare1;
		private Controls.ctrlSquareInfo ctrlSquareInfo1;
		private System.Windows.Forms.GroupBox grpSquare2;
		private Controls.ctrlSquareInfo ctrlSquareInfo2;
		private System.Windows.Forms.GroupBox grpTriangle;
		private Controls.ctrlTriangleInfo ctrlTriangleInfo;
		private System.Windows.Forms.GroupBox grpNoise;
		private Controls.ctrlNoiseInfo ctrlNoiseInfo;
		private System.Windows.Forms.GroupBox grpDmc;
		private Controls.ctrlDmcInfo ctrlDmcInfo;
		private System.Windows.Forms.GroupBox grpFrameCounter;
		private Controls.ctrlFrameCounterInfo ctrlFrameCounterInfo;
		private System.Windows.Forms.GroupBox grpChannelControl;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkFds;
		private System.Windows.Forms.CheckBox chkSquare1;
		private System.Windows.Forms.CheckBox chkSquare2;
		private System.Windows.Forms.CheckBox chkTriangle;
		private System.Windows.Forms.CheckBox chkNoise;
		private System.Windows.Forms.CheckBox chkDmc;
		private System.Windows.Forms.CheckBox chkMmc5;
		private System.Windows.Forms.CheckBox chkNamco;
		private System.Windows.Forms.CheckBox chkVrc7;
		private System.Windows.Forms.CheckBox chkVrc6;
		private System.Windows.Forms.CheckBox chkSunsoft;
	}
}