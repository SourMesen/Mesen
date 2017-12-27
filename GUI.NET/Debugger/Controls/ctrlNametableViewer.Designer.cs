namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlNametableViewer
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.picNametable = new System.Windows.Forms.PictureBox();
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowInChrViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCopyHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.grpTileInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPpuAddress = new System.Windows.Forms.TextBox();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.txtNametable = new System.Windows.Forms.TextBox();
			this.txtPaletteAddress = new System.Windows.Forms.TextBox();
			this.txtAttributeAddress = new System.Windows.Forms.TextBox();
			this.txtAttributeData = new System.Windows.Forms.TextBox();
			this.txtTileAddress = new System.Windows.Forms.TextBox();
			this.lblTileIndex = new System.Windows.Forms.Label();
			this.lblTileAddress = new System.Windows.Forms.Label();
			this.lblAttributeData = new System.Windows.Forms.Label();
			this.lblAttributeAddress = new System.Windows.Forms.Label();
			this.lblPaletteAddress = new System.Windows.Forms.Label();
			this.lblTile = new System.Windows.Forms.Label();
			this.txtTileIndex = new System.Windows.Forms.TextBox();
			this.picTile = new System.Windows.Forms.PictureBox();
			this.lblNametableIndex = new System.Windows.Forms.Label();
			this.lblLocation = new System.Windows.Forms.Label();
			this.lblPpuAddress = new System.Windows.Forms.Label();
			this.ctrlTilePalette = new Mesen.GUI.Debugger.Controls.ctrlTilePalette();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkShowPpuScrollOverlay = new System.Windows.Forms.CheckBox();
			this.chkShowTileGrid = new System.Windows.Forms.CheckBox();
			this.chkShowAttributeGrid = new System.Windows.Forms.CheckBox();
			this.chkHighlightChrTile = new System.Windows.Forms.CheckBox();
			this.mnuCopyNametableHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNametable)).BeginInit();
			this.ctxMenu.SuspendLayout();
			this.grpTileInfo.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.picNametable, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpTileInfo, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(697, 486);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// picNametable
			// 
			this.picNametable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picNametable.ContextMenuStrip = this.ctxMenu;
			this.picNametable.Location = new System.Drawing.Point(4, 4);
			this.picNametable.Margin = new System.Windows.Forms.Padding(4);
			this.picNametable.Name = "picNametable";
			this.tableLayoutPanel1.SetRowSpan(this.picNametable, 2);
			this.picNametable.Size = new System.Drawing.Size(514, 482);
			this.picNametable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picNametable.TabIndex = 0;
			this.picNametable.TabStop = false;
			this.picNametable.DoubleClick += new System.EventHandler(this.picNametable_DoubleClick);
			this.picNametable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picNametable_MouseMove);
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowInChrViewer,
            this.toolStripMenuItem1,
            this.mnuCopyHdPack,
            this.mnuCopyNametableHdPack,
            this.mnuCopyToClipboard});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(261, 120);
			this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
			// 
			// mnuShowInChrViewer
			// 
			this.mnuShowInChrViewer.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuShowInChrViewer.Name = "mnuShowInChrViewer";
			this.mnuShowInChrViewer.ShortcutKeyDisplayString = "Dbl-Click";
			this.mnuShowInChrViewer.Size = new System.Drawing.Size(260, 22);
			this.mnuShowInChrViewer.Text = "View in CHR viewer";
			this.mnuShowInChrViewer.Click += new System.EventHandler(this.mnuShowInChrViewer_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(257, 6);
			// 
			// mnuCopyHdPack
			// 
			this.mnuCopyHdPack.Name = "mnuCopyHdPack";
			this.mnuCopyHdPack.Size = new System.Drawing.Size(260, 22);
			this.mnuCopyHdPack.Text = "Copy Tile (HD Pack Format)";
			this.mnuCopyHdPack.Click += new System.EventHandler(this.mnuCopyHdPack_Click);
			// 
			// mnuCopyToClipboard
			// 
			this.mnuCopyToClipboard.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCopyToClipboard.Name = "mnuCopyToClipboard";
			this.mnuCopyToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.mnuCopyToClipboard.Size = new System.Drawing.Size(260, 22);
			this.mnuCopyToClipboard.Text = "Copy image to clipboard";
			this.mnuCopyToClipboard.Click += new System.EventHandler(this.mnuCopyToClipboard_Click);
			// 
			// grpTileInfo
			// 
			this.grpTileInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpTileInfo.Controls.Add(this.tableLayoutPanel2);
			this.grpTileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpTileInfo.Location = new System.Drawing.Point(525, 3);
			this.grpTileInfo.Name = "grpTileInfo";
			this.grpTileInfo.Size = new System.Drawing.Size(169, 375);
			this.grpTileInfo.TabIndex = 4;
			this.grpTileInfo.TabStop = false;
			this.grpTileInfo.Text = "Tile Info";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.txtPpuAddress, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtLocation, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtNametable, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtPaletteAddress, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.txtAttributeAddress, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.txtAttributeData, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.txtTileAddress, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.lblTileIndex, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblTileAddress, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.lblAttributeData, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.lblAttributeAddress, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.lblPaletteAddress, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.lblTile, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.txtTileIndex, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.picTile, 1, 8);
			this.tableLayoutPanel2.Controls.Add(this.lblNametableIndex, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblLocation, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblPpuAddress, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.ctrlTilePalette, 0, 9);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 11;
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(163, 356);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// txtPpuAddress
			// 
			this.txtPpuAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtPpuAddress.Location = new System.Drawing.Point(84, 3);
			this.txtPpuAddress.Name = "txtPpuAddress";
			this.txtPpuAddress.ReadOnly = true;
			this.txtPpuAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPpuAddress.TabIndex = 18;
			// 
			// txtLocation
			// 
			this.txtLocation.BackColor = System.Drawing.SystemColors.Window;
			this.txtLocation.Location = new System.Drawing.Point(84, 55);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.ReadOnly = true;
			this.txtLocation.Size = new System.Drawing.Size(42, 20);
			this.txtLocation.TabIndex = 16;
			// 
			// txtNametable
			// 
			this.txtNametable.BackColor = System.Drawing.SystemColors.Window;
			this.txtNametable.Location = new System.Drawing.Point(84, 29);
			this.txtNametable.Name = "txtNametable";
			this.txtNametable.ReadOnly = true;
			this.txtNametable.Size = new System.Drawing.Size(26, 20);
			this.txtNametable.TabIndex = 15;
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtPaletteAddress.Location = new System.Drawing.Point(84, 185);
			this.txtPaletteAddress.Name = "txtPaletteAddress";
			this.txtPaletteAddress.ReadOnly = true;
			this.txtPaletteAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPaletteAddress.TabIndex = 11;
			// 
			// txtAttributeAddress
			// 
			this.txtAttributeAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtAttributeAddress.Location = new System.Drawing.Point(84, 159);
			this.txtAttributeAddress.Name = "txtAttributeAddress";
			this.txtAttributeAddress.ReadOnly = true;
			this.txtAttributeAddress.Size = new System.Drawing.Size(42, 20);
			this.txtAttributeAddress.TabIndex = 10;
			// 
			// txtAttributeData
			// 
			this.txtAttributeData.BackColor = System.Drawing.SystemColors.Window;
			this.txtAttributeData.Location = new System.Drawing.Point(84, 133);
			this.txtAttributeData.Name = "txtAttributeData";
			this.txtAttributeData.ReadOnly = true;
			this.txtAttributeData.Size = new System.Drawing.Size(26, 20);
			this.txtAttributeData.TabIndex = 9;
			// 
			// txtTileAddress
			// 
			this.txtTileAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileAddress.Location = new System.Drawing.Point(84, 107);
			this.txtTileAddress.Name = "txtTileAddress";
			this.txtTileAddress.ReadOnly = true;
			this.txtTileAddress.Size = new System.Drawing.Size(42, 20);
			this.txtTileAddress.TabIndex = 8;
			// 
			// lblTileIndex
			// 
			this.lblTileIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileIndex.AutoSize = true;
			this.lblTileIndex.Location = new System.Drawing.Point(3, 84);
			this.lblTileIndex.Name = "lblTileIndex";
			this.lblTileIndex.Size = new System.Drawing.Size(56, 13);
			this.lblTileIndex.TabIndex = 0;
			this.lblTileIndex.Text = "Tile Index:";
			// 
			// lblTileAddress
			// 
			this.lblTileAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileAddress.AutoSize = true;
			this.lblTileAddress.Location = new System.Drawing.Point(3, 110);
			this.lblTileAddress.Name = "lblTileAddress";
			this.lblTileAddress.Size = new System.Drawing.Size(68, 13);
			this.lblTileAddress.TabIndex = 1;
			this.lblTileAddress.Text = "Tile Address:";
			// 
			// lblAttributeData
			// 
			this.lblAttributeData.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAttributeData.AutoSize = true;
			this.lblAttributeData.Location = new System.Drawing.Point(3, 136);
			this.lblAttributeData.Name = "lblAttributeData";
			this.lblAttributeData.Size = new System.Drawing.Size(75, 13);
			this.lblAttributeData.TabIndex = 2;
			this.lblAttributeData.Text = "Attribute Data:";
			// 
			// lblAttributeAddress
			// 
			this.lblAttributeAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAttributeAddress.AutoSize = true;
			this.lblAttributeAddress.Location = new System.Drawing.Point(3, 162);
			this.lblAttributeAddress.Name = "lblAttributeAddress";
			this.lblAttributeAddress.Size = new System.Drawing.Size(74, 13);
			this.lblAttributeAddress.TabIndex = 3;
			this.lblAttributeAddress.Text = "Attribute Addr:";
			// 
			// lblPaletteAddress
			// 
			this.lblPaletteAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPaletteAddress.AutoSize = true;
			this.lblPaletteAddress.Location = new System.Drawing.Point(3, 188);
			this.lblPaletteAddress.Name = "lblPaletteAddress";
			this.lblPaletteAddress.Size = new System.Drawing.Size(68, 13);
			this.lblPaletteAddress.TabIndex = 4;
			this.lblPaletteAddress.Text = "Palette Addr:";
			// 
			// lblTile
			// 
			this.lblTile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTile.AutoSize = true;
			this.lblTile.Location = new System.Drawing.Point(3, 237);
			this.lblTile.Name = "lblTile";
			this.lblTile.Size = new System.Drawing.Size(27, 13);
			this.lblTile.TabIndex = 6;
			this.lblTile.Text = "Tile:";
			// 
			// txtTileIndex
			// 
			this.txtTileIndex.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileIndex.Location = new System.Drawing.Point(84, 81);
			this.txtTileIndex.Name = "txtTileIndex";
			this.txtTileIndex.ReadOnly = true;
			this.txtTileIndex.Size = new System.Drawing.Size(26, 20);
			this.txtTileIndex.TabIndex = 7;
			// 
			// picTile
			// 
			this.picTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTile.Location = new System.Drawing.Point(84, 211);
			this.picTile.Name = "picTile";
			this.picTile.Size = new System.Drawing.Size(63, 66);
			this.picTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
			// 
			// lblNametableIndex
			// 
			this.lblNametableIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNametableIndex.AutoSize = true;
			this.lblNametableIndex.Location = new System.Drawing.Point(3, 32);
			this.lblNametableIndex.Name = "lblNametableIndex";
			this.lblNametableIndex.Size = new System.Drawing.Size(61, 13);
			this.lblNametableIndex.TabIndex = 13;
			this.lblNametableIndex.Text = "Nametable:";
			// 
			// lblLocation
			// 
			this.lblLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(3, 58);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(51, 13);
			this.lblLocation.TabIndex = 14;
			this.lblLocation.Text = "Location:";
			// 
			// lblPpuAddress
			// 
			this.lblPpuAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPpuAddress.AutoSize = true;
			this.lblPpuAddress.Location = new System.Drawing.Point(3, 6);
			this.lblPpuAddress.Name = "lblPpuAddress";
			this.lblPpuAddress.Size = new System.Drawing.Size(57, 13);
			this.lblPpuAddress.TabIndex = 17;
			this.lblPpuAddress.Text = "PPU Addr:";
			// 
			// ctrlTilePalette
			// 
			this.ctrlTilePalette.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.tableLayoutPanel2.SetColumnSpan(this.ctrlTilePalette, 2);
			this.ctrlTilePalette.DisplayIndexes = false;
			this.ctrlTilePalette.HighlightMouseOver = false;
			this.ctrlTilePalette.Location = new System.Drawing.Point(17, 283);
			this.ctrlTilePalette.Name = "ctrlTilePalette";
			this.ctrlTilePalette.Size = new System.Drawing.Size(130, 34);
			this.ctrlTilePalette.TabIndex = 19;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.chkShowPpuScrollOverlay);
			this.flowLayoutPanel1.Controls.Add(this.chkShowTileGrid);
			this.flowLayoutPanel1.Controls.Add(this.chkShowAttributeGrid);
			this.flowLayoutPanel1.Controls.Add(this.chkHighlightChrTile);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(522, 381);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(175, 109);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// chkShowPpuScrollOverlay
			// 
			this.chkShowPpuScrollOverlay.AutoSize = true;
			this.chkShowPpuScrollOverlay.Location = new System.Drawing.Point(3, 3);
			this.chkShowPpuScrollOverlay.Name = "chkShowPpuScrollOverlay";
			this.chkShowPpuScrollOverlay.Size = new System.Drawing.Size(146, 17);
			this.chkShowPpuScrollOverlay.TabIndex = 1;
			this.chkShowPpuScrollOverlay.Text = "Show PPU Scroll Overlay";
			this.chkShowPpuScrollOverlay.UseVisualStyleBackColor = true;
			this.chkShowPpuScrollOverlay.Click += new System.EventHandler(this.chkShowScrollWindow_Click);
			// 
			// chkShowTileGrid
			// 
			this.chkShowTileGrid.AutoSize = true;
			this.chkShowTileGrid.Location = new System.Drawing.Point(3, 26);
			this.chkShowTileGrid.Name = "chkShowTileGrid";
			this.chkShowTileGrid.Size = new System.Drawing.Size(95, 17);
			this.chkShowTileGrid.TabIndex = 2;
			this.chkShowTileGrid.Text = "Show Tile Grid";
			this.chkShowTileGrid.UseVisualStyleBackColor = true;
			this.chkShowTileGrid.Click += new System.EventHandler(this.chkShowTileGrid_Click);
			// 
			// chkShowAttributeGrid
			// 
			this.chkShowAttributeGrid.AutoSize = true;
			this.chkShowAttributeGrid.Location = new System.Drawing.Point(3, 49);
			this.chkShowAttributeGrid.Name = "chkShowAttributeGrid";
			this.chkShowAttributeGrid.Size = new System.Drawing.Size(117, 17);
			this.chkShowAttributeGrid.TabIndex = 3;
			this.chkShowAttributeGrid.Text = "Show Attribute Grid";
			this.chkShowAttributeGrid.UseVisualStyleBackColor = true;
			this.chkShowAttributeGrid.Click += new System.EventHandler(this.chkShowAttributeGrid_Click);
			// 
			// chkHighlightChrTile
			// 
			this.chkHighlightChrTile.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkHighlightChrTile.Location = new System.Drawing.Point(3, 72);
			this.chkHighlightChrTile.Name = "chkHighlightChrTile";
			this.chkHighlightChrTile.Size = new System.Drawing.Size(150, 31);
			this.chkHighlightChrTile.TabIndex = 4;
			this.chkHighlightChrTile.Text = "Highlight tile selected in CHR viewer";
			this.chkHighlightChrTile.UseVisualStyleBackColor = true;
			this.chkHighlightChrTile.Click += new System.EventHandler(this.chkHighlightChrTile_Click);
			// 
			// mnuCopyNametableHdPack
			// 
			this.mnuCopyNametableHdPack.Name = "mnuCopyNametableHdPack";
			this.mnuCopyNametableHdPack.Size = new System.Drawing.Size(260, 22);
			this.mnuCopyNametableHdPack.Text = "Copy Nametable (HD Pack Format)";
			this.mnuCopyNametableHdPack.Click += new System.EventHandler(this.mnuCopyNametableHdPack_Click);
			// 
			// ctrlNametableViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlNametableViewer";
			this.Size = new System.Drawing.Size(697, 486);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNametable)).EndInit();
			this.ctxMenu.ResumeLayout(false);
			this.grpTileInfo.ResumeLayout(false);
			this.grpTileInfo.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox picNametable;
		private System.Windows.Forms.GroupBox grpTileInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox txtPaletteAddress;
		private System.Windows.Forms.TextBox txtAttributeAddress;
		private System.Windows.Forms.TextBox txtAttributeData;
		private System.Windows.Forms.TextBox txtTileAddress;
		private System.Windows.Forms.Label lblTileIndex;
		private System.Windows.Forms.Label lblTileAddress;
		private System.Windows.Forms.Label lblAttributeData;
		private System.Windows.Forms.Label lblAttributeAddress;
		private System.Windows.Forms.Label lblPaletteAddress;
		private System.Windows.Forms.Label lblTile;
		private System.Windows.Forms.TextBox txtTileIndex;
		private System.Windows.Forms.PictureBox picTile;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.CheckBox chkShowPpuScrollOverlay;
		private System.Windows.Forms.CheckBox chkShowTileGrid;
		private System.Windows.Forms.CheckBox chkShowAttributeGrid;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.TextBox txtNametable;
		private System.Windows.Forms.Label lblNametableIndex;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.TextBox txtPpuAddress;
		private System.Windows.Forms.Label lblPpuAddress;
		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyHdPack;
		private ctrlTilePalette ctrlTilePalette;
		private System.Windows.Forms.CheckBox chkHighlightChrTile;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInChrViewer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyToClipboard;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyNametableHdPack;
	}
}
