namespace Mesen.GUI.Debugger
{
	partial class frmEventViewer
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
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgPpuView = new System.Windows.Forms.TabPage();
			this.ctrlEventViewerPpuView = new Mesen.GUI.Debugger.Controls.ctrlEventViewerPpuView();
			this.grpShow = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkBreakpoints = new System.Windows.Forms.CheckBox();
			this.chkShowMapperRegisterReads = new System.Windows.Forms.CheckBox();
			this.chkShowNmi = new System.Windows.Forms.CheckBox();
			this.chkShowIrq = new System.Windows.Forms.CheckBox();
			this.chkShowPpuRegisterReads = new System.Windows.Forms.CheckBox();
			this.chkShowPpuRegisterWrites = new System.Windows.Forms.CheckBox();
			this.chkShowMapperRegisterWrites = new System.Windows.Forms.CheckBox();
			this.chkShowSpriteZero = new System.Windows.Forms.CheckBox();
			this.chkShowPreviousFrameEvents = new System.Windows.Forms.CheckBox();
			this.tpgListView = new System.Windows.Forms.TabPage();
			this.ctrlEventViewerListView = new Mesen.GUI.Debugger.Controls.ctrlEventViewerListView();
			this.menuStrip1 = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuConfigureColors = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.chkToggleZoom = new System.Windows.Forms.CheckBox();
			this.btnToggleView = new System.Windows.Forms.Button();
			this.tabMain.SuspendLayout();
			this.tpgPpuView.SuspendLayout();
			this.grpShow.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tpgListView.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgPpuView);
			this.tabMain.Controls.Add(this.tpgListView);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 24);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(929, 564);
			this.tabMain.TabIndex = 0;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tpgPpuView
			// 
			this.tpgPpuView.Controls.Add(this.grpShow);
			this.tpgPpuView.Controls.Add(this.ctrlEventViewerPpuView);
			this.tpgPpuView.Location = new System.Drawing.Point(4, 22);
			this.tpgPpuView.Name = "tpgPpuView";
			this.tpgPpuView.Padding = new System.Windows.Forms.Padding(3);
			this.tpgPpuView.Size = new System.Drawing.Size(921, 538);
			this.tpgPpuView.TabIndex = 0;
			this.tpgPpuView.Text = "PPU View";
			this.tpgPpuView.UseVisualStyleBackColor = true;
			// 
			// ctrlEventViewerPpuView
			// 
			this.ctrlEventViewerPpuView.Location = new System.Drawing.Point(0, 0);
			this.ctrlEventViewerPpuView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.ctrlEventViewerPpuView.Name = "ctrlEventViewerPpuView";
			this.ctrlEventViewerPpuView.Size = new System.Drawing.Size(685, 532);
			this.ctrlEventViewerPpuView.TabIndex = 0;
			this.ctrlEventViewerPpuView.OnPictureResized += new System.EventHandler(this.ctrlEventViewerPpuView_OnPictureResized);
			// 
			// grpShow
			// 
			this.grpShow.Controls.Add(this.tableLayoutPanel2);
			this.grpShow.Dock = System.Windows.Forms.DockStyle.Right;
			this.grpShow.Location = new System.Drawing.Point(694, 3);
			this.grpShow.Name = "grpShow";
			this.grpShow.Size = new System.Drawing.Size(224, 532);
			this.grpShow.TabIndex = 3;
			this.grpShow.TabStop = false;
			this.grpShow.Text = "Show...";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.chkBreakpoints, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.chkShowMapperRegisterReads, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkShowNmi, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.chkShowIrq, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuRegisterReads, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPpuRegisterWrites, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowMapperRegisterWrites, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkShowSpriteZero, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPreviousFrameEvents, 0, 8);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 9;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(218, 513);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// chkBreakpoints
			// 
			this.chkBreakpoints.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkBreakpoints, 2);
			this.chkBreakpoints.Location = new System.Drawing.Point(3, 164);
			this.chkBreakpoints.Name = "chkBreakpoints";
			this.chkBreakpoints.Size = new System.Drawing.Size(121, 17);
			this.chkBreakpoints.TabIndex = 7;
			this.chkBreakpoints.Text = "Marked Breakpoints";
			this.chkBreakpoints.UseVisualStyleBackColor = true;
			this.chkBreakpoints.Click += new System.EventHandler(this.chkBreakpoints_Click);
			// 
			// chkShowMapperRegisterReads
			// 
			this.chkShowMapperRegisterReads.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowMapperRegisterReads, 2);
			this.chkShowMapperRegisterReads.Location = new System.Drawing.Point(3, 72);
			this.chkShowMapperRegisterReads.Name = "chkShowMapperRegisterReads";
			this.chkShowMapperRegisterReads.Size = new System.Drawing.Size(138, 17);
			this.chkShowMapperRegisterReads.TabIndex = 5;
			this.chkShowMapperRegisterReads.Text = "Mapper Register Reads";
			this.chkShowMapperRegisterReads.UseVisualStyleBackColor = true;
			this.chkShowMapperRegisterReads.Click += new System.EventHandler(this.chkShowMapperRegisterReads_Click);
			// 
			// chkShowNmi
			// 
			this.chkShowNmi.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowNmi, 2);
			this.chkShowNmi.Location = new System.Drawing.Point(3, 118);
			this.chkShowNmi.Name = "chkShowNmi";
			this.chkShowNmi.Size = new System.Drawing.Size(46, 17);
			this.chkShowNmi.TabIndex = 3;
			this.chkShowNmi.Text = "NMI";
			this.chkShowNmi.UseVisualStyleBackColor = true;
			this.chkShowNmi.Click += new System.EventHandler(this.chkShowNmi_Click);
			// 
			// chkShowIrq
			// 
			this.chkShowIrq.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowIrq, 2);
			this.chkShowIrq.Location = new System.Drawing.Point(3, 95);
			this.chkShowIrq.Name = "chkShowIrq";
			this.chkShowIrq.Size = new System.Drawing.Size(45, 17);
			this.chkShowIrq.TabIndex = 2;
			this.chkShowIrq.Text = "IRQ";
			this.chkShowIrq.UseVisualStyleBackColor = true;
			this.chkShowIrq.Click += new System.EventHandler(this.chkShowIrq_Click);
			// 
			// chkShowPpuRegisterReads
			// 
			this.chkShowPpuRegisterReads.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowPpuRegisterReads, 2);
			this.chkShowPpuRegisterReads.Location = new System.Drawing.Point(3, 26);
			this.chkShowPpuRegisterReads.Name = "chkShowPpuRegisterReads";
			this.chkShowPpuRegisterReads.Size = new System.Drawing.Size(124, 17);
			this.chkShowPpuRegisterReads.TabIndex = 1;
			this.chkShowPpuRegisterReads.Text = "PPU Register Reads";
			this.chkShowPpuRegisterReads.UseVisualStyleBackColor = true;
			this.chkShowPpuRegisterReads.Click += new System.EventHandler(this.chkShowPpuRegisterReads_Click);
			// 
			// chkShowPpuRegisterWrites
			// 
			this.chkShowPpuRegisterWrites.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowPpuRegisterWrites, 2);
			this.chkShowPpuRegisterWrites.Location = new System.Drawing.Point(3, 3);
			this.chkShowPpuRegisterWrites.Name = "chkShowPpuRegisterWrites";
			this.chkShowPpuRegisterWrites.Size = new System.Drawing.Size(123, 17);
			this.chkShowPpuRegisterWrites.TabIndex = 0;
			this.chkShowPpuRegisterWrites.Text = "PPU Register Writes";
			this.chkShowPpuRegisterWrites.UseVisualStyleBackColor = true;
			this.chkShowPpuRegisterWrites.Click += new System.EventHandler(this.chkShowPpuRegisterWrites_Click);
			// 
			// chkShowMapperRegisterWrites
			// 
			this.chkShowMapperRegisterWrites.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowMapperRegisterWrites, 2);
			this.chkShowMapperRegisterWrites.Location = new System.Drawing.Point(3, 49);
			this.chkShowMapperRegisterWrites.Name = "chkShowMapperRegisterWrites";
			this.chkShowMapperRegisterWrites.Size = new System.Drawing.Size(137, 17);
			this.chkShowMapperRegisterWrites.TabIndex = 6;
			this.chkShowMapperRegisterWrites.Text = "Mapper Register Writes";
			this.chkShowMapperRegisterWrites.UseVisualStyleBackColor = true;
			this.chkShowMapperRegisterWrites.Click += new System.EventHandler(this.chkShowMapperRegisterWrites_Click);
			// 
			// chkShowSpriteZero
			// 
			this.chkShowSpriteZero.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowSpriteZero, 2);
			this.chkShowSpriteZero.Location = new System.Drawing.Point(3, 141);
			this.chkShowSpriteZero.Name = "chkShowSpriteZero";
			this.chkShowSpriteZero.Size = new System.Drawing.Size(78, 17);
			this.chkShowSpriteZero.TabIndex = 4;
			this.chkShowSpriteZero.Text = "Sprite 0 Hit";
			this.chkShowSpriteZero.UseVisualStyleBackColor = true;
			this.chkShowSpriteZero.Click += new System.EventHandler(this.chkShowSpriteZero_Click);
			// 
			// chkShowPreviousFrameEvents
			// 
			this.chkShowPreviousFrameEvents.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowPreviousFrameEvents, 2);
			this.chkShowPreviousFrameEvents.Location = new System.Drawing.Point(3, 187);
			this.chkShowPreviousFrameEvents.Name = "chkShowPreviousFrameEvents";
			this.chkShowPreviousFrameEvents.Size = new System.Drawing.Size(167, 17);
			this.chkShowPreviousFrameEvents.TabIndex = 8;
			this.chkShowPreviousFrameEvents.Text = "Show previous frame\'s events";
			this.chkShowPreviousFrameEvents.UseVisualStyleBackColor = true;
			this.chkShowPreviousFrameEvents.Click += new System.EventHandler(this.chkShowPreviousFrameEvents_Click);
			// 
			// tpgListView
			// 
			this.tpgListView.Controls.Add(this.ctrlEventViewerListView);
			this.tpgListView.Location = new System.Drawing.Point(4, 22);
			this.tpgListView.Name = "tpgListView";
			this.tpgListView.Padding = new System.Windows.Forms.Padding(3);
			this.tpgListView.Size = new System.Drawing.Size(921, 538);
			this.tpgListView.TabIndex = 1;
			this.tpgListView.Text = "List View";
			this.tpgListView.UseVisualStyleBackColor = true;
			// 
			// ctrlEventViewerListView
			// 
			this.ctrlEventViewerListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEventViewerListView.Location = new System.Drawing.Point(3, 3);
			this.ctrlEventViewerListView.Name = "ctrlEventViewerListView";
			this.ctrlEventViewerListView.Size = new System.Drawing.Size(915, 532);
			this.ctrlEventViewerListView.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(929, 24);
			this.menuStrip1.TabIndex = 3;
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
            this.mnuConfigureColors,
            this.toolStripMenuItem1,
            this.mnuRefreshOnBreak});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// mnuConfigureColors
			// 
			this.mnuConfigureColors.Image = global::Mesen.GUI.Properties.Resources.PipetteSmall;
			this.mnuConfigureColors.Name = "mnuConfigureColors";
			this.mnuConfigureColors.Size = new System.Drawing.Size(198, 22);
			this.mnuConfigureColors.Text = "Configure Colors";
			this.mnuConfigureColors.Click += new System.EventHandler(this.mnuConfigureColors_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuRefreshOnBreak
			// 
			this.mnuRefreshOnBreak.CheckOnClick = true;
			this.mnuRefreshOnBreak.Name = "mnuRefreshOnBreak";
			this.mnuRefreshOnBreak.Size = new System.Drawing.Size(198, 22);
			this.mnuRefreshOnBreak.Text = "Refresh on pause/break";
			this.mnuRefreshOnBreak.Click += new System.EventHandler(this.mnuRefreshOnBreak_Click);
			// 
			// chkToggleZoom
			// 
			this.chkToggleZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkToggleZoom.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkToggleZoom.AutoCheck = false;
			this.chkToggleZoom.Image = global::Mesen.GUI.Properties.Resources.Zoom2x;
			this.chkToggleZoom.Location = new System.Drawing.Point(867, 1);
			this.chkToggleZoom.Name = "chkToggleZoom";
			this.chkToggleZoom.Size = new System.Drawing.Size(27, 22);
			this.chkToggleZoom.TabIndex = 8;
			this.chkToggleZoom.UseVisualStyleBackColor = true;
			this.chkToggleZoom.Click += new System.EventHandler(this.chkToggleZoom_Click);
			// 
			// btnToggleView
			// 
			this.btnToggleView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnToggleView.Image = global::Mesen.GUI.Properties.Resources.Collapse;
			this.btnToggleView.Location = new System.Drawing.Point(900, 1);
			this.btnToggleView.Name = "btnToggleView";
			this.btnToggleView.Size = new System.Drawing.Size(27, 22);
			this.btnToggleView.TabIndex = 7;
			this.btnToggleView.UseVisualStyleBackColor = true;
			this.btnToggleView.Click += new System.EventHandler(this.btnToggleView_Click);
			// 
			// frmEventViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(929, 588);
			this.Controls.Add(this.chkToggleZoom);
			this.Controls.Add(this.btnToggleView);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmEventViewer";
			this.Text = "Event Viewer";
			this.tabMain.ResumeLayout(false);
			this.tpgPpuView.ResumeLayout(false);
			this.grpShow.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tpgListView.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgPpuView;
		private Controls.ctrlEventViewerPpuView ctrlEventViewerPpuView;
		private Mesen.GUI.Controls.ctrlMesenMenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkShowMapperRegisterReads;
		private System.Windows.Forms.CheckBox chkShowNmi;
		private System.Windows.Forms.CheckBox chkShowIrq;
		private System.Windows.Forms.CheckBox chkShowPpuRegisterReads;
		private System.Windows.Forms.CheckBox chkShowPpuRegisterWrites;
		private System.Windows.Forms.CheckBox chkShowMapperRegisterWrites;
		private System.Windows.Forms.CheckBox chkShowSpriteZero;
		private System.Windows.Forms.CheckBox chkBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem mnuConfigureColors;
		private System.Windows.Forms.GroupBox grpShow;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshOnBreak;
		private System.Windows.Forms.TabPage tpgListView;
		private Controls.ctrlEventViewerListView ctrlEventViewerListView;
		private System.Windows.Forms.CheckBox chkShowPreviousFrameEvents;
		private System.Windows.Forms.CheckBox chkToggleZoom;
		private System.Windows.Forms.Button btnToggleView;
	}
}