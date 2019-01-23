﻿namespace Mesen.GUI.Debugger
{
	partial class frmImportSettings
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDbgImportComments = new System.Windows.Forms.CheckBox();
			this.chkDbgImportPrgRomLabels = new System.Windows.Forms.CheckBox();
			this.chkDbgImportRamLabels = new System.Windows.Forms.CheckBox();
			this.chkDbgImportWorkRamLabels = new System.Windows.Forms.CheckBox();
			this.chkDbgImportSaveRamLabels = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.chkMlbImportPrgRomLabels = new System.Windows.Forms.CheckBox();
			this.chkMlbImportWorkRamLabels = new System.Windows.Forms.CheckBox();
			this.chkMlbImportInternalRamLabels = new System.Windows.Forms.CheckBox();
			this.chkMlbImportComments = new System.Windows.Forms.CheckBox();
			this.chkMlbImportSaveRamLabels = new System.Windows.Forms.CheckBox();
			this.chkDbgImportRegisterLabels = new System.Windows.Forms.CheckBox();
			this.grpGeneral = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.chkResetLabelsOnImport = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.grpGeneral.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 222);
			this.baseConfigPanel.Size = new System.Drawing.Size(419, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.grpGeneral, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(419, 222);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 49);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(203, 170);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "DBG files (CA65/CC65 integration)";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.chkDbgImportComments, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkDbgImportRamLabels, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkDbgImportWorkRamLabels, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.chkDbgImportPrgRomLabels, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.chkDbgImportSaveRamLabels, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 6;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(197, 151);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkDbgImportComments
			// 
			this.chkDbgImportComments.AutoSize = true;
			this.chkDbgImportComments.Location = new System.Drawing.Point(3, 95);
			this.chkDbgImportComments.Name = "chkDbgImportComments";
			this.chkDbgImportComments.Size = new System.Drawing.Size(106, 17);
			this.chkDbgImportComments.TabIndex = 2;
			this.chkDbgImportComments.Text = "Import comments";
			this.chkDbgImportComments.UseVisualStyleBackColor = true;
			// 
			// chkDbgImportPrgRomLabels
			// 
			this.chkDbgImportPrgRomLabels.AutoSize = true;
			this.chkDbgImportPrgRomLabels.Location = new System.Drawing.Point(3, 72);
			this.chkDbgImportPrgRomLabels.Name = "chkDbgImportPrgRomLabels";
			this.chkDbgImportPrgRomLabels.Size = new System.Drawing.Size(139, 17);
			this.chkDbgImportPrgRomLabels.TabIndex = 1;
			this.chkDbgImportPrgRomLabels.Text = "Import PRG ROM labels";
			this.chkDbgImportPrgRomLabels.UseVisualStyleBackColor = true;
			// 
			// chkDbgImportRamLabels
			// 
			this.chkDbgImportRamLabels.AutoSize = true;
			this.chkDbgImportRamLabels.Location = new System.Drawing.Point(3, 3);
			this.chkDbgImportRamLabels.Name = "chkDbgImportRamLabels";
			this.chkDbgImportRamLabels.Size = new System.Drawing.Size(112, 17);
			this.chkDbgImportRamLabels.TabIndex = 0;
			this.chkDbgImportRamLabels.Text = "Import RAM labels";
			this.chkDbgImportRamLabels.UseVisualStyleBackColor = true;
			// 
			// chkDbgImportWorkRamLabels
			// 
			this.chkDbgImportWorkRamLabels.AutoSize = true;
			this.chkDbgImportWorkRamLabels.Location = new System.Drawing.Point(3, 26);
			this.chkDbgImportWorkRamLabels.Name = "chkDbgImportWorkRamLabels";
			this.chkDbgImportWorkRamLabels.Size = new System.Drawing.Size(141, 17);
			this.chkDbgImportWorkRamLabels.TabIndex = 3;
			this.chkDbgImportWorkRamLabels.Text = "Import Work RAM labels";
			this.chkDbgImportWorkRamLabels.UseVisualStyleBackColor = true;
			// 
			// chkDbgImportSaveRamLabels
			// 
			this.chkDbgImportSaveRamLabels.AutoSize = true;
			this.chkDbgImportSaveRamLabels.Location = new System.Drawing.Point(3, 49);
			this.chkDbgImportSaveRamLabels.Name = "chkDbgImportSaveRamLabels";
			this.chkDbgImportSaveRamLabels.Size = new System.Drawing.Size(140, 17);
			this.chkDbgImportSaveRamLabels.TabIndex = 4;
			this.chkDbgImportSaveRamLabels.Text = "Import Save RAM labels";
			this.chkDbgImportSaveRamLabels.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(212, 49);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(204, 170);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "MLB files (ASM6f integration)";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.chkMlbImportPrgRomLabels, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.chkMlbImportWorkRamLabels, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.chkMlbImportInternalRamLabels, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.chkMlbImportComments, 0, 5);
			this.tableLayoutPanel3.Controls.Add(this.chkMlbImportSaveRamLabels, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.chkDbgImportRegisterLabels, 0, 3);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 7;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(198, 151);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// chkMlbImportPrgRomLabels
			// 
			this.chkMlbImportPrgRomLabels.AutoSize = true;
			this.chkMlbImportPrgRomLabels.Location = new System.Drawing.Point(3, 95);
			this.chkMlbImportPrgRomLabels.Name = "chkMlbImportPrgRomLabels";
			this.chkMlbImportPrgRomLabels.Size = new System.Drawing.Size(139, 17);
			this.chkMlbImportPrgRomLabels.TabIndex = 4;
			this.chkMlbImportPrgRomLabels.Text = "Import PRG ROM labels";
			this.chkMlbImportPrgRomLabels.UseVisualStyleBackColor = true;
			// 
			// chkMlbImportWorkRamLabels
			// 
			this.chkMlbImportWorkRamLabels.AutoSize = true;
			this.chkMlbImportWorkRamLabels.Location = new System.Drawing.Point(3, 26);
			this.chkMlbImportWorkRamLabels.Name = "chkMlbImportWorkRamLabels";
			this.chkMlbImportWorkRamLabels.Size = new System.Drawing.Size(141, 17);
			this.chkMlbImportWorkRamLabels.TabIndex = 3;
			this.chkMlbImportWorkRamLabels.Text = "Import Work RAM labels";
			this.chkMlbImportWorkRamLabels.UseVisualStyleBackColor = true;
			// 
			// chkMlbImportInternalRamLabels
			// 
			this.chkMlbImportInternalRamLabels.AutoSize = true;
			this.chkMlbImportInternalRamLabels.Location = new System.Drawing.Point(3, 3);
			this.chkMlbImportInternalRamLabels.Name = "chkMlbImportInternalRamLabels";
			this.chkMlbImportInternalRamLabels.Size = new System.Drawing.Size(112, 17);
			this.chkMlbImportInternalRamLabels.TabIndex = 0;
			this.chkMlbImportInternalRamLabels.Text = "Import RAM labels";
			this.chkMlbImportInternalRamLabels.UseVisualStyleBackColor = true;
			// 
			// chkMlbImportComments
			// 
			this.chkMlbImportComments.AutoSize = true;
			this.chkMlbImportComments.Location = new System.Drawing.Point(3, 118);
			this.chkMlbImportComments.Name = "chkMlbImportComments";
			this.chkMlbImportComments.Size = new System.Drawing.Size(106, 17);
			this.chkMlbImportComments.TabIndex = 2;
			this.chkMlbImportComments.Text = "Import comments";
			this.chkMlbImportComments.UseVisualStyleBackColor = true;
			// 
			// chkMlbImportSaveRamLabels
			// 
			this.chkMlbImportSaveRamLabels.AutoSize = true;
			this.chkMlbImportSaveRamLabels.Location = new System.Drawing.Point(3, 49);
			this.chkMlbImportSaveRamLabels.Name = "chkMlbImportSaveRamLabels";
			this.chkMlbImportSaveRamLabels.Size = new System.Drawing.Size(140, 17);
			this.chkMlbImportSaveRamLabels.TabIndex = 1;
			this.chkMlbImportSaveRamLabels.Text = "Import Save RAM labels";
			this.chkMlbImportSaveRamLabels.UseVisualStyleBackColor = true;
			// 
			// chkDbgImportRegisterLabels
			// 
			this.chkDbgImportRegisterLabels.AutoSize = true;
			this.chkDbgImportRegisterLabels.Location = new System.Drawing.Point(3, 72);
			this.chkDbgImportRegisterLabels.Name = "chkDbgImportRegisterLabels";
			this.chkDbgImportRegisterLabels.Size = new System.Drawing.Size(127, 17);
			this.chkDbgImportRegisterLabels.TabIndex = 5;
			this.chkDbgImportRegisterLabels.Text = "Import Register labels";
			this.chkDbgImportRegisterLabels.UseVisualStyleBackColor = true;
			// 
			// grpGeneral
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.grpGeneral, 2);
			this.grpGeneral.Controls.Add(this.tableLayoutPanel4);
			this.grpGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpGeneral.Location = new System.Drawing.Point(3, 3);
			this.grpGeneral.Name = "grpGeneral";
			this.grpGeneral.Size = new System.Drawing.Size(413, 40);
			this.grpGeneral.TabIndex = 2;
			this.grpGeneral.TabStop = false;
			this.grpGeneral.Text = "General Settings";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.chkResetLabelsOnImport, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(407, 21);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// chkResetLabelsOnImport
			// 
			this.chkResetLabelsOnImport.AutoSize = true;
			this.chkResetLabelsOnImport.Location = new System.Drawing.Point(3, 3);
			this.chkResetLabelsOnImport.Name = "chkResetLabelsOnImport";
			this.chkResetLabelsOnImport.Size = new System.Drawing.Size(387, 17);
			this.chkResetLabelsOnImport.TabIndex = 2;
			this.chkResetLabelsOnImport.Text = "Reset workspace labels to their default state before importing DBG/MLB files";
			this.chkResetLabelsOnImport.UseVisualStyleBackColor = true;
			// 
			// frmImportSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(419, 251);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmImportSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import Settings";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.grpGeneral.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkDbgImportRamLabels;
		private System.Windows.Forms.CheckBox chkDbgImportComments;
		private System.Windows.Forms.CheckBox chkDbgImportPrgRomLabels;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox chkMlbImportPrgRomLabels;
		private System.Windows.Forms.CheckBox chkMlbImportWorkRamLabels;
		private System.Windows.Forms.CheckBox chkMlbImportInternalRamLabels;
		private System.Windows.Forms.CheckBox chkMlbImportComments;
		private System.Windows.Forms.CheckBox chkMlbImportSaveRamLabels;
		private System.Windows.Forms.CheckBox chkDbgImportRegisterLabels;
		private System.Windows.Forms.GroupBox grpGeneral;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.CheckBox chkResetLabelsOnImport;
		private System.Windows.Forms.CheckBox chkDbgImportWorkRamLabels;
		private System.Windows.Forms.CheckBox chkDbgImportSaveRamLabels;
	}
}