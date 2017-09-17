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
			this.grpColorInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPaletteAddress = new System.Windows.Forms.TextBox();
			this.lblColor = new System.Windows.Forms.Label();
			this.lblPaletteAddress = new System.Windows.Forms.Label();
			this.lblColorTile = new System.Windows.Forms.Label();
			this.txtColor = new System.Windows.Forms.TextBox();
			this.picColor = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.picPalette = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.lblClickColorHint = new System.Windows.Forms.Label();
			this.grpColorInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			this.SuspendLayout();
			// 
			// grpColorInfo
			// 
			this.grpColorInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpColorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpColorInfo.Location = new System.Drawing.Point(135, 3);
			this.grpColorInfo.Name = "grpColorInfo";
			this.tableLayoutPanel3.SetRowSpan(this.grpColorInfo, 2);
			this.grpColorInfo.Size = new System.Drawing.Size(544, 305);
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
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(538, 286);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// txtPaletteAddress
			// 
			this.txtPaletteAddress.Location = new System.Drawing.Point(93, 29);
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
			this.txtColor.Location = new System.Drawing.Point(93, 3);
			this.txtColor.Name = "txtColor";
			this.txtColor.ReadOnly = true;
			this.txtColor.Size = new System.Drawing.Size(26, 20);
			this.txtColor.TabIndex = 7;
			// 
			// picColor
			// 
			this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picColor.Location = new System.Drawing.Point(93, 55);
			this.picColor.Name = "picColor";
			this.picColor.Size = new System.Drawing.Size(66, 66);
			this.picColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picColor.TabIndex = 12;
			this.picColor.TabStop = false;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.grpColorInfo, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.picPalette, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(682, 311);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// picPalette
			// 
			this.picPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPalette.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPalette.Location = new System.Drawing.Point(1, 1);
			this.picPalette.Margin = new System.Windows.Forms.Padding(1);
			this.picPalette.Name = "picPalette";
			this.picPalette.Size = new System.Drawing.Size(130, 258);
			this.picPalette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPalette.TabIndex = 0;
			this.picPalette.TabStop = false;
			this.picPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
			this.picPalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.picHelp);
			this.flowLayoutPanel1.Controls.Add(this.lblClickColorHint);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 263);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(122, 42);
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
			this.lblClickColorHint.Location = new System.Drawing.Point(27, 0);
			this.lblClickColorHint.Name = "lblClickColorHint";
			this.lblClickColorHint.Size = new System.Drawing.Size(92, 42);
			this.lblClickColorHint.TabIndex = 5;
			this.lblClickColorHint.Text = "Click on a color to change it";
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
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPalette;
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
	}
}
