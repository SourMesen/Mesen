using Mesen.GUI.Controls;

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
			this.panel = new System.Windows.Forms.Panel();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlFlagCarry = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagNegative = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagZero = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagOverflow = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagInterrupt = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.ctrlFlagDecimal = new Mesen.GUI.Debugger.Controls.ctrlFlagStatus();
			this.lblAffectedStatusFlags = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblOpCodeDescription = new Mesen.GUI.Controls.ctrlAutoGrowLabel();
			this.tlpOpCodeInfo = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.lblByteCode = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblAddressingMode = new System.Windows.Forms.Label();
			this.lblCycleCount = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tlpOpCodeInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.AutoSize = true;
			this.panel.BackColor = System.Drawing.SystemColors.Info;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Controls.Add(this.tlpMain);
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 0);
			this.panel.Margin = new System.Windows.Forms.Padding(0);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(284, 357);
			this.panel.TabIndex = 0;
			// 
			// tlpMain
			// 
			this.tlpMain.AutoSize = true;
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.tableLayoutPanel1, 0, 5);
			this.tlpMain.Controls.Add(this.lblAffectedStatusFlags, 0, 3);
			this.tlpMain.Controls.Add(this.lblName, 0, 0);
			this.tlpMain.Controls.Add(this.lblOpCodeDescription, 0, 1);
			this.tlpMain.Controls.Add(this.tlpOpCodeInfo, 0, 2);
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 7;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Size = new System.Drawing.Size(283, 144);
			this.tlpMain.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagCarry, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagNegative, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagZero, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagOverflow, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagInterrupt, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlFlagDecimal, 4, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 117);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(283, 26);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// ctrlFlagCarry
			// 
			this.ctrlFlagCarry.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagCarry.Location = new System.Drawing.Point(56, 3);
			this.ctrlFlagCarry.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagCarry.Name = "ctrlFlagCarry";
			this.ctrlFlagCarry.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagCarry.TabIndex = 1;
			// 
			// ctrlFlagNegative
			// 
			this.ctrlFlagNegative.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagNegative.Location = new System.Drawing.Point(206, 3);
			this.ctrlFlagNegative.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagNegative.Name = "ctrlFlagNegative";
			this.ctrlFlagNegative.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagNegative.TabIndex = 6;
			// 
			// ctrlFlagZero
			// 
			this.ctrlFlagZero.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagZero.Location = new System.Drawing.Point(86, 3);
			this.ctrlFlagZero.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagZero.Name = "ctrlFlagZero";
			this.ctrlFlagZero.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagZero.TabIndex = 2;
			// 
			// ctrlFlagOverflow
			// 
			this.ctrlFlagOverflow.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagOverflow.Location = new System.Drawing.Point(176, 3);
			this.ctrlFlagOverflow.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagOverflow.Name = "ctrlFlagOverflow";
			this.ctrlFlagOverflow.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagOverflow.TabIndex = 5;
			// 
			// ctrlFlagInterrupt
			// 
			this.ctrlFlagInterrupt.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagInterrupt.Location = new System.Drawing.Point(116, 3);
			this.ctrlFlagInterrupt.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagInterrupt.Name = "ctrlFlagInterrupt";
			this.ctrlFlagInterrupt.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagInterrupt.TabIndex = 3;
			// 
			// ctrlFlagDecimal
			// 
			this.ctrlFlagDecimal.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlFlagDecimal.Location = new System.Drawing.Point(146, 3);
			this.ctrlFlagDecimal.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ctrlFlagDecimal.Name = "ctrlFlagDecimal";
			this.ctrlFlagDecimal.Size = new System.Drawing.Size(20, 20);
			this.ctrlFlagDecimal.TabIndex = 4;
			// 
			// lblAffectedStatusFlags
			// 
			this.lblAffectedStatusFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblAffectedStatusFlags.AutoSize = true;
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
			this.lblOpCodeDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblOpCodeDescription.Location = new System.Drawing.Point(3, 28);
			this.lblOpCodeDescription.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblOpCodeDescription.Name = "lblOpCodeDescription";
			this.lblOpCodeDescription.Size = new System.Drawing.Size(38, 13);
			this.lblOpCodeDescription.TabIndex = 0;
			this.lblOpCodeDescription.Text = "Desc";
			// 
			// tlpOpCodeInfo
			// 
			this.tlpOpCodeInfo.ColumnCount = 2;
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
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpOpCodeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpOpCodeInfo.Size = new System.Drawing.Size(283, 41);
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
			// lblByteCode
			// 
			this.lblByteCode.AutoSize = true;
			this.lblByteCode.Location = new System.Drawing.Point(101, 0);
			this.lblByteCode.Name = "lblByteCode";
			this.lblByteCode.Size = new System.Drawing.Size(56, 13);
			this.lblByteCode.TabIndex = 25;
			this.lblByteCode.Text = "Byte Code";
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
			this.ClientSize = new System.Drawing.Size(284, 357);
			this.ControlBox = false;
			this.Controls.Add(this.panel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOpCodeTooltip";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmCodeTooltip";
			this.panel.ResumeLayout(false);
			this.panel.PerformLayout();
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tlpOpCodeInfo.ResumeLayout(false);
			this.tlpOpCodeInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private ctrlAutoGrowLabel lblOpCodeDescription;
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
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}