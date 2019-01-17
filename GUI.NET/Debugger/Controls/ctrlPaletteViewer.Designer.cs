namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlPaletteViewer
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
			this.grpColorInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPaletteAddress = new System.Windows.Forms.TextBox();
			this.lblColor = new System.Windows.Forms.Label();
			this.lblPaletteAddress = new System.Windows.Forms.Label();
			this.lblColorTile = new System.Windows.Forms.Label();
			this.txtColor = new System.Windows.Forms.TextBox();
			this.picColor = new System.Windows.Forms.PictureBox();
			this.lblColorHex = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtColorCodeHex = new System.Windows.Forms.TextBox();
			this.txtColorCodeRgb = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPaletteSprites = new System.Windows.Forms.Label();
			this.picPaletteSprites = new System.Windows.Forms.PictureBox();
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCopyHexColor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyRgbColor = new System.Windows.Forms.ToolStripMenuItem();
			this.picPaletteBg = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.lblClickColorHint = new System.Windows.Forms.Label();
			this.lblBgPalette = new System.Windows.Forms.Label();
			this.grpColorInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSprites)).BeginInit();
			this.ctxMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteBg)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			this.SuspendLayout();
			// 
			// grpColorInfo
			// 
			this.grpColorInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpColorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpColorInfo.Location = new System.Drawing.Point(273, 3);
			this.grpColorInfo.Name = "grpColorInfo";
			this.tableLayoutPanel3.SetRowSpan(this.grpColorInfo, 3);
			this.grpColorInfo.Size = new System.Drawing.Size(406, 305);
			this.grpColorInfo.TabIndex = 4;
			this.grpColorInfo.TabStop = false;
			this.grpColorInfo.Text = "Color Info";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.txtPaletteAddress, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblColor, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.lblPaletteAddress, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblColorTile, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtColor, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.picColor, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.lblColorHex, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.txtColorCodeHex, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.txtColorCodeRgb, 1, 4);
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
			this.tableLayoutPanel4.Size = new System.Drawing.Size(400, 286);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtPaletteAddress.Location = new System.Drawing.Point(103, 29);
			this.txtPaletteAddress.Name = "txtPaletteAddress";
			this.txtPaletteAddress.ReadOnly = true;
			this.txtPaletteAddress.Size = new System.Drawing.Size(42, 20);
			this.txtPaletteAddress.TabIndex = 8;
			// 
			// lblColor
			// 
			this.lblColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(3, 6);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(34, 13);
			this.lblColor.TabIndex = 0;
			this.lblColor.Text = "Color:";
			// 
			// lblPaletteAddress
			// 
			this.lblPaletteAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPaletteAddress.AutoSize = true;
			this.lblPaletteAddress.Location = new System.Drawing.Point(3, 32);
			this.lblPaletteAddress.Name = "lblPaletteAddress";
			this.lblPaletteAddress.Size = new System.Drawing.Size(84, 13);
			this.lblPaletteAddress.TabIndex = 1;
			this.lblPaletteAddress.Text = "Palette Address:";
			// 
			// lblColorTile
			// 
			this.lblColorTile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblColorTile.AutoSize = true;
			this.lblColorTile.Location = new System.Drawing.Point(3, 81);
			this.lblColorTile.Name = "lblColorTile";
			this.lblColorTile.Size = new System.Drawing.Size(48, 13);
			this.lblColorTile.TabIndex = 6;
			this.lblColorTile.Text = "Preview:";
			// 
			// txtColor
			// 
			this.txtColor.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtColor.Location = new System.Drawing.Point(103, 3);
			this.txtColor.Name = "txtColor";
			this.txtColor.ReadOnly = true;
			this.txtColor.Size = new System.Drawing.Size(26, 20);
			this.txtColor.TabIndex = 7;
			// 
			// picColor
			// 
			this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picColor.Location = new System.Drawing.Point(103, 55);
			this.picColor.Name = "picColor";
			this.picColor.Size = new System.Drawing.Size(66, 66);
			this.picColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picColor.TabIndex = 12;
			this.picColor.TabStop = false;
			// 
			// lblColorHex
			// 
			this.lblColorHex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblColorHex.AutoSize = true;
			this.lblColorHex.Location = new System.Drawing.Point(3, 130);
			this.lblColorHex.Name = "lblColorHex";
			this.lblColorHex.Size = new System.Drawing.Size(90, 13);
			this.lblColorHex.TabIndex = 13;
			this.lblColorHex.Text = "Color Code (Hex):";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 156);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Color Code (RGB):";
			// 
			// txtColorCodeHex
			// 
			this.txtColorCodeHex.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtColorCodeHex.Location = new System.Drawing.Point(103, 127);
			this.txtColorCodeHex.Name = "txtColorCodeHex";
			this.txtColorCodeHex.ReadOnly = true;
			this.txtColorCodeHex.Size = new System.Drawing.Size(66, 20);
			this.txtColorCodeHex.TabIndex = 15;
			// 
			// txtColorCodeRgb
			// 
			this.txtColorCodeRgb.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtColorCodeRgb.Location = new System.Drawing.Point(103, 153);
			this.txtColorCodeRgb.Name = "txtColorCodeRgb";
			this.txtColorCodeRgb.ReadOnly = true;
			this.txtColorCodeRgb.Size = new System.Drawing.Size(108, 20);
			this.txtColorCodeRgb.TabIndex = 16;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.lblPaletteSprites, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.picPaletteSprites, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.grpColorInfo, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.picPaletteBg, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.lblBgPalette, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(682, 311);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// lblPaletteSprites
			// 
			this.lblPaletteSprites.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblPaletteSprites.AutoSize = true;
			this.lblPaletteSprites.Location = new System.Drawing.Point(183, 138);
			this.lblPaletteSprites.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPaletteSprites.Name = "lblPaletteSprites";
			this.lblPaletteSprites.Size = new System.Drawing.Size(39, 13);
			this.lblPaletteSprites.TabIndex = 8;
			this.lblPaletteSprites.Text = "Sprites";
			// 
			// picPaletteSprites
			// 
			this.picPaletteSprites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPaletteSprites.ContextMenuStrip = this.ctxMenu;
			this.picPaletteSprites.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPaletteSprites.Location = new System.Drawing.Point(138, 4);
			this.picPaletteSprites.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.picPaletteSprites.Name = "picPaletteSprites";
			this.picPaletteSprites.Size = new System.Drawing.Size(130, 130);
			this.picPaletteSprites.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPaletteSprites.TabIndex = 6;
			this.picPaletteSprites.TabStop = false;
			this.picPaletteSprites.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
			this.picPaletteSprites.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
			// 
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyHexColor,
            this.mnuCopyRgbColor});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(160, 48);
			this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
			// 
			// mnuCopyHexColor
			// 
			this.mnuCopyHexColor.Name = "mnuCopyHexColor";
			this.mnuCopyHexColor.Size = new System.Drawing.Size(159, 22);
			this.mnuCopyHexColor.Text = "Copy Hex Color";
			// 
			// mnuCopyRgbColor
			// 
			this.mnuCopyRgbColor.Name = "mnuCopyRgbColor";
			this.mnuCopyRgbColor.Size = new System.Drawing.Size(159, 22);
			this.mnuCopyRgbColor.Text = "Copy RGB Color";
			// 
			// picPaletteBg
			// 
			this.picPaletteBg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPaletteBg.ContextMenuStrip = this.ctxMenu;
			this.picPaletteBg.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPaletteBg.Location = new System.Drawing.Point(4, 4);
			this.picPaletteBg.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
			this.picPaletteBg.Name = "picPaletteBg";
			this.picPaletteBg.Size = new System.Drawing.Size(130, 130);
			this.picPaletteBg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPaletteBg.TabIndex = 0;
			this.picPaletteBg.TabStop = false;
			this.picPaletteBg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
			this.picPaletteBg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.picHelp);
			this.flowLayoutPanel1.Controls.Add(this.lblClickColorHint);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 154);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(264, 26);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// picHelp
			// 
			this.picHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picHelp.Location = new System.Drawing.Point(3, 5);
			this.picHelp.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picHelp.Name = "picHelp";
			this.picHelp.Size = new System.Drawing.Size(18, 18);
			this.picHelp.TabIndex = 9;
			this.picHelp.TabStop = false;
			// 
			// lblClickColorHint
			// 
			this.lblClickColorHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClickColorHint.AutoSize = true;
			this.lblClickColorHint.Location = new System.Drawing.Point(27, 6);
			this.lblClickColorHint.Name = "lblClickColorHint";
			this.lblClickColorHint.Size = new System.Drawing.Size(139, 13);
			this.lblClickColorHint.TabIndex = 5;
			this.lblClickColorHint.Text = "Click on a color to change it";
			// 
			// lblBgPalette
			// 
			this.lblBgPalette.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblBgPalette.AutoSize = true;
			this.lblBgPalette.Location = new System.Drawing.Point(36, 138);
			this.lblBgPalette.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
			this.lblBgPalette.Name = "lblBgPalette";
			this.lblBgPalette.Size = new System.Drawing.Size(65, 13);
			this.lblBgPalette.TabIndex = 7;
			this.lblBgPalette.Text = "Background";
			// 
			// ctrlPaletteViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlPaletteViewer";
			this.Size = new System.Drawing.Size(682, 311);
			this.grpColorInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSprites)).EndInit();
			this.ctxMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPaletteBg)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPaletteBg;
		private System.Windows.Forms.GroupBox grpColorInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtPaletteAddress;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.Label lblPaletteAddress;
		private System.Windows.Forms.Label lblColorTile;
		private System.Windows.Forms.TextBox txtColor;
		private System.Windows.Forms.PictureBox picColor;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label lblClickColorHint;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox picHelp;
		private System.Windows.Forms.Label lblColorHex;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtColorCodeHex;
		private System.Windows.Forms.TextBox txtColorCodeRgb;
		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyHexColor;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyRgbColor;
		private System.Windows.Forms.PictureBox picPaletteSprites;
		private System.Windows.Forms.Label lblBgPalette;
		private System.Windows.Forms.Label lblPaletteSprites;
	}
}
