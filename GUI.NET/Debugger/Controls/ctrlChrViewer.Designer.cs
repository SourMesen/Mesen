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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.picChrBank1 = new System.Windows.Forms.PictureBox();
			this.picChrBank2 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblPalette = new System.Windows.Forms.Label();
			this.cboPalette = new System.Windows.Forms.ComboBox();
			this.chkLargeSprites = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblChrSelection = new System.Windows.Forms.Label();
			this.cboChrSelection = new System.Windows.Forms.ComboBox();
			this.flpHighlight = new System.Windows.Forms.FlowLayoutPanel();
			this.lblHighlight = new System.Windows.Forms.Label();
			this.cboHighlightType = new System.Windows.Forms.ComboBox();
			this.grpDisplayOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpTileInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flpHighlight.SuspendLayout();
			this.grpDisplayOptions.SuspendLayout();
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
			// grpTileInfo
			// 
			this.grpTileInfo.Controls.Add(this.tableLayoutPanel4);
			this.grpTileInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpTileInfo.Location = new System.Drawing.Point(267, 3);
			this.grpTileInfo.Name = "grpTileInfo";
			this.grpTileInfo.Size = new System.Drawing.Size(264, 152);
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
			this.tableLayoutPanel4.Size = new System.Drawing.Size(258, 123);
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(258, 519);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// picChrBank1
			// 
			this.picChrBank1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank1.Location = new System.Drawing.Point(1, 1);
			this.picChrBank1.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank1.Name = "picChrBank1";
			this.picChrBank1.Size = new System.Drawing.Size(256, 257);
			this.picChrBank1.TabIndex = 0;
			this.picChrBank1.TabStop = false;
			this.picChrBank1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// picChrBank2
			// 
			this.picChrBank2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrBank2.Location = new System.Drawing.Point(1, 260);
			this.picChrBank2.Margin = new System.Windows.Forms.Padding(1);
			this.picChrBank2.Name = "picChrBank2";
			this.picChrBank2.Size = new System.Drawing.Size(256, 257);
			this.picChrBank2.TabIndex = 1;
			this.picChrBank2.TabStop = false;
			this.picChrBank2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChrBank_MouseMove);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkLargeSprites, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flpHighlight, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(258, 114);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblPalette);
			this.flowLayoutPanel1.Controls.Add(this.cboPalette);
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
			// chkLargeSprites
			// 
			this.chkLargeSprites.AutoSize = true;
			this.chkLargeSprites.Location = new System.Drawing.Point(3, 84);
			this.chkLargeSprites.Name = "chkLargeSprites";
			this.chkLargeSprites.Size = new System.Drawing.Size(133, 17);
			this.chkLargeSprites.TabIndex = 2;
			this.chkLargeSprites.Text = "Display as 8x16 sprites";
			this.chkLargeSprites.UseVisualStyleBackColor = true;
			this.chkLargeSprites.Click += new System.EventHandler(this.chkLargeSprites_Click);
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
			this.lblChrSelection.Size = new System.Drawing.Size(73, 13);
			this.lblChrSelection.TabIndex = 0;
			this.lblChrSelection.Text = "Chr Selection:";
			// 
			// cboChrSelection
			// 
			this.cboChrSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChrSelection.FormattingEnabled = true;
			this.cboChrSelection.Location = new System.Drawing.Point(82, 3);
			this.cboChrSelection.Name = "cboChrSelection";
			this.cboChrSelection.Size = new System.Drawing.Size(150, 21);
			this.cboChrSelection.TabIndex = 1;
			this.cboChrSelection.DropDown += new System.EventHandler(this.cboChrSelection_DropDown);
			this.cboChrSelection.SelectionChangeCommitted += new System.EventHandler(this.cboChrSelection_SelectionChangeCommitted);
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
			// grpDisplayOptions
			// 
			this.grpDisplayOptions.Controls.Add(this.tableLayoutPanel1);
			this.grpDisplayOptions.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpDisplayOptions.Location = new System.Drawing.Point(267, 151);
			this.grpDisplayOptions.Name = "grpDisplayOptions";
			this.grpDisplayOptions.Size = new System.Drawing.Size(264, 133);
			this.grpDisplayOptions.TabIndex = 4;
			this.grpDisplayOptions.TabStop = false;
			this.grpDisplayOptions.Text = "Display Options";
			// 
			// ctrlChrViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlChrViewer";
			this.Size = new System.Drawing.Size(534, 525);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.grpTileInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picChrBank1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrBank2)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flpHighlight.ResumeLayout(false);
			this.flpHighlight.PerformLayout();
			this.grpDisplayOptions.ResumeLayout(false);
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
	}
}
