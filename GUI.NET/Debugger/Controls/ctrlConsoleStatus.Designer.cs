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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlConsoleStatus));
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
			this.chkVerticalBlank = new System.Windows.Forms.CheckBox();
			this.chkSprite0Hit = new System.Windows.Forms.CheckBox();
			this.chkSpriteOverflow = new System.Windows.Forms.CheckBox();
			this.lblScanline = new System.Windows.Forms.Label();
			this.txtScanline = new System.Windows.Forms.TextBox();
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
			this.grpIRQ = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkExternal = new System.Windows.Forms.CheckBox();
			this.chkFrameCounter = new System.Windows.Forms.CheckBox();
			this.chkDMC = new System.Windows.Forms.CheckBox();
			this.chkNMI = new System.Windows.Forms.CheckBox();
			this.grpFlags = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblStatus = new System.Windows.Forms.Label();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.chkNegative = new System.Windows.Forms.CheckBox();
			this.chkOverflow = new System.Windows.Forms.CheckBox();
			this.chkReserved = new System.Windows.Forms.CheckBox();
			this.chkBreak = new System.Windows.Forms.CheckBox();
			this.chkDecimal = new System.Windows.Forms.CheckBox();
			this.chkInterrupt = new System.Windows.Forms.CheckBox();
			this.chkZero = new System.Windows.Forms.CheckBox();
			this.chkCarry = new System.Windows.Forms.CheckBox();
			this.grpStack = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblSP = new System.Windows.Forms.Label();
			this.txtSP = new System.Windows.Forms.TextBox();
			this.lstStack = new Mesen.GUI.Controls.DoubleBufferedListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
			this.tmrButton = new System.Windows.Forms.Timer(this.components);
			this.contextGoTo = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuGoToIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToNmiHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpPPUStatus.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.grpControlMask.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.grpCPUStatus.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpIRQ.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.grpFlags.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.grpStack.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.contextGoTo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.grpPPUStatus, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.grpCPUStatus, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel10, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 391);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// grpPPUStatus
			// 
			this.grpPPUStatus.Controls.Add(this.tableLayoutPanel8);
			this.grpPPUStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpPPUStatus.Location = new System.Drawing.Point(3, 180);
			this.grpPPUStatus.Name = "grpPPUStatus";
			this.grpPPUStatus.Size = new System.Drawing.Size(464, 179);
			this.grpPPUStatus.TabIndex = 2;
			this.grpPPUStatus.TabStop = false;
			this.grpPPUStatus.Text = "PPU Status";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.25163F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.74837F));
			this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel7, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.grpControlMask, 1, 0);
			this.tableLayoutPanel8.Location = new System.Drawing.Point(6, 19);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 1;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(455, 157);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add(this.txtNTAddr, 1, 3);
			this.tableLayoutPanel7.Controls.Add(this.lblNTAddr, 0, 3);
			this.tableLayoutPanel7.Controls.Add(this.lblVRAMAddr, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.lblCycle, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.txtCycle, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.txtVRAMAddr, 1, 2);
			this.tableLayoutPanel7.Controls.Add(this.chkVerticalBlank, 0, 4);
			this.tableLayoutPanel7.Controls.Add(this.chkSprite0Hit, 0, 5);
			this.tableLayoutPanel7.Controls.Add(this.chkSpriteOverflow, 0, 6);
			this.tableLayoutPanel7.Controls.Add(this.lblScanline, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.txtScanline, 1, 1);
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 7;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(195, 151);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// txtNTAddr
			// 
			this.txtNTAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtNTAddr.Location = new System.Drawing.Point(68, 74);
			this.txtNTAddr.Margin = new System.Windows.Forms.Padding(2);
			this.txtNTAddr.Name = "txtNTAddr";
			this.txtNTAddr.ReadOnly = true;
			this.txtNTAddr.Size = new System.Drawing.Size(58, 20);
			this.txtNTAddr.TabIndex = 1;
			// 
			// lblNTAddr
			// 
			this.lblNTAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNTAddr.AutoSize = true;
			this.lblNTAddr.Location = new System.Drawing.Point(0, 77);
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
			this.lblVRAMAddr.Location = new System.Drawing.Point(0, 53);
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
			this.lblCycle.Location = new System.Drawing.Point(0, 5);
			this.lblCycle.Margin = new System.Windows.Forms.Padding(0);
			this.lblCycle.Name = "lblCycle";
			this.lblCycle.Size = new System.Drawing.Size(36, 13);
			this.lblCycle.TabIndex = 1;
			this.lblCycle.Text = "Cycle:";
			this.lblCycle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtCycle
			// 
			this.txtCycle.Location = new System.Drawing.Point(68, 2);
			this.txtCycle.Margin = new System.Windows.Forms.Padding(2);
			this.txtCycle.MaxLength = 3;
			this.txtCycle.Name = "txtCycle";
			this.txtCycle.Size = new System.Drawing.Size(58, 20);
			this.txtCycle.TabIndex = 2;
			this.txtCycle.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// txtVRAMAddr
			// 
			this.txtVRAMAddr.Location = new System.Drawing.Point(68, 50);
			this.txtVRAMAddr.Margin = new System.Windows.Forms.Padding(2);
			this.txtVRAMAddr.Name = "txtVRAMAddr";
			this.txtVRAMAddr.Size = new System.Drawing.Size(58, 20);
			this.txtVRAMAddr.TabIndex = 6;
			this.txtVRAMAddr.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkVerticalBlank
			// 
			this.chkVerticalBlank.AutoSize = true;
			this.tableLayoutPanel7.SetColumnSpan(this.chkVerticalBlank, 2);
			this.chkVerticalBlank.Location = new System.Drawing.Point(0, 98);
			this.chkVerticalBlank.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.chkVerticalBlank.Name = "chkVerticalBlank";
			this.chkVerticalBlank.Size = new System.Drawing.Size(91, 17);
			this.chkVerticalBlank.TabIndex = 7;
			this.chkVerticalBlank.Text = "Vertical Blank";
			this.chkVerticalBlank.UseVisualStyleBackColor = true;
			this.chkVerticalBlank.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkSprite0Hit
			// 
			this.chkSprite0Hit.AutoSize = true;
			this.tableLayoutPanel7.SetColumnSpan(this.chkSprite0Hit, 2);
			this.chkSprite0Hit.Location = new System.Drawing.Point(0, 115);
			this.chkSprite0Hit.Margin = new System.Windows.Forms.Padding(0);
			this.chkSprite0Hit.Name = "chkSprite0Hit";
			this.chkSprite0Hit.Size = new System.Drawing.Size(78, 17);
			this.chkSprite0Hit.TabIndex = 9;
			this.chkSprite0Hit.Text = "Sprite 0 Hit";
			this.chkSprite0Hit.UseVisualStyleBackColor = true;
			this.chkSprite0Hit.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkSpriteOverflow
			// 
			this.chkSpriteOverflow.AutoSize = true;
			this.tableLayoutPanel7.SetColumnSpan(this.chkSpriteOverflow, 2);
			this.chkSpriteOverflow.Location = new System.Drawing.Point(0, 132);
			this.chkSpriteOverflow.Margin = new System.Windows.Forms.Padding(0);
			this.chkSpriteOverflow.Name = "chkSpriteOverflow";
			this.chkSpriteOverflow.Size = new System.Drawing.Size(98, 17);
			this.chkSpriteOverflow.TabIndex = 8;
			this.chkSpriteOverflow.Text = "Sprite Overflow";
			this.chkSpriteOverflow.UseVisualStyleBackColor = true;
			this.chkSpriteOverflow.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// lblScanline
			// 
			this.lblScanline.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblScanline.AutoSize = true;
			this.lblScanline.Location = new System.Drawing.Point(0, 29);
			this.lblScanline.Margin = new System.Windows.Forms.Padding(0);
			this.lblScanline.Name = "lblScanline";
			this.lblScanline.Size = new System.Drawing.Size(51, 13);
			this.lblScanline.TabIndex = 3;
			this.lblScanline.Text = "Scanline:";
			this.lblScanline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtScanline
			// 
			this.txtScanline.Location = new System.Drawing.Point(68, 26);
			this.txtScanline.Margin = new System.Windows.Forms.Padding(2);
			this.txtScanline.MaxLength = 3;
			this.txtScanline.Name = "txtScanline";
			this.txtScanline.Size = new System.Drawing.Size(58, 20);
			this.txtScanline.TabIndex = 4;
			this.txtScanline.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// grpControlMask
			// 
			this.grpControlMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.grpControlMask.Controls.Add(this.tableLayoutPanel9);
			this.grpControlMask.Location = new System.Drawing.Point(210, 0);
			this.grpControlMask.Margin = new System.Windows.Forms.Padding(0);
			this.grpControlMask.Name = "grpControlMask";
			this.grpControlMask.Size = new System.Drawing.Size(245, 139);
			this.grpControlMask.TabIndex = 1;
			this.grpControlMask.TabStop = false;
			this.grpControlMask.Text = "Control && Mask";
			// 
			// tableLayoutPanel9
			// 
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
			this.tableLayoutPanel9.RowCount = 6;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(239, 120);
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
			this.flowLayoutPanel7.Controls.Add(this.lblSprAddr);
			this.flowLayoutPanel7.Controls.Add(this.txtSprAddr);
			this.flowLayoutPanel7.Location = new System.Drawing.Point(0, 24);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(116, 26);
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
			this.txtSprAddr.Size = new System.Drawing.Size(50, 20);
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(135, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel9.SetRowSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(104, 51);
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
			this.flowLayoutPanel6.Controls.Add(this.lblBGAddr);
			this.flowLayoutPanel6.Controls.Add(this.txtBGAddr);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(116, 24);
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
			this.txtBGAddr.Size = new System.Drawing.Size(50, 20);
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
			this.chkGrayscale.Location = new System.Drawing.Point(135, 51);
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
			this.chkIntensifyRed.Location = new System.Drawing.Point(135, 68);
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
			this.chkIntensifyGreen.Location = new System.Drawing.Point(135, 85);
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
			this.chkIntensifyBlue.Location = new System.Drawing.Point(135, 102);
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
			this.grpCPUStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpCPUStatus.Controls.Add(this.tableLayoutPanel3);
			this.grpCPUStatus.Location = new System.Drawing.Point(3, 3);
			this.grpCPUStatus.Name = "grpCPUStatus";
			this.grpCPUStatus.Size = new System.Drawing.Size(464, 171);
			this.grpCPUStatus.TabIndex = 0;
			this.grpCPUStatus.TabStop = false;
			this.grpCPUStatus.Text = "CPU Status";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.grpIRQ, 0, 2);
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
			this.tableLayoutPanel3.Size = new System.Drawing.Size(458, 152);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// grpIRQ
			// 
			this.grpIRQ.Controls.Add(this.flowLayoutPanel3);
			this.grpIRQ.Location = new System.Drawing.Point(3, 108);
			this.grpIRQ.Name = "grpIRQ";
			this.grpIRQ.Size = new System.Drawing.Size(348, 41);
			this.grpIRQ.TabIndex = 4;
			this.grpIRQ.TabStop = false;
			this.grpIRQ.Text = "IRQs";
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.chkExternal);
			this.flowLayoutPanel3.Controls.Add(this.chkFrameCounter);
			this.flowLayoutPanel3.Controls.Add(this.chkDMC);
			this.flowLayoutPanel3.Controls.Add(this.chkNMI);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(342, 22);
			this.flowLayoutPanel3.TabIndex = 4;
			// 
			// chkExternal
			// 
			this.chkExternal.AutoSize = true;
			this.chkExternal.Location = new System.Drawing.Point(0, 0);
			this.chkExternal.Margin = new System.Windows.Forms.Padding(0);
			this.chkExternal.Name = "chkExternal";
			this.chkExternal.Size = new System.Drawing.Size(64, 17);
			this.chkExternal.TabIndex = 1;
			this.chkExternal.Text = "External";
			this.chkExternal.UseVisualStyleBackColor = true;
			this.chkExternal.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkFrameCounter
			// 
			this.chkFrameCounter.AutoSize = true;
			this.chkFrameCounter.Location = new System.Drawing.Point(64, 0);
			this.chkFrameCounter.Margin = new System.Windows.Forms.Padding(0);
			this.chkFrameCounter.Name = "chkFrameCounter";
			this.chkFrameCounter.Size = new System.Drawing.Size(95, 17);
			this.chkFrameCounter.TabIndex = 2;
			this.chkFrameCounter.Text = "Frame Counter";
			this.chkFrameCounter.UseVisualStyleBackColor = true;
			this.chkFrameCounter.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkDMC
			// 
			this.chkDMC.AutoSize = true;
			this.chkDMC.Location = new System.Drawing.Point(159, 0);
			this.chkDMC.Margin = new System.Windows.Forms.Padding(0);
			this.chkDMC.Name = "chkDMC";
			this.chkDMC.Size = new System.Drawing.Size(50, 17);
			this.chkDMC.TabIndex = 3;
			this.chkDMC.Text = "DMC";
			this.chkDMC.UseVisualStyleBackColor = true;
			this.chkDMC.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// chkNMI
			// 
			this.chkNMI.AutoSize = true;
			this.chkNMI.Location = new System.Drawing.Point(209, 0);
			this.chkNMI.Margin = new System.Windows.Forms.Padding(0);
			this.chkNMI.Name = "chkNMI";
			this.chkNMI.Size = new System.Drawing.Size(46, 17);
			this.chkNMI.TabIndex = 4;
			this.chkNMI.Text = "NMI";
			this.chkNMI.UseVisualStyleBackColor = true;
			this.chkNMI.Click += new System.EventHandler(this.OnOptionChanged);
			// 
			// grpFlags
			// 
			this.grpFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpFlags.Controls.Add(this.tableLayoutPanel4);
			this.grpFlags.Location = new System.Drawing.Point(3, 30);
			this.grpFlags.Name = "grpFlags";
			this.grpFlags.Size = new System.Drawing.Size(353, 72);
			this.grpFlags.TabIndex = 3;
			this.grpFlags.TabStop = false;
			this.grpFlags.Text = "Flags";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(347, 53);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblStatus);
			this.flowLayoutPanel2.Controls.Add(this.txtStatus);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(347, 21);
			this.flowLayoutPanel2.TabIndex = 3;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblStatus.Location = new System.Drawing.Point(0, 0);
			this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(40, 20);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "Status:";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtStatus
			// 
			this.txtStatus.Location = new System.Drawing.Point(40, 0);
			this.txtStatus.Margin = new System.Windows.Forms.Padding(0);
			this.txtStatus.MaxLength = 2;
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(27, 20);
			this.txtStatus.TabIndex = 1;
			this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 4;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.Controls.Add(this.chkNegative, 3, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkOverflow, 2, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkReserved, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkBreak, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.chkDecimal, 3, 0);
			this.tableLayoutPanel5.Controls.Add(this.chkInterrupt, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.chkZero, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.chkCarry, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 21);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(347, 48);
			this.tableLayoutPanel5.TabIndex = 4;
			// 
			// chkNegative
			// 
			this.chkNegative.AutoSize = true;
			this.chkNegative.Location = new System.Drawing.Point(258, 17);
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
			this.chkOverflow.Location = new System.Drawing.Point(172, 17);
			this.chkOverflow.Margin = new System.Windows.Forms.Padding(0);
			this.chkOverflow.Name = "chkOverflow";
			this.chkOverflow.Size = new System.Drawing.Size(68, 17);
			this.chkOverflow.TabIndex = 6;
			this.chkOverflow.Text = "Overflow";
			this.chkOverflow.UseVisualStyleBackColor = true;
			this.chkOverflow.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkReserved
			// 
			this.chkReserved.AutoSize = true;
			this.chkReserved.Location = new System.Drawing.Point(86, 17);
			this.chkReserved.Margin = new System.Windows.Forms.Padding(0);
			this.chkReserved.Name = "chkReserved";
			this.chkReserved.Size = new System.Drawing.Size(72, 17);
			this.chkReserved.TabIndex = 5;
			this.chkReserved.Text = "Reserved";
			this.chkReserved.UseVisualStyleBackColor = true;
			this.chkReserved.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkBreak
			// 
			this.chkBreak.AutoSize = true;
			this.chkBreak.Location = new System.Drawing.Point(0, 17);
			this.chkBreak.Margin = new System.Windows.Forms.Padding(0);
			this.chkBreak.Name = "chkBreak";
			this.chkBreak.Size = new System.Drawing.Size(54, 17);
			this.chkBreak.TabIndex = 4;
			this.chkBreak.Text = "Break";
			this.chkBreak.UseVisualStyleBackColor = true;
			this.chkBreak.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkDecimal
			// 
			this.chkDecimal.AutoSize = true;
			this.chkDecimal.Location = new System.Drawing.Point(258, 0);
			this.chkDecimal.Margin = new System.Windows.Forms.Padding(0);
			this.chkDecimal.Name = "chkDecimal";
			this.chkDecimal.Size = new System.Drawing.Size(63, 17);
			this.chkDecimal.TabIndex = 3;
			this.chkDecimal.Text = "Unused";
			this.chkDecimal.UseVisualStyleBackColor = true;
			this.chkDecimal.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkInterrupt
			// 
			this.chkInterrupt.AutoSize = true;
			this.chkInterrupt.Location = new System.Drawing.Point(172, 0);
			this.chkInterrupt.Margin = new System.Windows.Forms.Padding(0);
			this.chkInterrupt.Name = "chkInterrupt";
			this.chkInterrupt.Size = new System.Drawing.Size(65, 17);
			this.chkInterrupt.TabIndex = 2;
			this.chkInterrupt.Text = "Interrupt";
			this.chkInterrupt.UseVisualStyleBackColor = true;
			this.chkInterrupt.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// chkZero
			// 
			this.chkZero.AutoSize = true;
			this.chkZero.Location = new System.Drawing.Point(86, 0);
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
			this.chkCarry.Location = new System.Drawing.Point(0, 0);
			this.chkCarry.Margin = new System.Windows.Forms.Padding(0);
			this.chkCarry.Name = "chkCarry";
			this.chkCarry.Size = new System.Drawing.Size(50, 17);
			this.chkCarry.TabIndex = 0;
			this.chkCarry.Text = "Carry";
			this.chkCarry.UseVisualStyleBackColor = true;
			this.chkCarry.Click += new System.EventHandler(this.chkCpuFlag_Click);
			// 
			// grpStack
			// 
			this.grpStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpStack.Controls.Add(this.tableLayoutPanel6);
			this.grpStack.Location = new System.Drawing.Point(362, 3);
			this.grpStack.Name = "grpStack";
			this.tableLayoutPanel3.SetRowSpan(this.grpStack, 3);
			this.grpStack.Size = new System.Drawing.Size(93, 146);
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
			this.tableLayoutPanel6.Controls.Add(this.lstStack, 0, 1);
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(87, 124);
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
			this.flowLayoutPanel4.Size = new System.Drawing.Size(87, 21);
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
			this.txtSP.Size = new System.Drawing.Size(49, 20);
			this.txtSP.TabIndex = 1;
			this.txtSP.TextChanged += new System.EventHandler(this.OnOptionChanged);
			// 
			// lstStack
			// 
			this.lstStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstStack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lstStack.FullRowSelect = true;
			this.lstStack.GridLines = true;
			this.lstStack.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstStack.Location = new System.Drawing.Point(3, 24);
			this.lstStack.Name = "lstStack";
			this.lstStack.Size = new System.Drawing.Size(81, 97);
			this.lstStack.TabIndex = 4;
			this.lstStack.UseCompatibleStateImageBehavior = false;
			this.lstStack.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Value";
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
			this.flowLayoutPanel1.Size = new System.Drawing.Size(353, 21);
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
			this.txtCycleCount.MaxLength = 8;
			this.txtCycleCount.Name = "txtCycleCount";
			this.txtCycleCount.Size = new System.Drawing.Size(77, 20);
			this.txtCycleCount.TabIndex = 9;
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
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 362);
			this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 2;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(470, 29);
			this.tableLayoutPanel10.TabIndex = 3;
			// 
			// btnGoto
			// 
			this.btnGoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGoto.AutoSize = true;
			this.btnGoto.Image = ((System.Drawing.Image)(resources.GetObject("btnGoto.Image")));
			this.btnGoto.Location = new System.Drawing.Point(395, 3);
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
			this.btnUndo.Location = new System.Drawing.Point(3, 3);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(87, 23);
			this.btnUndo.TabIndex = 3;
			this.btnUndo.Text = "Undo changes";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
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
            this.mnuGoToResetHandler});
			this.contextGoTo.Name = "contextGoTo";
			this.contextGoTo.Size = new System.Drawing.Size(148, 70);
			this.contextGoTo.Opening += new System.ComponentModel.CancelEventHandler(this.contextGoTo_Opening);
			// 
			// mnuGoToIrqHandler
			// 
			this.mnuGoToIrqHandler.Name = "mnuGoToIrqHandler";
			this.mnuGoToIrqHandler.Size = new System.Drawing.Size(147, 22);
			this.mnuGoToIrqHandler.Text = "IRQ Handler";
			this.mnuGoToIrqHandler.Click += new System.EventHandler(this.mnuGoToIrqHandler_Click);
			// 
			// mnuGoToNmiHandler
			// 
			this.mnuGoToNmiHandler.Name = "mnuGoToNmiHandler";
			this.mnuGoToNmiHandler.Size = new System.Drawing.Size(147, 22);
			this.mnuGoToNmiHandler.Text = "NMI Handler";
			this.mnuGoToNmiHandler.Click += new System.EventHandler(this.mnuGoToNmiHandler_Click);
			// 
			// mnuGoToResetHandler
			// 
			this.mnuGoToResetHandler.Name = "mnuGoToResetHandler";
			this.mnuGoToResetHandler.Size = new System.Drawing.Size(147, 22);
			this.mnuGoToResetHandler.Text = "Reset Handler";
			this.mnuGoToResetHandler.Click += new System.EventHandler(this.mnuGoToResetHandler_Click);
			// 
			// ctrlConsoleStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel2);
			this.Name = "ctrlConsoleStatus";
			this.Size = new System.Drawing.Size(470, 391);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.grpPPUStatus.ResumeLayout(false);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.grpControlMask.ResumeLayout(false);
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
			this.grpIRQ.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.grpFlags.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.grpStack.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
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
		private System.Windows.Forms.GroupBox grpIRQ;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.CheckBox chkExternal;
		private System.Windows.Forms.CheckBox chkFrameCounter;
		private System.Windows.Forms.CheckBox chkDMC;
		private System.Windows.Forms.CheckBox chkNMI;
		private System.Windows.Forms.GroupBox grpFlags;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.CheckBox chkNegative;
		private System.Windows.Forms.CheckBox chkOverflow;
		private System.Windows.Forms.CheckBox chkReserved;
		private System.Windows.Forms.CheckBox chkBreak;
		private System.Windows.Forms.CheckBox chkDecimal;
		private System.Windows.Forms.CheckBox chkInterrupt;
		private System.Windows.Forms.CheckBox chkZero;
		private System.Windows.Forms.CheckBox chkCarry;
		private System.Windows.Forms.GroupBox grpStack;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblSP;
		private System.Windows.Forms.TextBox txtSP;
		private Mesen.GUI.Controls.DoubleBufferedListView lstStack;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblA;
		private System.Windows.Forms.TextBox txtA;
		private System.Windows.Forms.Label lblX;
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.Label lblY;
		private System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.Label lblPC;
		private System.Windows.Forms.TextBox txtPC;
		private System.Windows.Forms.ColumnHeader columnHeader1;
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
	}
}
