using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	partial class frmPpuViewer
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
			if(this._notifListener != null) {
				this._notifListener.Dispose();
				this._notifListener = null;
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
			this.menuStrip1 = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgNametableViewer = new System.Windows.Forms.TabPage();
			this.ctrlNametableViewer = new Mesen.GUI.Debugger.Controls.ctrlNametableViewer();
			this.tpgChrViewer = new System.Windows.Forms.TabPage();
			this.ctrlChrViewer = new Mesen.GUI.Debugger.Controls.ctrlChrViewer();
			this.tpgSpriteViewer = new System.Windows.Forms.TabPage();
			this.ctrlSpriteViewer = new Mesen.GUI.Debugger.Controls.ctrlSpriteViewer();
			this.tpgPaletteViewer = new System.Windows.Forms.TabPage();
			this.ctrlPaletteViewer = new Mesen.GUI.Debugger.Controls.ctrlPaletteViewer();
			this.flpRefreshTiming = new System.Windows.Forms.FlowLayoutPanel();
			this.lblShowFrameAt = new System.Windows.Forms.Label();
			this.nudScanline = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.lblCycle = new System.Windows.Forms.Label();
			this.nudCycle = new Mesen.GUI.Controls.MesenNumericUpDown();
			this.btnReset = new System.Windows.Forms.Button();
			this.mnuRefreshOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgNametableViewer.SuspendLayout();
			this.tpgChrViewer.SuspendLayout();
			this.tpgSpriteViewer.SuspendLayout();
			this.tpgPaletteViewer.SuspendLayout();
			this.flpRefreshTiming.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(709, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClose});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuClose
			// 
			this.mnuClose.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(103, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefresh,
            this.toolStripMenuItem1,
            this.mnuAutoRefresh,
            this.mnuRefreshOnBreak});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.Size = new System.Drawing.Size(198, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(198, 22);
			this.mnuAutoRefresh.Text = "Auto-refresh";
			this.mnuAutoRefresh.Click += new System.EventHandler(this.mnuAutoRefresh_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgNametableViewer);
			this.tabMain.Controls.Add(this.tpgChrViewer);
			this.tabMain.Controls.Add(this.tpgSpriteViewer);
			this.tabMain.Controls.Add(this.tpgPaletteViewer);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 24);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(709, 551);
			this.tabMain.TabIndex = 3;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tpgNametableViewer
			// 
			this.tpgNametableViewer.Controls.Add(this.ctrlNametableViewer);
			this.tpgNametableViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgNametableViewer.Name = "tpgNametableViewer";
			this.tpgNametableViewer.Size = new System.Drawing.Size(701, 525);
			this.tpgNametableViewer.TabIndex = 0;
			this.tpgNametableViewer.Text = "Nametable Viewer";
			this.tpgNametableViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlNametableViewer
			// 
			this.ctrlNametableViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNametableViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlNametableViewer.Name = "ctrlNametableViewer";
			this.ctrlNametableViewer.Size = new System.Drawing.Size(701, 525);
			this.ctrlNametableViewer.TabIndex = 0;
			this.ctrlNametableViewer.OnSelectChrTile += new System.EventHandler(this.ctrlNametableViewer_OnSelectChrTile);
			// 
			// tpgChrViewer
			// 
			this.tpgChrViewer.Controls.Add(this.ctrlChrViewer);
			this.tpgChrViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgChrViewer.Name = "tpgChrViewer";
			this.tpgChrViewer.Size = new System.Drawing.Size(701, 525);
			this.tpgChrViewer.TabIndex = 1;
			this.tpgChrViewer.Text = "CHR Viewer";
			this.tpgChrViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlChrViewer
			// 
			this.ctrlChrViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlChrViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlChrViewer.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlChrViewer.Name = "ctrlChrViewer";
			this.ctrlChrViewer.Size = new System.Drawing.Size(701, 525);
			this.ctrlChrViewer.TabIndex = 2;
			// 
			// tpgSpriteViewer
			// 
			this.tpgSpriteViewer.Controls.Add(this.ctrlSpriteViewer);
			this.tpgSpriteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgSpriteViewer.Name = "tpgSpriteViewer";
			this.tpgSpriteViewer.Size = new System.Drawing.Size(701, 525);
			this.tpgSpriteViewer.TabIndex = 2;
			this.tpgSpriteViewer.Text = "Sprite Viewer";
			this.tpgSpriteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlSpriteViewer
			// 
			this.ctrlSpriteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSpriteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlSpriteViewer.Name = "ctrlSpriteViewer";
			this.ctrlSpriteViewer.Size = new System.Drawing.Size(701, 525);
			this.ctrlSpriteViewer.TabIndex = 0;
			this.ctrlSpriteViewer.OnSelectTilePalette += new Mesen.GUI.Debugger.Controls.ctrlSpriteViewer.SelectTilePaletteHandler(this.ctrlSpriteViewer_OnSelectTilePalette);
			// 
			// tpgPaletteViewer
			// 
			this.tpgPaletteViewer.Controls.Add(this.ctrlPaletteViewer);
			this.tpgPaletteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgPaletteViewer.Name = "tpgPaletteViewer";
			this.tpgPaletteViewer.Size = new System.Drawing.Size(701, 525);
			this.tpgPaletteViewer.TabIndex = 3;
			this.tpgPaletteViewer.Text = "Palette Viewer";
			this.tpgPaletteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlPaletteViewer
			// 
			this.ctrlPaletteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPaletteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlPaletteViewer.Name = "ctrlPaletteViewer";
			this.ctrlPaletteViewer.Size = new System.Drawing.Size(701, 525);
			this.ctrlPaletteViewer.TabIndex = 0;
			// 
			// flpRefreshTiming
			// 
			this.flpRefreshTiming.Controls.Add(this.lblShowFrameAt);
			this.flpRefreshTiming.Controls.Add(this.nudScanline);
			this.flpRefreshTiming.Controls.Add(this.lblCycle);
			this.flpRefreshTiming.Controls.Add(this.nudCycle);
			this.flpRefreshTiming.Controls.Add(this.btnReset);
			this.flpRefreshTiming.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flpRefreshTiming.Location = new System.Drawing.Point(0, 575);
			this.flpRefreshTiming.Name = "flpRefreshTiming";
			this.flpRefreshTiming.Size = new System.Drawing.Size(709, 30);
			this.flpRefreshTiming.TabIndex = 4;
			// 
			// lblShowFrameAt
			// 
			this.lblShowFrameAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblShowFrameAt.AutoSize = true;
			this.lblShowFrameAt.Location = new System.Drawing.Point(3, 8);
			this.lblShowFrameAt.Name = "lblShowFrameAt";
			this.lblShowFrameAt.Size = new System.Drawing.Size(266, 13);
			this.lblShowFrameAt.TabIndex = 0;
			this.lblShowFrameAt.Text = "When emulation is running, show PPU data at scanline";
			// 
			// nudScanline
			// 
			this.nudScanline.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudScanline.DecimalPlaces = 0;
			this.nudScanline.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudScanline.Location = new System.Drawing.Point(275, 4);
			this.nudScanline.Maximum = new decimal(new int[] {
            260,
            0,
            0,
            0});
			this.nudScanline.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudScanline.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.nudScanline.Name = "nudScanline";
			this.nudScanline.Size = new System.Drawing.Size(52, 20);
			this.nudScanline.TabIndex = 5;
			this.nudScanline.Value = new decimal(new int[] {
            241,
            0,
            0,
            0});
			this.nudScanline.ValueChanged += new System.EventHandler(this.nudScanlineCycle_ValueChanged);
			// 
			// lblCycle
			// 
			this.lblCycle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCycle.AutoSize = true;
			this.lblCycle.Location = new System.Drawing.Point(333, 8);
			this.lblCycle.Name = "lblCycle";
			this.lblCycle.Size = new System.Drawing.Size(53, 13);
			this.lblCycle.TabIndex = 5;
			this.lblCycle.Text = "and cycle";
			// 
			// nudCycle
			// 
			this.nudCycle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nudCycle.DecimalPlaces = 0;
			this.nudCycle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudCycle.Location = new System.Drawing.Point(392, 4);
			this.nudCycle.Maximum = new decimal(new int[] {
            340,
            0,
            0,
            0});
			this.nudCycle.MaximumSize = new System.Drawing.Size(10000, 20);
			this.nudCycle.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudCycle.Name = "nudCycle";
			this.nudCycle.Size = new System.Drawing.Size(52, 20);
			this.nudCycle.TabIndex = 6;
			this.nudCycle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.nudCycle.ValueChanged += new System.EventHandler(this.nudScanlineCycle_ValueChanged);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(450, 3);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(75, 23);
			this.btnReset.TabIndex = 7;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// mnuRefreshOnBreak
			// 
			this.mnuRefreshOnBreak.CheckOnClick = true;
			this.mnuRefreshOnBreak.Name = "mnuRefreshOnBreak";
			this.mnuRefreshOnBreak.Size = new System.Drawing.Size(198, 22);
			this.mnuRefreshOnBreak.Text = "Refresh on pause/break";
			this.mnuRefreshOnBreak.Click += new System.EventHandler(this.mnuRefreshOnBreak_Click);
			// 
			// frmPpuViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 605);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.flpRefreshTiming);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(725, 644);
			this.Name = "frmPpuViewer";
			this.Text = "PPU Viewer";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgNametableViewer.ResumeLayout(false);
			this.tpgChrViewer.ResumeLayout(false);
			this.tpgSpriteViewer.ResumeLayout(false);
			this.tpgPaletteViewer.ResumeLayout(false);
			this.flpRefreshTiming.ResumeLayout(false);
			this.flpRefreshTiming.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Mesen.GUI.Controls.ctrlMesenMenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefresh;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgNametableViewer;
		private System.Windows.Forms.TabPage tpgChrViewer;
		private System.Windows.Forms.TabPage tpgSpriteViewer;
		private Controls.ctrlNametableViewer ctrlNametableViewer;
		private Controls.ctrlChrViewer ctrlChrViewer;
		private Controls.ctrlSpriteViewer ctrlSpriteViewer;
		private System.Windows.Forms.TabPage tpgPaletteViewer;
		private Controls.ctrlPaletteViewer ctrlPaletteViewer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.FlowLayoutPanel flpRefreshTiming;
		private System.Windows.Forms.Label lblShowFrameAt;
		private MesenNumericUpDown nudScanline;
		private System.Windows.Forms.Label lblCycle;
		private MesenNumericUpDown nudCycle;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshOnBreak;
	}
}