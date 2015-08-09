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
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpSpriteInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPosition = new System.Windows.Forms.TextBox();
			this.txtPaletteAddress = new System.Windows.Forms.TextBox();
			this.txtTileAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTile = new System.Windows.Forms.Label();
			this.txtTileIndex = new System.Windows.Forms.TextBox();
			this.chkVerticalMirroring = new System.Windows.Forms.CheckBox();
			this.chkHorizontalMirroring = new System.Windows.Forms.CheckBox();
			this.lblPaletteAddr = new System.Windows.Forms.Label();
			this.lblPosition = new System.Windows.Forms.Label();
			this.picTile = new System.Windows.Forms.PictureBox();
			this.picSprites = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpSpriteInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
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
			this.tableLayoutPanel3.Size = new System.Drawing.Size(682, 516);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// grpSpriteInfo
			// 
			this.grpSpriteInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpSpriteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSpriteInfo.Location = new System.Drawing.Point(263, 3);
			this.grpSpriteInfo.Name = "grpSpriteInfo";
			this.grpSpriteInfo.Size = new System.Drawing.Size(416, 510);
			this.grpSpriteInfo.TabIndex = 4;
			this.grpSpriteInfo.TabStop = false;
			this.grpSpriteInfo.Text = "Sprite Info";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.txtPosition, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.txtPaletteAddress, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtTileAddress, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblTile, 0, 6);
			this.tableLayoutPanel4.Controls.Add(this.txtTileIndex, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.picTile, 1, 6);
			this.tableLayoutPanel4.Controls.Add(this.chkVerticalMirroring, 0, 5);
			this.tableLayoutPanel4.Controls.Add(this.chkHorizontalMirroring, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.lblPaletteAddr, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.lblPosition, 0, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 8;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(410, 491);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// txtPosition
			// 
			this.txtPosition.Location = new System.Drawing.Point(93, 81);
			this.txtPosition.Name = "txtPosition";
			this.txtPosition.ReadOnly = true;
			this.txtPosition.Size = new System.Drawing.Size(42, 20);
			this.txtPosition.TabIndex = 18;
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.Location = new System.Drawing.Point(93, 55);
			this.txtPaletteAddress.Name = "txtPaletteAddress";
			this.txtPaletteAddress.ReadOnly = true;
			this.txtPaletteAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPaletteAddress.TabIndex = 17;
			// 
			// txtTileAddress
			// 
			this.txtTileAddress.Location = new System.Drawing.Point(93, 29);
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
			// lblTile
			// 
			this.lblTile.AutoSize = true;
			this.lblTile.Location = new System.Drawing.Point(3, 155);
			this.lblTile.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblTile.Name = "lblTile";
			this.lblTile.Size = new System.Drawing.Size(27, 13);
			this.lblTile.TabIndex = 6;
			this.lblTile.Text = "Tile:";
			// 
			// txtTileIndex
			// 
			this.txtTileIndex.Location = new System.Drawing.Point(93, 3);
			this.txtTileIndex.Name = "txtTileIndex";
			this.txtTileIndex.ReadOnly = true;
			this.txtTileIndex.Size = new System.Drawing.Size(26, 20);
			this.txtTileIndex.TabIndex = 7;
			// 
			// chkVerticalMirroring
			// 
			this.chkVerticalMirroring.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.chkVerticalMirroring, 2);
			this.chkVerticalMirroring.Location = new System.Drawing.Point(3, 130);
			this.chkVerticalMirroring.Name = "chkVerticalMirroring";
			this.chkVerticalMirroring.Size = new System.Drawing.Size(77, 17);
			this.chkVerticalMirroring.TabIndex = 14;
			this.chkVerticalMirroring.Text = "Vertical flip";
			this.chkVerticalMirroring.UseVisualStyleBackColor = true;
			// 
			// chkHorizontalMirroring
			// 
			this.chkHorizontalMirroring.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.chkHorizontalMirroring, 2);
			this.chkHorizontalMirroring.Location = new System.Drawing.Point(3, 107);
			this.chkHorizontalMirroring.Name = "chkHorizontalMirroring";
			this.chkHorizontalMirroring.Size = new System.Drawing.Size(89, 17);
			this.chkHorizontalMirroring.TabIndex = 13;
			this.chkHorizontalMirroring.Text = "Horizontal flip";
			this.chkHorizontalMirroring.UseVisualStyleBackColor = true;
			// 
			// lblPaletteAddr
			// 
			this.lblPaletteAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPaletteAddr.AutoSize = true;
			this.lblPaletteAddr.Location = new System.Drawing.Point(3, 58);
			this.lblPaletteAddr.Name = "lblPaletteAddr";
			this.lblPaletteAddr.Size = new System.Drawing.Size(84, 13);
			this.lblPaletteAddr.TabIndex = 15;
			this.lblPaletteAddr.Text = "Palette Address:";
			// 
			// lblPosition
			// 
			this.lblPosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPosition.AutoSize = true;
			this.lblPosition.Location = new System.Drawing.Point(3, 84);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(70, 13);
			this.lblPosition.TabIndex = 16;
			this.lblPosition.Text = "Position (X,Y)";
			// 
			// picTile
			// 
			this.picTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.picTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTile.Location = new System.Drawing.Point(93, 153);
			this.picTile.Name = "picTile";
			this.picTile.Size = new System.Drawing.Size(66, 130);
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
			// 
			// picSprites
			// 
			this.picSprites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picSprites.Location = new System.Drawing.Point(1, 1);
			this.picSprites.Margin = new System.Windows.Forms.Padding(1);
			this.picSprites.Name = "picSprites";
			this.picSprites.Size = new System.Drawing.Size(258, 514);
			this.picSprites.TabIndex = 0;
			this.picSprites.TabStop = false;
			this.picSprites.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSprites_MouseMove);
			// 
			// ctrlSpriteViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlSpriteViewer";
			this.Size = new System.Drawing.Size(682, 516);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpSpriteInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSprites)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox picSprites;
		private System.Windows.Forms.GroupBox grpSpriteInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtTileAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblTile;
		private System.Windows.Forms.TextBox txtTileIndex;
		private System.Windows.Forms.PictureBox picTile;
		private System.Windows.Forms.TextBox txtPosition;
		private System.Windows.Forms.TextBox txtPaletteAddress;
		private System.Windows.Forms.CheckBox chkVerticalMirroring;
		private System.Windows.Forms.CheckBox chkHorizontalMirroring;
		private System.Windows.Forms.Label lblPaletteAddr;
		private System.Windows.Forms.Label lblPosition;
	}
}
