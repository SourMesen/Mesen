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
			this.grpShow = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPpuWrites = new System.Windows.Forms.Label();
			this.chkShowMapperRegisterWrites = new System.Windows.Forms.CheckBox();
			this.chkShowPreviousFrameEvents = new System.Windows.Forms.CheckBox();
			this.chkWrite2000 = new System.Windows.Forms.CheckBox();
			this.picWrite2000 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.chkWrite2001 = new System.Windows.Forms.CheckBox();
			this.chkWrite2003 = new System.Windows.Forms.CheckBox();
			this.chkWrite2004 = new System.Windows.Forms.CheckBox();
			this.chkWrite2005 = new System.Windows.Forms.CheckBox();
			this.chkWrite2006 = new System.Windows.Forms.CheckBox();
			this.chkWrite2007 = new System.Windows.Forms.CheckBox();
			this.picWrite2001 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picWrite2003 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.lblPpuReads = new System.Windows.Forms.Label();
			this.chkRead2002 = new System.Windows.Forms.CheckBox();
			this.chkRead2004 = new System.Windows.Forms.CheckBox();
			this.chkRead2007 = new System.Windows.Forms.CheckBox();
			this.picRead2002 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picRead2004 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picRead2007 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picWrite2004 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picWrite2005 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picWrite2007 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picWrite2006 = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.label1 = new System.Windows.Forms.Label();
			this.chkShowMapperRegisterReads = new System.Windows.Forms.CheckBox();
			this.picMapperRead = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picMapperWrite = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.chkShowIrq = new System.Windows.Forms.CheckBox();
			this.chkShowNmi = new System.Windows.Forms.CheckBox();
			this.chkShowSpriteZero = new System.Windows.Forms.CheckBox();
			this.chkBreakpoints = new System.Windows.Forms.CheckBox();
			this.chkShowDmcDmaRead = new System.Windows.Forms.CheckBox();
			this.picIrq = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picNmi = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picSpriteZeroHit = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picBreakpoint = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.picDmcDmaRead = new Mesen.GUI.Debugger.ctrlColorPicker();
			this.ctrlEventViewerPpuView = new Mesen.GUI.Debugger.Controls.ctrlEventViewerPpuView();
			this.tpgListView = new System.Windows.Forms.TabPage();
			this.ctrlEventViewerListView = new Mesen.GUI.Debugger.Controls.ctrlEventViewerListView();
			this.menuStrip1 = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefreshOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuResetColors = new System.Windows.Forms.ToolStripMenuItem();
			this.chkToggleZoom = new System.Windows.Forms.CheckBox();
			this.btnToggleView = new System.Windows.Forms.Button();
			this.tabMain.SuspendLayout();
			this.tpgPpuView.SuspendLayout();
			this.grpShow.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2000)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2001)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2003)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2002)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2004)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2007)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2004)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2005)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2007)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2006)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picMapperRead)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picMapperWrite)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picIrq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picNmi)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picSpriteZeroHit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBreakpoint)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picDmcDmaRead)).BeginInit();
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
			this.tabMain.Size = new System.Drawing.Size(886, 564);
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
			this.tpgPpuView.Size = new System.Drawing.Size(878, 538);
			this.tpgPpuView.TabIndex = 0;
			this.tpgPpuView.Text = "PPU View";
			this.tpgPpuView.UseVisualStyleBackColor = true;
			// 
			// grpShow
			// 
			this.grpShow.Controls.Add(this.tableLayoutPanel2);
			this.grpShow.Dock = System.Windows.Forms.DockStyle.Right;
			this.grpShow.Location = new System.Drawing.Point(689, 3);
			this.grpShow.Name = "grpShow";
			this.grpShow.Size = new System.Drawing.Size(186, 532);
			this.grpShow.TabIndex = 3;
			this.grpShow.TabStop = false;
			this.grpShow.Text = "Show...";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel2.Controls.Add(this.lblPpuWrites, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkShowMapperRegisterWrites, 0, 9);
			this.tableLayoutPanel2.Controls.Add(this.chkShowPreviousFrameEvents, 0, 19);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2000, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2000, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2001, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2003, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2004, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2005, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2006, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkWrite2007, 2, 3);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2001, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2003, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblPpuReads, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.chkRead2002, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.chkRead2004, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.chkRead2007, 2, 6);
			this.tableLayoutPanel2.Controls.Add(this.picRead2002, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.picRead2004, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.picRead2007, 3, 6);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2004, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2005, 3, 1);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2007, 3, 3);
			this.tableLayoutPanel2.Controls.Add(this.picWrite2006, 3, 2);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.chkShowMapperRegisterReads, 0, 10);
			this.tableLayoutPanel2.Controls.Add(this.picMapperRead, 3, 10);
			this.tableLayoutPanel2.Controls.Add(this.picMapperWrite, 3, 9);
			this.tableLayoutPanel2.Controls.Add(this.chkShowIrq, 0, 12);
			this.tableLayoutPanel2.Controls.Add(this.chkShowNmi, 0, 13);
			this.tableLayoutPanel2.Controls.Add(this.chkShowSpriteZero, 0, 14);
			this.tableLayoutPanel2.Controls.Add(this.chkBreakpoints, 0, 18);
			this.tableLayoutPanel2.Controls.Add(this.chkShowDmcDmaRead, 0, 16);
			this.tableLayoutPanel2.Controls.Add(this.picIrq, 3, 12);
			this.tableLayoutPanel2.Controls.Add(this.picNmi, 3, 13);
			this.tableLayoutPanel2.Controls.Add(this.picSpriteZeroHit, 3, 14);
			this.tableLayoutPanel2.Controls.Add(this.picBreakpoint, 3, 18);
			this.tableLayoutPanel2.Controls.Add(this.picDmcDmaRead, 3, 16);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 20;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(180, 513);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// lblPpuWrites
			// 
			this.lblPpuWrites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPpuWrites.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.lblPpuWrites, 4);
			this.lblPpuWrites.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblPpuWrites.Location = new System.Drawing.Point(0, 5);
			this.lblPpuWrites.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
			this.lblPpuWrites.Name = "lblPpuWrites";
			this.lblPpuWrites.Size = new System.Drawing.Size(104, 13);
			this.lblPpuWrites.TabIndex = 36;
			this.lblPpuWrites.Text = "PPU Register Writes";
			// 
			// chkShowMapperRegisterWrites
			// 
			this.chkShowMapperRegisterWrites.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowMapperRegisterWrites, 3);
			this.chkShowMapperRegisterWrites.Location = new System.Drawing.Point(3, 221);
			this.chkShowMapperRegisterWrites.Name = "chkShowMapperRegisterWrites";
			this.chkShowMapperRegisterWrites.Size = new System.Drawing.Size(137, 17);
			this.chkShowMapperRegisterWrites.TabIndex = 6;
			this.chkShowMapperRegisterWrites.Text = "Mapper Register Writes";
			this.chkShowMapperRegisterWrites.UseVisualStyleBackColor = true;
			this.chkShowMapperRegisterWrites.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkShowPreviousFrameEvents
			// 
			this.chkShowPreviousFrameEvents.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowPreviousFrameEvents, 4);
			this.chkShowPreviousFrameEvents.Location = new System.Drawing.Point(3, 382);
			this.chkShowPreviousFrameEvents.Name = "chkShowPreviousFrameEvents";
			this.chkShowPreviousFrameEvents.Size = new System.Drawing.Size(167, 17);
			this.chkShowPreviousFrameEvents.TabIndex = 8;
			this.chkShowPreviousFrameEvents.Text = "Show previous frame\'s events";
			this.chkShowPreviousFrameEvents.UseVisualStyleBackColor = true;
			this.chkShowPreviousFrameEvents.Click += new System.EventHandler(this.chkShowPreviousFrameEvents_Click);
			// 
			// chkWrite2000
			// 
			this.chkWrite2000.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2000.AutoSize = true;
			this.chkWrite2000.Location = new System.Drawing.Point(3, 23);
			this.chkWrite2000.Name = "chkWrite2000";
			this.chkWrite2000.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2000.TabIndex = 37;
			this.chkWrite2000.Text = "$2000";
			this.chkWrite2000.UseVisualStyleBackColor = true;
			this.chkWrite2000.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// picWrite2000
			// 
			this.picWrite2000.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2000.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2000.Location = new System.Drawing.Point(65, 24);
			this.picWrite2000.Name = "picWrite2000";
			this.picWrite2000.Size = new System.Drawing.Size(14, 14);
			this.picWrite2000.TabIndex = 38;
			this.picWrite2000.TabStop = false;
			this.picWrite2000.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// chkWrite2001
			// 
			this.chkWrite2001.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2001.AutoSize = true;
			this.chkWrite2001.Location = new System.Drawing.Point(3, 46);
			this.chkWrite2001.Name = "chkWrite2001";
			this.chkWrite2001.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2001.TabIndex = 39;
			this.chkWrite2001.Text = "$2001";
			this.chkWrite2001.UseVisualStyleBackColor = true;
			this.chkWrite2001.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkWrite2003
			// 
			this.chkWrite2003.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2003.AutoSize = true;
			this.chkWrite2003.Location = new System.Drawing.Point(3, 69);
			this.chkWrite2003.Name = "chkWrite2003";
			this.chkWrite2003.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2003.TabIndex = 40;
			this.chkWrite2003.Text = "$2003";
			this.chkWrite2003.UseVisualStyleBackColor = true;
			this.chkWrite2003.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkWrite2004
			// 
			this.chkWrite2004.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2004.AutoSize = true;
			this.chkWrite2004.Location = new System.Drawing.Point(3, 92);
			this.chkWrite2004.Name = "chkWrite2004";
			this.chkWrite2004.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2004.TabIndex = 41;
			this.chkWrite2004.Text = "$2004";
			this.chkWrite2004.UseVisualStyleBackColor = true;
			this.chkWrite2004.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkWrite2005
			// 
			this.chkWrite2005.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2005.AutoSize = true;
			this.chkWrite2005.Location = new System.Drawing.Point(96, 23);
			this.chkWrite2005.Name = "chkWrite2005";
			this.chkWrite2005.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2005.TabIndex = 42;
			this.chkWrite2005.Text = "$2005";
			this.chkWrite2005.UseVisualStyleBackColor = true;
			this.chkWrite2005.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkWrite2006
			// 
			this.chkWrite2006.AutoSize = true;
			this.chkWrite2006.Location = new System.Drawing.Point(96, 46);
			this.chkWrite2006.Name = "chkWrite2006";
			this.chkWrite2006.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2006.TabIndex = 43;
			this.chkWrite2006.Text = "$2006";
			this.chkWrite2006.UseVisualStyleBackColor = true;
			this.chkWrite2006.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkWrite2007
			// 
			this.chkWrite2007.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkWrite2007.AutoSize = true;
			this.chkWrite2007.Location = new System.Drawing.Point(96, 69);
			this.chkWrite2007.Name = "chkWrite2007";
			this.chkWrite2007.Size = new System.Drawing.Size(56, 17);
			this.chkWrite2007.TabIndex = 44;
			this.chkWrite2007.Text = "$2007";
			this.chkWrite2007.UseVisualStyleBackColor = true;
			this.chkWrite2007.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// picWrite2001
			// 
			this.picWrite2001.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2001.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2001.Location = new System.Drawing.Point(65, 47);
			this.picWrite2001.Name = "picWrite2001";
			this.picWrite2001.Size = new System.Drawing.Size(14, 14);
			this.picWrite2001.TabIndex = 45;
			this.picWrite2001.TabStop = false;
			this.picWrite2001.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picWrite2003
			// 
			this.picWrite2003.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2003.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2003.Location = new System.Drawing.Point(65, 70);
			this.picWrite2003.Name = "picWrite2003";
			this.picWrite2003.Size = new System.Drawing.Size(14, 14);
			this.picWrite2003.TabIndex = 46;
			this.picWrite2003.TabStop = false;
			this.picWrite2003.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// lblPpuReads
			// 
			this.lblPpuReads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPpuReads.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.lblPpuReads, 4);
			this.lblPpuReads.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblPpuReads.Location = new System.Drawing.Point(0, 127);
			this.lblPpuReads.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
			this.lblPpuReads.Name = "lblPpuReads";
			this.lblPpuReads.Size = new System.Drawing.Size(105, 13);
			this.lblPpuReads.TabIndex = 51;
			this.lblPpuReads.Text = "PPU Register Reads";
			// 
			// chkRead2002
			// 
			this.chkRead2002.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkRead2002.AutoSize = true;
			this.chkRead2002.Location = new System.Drawing.Point(3, 145);
			this.chkRead2002.Name = "chkRead2002";
			this.chkRead2002.Size = new System.Drawing.Size(56, 17);
			this.chkRead2002.TabIndex = 52;
			this.chkRead2002.Text = "$2002";
			this.chkRead2002.UseVisualStyleBackColor = true;
			this.chkRead2002.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkRead2004
			// 
			this.chkRead2004.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkRead2004.AutoSize = true;
			this.chkRead2004.Location = new System.Drawing.Point(3, 168);
			this.chkRead2004.Name = "chkRead2004";
			this.chkRead2004.Size = new System.Drawing.Size(56, 17);
			this.chkRead2004.TabIndex = 53;
			this.chkRead2004.Text = "$2004";
			this.chkRead2004.UseVisualStyleBackColor = true;
			this.chkRead2004.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkRead2007
			// 
			this.chkRead2007.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkRead2007.AutoSize = true;
			this.chkRead2007.Location = new System.Drawing.Point(96, 145);
			this.chkRead2007.Name = "chkRead2007";
			this.chkRead2007.Size = new System.Drawing.Size(56, 17);
			this.chkRead2007.TabIndex = 54;
			this.chkRead2007.Text = "$2007";
			this.chkRead2007.UseVisualStyleBackColor = true;
			this.chkRead2007.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// picRead2002
			// 
			this.picRead2002.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picRead2002.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picRead2002.Location = new System.Drawing.Point(65, 146);
			this.picRead2002.Name = "picRead2002";
			this.picRead2002.Size = new System.Drawing.Size(14, 14);
			this.picRead2002.TabIndex = 55;
			this.picRead2002.TabStop = false;
			this.picRead2002.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picRead2004
			// 
			this.picRead2004.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picRead2004.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picRead2004.Location = new System.Drawing.Point(65, 169);
			this.picRead2004.Name = "picRead2004";
			this.picRead2004.Size = new System.Drawing.Size(14, 14);
			this.picRead2004.TabIndex = 56;
			this.picRead2004.TabStop = false;
			this.picRead2004.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picRead2007
			// 
			this.picRead2007.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picRead2007.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picRead2007.Location = new System.Drawing.Point(158, 146);
			this.picRead2007.Name = "picRead2007";
			this.picRead2007.Size = new System.Drawing.Size(14, 14);
			this.picRead2007.TabIndex = 57;
			this.picRead2007.TabStop = false;
			this.picRead2007.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picWrite2004
			// 
			this.picWrite2004.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2004.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2004.Location = new System.Drawing.Point(65, 93);
			this.picWrite2004.Name = "picWrite2004";
			this.picWrite2004.Size = new System.Drawing.Size(14, 14);
			this.picWrite2004.TabIndex = 47;
			this.picWrite2004.TabStop = false;
			this.picWrite2004.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picWrite2005
			// 
			this.picWrite2005.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2005.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2005.Location = new System.Drawing.Point(158, 24);
			this.picWrite2005.Name = "picWrite2005";
			this.picWrite2005.Size = new System.Drawing.Size(14, 14);
			this.picWrite2005.TabIndex = 48;
			this.picWrite2005.TabStop = false;
			this.picWrite2005.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picWrite2007
			// 
			this.picWrite2007.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2007.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2007.Location = new System.Drawing.Point(158, 70);
			this.picWrite2007.Name = "picWrite2007";
			this.picWrite2007.Size = new System.Drawing.Size(14, 14);
			this.picWrite2007.TabIndex = 50;
			this.picWrite2007.TabStop = false;
			this.picWrite2007.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picWrite2006
			// 
			this.picWrite2006.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picWrite2006.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite2006.Location = new System.Drawing.Point(158, 47);
			this.picWrite2006.Name = "picWrite2006";
			this.picWrite2006.Size = new System.Drawing.Size(14, 14);
			this.picWrite2006.TabIndex = 49;
			this.picWrite2006.TabStop = false;
			this.picWrite2006.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.label1, 4);
			this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label1.Location = new System.Drawing.Point(0, 203);
			this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 58;
			this.label1.Text = "Others";
			// 
			// chkShowMapperRegisterReads
			// 
			this.chkShowMapperRegisterReads.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowMapperRegisterReads, 3);
			this.chkShowMapperRegisterReads.Location = new System.Drawing.Point(3, 244);
			this.chkShowMapperRegisterReads.Name = "chkShowMapperRegisterReads";
			this.chkShowMapperRegisterReads.Size = new System.Drawing.Size(138, 17);
			this.chkShowMapperRegisterReads.TabIndex = 5;
			this.chkShowMapperRegisterReads.Text = "Mapper Register Reads";
			this.chkShowMapperRegisterReads.UseVisualStyleBackColor = true;
			this.chkShowMapperRegisterReads.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// picMapperRead
			// 
			this.picMapperRead.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picMapperRead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picMapperRead.Location = new System.Drawing.Point(158, 245);
			this.picMapperRead.Name = "picMapperRead";
			this.picMapperRead.Size = new System.Drawing.Size(14, 14);
			this.picMapperRead.TabIndex = 62;
			this.picMapperRead.TabStop = false;
			this.picMapperRead.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picMapperWrite
			// 
			this.picMapperWrite.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picMapperWrite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picMapperWrite.Location = new System.Drawing.Point(158, 222);
			this.picMapperWrite.Name = "picMapperWrite";
			this.picMapperWrite.Size = new System.Drawing.Size(14, 14);
			this.picMapperWrite.TabIndex = 59;
			this.picMapperWrite.TabStop = false;
			this.picMapperWrite.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// chkShowIrq
			// 
			this.chkShowIrq.AutoSize = true;
			this.chkShowIrq.Location = new System.Drawing.Point(3, 267);
			this.chkShowIrq.Name = "chkShowIrq";
			this.chkShowIrq.Size = new System.Drawing.Size(45, 17);
			this.chkShowIrq.TabIndex = 2;
			this.chkShowIrq.Text = "IRQ";
			this.chkShowIrq.UseVisualStyleBackColor = true;
			this.chkShowIrq.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkShowNmi
			// 
			this.chkShowNmi.AutoSize = true;
			this.chkShowNmi.Location = new System.Drawing.Point(3, 290);
			this.chkShowNmi.Name = "chkShowNmi";
			this.chkShowNmi.Size = new System.Drawing.Size(46, 17);
			this.chkShowNmi.TabIndex = 3;
			this.chkShowNmi.Text = "NMI";
			this.chkShowNmi.UseVisualStyleBackColor = true;
			this.chkShowNmi.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkShowSpriteZero
			// 
			this.chkShowSpriteZero.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowSpriteZero, 2);
			this.chkShowSpriteZero.Location = new System.Drawing.Point(3, 313);
			this.chkShowSpriteZero.Name = "chkShowSpriteZero";
			this.chkShowSpriteZero.Size = new System.Drawing.Size(78, 17);
			this.chkShowSpriteZero.TabIndex = 4;
			this.chkShowSpriteZero.Text = "Sprite 0 Hit";
			this.chkShowSpriteZero.UseVisualStyleBackColor = true;
			this.chkShowSpriteZero.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkBreakpoints
			// 
			this.chkBreakpoints.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkBreakpoints, 3);
			this.chkBreakpoints.Location = new System.Drawing.Point(3, 359);
			this.chkBreakpoints.Name = "chkBreakpoints";
			this.chkBreakpoints.Size = new System.Drawing.Size(121, 17);
			this.chkBreakpoints.TabIndex = 7;
			this.chkBreakpoints.Text = "Marked Breakpoints";
			this.chkBreakpoints.UseVisualStyleBackColor = true;
			this.chkBreakpoints.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// chkShowDmcDmaRead
			// 
			this.chkShowDmcDmaRead.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkShowDmcDmaRead, 3);
			this.chkShowDmcDmaRead.Location = new System.Drawing.Point(3, 336);
			this.chkShowDmcDmaRead.Name = "chkShowDmcDmaRead";
			this.chkShowDmcDmaRead.Size = new System.Drawing.Size(106, 17);
			this.chkShowDmcDmaRead.TabIndex = 65;
			this.chkShowDmcDmaRead.Text = "DMC DMA Read";
			this.chkShowDmcDmaRead.UseVisualStyleBackColor = true;
			this.chkShowDmcDmaRead.Click += new System.EventHandler(this.chkShowHide_Click);
			// 
			// picIrq
			// 
			this.picIrq.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picIrq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picIrq.Location = new System.Drawing.Point(158, 268);
			this.picIrq.Name = "picIrq";
			this.picIrq.Size = new System.Drawing.Size(14, 14);
			this.picIrq.TabIndex = 60;
			this.picIrq.TabStop = false;
			this.picIrq.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picNmi
			// 
			this.picNmi.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picNmi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picNmi.Location = new System.Drawing.Point(158, 291);
			this.picNmi.Name = "picNmi";
			this.picNmi.Size = new System.Drawing.Size(14, 14);
			this.picNmi.TabIndex = 63;
			this.picNmi.TabStop = false;
			this.picNmi.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picSpriteZeroHit
			// 
			this.picSpriteZeroHit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picSpriteZeroHit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picSpriteZeroHit.Location = new System.Drawing.Point(158, 314);
			this.picSpriteZeroHit.Name = "picSpriteZeroHit";
			this.picSpriteZeroHit.Size = new System.Drawing.Size(14, 14);
			this.picSpriteZeroHit.TabIndex = 61;
			this.picSpriteZeroHit.TabStop = false;
			this.picSpriteZeroHit.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picBreakpoint
			// 
			this.picBreakpoint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picBreakpoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picBreakpoint.Location = new System.Drawing.Point(158, 360);
			this.picBreakpoint.Name = "picBreakpoint";
			this.picBreakpoint.Size = new System.Drawing.Size(14, 14);
			this.picBreakpoint.TabIndex = 64;
			this.picBreakpoint.TabStop = false;
			this.picBreakpoint.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// picDmcDmaRead
			// 
			this.picDmcDmaRead.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picDmcDmaRead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picDmcDmaRead.Location = new System.Drawing.Point(158, 337);
			this.picDmcDmaRead.Name = "picDmcDmaRead";
			this.picDmcDmaRead.Size = new System.Drawing.Size(14, 14);
			this.picDmcDmaRead.TabIndex = 66;
			this.picDmcDmaRead.TabStop = false;
			this.picDmcDmaRead.BackColorChanged += new System.EventHandler(this.picColor_BackColorChanged);
			// 
			// ctrlEventViewerPpuView
			// 
			this.ctrlEventViewerPpuView.Location = new System.Drawing.Point(0, 0);
			this.ctrlEventViewerPpuView.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlEventViewerPpuView.Name = "ctrlEventViewerPpuView";
			this.ctrlEventViewerPpuView.Size = new System.Drawing.Size(685, 532);
			this.ctrlEventViewerPpuView.TabIndex = 0;
			this.ctrlEventViewerPpuView.OnPictureResized += new System.EventHandler(this.ctrlEventViewerPpuView_OnPictureResized);
			// 
			// tpgListView
			// 
			this.tpgListView.Controls.Add(this.ctrlEventViewerListView);
			this.tpgListView.Location = new System.Drawing.Point(4, 22);
			this.tpgListView.Name = "tpgListView";
			this.tpgListView.Padding = new System.Windows.Forms.Padding(3);
			this.tpgListView.Size = new System.Drawing.Size(878, 538);
			this.tpgListView.TabIndex = 1;
			this.tpgListView.Text = "List View";
			this.tpgListView.UseVisualStyleBackColor = true;
			// 
			// ctrlEventViewerListView
			// 
			this.ctrlEventViewerListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEventViewerListView.Location = new System.Drawing.Point(3, 3);
			this.ctrlEventViewerListView.Name = "ctrlEventViewerListView";
			this.ctrlEventViewerListView.Size = new System.Drawing.Size(872, 532);
			this.ctrlEventViewerListView.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(886, 24);
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
            this.mnuRefreshOnBreak,
            this.toolStripMenuItem1,
            this.mnuResetColors});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// mnuRefreshOnBreak
			// 
			this.mnuRefreshOnBreak.CheckOnClick = true;
			this.mnuRefreshOnBreak.Name = "mnuRefreshOnBreak";
			this.mnuRefreshOnBreak.Size = new System.Drawing.Size(198, 22);
			this.mnuRefreshOnBreak.Text = "Refresh on pause/break";
			this.mnuRefreshOnBreak.Click += new System.EventHandler(this.mnuRefreshOnBreak_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
			// 
			// mnuResetColors
			// 
			this.mnuResetColors.Name = "mnuResetColors";
			this.mnuResetColors.Size = new System.Drawing.Size(198, 22);
			this.mnuResetColors.Text = "Reset colors to default";
			this.mnuResetColors.Click += new System.EventHandler(this.mnuResetColors_Click);
			// 
			// chkToggleZoom
			// 
			this.chkToggleZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkToggleZoom.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkToggleZoom.AutoCheck = false;
			this.chkToggleZoom.Image = global::Mesen.GUI.Properties.Resources.Zoom2x;
			this.chkToggleZoom.Location = new System.Drawing.Point(824, 1);
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
			this.btnToggleView.Location = new System.Drawing.Point(857, 1);
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
			this.ClientSize = new System.Drawing.Size(886, 588);
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
			((System.ComponentModel.ISupportInitialize)(this.picWrite2000)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2001)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2003)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2002)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2004)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead2007)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2004)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2005)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2007)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite2006)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picMapperRead)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picMapperWrite)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picIrq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picNmi)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSpriteZeroHit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBreakpoint)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picDmcDmaRead)).EndInit();
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
		private System.Windows.Forms.CheckBox chkShowMapperRegisterWrites;
		private System.Windows.Forms.CheckBox chkShowSpriteZero;
		private System.Windows.Forms.CheckBox chkBreakpoints;
		private System.Windows.Forms.GroupBox grpShow;
		private System.Windows.Forms.ToolStripMenuItem mnuRefreshOnBreak;
		private System.Windows.Forms.TabPage tpgListView;
		private Controls.ctrlEventViewerListView ctrlEventViewerListView;
		private System.Windows.Forms.CheckBox chkShowPreviousFrameEvents;
		private System.Windows.Forms.CheckBox chkToggleZoom;
		private System.Windows.Forms.Button btnToggleView;
		private System.Windows.Forms.Label lblPpuWrites;
		private System.Windows.Forms.CheckBox chkWrite2000;
		private ctrlColorPicker picWrite2000;
		private System.Windows.Forms.CheckBox chkWrite2001;
		private System.Windows.Forms.CheckBox chkWrite2003;
		private System.Windows.Forms.CheckBox chkWrite2004;
		private System.Windows.Forms.CheckBox chkWrite2005;
		private System.Windows.Forms.CheckBox chkWrite2006;
		private System.Windows.Forms.CheckBox chkWrite2007;
		private ctrlColorPicker picWrite2001;
		private ctrlColorPicker picWrite2003;
		private ctrlColorPicker picWrite2004;
		private ctrlColorPicker picWrite2005;
		private ctrlColorPicker picWrite2006;
		private ctrlColorPicker picWrite2007;
		private System.Windows.Forms.Label lblPpuReads;
		private System.Windows.Forms.CheckBox chkRead2002;
		private System.Windows.Forms.CheckBox chkRead2004;
		private System.Windows.Forms.CheckBox chkRead2007;
		private ctrlColorPicker picRead2002;
		private ctrlColorPicker picRead2004;
		private ctrlColorPicker picRead2007;
		private System.Windows.Forms.Label label1;
		private ctrlColorPicker picMapperWrite;
		private ctrlColorPicker picIrq;
		private ctrlColorPicker picSpriteZeroHit;
		private ctrlColorPicker picMapperRead;
		private ctrlColorPicker picNmi;
		private ctrlColorPicker picBreakpoint;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuResetColors;
		private System.Windows.Forms.CheckBox chkShowDmcDmaRead;
		private ctrlColorPicker picDmcDmaRead;
	}
}