namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlDmcInfo
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
			this.chkIrqEnabled = new System.Windows.Forms.CheckBox();
			this.chkLoop = new System.Windows.Forms.CheckBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblSampleAddr = new System.Windows.Forms.Label();
			this.txtSampleAddress = new System.Windows.Forms.TextBox();
			this.lblSampleLength = new System.Windows.Forms.Label();
			this.lblBytesRemaining = new System.Windows.Forms.Label();
			this.txtSampleLength = new System.Windows.Forms.TextBox();
			this.txtBytesRemaining = new System.Windows.Forms.TextBox();
			this.lblPeriod = new System.Windows.Forms.Label();
			this.txtPeriod = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtOutputVolume = new System.Windows.Forms.TextBox();
			this.lblHz = new System.Windows.Forms.Label();
			this.txtSampleRate = new System.Windows.Forms.TextBox();
			this.lblSampleRate = new System.Windows.Forms.Label();
			this.lblTimer = new System.Windows.Forms.Label();
			this.txtTimer = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 9;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkIrqEnabled, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkLoop, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkEnabled, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblSampleAddr, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtSampleAddress, 8, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblSampleLength, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblBytesRemaining, 5, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtSampleLength, 8, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtBytesRemaining, 8, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblPeriod, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtPeriod, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtOutputVolume, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblHz, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtSampleRate, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblSampleRate, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblTimer, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtTimer, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(415, 105);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkIrqEnabled
			// 
			this.chkIrqEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkIrqEnabled.AutoCheck = false;
			this.chkIrqEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkIrqEnabled, 2);
			this.chkIrqEnabled.Location = new System.Drawing.Point(3, 56);
			this.chkIrqEnabled.Name = "chkIrqEnabled";
			this.chkIrqEnabled.Size = new System.Drawing.Size(87, 17);
			this.chkIrqEnabled.TabIndex = 16;
			this.chkIrqEnabled.Text = "IRQ Enabled";
			this.chkIrqEnabled.UseVisualStyleBackColor = true;
			// 
			// chkLoop
			// 
			this.chkLoop.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkLoop.AutoCheck = false;
			this.chkLoop.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkLoop, 2);
			this.chkLoop.Location = new System.Drawing.Point(3, 30);
			this.chkLoop.Name = "chkLoop";
			this.chkLoop.Size = new System.Drawing.Size(50, 17);
			this.chkLoop.TabIndex = 15;
			this.chkLoop.Text = "Loop";
			this.chkLoop.UseVisualStyleBackColor = true;
			// 
			// chkEnabled
			// 
			this.chkEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkEnabled.AutoCheck = false;
			this.chkEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkEnabled, 2);
			this.chkEnabled.Location = new System.Drawing.Point(3, 4);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 3;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// lblSampleAddr
			// 
			this.lblSampleAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSampleAddr.AutoSize = true;
			this.lblSampleAddr.Location = new System.Drawing.Point(273, 6);
			this.lblSampleAddr.Name = "lblSampleAddr";
			this.lblSampleAddr.Size = new System.Drawing.Size(86, 13);
			this.lblSampleAddr.TabIndex = 6;
			this.lblSampleAddr.Text = "Sample Address:";
			// 
			// txtSampleAddress
			// 
			this.txtSampleAddress.BackColor = System.Drawing.Color.White;
			this.txtSampleAddress.Location = new System.Drawing.Point(365, 3);
			this.txtSampleAddress.Name = "txtSampleAddress";
			this.txtSampleAddress.ReadOnly = true;
			this.txtSampleAddress.Size = new System.Drawing.Size(39, 20);
			this.txtSampleAddress.TabIndex = 9;
			// 
			// lblSampleLength
			// 
			this.lblSampleLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSampleLength.AutoSize = true;
			this.lblSampleLength.Location = new System.Drawing.Point(273, 32);
			this.lblSampleLength.Name = "lblSampleLength";
			this.lblSampleLength.Size = new System.Drawing.Size(81, 13);
			this.lblSampleLength.TabIndex = 17;
			this.lblSampleLength.Text = "Sample Length:";
			// 
			// lblBytesRemaining
			// 
			this.lblBytesRemaining.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBytesRemaining.AutoSize = true;
			this.lblBytesRemaining.Location = new System.Drawing.Point(273, 58);
			this.lblBytesRemaining.Name = "lblBytesRemaining";
			this.lblBytesRemaining.Size = new System.Drawing.Size(84, 13);
			this.lblBytesRemaining.TabIndex = 18;
			this.lblBytesRemaining.Text = "Bytes remaining:";
			// 
			// txtSampleLength
			// 
			this.txtSampleLength.BackColor = System.Drawing.Color.White;
			this.txtSampleLength.Location = new System.Drawing.Point(365, 29);
			this.txtSampleLength.Name = "txtSampleLength";
			this.txtSampleLength.ReadOnly = true;
			this.txtSampleLength.Size = new System.Drawing.Size(39, 20);
			this.txtSampleLength.TabIndex = 19;
			// 
			// txtBytesRemaining
			// 
			this.txtBytesRemaining.BackColor = System.Drawing.Color.White;
			this.txtBytesRemaining.Location = new System.Drawing.Point(365, 55);
			this.txtBytesRemaining.Name = "txtBytesRemaining";
			this.txtBytesRemaining.ReadOnly = true;
			this.txtBytesRemaining.Size = new System.Drawing.Size(39, 20);
			this.txtBytesRemaining.TabIndex = 20;
			// 
			// lblPeriod
			// 
			this.lblPeriod.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPeriod.AutoSize = true;
			this.lblPeriod.Location = new System.Drawing.Point(96, 6);
			this.lblPeriod.Name = "lblPeriod";
			this.lblPeriod.Size = new System.Drawing.Size(40, 13);
			this.lblPeriod.TabIndex = 0;
			this.lblPeriod.Text = "Period:";
			// 
			// txtPeriod
			// 
			this.txtPeriod.BackColor = System.Drawing.Color.White;
			this.txtPeriod.Location = new System.Drawing.Point(182, 3);
			this.txtPeriod.Name = "txtPeriod";
			this.txtPeriod.ReadOnly = true;
			this.txtPeriod.Size = new System.Drawing.Size(39, 20);
			this.txtPeriod.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(96, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Output Volume:";
			// 
			// txtOutputVolume
			// 
			this.txtOutputVolume.BackColor = System.Drawing.Color.White;
			this.txtOutputVolume.Location = new System.Drawing.Point(182, 81);
			this.txtOutputVolume.Name = "txtOutputVolume";
			this.txtOutputVolume.ReadOnly = true;
			this.txtOutputVolume.Size = new System.Drawing.Size(39, 20);
			this.txtOutputVolume.TabIndex = 8;
			// 
			// lblHz
			// 
			this.lblHz.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHz.AutoSize = true;
			this.lblHz.Location = new System.Drawing.Point(247, 58);
			this.lblHz.Name = "lblHz";
			this.lblHz.Size = new System.Drawing.Size(20, 13);
			this.lblHz.TabIndex = 14;
			this.lblHz.Text = "Hz";
			// 
			// txtSampleRate
			// 
			this.txtSampleRate.BackColor = System.Drawing.Color.White;
			this.txtSampleRate.Location = new System.Drawing.Point(182, 55);
			this.txtSampleRate.Name = "txtSampleRate";
			this.txtSampleRate.ReadOnly = true;
			this.txtSampleRate.Size = new System.Drawing.Size(59, 20);
			this.txtSampleRate.TabIndex = 5;
			// 
			// lblSampleRate
			// 
			this.lblSampleRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSampleRate.AutoSize = true;
			this.lblSampleRate.Location = new System.Drawing.Point(96, 58);
			this.lblSampleRate.Name = "lblSampleRate";
			this.lblSampleRate.Size = new System.Drawing.Size(71, 13);
			this.lblSampleRate.TabIndex = 1;
			this.lblSampleRate.Text = "Sample Rate:";
			// 
			// lblTimer
			// 
			this.lblTimer.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTimer.AutoSize = true;
			this.lblTimer.Location = new System.Drawing.Point(96, 32);
			this.lblTimer.Name = "lblTimer";
			this.lblTimer.Size = new System.Drawing.Size(36, 13);
			this.lblTimer.TabIndex = 21;
			this.lblTimer.Text = "Timer:";
			// 
			// txtTimer
			// 
			this.txtTimer.BackColor = System.Drawing.Color.White;
			this.txtTimer.Location = new System.Drawing.Point(182, 29);
			this.txtTimer.Name = "txtTimer";
			this.txtTimer.ReadOnly = true;
			this.txtTimer.Size = new System.Drawing.Size(39, 20);
			this.txtTimer.TabIndex = 22;
			// 
			// ctrlDmcInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlDmcInfo";
			this.Size = new System.Drawing.Size(415, 105);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblPeriod;
		private System.Windows.Forms.Label lblSampleRate;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtPeriod;
		private System.Windows.Forms.TextBox txtSampleRate;
		private System.Windows.Forms.Label lblSampleAddr;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOutputVolume;
		private System.Windows.Forms.TextBox txtSampleAddress;
		private System.Windows.Forms.Label lblHz;
		private System.Windows.Forms.CheckBox chkLoop;
		private System.Windows.Forms.CheckBox chkIrqEnabled;
		private System.Windows.Forms.Label lblSampleLength;
		private System.Windows.Forms.Label lblBytesRemaining;
		private System.Windows.Forms.TextBox txtSampleLength;
		private System.Windows.Forms.TextBox txtBytesRemaining;
		private System.Windows.Forms.Label lblTimer;
		private System.Windows.Forms.TextBox txtTimer;
	}
}
