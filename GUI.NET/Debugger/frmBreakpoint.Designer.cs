namespace Mesen.GUI.Debugger
{
	partial class frmBreakpoint
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
			this.lblBreakOn = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblCondition = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.txtCondition = new System.Windows.Forms.TextBox();
			this.picExpressionWarning = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.chkAbsolute = new System.Windows.Forms.CheckBox();
			this.radSpecificAddress = new System.Windows.Forms.RadioButton();
			this.radAnyAddress = new System.Windows.Forms.RadioButton();
			this.lblAddressSign = new System.Windows.Forms.Label();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkReadVram = new System.Windows.Forms.CheckBox();
			this.chkWriteVram = new System.Windows.Forms.CheckBox();
			this.radCpu = new System.Windows.Forms.RadioButton();
			this.radPpu = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkRead = new System.Windows.Forms.CheckBox();
			this.chkWrite = new System.Windows.Forms.CheckBox();
			this.chkExec = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picExpressionWarning)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 151);
			this.baseConfigPanel.Size = new System.Drawing.Size(378, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblBreakOn, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkEnabled, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblCondition, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(378, 180);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// lblBreakOn
			// 
			this.lblBreakOn.AutoSize = true;
			this.lblBreakOn.Location = new System.Drawing.Point(3, 4);
			this.lblBreakOn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
			this.lblBreakOn.Name = "lblBreakOn";
			this.lblBreakOn.Size = new System.Drawing.Size(53, 13);
			this.lblBreakOn.TabIndex = 0;
			this.lblBreakOn.Text = "Break on:";
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 51);
			this.lblAddress.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(48, 13);
			this.lblAddress.TabIndex = 3;
			this.lblAddress.Text = "Address:";
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkEnabled, 2);
			this.chkEnabled.Location = new System.Drawing.Point(3, 123);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 2;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// lblCondition
			// 
			this.lblCondition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCondition.AutoSize = true;
			this.lblCondition.Location = new System.Drawing.Point(3, 100);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(54, 13);
			this.lblCondition.TabIndex = 7;
			this.lblCondition.Text = "Condition:";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.picHelp, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtCondition, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.picExpressionWarning, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(60, 94);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(318, 26);
			this.tableLayoutPanel2.TabIndex = 10;
			// 
			// picHelp
			// 
			this.picHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picHelp.Location = new System.Drawing.Point(297, 5);
			this.picHelp.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picHelp.Name = "picHelp";
			this.picHelp.Size = new System.Drawing.Size(18, 18);
			this.picHelp.TabIndex = 8;
			this.picHelp.TabStop = false;
			// 
			// txtCondition
			// 
			this.txtCondition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCondition.Location = new System.Drawing.Point(3, 3);
			this.txtCondition.MaxLength = 900;
			this.txtCondition.Name = "txtCondition";
			this.txtCondition.Size = new System.Drawing.Size(264, 20);
			this.txtCondition.TabIndex = 6;
			// 
			// picExpressionWarning
			// 
			this.picExpressionWarning.Image = global::Mesen.GUI.Properties.Resources.Warning;
			this.picExpressionWarning.Location = new System.Drawing.Point(273, 5);
			this.picExpressionWarning.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.picExpressionWarning.Name = "picExpressionWarning";
			this.picExpressionWarning.Size = new System.Drawing.Size(18, 18);
			this.picExpressionWarning.TabIndex = 9;
			this.picExpressionWarning.TabStop = false;
			this.picExpressionWarning.Visible = false;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 4;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.txtAddress, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.chkAbsolute, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.radSpecificAddress, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.radAnyAddress, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.lblAddressSign, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(60, 46);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(318, 48);
			this.tableLayoutPanel3.TabIndex = 11;
			// 
			// txtAddress
			// 
			this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAddress.Location = new System.Drawing.Point(85, 3);
			this.txtAddress.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(157, 20);
			this.txtAddress.TabIndex = 5;
			this.txtAddress.Enter += new System.EventHandler(this.txtAddress_Enter);
			// 
			// chkAbsolute
			// 
			this.chkAbsolute.AutoSize = true;
			this.chkAbsolute.Location = new System.Drawing.Point(248, 5);
			this.chkAbsolute.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.chkAbsolute.Name = "chkAbsolute";
			this.chkAbsolute.Size = new System.Drawing.Size(67, 17);
			this.chkAbsolute.TabIndex = 6;
			this.chkAbsolute.Text = "Absolute";
			this.chkAbsolute.UseVisualStyleBackColor = true;
			this.chkAbsolute.Enter += new System.EventHandler(this.txtAddress_Enter);
			// 
			// radSpecificAddress
			// 
			this.radSpecificAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.radSpecificAddress.AutoSize = true;
			this.radSpecificAddress.Location = new System.Drawing.Point(3, 4);
			this.radSpecificAddress.Name = "radSpecificAddress";
			this.radSpecificAddress.Size = new System.Drawing.Size(66, 17);
			this.radSpecificAddress.TabIndex = 7;
			this.radSpecificAddress.TabStop = true;
			this.radSpecificAddress.Text = "Specific:";
			this.radSpecificAddress.UseVisualStyleBackColor = true;
			// 
			// radAnyAddress
			// 
			this.radAnyAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.radAnyAddress.AutoSize = true;
			this.radAnyAddress.Location = new System.Drawing.Point(3, 29);
			this.radAnyAddress.Name = "radAnyAddress";
			this.radAnyAddress.Size = new System.Drawing.Size(43, 17);
			this.radAnyAddress.TabIndex = 8;
			this.radAnyAddress.TabStop = true;
			this.radAnyAddress.Text = "Any";
			this.radAnyAddress.UseVisualStyleBackColor = true;
			// 
			// lblAddressSign
			// 
			this.lblAddressSign.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddressSign.AutoSize = true;
			this.lblAddressSign.Location = new System.Drawing.Point(72, 6);
			this.lblAddressSign.Margin = new System.Windows.Forms.Padding(0);
			this.lblAddressSign.Name = "lblAddressSign";
			this.lblAddressSign.Size = new System.Drawing.Size(13, 13);
			this.lblAddressSign.TabIndex = 9;
			this.lblAddressSign.Text = "$";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.radCpu, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.radPpu, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel2, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(60, 0);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(318, 46);
			this.tableLayoutPanel4.TabIndex = 12;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.chkReadVram);
			this.flowLayoutPanel3.Controls.Add(this.chkWriteVram);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(56, 23);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(262, 23);
			this.flowLayoutPanel3.TabIndex = 5;
			// 
			// chkReadVram
			// 
			this.chkReadVram.AutoSize = true;
			this.chkReadVram.Location = new System.Drawing.Point(3, 3);
			this.chkReadVram.Name = "chkReadVram";
			this.chkReadVram.Size = new System.Drawing.Size(52, 17);
			this.chkReadVram.TabIndex = 4;
			this.chkReadVram.Text = "Read";
			this.chkReadVram.UseVisualStyleBackColor = true;
			this.chkReadVram.Enter += new System.EventHandler(this.chkWriteVram_Enter);
			// 
			// chkWriteVram
			// 
			this.chkWriteVram.AutoSize = true;
			this.chkWriteVram.Location = new System.Drawing.Point(61, 3);
			this.chkWriteVram.Name = "chkWriteVram";
			this.chkWriteVram.Size = new System.Drawing.Size(51, 17);
			this.chkWriteVram.TabIndex = 5;
			this.chkWriteVram.Text = "Write";
			this.chkWriteVram.UseVisualStyleBackColor = true;
			this.chkWriteVram.Enter += new System.EventHandler(this.chkWriteVram_Enter);
			// 
			// radCpu
			// 
			this.radCpu.AutoSize = true;
			this.radCpu.Location = new System.Drawing.Point(3, 3);
			this.radCpu.Name = "radCpu";
			this.radCpu.Size = new System.Drawing.Size(50, 17);
			this.radCpu.TabIndex = 0;
			this.radCpu.TabStop = true;
			this.radCpu.Text = "CPU:";
			this.radCpu.UseVisualStyleBackColor = true;
			this.radCpu.CheckedChanged += new System.EventHandler(this.radCpu_CheckedChanged);
			// 
			// radPpu
			// 
			this.radPpu.AutoSize = true;
			this.radPpu.Location = new System.Drawing.Point(3, 26);
			this.radPpu.Name = "radPpu";
			this.radPpu.Size = new System.Drawing.Size(50, 17);
			this.radPpu.TabIndex = 1;
			this.radPpu.TabStop = true;
			this.radPpu.Text = "PPU:";
			this.radPpu.UseVisualStyleBackColor = true;
			this.radPpu.CheckedChanged += new System.EventHandler(this.radPpu_CheckedChanged);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.chkRead);
			this.flowLayoutPanel2.Controls.Add(this.chkWrite);
			this.flowLayoutPanel2.Controls.Add(this.chkExec);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(56, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(262, 23);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// chkRead
			// 
			this.chkRead.AutoSize = true;
			this.chkRead.Location = new System.Drawing.Point(3, 3);
			this.chkRead.Name = "chkRead";
			this.chkRead.Size = new System.Drawing.Size(52, 17);
			this.chkRead.TabIndex = 4;
			this.chkRead.Text = "Read";
			this.chkRead.UseVisualStyleBackColor = true;
			this.chkRead.Enter += new System.EventHandler(this.chkRead_Enter);
			// 
			// chkWrite
			// 
			this.chkWrite.AutoSize = true;
			this.chkWrite.Location = new System.Drawing.Point(61, 3);
			this.chkWrite.Name = "chkWrite";
			this.chkWrite.Size = new System.Drawing.Size(51, 17);
			this.chkWrite.TabIndex = 5;
			this.chkWrite.Text = "Write";
			this.chkWrite.UseVisualStyleBackColor = true;
			this.chkWrite.Enter += new System.EventHandler(this.chkRead_Enter);
			// 
			// chkExec
			// 
			this.chkExec.AutoSize = true;
			this.chkExec.Location = new System.Drawing.Point(118, 3);
			this.chkExec.Name = "chkExec";
			this.chkExec.Size = new System.Drawing.Size(73, 17);
			this.chkExec.TabIndex = 3;
			this.chkExec.Text = "Execution";
			this.chkExec.UseVisualStyleBackColor = true;
			this.chkExec.Enter += new System.EventHandler(this.chkRead_Enter);
			// 
			// frmBreakpoint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 180);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmBreakpoint";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Breakpoint";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picExpressionWarning)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblBreakOn;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtCondition;
		private System.Windows.Forms.Label lblCondition;
		private System.Windows.Forms.PictureBox picHelp;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox chkExec;
		private System.Windows.Forms.CheckBox chkRead;
		private System.Windows.Forms.CheckBox chkWrite;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox chkAbsolute;
		private System.Windows.Forms.RadioButton radSpecificAddress;
		private System.Windows.Forms.RadioButton radAnyAddress;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.CheckBox chkReadVram;
		private System.Windows.Forms.CheckBox chkWriteVram;
		private System.Windows.Forms.RadioButton radCpu;
		private System.Windows.Forms.RadioButton radPpu;
		private System.Windows.Forms.Label lblAddressSign;
		private System.Windows.Forms.PictureBox picExpressionWarning;
	}
}