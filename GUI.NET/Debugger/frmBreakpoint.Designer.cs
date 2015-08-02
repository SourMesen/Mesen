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
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.radTypeExecute = new System.Windows.Forms.RadioButton();
			this.radTypeRead = new System.Windows.Forms.RadioButton();
			this.radTypeWrite = new System.Windows.Forms.RadioButton();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 142);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblBreakOn, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkEnabled, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 142);
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
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 76);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(48, 13);
			this.lblAddress.TabIndex = 3;
			this.lblAddress.Text = "Address:";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.radTypeExecute);
			this.flowLayoutPanel2.Controls.Add(this.radTypeRead);
			this.flowLayoutPanel2.Controls.Add(this.radTypeWrite);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(59, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(268, 70);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// radTypeExecute
			// 
			this.radTypeExecute.AutoSize = true;
			this.radTypeExecute.Location = new System.Drawing.Point(3, 3);
			this.radTypeExecute.Name = "radTypeExecute";
			this.radTypeExecute.Size = new System.Drawing.Size(72, 17);
			this.radTypeExecute.TabIndex = 0;
			this.radTypeExecute.TabStop = true;
			this.radTypeExecute.Text = "Execution";
			this.radTypeExecute.UseVisualStyleBackColor = true;
			// 
			// radTypeRead
			// 
			this.radTypeRead.AutoSize = true;
			this.radTypeRead.Location = new System.Drawing.Point(3, 26);
			this.radTypeRead.Name = "radTypeRead";
			this.radTypeRead.Size = new System.Drawing.Size(51, 17);
			this.radTypeRead.TabIndex = 1;
			this.radTypeRead.TabStop = true;
			this.radTypeRead.Text = "Read";
			this.radTypeRead.UseVisualStyleBackColor = true;
			// 
			// radTypeWrite
			// 
			this.radTypeWrite.AutoSize = true;
			this.radTypeWrite.Location = new System.Drawing.Point(3, 49);
			this.radTypeWrite.Name = "radTypeWrite";
			this.radTypeWrite.Size = new System.Drawing.Size(50, 17);
			this.radTypeWrite.TabIndex = 2;
			this.radTypeWrite.TabStop = true;
			this.radTypeWrite.Text = "Write";
			this.radTypeWrite.UseVisualStyleBackColor = true;
			// 
			// txtAddress
			// 
			this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAddress.Location = new System.Drawing.Point(62, 73);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(262, 20);
			this.txtAddress.TabIndex = 5;
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkEnabled, 2);
			this.chkEnabled.Location = new System.Drawing.Point(3, 99);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 2;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// frmBreakpoint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(327, 171);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmBreakpoint";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Breakpoint";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblBreakOn;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.RadioButton radTypeExecute;
		private System.Windows.Forms.RadioButton radTypeRead;
		private System.Windows.Forms.RadioButton radTypeWrite;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.CheckBox chkEnabled;
	}
}