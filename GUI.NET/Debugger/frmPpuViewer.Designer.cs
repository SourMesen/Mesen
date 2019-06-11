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
			this.menuStrip = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.autorefreshSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoRefreshLow = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoRefreshNormal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoRefreshHigh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefreshOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowInformationOverlay = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgNametableViewer = new System.Windows.Forms.TabPage();
			this.ctrlNametableViewer = new Mesen.GUI.Debugger.Controls.ctrlNametableViewer();
			this.tpgChrViewer = new System.Windows.Forms.TabPage();
			this.ctrlChrViewer = new Mesen.GUI.Debugger.Controls.ctrlChrViewer();
			this.tpgSpriteViewer = new System.Windows.Forms.TabPage();
			this.ctrlSpriteViewer = new Mesen.GUI.Debugger.Controls.ctrlSpriteViewer();
			this.tpgPaletteViewer = new System.Windows.Forms.TabPage();
			this.ctrlPaletteViewer = new Mesen.GUI.Debugger.Controls.ctrlPaletteViewer();
			this.ctrlScanlineCycle = new Mesen.GUI.Debugger.Controls.ctrlScanlineCycleSelect();
			this.btnToggleView = new System.Windows.Forms.Button();
			this.chkToggleZoom = new System.Windows.Forms.CheckBox();
			this.menuStrip.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgNametableViewer.SuspendLayout();
			this.tpgChrViewer.SuspendLayout();
			this.tpgSpriteViewer.SuspendLayout();
			this.tpgPaletteViewer.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(709, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
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
            this.autorefreshSpeedToolStripMenuItem,
            this.toolStripMenuItem1,
            this.mnuAutoRefresh,
            this.mnuRefreshOnBreak,
            this.mnuShowInformationOverlay});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Image = global::Mesen.GUI.Properties.Resources.Reset;
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.Size = new System.Drawing.Size(210, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// autorefreshSpeedToolStripMenuItem
			// 
			this.autorefreshSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAutoRefreshLow,
            this.mnuAutoRefreshNormal,
            this.mnuAutoRefreshHigh});
			this.autorefreshSpeedToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.Speed;
			this.autorefreshSpeedToolStripMenuItem.Name = "autorefreshSpeedToolStripMenuItem";
			this.autorefreshSpeedToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.autorefreshSpeedToolStripMenuItem.Text = "Auto-refresh Speed";
			// 
			// mnuAutoRefreshLow
			// 
			this.mnuAutoRefreshLow.Name = "mnuAutoRefreshLow";
			this.mnuAutoRefreshLow.Size = new System.Drawing.Size(159, 22);
			this.mnuAutoRefreshLow.Text = "Low (15 FPS)";
			this.mnuAutoRefreshLow.Click += new System.EventHandler(this.mnuAutoRefreshSpeed_Click);
			// 
			// mnuAutoRefreshNormal
			// 
			this.mnuAutoRefreshNormal.Name = "mnuAutoRefreshNormal";
			this.mnuAutoRefreshNormal.Size = new System.Drawing.Size(159, 22);
			this.mnuAutoRefreshNormal.Text = "Normal (30 FPS)";
			this.mnuAutoRefreshNormal.Click += new System.EventHandler(this.mnuAutoRefreshSpeed_Click);
			// 
			// mnuAutoRefreshHigh
			// 
			this.mnuAutoRefreshHigh.Name = "mnuAutoRefreshHigh";
			this.mnuAutoRefreshHigh.Size = new System.Drawing.Size(159, 22);
			this.mnuAutoRefreshHigh.Text = "High (60 FPS)";
			this.mnuAutoRefreshHigh.Click += new System.EventHandler(this.mnuAutoRefreshSpeed_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(210, 22);
			this.mnuAutoRefresh.Text = "Auto-refresh";
			this.mnuAutoRefresh.Click += new System.EventHandler(this.mnuAutoRefresh_Click);
			// 
			// mnuRefreshOnBreak
			// 
			this.mnuRefreshOnBreak.CheckOnClick = true;
			this.mnuRefreshOnBreak.Name = "mnuRefreshOnBreak";
			this.mnuRefreshOnBreak.Size = new System.Drawing.Size(210, 22);
			this.mnuRefreshOnBreak.Text = "Refresh on pause/break";
			this.mnuRefreshOnBreak.Click += new System.EventHandler(this.mnuRefreshOnBreak_Click);
			// 
			// mnuShowInformationOverlay
			// 
			this.mnuShowInformationOverlay.CheckOnClick = true;
			this.mnuShowInformationOverlay.Name = "mnuShowInformationOverlay";
			this.mnuShowInformationOverlay.Size = new System.Drawing.Size(210, 22);
			this.mnuShowInformationOverlay.Text = "Show information overlay";
			this.mnuShowInformationOverlay.Click += new System.EventHandler(this.mnuShowInformationOverlay_Click);
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
			this.tabMain.Size = new System.Drawing.Size(709, 553);
			this.tabMain.TabIndex = 3;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tpgNametableViewer
			// 
			this.tpgNametableViewer.Controls.Add(this.ctrlNametableViewer);
			this.tpgNametableViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgNametableViewer.Name = "tpgNametableViewer";
			this.tpgNametableViewer.Size = new System.Drawing.Size(701, 527);
			this.tpgNametableViewer.TabIndex = 0;
			this.tpgNametableViewer.Text = "Nametable Viewer";
			this.tpgNametableViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlNametableViewer
			// 
			this.ctrlNametableViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNametableViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlNametableViewer.Name = "ctrlNametableViewer";
			this.ctrlNametableViewer.Size = new System.Drawing.Size(701, 527);
			this.ctrlNametableViewer.TabIndex = 0;
			this.ctrlNametableViewer.OnSelectChrTile += new Mesen.GUI.Debugger.Controls.ctrlNametableViewer.OnSelectChrTileHandler(this.ctrlNametableViewer_OnSelectChrTile);
			// 
			// tpgChrViewer
			// 
			this.tpgChrViewer.Controls.Add(this.ctrlChrViewer);
			this.tpgChrViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgChrViewer.Name = "tpgChrViewer";
			this.tpgChrViewer.Size = new System.Drawing.Size(701, 527);
			this.tpgChrViewer.TabIndex = 1;
			this.tpgChrViewer.Text = "CHR Viewer";
			this.tpgChrViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlChrViewer
			// 
			this.ctrlChrViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlChrViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlChrViewer.Name = "ctrlChrViewer";
			this.ctrlChrViewer.Size = new System.Drawing.Size(701, 527);
			this.ctrlChrViewer.TabIndex = 2;
			// 
			// tpgSpriteViewer
			// 
			this.tpgSpriteViewer.Controls.Add(this.ctrlSpriteViewer);
			this.tpgSpriteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgSpriteViewer.Name = "tpgSpriteViewer";
			this.tpgSpriteViewer.Size = new System.Drawing.Size(701, 527);
			this.tpgSpriteViewer.TabIndex = 2;
			this.tpgSpriteViewer.Text = "Sprite Viewer";
			this.tpgSpriteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlSpriteViewer
			// 
			this.ctrlSpriteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSpriteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlSpriteViewer.Name = "ctrlSpriteViewer";
			this.ctrlSpriteViewer.Size = new System.Drawing.Size(701, 527);
			this.ctrlSpriteViewer.TabIndex = 0;
			this.ctrlSpriteViewer.OnSelectTilePalette += new Mesen.GUI.Debugger.Controls.ctrlSpriteViewer.SelectTilePaletteHandler(this.ctrlSpriteViewer_OnSelectTilePalette);
			// 
			// tpgPaletteViewer
			// 
			this.tpgPaletteViewer.Controls.Add(this.ctrlPaletteViewer);
			this.tpgPaletteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgPaletteViewer.Name = "tpgPaletteViewer";
			this.tpgPaletteViewer.Size = new System.Drawing.Size(701, 527);
			this.tpgPaletteViewer.TabIndex = 3;
			this.tpgPaletteViewer.Text = "Palette Viewer";
			this.tpgPaletteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlPaletteViewer
			// 
			this.ctrlPaletteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPaletteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlPaletteViewer.Name = "ctrlPaletteViewer";
			this.ctrlPaletteViewer.Size = new System.Drawing.Size(701, 527);
			this.ctrlPaletteViewer.TabIndex = 0;
			// 
			// ctrlScanlineCycle
			// 
			this.ctrlScanlineCycle.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ctrlScanlineCycle.Location = new System.Drawing.Point(0, 577);
			this.ctrlScanlineCycle.Name = "ctrlScanlineCycle";
			this.ctrlScanlineCycle.Size = new System.Drawing.Size(709, 28);
			this.ctrlScanlineCycle.TabIndex = 4;
			// 
			// btnToggleView
			// 
			this.btnToggleView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnToggleView.Image = global::Mesen.GUI.Properties.Resources.Collapse;
			this.btnToggleView.Location = new System.Drawing.Point(680, 1);
			this.btnToggleView.Name = "btnToggleView";
			this.btnToggleView.Size = new System.Drawing.Size(27, 22);
			this.btnToggleView.TabIndex = 1;
			this.btnToggleView.UseVisualStyleBackColor = true;
			this.btnToggleView.Click += new System.EventHandler(this.btnToggleView_Click);
			// 
			// chkToggleZoom
			// 
			this.chkToggleZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkToggleZoom.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkToggleZoom.AutoCheck = false;
			this.chkToggleZoom.Image = global::Mesen.GUI.Properties.Resources.Zoom2x;
			this.chkToggleZoom.Location = new System.Drawing.Point(647, 1);
			this.chkToggleZoom.Name = "chkToggleZoom";
			this.chkToggleZoom.Size = new System.Drawing.Size(27, 22);
			this.chkToggleZoom.TabIndex = 6;
			this.chkToggleZoom.UseVisualStyleBackColor = true;
			this.chkToggleZoom.Click += new System.EventHandler(this.chkToggleZoom_Click);
			// 
			// frmPpuViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 605);
			this.Controls.Add(this.chkToggleZoom);
			this.Controls.Add(this.btnToggleView);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.ctrlScanlineCycle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip;
			this.MaximizeBox = false;
			this.Name = "frmPpuViewer";
			this.Text = "PPU Viewer";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgNametableViewer.ResumeLayout(false);
			this.tpgChrViewer.ResumeLayout(false);
			this.tpgSpriteViewer.ResumeLayout(false);
			this.tpgPaletteViewer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Mesen.GUI.Controls.ctrlMesenMenuStrip menuStrip;
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
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshOnBreak;
		private Controls.ctrlScanlineCycleSelect ctrlScanlineCycle;
		private System.Windows.Forms.ToolStripMenuItem autorefreshSpeedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefreshLow;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefreshNormal;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoRefreshHigh;
		private System.Windows.Forms.Button btnToggleView;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInformationOverlay;
		private System.Windows.Forms.CheckBox chkToggleZoom;
	}
}