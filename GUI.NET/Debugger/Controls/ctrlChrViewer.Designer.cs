using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlChrViewer
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
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpDisplayOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkShowSingleColorTilesInGrayscale = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblPalette = new System.Windows.Forms.Label();
			this.cboPalette = new System.Windows.Forms.ComboBox();
			this.picPaletteTooltip = new System.Windows.Forms.PictureBox();
			this.chkLargeSprites = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblChrSelection = new System.Windows.Forms.Label();
			this.cboChrSelection = new System.Windows.Forms.ComboBox();
			this.flpHighlight = new System.Windows.Forms.FlowLayoutPanel();
			this.lblHighlight = new System.Windows.Forms.Label();
			this.cboHighlightType = new System.Windows.Forms.ComboBox();
			this.chkAutoPalette = new System.Windows.Forms.CheckBox();
			this.grpTileInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.txtTileAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTileIndex = new System.Windows.Forms.TextBox();
			this.picTile = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.picColorTooltip = new System.Windows.Forms.PictureBox();
			this.picTileTooltip = new System.Windows.Forms.PictureBox();
			this.ctrlTilePalette = new Mesen.GUI.Debugger.Controls.ctrlTilePalette();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.picChrBank1 = new System.Windows.Forms.PictureBox();
			this.ctxMenu = new ctrlMesenContextMenuStrip(this.components);
			this.mnuCopyHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportToPng = new System.Windows.Forms.ToolStripMenuItem();
			this.picChrBank2 = new System.Windows.Forms.PictureBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.mnuEditInMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpDisplayOptions.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteTooltip)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			this.flpHighlight.SuspendLayout();
			this.grpTileInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picColorTooltip)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picTileTooltip)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).BeginInit();
			this.ctxMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.grpDisplayOptions, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.grpTileInfo, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(534, 525);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// grpDisplayOptions
			// 
			this.grpDisplayOptions.Controls.Add(this.tableLayoutPanel1);
			this.grpDisplayOptions.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpDisplayOptions.Location = new System.Drawing.Point(269, 300);
			this.grpDisplayOptions.Name = "grpDisplayOptions";
			this.grpDisplayOptions.Size = new System.Drawing.Size(264, 175);
			this.grpDisplayOptions.TabIndex = 4;
			this.grpDisplayOptions.TabStop = false;
			this.grpDisplayOptions.Text = "Display Options";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkShowSingleColorTilesInGrayscale, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkLargeSprites, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flpHighlight, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkAutoPalette, 0, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(258, 156);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// chkShowSingleColorTilesInGrayscale
			// 
			this.chkShowSingleColorTilesInGrayscale.AutoSize = true;
			this.chkShowSingleColorTilesInGrayscale.Location = new System.Drawing.Point(3, 130);
			this.chkShowSingleColorTilesInGrayscale.Name = "chkShowSingleColorTilesInGrayscale";
			this.chkShowSingleColorTilesInGrayscale.Size = new System.Drawing.Size(241, 17);
			this.chkShowSingleColorTilesInGrayscale.TabIndex = 9;
			this.chkShowSingleColorTilesInGrayscale.Text = "Show single color tiles using grayscale palette";
			this.chkShowSingleColorTilesInGrayscale.UseVisualStyleBackColor = true;
			this.chkShowSingleColorTilesInGrayscale.CheckedChanged += new System.EventHandler(this.chkShowSingleColorTilesInGrayscale_CheckedChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblPalette);
			this.flowLayoutPanel1.Controls.Add(this.cboPalette);
			this.flowLayoutPanel1.Controls.Add(this.picPaletteTooltip);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 27);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(258, 27);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// lblPalette
			// 
			this.lblPalette.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPalette.AutoSize = true;
			this.lblPalette.Location = new System.Drawing.Point(3, 7);
			this.lblPalette.Name = "lblPalette";
			this.lblPalette.Size = new System.Drawing.Size(43, 13);
			this.lblPalette.TabIndex = 0;
			this.lblPalette.Text = "Palette:";
			// 
			// cboPalette
			// 
			this.cboPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPalette.FormattingEnabled = true;
			this.cboPalette.Items.AddRange(new object[] {
            "Tile 0",
            "Tile 1",
            "Tile 2",
            "Tile 3",
            "Sprite 0",
            "Sprite 1",
            "Sprite 2",
            "Sprite 3",
            "Grayscale"});
			this.cboPalette.Location = new System.Drawing.Point(52, 3);
			this.cboPalette.Name = "cboPalette";
			this.cboPalette.Size = new System.Drawing.Size(84, 21);
			this.cboPalette.TabIndex = 1;
			this.cboPalette.SelectedIndexChanged += new System.EventHandler(this.cboPalette_SelectedIndexChanged);
			// 
			// picPaletteTooltip
			// 
			this.picPaletteTooltip.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picPaletteTooltip.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picPaletteTooltip.Location = new System.Drawing.Point(142, 7);
			this.picPaletteTooltip.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
			this.picPaletteTooltip.Name = "picPaletteTooltip";
			this.picPaletteTooltip.Size = new System.Drawing.Size(14, 14);
			this.picPaletteTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPaletteTooltip.TabIndex = 16;
			this.picPaletteTooltip.TabStop = false;
			// 
			// chkLargeSprites
			// 
			this.chkLargeSprites.AutoSize = true;
			this.chkLargeSprites.Location = new System.Drawing.Point(3, 84);
			this.chkLargeSprites.Name = "chkLargeSprites";
			this.chkLargeSprites.Size = new System.Drawing.Size(133, 17);
			this.chkLargeSprites.TabIndex = 2;
			this.chkLargeSprites.Text = "Display as 8x16 sprites";
			this.chkLargeSprites.UseVisualStyleBackColor = true;
			this.chkLargeSprites.CheckedChanged += new System.EventHandler(this.chkLargeSprites_CheckedChanged);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblChrSelection);
			this.flowLayoutPanel2.Controls.Add(this.cboChrSelection);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(258, 27);
			this.flowLayoutPanel2.TabIndex = 6;
			// 
			// lblChrSelection
			// 
			this.lblChrSelection.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblChrSelection.AutoSize = true;
			this.lblChrSelection.Location = new System.Drawing.Point(3, 7);
			this.lblChrSelection.Name = "lblChrSelection";
			this.lblChrSelection.Size = new System.Drawing.Size(80, 13);
			this.lblChrSelection.TabIndex = 0;
			this.lblChrSelection.Text = "CHR Selection:";
			// 
			// cboChrSelection
			// 
			this.cboChrSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChrSelection.FormattingEnabled = true;
			this.cboChrSelection.Location = new System.Drawing.Point(89, 3);
			this.cboChrSelection.Name = "cboChrSelection";
			this.cboChrSelection.Size = new System.Drawing.Size(150, 21);
			this.cboChrSelection.TabIndex = 1;
			this.cboChrSelection.DropDown += new System.EventHandler(this.cboChrSelection_DropDown);
			this.cboChrSelection.SelectedIndexChanged += new System.EventHandler(this.cboChrSelection_SelectedIndexChanged);
			// 
			// flpHighlight
			// 
			this.flpHighlight.Controls.Add(this.lblHighlight);
			this.flpHighlight.Controls.Add(this.cboHighlightType);
			this.flpHighlight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpHighlight.Location = new System.Drawing.Point(0, 54);
			this.flpHighlight.Margin = new System.Windows.Forms.Padding(0);
			this.flpHighlight.Name = "flpHighlight";
			this.flpHighlight.Size = new System.Drawing.Size(258, 27);
			this.flpHighlight.TabIndex = 7;
			// 
			// lblHighlight
			// 
			this.lblHighlight.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHighlight.AutoSize = true;
			this.lblHighlight.Location = new System.Drawing.Point(3, 7);
			this.lblHighlight.Name = "lblHighlight";
			this.lblHighlight.Size = new System.Drawing.Size(51, 13);
			this.lblHighlight.TabIndex = 0;
			this.lblHighlight.Text = "Highlight:";
			// 
			// cboHighlightType
			// 
			this.cboHighlightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboHighlightType.FormattingEnabled = true;
			this.cboHighlightType.Items.AddRange(new object[] {
            "None",
            "Used Tiles",
            "Unused Tiles"});
			this.cboHighlightType.Location = new System.Drawing.Point(60, 3);
			this.cboHighlightType.Name = "cboHighlightType";
			this.cboHighlightType.Size = new System.Drawing.Size(92, 21);
			this.cboHighlightType.TabIndex = 1;
			this.cboHighlightType.SelectedIndexChanged += new System.EventHandler(this.cboHighlightType_SelectedIndexChanged);
			// 
			// chkAutoPalette
			// 
			this.chkAutoPalette.AutoSize = true;
			this.chkAutoPalette.Location = new System.Drawing.Point(3, 107);
			this.chkAutoPalette.Name = "chkAutoPalette";
			this.chkAutoPalette.Size = new System.Drawing.Size(221, 17);
			this.chkAutoPalette.TabIndex = 8;
			this.chkAutoPalette.Text = "Display tiles using their last known palette";
			this.chkAutoPalette.UseVisualStyleBackColor = true;
			this.chkAutoPalette.CheckedChanged += new System.EventHandler(this.chkAutoPalette_CheckedChanged);
			// 
			// grpTileInfo
			// 
			this.grpTileInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpTileInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpTileInfo.Location = new System.Drawing.Point(269, 3);
			this.grpTileInfo.Name = "grpTileInfo";
			this.grpTileInfo.Size = new System.Drawing.Size(264, 291);
			this.grpTileInfo.TabIndex = 4;
			this.grpTileInfo.TabStop = false;
			this.grpTileInfo.Text = "Tile Info";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 3;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.txtTileAddress, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label6, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtTileIndex, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.picTile, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.label3, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.picColorTooltip, 2, 3);
			this.tableLayoutPanel4.Controls.Add(this.picTileTooltip, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.ctrlTilePalette, 1, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 5;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(258, 272);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// txtTileAddress
			// 
			this.txtTileAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileAddress.Location = new System.Drawing.Point(77, 29);
			this.txtTileAddress.Name = "txtTileAddress";
			this.txtTileAddress.ReadOnly = true;
			this.txtTileAddress.Size = new System.Drawing.Size(42, 20);
			this.txtTileAddress.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tile Index:";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Tile Address:";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 113);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Tile:";
			// 
			// txtTileIndex
			// 
			this.txtTileIndex.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileIndex.Location = new System.Drawing.Point(77, 3);
			this.txtTileIndex.Name = "txtTileIndex";
			this.txtTileIndex.ReadOnly = true;
			this.txtTileIndex.Size = new System.Drawing.Size(26, 20);
			this.txtTileIndex.TabIndex = 7;
			// 
			// picTile
			// 
			this.picTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTile.Location = new System.Drawing.Point(77, 55);
			this.picTile.Name = "picTile";
			this.picTile.Size = new System.Drawing.Size(130, 130);
			this.picTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
			this.picTile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTile_MouseDown);
			this.picTile.MouseLeave += new System.EventHandler(this.picTile_MouseLeave);
			this.picTile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTile_MouseMove);
			this.picTile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTile_MouseUp);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 201);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Color Picker:";
			// 
			// picColorTooltip
			// 
			this.picColorTooltip.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picColorTooltip.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picColorTooltip.Location = new System.Drawing.Point(213, 201);
			this.picColorTooltip.Name = "picColorTooltip";
			this.picColorTooltip.Size = new System.Drawing.Size(14, 14);
			this.picColorTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picColorTooltip.TabIndex = 15;
			this.picColorTooltip.TabStop = false;
			// 
			// picTileTooltip
			// 
			this.picTileTooltip.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picTileTooltip.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picTileTooltip.Location = new System.Drawing.Point(213, 113);
			this.picTileTooltip.Name = "picTileTooltip";
			this.picTileTooltip.Size = new System.Drawing.Size(14, 14);
			this.picTileTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTileTooltip.TabIndex = 16;
			this.picTileTooltip.TabStop = false;
			// 
			// ctrlTilePalette
			// 
			this.ctrlTilePalette.DisplayIndexes = false;
			this.ctrlTilePalette.HighlightMouseOver = false;
			this.ctrlTilePalette.Location = new System.Drawing.Point(77, 191);
			this.ctrlTilePalette.Name = "ctrlTilePalette";
			this.ctrlTilePalette.Size = new System.Drawing.Size(130, 34);
			this.ctrlTilePalette.TabIndex = 17;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.picChrBank1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.picChrBank2, 0, 1);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel3.SetRowSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(260, 520);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// picChrBank1
			// 
			this.picChrBank1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank1.ContextMenuStrip = this.ctxMenu;
			this.picChrBank1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picChrBank1.Location = new System.Drawing.Point(1, 1);
			this.picChrBank1.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank1.Name = "picChrBank1";
			this.picChrBank1.Size = new System.Drawing.Size(258, 258);
			this.picChrBank1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picChrBank1.TabIndex = 0;
			this.picChrBank1.TabStop = false;
			this.picChrBank1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseDown);
			this.picChrBank1.MouseEnter += new System.EventHandler(this.picChrBank_MouseEnter);
			this.picChrBank1.MouseLeave += new System.EventHandler(this.picChrBank_MouseLeave);
			this.picChrBank1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditInMemoryViewer,
            this.toolStripMenuItem2,
            this.mnuCopyHdPack,
            this.toolStripMenuItem1,
            this.mnuCopyToClipboard,
            this.mnuExportToPng});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(222, 126);
			this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
			// 
			// mnuCopyHdPack
			// 
			this.mnuCopyHdPack.Name = "mnuCopyHdPack";
			this.mnuCopyHdPack.Size = new System.Drawing.Size(221, 22);
			this.mnuCopyHdPack.Text = "Copy Tile (HD Pack Format)";
			this.mnuCopyHdPack.Click += new System.EventHandler(this.mnuCopyHdPack_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(218, 6);
			// 
			// mnuCopyToClipboard
			// 
			this.mnuCopyToClipboard.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCopyToClipboard.Name = "mnuCopyToClipboard";
			this.mnuCopyToClipboard.Size = new System.Drawing.Size(221, 22);
			this.mnuCopyToClipboard.Text = "Copy image to clipboard";
			this.mnuCopyToClipboard.Click += new System.EventHandler(this.mnuCopyToClipboard_Click);
			// 
			// mnuExportToPng
			// 
			this.mnuExportToPng.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportToPng.Name = "mnuExportToPng";
			this.mnuExportToPng.Size = new System.Drawing.Size(221, 22);
			this.mnuExportToPng.Text = "Export image to PNG";
			this.mnuExportToPng.Click += new System.EventHandler(this.mnuExportToPng_Click);
			// 
			// picChrBank2
			// 
			this.picChrBank2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank2.ContextMenuStrip = this.ctxMenu;
			this.picChrBank2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picChrBank2.Location = new System.Drawing.Point(1, 261);
			this.picChrBank2.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank2.Name = "picChrBank2";
			this.picChrBank2.Size = new System.Drawing.Size(258, 258);
			this.picChrBank2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picChrBank2.TabIndex = 1;
			this.picChrBank2.TabStop = false;
			this.picChrBank2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseDown);
			this.picChrBank2.MouseEnter += new System.EventHandler(this.picChrBank_MouseEnter);
			this.picChrBank2.MouseLeave += new System.EventHandler(this.picChrBank_MouseLeave);
			this.picChrBank2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 32700;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 10;
			// 
			// mnuEditInMemoryViewer
			// 
			this.mnuEditInMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuEditInMemoryViewer.Name = "mnuEditInMemoryViewer";
			this.mnuEditInMemoryViewer.Size = new System.Drawing.Size(221, 22);
			this.mnuEditInMemoryViewer.Text = "Edit in Memory Viewer";
			this.mnuEditInMemoryViewer.Click += new System.EventHandler(this.mnuEditInMemoryViewer_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(218, 6);
			// 
			// ctrlChrViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlChrViewer";
			this.Size = new System.Drawing.Size(534, 525);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpDisplayOptions.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteTooltip)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flpHighlight.ResumeLayout(false);
			this.flpHighlight.PerformLayout();
			this.grpTileInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picColorTooltip)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picTileTooltip)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).EndInit();
			this.ctxMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox picChrBank1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblPalette;
		private System.Windows.Forms.ComboBox cboPalette;
		private System.Windows.Forms.CheckBox chkLargeSprites;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblChrSelection;
		private System.Windows.Forms.ComboBox cboChrSelection;
		private System.Windows.Forms.FlowLayoutPanel flpHighlight;
		private System.Windows.Forms.Label lblHighlight;
		private System.Windows.Forms.ComboBox cboHighlightType;
		private System.Windows.Forms.GroupBox grpTileInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtTileAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtTileIndex;
		private System.Windows.Forms.PictureBox picTile;
		private System.Windows.Forms.PictureBox picChrBank2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.GroupBox grpDisplayOptions;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox picColorTooltip;
		private System.Windows.Forms.PictureBox picTileTooltip;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.PictureBox picPaletteTooltip;
		private ctrlMesenContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyHdPack;
		private ctrlTilePalette ctrlTilePalette;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyToClipboard;
		private System.Windows.Forms.CheckBox chkAutoPalette;
		private System.Windows.Forms.CheckBox chkShowSingleColorTilesInGrayscale;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuExportToPng;
		private System.Windows.Forms.ToolStripMenuItem mnuEditInMemoryViewer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
	}
}
