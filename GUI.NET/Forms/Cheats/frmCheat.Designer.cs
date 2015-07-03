namespace Mesen.GUI.Forms.Cheats
{
	partial class frmCheat
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheat));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCheatName = new System.Windows.Forms.TextBox();
			this.grpCode = new System.Windows.Forms.GroupBox();
			this.tlpAdd = new System.Windows.Forms.TableLayoutPanel();
			this.radCustom = new System.Windows.Forms.RadioButton();
			this.txtProActionRocky = new System.Windows.Forms.TextBox();
			this.txtGameGenie = new System.Windows.Forms.TextBox();
			this.radGameGenie = new System.Windows.Forms.RadioButton();
			this.radProActionRocky = new System.Windows.Forms.RadioButton();
			this.tlpCustom = new System.Windows.Forms.TableLayoutPanel();
			this.lblAddress = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblNewValue = new System.Windows.Forms.Label();
			this.lblCompare = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.radRelativeAddress = new System.Windows.Forms.RadioButton();
			this.radAbsoluteAddress = new System.Windows.Forms.RadioButton();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.txtCompare = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtGameName = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpCode.SuspendLayout();
			this.tlpAdd.SuspendLayout();
			this.tlpCustom.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtCheatName, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.grpCode, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkEnabled, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(385, 264);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Game:";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Cheat Name:";
			// 
			// txtCheatName
			// 
			this.txtCheatName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCheatName.Location = new System.Drawing.Point(78, 29);
			this.txtCheatName.MaxLength = 255;
			this.txtCheatName.Name = "txtCheatName";
			this.txtCheatName.Size = new System.Drawing.Size(304, 20);
			this.txtCheatName.TabIndex = 2;
			this.txtCheatName.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			// 
			// grpCode
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.grpCode, 2);
			this.grpCode.Controls.Add(this.tlpAdd);
			this.grpCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCode.Location = new System.Drawing.Point(3, 78);
			this.grpCode.Name = "grpCode";
			this.grpCode.Size = new System.Drawing.Size(379, 183);
			this.grpCode.TabIndex = 3;
			this.grpCode.TabStop = false;
			this.grpCode.Text = "Code";
			// 
			// tlpAdd
			// 
			this.tlpAdd.ColumnCount = 2;
			this.tlpAdd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpAdd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpAdd.Controls.Add(this.radCustom, 0, 2);
			this.tlpAdd.Controls.Add(this.txtProActionRocky, 1, 1);
			this.tlpAdd.Controls.Add(this.txtGameGenie, 1, 0);
			this.tlpAdd.Controls.Add(this.radGameGenie, 0, 0);
			this.tlpAdd.Controls.Add(this.radProActionRocky, 0, 1);
			this.tlpAdd.Controls.Add(this.tlpCustom, 1, 2);
			this.tlpAdd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpAdd.Location = new System.Drawing.Point(3, 16);
			this.tlpAdd.Name = "tlpAdd";
			this.tlpAdd.RowCount = 3;
			this.tlpAdd.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpAdd.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpAdd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpAdd.Size = new System.Drawing.Size(373, 164);
			this.tlpAdd.TabIndex = 0;
			// 
			// radCustom
			// 
			this.radCustom.AutoSize = true;
			this.radCustom.Location = new System.Drawing.Point(3, 55);
			this.radCustom.Name = "radCustom";
			this.radCustom.Size = new System.Drawing.Size(63, 17);
			this.radCustom.TabIndex = 3;
			this.radCustom.Text = "Custom:";
			this.radCustom.UseVisualStyleBackColor = true;
			this.radCustom.CheckedChanged += new System.EventHandler(this.radType_CheckedChanged);
			// 
			// txtProActionRocky
			// 
			this.txtProActionRocky.Location = new System.Drawing.Point(120, 29);
			this.txtProActionRocky.MaxLength = 8;
			this.txtProActionRocky.Name = "txtProActionRocky";
			this.txtProActionRocky.Size = new System.Drawing.Size(71, 20);
			this.txtProActionRocky.TabIndex = 1;
			this.txtProActionRocky.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			this.txtProActionRocky.Enter += new System.EventHandler(this.txtProActionRocky_Enter);
			// 
			// txtGameGenie
			// 
			this.txtGameGenie.Location = new System.Drawing.Point(120, 3);
			this.txtGameGenie.MaxLength = 8;
			this.txtGameGenie.Name = "txtGameGenie";
			this.txtGameGenie.Size = new System.Drawing.Size(71, 20);
			this.txtGameGenie.TabIndex = 1;
			this.txtGameGenie.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			this.txtGameGenie.Enter += new System.EventHandler(this.txtGameGenie_Enter);
			// 
			// radGameGenie
			// 
			this.radGameGenie.AutoSize = true;
			this.radGameGenie.Checked = true;
			this.radGameGenie.Location = new System.Drawing.Point(3, 3);
			this.radGameGenie.Name = "radGameGenie";
			this.radGameGenie.Size = new System.Drawing.Size(87, 17);
			this.radGameGenie.TabIndex = 2;
			this.radGameGenie.TabStop = true;
			this.radGameGenie.Text = "Game Genie:";
			this.radGameGenie.UseVisualStyleBackColor = true;
			this.radGameGenie.CheckedChanged += new System.EventHandler(this.radType_CheckedChanged);
			// 
			// radProActionRocky
			// 
			this.radProActionRocky.AutoSize = true;
			this.radProActionRocky.Location = new System.Drawing.Point(3, 29);
			this.radProActionRocky.Name = "radProActionRocky";
			this.radProActionRocky.Size = new System.Drawing.Size(111, 17);
			this.radProActionRocky.TabIndex = 2;
			this.radProActionRocky.Text = "Pro Action Rocky:";
			this.radProActionRocky.UseVisualStyleBackColor = true;
			this.radProActionRocky.CheckedChanged += new System.EventHandler(this.radType_CheckedChanged);
			// 
			// tlpCustom
			// 
			this.tlpCustom.ColumnCount = 2;
			this.tlpCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCustom.Controls.Add(this.lblAddress, 0, 0);
			this.tlpCustom.Controls.Add(this.txtAddress, 1, 0);
			this.tlpCustom.Controls.Add(this.lblNewValue, 0, 2);
			this.tlpCustom.Controls.Add(this.lblCompare, 0, 3);
			this.tlpCustom.Controls.Add(this.flowLayoutPanel2, 1, 1);
			this.tlpCustom.Controls.Add(this.txtValue, 1, 2);
			this.tlpCustom.Controls.Add(this.txtCompare, 1, 3);
			this.tlpCustom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpCustom.Location = new System.Drawing.Point(120, 55);
			this.tlpCustom.Name = "tlpCustom";
			this.tlpCustom.RowCount = 5;
			this.tlpCustom.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCustom.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCustom.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCustom.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCustom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCustom.Size = new System.Drawing.Size(250, 106);
			this.tlpCustom.TabIndex = 4;
			// 
			// lblAddress
			// 
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 6);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(48, 13);
			this.lblAddress.TabIndex = 0;
			this.lblAddress.Text = "Address:";
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(91, 3);
			this.txtAddress.MaxLength = 8;
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(69, 20);
			this.txtAddress.TabIndex = 1;
			this.txtAddress.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			this.txtAddress.Enter += new System.EventHandler(this.customField_Enter);
			// 
			// lblNewValue
			// 
			this.lblNewValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNewValue.AutoSize = true;
			this.lblNewValue.Location = new System.Drawing.Point(3, 59);
			this.lblNewValue.Name = "lblNewValue";
			this.lblNewValue.Size = new System.Drawing.Size(62, 13);
			this.lblNewValue.TabIndex = 3;
			this.lblNewValue.Text = "New Value:";
			// 
			// lblCompare
			// 
			this.lblCompare.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCompare.AutoSize = true;
			this.lblCompare.Location = new System.Drawing.Point(3, 85);
			this.lblCompare.Name = "lblCompare";
			this.lblCompare.Size = new System.Drawing.Size(82, 13);
			this.lblCompare.TabIndex = 2;
			this.lblCompare.Text = "Compare Value:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.radRelativeAddress);
			this.flowLayoutPanel2.Controls.Add(this.radAbsoluteAddress);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(88, 26);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(159, 27);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// radRelativeAddress
			// 
			this.radRelativeAddress.AutoSize = true;
			this.radRelativeAddress.Checked = true;
			this.radRelativeAddress.Location = new System.Drawing.Point(3, 3);
			this.radRelativeAddress.Name = "radRelativeAddress";
			this.radRelativeAddress.Size = new System.Drawing.Size(62, 17);
			this.radRelativeAddress.TabIndex = 0;
			this.radRelativeAddress.TabStop = true;
			this.radRelativeAddress.Text = "Memory";
			this.radRelativeAddress.UseVisualStyleBackColor = true;
			this.radRelativeAddress.Enter += new System.EventHandler(this.customField_Enter);
			// 
			// radAbsoluteAddress
			// 
			this.radAbsoluteAddress.AutoSize = true;
			this.radAbsoluteAddress.Location = new System.Drawing.Point(71, 3);
			this.radAbsoluteAddress.Name = "radAbsoluteAddress";
			this.radAbsoluteAddress.Size = new System.Drawing.Size(81, 17);
			this.radAbsoluteAddress.TabIndex = 1;
			this.radAbsoluteAddress.Text = "Game Code";
			this.radAbsoluteAddress.UseVisualStyleBackColor = true;
			this.radAbsoluteAddress.Enter += new System.EventHandler(this.customField_Enter);
			// 
			// txtValue
			// 
			this.txtValue.Location = new System.Drawing.Point(91, 56);
			this.txtValue.MaxLength = 2;
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(30, 20);
			this.txtValue.TabIndex = 5;
			this.txtValue.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			this.txtValue.Enter += new System.EventHandler(this.customField_Enter);
			// 
			// txtCompare
			// 
			this.txtCompare.Location = new System.Drawing.Point(91, 82);
			this.txtCompare.MaxLength = 2;
			this.txtCompare.Name = "txtCompare";
			this.txtCompare.Size = new System.Drawing.Size(30, 20);
			this.txtCompare.TabIndex = 6;
			this.txtCompare.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.txtGameName);
			this.flowLayoutPanel3.Controls.Add(this.btnBrowse);
			this.flowLayoutPanel3.Location = new System.Drawing.Point(75, 0);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(310, 26);
			this.flowLayoutPanel3.TabIndex = 5;
			// 
			// txtGame
			// 
			this.txtGameName.Location = new System.Drawing.Point(3, 3);
			this.txtGameName.Name = "txtGame";
			this.txtGameName.ReadOnly = true;
			this.txtGameName.Size = new System.Drawing.Size(223, 20);
			this.txtGameName.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(232, 3);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkEnabled, 2);
			this.chkEnabled.Location = new System.Drawing.Point(3, 55);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(96, 17);
			this.chkEnabled.TabIndex = 6;
			this.chkEnabled.Text = "Cheat Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// frmCheat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(385, 294);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(401, 332);
			this.MinimumSize = new System.Drawing.Size(401, 332);
			this.Name = "frmCheat";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Cheat";
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpCode.ResumeLayout(false);
			this.tlpAdd.ResumeLayout(false);
			this.tlpAdd.PerformLayout();
			this.tlpCustom.ResumeLayout(false);
			this.tlpCustom.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCheatName;
		private System.Windows.Forms.GroupBox grpCode;
		private System.Windows.Forms.TableLayoutPanel tlpAdd;
		private System.Windows.Forms.RadioButton radCustom;
		private System.Windows.Forms.TextBox txtProActionRocky;
		private System.Windows.Forms.TextBox txtGameGenie;
		private System.Windows.Forms.RadioButton radGameGenie;
		private System.Windows.Forms.RadioButton radProActionRocky;
		private System.Windows.Forms.TableLayoutPanel tlpCustom;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblNewValue;
		private System.Windows.Forms.Label lblCompare;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.RadioButton radRelativeAddress;
		private System.Windows.Forms.RadioButton radAbsoluteAddress;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.TextBox txtCompare;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.TextBox txtGameName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.CheckBox chkEnabled;


	}
}