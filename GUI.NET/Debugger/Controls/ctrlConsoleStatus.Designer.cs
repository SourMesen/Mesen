namespace Mesen.GUI.Debugger
{
	partial class ctrlConsoleStatus
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.grpPPUStatus = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.txtNTAddr = new System.Windows.Forms.TextBox();
			this.lblNTAddr = new System.Windows.Forms.Label();
			this.lblVRAMAddr = new System.Windows.Forms.Label();
			this.lblCycle = new System.Windows.Forms.Label();
			this.txtCycle = new System.Windows.Forms.TextBox();
			this.txtVRAMAddr = new System.Windows.Forms.TextBox();
			this.lblScanline = new System.Windows.Forms.Label();
			this.txtScanline = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.chkWriteToggle = new System.Windows.Forms.CheckBox();
			this.chkSpriteOverflow = new System.Windows.Forms.CheckBox();
			this.chkSprite0Hit = new System.Windows.Forms.CheckBox();
			this.chkVerticalBlank = new System.Windows.Forms.CheckBox();
			this.lblXScroll = new System.Windows.Forms.Label();
			this.txtXScroll = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtTmpAddr = new System.Windows.Forms.TextBox();
			this.lblFrameCount = new System.Windows.Forms.Label();
			this.txtFrameCount = new System.Windows.Forms.TextBox();
			this.grpControlMask = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDrawLeftSpr = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblSprAddr = new System.Windows.Forms.Label();
			this.txtSprAddr = new System.Windows.Forms.TextBox();
			this.chkSpritesEnabled = new System.Windows.Forms.CheckBox();
			this.chkBGEnabled = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkNMIOnBlank = new System.Windows.Forms.CheckBox();
			this.chkLargeSprites = new System.Windows.Forms.CheckBox();
			this.chkVerticalWrite = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblBGAddr = new System.Windows.Forms.Label();
			this.txtBGAddr = new System.Windows.Forms.TextBox();
			this.chkDrawLeftBG = new System.Windows.Forms.CheckBox();
			this.chkGrayscale = new System.Windows.Forms.CheckBox();
			this.chkIntensifyRed = new System.Windows.Forms.CheckBox();
			this.chkIntensifyGreen = new System.Windows.Forms.CheckBox();
			this.chkIntensifyBlue = new System.Windows.Forms.CheckBox();
			this.grpCPUStatus = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpFlags = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.chkFrameCounter = new System.Windows.Forms.CheckBox();
			this.chkExternal = new System.Windows.Forms.CheckBox();
			this.chkNMI = new System.Windows.Forms.CheckBox();
			this.chkDMC = new System.Windows.Forms.CheckBox();
			this.chkBreak = new System.Windows.Forms.CheckBox();
			this.chkNegative = new System.Windows.Forms.CheckBox();
			this.chkOverflow = new System.Windows.Forms.CheckBox();
			this.chkDecimal = new System.Windows.Forms.CheckBox();
			this.chkReserved = new System.Windows.Forms.CheckBox();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.chkInterrupt = new System.Windows.Forms.CheckBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.chkZero = new System.Windows.Forms.CheckBox();
			this.chkCarry = new System.Windows.Forms.CheckBox();
			this.lblIrqs = new System.Windows.Forms.Label();
			this.grpStack = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblSP = new System.Windows.Forms.Label();
			this.txtSP = new System.Windows.Forms.TextBox();
			this.txtStack = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblA = new System.Windows.Forms.Label();
			this.txtA = new System.Windows.Forms.TextBox();
			this.lblX = new System.Windows.Forms.Label();
			this.txtX = new System.Windows.Forms.TextBox();
			this.lblY = new System.Windows.Forms.Label();
			this.txtY = new System.Windows.Forms.TextBox();
			this.lblPC = new System.Windows.Forms.Label();
			this.txtPC = new System.Windows.Forms.TextBox();
			this.lblCycleCount = new System.Windows.Forms.Label();
			this.txtCycleCount = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.btnGoto = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.grpInputStatus = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlControllerInput3 = new Mesen.GUI.Debugger.Controls.ctrlControllerInput();
			this.ctrlControllerInput4 = new Mesen.GUI.Debugger.Controls.ctrlControllerInput();
			this.ctrlControllerInput1 = new Mesen.GUI.Debugger.Controls.ctrlControllerInput();
			this.ctrlControllerInput2 = new Mesen.GUI.Debugger.Controls.ctrlControllerInput();
			this.tmrButton = new System.Windows.Forms.Timer(this.components);
			this.contextGoTo = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuGoToIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToNmiHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToInitHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToPlayHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.sepFds = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFdsIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFdsNmiHandler1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFdsNmiHandler2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFdsNmiHandler3 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFdsResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToProgramCounter = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpPPUStatus.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.grpControlMask.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.grpCPUStatus.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpFlags.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.grpStack.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.grpInputStatus.SuspendLayout();
			this.tableLayoutPanel11.SuspendLayout();
			this.contextGoTo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.grpPPUStatus, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.grpCPUStatus, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel10, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.grpInputStatus, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(453, 415);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// grpPPUStatus
			// 
			this.grpPPUStatus.Controls.Add(this.tableLayoutPanel8);
			this.grpPPUStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpPPUStatus.Location = new System.Drawing.Point(1, 128);
			this.grpPPUStatus.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.grpPPUStatus.Name = "grpPPUStatus";
			this.grpPPUStatus.Size = new System.Drawing.Size(451, 173);
			this.grpPPUStatus.TabIndex = 2;
			this.grpPPUStatus.TabStop = false;
			this.grpPPUStatus.Text = "PPU Status";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.25163F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.74837F));
			this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel7, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.grpControlMask, 1, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 1;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(445, 154);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.ColumnCount = 4;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add(this.txtNTAddr, 1, 3);
			this.tableLayoutPanel7.Controls.Add(this.lblNTAddr, 0, 3);
			this.tableLayoutPanel7.Controls.Add(this.lblVRAMAddr, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.lblCycle, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.txtCycle, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.txtVRAMAddr, 1, 2);
			this.tableLayoutPanel7.Controls.Add(this.lblScanline, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.txtScanline, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel5, 0, 5);
			this.tableLayoutPanel7.Controls.Add(this.lblXScroll, 0, 4);
			this.tableLayoutPanel7.Controls.Add(this.txtXScroll, 1, 4);
			this.tableLayoutPanel7.Controls.Add(this.label1, 2, 2);
			this.tableLayoutPanel7.Controls.Add(this.txtTmpAddr, 3, 2);
			this.tableLayoutPanel7.Controls.Add(this.lblFrameCount, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.txtFrameCount, 3, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 7;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(193, 148);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// txtNTAddr
			// 
			this.txtNTAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtNTAddr.Location = new System.Drawing.Point(66, 60);
			this.txtNTAddr.Margin = new System.Windows.Forms.Padding(0);
			this.txtNTAddr.Name = "txtNTAddr";
			this.txtNTAddr.ReadOnly = true;
			this.txtNTAddr.Size = new System.Drawing.Size(45, 20);
			this.txtNTAddr.TabIndex = 1;
			// 
			// lblNTAddr
			// 
			this.lblNTAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNTAddr.AutoSize = true;
			this.lblNTAddr.Location = new System.Drawing.Point(0, 63);
			this.lblNTAddr.Margin = new System.Windows.Forms.Padding(0);
			this.lblNTAddr.Name = "lblNTAddr";
			this.lblNTAddr.Size = new System.Drawing.Size(50, 13);
			this.lblNTAddr.TabIndex = 0;
			this.lblNTAddr.Text = "NT Addr:";
			this.lblNTAddr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblVRAMAddr
			// 
			this.lblVRAMAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVRAMAddr.AutoSize = true;
			this.lblVRAMAddr.Location = new System.Drawing.Point(0, 43);
			this.lblVRAMAddr.Margin = new System.Windows.Forms.Padding(0);
			this.lblVRAMAddr.Name = "lblVRAMAddr";
			this.lblVRAMAddr.Size = new System.Drawing.Size(66, 13);
			this.lblVRAMAddr.TabIndex = 5;
			this.lblVRAMAddr.Text = "VRAM Addr:";
			this.lblVRAMAddr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCycle
			// 
			this.lblCycle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCycle.AutoSize = true;
			this.lblCycle.Location = new System.Drawing.Point(0, 3);
			this.lblCycle.Margin = new System.Windows.Forms.Padding(0);
			this.lblCycle.Name = "lblCycle";
			this.lblCycle.Size = new System.Drawing.Size(36, 13);
			this.lblCycle.TabIndex = 1;
			this.lblCycle.Text = "Cycle:";
			this.lblCycle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtCycle
			// 
			this.txtCycle.Location = new System.Drawing.Point(66, 0);
			this.txtCycle.Margin = new System.Windows.Forms.Padding(0);
			this.txtCycle.MaxLength = 3;
			this.txtCycle.Name = "txtCycle";
			this.txtCycle.Size = new System.Drawing.Size(45, 20);
			this.txtCycle.TabIndex = 2;
			this.txtCycle.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// txtVRAMAddr
			// 
			this.txtVRAMAddr.Location = new System.Drawing.Point(66, 40);
			this.txtVRAMAddr.Margin = new System.Windows.Forms.Padding(0);
			this.txtVRAMAddr.Name = "txtVRAMAddr";
			this.txtVRAMAddr.Size = new System.Drawing.Size(45, 20);
			this.txtVRAMAddr.TabIndex = 6;
			this.txtVRAMAddr.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblScanline
			// 
			this.lblScanline.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblScanline.AutoSize = true;
			this.lblScanline.Location = new System.Drawing.Point(0, 23);
			this.lblScanline.Margin = new System.Windows.Forms.Padding(0);
			this.lblScanline.Name = "lblScanline";
			this.lblScanline.Size = new System.Drawing.Size(51, 13);
			this.lblScanline.TabIndex = 3;
			this.lblScanline.Text = "Scanline:";
			this.lblScanline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtScanline
			// 
			this.txtScanline.Location = new System.Drawing.Point(66, 20);
			this.txtScanline.Margin = new System.Windows.Forms.Padding(0);
			this.txtScanline.MaxLength = 3;
			this.txtScanline.Name = "txtScanline";
			this.txtScanline.Size = new System.Drawing.Size(45, 20);
			this.txtScanline.TabIndex = 4;
			this.txtScanline.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel7.SetColumnSpan(this.tableLayoutPanel5, 4);
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add(this.chkWriteToggle, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkSpriteOverflow, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkSprite0Hit, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.chkVerticalBlank, 1, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 100);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(193, 36);
			this.tableLayoutPanel5.TabIndex = 10;
			// 
			// chkWriteToggle
			// 
			this.chkWriteToggle.AutoSize = true;
			this.chkWriteToggle.Location = new System.Drawing.Point(98, 21);
			this.chkWriteToggle.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.chkWriteToggle.Name = "chkWriteToggle";
			this.chkWriteToggle.Size = new System.Drawing.Size(87, 17);
			this.chkWriteToggle.TabIndex = 10;
			this.chkWriteToggle.Text = "Write Toggle";
			this.chkWriteToggle.UseVisualStyleBackColor = true;
			this.chkWriteToggle.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkSpriteOverflow
			// 
			this.chkSpriteOverflow.AutoSize = true;
			this.chkSpriteOverflow.Location = new System.Drawing.Point(0, 21);
			this.chkSpriteOverflow.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.chkSpriteOverflow.Name = "chkSpriteOverflow";
			this.chkSpriteOverflow.Size = new System.Drawing.Size(98, 17);
			this.chkSpriteOverflow.TabIndex = 8;
			this.chkSpriteOverflow.Text = "Sprite Overflow";
			this.chkSpriteOverflow.UseVisualStyleBackColor = true;
			this.chkSpriteOverflow.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkSprite0Hit
			// 
			this.chkSprite0Hit.AutoSize = true;
			this.chkSprite0Hit.Location = new System.Drawing.Point(0, 2);
			this.chkSprite0Hit.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.chkSprite0Hit.Name = "chkSprite0Hit";
			this.chkSprite0Hit.Size = new System.Drawing.Size(78, 17);
			this.chkSprite0Hit.TabIndex = 9;
			this.chkSprite0Hit.Text = "Sprite 0 Hit";
			this.chkSprite0Hit.UseVisualStyleBackColor = true;
			this.chkSprite0Hit.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkVerticalBlank
			// 
			this.chkVerticalBlank.AutoSize = true;
			this.chkVerticalBlank.Location = new System.Drawing.Point(98, 2);
			this.chkVerticalBlank.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.chkVerticalBlank.Name = "chkVerticalBlank";
			this.chkVerticalBlank.Size = new System.Drawing.Size(91, 17);
			this.chkVerticalBlank.TabIndex = 7;
			this.chkVerticalBlank.Text = "Vertical Blank";
			this.chkVerticalBlank.UseVisualStyleBackColor = true;
			this.chkVerticalBlank.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblXScroll
			// 
			this.lblXScroll.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblXScroll.AutoSize = true;
			this.lblXScroll.Location = new System.Drawing.Point(0, 83);
			this.lblXScroll.Margin = new System.Windows.Forms.Padding(0);
			this.lblXScroll.Name = "lblXScroll";
			this.lblXScroll.Size = new System.Drawing.Size(46, 13);
			this.lblXScroll.TabIndex = 11;
			this.lblXScroll.Text = "X Scroll:";
			this.lblXScroll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtXScroll
			// 
			this.txtXScroll.Location = new System.Drawing.Point(66, 80);
			this.txtXScroll.Margin = new System.Windows.Forms.Padding(0);
			this.txtXScroll.Name = "txtXScroll";
			this.txtXScroll.Size = new System.Drawing.Size(45, 20);
			this.txtXScroll.TabIndex = 12;
			this.txtXScroll.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(111, 43);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(17, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "T:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTmpAddr
			// 
			this.txtTmpAddr.Location = new System.Drawing.Point(137, 40);
			this.txtTmpAddr.Margin = new System.Windows.Forms.Padding(0);
			this.txtTmpAddr.Name = "txtTmpAddr";
			this.txtTmpAddr.Size = new System.Drawing.Size(45, 20);
			this.txtTmpAddr.TabIndex = 14;
			this.txtTmpAddr.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblFrameCount
			// 
			this.lblFrameCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFrameCount.AutoSize = true;
			this.lblFrameCount.Location = new System.Drawing.Point(111, 3);
			this.lblFrameCount.Margin = new System.Windows.Forms.Padding(0);
			this.lblFrameCount.Name = "lblFrameCount";
			this.lblFrameCount.Size = new System.Drawing.Size(26, 13);
			this.lblFrameCount.TabIndex = 15;
			this.lblFrameCount.Text = "Fr#:";
			this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFrameCount
			// 
			this.txtFrameCount.Location = new System.Drawing.Point(137, 0);
			this.txtFrameCount.Margin = new System.Windows.Forms.Padding(0);
			this.txtFrameCount.Name = "txtFrameCount";
			this.txtFrameCount.Size = new System.Drawing.Size(52, 20);
			this.txtFrameCount.TabIndex = 16;
			this.txtFrameCount.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// grpControlMask
			// 
			this.grpControlMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.grpControlMask.AutoSize = true;
			this.grpControlMask.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpControlMask.Controls.Add(this.tableLayoutPanel9);
			this.grpControlMask.Location = new System.Drawing.Point(196, 0);
			this.grpControlMask.Margin = new System.Windows.Forms.Padding(0);
			this.grpControlMask.Name = "grpControlMask";
			this.grpControlMask.Size = new System.Drawing.Size(249, 138);
			this.grpControlMask.TabIndex = 1;
			this.grpControlMask.TabStop = false;
			this.grpControlMask.Text = "Control && Mask";
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel9.ColumnCount = 2;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.48536F));
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.51464F));
			this.tableLayoutPanel9.Controls.Add(this.chkDrawLeftSpr, 0, 5);
			this.tableLayoutPanel9.Controls.Add(this.flowLayoutPanel7, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.chkSpritesEnabled, 0, 3);
			this.tableLayoutPanel9.Controls.Add(this.chkBGEnabled, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.flowLayoutPanel6, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.chkDrawLeftBG, 0, 4);
			this.tableLayoutPanel9.Controls.Add(this.chkGrayscale, 1, 2);
			this.tableLayoutPanel9.Controls.Add(this.chkIntensifyRed, 1, 3);
			this.tableLayoutPanel9.Controls.Add(this.chkIntensifyGreen, 1, 4);
			this.tableLayoutPanel9.Controls.Add(this.chkIntensifyBlue, 1, 5);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 7;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(243, 119);
			this.tableLayoutPanel9.TabIndex = 1;
			// 
			// chkDrawLeftSpr
			// 
			this.chkDrawLeftSpr.AutoSize = true;
			this.chkDrawLeftSpr.Location = new System.Drawing.Point(0, 102);
			this.chkDrawLeftSpr.Margin = new System.Windows.Forms.Padding(0);
			this.chkDrawLeftSpr.Name = "chkDrawLeftSpr";
			this.chkDrawLeftSpr.Size = new System.Drawing.Size(129, 17);
			this.chkDrawLeftSpr.TabIndex = 23;
			this.chkDrawLeftSpr.Text = "Draw left Sprites (8px)";
			this.chkDrawLeftSpr.UseVisualStyleBackColor = true;
			this.chkDrawLeftSpr.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.AutoSize = true;
			this.flowLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel7.Controls.Add(this.lblSprAddr);
			this.flowLayoutPanel7.Controls.Add(this.txtSprAddr);
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 22);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(96, 22);
			this.flowLayoutPanel7.TabIndex = 22;
			// 
			// lblSprAddr
			// 
			this.lblSprAddr.AutoSize = true;
			this.lblSprAddr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblSprAddr.Location = new System.Drawing.Point(0, 0);
			this.lblSprAddr.Margin = new System.Windows.Forms.Padding(0);
			this.lblSprAddr.Name = "lblSprAddr";
			this.lblSprAddr.Size = new System.Drawing.Size(51, 22);
			this.lblSprAddr.TabIndex = 0;
			this.lblSprAddr.Text = "Spr Addr:";
			this.lblSprAddr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSprAddr
			// 
			this.txtSprAddr.Location = new System.Drawing.Point(51, 2);
			this.txtSprAddr.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.txtSprAddr.MaxLength = 4;
			this.txtSprAddr.Name = "txtSprAddr";
			this.txtSprAddr.Size = new System.Drawing.Size(45, 20);
			this.txtSprAddr.TabIndex = 1;
			this.txtSprAddr.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkSpritesEnabled
			// 
			this.chkSpritesEnabled.AutoSize = true;
			this.chkSpritesEnabled.Location = new System.Drawing.Point(0, 68);
			this.chkSpritesEnabled.Margin = new System.Windows.Forms.Padding(0);
			this.chkSpritesEnabled.Name = "chkSpritesEnabled";
			this.chkSpritesEnabled.Size = new System.Drawing.Size(100, 17);
			this.chkSpritesEnabled.TabIndex = 8;
			this.chkSpritesEnabled.Text = "Sprites Enabled";
			this.chkSpritesEnabled.UseVisualStyleBackColor = true;
			this.chkSpritesEnabled.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkBGEnabled
			// 
			this.chkBGEnabled.AutoSize = true;
			this.chkBGEnabled.Location = new System.Drawing.Point(0, 51);
			this.chkBGEnabled.Margin = new System.Windows.Forms.Padding(0);
			this.chkBGEnabled.Name = "chkBGEnabled";
			this.chkBGEnabled.Size = new System.Drawing.Size(83, 17);
			this.chkBGEnabled.TabIndex = 7;
			this.chkBGEnabled.Text = "BG Enabled";
			this.chkBGEnabled.UseVisualStyleBackColor = true;
			this.chkBGEnabled.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkNMIOnBlank, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkLargeSprites, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkVerticalWrite, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(137, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel9.SetRowSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(106, 51);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// chkNMIOnBlank
			// 
			this.chkNMIOnBlank.AutoSize = true;
			this.chkNMIOnBlank.Location = new System.Drawing.Point(0, 17);
			this.chkNMIOnBlank.Margin = new System.Windows.Forms.Padding(0);
			this.chkNMIOnBlank.Name = "chkNMIOnBlank";
			this.chkNMIOnBlank.Size = new System.Drawing.Size(97, 17);
			this.chkNMIOnBlank.TabIndex = 18;
			this.chkNMIOnBlank.Text = "NMI on vBlank";
			this.chkNMIOnBlank.UseVisualStyleBackColor = true;
			this.chkNMIOnBlank.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkLargeSprites
			// 
			this.chkLargeSprites.AutoSize = true;
			this.chkLargeSprites.Location = new System.Drawing.Point(0, 34);
			this.chkLargeSprites.Margin = new System.Windows.Forms.Padding(0);
			this.chkLargeSprites.Name = "chkLargeSprites";
			this.chkLargeSprites.Size = new System.Drawing.Size(88, 17);
			this.chkLargeSprites.TabIndex = 19;
			this.chkLargeSprites.Text = "Large Sprites";
			this.chkLargeSprites.UseVisualStyleBackColor = true;
			this.chkLargeSprites.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkVerticalWrite
			// 
			this.chkVerticalWrite.AutoSize = true;
			this.chkVerticalWrite.Location = new System.Drawing.Point(0, 0);
			this.chkVerticalWrite.Margin = new System.Windows.Forms.Padding(0);
			this.chkVerticalWrite.Name = "chkVerticalWrite";
			this.chkVerticalWrite.Size = new System.Drawing.Size(89, 17);
			this.chkVerticalWrite.TabIndex = 17;
			this.chkVerticalWrite.Text = "Vertical Write";
			this.chkVerticalWrite.UseVisualStyleBackColor = true;
			this.chkVerticalWrite.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel6.Controls.Add(this.lblBGAddr);
			this.flowLayoutPanel6.Controls.Add(this.txtBGAddr);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(96, 22);
			this.flowLayoutPanel6.TabIndex = 21;
			// 
			// lblBGAddr
			// 
			this.lblBGAddr.AutoSize = true;
			this.lblBGAddr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblBGAddr.Location = new System.Drawing.Point(0, 0);
			this.lblBGAddr.Margin = new System.Windows.Forms.Padding(0);
			this.lblBGAddr.Name = "lblBGAddr";
			this.lblBGAddr.Size = new System.Drawing.Size(50, 22);
			this.lblBGAddr.TabIndex = 0;
			this.lblBGAddr.Text = "BG Addr:";
			this.lblBGAddr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtBGAddr
			// 
			this.txtBGAddr.Location = new System.Drawing.Point(51, 2);
			this.txtBGAddr.Margin = new System.Windows.Forms.Padding(1, 2, 0, 0);
			this.txtBGAddr.MaxLength = 4;
			this.txtBGAddr.Name = "txtBGAddr";
			this.txtBGAddr.Size = new System.Drawing.Size(45, 20);
			this.txtBGAddr.TabIndex = 1;
			this.txtBGAddr.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkDrawLeftBG
			// 
			this.chkDrawLeftBG.AutoSize = true;
			this.chkDrawLeftBG.Location = new System.Drawing.Point(0, 85);
			this.chkDrawLeftBG.Margin = new System.Windows.Forms.Padding(0);
			this.chkDrawLeftBG.Name = "chkDrawLeftBG";
			this.chkDrawLeftBG.Size = new System.Drawing.Size(112, 17);
			this.chkDrawLeftBG.TabIndex = 13;
			this.chkDrawLeftBG.Text = "Draw left BG (8px)";
			this.chkDrawLeftBG.UseVisualStyleBackColor = true;
			this.chkDrawLeftBG.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkGrayscale
			// 
			this.chkGrayscale.AutoSize = true;
			this.chkGrayscale.Location = new System.Drawing.Point(137, 51);
			this.chkGrayscale.Margin = new System.Windows.Forms.Padding(0);
			this.chkGrayscale.Name = "chkGrayscale";
			this.chkGrayscale.Size = new System.Drawing.Size(73, 17);
			this.chkGrayscale.TabIndex = 12;
			this.chkGrayscale.Text = "Grayscale";
			this.chkGrayscale.UseVisualStyleBackColor = true;
			this.chkGrayscale.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkIntensifyRed
			// 
			this.chkIntensifyRed.AutoSize = true;
			this.chkIntensifyRed.Location = new System.Drawing.Point(137, 68);
			this.chkIntensifyRed.Margin = new System.Windows.Forms.Padding(0);
			this.chkIntensifyRed.Name = "chkIntensifyRed";
			this.chkIntensifyRed.Size = new System.Drawing.Size(88, 17);
			this.chkIntensifyRed.TabIndex = 16;
			this.chkIntensifyRed.Text = "Intensify Red";
			this.chkIntensifyRed.UseVisualStyleBackColor = true;
			this.chkIntensifyRed.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkIntensifyGreen
			// 
			this.chkIntensifyGreen.AutoSize = true;
			this.chkIntensifyGreen.Location = new System.Drawing.Point(137, 85);
			this.chkIntensifyGreen.Margin = new System.Windows.Forms.Padding(0);
			this.chkIntensifyGreen.Name = "chkIntensifyGreen";
			this.chkIntensifyGreen.Size = new System.Drawing.Size(97, 17);
			this.chkIntensifyGreen.TabIndex = 14;
			this.chkIntensifyGreen.Text = "Intensify Green";
			this.chkIntensifyGreen.UseVisualStyleBackColor = true;
			this.chkIntensifyGreen.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkIntensifyBlue
			// 
			this.chkIntensifyBlue.AutoSize = true;
			this.chkIntensifyBlue.Location = new System.Drawing.Point(137, 102);
			this.chkIntensifyBlue.Margin = new System.Windows.Forms.Padding(0);
			this.chkIntensifyBlue.Name = "chkIntensifyBlue";
			this.chkIntensifyBlue.Size = new System.Drawing.Size(89, 17);
			this.chkIntensifyBlue.TabIndex = 24;
			this.chkIntensifyBlue.Text = "Intensify Blue";
			this.chkIntensifyBlue.UseVisualStyleBackColor = true;
			this.chkIntensifyBlue.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// grpCPUStatus
			// 
			this.grpCPUStatus.Controls.Add(this.tableLayoutPanel3);
			this.grpCPUStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCPUStatus.Location = new System.Drawing.Point(1, 0);
			this.grpCPUStatus.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.grpCPUStatus.Name = "grpCPUStatus";
			this.grpCPUStatus.Size = new System.Drawing.Size(451, 128);
			this.grpCPUStatus.TabIndex = 0;
			this.grpCPUStatus.TabStop = false;
			this.grpCPUStatus.Text = "CPU Status";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.grpFlags, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpStack, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(445, 109);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// grpFlags
			// 
			this.grpFlags.Controls.Add(this.tableLayoutPanel4);
			this.grpFlags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFlags.Location = new System.Drawing.Point(0, 27);
			this.grpFlags.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.grpFlags.Name = "grpFlags";
			this.grpFlags.Padding = new System.Windows.Forms.Padding(2);
			this.grpFlags.Size = new System.Drawing.Size(356, 82);
			this.grpFlags.TabIndex = 3;
			this.grpFlags.TabStop = false;
			this.grpFlags.Text = "Flags";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 7;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.chkFrameCounter, 5, 3);
			this.tableLayoutPanel4.Controls.Add(this.chkExternal, 4, 3);
			this.tableLayoutPanel4.Controls.Add(this.chkNMI, 3, 3);
			this.tableLayoutPanel4.Controls.Add(this.chkDMC, 2, 3);
			this.tableLayoutPanel4.Controls.Add(this.chkBreak, 2, 1);
			this.tableLayoutPanel4.Controls.Add(this.chkNegative, 5, 1);
			this.tableLayoutPanel4.Controls.Add(this.chkOverflow, 4, 1);
			this.tableLayoutPanel4.Controls.Add(this.chkDecimal, 5, 0);
			this.tableLayoutPanel4.Controls.Add(this.chkReserved, 3, 1);
			this.tableLayoutPanel4.Controls.Add(this.txtStatus, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.chkInterrupt, 4, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblStatus, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.chkZero, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.chkCarry, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblIrqs, 0, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 15);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 5;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(352, 65);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// chkFrameCounter
			// 
			this.chkFrameCounter.AutoSize = true;
			this.chkFrameCounter.Location = new System.Drawing.Point(243, 42);
			this.chkFrameCounter.Margin = new System.Windows.Forms.Padding(0);
			this.chkFrameCounter.Name = "chkFrameCounter";
			this.chkFrameCounter.Size = new System.Drawing.Size(95, 17);
			this.chkFrameCounter.TabIndex = 2;
			this.chkFrameCounter.Text = "Frame Counter";
			this.chkFrameCounter.UseVisualStyleBackColor = true;
			this.chkFrameCounter.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkExternal
			// 
			this.chkExternal.AutoSize = true;
			this.chkExternal.Location = new System.Drawing.Point(175, 42);
			this.chkExternal.Margin = new System.Windows.Forms.Padding(0);
			this.chkExternal.Name = "chkExternal";
			this.chkExternal.Size = new System.Drawing.Size(64, 17);
			this.chkExternal.TabIndex = 1;
			this.chkExternal.Text = "External";
			this.chkExternal.UseVisualStyleBackColor = true;
			this.chkExternal.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkNMI
			// 
			this.chkNMI.AutoSize = true;
			this.chkNMI.Location = new System.Drawing.Point(103, 42);
			this.chkNMI.Margin = new System.Windows.Forms.Padding(0);
			this.chkNMI.Name = "chkNMI";
			this.chkNMI.Size = new System.Drawing.Size(46, 17);
			this.chkNMI.TabIndex = 4;
			this.chkNMI.Text = "NMI";
			this.chkNMI.UseVisualStyleBackColor = true;
			this.chkNMI.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkDMC
			// 
			this.chkDMC.AutoSize = true;
			this.chkDMC.Location = new System.Drawing.Point(49, 42);
			this.chkDMC.Margin = new System.Windows.Forms.Padding(0);
			this.chkDMC.Name = "chkDMC";
			this.chkDMC.Size = new System.Drawing.Size(50, 17);
			this.chkDMC.TabIndex = 3;
			this.chkDMC.Text = "DMC";
			this.chkDMC.UseVisualStyleBackColor = true;
			this.chkDMC.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkBreak
			// 
			this.chkBreak.AutoSize = true;
			this.chkBreak.Enabled = false;
			this.chkBreak.Location = new System.Drawing.Point(49, 17);
			this.chkBreak.Margin = new System.Windows.Forms.Padding(0);
			this.chkBreak.Name = "chkBreak";
			this.chkBreak.Size = new System.Drawing.Size(54, 17);
			this.chkBreak.TabIndex = 4;
			this.chkBreak.Text = "Break";
			this.chkBreak.UseVisualStyleBackColor = true;
			this.chkBreak.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkNegative
			// 
			this.chkNegative.AutoSize = true;
			this.chkNegative.Location = new System.Drawing.Point(243, 17);
			this.chkNegative.Margin = new System.Windows.Forms.Padding(0);
			this.chkNegative.Name = "chkNegative";
			this.chkNegative.Size = new System.Drawing.Size(69, 17);
			this.chkNegative.TabIndex = 7;
			this.chkNegative.Text = "Negative";
			this.chkNegative.UseVisualStyleBackColor = true;
			this.chkNegative.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkOverflow
			// 
			this.chkOverflow.AutoSize = true;
			this.chkOverflow.Location = new System.Drawing.Point(175, 17);
			this.chkOverflow.Margin = new System.Windows.Forms.Padding(0);
			this.chkOverflow.Name = "chkOverflow";
			this.chkOverflow.Size = new System.Drawing.Size(68, 17);
			this.chkOverflow.TabIndex = 6;
			this.chkOverflow.Text = "Overflow";
			this.chkOverflow.UseVisualStyleBackColor = true;
			this.chkOverflow.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkDecimal
			// 
			this.chkDecimal.AutoSize = true;
			this.chkDecimal.Location = new System.Drawing.Point(243, 0);
			this.chkDecimal.Margin = new System.Windows.Forms.Padding(0);
			this.chkDecimal.Name = "chkDecimal";
			this.chkDecimal.Size = new System.Drawing.Size(63, 17);
			this.chkDecimal.TabIndex = 3;
			this.chkDecimal.Text = "Unused";
			this.chkDecimal.UseVisualStyleBackColor = true;
			this.chkDecimal.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkReserved
			// 
			this.chkReserved.AutoSize = true;
			this.chkReserved.Enabled = false;
			this.chkReserved.Location = new System.Drawing.Point(103, 17);
			this.chkReserved.Margin = new System.Windows.Forms.Padding(0);
			this.chkReserved.Name = "chkReserved";
			this.chkReserved.Size = new System.Drawing.Size(72, 17);
			this.chkReserved.TabIndex = 5;
			this.chkReserved.Text = "Reserved";
			this.chkReserved.UseVisualStyleBackColor = true;
			this.chkReserved.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// txtStatus
			// 
			this.txtStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtStatus.Location = new System.Drawing.Point(17, 7);
			this.txtStatus.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.txtStatus.MaxLength = 2;
			this.txtStatus.Name = "txtStatus";
			this.tableLayoutPanel4.SetRowSpan(this.txtStatus, 2);
			this.txtStatus.Size = new System.Drawing.Size(27, 20);
			this.txtStatus.TabIndex = 1;
			this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
			// 
			// chkInterrupt
			// 
			this.chkInterrupt.AutoSize = true;
			this.chkInterrupt.Location = new System.Drawing.Point(175, 0);
			this.chkInterrupt.Margin = new System.Windows.Forms.Padding(0);
			this.chkInterrupt.Name = "chkInterrupt";
			this.chkInterrupt.Size = new System.Drawing.Size(65, 17);
			this.chkInterrupt.TabIndex = 2;
			this.chkInterrupt.Text = "Interrupt";
			this.chkInterrupt.UseVisualStyleBackColor = true;
			this.chkInterrupt.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(0, 10);
			this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
			this.lblStatus.Name = "lblStatus";
			this.tableLayoutPanel4.SetRowSpan(this.lblStatus, 2);
			this.lblStatus.Size = new System.Drawing.Size(17, 13);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "P:";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkZero
			// 
			this.chkZero.AutoSize = true;
			this.chkZero.Location = new System.Drawing.Point(103, 0);
			this.chkZero.Margin = new System.Windows.Forms.Padding(0);
			this.chkZero.Name = "chkZero";
			this.chkZero.Size = new System.Drawing.Size(48, 17);
			this.chkZero.TabIndex = 1;
			this.chkZero.Text = "Zero";
			this.chkZero.UseVisualStyleBackColor = true;
			this.chkZero.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkCarry
			// 
			this.chkCarry.AutoSize = true;
			this.chkCarry.Location = new System.Drawing.Point(49, 0);
			this.chkCarry.Margin = new System.Windows.Forms.Padding(0);
			this.chkCarry.Name = "chkCarry";
			this.chkCarry.Size = new System.Drawing.Size(50, 17);
			this.chkCarry.TabIndex = 0;
			this.chkCarry.Text = "Carry";
			this.chkCarry.UseVisualStyleBackColor = true;
			this.chkCarry.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// lblIrqs
			// 
			this.lblIrqs.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.lblIrqs, 2);
			this.lblIrqs.Location = new System.Drawing.Point(3, 42);
			this.lblIrqs.Name = "lblIrqs";
			this.lblIrqs.Size = new System.Drawing.Size(34, 13);
			this.lblIrqs.TabIndex = 0;
			this.lblIrqs.Text = "IRQs:";
			// 
			// grpStack
			// 
			this.grpStack.Controls.Add(this.tableLayoutPanel6);
			this.grpStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpStack.Location = new System.Drawing.Point(358, 0);
			this.grpStack.Margin = new System.Windows.Forms.Padding(0);
			this.grpStack.Name = "grpStack";
			this.grpStack.Padding = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel3.SetRowSpan(this.grpStack, 3);
			this.grpStack.Size = new System.Drawing.Size(87, 109);
			this.grpStack.TabIndex = 1;
			this.grpStack.TabStop = false;
			this.grpStack.Text = "Stack";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel4, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.txtStack, 0, 1);
			this.tableLayoutPanel6.Location = new System.Drawing.Point(2, 15);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(83, 89);
			this.tableLayoutPanel6.TabIndex = 1;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.lblSP);
			this.flowLayoutPanel4.Controls.Add(this.txtSP);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(83, 21);
			this.flowLayoutPanel4.TabIndex = 3;
			// 
			// lblSP
			// 
			this.lblSP.AutoSize = true;
			this.lblSP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblSP.Location = new System.Drawing.Point(0, 0);
			this.lblSP.Margin = new System.Windows.Forms.Padding(0);
			this.lblSP.Name = "lblSP";
			this.lblSP.Size = new System.Drawing.Size(24, 20);
			this.lblSP.TabIndex = 0;
			this.lblSP.Text = "SP:";
			this.lblSP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSP
			// 
			this.txtSP.Location = new System.Drawing.Point(24, 0);
			this.txtSP.Margin = new System.Windows.Forms.Padding(0);
			this.txtSP.MaxLength = 2;
			this.txtSP.Name = "txtSP";
			this.txtSP.Size = new System.Drawing.Size(39, 20);
			this.txtSP.TabIndex = 1;
			this.txtSP.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// txtStack
			// 
			this.txtStack.BackColor = System.Drawing.SystemColors.Window;
			this.txtStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtStack.Location = new System.Drawing.Point(0, 21);
			this.txtStack.Margin = new System.Windows.Forms.Padding(0);
			this.txtStack.Multiline = true;
			this.txtStack.Name = "txtStack";
			this.txtStack.ReadOnly = true;
			this.txtStack.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtStack.Size = new System.Drawing.Size(83, 68);
			this.txtStack.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblA);
			this.flowLayoutPanel1.Controls.Add(this.txtA);
			this.flowLayoutPanel1.Controls.Add(this.lblX);
			this.flowLayoutPanel1.Controls.Add(this.txtX);
			this.flowLayoutPanel1.Controls.Add(this.lblY);
			this.flowLayoutPanel1.Controls.Add(this.txtY);
			this.flowLayoutPanel1.Controls.Add(this.lblPC);
			this.flowLayoutPanel1.Controls.Add(this.txtPC);
			this.flowLayoutPanel1.Controls.Add(this.lblCycleCount);
			this.flowLayoutPanel1.Controls.Add(this.txtCycleCount);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(352, 21);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// lblA
			// 
			this.lblA.AutoSize = true;
			this.lblA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblA.Location = new System.Drawing.Point(0, 0);
			this.lblA.Margin = new System.Windows.Forms.Padding(0);
			this.lblA.Name = "lblA";
			this.lblA.Size = new System.Drawing.Size(17, 20);
			this.lblA.TabIndex = 0;
			this.lblA.Text = "A:";
			this.lblA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtA
			// 
			this.txtA.Location = new System.Drawing.Point(17, 0);
			this.txtA.Margin = new System.Windows.Forms.Padding(0);
			this.txtA.MaxLength = 2;
			this.txtA.Name = "txtA";
			this.txtA.Size = new System.Drawing.Size(27, 20);
			this.txtA.TabIndex = 1;
			this.txtA.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblX
			// 
			this.lblX.AutoSize = true;
			this.lblX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblX.Location = new System.Drawing.Point(44, 0);
			this.lblX.Margin = new System.Windows.Forms.Padding(0);
			this.lblX.Name = "lblX";
			this.lblX.Size = new System.Drawing.Size(17, 20);
			this.lblX.TabIndex = 2;
			this.lblX.Text = "X:";
			this.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(61, 0);
			this.txtX.Margin = new System.Windows.Forms.Padding(0);
			this.txtX.MaxLength = 2;
			this.txtX.Name = "txtX";
			this.txtX.Size = new System.Drawing.Size(27, 20);
			this.txtX.TabIndex = 3;
			this.txtX.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblY
			// 
			this.lblY.AutoSize = true;
			this.lblY.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblY.Location = new System.Drawing.Point(88, 0);
			this.lblY.Margin = new System.Windows.Forms.Padding(0);
			this.lblY.Name = "lblY";
			this.lblY.Size = new System.Drawing.Size(17, 20);
			this.lblY.TabIndex = 4;
			this.lblY.Text = "Y:";
			this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtY
			// 
			this.txtY.Location = new System.Drawing.Point(105, 0);
			this.txtY.Margin = new System.Windows.Forms.Padding(0);
			this.txtY.MaxLength = 2;
			this.txtY.Name = "txtY";
			this.txtY.Size = new System.Drawing.Size(27, 20);
			this.txtY.TabIndex = 5;
			this.txtY.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblPC
			// 
			this.lblPC.AutoSize = true;
			this.lblPC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblPC.Location = new System.Drawing.Point(132, 0);
			this.lblPC.Margin = new System.Windows.Forms.Padding(0);
			this.lblPC.Name = "lblPC";
			this.lblPC.Size = new System.Drawing.Size(24, 20);
			this.lblPC.TabIndex = 6;
			this.lblPC.Text = "PC:";
			this.lblPC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPC
			// 
			this.txtPC.Location = new System.Drawing.Point(156, 0);
			this.txtPC.Margin = new System.Windows.Forms.Padding(0);
			this.txtPC.MaxLength = 4;
			this.txtPC.Name = "txtPC";
			this.txtPC.Size = new System.Drawing.Size(42, 20);
			this.txtPC.TabIndex = 7;
			this.txtPC.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblCycleCount
			// 
			this.lblCycleCount.AutoSize = true;
			this.lblCycleCount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCycleCount.Location = new System.Drawing.Point(198, 0);
			this.lblCycleCount.Margin = new System.Windows.Forms.Padding(0);
			this.lblCycleCount.Name = "lblCycleCount";
			this.lblCycleCount.Size = new System.Drawing.Size(36, 20);
			this.lblCycleCount.TabIndex = 8;
			this.lblCycleCount.Text = "Cycle:";
			this.lblCycleCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtCycleCount
			// 
			this.txtCycleCount.Location = new System.Drawing.Point(234, 0);
			this.txtCycleCount.Margin = new System.Windows.Forms.Padding(0);
			this.txtCycleCount.MaxLength = 11;
			this.txtCycleCount.Name = "txtCycleCount";
			this.txtCycleCount.Size = new System.Drawing.Size(85, 20);
			this.txtCycleCount.TabIndex = 9;
			this.txtCycleCount.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 3;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.Controls.Add(this.btnGoto, 2, 0);
			this.tableLayoutPanel10.Controls.Add(this.btnUndo, 0, 0);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 368);
			this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 2;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(453, 47);
			this.tableLayoutPanel10.TabIndex = 3;
			// 
			// btnGoto
			// 
			this.btnGoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGoto.AutoSize = true;
			this.btnGoto.Image = global::Mesen.GUI.Properties.Resources.DownArrow;
			this.btnGoto.Location = new System.Drawing.Point(378, 3);
			this.btnGoto.Name = "btnGoto";
			this.btnGoto.Size = new System.Drawing.Size(72, 23);
			this.btnGoto.TabIndex = 4;
			this.btnGoto.Text = "Go To...";
			this.btnGoto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnGoto.UseVisualStyleBackColor = true;
			this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.AutoSize = true;
			this.btnUndo.Location = new System.Drawing.Point(3, 3);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(87, 23);
			this.btnUndo.TabIndex = 3;
			this.btnUndo.Text = "Undo changes";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// grpInputStatus
			// 
			this.grpInputStatus.Controls.Add(this.tableLayoutPanel11);
			this.grpInputStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpInputStatus.Location = new System.Drawing.Point(1, 301);
			this.grpInputStatus.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.grpInputStatus.Name = "grpInputStatus";
			this.grpInputStatus.Size = new System.Drawing.Size(451, 67);
			this.grpInputStatus.TabIndex = 4;
			this.grpInputStatus.TabStop = false;
			this.grpInputStatus.Text = "Input Button Setup";
			// 
			// tableLayoutPanel11
			// 
			this.tableLayoutPanel11.ColumnCount = 4;
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel11.Controls.Add(this.ctrlControllerInput3, 2, 0);
			this.tableLayoutPanel11.Controls.Add(this.ctrlControllerInput4, 3, 0);
			this.tableLayoutPanel11.Controls.Add(this.ctrlControllerInput1, 0, 0);
			this.tableLayoutPanel11.Controls.Add(this.ctrlControllerInput2, 1, 0);
			this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			this.tableLayoutPanel11.RowCount = 1;
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Size = new System.Drawing.Size(445, 48);
			this.tableLayoutPanel11.TabIndex = 0;
			// 
			// ctrlControllerInput3
			// 
			this.ctrlControllerInput3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlControllerInput3.Location = new System.Drawing.Point(233, 8);
			this.ctrlControllerInput3.Name = "ctrlControllerInput3";
			this.ctrlControllerInput3.PlayerNumber = 2;
			this.ctrlControllerInput3.Size = new System.Drawing.Size(88, 32);
			this.ctrlControllerInput3.TabIndex = 3;
			this.ctrlControllerInput3.Text = "ctrlControllerInput4";
			// 
			// ctrlControllerInput4
			// 
			this.ctrlControllerInput4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlControllerInput4.Location = new System.Drawing.Point(345, 8);
			this.ctrlControllerInput4.Name = "ctrlControllerInput4";
			this.ctrlControllerInput4.PlayerNumber = 3;
			this.ctrlControllerInput4.Size = new System.Drawing.Size(88, 32);
			this.ctrlControllerInput4.TabIndex = 2;
			this.ctrlControllerInput4.Text = "ctrlControllerInput3";
			// 
			// ctrlControllerInput1
			// 
			this.ctrlControllerInput1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlControllerInput1.Location = new System.Drawing.Point(11, 8);
			this.ctrlControllerInput1.Name = "ctrlControllerInput1";
			this.ctrlControllerInput1.PlayerNumber = 0;
			this.ctrlControllerInput1.Size = new System.Drawing.Size(88, 32);
			this.ctrlControllerInput1.TabIndex = 1;
			this.ctrlControllerInput1.Text = "ctrlControllerInput2";
			// 
			// ctrlControllerInput2
			// 
			this.ctrlControllerInput2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlControllerInput2.Location = new System.Drawing.Point(122, 8);
			this.ctrlControllerInput2.Name = "ctrlControllerInput2";
			this.ctrlControllerInput2.PlayerNumber = 1;
			this.ctrlControllerInput2.Size = new System.Drawing.Size(88, 32);
			this.ctrlControllerInput2.TabIndex = 0;
			this.ctrlControllerInput2.Text = "ctrlControllerInput1";
			// 
			// tmrButton
			// 
			this.tmrButton.Tick += new System.EventHandler(this.tmrButton_Tick);
			// 
			// contextGoTo
			// 
			this.contextGoTo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToIrqHandler,
            this.mnuGoToNmiHandler,
            this.mnuGoToResetHandler,
            this.mnuGoToInitHandler,
            this.mnuGoToPlayHandler,
            this.sepFds,
            this.mnuFdsIrqHandler,
            this.mnuFdsNmiHandler1,
            this.mnuFdsNmiHandler2,
            this.mnuFdsNmiHandler3,
            this.mnuFdsResetHandler,
            this.toolStripMenuItem1,
            this.mnuGoToProgramCounter});
			this.contextGoTo.Name = "contextGoTo";
			this.contextGoTo.Size = new System.Drawing.Size(182, 280);
			this.contextGoTo.Opening += new System.ComponentModel.CancelEventHandler(this.contextGoTo_Opening);
			// 
			// mnuGoToIrqHandler
			// 
			this.mnuGoToIrqHandler.Name = "mnuGoToIrqHandler";
			this.mnuGoToIrqHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToIrqHandler.Text = "IRQ Handler";
			this.mnuGoToIrqHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuGoToNmiHandler
			// 
			this.mnuGoToNmiHandler.Name = "mnuGoToNmiHandler";
			this.mnuGoToNmiHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToNmiHandler.Text = "NMI Handler";
			this.mnuGoToNmiHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuGoToResetHandler
			// 
			this.mnuGoToResetHandler.Name = "mnuGoToResetHandler";
			this.mnuGoToResetHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToResetHandler.Text = "Reset Handler";
			this.mnuGoToResetHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuGoToInitHandler
			// 
			this.mnuGoToInitHandler.Name = "mnuGoToInitHandler";
			this.mnuGoToInitHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToInitHandler.Text = "Init Handler";
			this.mnuGoToInitHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuGoToPlayHandler
			// 
			this.mnuGoToPlayHandler.Name = "mnuGoToPlayHandler";
			this.mnuGoToPlayHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToPlayHandler.Text = "Play Handler";
			this.mnuGoToPlayHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// sepFds
			// 
			this.sepFds.Name = "sepFds";
			this.sepFds.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuFdsIrqHandler
			// 
			this.mnuFdsIrqHandler.Name = "mnuFdsIrqHandler";
			this.mnuFdsIrqHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuFdsIrqHandler.Text = "FDS IRQ Handler";
			this.mnuFdsIrqHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuFdsNmiHandler1
			// 
			this.mnuFdsNmiHandler1.Name = "mnuFdsNmiHandler1";
			this.mnuFdsNmiHandler1.Size = new System.Drawing.Size(181, 22);
			this.mnuFdsNmiHandler1.Text = "FDS NMI Handler #1";
			this.mnuFdsNmiHandler1.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuFdsNmiHandler2
			// 
			this.mnuFdsNmiHandler2.Name = "mnuFdsNmiHandler2";
			this.mnuFdsNmiHandler2.Size = new System.Drawing.Size(181, 22);
			this.mnuFdsNmiHandler2.Text = "FDS NMI Handler #2";
			this.mnuFdsNmiHandler2.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuFdsNmiHandler3
			// 
			this.mnuFdsNmiHandler3.Name = "mnuFdsNmiHandler3";
			this.mnuFdsNmiHandler3.Size = new System.Drawing.Size(181, 22);
			this.mnuFdsNmiHandler3.Text = "FDS NMI Handler #3";
			this.mnuFdsNmiHandler3.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// mnuFdsResetHandler
			// 
			this.mnuFdsResetHandler.Name = "mnuFdsResetHandler";
			this.mnuFdsResetHandler.Size = new System.Drawing.Size(181, 22);
			this.mnuFdsResetHandler.Text = "FDS Reset Handler";
			this.mnuFdsResetHandler.Click += new System.EventHandler(this.mnuGoToHandler_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuGoToProgramCounter
			// 
			this.mnuGoToProgramCounter.Name = "mnuGoToProgramCounter";
			this.mnuGoToProgramCounter.Size = new System.Drawing.Size(181, 22);
			this.mnuGoToProgramCounter.Text = "Program Counter";
			this.mnuGoToProgramCounter.ToolTipText = "Alt+*";
			this.mnuGoToProgramCounter.Click += new System.EventHandler(this.mnuGoToProgramCounter_Click);
			// 
			// ctrlConsoleStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel2);
			this.Name = "ctrlConsoleStatus";
			this.Size = new System.Drawing.Size(453, 415);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.grpPPUStatus.ResumeLayout(false);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.grpControlMask.ResumeLayout(false);
			this.grpControlMask.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.grpCPUStatus.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpFlags.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.grpStack.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.grpInputStatus.ResumeLayout(false);
			this.tableLayoutPanel11.ResumeLayout(false);
			this.contextGoTo.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.GroupBox grpPPUStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Label lblVRAMAddr;
		private System.Windows.Forms.Label lblCycle;
		private System.Windows.Forms.TextBox txtCycle;
		private System.Windows.Forms.Label lblScanline;
		private System.Windows.Forms.TextBox txtScanline;
		private System.Windows.Forms.TextBox txtVRAMAddr;
		private System.Windows.Forms.CheckBox chkVerticalBlank;
		private System.Windows.Forms.CheckBox chkSprite0Hit;
		private System.Windows.Forms.CheckBox chkSpriteOverflow;
		private System.Windows.Forms.GroupBox grpControlMask;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.CheckBox chkDrawLeftSpr;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.Label lblSprAddr;
		private System.Windows.Forms.TextBox txtSprAddr;
		private System.Windows.Forms.CheckBox chkSpritesEnabled;
		private System.Windows.Forms.CheckBox chkBGEnabled;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblBGAddr;
		private System.Windows.Forms.TextBox txtBGAddr;
		private System.Windows.Forms.CheckBox chkDrawLeftBG;
		private System.Windows.Forms.CheckBox chkGrayscale;
		private System.Windows.Forms.CheckBox chkIntensifyRed;
		private System.Windows.Forms.CheckBox chkIntensifyGreen;
		private System.Windows.Forms.CheckBox chkVerticalWrite;
		private System.Windows.Forms.CheckBox chkNMIOnBlank;
		private System.Windows.Forms.CheckBox chkLargeSprites;
		private System.Windows.Forms.Label lblNTAddr;
		private System.Windows.Forms.TextBox txtNTAddr;
		private System.Windows.Forms.CheckBox chkIntensifyBlue;
		private System.Windows.Forms.GroupBox grpCPUStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox grpFlags;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.CheckBox chkNegative;
		private System.Windows.Forms.CheckBox chkOverflow;
		private System.Windows.Forms.CheckBox chkDecimal;
		private System.Windows.Forms.CheckBox chkInterrupt;
		private System.Windows.Forms.CheckBox chkZero;
		private System.Windows.Forms.CheckBox chkCarry;
		private System.Windows.Forms.GroupBox grpStack;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblSP;
		private System.Windows.Forms.TextBox txtSP;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblA;
		private System.Windows.Forms.TextBox txtA;
		private System.Windows.Forms.Label lblX;
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.Label lblY;
		private System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.Label lblPC;
		private System.Windows.Forms.TextBox txtPC;
		private System.Windows.Forms.Label lblCycleCount;
		private System.Windows.Forms.TextBox txtCycleCount;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.Timer tmrButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.Button btnGoto;
		private System.Windows.Forms.ContextMenuStrip contextGoTo;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToIrqHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToNmiHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToResetHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToInitHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToPlayHandler;
		private System.Windows.Forms.GroupBox grpInputStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
		private Controls.ctrlControllerInput ctrlControllerInput2;
		private Controls.ctrlControllerInput ctrlControllerInput3;
		private Controls.ctrlControllerInput ctrlControllerInput4;
		private Controls.ctrlControllerInput ctrlControllerInput1;
		private System.Windows.Forms.CheckBox chkFrameCounter;
		private System.Windows.Forms.CheckBox chkExternal;
		private System.Windows.Forms.CheckBox chkNMI;
		private System.Windows.Forms.CheckBox chkDMC;
		private System.Windows.Forms.Label lblIrqs;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.CheckBox chkWriteToggle;
		private System.Windows.Forms.Label lblXScroll;
		private System.Windows.Forms.TextBox txtXScroll;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTmpAddr;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToProgramCounter;
		private System.Windows.Forms.TextBox txtStack;
		private System.Windows.Forms.CheckBox chkBreak;
		private System.Windows.Forms.CheckBox chkReserved;
		private System.Windows.Forms.Label lblFrameCount;
		private System.Windows.Forms.TextBox txtFrameCount;
	  private System.Windows.Forms.ToolStripSeparator sepFds;
	  private System.Windows.Forms.ToolStripMenuItem mnuFdsNmiHandler3;
	  private System.Windows.Forms.ToolStripMenuItem mnuFdsIrqHandler;
	  private System.Windows.Forms.ToolStripMenuItem mnuFdsResetHandler;
	  private System.Windows.Forms.ToolStripMenuItem mnuFdsNmiHandler1;
	  private System.Windows.Forms.ToolStripMenuItem mnuFdsNmiHandler2;
   }
}
