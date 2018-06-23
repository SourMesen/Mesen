using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	partial class frmTextHooker
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
			this.mnuRefreshOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgTextHooker = new System.Windows.Forms.TabPage();
			this.ctrlTextHooker = new Mesen.GUI.Debugger.Controls.ctrlTextHooker();
			this.tpgCharacterMappings = new System.Windows.Forms.TabPage();
			this.ctrlCharacterMappings = new Mesen.GUI.Debugger.Controls.ctrlCharacterMappings();
			this.ctrlScanlineCycle = new Mesen.GUI.Debugger.Controls.ctrlScanlineCycleSelect();
			this.menuStrip1.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgTextHooker.SuspendLayout();
			this.tpgCharacterMappings.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(857, 24);
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
			// mnuRefreshOnBreak
			// 
			this.mnuRefreshOnBreak.CheckOnClick = true;
			this.mnuRefreshOnBreak.Name = "mnuRefreshOnBreak";
			this.mnuRefreshOnBreak.Size = new System.Drawing.Size(198, 22);
			this.mnuRefreshOnBreak.Text = "Refresh on pause/break";
			this.mnuRefreshOnBreak.Click += new System.EventHandler(this.mnuRefreshOnBreak_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgTextHooker);
			this.tabMain.Controls.Add(this.tpgCharacterMappings);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 24);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(857, 515);
			this.tabMain.TabIndex = 3;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tpgTextHooker
			// 
			this.tpgTextHooker.Controls.Add(this.ctrlTextHooker);
			this.tpgTextHooker.Location = new System.Drawing.Point(4, 22);
			this.tpgTextHooker.Name = "tpgTextHooker";
			this.tpgTextHooker.Size = new System.Drawing.Size(849, 489);
			this.tpgTextHooker.TabIndex = 0;
			this.tpgTextHooker.Text = "Text Hooker";
			this.tpgTextHooker.UseVisualStyleBackColor = true;
			// 
			// ctrlTextHooker
			// 
			this.ctrlTextHooker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlTextHooker.Location = new System.Drawing.Point(0, 0);
			this.ctrlTextHooker.Name = "ctrlTextHooker";
			this.ctrlTextHooker.Size = new System.Drawing.Size(849, 489);
			this.ctrlTextHooker.TabIndex = 0;
			// 
			// tpgCharacterMappings
			// 
			this.tpgCharacterMappings.Controls.Add(this.ctrlCharacterMappings);
			this.tpgCharacterMappings.Location = new System.Drawing.Point(4, 22);
			this.tpgCharacterMappings.Name = "tpgCharacterMappings";
			this.tpgCharacterMappings.Size = new System.Drawing.Size(849, 489);
			this.tpgCharacterMappings.TabIndex = 1;
			this.tpgCharacterMappings.Text = "Character Mappings";
			this.tpgCharacterMappings.UseVisualStyleBackColor = true;
			// 
			// ctrlCharacterMappings
			// 
			this.ctrlCharacterMappings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCharacterMappings.Location = new System.Drawing.Point(0, 0);
			this.ctrlCharacterMappings.Name = "ctrlCharacterMappings";
			this.ctrlCharacterMappings.Size = new System.Drawing.Size(849, 489);
			this.ctrlCharacterMappings.TabIndex = 0;
			// 
			// ctrlScanlineCycle
			// 
			this.ctrlScanlineCycle.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ctrlScanlineCycle.Location = new System.Drawing.Point(0, 539);
			this.ctrlScanlineCycle.Name = "ctrlScanlineCycle";
			this.ctrlScanlineCycle.Size = new System.Drawing.Size(857, 28);
			this.ctrlScanlineCycle.TabIndex = 4;
			// 
			// frmTextHooker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(857, 567);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.ctrlScanlineCycle);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmTextHooker";
			this.Text = "Text Hooker";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgTextHooker.ResumeLayout(false);
			this.tpgCharacterMappings.ResumeLayout(false);
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
		private System.Windows.Forms.TabPage tpgTextHooker;
		private System.Windows.Forms.TabPage tpgCharacterMappings;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshOnBreak;
		private Controls.ctrlTextHooker ctrlTextHooker;
		private Controls.ctrlCharacterMappings ctrlCharacterMappings;
		private Controls.ctrlScanlineCycleSelect ctrlScanlineCycle;
	}
}