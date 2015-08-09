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
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.grpTileInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.txtTileAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTileIndex = new System.Windows.Forms.TextBox();
			this.picTile = new System.Windows.Forms.PictureBox();
			this.picChrBank1 = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblPalette = new System.Windows.Forms.Label();
			this.cboPalette = new System.Windows.Forms.ComboBox();
			this.picChrBank2 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpTileInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.grpTileInfo, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.picChrBank1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.picChrBank2, 0, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(446, 522);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// grpTileInfo
			// 
			this.grpTileInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpTileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpTileInfo.Location = new System.Drawing.Point(263, 31);
			this.grpTileInfo.Name = "grpTileInfo";
			this.tableLayoutPanel3.SetRowSpan(this.grpTileInfo, 2);
			this.grpTileInfo.Size = new System.Drawing.Size(180, 488);
			this.grpTileInfo.TabIndex = 4;
			this.grpTileInfo.TabStop = false;
			this.grpTileInfo.Text = "Tile Info";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.txtTileAddress, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label6, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtTileIndex, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.picTile, 1, 2);
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
			this.tableLayoutPanel4.Size = new System.Drawing.Size(174, 469);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// txtTileAddress
			// 
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
			this.label6.Location = new System.Drawing.Point(3, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Tile:";
			// 
			// txtTileIndex
			// 
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
			this.picTile.Size = new System.Drawing.Size(66, 66);
			this.picTile.TabIndex = 12;
			this.picTile.TabStop = false;
			// 
			// picChrBank1
			// 
			this.picChrBank1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank1.Location = new System.Drawing.Point(1, 1);
			this.picChrBank1.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank1.Name = "picChrBank1";
			this.tableLayoutPanel3.SetRowSpan(this.picChrBank1, 2);
			this.picChrBank1.Size = new System.Drawing.Size(258, 258);
			this.picChrBank1.TabIndex = 0;
			this.picChrBank1.TabStop = false;
			this.picChrBank1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblPalette);
			this.flowLayoutPanel1.Controls.Add(this.cboPalette);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(260, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(166, 28);
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
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
			this.cboPalette.Location = new System.Drawing.Point(52, 3);
			this.cboPalette.Name = "cboPalette";
			this.cboPalette.Size = new System.Drawing.Size(45, 21);
			this.cboPalette.TabIndex = 1;
			this.cboPalette.SelectedIndexChanged += new System.EventHandler(this.cboPalette_SelectedIndexChanged);
			// 
			// picChrBank2
			// 
			this.picChrBank2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank2.Location = new System.Drawing.Point(1, 261);
			this.picChrBank2.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank2.Name = "picChrBank2";
			this.picChrBank2.Size = new System.Drawing.Size(258, 258);
			this.picChrBank2.TabIndex = 1;
			this.picChrBank2.TabStop = false;
			this.picChrBank2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// ctrlChrViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlChrViewer";
			this.Size = new System.Drawing.Size(446, 522);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpTileInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox picChrBank2;
		private System.Windows.Forms.PictureBox picChrBank1;
		private System.Windows.Forms.GroupBox grpTileInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtTileAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtTileIndex;
		private System.Windows.Forms.PictureBox picTile;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblPalette;
		private System.Windows.Forms.ComboBox cboPalette;
	}
}
