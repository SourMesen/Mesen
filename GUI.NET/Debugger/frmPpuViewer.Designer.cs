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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
			this.menuStrip1.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgNametableViewer.SuspendLayout();
			this.tpgChrViewer.SuspendLayout();
			this.tpgSpriteViewer.SuspendLayout();
			this.tpgPaletteViewer.SuspendLayout();
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
            this.mnuAutoRefresh});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Name = "mnuRefresh";
			this.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefresh.Size = new System.Drawing.Size(152, 22);
			this.mnuRefresh.Text = "Refresh";
			this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// mnuAutoRefresh
			// 
			this.mnuAutoRefresh.Checked = true;
			this.mnuAutoRefresh.CheckOnClick = true;
			this.mnuAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuAutoRefresh.Name = "mnuAutoRefresh";
			this.mnuAutoRefresh.Size = new System.Drawing.Size(152, 22);
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
			this.tabMain.Size = new System.Drawing.Size(709, 554);
			this.tabMain.TabIndex = 3;
			// 
			// tpgNametableViewer
			// 
			this.tpgNametableViewer.Controls.Add(this.ctrlNametableViewer);
			this.tpgNametableViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgNametableViewer.Name = "tpgNametableViewer";
			this.tpgNametableViewer.Padding = new System.Windows.Forms.Padding(3);
			this.tpgNametableViewer.Size = new System.Drawing.Size(701, 528);
			this.tpgNametableViewer.TabIndex = 0;
			this.tpgNametableViewer.Text = "Nametable Viewer";
			this.tpgNametableViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlNametableViewer
			// 
			this.ctrlNametableViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlNametableViewer.Location = new System.Drawing.Point(3, 3);
			this.ctrlNametableViewer.Name = "ctrlNametableViewer";
			this.ctrlNametableViewer.Size = new System.Drawing.Size(695, 522);
			this.ctrlNametableViewer.TabIndex = 0;
			// 
			// tpgChrViewer
			// 
			this.tpgChrViewer.Controls.Add(this.ctrlChrViewer);
			this.tpgChrViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgChrViewer.Name = "tpgChrViewer";
			this.tpgChrViewer.Padding = new System.Windows.Forms.Padding(3);
			this.tpgChrViewer.Size = new System.Drawing.Size(701, 528);
			this.tpgChrViewer.TabIndex = 1;
			this.tpgChrViewer.Text = "CHR Viewer";
			this.tpgChrViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlChrViewer
			// 
			this.ctrlChrViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlChrViewer.Location = new System.Drawing.Point(3, 3);
			this.ctrlChrViewer.Name = "ctrlChrViewer";
			this.ctrlChrViewer.Size = new System.Drawing.Size(695, 522);
			this.ctrlChrViewer.TabIndex = 2;
			// 
			// tpgSpriteViewer
			// 
			this.tpgSpriteViewer.Controls.Add(this.ctrlSpriteViewer);
			this.tpgSpriteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgSpriteViewer.Name = "tpgSpriteViewer";
			this.tpgSpriteViewer.Size = new System.Drawing.Size(701, 528);
			this.tpgSpriteViewer.TabIndex = 2;
			this.tpgSpriteViewer.Text = "Sprite Viewer";
			this.tpgSpriteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlSpriteViewer
			// 
			this.ctrlSpriteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSpriteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlSpriteViewer.Name = "ctrlSpriteViewer";
			this.ctrlSpriteViewer.Size = new System.Drawing.Size(701, 528);
			this.ctrlSpriteViewer.TabIndex = 0;
			// 
			// tpgPaletteViewer
			// 
			this.tpgPaletteViewer.Controls.Add(this.ctrlPaletteViewer);
			this.tpgPaletteViewer.Location = new System.Drawing.Point(4, 22);
			this.tpgPaletteViewer.Name = "tpgPaletteViewer";
			this.tpgPaletteViewer.Size = new System.Drawing.Size(701, 528);
			this.tpgPaletteViewer.TabIndex = 3;
			this.tpgPaletteViewer.Text = "Palette Viewer";
			this.tpgPaletteViewer.UseVisualStyleBackColor = true;
			// 
			// ctrlPaletteViewer
			// 
			this.ctrlPaletteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPaletteViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlPaletteViewer.Name = "ctrlPaletteViewer";
			this.ctrlPaletteViewer.Size = new System.Drawing.Size(701, 528);
			this.ctrlPaletteViewer.TabIndex = 0;
			// 
			// frmPpuViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 578);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(725, 607);
			this.Name = "frmPpuViewer";
			this.Text = "PPU Viewer";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgNametableViewer.ResumeLayout(false);
			this.tpgChrViewer.ResumeLayout(false);
			this.tpgSpriteViewer.ResumeLayout(false);
			this.tpgPaletteViewer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
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
	}
}