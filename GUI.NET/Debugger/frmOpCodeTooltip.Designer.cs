namespace Mesen.GUI.Debugger
{
	partial class frmOpCodeTooltip
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.lblAffectedStatusFlags = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblOpCodeDescription = new System.Windows.Forms.Label();
			this.lblByteCode = new System.Windows.Forms.Label();
			this.lblAddressingMode = new System.Windows.Forms.Label();
			this.lblCycleCount = new System.Windows.Forms.Label();
			this.ctrlFlagNegative = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagOverflow = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagDecimal = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagInterrupt = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagZero = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagCarry = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.label1 = new System.Windows.Forms.Label();
			this.tlpOpCodeInfo = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.tlpOpCodeInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.BackColor = System.Drawing.SystemColors.Info;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.tlpMain);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(305, 357);
			this.panel1.TabIndex = 0;
			// 
			// tlpMain
			// 
			this.tlpMain.AutoSize = true;
			this.tlpMain.ColumnCount = 8;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpMain.Controls.Add(this.lblAffectedStatusFlags, 0, 3);
			this.tlpMain.Controls.Add(this.ctrlFlagNegative, 6, 4);
			this.tlpMain.Controls.Add(this.ctrlFlagOverflow, 5, 4);
			this.tlpMain.Controls.Add(this.ctrlFlagDecimal, 4, 4);
			this.tlpMain.Controls.Add(this.ctrlFlagInterrupt, 3, 4);
			this.tlpMain.Controls.Add(this.ctrlFlagZero, 2, 4);
			this.tlpMain.Controls.Add(this.ctrlFlagCarry, 1, 4);
			this.tlpMain.Controls.Add(this.lblName, 0, 0);
			this.tlpMain.Controls.Add(this.lblOpCodeDescription, 0, 1);
			this.tlpMain.Controls.Add(this.tlpOpCodeInfo, 0, 2);
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.MaximumSize = new System.Drawing.Size(295, 0);
			this.tlpMain.MinimumSize = new System.Drawing.Size(295, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 6;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.Size = new System.Drawing.Size(295, 159);
			this.tlpMain.TabIndex = 0;
			// 
			// lblAffectedStatusFlags
			// 
			this.lblAffectedStatusFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblAffectedStatusFlags.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.lblAffectedStatusFlags, 8);
			this.lblAffectedStatusFlags.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblAffectedStatusFlags.Location = new System.Drawing.Point(0, 104);
			this.lblAffectedStatusFlags.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblAffectedStatusFlags.Name = "lblAffectedStatusFlags";
			this.lblAffectedStatusFlags.Size = new System.Drawing.Size(108, 13);
			this.lblAffectedStatusFlags.TabIndex = 24;
			this.lblAffectedStatusFlags.Text = "Affected Status Flags";
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.lblName, 8);
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(3, 3);
			this.lblName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.lblName.MaximumSize = new System.Drawing.Size(295, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(43, 20);
			this.lblName.TabIndex = 7;
			this.lblName.Text = "TSX";
			// 
			// lblOpCodeDescription
			// 
			this.lblOpCodeDescription.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.lblOpCodeDescription, 8);
			this.lblOpCodeDescription.Location = new System.Drawing.Point(3, 28);
			this.lblOpCodeDescription.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblOpCodeDescription.MaximumSize = new System.Drawing.Size(280, 0);
			this.lblOpCodeDescription.Name = "lblOpCodeDescription";
			this.lblOpCodeDescription.Size = new System.Drawing.Size(32, 13);
			this.lblOpCodeDescription.TabIndex = 0;
			this.lblOpCodeDescription.Text = "Desc";
			// 
			// lblByteCode
			// 
			this.lblByteCode.AutoSize = true;
			this.lblByteCode.Location = new System.Drawing.Point(101, 0);
			this.lblByteCode.Name = "lblByteCode";
			this.lblByteCode.Size = new System.Drawing.Size(56, 13);
			this.lblByteCode.TabIndex = 25;
			this.lblByteCode.Text = "Byte Code";
			// 
			// lblAddressingMode
			// 
			this.lblAddressingMode.AutoSize = true;
			this.lblAddressingMode.Location = new System.Drawing.Point(101, 13);
			this.lblAddressingMode.Name = "lblAddressingMode";
			this.lblAddressingMode.Size = new System.Drawing.Size(89, 13);
			this.lblAddressingMode.TabIndex = 26;
			this.lblAddressingMode.Text = "Addressing Mode";
			// 
			// lblCycleCount
			// 
			this.lblCycleCount.AutoSize = true;
			this.lblCycleCount.Location = new System.Drawing.Point(101, 26);
			this.lblCycleCount.Name = "lblCycleCount";
			this.lblCycleCount.Size = new System.Drawing.Size(64, 13);
			this.lblCycleCount.TabIndex = 27;
			this.lblCycleCount.Text = "Cycle Count";
			// 
			// ctrlFlagNegative
			// 
			this.ctrlFlagNegative.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagNegative.Location = new System.Drawing.Point(224, 120);
			this.ctrlFlagNegative.Name = "ctrlFlagNegative";
			this.ctrlFlagNegative.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagNegative.TabIndex = 6;
			// 
			// ctrlFlagOverflow
			// 
			this.ctrlFlagOverflow.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagOverflow.Location = new System.Drawing.Point(189, 120);
			this.ctrlFlagOverflow.Name = "ctrlFlagOverflow";
			this.ctrlFlagOverflow.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagOverflow.TabIndex = 5;
			// 
			// ctrlFlagDecimal
			// 
			this.ctrlFlagDecimal.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagDecimal.Location = new System.Drawing.Point(154, 120);
			this.ctrlFlagDecimal.Name = "ctrlFlagDecimal";
			this.ctrlFlagDecimal.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagDecimal.TabIndex = 4;
			// 
			// ctrlFlagInterrupt
			// 
			this.ctrlFlagInterrupt.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagInterrupt.Location = new System.Drawing.Point(119, 120);
			this.ctrlFlagInterrupt.Name = "ctrlFlagInterrupt";
			this.ctrlFlagInterrupt.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagInterrupt.TabIndex = 3;
			// 
			// ctrlFlagZero
			// 
			this.ctrlFlagZero.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagZero.Location = new System.Drawing.Point(84, 120);
			this.ctrlFlagZero.Name = "ctrlFlagZero";
			this.ctrlFlagZero.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagZero.TabIndex = 2;
			// 
			// ctrlFlagCarry
			// 
			this.ctrlFlagCarry.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagCarry.Location = new System.Drawing.Point(49, 120);
			this.ctrlFlagCarry.Name = "ctrlFlagCarry";
			this.ctrlFlagCarry.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagCarry.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.MaximumSize = new System.Drawing.Size(280, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 28;
			this.label1.Text = "Byte Code:";
			// 
			// tlpOpCodeInfo
			// 
			this.tlpOpCodeInfo.ColumnCount = 2;
			this.tlpMain.SetColumnSpan(this.tlpOpCodeInfo, 8);
			this.tlpOpCodeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpOpCodeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpOpCodeInfo.Controls.Add(this.label2, 0, 1);
			this.tlpOpCodeInfo.Controls.Add(this.lblByteCode, 1, 0);
			this.tlpOpCodeInfo.Controls.Add(this.label1, 0, 0);
			this.tlpOpCodeInfo.Controls.Add(this.lblAddressingMode, 1, 1);
			this.tlpOpCodeInfo.Controls.Add(this.lblCycleCount, 1, 2);
			this.tlpOpCodeInfo.Controls.Add(this.label3, 0, 2);
			this.tlpOpCodeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpOpCodeInfo.Location = new System.Drawing.Point(0, 51);
			this.tlpOpCodeInfo.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.tlpOpCodeInfo.Name = "tlpOpCodeInfo";
			this.tlpOpCodeInfo.RowCount = 4;
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpOpCodeInfo.Size = new System.Drawing.Size(295, 41);
			this.tlpOpCodeInfo.TabIndex = 29;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label2.Location = new System.Drawing.Point(3, 13);
			this.label2.MaximumSize = new System.Drawing.Size(280, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 29;
			this.label2.Text = "Addressing Mode:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label3.Location = new System.Drawing.Point(3, 26);
			this.label3.MaximumSize = new System.Drawing.Size(280, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 13);
			this.label3.TabIndex = 30;
			this.label3.Text = "Cycle Count:";
			// 
			// frmOpCodeTooltip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(305, 357);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOpCodeTooltip";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmCodeTooltip";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.tlpOpCodeInfo.ResumeLayout(false);
			this.tlpOpCodeInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.Label lblOpCodeDescription;
		private Controls.ctrlFlagStatus ctrlFlagNegative;
		private Controls.ctrlFlagStatus ctrlFlagOverflow;
		private Controls.ctrlFlagStatus ctrlFlagDecimal;
		private Controls.ctrlFlagStatus ctrlFlagInterrupt;
		private Controls.ctrlFlagStatus ctrlFlagZero;
		private Controls.ctrlFlagStatus ctrlFlagCarry;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblAffectedStatusFlags;
		private System.Windows.Forms.Label lblByteCode;
		private System.Windows.Forms.Label lblAddressingMode;
		private System.Windows.Forms.Label lblCycleCount;
		private System.Windows.Forms.TableLayoutPanel tlpOpCodeInfo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
	}
}