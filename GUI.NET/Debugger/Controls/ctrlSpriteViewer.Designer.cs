namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlSpriteViewer
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
			this.grpSpriteInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPalette = new System.Windows.Forms.Label();
			this.txtSpriteIndex = new System.Windows.Forms.TextBox();
			this.lblSpriteIndex = new System.Windows.Forms.Label();
			this.picPreview = new System.Windows.Forms.PictureBox();
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowInChrViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCopyHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyAllSpritesHdPack = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.lblScreenPreview = new System.Windows.Forms.Label();
			this.lblTile = new System.Windows.Forms.Label();
			this.picTile = new System.Windows.Forms.PictureBox();
			this.lblPosition = new System.Windows.Forms.Label();
			this.txtPosition = new System.Windows.Forms.TextBox();
			this.lblTileIndex = new System.Windows.Forms.Label();
			this.txtTileIndex = new System.Windows.Forms.TextBox();
			this.lblPaletteAddr = new System.Windows.Forms.Label();
			this.lblTileAddress = new System.Windows.Forms.Label();
			this.txtPaletteAddress = new System.Windows.Forms.TextBox();
			this.txtTileAddress = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkHorizontalMirroring = new System.Windows.Forms.CheckBox();
			this.chkVerticalMirroring = new System.Windows.Forms.CheckBox();
			this.chkBackgroundPriority = new System.Windows.Forms.CheckBox();
			this.ctrlTilePalette = new Mesen.GUI.Debugger.Controls.ctrlTilePalette();
			this.picSprites = new System.Windows.Forms.PictureBox();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExportToPng = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpSpriteInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
			this.ctxMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSprites)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.grpSpriteInfo, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.picSprites, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(682, 527);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// grpSpriteInfo
			// 
			this.grpSpriteInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpSpriteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSpriteInfo.Location = new System.Drawing.Point(269, 3);
			this.grpSpriteInfo.Name = "grpSpriteInfo";
			this.grpSpriteInfo.Size = new System.Drawing.Size(410, 521);
			this.grpSpriteInfo.TabIndex = 4;
			this.grpSpriteInfo.TabStop = false;
			this.grpSpriteInfo.Text = "Sprite Info";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 5;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.lblPalette, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.txtSpriteIndex, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblSpriteIndex, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.picPreview, 1, 5);
			this.tableLayoutPanel4.Controls.Add(this.lblScreenPreview, 0, 5);
			this.tableLayoutPanel4.Controls.Add(this.lblTile, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.picTile, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.lblPosition, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtPosition, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.lblTileIndex, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.txtTileIndex, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblPaletteAddr, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblTileAddress, 2, 1);
			this.tableLayoutPanel4.Controls.Add(this.txtPaletteAddress, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.txtTileAddress, 3, 1);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.ctrlTilePalette, 1, 4);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 6;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(404, 502);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// lblPalette
			// 
			this.lblPalette.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPalette.AutoSize = true;
			this.lblPalette.Location = new System.Drawing.Point(3, 230);
			this.lblPalette.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblPalette.Name = "lblPalette";
			this.lblPalette.Size = new System.Drawing.Size(43, 13);
			this.lblPalette.TabIndex = 26;
			this.lblPalette.Text = "Palette:";
			// 
			// txtSpriteIndex
			// 
			this.txtSpriteIndex.BackColor = System.Drawing.SystemColors.Window;
			this.txtSpriteIndex.Location = new System.Drawing.Point(94, 3);
			this.txtSpriteIndex.Name = "txtSpriteIndex";
			this.txtSpriteIndex.ReadOnly = true;
			this.txtSpriteIndex.Size = new System.Drawing.Size(26, 20);
			this.txtSpriteIndex.TabIndex = 23;
			// 
			// lblSpriteIndex
			// 
			this.lblSpriteIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSpriteIndex.AutoSize = true;
			this.lblSpriteIndex.Location = new System.Drawing.Point(3, 6);
			this.lblSpriteIndex.Name = "lblSpriteIndex";
			this.lblSpriteIndex.Size = new System.Drawing.Size(66, 13);
			this.lblSpriteIndex.TabIndex = 22;
			this.lblSpriteIndex.Text = "Sprite Index:";
			// 
			// picPreview
			// 
			this.picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel4.SetColumnSpan(this.picPreview, 4);
			this.picPreview.ContextMenuStrip = this.ctxMenu;
			this.picPreview.Location = new System.Drawing.Point(94, 257);
			this.picPreview.Name = "picPreview";
			this.picPreview.Size = new System.Drawing.Size(258, 242);
			this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPreview.TabIndex = 21;
			this.picPreview.TabStop = false;
			this.picPreview.DoubleClick += new System.EventHandler(this.picSprites_DoubleClick);
			this.picPreview.MouseEnter += new System.EventHandler(this.picPreview_MouseEnter);
			this.picPreview.MouseLeave += new System.EventHandler(this.picPreview_MouseLeave);
			this.picPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPreview_MouseMove);
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowInChrViewer,
            this.toolStripMenuItem1,
            this.mnuCopyHdPack,
            this.mnuCopyAllSpritesHdPack,
            this.toolStripMenuItem2,
            this.mnuCopyToClipboard,
            this.mnuExportToPng});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(255, 148);
			this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
			// 
			// mnuShowInChrViewer
			// 
			this.mnuShowInChrViewer.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuShowInChrViewer.Name = "mnuShowInChrViewer";
			this.mnuShowInChrViewer.ShortcutKeyDisplayString = "Dbl-Click";
			this.mnuShowInChrViewer.Size = new System.Drawing.Size(254, 22);
			this.mnuShowInChrViewer.Text = "View in CHR viewer";
			this.mnuShowInChrViewer.Click += new System.EventHandler(this.mnuShowInChrViewer_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
			// 
			// mnuCopyHdPack
			// 
			this.mnuCopyHdPack.Name = "mnuCopyHdPack";
			this.mnuCopyHdPack.Size = new System.Drawing.Size(254, 22);
			this.mnuCopyHdPack.Text = "Copy Tile (HD Pack Format)";
			this.mnuCopyHdPack.Click += new System.EventHandler(this.mnuCopyHdPack_Click);
			// 
			// mnuCopyAllSpritesHdPack
			// 
			this.mnuCopyAllSpritesHdPack.Name = "mnuCopyAllSpritesHdPack";
			this.mnuCopyAllSpritesHdPack.Size = new System.Drawing.Size(254, 22);
			this.mnuCopyAllSpritesHdPack.Text = "Copy All Sprites (HD Pack Format)";
			this.mnuCopyAllSpritesHdPack.Click += new System.EventHandler(this.mnuCopyAllSpritesHdPack_Click);
			// 
			// mnuCopyToClipboard
			// 
			this.mnuCopyToClipboard.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCopyToClipboard.Name = "mnuCopyToClipboard";
			this.mnuCopyToClipboard.Size = new System.Drawing.Size(254, 22);
			this.mnuCopyToClipboard.Text = "Copy image to clipboard";
			this.mnuCopyToClipboard.Click += new System.EventHandler(this.mnuCopyToClipboard_Click);
			// 
			// lblScreenPreview
			// 
			this.lblScreenPreview.AutoSize = true;
			this.lblScreenPreview.Location = new System.Drawing.Point(3, 259);
			this.lblScreenPreview.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblScreenPreview.Name = "lblScreenPreview";
			this.lblScreenPreview.Size = new System.Drawing.Size(85, 13);
			this.lblScreenPreview.TabIndex = 20;
			this.lblScreenPreview.Text = "Screen Preview:";
			// 
			// lblTile
			// 
			this.lblTile.AutoSize = true;
			this.lblTile.Location = new System.Drawing.Point(3, 83);
			this.lblTile.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblTile.Name = "lblTile";
			this.lblTile.Size = new System.Drawing.Size(27, 13);
			this.lblTile.TabIndex = 6;
			this.lblTile.Text = "Tile:";
			// 
			// picTile
			// 
			this.picTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.picTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTile.Location = new System.Drawing.Point(94, 81);
			this.picTile.Name = "picTile";
			this.picTile.Size = new System.Drawing.Size(66, 130);
			this.picTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
			// 
			// lblPosition
			// 
			this.lblPosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPosition.AutoSize = true;
			this.lblPosition.Location = new System.Drawing.Point(3, 58);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(73, 13);
			this.lblPosition.TabIndex = 16;
			this.lblPosition.Text = "Position (X,Y):";
			// 
			// txtPosition
			// 
			this.txtPosition.BackColor = System.Drawing.SystemColors.Window;
			this.txtPosition.Location = new System.Drawing.Point(94, 55);
			this.txtPosition.Name = "txtPosition";
			this.txtPosition.ReadOnly = true;
			this.txtPosition.Size = new System.Drawing.Size(66, 20);
			this.txtPosition.TabIndex = 18;
			// 
			// lblTileIndex
			// 
			this.lblTileIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileIndex.AutoSize = true;
			this.lblTileIndex.Location = new System.Drawing.Point(3, 32);
			this.lblTileIndex.Name = "lblTileIndex";
			this.lblTileIndex.Size = new System.Drawing.Size(56, 13);
			this.lblTileIndex.TabIndex = 0;
			this.lblTileIndex.Text = "Tile Index:";
			// 
			// txtTileIndex
			// 
			this.txtTileIndex.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileIndex.Location = new System.Drawing.Point(94, 29);
			this.txtTileIndex.Name = "txtTileIndex";
			this.txtTileIndex.ReadOnly = true;
			this.txtTileIndex.Size = new System.Drawing.Size(26, 20);
			this.txtTileIndex.TabIndex = 7;
			// 
			// lblPaletteAddr
			// 
			this.lblPaletteAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPaletteAddr.AutoSize = true;
			this.lblPaletteAddr.Location = new System.Drawing.Point(166, 6);
			this.lblPaletteAddr.Name = "lblPaletteAddr";
			this.lblPaletteAddr.Size = new System.Drawing.Size(84, 13);
			this.lblPaletteAddr.TabIndex = 15;
			this.lblPaletteAddr.Text = "Palette Address:";
			// 
			// lblTileAddress
			// 
			this.lblTileAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileAddress.AutoSize = true;
			this.lblTileAddress.Location = new System.Drawing.Point(166, 32);
			this.lblTileAddress.Name = "lblTileAddress";
			this.lblTileAddress.Size = new System.Drawing.Size(68, 13);
			this.lblTileAddress.TabIndex = 1;
			this.lblTileAddress.Text = "Tile Address:";
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtPaletteAddress.Location = new System.Drawing.Point(256, 3);
			this.txtPaletteAddress.Name = "txtPaletteAddress";
			this.txtPaletteAddress.ReadOnly = true;
			this.txtPaletteAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPaletteAddress.TabIndex = 17;
			// 
			// txtTileAddress
			// 
			this.txtTileAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtTileAddress.Location = new System.Drawing.Point(256, 29);
			this.txtTileAddress.Name = "txtTileAddress";
			this.txtTileAddress.ReadOnly = true;
			this.txtTileAddress.Size = new System.Drawing.Size(42, 20);
			this.txtTileAddress.TabIndex = 8;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkHorizontalMirroring, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkVerticalMirroring, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkBackgroundPriority, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(163, 52);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel4.SetRowSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(130, 75);
			this.tableLayoutPanel1.TabIndex = 24;
			// 
			// chkHorizontalMirroring
			// 
			this.chkHorizontalMirroring.AutoCheck = false;
			this.chkHorizontalMirroring.AutoSize = true;
			this.chkHorizontalMirroring.Location = new System.Drawing.Point(6, 6);
			this.chkHorizontalMirroring.Name = "chkHorizontalMirroring";
			this.chkHorizontalMirroring.Size = new System.Drawing.Size(89, 17);
			this.chkHorizontalMirroring.TabIndex = 13;
			this.chkHorizontalMirroring.Text = "Horizontal flip";
			this.chkHorizontalMirroring.UseVisualStyleBackColor = true;
			// 
			// chkVerticalMirroring
			// 
			this.chkVerticalMirroring.AutoCheck = false;
			this.chkVerticalMirroring.AutoSize = true;
			this.chkVerticalMirroring.Location = new System.Drawing.Point(6, 29);
			this.chkVerticalMirroring.Name = "chkVerticalMirroring";
			this.chkVerticalMirroring.Size = new System.Drawing.Size(77, 17);
			this.chkVerticalMirroring.TabIndex = 14;
			this.chkVerticalMirroring.Text = "Vertical flip";
			this.chkVerticalMirroring.UseVisualStyleBackColor = true;
			// 
			// chkBackgroundPriority
			// 
			this.chkBackgroundPriority.AutoCheck = false;
			this.chkBackgroundPriority.AutoSize = true;
			this.chkBackgroundPriority.Location = new System.Drawing.Point(6, 52);
			this.chkBackgroundPriority.Name = "chkBackgroundPriority";
			this.chkBackgroundPriority.Size = new System.Drawing.Size(118, 17);
			this.chkBackgroundPriority.TabIndex = 19;
			this.chkBackgroundPriority.Text = "Background Priority";
			this.chkBackgroundPriority.UseVisualStyleBackColor = true;
			// 
			// ctrlTilePalette
			// 
			this.ctrlTilePalette.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.tableLayoutPanel4.SetColumnSpan(this.ctrlTilePalette, 2);
			this.ctrlTilePalette.DisplayIndexes = false;
			this.ctrlTilePalette.HighlightMouseOver = false;
			this.ctrlTilePalette.Location = new System.Drawing.Point(94, 217);
			this.ctrlTilePalette.Name = "ctrlTilePalette";
			this.ctrlTilePalette.Size = new System.Drawing.Size(130, 34);
			this.ctrlTilePalette.TabIndex = 25;
			// 
			// picSprites
			// 
			this.picSprites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picSprites.ContextMenuStrip = this.ctxMenu;
			this.picSprites.Location = new System.Drawing.Point(4, 4);
			this.picSprites.Margin = new System.Windows.Forms.Padding(4);
			this.picSprites.Name = "picSprites";
			this.picSprites.Size = new System.Drawing.Size(258, 514);
			this.picSprites.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picSprites.TabIndex = 0;
			this.picSprites.TabStop = false;
			this.picSprites.DoubleClick += new System.EventHandler(this.picSprites_DoubleClick);
			this.picSprites.MouseEnter += new System.EventHandler(this.picSprites_MouseEnter);
			this.picSprites.MouseLeave += new System.EventHandler(this.picSprites_MouseLeave);
			this.picSprites.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSprites_MouseMove);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(251, 6);
			// 
			// mnuExportToPng
			// 
			this.mnuExportToPng.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportToPng.Name = "mnuExportToPng";
			this.mnuExportToPng.Size = new System.Drawing.Size(254, 22);
			this.mnuExportToPng.Text = "Export image to PNG";
			this.mnuExportToPng.Click += new System.EventHandler(this.mnuExportToPng_Click);
			// 
			// ctrlSpriteViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlSpriteViewer";
			this.Size = new System.Drawing.Size(682, 527);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpSpriteInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
			this.ctxMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSprites)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox picSprites;
		private System.Windows.Forms.GroupBox grpSpriteInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtTileAddress;
		private System.Windows.Forms.Label lblTileIndex;
		private System.Windows.Forms.Label lblTileAddress;
		private System.Windows.Forms.Label lblTile;
		private System.Windows.Forms.TextBox txtTileIndex;
		private System.Windows.Forms.PictureBox picTile;
		private System.Windows.Forms.TextBox txtPosition;
		private System.Windows.Forms.TextBox txtPaletteAddress;
		private System.Windows.Forms.CheckBox chkVerticalMirroring;
		private System.Windows.Forms.CheckBox chkHorizontalMirroring;
		private System.Windows.Forms.Label lblPaletteAddr;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.CheckBox chkBackgroundPriority;
		private System.Windows.Forms.PictureBox picPreview;
		private System.Windows.Forms.Label lblScreenPreview;
		private System.Windows.Forms.Label lblSpriteIndex;
		private System.Windows.Forms.TextBox txtSpriteIndex;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyHdPack;
		private System.Windows.Forms.Label lblPalette;
		private ctrlTilePalette ctrlTilePalette;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInChrViewer;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyToClipboard;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyAllSpritesHdPack;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuExportToPng;
	}
}
