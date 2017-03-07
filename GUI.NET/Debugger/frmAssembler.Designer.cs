namespace Mesen.GUI.Debugger
{
	partial class frmAssembler
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
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlHexBox = new Be.Windows.Forms.HexBox();
			this.lstErrors = new System.Windows.Forms.ListBox();
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtStartAddress = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.lblByteUsage = new System.Windows.Forms.Label();
			this.picSizeWarning = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblNoChanges = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtCode = new System.Windows.Forms.RichTextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpSettings.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSizeWarning)).BeginInit();
			this.flowLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(279, 3);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Apply";
			this.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(360, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.ctrlHexBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lstErrors, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.grpSettings, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 141F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(999, 534);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// ctrlHexBox
			// 
			this.ctrlHexBox.ByteColorProvider = null;
			this.ctrlHexBox.ColumnInfoVisible = true;
			this.ctrlHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlHexBox.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.ctrlHexBox.InfoBackColor = System.Drawing.Color.DarkGray;
			this.ctrlHexBox.LineInfoVisible = true;
			this.ctrlHexBox.Location = new System.Drawing.Point(552, 3);
			this.ctrlHexBox.Name = "ctrlHexBox";
			this.ctrlHexBox.ReadOnly = true;
			this.ctrlHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
			this.ctrlHexBox.Size = new System.Drawing.Size(444, 387);
			this.ctrlHexBox.TabIndex = 1;
			this.ctrlHexBox.UseFixedBytesPerLine = true;
			this.ctrlHexBox.VScrollBarVisible = true;
			// 
			// lstErrors
			// 
			this.lstErrors.FormattingEnabled = true;
			this.lstErrors.Location = new System.Drawing.Point(3, 396);
			this.lstErrors.Name = "lstErrors";
			this.lstErrors.Size = new System.Drawing.Size(543, 134);
			this.lstErrors.TabIndex = 2;
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.tableLayoutPanel2);
			this.grpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpSettings.Location = new System.Drawing.Point(552, 396);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(444, 135);
			this.grpSettings.TabIndex = 3;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Settings";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(438, 116);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this.txtStartAddress);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(438, 25);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Start address: $";
			// 
			// txtStartAddress
			// 
			this.txtStartAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtStartAddress.Location = new System.Drawing.Point(84, 3);
			this.txtStartAddress.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.txtStartAddress.Name = "txtStartAddress";
			this.txtStartAddress.Size = new System.Drawing.Size(65, 20);
			this.txtStartAddress.TabIndex = 1;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.label2);
			this.flowLayoutPanel2.Controls.Add(this.lblByteUsage);
			this.flowLayoutPanel2.Controls.Add(this.picSizeWarning);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 25);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(438, 25);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Byte usage:";
			// 
			// lblByteUsage
			// 
			this.lblByteUsage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblByteUsage.AutoSize = true;
			this.lblByteUsage.Location = new System.Drawing.Point(72, 6);
			this.lblByteUsage.Name = "lblByteUsage";
			this.lblByteUsage.Size = new System.Drawing.Size(30, 13);
			this.lblByteUsage.TabIndex = 3;
			this.lblByteUsage.Text = "0 / 0";
			// 
			// picSizeWarning
			// 
			this.picSizeWarning.Image = global::Mesen.GUI.Properties.Resources.Warning;
			this.picSizeWarning.Location = new System.Drawing.Point(108, 5);
			this.picSizeWarning.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picSizeWarning.Name = "picSizeWarning";
			this.picSizeWarning.Size = new System.Drawing.Size(18, 18);
			this.picSizeWarning.TabIndex = 10;
			this.picSizeWarning.TabStop = false;
			this.picSizeWarning.Visible = false;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.btnCancel);
			this.flowLayoutPanel3.Controls.Add(this.btnOk);
			this.flowLayoutPanel3.Controls.Add(this.lblNoChanges);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 86);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(438, 30);
			this.flowLayoutPanel3.TabIndex = 5;
			// 
			// lblNoChanges
			// 
			this.lblNoChanges.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNoChanges.AutoSize = true;
			this.lblNoChanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblNoChanges.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblNoChanges.Location = new System.Drawing.Point(99, 8);
			this.lblNoChanges.Name = "lblNoChanges";
			this.lblNoChanges.Size = new System.Drawing.Size(174, 13);
			this.lblNoChanges.TabIndex = 2;
			this.lblNoChanges.Text = "Current code matches original code";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Controls.Add(this.txtCode);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(1);
			this.panel1.Size = new System.Drawing.Size(543, 387);
			this.panel1.TabIndex = 4;
			// 
			// txtCode
			// 
			this.txtCode.AcceptsTab = true;
			this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCode.DetectUrls = false;
			this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCode.Location = new System.Drawing.Point(1, 1);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(541, 385);
			this.txtCode.TabIndex = 4;
			this.txtCode.Text = "";
			this.txtCode.WordWrap = false;
			this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
			this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
			// 
			// frmAssembler
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(999, 534);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmAssembler";
			this.Text = "Assembler";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpSettings.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSizeWarning)).EndInit();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Be.Windows.Forms.HexBox ctrlHexBox;
		private System.Windows.Forms.ListBox lstErrors;
		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStartAddress;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblByteUsage;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.PictureBox picSizeWarning;
		private System.Windows.Forms.RichTextBox txtCode;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblNoChanges;
	}
}