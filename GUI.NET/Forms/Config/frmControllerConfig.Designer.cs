namespace Mesen.GUI.Forms.Config
{
	partial class frmControllerConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmControllerConfig));
			this.ctrlStandardController0 = new Mesen.GUI.Forms.Config.ctrlStandardController();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgSet1 = new System.Windows.Forms.TabPage();
			this.tpgSet2 = new System.Windows.Forms.TabPage();
			this.ctrlStandardController1 = new Mesen.GUI.Forms.Config.ctrlStandardController();
			this.tpgSet3 = new System.Windows.Forms.TabPage();
			this.ctrlStandardController2 = new Mesen.GUI.Forms.Config.ctrlStandardController();
			this.tpgSet4 = new System.Windows.Forms.TabPage();
			this.ctrlStandardController3 = new Mesen.GUI.Forms.Config.ctrlStandardController();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.btnClear = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSelectPreset = new System.Windows.Forms.Button();
			this.trkTurboSpeed = new System.Windows.Forms.TrackBar();
			this.lblTurboSpeed = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblTurboFast = new System.Windows.Forms.Label();
			this.lblSlow = new System.Windows.Forms.Label();
			this.pnlHint = new System.Windows.Forms.Panel();
			this.flpHint = new System.Windows.Forms.FlowLayoutPanel();
			this.picHint = new System.Windows.Forms.PictureBox();
			this.lblHint = new System.Windows.Forms.Label();
			this.mnuStripPreset = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuKeyboard = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWasdLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuArrowLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFceuxLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNestopiaLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuXboxController = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXboxLayout1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXboxLayout2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPs4Controller = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPs4Layout1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPs4Layout2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSnes30Controller = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSnes30Layout1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSnes30Layout2 = new System.Windows.Forms.ToolStripMenuItem();
			this.baseConfigPanel.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgSet1.SuspendLayout();
			this.tpgSet2.SuspendLayout();
			this.tpgSet3.SuspendLayout();
			this.tpgSet4.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkTurboSpeed)).BeginInit();
			this.panel1.SuspendLayout();
			this.pnlHint.SuspendLayout();
			this.flpHint.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).BeginInit();
			this.mnuStripPreset.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.flowLayoutPanel2);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 324);
			this.baseConfigPanel.Size = new System.Drawing.Size(599, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.flowLayoutPanel2, 0);
			// 
			// ctrlStandardController0
			// 
			this.ctrlStandardController0.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlStandardController0.Location = new System.Drawing.Point(0, 0);
			this.ctrlStandardController0.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlStandardController0.Name = "ctrlStandardController0";
			this.ctrlStandardController0.Size = new System.Drawing.Size(585, 209);
			this.ctrlStandardController0.TabIndex = 0;
			this.ctrlStandardController0.OnChange += new System.EventHandler(this.ctrlStandardController_OnChange);
			// 
			// tabMain
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.tabMain, 3);
			this.tabMain.Controls.Add(this.tpgSet1);
			this.tabMain.Controls.Add(this.tpgSet2);
			this.tabMain.Controls.Add(this.tpgSet3);
			this.tabMain.Controls.Add(this.tpgSet4);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.ImageList = this.imageList;
			this.tabMain.Location = new System.Drawing.Point(3, 38);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(593, 236);
			this.tabMain.TabIndex = 3;
			// 
			// tpgSet1
			// 
			this.tpgSet1.Controls.Add(this.ctrlStandardController0);
			this.tpgSet1.Location = new System.Drawing.Point(4, 23);
			this.tpgSet1.Name = "tpgSet1";
			this.tpgSet1.Size = new System.Drawing.Size(585, 209);
			this.tpgSet1.TabIndex = 0;
			this.tpgSet1.Text = "Key Set #1";
			this.tpgSet1.UseVisualStyleBackColor = true;
			// 
			// tpgSet2
			// 
			this.tpgSet2.Controls.Add(this.ctrlStandardController1);
			this.tpgSet2.Location = new System.Drawing.Point(4, 23);
			this.tpgSet2.Name = "tpgSet2";
			this.tpgSet2.Size = new System.Drawing.Size(585, 209);
			this.tpgSet2.TabIndex = 1;
			this.tpgSet2.Text = "Key Set #2";
			this.tpgSet2.UseVisualStyleBackColor = true;
			// 
			// ctrlStandardController1
			// 
			this.ctrlStandardController1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlStandardController1.Location = new System.Drawing.Point(0, 0);
			this.ctrlStandardController1.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlStandardController1.Name = "ctrlStandardController1";
			this.ctrlStandardController1.Size = new System.Drawing.Size(585, 209);
			this.ctrlStandardController1.TabIndex = 1;
			this.ctrlStandardController1.OnChange += new System.EventHandler(this.ctrlStandardController_OnChange);
			// 
			// tpgSet3
			// 
			this.tpgSet3.Controls.Add(this.ctrlStandardController2);
			this.tpgSet3.Location = new System.Drawing.Point(4, 23);
			this.tpgSet3.Name = "tpgSet3";
			this.tpgSet3.Size = new System.Drawing.Size(585, 209);
			this.tpgSet3.TabIndex = 2;
			this.tpgSet3.Text = "Key Set #3";
			this.tpgSet3.UseVisualStyleBackColor = true;
			// 
			// ctrlStandardController2
			// 
			this.ctrlStandardController2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlStandardController2.Location = new System.Drawing.Point(0, 0);
			this.ctrlStandardController2.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlStandardController2.Name = "ctrlStandardController2";
			this.ctrlStandardController2.Size = new System.Drawing.Size(585, 209);
			this.ctrlStandardController2.TabIndex = 1;
			this.ctrlStandardController2.OnChange += new System.EventHandler(this.ctrlStandardController_OnChange);
			// 
			// tpgSet4
			// 
			this.tpgSet4.Controls.Add(this.ctrlStandardController3);
			this.tpgSet4.Location = new System.Drawing.Point(4, 23);
			this.tpgSet4.Name = "tpgSet4";
			this.tpgSet4.Size = new System.Drawing.Size(585, 209);
			this.tpgSet4.TabIndex = 3;
			this.tpgSet4.Text = "Key Set #4";
			this.tpgSet4.UseVisualStyleBackColor = true;
			// 
			// ctrlStandardController3
			// 
			this.ctrlStandardController3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlStandardController3.Location = new System.Drawing.Point(0, 0);
			this.ctrlStandardController3.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlStandardController3.Name = "ctrlStandardController3";
			this.ctrlStandardController3.Size = new System.Drawing.Size(585, 209);
			this.ctrlStandardController3.TabIndex = 1;
			this.ctrlStandardController3.OnChange += new System.EventHandler(this.ctrlStandardController_OnChange);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Keyboard");
			this.imageList.Images.SetKeyName(1, "Controller");
			// 
			// btnClear
			// 
			this.btnClear.AutoSize = true;
			this.btnClear.Location = new System.Drawing.Point(3, 3);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(105, 23);
			this.btnClear.TabIndex = 3;
			this.btnClear.Text = "Clear Key Bindings";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.btnClear);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(396, 29);
			this.flowLayoutPanel2.TabIndex = 5;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.btnSelectPreset, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.trkTurboSpeed, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblTurboSpeed, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.tabMain, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.pnlHint, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(599, 324);
			this.tableLayoutPanel1.TabIndex = 23;
			// 
			// btnSelectPreset
			// 
			this.btnSelectPreset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnSelectPreset.AutoSize = true;
			this.btnSelectPreset.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectPreset.Image")));
			this.btnSelectPreset.Location = new System.Drawing.Point(3, 281);
			this.btnSelectPreset.Name = "btnSelectPreset";
			this.btnSelectPreset.Size = new System.Drawing.Size(105, 23);
			this.btnSelectPreset.TabIndex = 4;
			this.btnSelectPreset.Text = "Select Preset...";
			this.btnSelectPreset.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnSelectPreset.UseVisualStyleBackColor = true;
			this.btnSelectPreset.Click += new System.EventHandler(this.btnSelectPreset_Click);
			// 
			// trkTurboSpeed
			// 
			this.trkTurboSpeed.LargeChange = 2;
			this.trkTurboSpeed.Location = new System.Drawing.Point(479, 280);
			this.trkTurboSpeed.Maximum = 3;
			this.trkTurboSpeed.Name = "trkTurboSpeed";
			this.trkTurboSpeed.Size = new System.Drawing.Size(117, 26);
			this.trkTurboSpeed.TabIndex = 0;
			// 
			// lblTurboSpeed
			// 
			this.lblTurboSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTurboSpeed.AutoSize = true;
			this.lblTurboSpeed.Location = new System.Drawing.Point(401, 286);
			this.lblTurboSpeed.Name = "lblTurboSpeed";
			this.lblTurboSpeed.Size = new System.Drawing.Size(72, 13);
			this.lblTurboSpeed.TabIndex = 1;
			this.lblTurboSpeed.Text = "Turbo Speed:";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblTurboFast);
			this.panel1.Controls.Add(this.lblSlow);
			this.panel1.Location = new System.Drawing.Point(476, 309);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(120, 15);
			this.panel1.TabIndex = 2;
			// 
			// lblTurboFast
			// 
			this.lblTurboFast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTurboFast.Location = new System.Drawing.Point(70, 0);
			this.lblTurboFast.Name = "lblTurboFast";
			this.lblTurboFast.Size = new System.Drawing.Size(47, 15);
			this.lblTurboFast.TabIndex = 1;
			this.lblTurboFast.Text = "Fast";
			this.lblTurboFast.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblSlow
			// 
			this.lblSlow.AutoSize = true;
			this.lblSlow.Location = new System.Drawing.Point(3, 0);
			this.lblSlow.Name = "lblSlow";
			this.lblSlow.Size = new System.Drawing.Size(30, 13);
			this.lblSlow.TabIndex = 0;
			this.lblSlow.Text = "Slow";
			// 
			// pnlHint
			// 
			this.pnlHint.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.pnlHint, 3);
			this.pnlHint.Controls.Add(this.flpHint);
			this.pnlHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlHint.Location = new System.Drawing.Point(3, 3);
			this.pnlHint.Name = "pnlHint";
			this.pnlHint.Size = new System.Drawing.Size(593, 29);
			this.pnlHint.TabIndex = 5;
			// 
			// flpHint
			// 
			this.flpHint.Controls.Add(this.picHint);
			this.flpHint.Controls.Add(this.lblHint);
			this.flpHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpHint.Location = new System.Drawing.Point(0, 0);
			this.flpHint.Margin = new System.Windows.Forms.Padding(0);
			this.flpHint.Name = "flpHint";
			this.flpHint.Size = new System.Drawing.Size(591, 27);
			this.flpHint.TabIndex = 0;
			// 
			// picHint
			// 
			this.picHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picHint.BackgroundImage = global::Mesen.GUI.Properties.Resources.Help;
			this.picHint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picHint.Location = new System.Drawing.Point(3, 5);
			this.picHint.Name = "picHint";
			this.picHint.Size = new System.Drawing.Size(16, 16);
			this.picHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picHint.TabIndex = 0;
			this.picHint.TabStop = false;
			// 
			// lblHint
			// 
			this.lblHint.AutoSize = true;
			this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHint.Location = new System.Drawing.Point(25, 0);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(403, 26);
			this.lblHint.TabIndex = 1;
			this.lblHint.Text = "Tabs with an icon contain key bindings for this player.\r\nEach button can be mappe" +
    "d to up to 4 different keyboard keys or gamepad buttons.";
			// 
			// mnuStripPreset
			// 
			this.mnuStripPreset.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuKeyboard,
            this.toolStripMenuItem1,
            this.mnuXboxController,
            this.mnuPs4Controller,
            this.mnuSnes30Controller});
			this.mnuStripPreset.Name = "mnuStripPreset";
			this.mnuStripPreset.Size = new System.Drawing.Size(170, 98);
			// 
			// mnuKeyboard
			// 
			this.mnuKeyboard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWasdLayout,
            this.mnuArrowLayout,
            this.mnuFceuxLayout,
            this.mnuNestopiaLayout});
			this.mnuKeyboard.Name = "mnuKeyboard";
			this.mnuKeyboard.Size = new System.Drawing.Size(169, 22);
			this.mnuKeyboard.Text = "Keyboard";
			// 
			// mnuWasdLayout
			// 
			this.mnuWasdLayout.Name = "mnuWasdLayout";
			this.mnuWasdLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuWasdLayout.Text = "WASD Layout";
			this.mnuWasdLayout.Click += new System.EventHandler(this.mnuWasdLayout_Click);
			// 
			// mnuArrowLayout
			// 
			this.mnuArrowLayout.Name = "mnuArrowLayout";
			this.mnuArrowLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuArrowLayout.Text = "Arrow Keys Layout";
			this.mnuArrowLayout.Click += new System.EventHandler(this.mnuArrowLayout_Click);
			// 
			// mnuFceuxLayout
			// 
			this.mnuFceuxLayout.Name = "mnuFceuxLayout";
			this.mnuFceuxLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuFceuxLayout.Text = "FCEUX Default";
			this.mnuFceuxLayout.Click += new System.EventHandler(this.mnuFceuxLayout_Click);
			// 
			// mnuNestopiaLayout
			// 
			this.mnuNestopiaLayout.Name = "mnuNestopiaLayout";
			this.mnuNestopiaLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuNestopiaLayout.Text = "Nestopia Default";
			this.mnuNestopiaLayout.Click += new System.EventHandler(this.mnuNestopiaLayout_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 6);
			// 
			// mnuXboxController
			// 
			this.mnuXboxController.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuXboxLayout1,
            this.mnuXboxLayout2});
			this.mnuXboxController.Name = "mnuXboxController";
			this.mnuXboxController.Size = new System.Drawing.Size(169, 22);
			this.mnuXboxController.Text = "Xbox Controller";
			// 
			// mnuXboxLayout1
			// 
			this.mnuXboxLayout1.Name = "mnuXboxLayout1";
			this.mnuXboxLayout1.Size = new System.Drawing.Size(143, 22);
			this.mnuXboxLayout1.Text = "Controller #1";
			this.mnuXboxLayout1.Click += new System.EventHandler(this.mnuXboxLayout1_Click);
			// 
			// mnuXboxLayout2
			// 
			this.mnuXboxLayout2.Name = "mnuXboxLayout2";
			this.mnuXboxLayout2.Size = new System.Drawing.Size(143, 22);
			this.mnuXboxLayout2.Text = "Controller #2";
			this.mnuXboxLayout2.Click += new System.EventHandler(this.mnuXboxLayout2_Click);
			// 
			// mnuPs4Controller
			// 
			this.mnuPs4Controller.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPs4Layout1,
            this.mnuPs4Layout2});
			this.mnuPs4Controller.Name = "mnuPs4Controller";
			this.mnuPs4Controller.Size = new System.Drawing.Size(169, 22);
			this.mnuPs4Controller.Text = "PS4 Controller";
			// 
			// mnuPs4Layout1
			// 
			this.mnuPs4Layout1.Name = "mnuPs4Layout1";
			this.mnuPs4Layout1.Size = new System.Drawing.Size(143, 22);
			this.mnuPs4Layout1.Text = "Controller #1";
			this.mnuPs4Layout1.Click += new System.EventHandler(this.mnuPs4Layout1_Click);
			// 
			// mnuPs4Layout2
			// 
			this.mnuPs4Layout2.Name = "mnuPs4Layout2";
			this.mnuPs4Layout2.Size = new System.Drawing.Size(143, 22);
			this.mnuPs4Layout2.Text = "Controller #2";
			this.mnuPs4Layout2.Click += new System.EventHandler(this.mnuPs4Layout2_Click);
			// 
			// mnuSnes30Controller
			// 
			this.mnuSnes30Controller.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSnes30Layout1,
            this.mnuSnes30Layout2});
			this.mnuSnes30Controller.Name = "mnuSnes30Controller";
			this.mnuSnes30Controller.Size = new System.Drawing.Size(169, 22);
			this.mnuSnes30Controller.Text = "SNES30 Controller";
			// 
			// mnuSnes30Layout1
			// 
			this.mnuSnes30Layout1.Name = "mnuSnes30Layout1";
			this.mnuSnes30Layout1.Size = new System.Drawing.Size(143, 22);
			this.mnuSnes30Layout1.Text = "Controller #1";
			this.mnuSnes30Layout1.Click += new System.EventHandler(this.mnuSnes30Layout1_Click);
			// 
			// mnuSnes30Layout2
			// 
			this.mnuSnes30Layout2.Name = "mnuSnes30Layout2";
			this.mnuSnes30Layout2.Size = new System.Drawing.Size(143, 22);
			this.mnuSnes30Layout2.Text = "Controller #2";
			this.mnuSnes30Layout2.Click += new System.EventHandler(this.mnuSnes30Layout2_Click);
			// 
			// frmControllerConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(599, 353);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmControllerConfig";
			this.Text = "Standard Controller";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tpgSet1.ResumeLayout(false);
			this.tpgSet2.ResumeLayout(false);
			this.tpgSet3.ResumeLayout(false);
			this.tpgSet4.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkTurboSpeed)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.pnlHint.ResumeLayout(false);
			this.flpHint.ResumeLayout(false);
			this.flpHint.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).EndInit();
			this.mnuStripPreset.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ctrlStandardController ctrlStandardController0;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgSet1;
		private System.Windows.Forms.TabPage tpgSet2;
		private ctrlStandardController ctrlStandardController1;
		private System.Windows.Forms.TabPage tpgSet3;
		private ctrlStandardController ctrlStandardController2;
		private System.Windows.Forms.TabPage tpgSet4;
		private ctrlStandardController ctrlStandardController3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TrackBar trkTurboSpeed;
		private System.Windows.Forms.Label lblTurboSpeed;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTurboFast;
		private System.Windows.Forms.Label lblSlow;
		private System.Windows.Forms.Button btnSelectPreset;
		private System.Windows.Forms.ContextMenuStrip mnuStripPreset;
		private System.Windows.Forms.ToolStripMenuItem mnuKeyboard;
		private System.Windows.Forms.ToolStripMenuItem mnuWasdLayout;
		private System.Windows.Forms.ToolStripMenuItem mnuArrowLayout;
		private System.Windows.Forms.ToolStripMenuItem mnuFceuxLayout;
		private System.Windows.Forms.ToolStripMenuItem mnuNestopiaLayout;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuXboxController;
		private System.Windows.Forms.ToolStripMenuItem mnuXboxLayout1;
		private System.Windows.Forms.ToolStripMenuItem mnuXboxLayout2;
		private System.Windows.Forms.ToolStripMenuItem mnuSnes30Controller;
		private System.Windows.Forms.ToolStripMenuItem mnuPs4Controller;
		private System.Windows.Forms.ToolStripMenuItem mnuPs4Layout1;
		private System.Windows.Forms.ToolStripMenuItem mnuPs4Layout2;
		private System.Windows.Forms.ToolStripMenuItem mnuSnes30Layout1;
		private System.Windows.Forms.ToolStripMenuItem mnuSnes30Layout2;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Panel pnlHint;
		private System.Windows.Forms.FlowLayoutPanel flpHint;
		private System.Windows.Forms.PictureBox picHint;
		private System.Windows.Forms.Label lblHint;
	}
}