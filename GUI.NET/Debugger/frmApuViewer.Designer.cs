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
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 533);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// grpSquare1
			// 
			this.grpSquare1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpSquare1.Controls.Add(this.ctrlSquareInfo1);
			this.grpSquare1.Location = new System.Drawing.Point(3, 3);
			this.grpSquare1.Name = "grpSquare1";
			this.grpSquare1.Size = new System.Drawing.Size(549, 196);
			this.grpSquare1.TabIndex = 3;
			this.grpSquare1.TabStop = false;
			this.grpSquare1.Text = "Square 1";
			// 
			// ctrlSquareInfo1
			// 
			this.ctrlSquareInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSquareInfo1.Location = new System.Drawing.Point(3, 16);
			this.ctrlSquareInfo1.Name = "ctrlSquareInfo1";
			this.ctrlSquareInfo1.Size = new System.Drawing.Size(543, 177);
			this.ctrlSquareInfo1.TabIndex = 1;
			// 
			// grpSquare2
			// 
			this.grpSquare2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpSquare2.Controls.Add(this.ctrlSquareInfo2);
			this.grpSquare2.Location = new System.Drawing.Point(3, 205);
			this.grpSquare2.Name = "grpSquare2";
			this.grpSquare2.Size = new System.Drawing.Size(549, 197);
			this.grpSquare2.TabIndex = 2;
			this.grpSquare2.TabStop = false;
			this.grpSquare2.Text = "Square 2";
			// 
			// ctrlSquareInfo2
			// 
			this.ctrlSquareInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSquareInfo2.Location = new System.Drawing.Point(3, 16);
			this.ctrlSquareInfo2.Name = "ctrlSquareInfo2";
			this.ctrlSquareInfo2.Size = new System.Drawing.Size(543, 178);
			this.ctrlSquareInfo2.TabIndex = 1;
			// 
			// grpTriangle
			// 
			this.grpTriangle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpTriangle.Controls.Add(this.ctrlTriangleInfo);
			this.grpTriangle.Location = new System.Drawing.Point(558, 3);
			this.grpTriangle.Name = "grpTriangle";
			this.grpTriangle.Size = new System.Drawing.Size(429, 196);
			this.grpTriangle.TabIndex = 4;
			this.grpTriangle.TabStop = false;
			this.grpTriangle.Text = "Triangle";
			// 
			// ctrlTriangleInfo
			// 
			this.ctrlTriangleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlTriangleInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlTriangleInfo.Name = "ctrlTriangleInfo";
			this.ctrlTriangleInfo.Size = new System.Drawing.Size(423, 177);
			this.ctrlTriangleInfo.TabIndex = 0;
			// 
			// grpNoise
			// 
			this.grpNoise.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpNoise.Controls.Add(this.ctrlNoiseInfo);
			this.grpNoise.Location = new System.Drawing.Point(558, 205);
			this.grpNoise.Name = "grpNoise";
			this.grpNoise.Size = new System.Drawing.Size(429, 197);
			this.grpNoise.TabIndex = 5;
			this.grpNoise.TabStop = false;
			this.grpNoise.Text = "Noise";
			// 
			// ctrlNoiseInfo
			// 
			this.ctrlNoiseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNoiseInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlNoiseInfo.Name = "ctrlNoiseInfo";
			this.ctrlNoiseInfo.Size = new System.Drawing.Size(423, 178);
			this.ctrlNoiseInfo.TabIndex = 0;
			// 
			// grpDmc
			// 
			this.grpDmc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDmc.Controls.Add(this.ctrlDmcInfo);
			this.grpDmc.Location = new System.Drawing.Point(3, 408);
			this.grpDmc.Name = "grpDmc";
			this.grpDmc.Size = new System.Drawing.Size(549, 121);
			this.grpDmc.TabIndex = 6;
			this.grpDmc.TabStop = false;
			this.grpDmc.Text = "DMC";
			// 
			// ctrlDmcInfo
			// 
			this.ctrlDmcInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDmcInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlDmcInfo.Name = "ctrlDmcInfo";
			this.ctrlDmcInfo.Size = new System.Drawing.Size(543, 102);
			this.ctrlDmcInfo.TabIndex = 0;
			// 
			// grpFrameCounter
			// 
			this.grpFrameCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpFrameCounter.Controls.Add(this.ctrlFrameCounterInfo);
			this.grpFrameCounter.Location = new System.Drawing.Point(558, 408);
			this.grpFrameCounter.Name = "grpFrameCounter";
			this.grpFrameCounter.Size = new System.Drawing.Size(429, 121);
			this.grpFrameCounter.TabIndex = 7;
			this.grpFrameCounter.TabStop = false;
			this.grpFrameCounter.Text = "Frame Counter";
			// 
			// ctrlFrameCounterInfo
			// 
			this.ctrlFrameCounterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFrameCounterInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlFrameCounterInfo.Name = "ctrlFrameCounterInfo";
			this.ctrlFrameCounterInfo.Size = new System.Drawing.Size(423, 102);
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
			this.ClientSize = new System.Drawing.Size(990, 533);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmApuViewer";
			this.Text = "APU Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
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
	}
}