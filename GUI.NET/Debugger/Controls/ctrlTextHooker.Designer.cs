namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlTextHooker
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
			this.picNametable = new System.Windows.Forms.PictureBox();
			this.grpText = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkAutoCopyToClipboard = new System.Windows.Forms.CheckBox();
			this.chkIgnoreMirroredNametables = new System.Windows.Forms.CheckBox();
			this.btnClearSelection = new System.Windows.Forms.Button();
			this.txtSelectedText = new System.Windows.Forms.TextBox();
			this.chkUseScrollOffsets = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblDakutenMode = new System.Windows.Forms.Label();
			this.cboDakutenMode = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNametable)).BeginInit();
			this.grpText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.picNametable, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpText, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(923, 491);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// picNametable
			// 
			this.picNametable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picNametable.Location = new System.Drawing.Point(4, 4);
			this.picNametable.Margin = new System.Windows.Forms.Padding(4);
			this.picNametable.Name = "picNametable";
			this.picNametable.Size = new System.Drawing.Size(514, 482);
			this.picNametable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picNametable.TabIndex = 0;
			this.picNametable.TabStop = false;
			// 
			// grpText
			// 
			this.grpText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpText.Controls.Add(this.tableLayoutPanel2);
			this.grpText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpText.Location = new System.Drawing.Point(525, 3);
			this.grpText.Name = "grpText";
			this.grpText.Size = new System.Drawing.Size(395, 484);
			this.grpText.TabIndex = 4;
			this.grpText.TabStop = false;
			this.grpText.Text = "Text Hooker";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.chkAutoCopyToClipboard, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkIgnoreMirroredNametables, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.btnClearSelection, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.txtSelectedText, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkUseScrollOffsets, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 4);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 6;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(389, 465);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkAutoCopyToClipboard
			// 
			this.chkAutoCopyToClipboard.AutoSize = true;
			this.chkAutoCopyToClipboard.Location = new System.Drawing.Point(3, 342);
			this.chkAutoCopyToClipboard.Name = "chkAutoCopyToClipboard";
			this.chkAutoCopyToClipboard.Size = new System.Drawing.Size(132, 17);
			this.chkAutoCopyToClipboard.TabIndex = 7;
			this.chkAutoCopyToClipboard.Text = "Auto-copy to clipboard";
			this.chkAutoCopyToClipboard.UseVisualStyleBackColor = true;
			// 
			// chkIgnoreMirroredNametables
			// 
			this.chkIgnoreMirroredNametables.AutoSize = true;
			this.chkIgnoreMirroredNametables.Location = new System.Drawing.Point(3, 365);
			this.chkIgnoreMirroredNametables.Name = "chkIgnoreMirroredNametables";
			this.chkIgnoreMirroredNametables.Size = new System.Drawing.Size(153, 17);
			this.chkIgnoreMirroredNametables.TabIndex = 6;
			this.chkIgnoreMirroredNametables.Text = "Ignore mirrored nametables";
			this.chkIgnoreMirroredNametables.UseVisualStyleBackColor = true;
			// 
			// btnClearSelection
			// 
			this.btnClearSelection.Location = new System.Drawing.Point(3, 437);
			this.btnClearSelection.Name = "btnClearSelection";
			this.btnClearSelection.Size = new System.Drawing.Size(92, 23);
			this.btnClearSelection.TabIndex = 0;
			this.btnClearSelection.Text = "Clear Selection";
			this.btnClearSelection.UseVisualStyleBackColor = true;
			this.btnClearSelection.Visible = false;
			// 
			// txtSelectedText
			// 
			this.txtSelectedText.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.tableLayoutPanel2.SetColumnSpan(this.txtSelectedText, 2);
			this.txtSelectedText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSelectedText.Location = new System.Drawing.Point(3, 3);
			this.txtSelectedText.Multiline = true;
			this.txtSelectedText.Name = "txtSelectedText";
			this.txtSelectedText.ReadOnly = true;
			this.txtSelectedText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSelectedText.Size = new System.Drawing.Size(383, 333);
			this.txtSelectedText.TabIndex = 1;
			// 
			// chkUseScrollOffsets
			// 
			this.chkUseScrollOffsets.AutoSize = true;
			this.chkUseScrollOffsets.Location = new System.Drawing.Point(3, 388);
			this.chkUseScrollOffsets.Name = "chkUseScrollOffsets";
			this.chkUseScrollOffsets.Size = new System.Drawing.Size(187, 17);
			this.chkUseScrollOffsets.TabIndex = 2;
			this.chkUseScrollOffsets.Text = "Adjust viewport by scrolling offsets";
			this.chkUseScrollOffsets.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 2);
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.lblDakutenMode, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.cboDakutenMode, 1, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 408);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(219, 26);
			this.tableLayoutPanel3.TabIndex = 5;
			// 
			// lblDakutenMode
			// 
			this.lblDakutenMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDakutenMode.AutoSize = true;
			this.lblDakutenMode.Location = new System.Drawing.Point(3, 6);
			this.lblDakutenMode.Name = "lblDakutenMode";
			this.lblDakutenMode.Size = new System.Drawing.Size(81, 13);
			this.lblDakutenMode.TabIndex = 4;
			this.lblDakutenMode.Text = "Dakuten Mode:";
			// 
			// cboDakutenMode
			// 
			this.cboDakutenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDakutenMode.FormattingEnabled = true;
			this.cboDakutenMode.Location = new System.Drawing.Point(90, 3);
			this.cboDakutenMode.Name = "cboDakutenMode";
			this.cboDakutenMode.Size = new System.Drawing.Size(121, 21);
			this.cboDakutenMode.TabIndex = 5;
			// 
			// ctrlTextHooker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlTextHooker";
			this.Size = new System.Drawing.Size(923, 491);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picNametable)).EndInit();
			this.grpText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox picNametable;
		private System.Windows.Forms.GroupBox grpText;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnClearSelection;
		private System.Windows.Forms.TextBox txtSelectedText;
		private System.Windows.Forms.CheckBox chkUseScrollOffsets;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label lblDakutenMode;
		private System.Windows.Forms.ComboBox cboDakutenMode;
		private System.Windows.Forms.CheckBox chkAutoCopyToClipboard;
		private System.Windows.Forms.CheckBox chkIgnoreMirroredNametables;
	}
}
