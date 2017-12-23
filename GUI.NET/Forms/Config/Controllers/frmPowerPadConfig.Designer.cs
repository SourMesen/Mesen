namespace Mesen.GUI.Forms.Config
{
	partial class frmPowerPadConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPowerPadConfig));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.btnClear = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnSetDefault = new System.Windows.Forms.Button();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgSet1 = new System.Windows.Forms.TabPage();
			this.ctrlPowerPadConfig0 = new Mesen.GUI.Forms.Config.ctrlPowerPadConfig();
			this.tpgSet2 = new System.Windows.Forms.TabPage();
			this.ctrlPowerPadConfig1 = new Mesen.GUI.Forms.Config.ctrlPowerPadConfig();
			this.tpgSet3 = new System.Windows.Forms.TabPage();
			this.ctrlPowerPadConfig2 = new Mesen.GUI.Forms.Config.ctrlPowerPadConfig();
			this.tpgSet4 = new System.Windows.Forms.TabPage();
			this.ctrlPowerPadConfig3 = new Mesen.GUI.Forms.Config.ctrlPowerPadConfig();
			this.chkUseSideA = new System.Windows.Forms.CheckBox();
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
			this.ctrlKeyBindingHint1 = new Mesen.GUI.Forms.Config.ctrlKeyBindingHint();
			this.baseConfigPanel.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tpgSet1.SuspendLayout();
			this.tpgSet2.SuspendLayout();
			this.tpgSet3.SuspendLayout();
			this.tpgSet4.SuspendLayout();
			this.mnuStripPreset.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.flowLayoutPanel2);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 405);
			this.baseConfigPanel.Size = new System.Drawing.Size(496, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.flowLayoutPanel2, 0);
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
			this.flowLayoutPanel2.Controls.Add(this.btnSetDefault);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(264, 29);
			this.flowLayoutPanel2.TabIndex = 5;
			// 
			// btnSetDefault
			// 
			this.btnSetDefault.AutoSize = true;
			this.btnSetDefault.Location = new System.Drawing.Point(114, 3);
			this.btnSetDefault.Name = "btnSetDefault";
			this.btnSetDefault.Size = new System.Drawing.Size(113, 23);
			this.btnSetDefault.TabIndex = 4;
			this.btnSetDefault.Text = "Set Default Bindings";
			this.btnSetDefault.UseVisualStyleBackColor = true;
			this.btnSetDefault.Click += new System.EventHandler(this.btnSetDefault_Click);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.tabMain, 0, 1);
			this.tlpMain.Controls.Add(this.chkUseSideA, 0, 2);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 31);
			this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 4;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(496, 374);
			this.tlpMain.TabIndex = 23;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgSet1);
			this.tabMain.Controls.Add(this.tpgSet2);
			this.tabMain.Controls.Add(this.tpgSet3);
			this.tabMain.Controls.Add(this.tpgSet4);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.ImageList = this.imageList;
			this.tabMain.Location = new System.Drawing.Point(3, 3);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(490, 345);
			this.tabMain.TabIndex = 3;
			// 
			// tpgSet1
			// 
			this.tpgSet1.Controls.Add(this.ctrlPowerPadConfig0);
			this.tpgSet1.Location = new System.Drawing.Point(4, 23);
			this.tpgSet1.Name = "tpgSet1";
			this.tpgSet1.Size = new System.Drawing.Size(482, 318);
			this.tpgSet1.TabIndex = 0;
			this.tpgSet1.Text = "Key Set #1";
			this.tpgSet1.UseVisualStyleBackColor = true;
			// 
			// ctrlPowerPadConfig0
			// 
			this.ctrlPowerPadConfig0.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPowerPadConfig0.Location = new System.Drawing.Point(0, 0);
			this.ctrlPowerPadConfig0.Name = "ctrlPowerPadConfig0";
			this.ctrlPowerPadConfig0.Size = new System.Drawing.Size(482, 318);
			this.ctrlPowerPadConfig0.TabIndex = 0;
			// 
			// tpgSet2
			// 
			this.tpgSet2.Controls.Add(this.ctrlPowerPadConfig1);
			this.tpgSet2.Location = new System.Drawing.Point(4, 23);
			this.tpgSet2.Name = "tpgSet2";
			this.tpgSet2.Size = new System.Drawing.Size(482, 318);
			this.tpgSet2.TabIndex = 1;
			this.tpgSet2.Text = "Key Set #2";
			this.tpgSet2.UseVisualStyleBackColor = true;
			// 
			// ctrlPowerPadConfig1
			// 
			this.ctrlPowerPadConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPowerPadConfig1.Location = new System.Drawing.Point(0, 0);
			this.ctrlPowerPadConfig1.Name = "ctrlPowerPadConfig1";
			this.ctrlPowerPadConfig1.Size = new System.Drawing.Size(482, 318);
			this.ctrlPowerPadConfig1.TabIndex = 1;
			// 
			// tpgSet3
			// 
			this.tpgSet3.Controls.Add(this.ctrlPowerPadConfig2);
			this.tpgSet3.Location = new System.Drawing.Point(4, 23);
			this.tpgSet3.Name = "tpgSet3";
			this.tpgSet3.Size = new System.Drawing.Size(482, 318);
			this.tpgSet3.TabIndex = 2;
			this.tpgSet3.Text = "Key Set #3";
			this.tpgSet3.UseVisualStyleBackColor = true;
			// 
			// ctrlPowerPadConfig2
			// 
			this.ctrlPowerPadConfig2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPowerPadConfig2.Location = new System.Drawing.Point(0, 0);
			this.ctrlPowerPadConfig2.Name = "ctrlPowerPadConfig2";
			this.ctrlPowerPadConfig2.Size = new System.Drawing.Size(482, 318);
			this.ctrlPowerPadConfig2.TabIndex = 1;
			// 
			// tpgSet4
			// 
			this.tpgSet4.Controls.Add(this.ctrlPowerPadConfig3);
			this.tpgSet4.Location = new System.Drawing.Point(4, 23);
			this.tpgSet4.Name = "tpgSet4";
			this.tpgSet4.Size = new System.Drawing.Size(482, 318);
			this.tpgSet4.TabIndex = 3;
			this.tpgSet4.Text = "Key Set #4";
			this.tpgSet4.UseVisualStyleBackColor = true;
			// 
			// ctrlPowerPadConfig3
			// 
			this.ctrlPowerPadConfig3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlPowerPadConfig3.Location = new System.Drawing.Point(0, 0);
			this.ctrlPowerPadConfig3.Name = "ctrlPowerPadConfig3";
			this.ctrlPowerPadConfig3.Size = new System.Drawing.Size(482, 318);
			this.ctrlPowerPadConfig3.TabIndex = 1;
			// 
			// chkUseSideA
			// 
			this.chkUseSideA.AutoSize = true;
			this.chkUseSideA.Location = new System.Drawing.Point(8, 354);
			this.chkUseSideA.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
			this.chkUseSideA.Name = "chkUseSideA";
			this.chkUseSideA.Size = new System.Drawing.Size(77, 17);
			this.chkUseSideA.TabIndex = 4;
			this.chkUseSideA.Text = "Use side A";
			this.chkUseSideA.UseVisualStyleBackColor = true;
			this.chkUseSideA.CheckedChanged += new System.EventHandler(this.chkShowSideA_CheckedChanged);
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
			// 
			// mnuArrowLayout
			// 
			this.mnuArrowLayout.Name = "mnuArrowLayout";
			this.mnuArrowLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuArrowLayout.Text = "Arrow Keys Layout";
			// 
			// mnuFceuxLayout
			// 
			this.mnuFceuxLayout.Name = "mnuFceuxLayout";
			this.mnuFceuxLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuFceuxLayout.Text = "FCEUX Default";
			// 
			// mnuNestopiaLayout
			// 
			this.mnuNestopiaLayout.Name = "mnuNestopiaLayout";
			this.mnuNestopiaLayout.Size = new System.Drawing.Size(172, 22);
			this.mnuNestopiaLayout.Text = "Nestopia Default";
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
			// 
			// mnuXboxLayout2
			// 
			this.mnuXboxLayout2.Name = "mnuXboxLayout2";
			this.mnuXboxLayout2.Size = new System.Drawing.Size(143, 22);
			this.mnuXboxLayout2.Text = "Controller #2";
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
			// 
			// mnuPs4Layout2
			// 
			this.mnuPs4Layout2.Name = "mnuPs4Layout2";
			this.mnuPs4Layout2.Size = new System.Drawing.Size(143, 22);
			this.mnuPs4Layout2.Text = "Controller #2";
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
			// 
			// mnuSnes30Layout2
			// 
			this.mnuSnes30Layout2.Name = "mnuSnes30Layout2";
			this.mnuSnes30Layout2.Size = new System.Drawing.Size(143, 22);
			this.mnuSnes30Layout2.Text = "Controller #2";
			// 
			// ctrlKeyBindingHint1
			// 
			this.ctrlKeyBindingHint1.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctrlKeyBindingHint1.Location = new System.Drawing.Point(0, 0);
			this.ctrlKeyBindingHint1.Name = "ctrlKeyBindingHint1";
			this.ctrlKeyBindingHint1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.ctrlKeyBindingHint1.Size = new System.Drawing.Size(496, 31);
			this.ctrlKeyBindingHint1.TabIndex = 24;
			// 
			// frmPowerPadConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(496, 434);
			this.Controls.Add(this.tlpMain);
			this.Controls.Add(this.ctrlKeyBindingHint1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmPowerPadConfig";
			this.Text = "Power Pad / Family Mat Trainer";
			this.Controls.SetChildIndex(this.ctrlKeyBindingHint1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tlpMain, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tpgSet1.ResumeLayout(false);
			this.tpgSet2.ResumeLayout(false);
			this.tpgSet3.ResumeLayout(false);
			this.tpgSet4.ResumeLayout(false);
			this.mnuStripPreset.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.TableLayoutPanel tlpMain;
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
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgSet1;
		private ctrlPowerPadConfig ctrlPowerPadConfig0;
		private System.Windows.Forms.TabPage tpgSet2;
		private ctrlPowerPadConfig ctrlPowerPadConfig1;
		private System.Windows.Forms.TabPage tpgSet3;
		private ctrlPowerPadConfig ctrlPowerPadConfig2;
		private System.Windows.Forms.TabPage tpgSet4;
		private ctrlPowerPadConfig ctrlPowerPadConfig3;
		private ctrlKeyBindingHint ctrlKeyBindingHint1;
		private System.Windows.Forms.Button btnSetDefault;
		private System.Windows.Forms.CheckBox chkUseSideA;
	}
}