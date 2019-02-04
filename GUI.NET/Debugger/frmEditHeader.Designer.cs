namespace Mesen.GUI.Debugger
{
	partial class frmEditHeader
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cboInputType = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.radNes2 = new System.Windows.Forms.RadioButton();
			this.radiNes = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.txtChrRomSize = new System.Windows.Forms.TextBox();
			this.lblPrgSize = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cboWorkRam = new System.Windows.Forms.ComboBox();
			this.cboSaveRam = new System.Windows.Forms.ComboBox();
			this.lblMirroringType = new System.Windows.Forms.Label();
			this.cboMirroringType = new System.Windows.Forms.ComboBox();
			this.lblSystem = new System.Windows.Forms.Label();
			this.cboSystem = new System.Windows.Forms.ComboBox();
			this.hexBox = new Be.Windows.Forms.HexBox();
			this.lblHeader = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPrgKb = new System.Windows.Forms.Label();
			this.txtPrgRomSize = new System.Windows.Forms.TextBox();
			this.lblSubmapperId = new System.Windows.Forms.Label();
			this.lblMapperId = new System.Windows.Forms.Label();
			this.txtSubmapperId = new System.Windows.Forms.TextBox();
			this.txtMapperId = new System.Windows.Forms.TextBox();
			this.lblHeaderType = new System.Windows.Forms.Label();
			this.lblInputType = new System.Windows.Forms.Label();
			this.lblVsPpuType = new System.Windows.Forms.Label();
			this.lblVsSystemType = new System.Windows.Forms.Label();
			this.cboVsPpuType = new System.Windows.Forms.ComboBox();
			this.cboVsSystemType = new System.Windows.Forms.ComboBox();
			this.lblFrameTiming = new System.Windows.Forms.Label();
			this.cboFrameTiming = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboChrRam = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cboChrRamBattery = new System.Windows.Forms.ComboBox();
			this.chkBattery = new System.Windows.Forms.CheckBox();
			this.chkTrainer = new System.Windows.Forms.CheckBox();
			this.lblError = new System.Windows.Forms.Label();
			this.baseConfigPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.lblError);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 284);
			this.baseConfigPanel.Size = new System.Drawing.Size(509, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.lblError, 0);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.68224F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.31776F));
			this.tableLayoutPanel1.Controls.Add(this.cboInputType, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPrgSize, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.cboWorkRam, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.cboSaveRam, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblMirroringType, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.cboMirroringType, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblSystem, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.cboSystem, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.hexBox, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.lblHeader, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblSubmapperId, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblMapperId, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtSubmapperId, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtMapperId, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblHeaderType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblInputType, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblVsPpuType, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.lblVsSystemType, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.cboVsPpuType, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.cboVsSystemType, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.lblFrameTiming, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.cboFrameTiming, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.label1, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.cboChrRam, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.label5, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.cboChrRamBattery, 3, 5);
			this.tableLayoutPanel1.Controls.Add(this.chkBattery, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.chkTrainer, 2, 7);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 12;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(509, 284);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// cboInputType
			// 
			this.cboInputType.FormattingEnabled = true;
			this.cboInputType.Location = new System.Drawing.Point(97, 163);
			this.cboInputType.Name = "cboInputType";
			this.cboInputType.Size = new System.Drawing.Size(191, 21);
			this.cboInputType.TabIndex = 33;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.radNes2, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.radiNes, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(94, 0);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(197, 26);
			this.tableLayoutPanel4.TabIndex = 31;
			// 
			// radNes2
			// 
			this.radNes2.AutoSize = true;
			this.radNes2.Location = new System.Drawing.Point(58, 3);
			this.radNes2.Name = "radNes2";
			this.radNes2.Size = new System.Drawing.Size(65, 17);
			this.radNes2.TabIndex = 1;
			this.radNes2.TabStop = true;
			this.radNes2.Text = "NES 2.0";
			this.radNes2.UseVisualStyleBackColor = true;
			this.radNes2.CheckedChanged += new System.EventHandler(this.radVersion_CheckedChanged);
			// 
			// radiNes
			// 
			this.radiNes.AutoSize = true;
			this.radiNes.Location = new System.Drawing.Point(3, 3);
			this.radiNes.Name = "radiNes";
			this.radiNes.Size = new System.Drawing.Size(49, 17);
			this.radiNes.TabIndex = 0;
			this.radiNes.TabStop = true;
			this.radiNes.Text = "iNES";
			this.radiNes.UseVisualStyleBackColor = true;
			this.radiNes.CheckedChanged += new System.EventHandler(this.radVersion_CheckedChanged);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.label6, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.txtChrRomSize, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(385, 26);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(124, 26);
			this.tableLayoutPanel3.TabIndex = 29;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(93, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(21, 13);
			this.label6.TabIndex = 15;
			this.label6.Text = "KB";
			// 
			// txtChrRomSize
			// 
			this.txtChrRomSize.Location = new System.Drawing.Point(3, 3);
			this.txtChrRomSize.Name = "txtChrRomSize";
			this.txtChrRomSize.Size = new System.Drawing.Size(84, 20);
			this.txtChrRomSize.TabIndex = 15;
			// 
			// lblPrgSize
			// 
			this.lblPrgSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPrgSize.AutoSize = true;
			this.lblPrgSize.Location = new System.Drawing.Point(294, 6);
			this.lblPrgSize.Name = "lblPrgSize";
			this.lblPrgSize.Size = new System.Drawing.Size(61, 13);
			this.lblPrgSize.TabIndex = 4;
			this.lblPrgSize.Text = "PRG ROM:";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(294, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "CHR ROM:";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(294, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Work RAM:";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(294, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Save RAM:";
			// 
			// cboWorkRam
			// 
			this.cboWorkRam.FormattingEnabled = true;
			this.cboWorkRam.Location = new System.Drawing.Point(388, 55);
			this.cboWorkRam.Name = "cboWorkRam";
			this.cboWorkRam.Size = new System.Drawing.Size(84, 21);
			this.cboWorkRam.TabIndex = 10;
			// 
			// cboSaveRam
			// 
			this.cboSaveRam.FormattingEnabled = true;
			this.cboSaveRam.Location = new System.Drawing.Point(388, 82);
			this.cboSaveRam.Name = "cboSaveRam";
			this.cboSaveRam.Size = new System.Drawing.Size(84, 21);
			this.cboSaveRam.TabIndex = 12;
			this.cboSaveRam.SelectedIndexChanged += new System.EventHandler(this.cboSaveRam_SelectedIndexChanged);
			// 
			// lblMirroringType
			// 
			this.lblMirroringType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMirroringType.AutoSize = true;
			this.lblMirroringType.Location = new System.Drawing.Point(3, 86);
			this.lblMirroringType.Name = "lblMirroringType";
			this.lblMirroringType.Size = new System.Drawing.Size(77, 13);
			this.lblMirroringType.TabIndex = 16;
			this.lblMirroringType.Text = "Mirroring Type:";
			// 
			// cboMirroringType
			// 
			this.cboMirroringType.FormattingEnabled = true;
			this.cboMirroringType.Location = new System.Drawing.Point(97, 82);
			this.cboMirroringType.Name = "cboMirroringType";
			this.cboMirroringType.Size = new System.Drawing.Size(101, 21);
			this.cboMirroringType.TabIndex = 17;
			// 
			// lblSystem
			// 
			this.lblSystem.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSystem.AutoSize = true;
			this.lblSystem.Location = new System.Drawing.Point(3, 140);
			this.lblSystem.Name = "lblSystem";
			this.lblSystem.Size = new System.Drawing.Size(44, 13);
			this.lblSystem.TabIndex = 22;
			this.lblSystem.Text = "System:";
			// 
			// cboSystem
			// 
			this.cboSystem.FormattingEnabled = true;
			this.cboSystem.Location = new System.Drawing.Point(97, 136);
			this.cboSystem.Name = "cboSystem";
			this.cboSystem.Size = new System.Drawing.Size(191, 21);
			this.cboSystem.TabIndex = 23;
			this.cboSystem.SelectedIndexChanged += new System.EventHandler(this.cboSystem_SelectedIndexChanged);
			// 
			// hexBox
			// 
			this.hexBox.ByteColorProvider = null;
			this.hexBox.ByteEditingMode = false;
			this.tableLayoutPanel1.SetColumnSpan(this.hexBox, 3);
			this.hexBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hexBox.EnablePerByteNavigation = false;
			this.hexBox.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.hexBox.HighDensityMode = false;
			this.hexBox.InfoBackColor = System.Drawing.Color.DarkGray;
			this.hexBox.Location = new System.Drawing.Point(97, 252);
			this.hexBox.Name = "hexBox";
			this.hexBox.ReadOnly = true;
			this.hexBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
			this.hexBox.Size = new System.Drawing.Size(409, 18);
			this.hexBox.TabIndex = 26;
			// 
			// lblHeader
			// 
			this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHeader.AutoSize = true;
			this.lblHeader.Location = new System.Drawing.Point(3, 254);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(77, 13);
			this.lblHeader.TabIndex = 27;
			this.lblHeader.Text = "Binary Header:";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblPrgKb, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtPrgRomSize, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(385, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(124, 26);
			this.tableLayoutPanel2.TabIndex = 28;
			// 
			// lblPrgKb
			// 
			this.lblPrgKb.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPrgKb.AutoSize = true;
			this.lblPrgKb.Location = new System.Drawing.Point(93, 6);
			this.lblPrgKb.Name = "lblPrgKb";
			this.lblPrgKb.Size = new System.Drawing.Size(21, 13);
			this.lblPrgKb.TabIndex = 15;
			this.lblPrgKb.Text = "KB";
			// 
			// txtPrgRomSize
			// 
			this.txtPrgRomSize.Location = new System.Drawing.Point(3, 3);
			this.txtPrgRomSize.Name = "txtPrgRomSize";
			this.txtPrgRomSize.Size = new System.Drawing.Size(84, 20);
			this.txtPrgRomSize.TabIndex = 14;
			// 
			// lblSubmapperId
			// 
			this.lblSubmapperId.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSubmapperId.AutoSize = true;
			this.lblSubmapperId.Location = new System.Drawing.Point(3, 59);
			this.lblSubmapperId.Name = "lblSubmapperId";
			this.lblSubmapperId.Size = new System.Drawing.Size(64, 13);
			this.lblSubmapperId.TabIndex = 1;
			this.lblSubmapperId.Text = "Submapper:";
			// 
			// lblMapperId
			// 
			this.lblMapperId.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMapperId.AutoSize = true;
			this.lblMapperId.Location = new System.Drawing.Point(3, 32);
			this.lblMapperId.Name = "lblMapperId";
			this.lblMapperId.Size = new System.Drawing.Size(46, 13);
			this.lblMapperId.TabIndex = 0;
			this.lblMapperId.Text = "Mapper:";
			// 
			// txtSubmapperId
			// 
			this.txtSubmapperId.Location = new System.Drawing.Point(97, 55);
			this.txtSubmapperId.Name = "txtSubmapperId";
			this.txtSubmapperId.Size = new System.Drawing.Size(55, 20);
			this.txtSubmapperId.TabIndex = 3;
			// 
			// txtMapperId
			// 
			this.txtMapperId.Location = new System.Drawing.Point(97, 29);
			this.txtMapperId.Name = "txtMapperId";
			this.txtMapperId.Size = new System.Drawing.Size(55, 20);
			this.txtMapperId.TabIndex = 2;
			// 
			// lblHeaderType
			// 
			this.lblHeaderType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHeaderType.AutoSize = true;
			this.lblHeaderType.Location = new System.Drawing.Point(3, 6);
			this.lblHeaderType.Name = "lblHeaderType";
			this.lblHeaderType.Size = new System.Drawing.Size(53, 13);
			this.lblHeaderType.TabIndex = 30;
			this.lblHeaderType.Text = "File Type:";
			// 
			// lblInputType
			// 
			this.lblInputType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblInputType.AutoSize = true;
			this.lblInputType.Location = new System.Drawing.Point(3, 167);
			this.lblInputType.Name = "lblInputType";
			this.lblInputType.Size = new System.Drawing.Size(61, 13);
			this.lblInputType.TabIndex = 32;
			this.lblInputType.Text = "Input Type:";
			// 
			// lblVsPpuType
			// 
			this.lblVsPpuType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVsPpuType.AutoSize = true;
			this.lblVsPpuType.Location = new System.Drawing.Point(3, 219);
			this.lblVsPpuType.Name = "lblVsPpuType";
			this.lblVsPpuType.Size = new System.Drawing.Size(76, 13);
			this.lblVsPpuType.TabIndex = 24;
			this.lblVsPpuType.Text = "VS PPU Type:";
			// 
			// lblVsSystemType
			// 
			this.lblVsSystemType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVsSystemType.AutoSize = true;
			this.lblVsSystemType.Location = new System.Drawing.Point(3, 193);
			this.lblVsSystemType.Name = "lblVsSystemType";
			this.lblVsSystemType.Size = new System.Drawing.Size(88, 13);
			this.lblVsSystemType.TabIndex = 34;
			this.lblVsSystemType.Text = "VS System Type:";
			// 
			// cboVsPpuType
			// 
			this.cboVsPpuType.FormattingEnabled = true;
			this.cboVsPpuType.Location = new System.Drawing.Point(97, 216);
			this.cboVsPpuType.Name = "cboVsPpuType";
			this.cboVsPpuType.Size = new System.Drawing.Size(101, 21);
			this.cboVsPpuType.TabIndex = 25;
			// 
			// cboVsSystemType
			// 
			this.cboVsSystemType.FormattingEnabled = true;
			this.cboVsSystemType.Location = new System.Drawing.Point(97, 190);
			this.cboVsSystemType.Name = "cboVsSystemType";
			this.cboVsSystemType.Size = new System.Drawing.Size(147, 21);
			this.cboVsSystemType.TabIndex = 35;
			// 
			// lblFrameTiming
			// 
			this.lblFrameTiming.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFrameTiming.AutoSize = true;
			this.lblFrameTiming.Location = new System.Drawing.Point(3, 113);
			this.lblFrameTiming.Name = "lblFrameTiming";
			this.lblFrameTiming.Size = new System.Drawing.Size(73, 13);
			this.lblFrameTiming.TabIndex = 36;
			this.lblFrameTiming.Text = "Frame Timing:";
			// 
			// cboFrameTiming
			// 
			this.cboFrameTiming.FormattingEnabled = true;
			this.cboFrameTiming.Location = new System.Drawing.Point(97, 109);
			this.cboFrameTiming.Name = "cboFrameTiming";
			this.cboFrameTiming.Size = new System.Drawing.Size(101, 21);
			this.cboFrameTiming.TabIndex = 37;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(294, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "CHR RAM:";
			// 
			// cboChrRam
			// 
			this.cboChrRam.FormattingEnabled = true;
			this.cboChrRam.Location = new System.Drawing.Point(388, 109);
			this.cboChrRam.Name = "cboChrRam";
			this.cboChrRam.Size = new System.Drawing.Size(84, 21);
			this.cboChrRam.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(294, 140);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "CHR Save RAM:";
			// 
			// cboChrRamBattery
			// 
			this.cboChrRamBattery.FormattingEnabled = true;
			this.cboChrRamBattery.Location = new System.Drawing.Point(388, 136);
			this.cboChrRamBattery.Name = "cboChrRamBattery";
			this.cboChrRamBattery.Size = new System.Drawing.Size(84, 21);
			this.cboChrRamBattery.TabIndex = 13;
			this.cboChrRamBattery.SelectedIndexChanged += new System.EventHandler(this.cboSaveRam_SelectedIndexChanged);
			// 
			// chkBattery
			// 
			this.chkBattery.AutoSize = true;
			this.chkBattery.Location = new System.Drawing.Point(294, 163);
			this.chkBattery.Name = "chkBattery";
			this.chkBattery.Size = new System.Drawing.Size(59, 17);
			this.chkBattery.TabIndex = 19;
			this.chkBattery.Text = "Battery";
			this.chkBattery.UseVisualStyleBackColor = true;
			// 
			// chkTrainer
			// 
			this.chkTrainer.AutoSize = true;
			this.chkTrainer.Location = new System.Drawing.Point(294, 190);
			this.chkTrainer.Name = "chkTrainer";
			this.chkTrainer.Size = new System.Drawing.Size(59, 17);
			this.chkTrainer.TabIndex = 21;
			this.chkTrainer.Text = "Trainer";
			this.chkTrainer.UseVisualStyleBackColor = true;
			// 
			// lblError
			// 
			this.lblError.AutoSize = true;
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(4, 8);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(44, 13);
			this.lblError.TabIndex = 3;
			this.lblError.Text = "warning";
			this.lblError.Visible = false;
			// 
			// frmEditHeader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(509, 313);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(493, 290);
			this.Name = "frmEditHeader";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NES Header Editor";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.baseConfigPanel.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblMapperId;
		private System.Windows.Forms.Label lblSubmapperId;
		private System.Windows.Forms.TextBox txtMapperId;
		private System.Windows.Forms.TextBox txtSubmapperId;
		private System.Windows.Forms.Label lblPrgSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cboWorkRam;
		private System.Windows.Forms.ComboBox cboSaveRam;
		private System.Windows.Forms.ComboBox cboChrRam;
		private System.Windows.Forms.ComboBox cboChrRamBattery;
		private System.Windows.Forms.TextBox txtChrRomSize;
		private System.Windows.Forms.TextBox txtPrgRomSize;
		private System.Windows.Forms.Label lblMirroringType;
		private System.Windows.Forms.ComboBox cboMirroringType;
		private System.Windows.Forms.CheckBox chkTrainer;
		private System.Windows.Forms.CheckBox chkBattery;
		private System.Windows.Forms.Label lblSystem;
		private System.Windows.Forms.ComboBox cboSystem;
		private System.Windows.Forms.Label lblVsPpuType;
		private System.Windows.Forms.ComboBox cboVsPpuType;
		private System.Windows.Forms.Label lblError;
		private Be.Windows.Forms.HexBox hexBox;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.RadioButton radNes2;
		private System.Windows.Forms.RadioButton radiNes;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblPrgKb;
		private System.Windows.Forms.Label lblHeaderType;
		private System.Windows.Forms.Label lblInputType;
		private System.Windows.Forms.ComboBox cboInputType;
		private System.Windows.Forms.Label lblVsSystemType;
		private System.Windows.Forms.ComboBox cboVsSystemType;
		private System.Windows.Forms.Label lblFrameTiming;
		private System.Windows.Forms.ComboBox cboFrameTiming;
	}
}