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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkShowPpuScrollOverlay = new System.Windows.Forms.CheckBox();
			this.picNametable = new System.Windows.Forms.PictureBox();
			this.grpTileInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
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
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNametable)).BeginInit();
			this.grpTileInfo.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.picNametable, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpTileInfo, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkShowPpuScrollOverlay, 1, 1);
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
			// chkShowPpuScrollOverlay
			// 
			this.chkShowPpuScrollOverlay.AutoSize = true;
			this.chkShowPpuScrollOverlay.Location = new System.Drawing.Point(519, 233);
			this.chkShowPpuScrollOverlay.Name = "chkShowPpuScrollOverlay";
			this.chkShowPpuScrollOverlay.Size = new System.Drawing.Size(146, 17);
			this.chkShowPpuScrollOverlay.TabIndex = 0;
			this.chkShowPpuScrollOverlay.Text = "Show PPU Scroll Overlay";
			this.chkShowPpuScrollOverlay.UseVisualStyleBackColor = true;
			// 
			// picNametable
			// 
			this.picNametable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picNametable.Location = new System.Drawing.Point(1, 1);
			this.picNametable.Margin = new System.Windows.Forms.Padding(1);
			this.picNametable.Name = "picNametable";
			this.tableLayoutPanel1.SetRowSpan(this.picNametable, 2);
			this.picNametable.Size = new System.Drawing.Size(514, 482);
			this.picNametable.TabIndex = 0;
			this.picNametable.TabStop = false;
			this.picNametable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picNametable_MouseMove);
			// 
			// grpTileInfo
			// 
			this.grpTileInfo.Controls.Add(this.tableLayoutPanel2);
			this.grpTileInfo.Location = new System.Drawing.Point(519, 3);
			this.grpTileInfo.Name = "grpTileInfo";
			this.grpTileInfo.Size = new System.Drawing.Size(175, 224);
			this.grpTileInfo.TabIndex = 4;
			this.grpTileInfo.TabStop = false;
			this.grpTileInfo.Text = "Tile Info";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.txtPaletteAddress, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.txtAttributeAddress, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.txtAttributeData, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtTileAddress, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblTileIndex, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblTileAddress, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblAttributeData, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblAttributeAddress, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblPaletteAddress, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.lblTile, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.txtTileIndex, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.picTile, 1, 5);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 7;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(169, 205);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.Location = new System.Drawing.Point(99, 107);
			this.txtPaletteAddress.Name = "txtPaletteAddress";
			this.txtPaletteAddress.ReadOnly = true;
			this.txtPaletteAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPaletteAddress.TabIndex = 11;
			// 
			// txtAttributeAddress
			// 
			this.txtAttributeAddress.Location = new System.Drawing.Point(99, 81);
			this.txtAttributeAddress.Name = "txtAttributeAddress";
			this.txtAttributeAddress.ReadOnly = true;
			this.txtAttributeAddress.Size = new System.Drawing.Size(42, 20);
			this.txtAttributeAddress.TabIndex = 10;
			// 
			// txtAttributeData
			// 
			this.txtAttributeData.Location = new System.Drawing.Point(99, 55);
			this.txtAttributeData.Name = "txtAttributeData";
			this.txtAttributeData.ReadOnly = true;
			this.txtAttributeData.Size = new System.Drawing.Size(26, 20);
			this.txtAttributeData.TabIndex = 9;
			// 
			// txtTileAddress
			// 
			this.txtTileAddress.Location = new System.Drawing.Point(99, 29);
			this.txtTileAddress.Name = "txtTileAddress";
			this.txtTileAddress.ReadOnly = true;
			this.txtTileAddress.Size = new System.Drawing.Size(42, 20);
			this.txtTileAddress.TabIndex = 8;
			// 
			// lblTileIndex
			// 
			this.lblTileIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileIndex.AutoSize = true;
			this.lblTileIndex.Location = new System.Drawing.Point(3, 6);
			this.lblTileIndex.Name = "lblTileIndex";
			this.lblTileIndex.Size = new System.Drawing.Size(56, 13);
			this.lblTileIndex.TabIndex = 0;
			this.lblTileIndex.Text = "Tile Index:";
			// 
			// lblTileAddress
			// 
			this.lblTileAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTileAddress.AutoSize = true;
			this.lblTileAddress.Location = new System.Drawing.Point(3, 32);
			this.lblTileAddress.Name = "lblTileAddress";
			this.lblTileAddress.Size = new System.Drawing.Size(68, 13);
			this.lblTileAddress.TabIndex = 1;
			this.lblTileAddress.Text = "Tile Address:";
			// 
			// lblAttributeData
			// 
			this.lblAttributeData.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAttributeData.AutoSize = true;
			this.lblAttributeData.Location = new System.Drawing.Point(3, 58);
			this.lblAttributeData.Name = "lblAttributeData";
			this.lblAttributeData.Size = new System.Drawing.Size(75, 13);
			this.lblAttributeData.TabIndex = 2;
			this.lblAttributeData.Text = "Attribute Data:";
			// 
			// lblAttributeAddress
			// 
			this.lblAttributeAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAttributeAddress.AutoSize = true;
			this.lblAttributeAddress.Location = new System.Drawing.Point(3, 84);
			this.lblAttributeAddress.Name = "lblAttributeAddress";
			this.lblAttributeAddress.Size = new System.Drawing.Size(90, 13);
			this.lblAttributeAddress.TabIndex = 3;
			this.lblAttributeAddress.Text = "Attribute Address:";
			// 
			// lblPaletteAddress
			// 
			this.lblPaletteAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPaletteAddress.AutoSize = true;
			this.lblPaletteAddress.Location = new System.Drawing.Point(3, 110);
			this.lblPaletteAddress.Name = "lblPaletteAddress";
			this.lblPaletteAddress.Size = new System.Drawing.Size(84, 13);
			this.lblPaletteAddress.TabIndex = 4;
			this.lblPaletteAddress.Text = "Palette Address:";
			// 
			// lblTile
			// 
			this.lblTile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTile.AutoSize = true;
			this.lblTile.Location = new System.Drawing.Point(3, 159);
			this.lblTile.Name = "lblTile";
			this.lblTile.Size = new System.Drawing.Size(27, 13);
			this.lblTile.TabIndex = 6;
			this.lblTile.Text = "Tile:";
			// 
			// txtTileIndex
			// 
			this.txtTileIndex.Location = new System.Drawing.Point(99, 3);
			this.txtTileIndex.Name = "txtTileIndex";
			this.txtTileIndex.ReadOnly = true;
			this.txtTileIndex.Size = new System.Drawing.Size(26, 20);
			this.txtTileIndex.TabIndex = 7;
			// 
			// picTile
			// 
			this.picTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTile.Location = new System.Drawing.Point(99, 133);
			this.picTile.Name = "picTile";
			this.picTile.Size = new System.Drawing.Size(63, 66);
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
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
			this.grpTileInfo.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
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
		private System.Windows.Forms.CheckBox chkShowPpuScrollOverlay;
	}
}
