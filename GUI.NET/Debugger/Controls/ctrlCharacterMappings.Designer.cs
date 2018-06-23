namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlCharacterMappings
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
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblChrSelection = new System.Windows.Forms.Label();
			this.cboChrSelection = new System.Windows.Forms.ComboBox();
			this.grpTileInfo = new System.Windows.Forms.GroupBox();
			this.tlpTileMappings = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblHint = new System.Windows.Forms.Label();
			this.picHint = new System.Windows.Forms.PictureBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.grpTileInfo.SuspendLayout();
			this.tlpTileMappings.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.grpTileInfo, 1, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(816, 492);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.lblChrSelection);
			this.flowLayoutPanel2.Controls.Add(this.cboChrSelection);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(332, 27);
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
			this.cboChrSelection.Size = new System.Drawing.Size(183, 21);
			this.cboChrSelection.TabIndex = 1;
			this.cboChrSelection.DropDown += new System.EventHandler(this.cboChrSelection_DropDown);
			this.cboChrSelection.SelectedIndexChanged += new System.EventHandler(this.cboChrSelection_SelectedIndexChanged);
			// 
			// grpTileInfo
			// 
			this.grpTileInfo.Controls.Add(this.tlpTileMappings);
			this.grpTileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpTileInfo.Location = new System.Drawing.Point(3, 30);
			this.grpTileInfo.Name = "grpTileInfo";
			this.grpTileInfo.Size = new System.Drawing.Size(811, 455);
			this.grpTileInfo.TabIndex = 4;
			this.grpTileInfo.TabStop = false;
			this.grpTileInfo.Text = "Tile Mappings";
			// 
			// tlpTileMappings
			// 
			this.tlpTileMappings.ColumnCount = 3;
			this.tlpTileMappings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTileMappings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTileMappings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTileMappings.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tlpTileMappings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTileMappings.Location = new System.Drawing.Point(3, 16);
			this.tlpTileMappings.Margin = new System.Windows.Forms.Padding(0);
			this.tlpTileMappings.Name = "tlpTileMappings";
			this.tlpTileMappings.RowCount = 10;
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTileMappings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpTileMappings.Size = new System.Drawing.Size(805, 436);
			this.tlpTileMappings.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tlpTileMappings.SetColumnSpan(this.tableLayoutPanel1, 3);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblHint, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.picHint, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(799, 26);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// lblHint
			// 
			this.lblHint.AutoSize = true;
			this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHint.Location = new System.Drawing.Point(25, 0);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(771, 26);
			this.lblHint.TabIndex = 1;
			this.lblHint.Text = "Select a CHR page above and then enter the character matching each tile in the te" +
    "xtbox below the tile itself.\r\nCharacter mappings are shared between all roms.";
			// 
			// picHint
			// 
			this.picHint.BackgroundImage = global::Mesen.GUI.Properties.Resources.Help;
			this.picHint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picHint.Location = new System.Drawing.Point(3, 5);
			this.picHint.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picHint.Name = "picHint";
			this.picHint.Size = new System.Drawing.Size(16, 16);
			this.picHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picHint.TabIndex = 0;
			this.picHint.TabStop = false;
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 32700;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 10;
			// 
			// ctrlCharacterMappings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "ctrlCharacterMappings";
			this.Size = new System.Drawing.Size(816, 492);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.grpTileInfo.ResumeLayout(false);
			this.tlpTileMappings.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHint)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.GroupBox grpTileInfo;
		private System.Windows.Forms.TableLayoutPanel tlpTileMappings;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblChrSelection;
		private System.Windows.Forms.ComboBox cboChrSelection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.PictureBox picHint;
	}
}
