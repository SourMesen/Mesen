namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlNoiseInfo
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
			this.chkModeFlag = new System.Windows.Forms.CheckBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblPeriod = new System.Windows.Forms.Label();
			this.txtPeriod = new System.Windows.Forms.TextBox();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.txtFrequency = new System.Windows.Forms.TextBox();
			this.lblShiftRegister = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtOutputVolume = new System.Windows.Forms.TextBox();
			this.txtShiftRegister = new System.Windows.Forms.TextBox();
			this.grpEnvelope = new System.Windows.Forms.GroupBox();
			this.ctrlEnvelopeInfo = new Mesen.GUI.Debugger.Controls.ApuViewer.ctrlEnvelopeInfo();
			this.grpLengthCounter = new System.Windows.Forms.GroupBox();
			this.ctrlLengthCounterInfo = new Mesen.GUI.Debugger.Controls.ApuViewer.ctrlLengthCounterInfo();
			this.lblHz = new System.Windows.Forms.Label();
			this.lblTimer = new System.Windows.Forms.Label();
			this.txtTimer = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpEnvelope.SuspendLayout();
			this.grpLengthCounter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkModeFlag, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkEnabled, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblPeriod, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPeriod, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblFrequency, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtFrequency, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblShiftRegister, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtOutputVolume, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtShiftRegister, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.grpEnvelope, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpLengthCounter, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblHz, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblTimer, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtTimer, 1, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 177);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkModeFlag
			// 
			this.chkModeFlag.AutoCheck = false;
			this.chkModeFlag.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkModeFlag, 2);
			this.chkModeFlag.Location = new System.Drawing.Point(3, 26);
			this.chkModeFlag.Name = "chkModeFlag";
			this.chkModeFlag.Size = new System.Drawing.Size(76, 17);
			this.chkModeFlag.TabIndex = 15;
			this.chkModeFlag.Text = "Mode Flag";
			this.chkModeFlag.UseVisualStyleBackColor = true;
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoCheck = false;
			this.chkEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkEnabled, 2);
			this.chkEnabled.Location = new System.Drawing.Point(3, 3);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 3;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// lblPeriod
			// 
			this.lblPeriod.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPeriod.AutoSize = true;
			this.lblPeriod.Location = new System.Drawing.Point(0, 52);
			this.lblPeriod.Margin = new System.Windows.Forms.Padding(0);
			this.lblPeriod.Name = "lblPeriod";
			this.lblPeriod.Size = new System.Drawing.Size(40, 13);
			this.lblPeriod.TabIndex = 0;
			this.lblPeriod.Text = "Period:";
			// 
			// txtPeriod
			// 
			this.txtPeriod.BackColor = System.Drawing.Color.White;
			this.txtPeriod.Location = new System.Drawing.Point(83, 49);
			this.txtPeriod.Name = "txtPeriod";
			this.txtPeriod.ReadOnly = true;
			this.txtPeriod.Size = new System.Drawing.Size(39, 20);
			this.txtPeriod.TabIndex = 4;
			// 
			// lblFrequency
			// 
			this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFrequency.AutoSize = true;
			this.lblFrequency.Location = new System.Drawing.Point(0, 104);
			this.lblFrequency.Margin = new System.Windows.Forms.Padding(0);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(60, 13);
			this.lblFrequency.TabIndex = 1;
			this.lblFrequency.Text = "Frequency:";
			// 
			// txtFrequency
			// 
			this.txtFrequency.BackColor = System.Drawing.Color.White;
			this.txtFrequency.Location = new System.Drawing.Point(83, 101);
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.ReadOnly = true;
			this.txtFrequency.Size = new System.Drawing.Size(59, 20);
			this.txtFrequency.TabIndex = 5;
			// 
			// lblShiftRegister
			// 
			this.lblShiftRegister.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblShiftRegister.AutoSize = true;
			this.lblShiftRegister.Location = new System.Drawing.Point(0, 130);
			this.lblShiftRegister.Margin = new System.Windows.Forms.Padding(0);
			this.lblShiftRegister.Name = "lblShiftRegister";
			this.lblShiftRegister.Size = new System.Drawing.Size(73, 13);
			this.lblShiftRegister.TabIndex = 6;
			this.lblShiftRegister.Text = "Shift Register:";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 156);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Output Volume:";
			// 
			// txtOutputVolume
			// 
			this.txtOutputVolume.BackColor = System.Drawing.Color.White;
			this.txtOutputVolume.Location = new System.Drawing.Point(83, 153);
			this.txtOutputVolume.Name = "txtOutputVolume";
			this.txtOutputVolume.ReadOnly = true;
			this.txtOutputVolume.Size = new System.Drawing.Size(39, 20);
			this.txtOutputVolume.TabIndex = 8;
			// 
			// txtShiftRegister
			// 
			this.txtShiftRegister.BackColor = System.Drawing.Color.White;
			this.txtShiftRegister.Location = new System.Drawing.Point(83, 127);
			this.txtShiftRegister.Name = "txtShiftRegister";
			this.txtShiftRegister.ReadOnly = true;
			this.txtShiftRegister.Size = new System.Drawing.Size(39, 20);
			this.txtShiftRegister.TabIndex = 9;
			// 
			// grpEnvelope
			// 
			this.grpEnvelope.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpEnvelope.Controls.Add(this.ctrlEnvelopeInfo);
			this.grpEnvelope.Location = new System.Drawing.Point(168, 3);
			this.grpEnvelope.Name = "grpEnvelope";
			this.tableLayoutPanel1.SetRowSpan(this.grpEnvelope, 8);
			this.grpEnvelope.Size = new System.Drawing.Size(122, 171);
			this.grpEnvelope.TabIndex = 12;
			this.grpEnvelope.TabStop = false;
			this.grpEnvelope.Text = "Envelope";
			// 
			// ctrlEnvelopeInfo
			// 
			this.ctrlEnvelopeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEnvelopeInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlEnvelopeInfo.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlEnvelopeInfo.Name = "ctrlEnvelopeInfo";
			this.ctrlEnvelopeInfo.Size = new System.Drawing.Size(116, 152);
			this.ctrlEnvelopeInfo.TabIndex = 1;
			// 
			// grpLengthCounter
			// 
			this.grpLengthCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpLengthCounter.Controls.Add(this.ctrlLengthCounterInfo);
			this.grpLengthCounter.Location = new System.Drawing.Point(296, 3);
			this.grpLengthCounter.Name = "grpLengthCounter";
			this.tableLayoutPanel1.SetRowSpan(this.grpLengthCounter, 8);
			this.grpLengthCounter.Size = new System.Drawing.Size(126, 171);
			this.grpLengthCounter.TabIndex = 10;
			this.grpLengthCounter.TabStop = false;
			this.grpLengthCounter.Text = "Length Counter";
			// 
			// ctrlLengthCounterInfo
			// 
			this.ctrlLengthCounterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLengthCounterInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlLengthCounterInfo.Name = "ctrlLengthCounterInfo";
			this.ctrlLengthCounterInfo.Size = new System.Drawing.Size(120, 152);
			this.ctrlLengthCounterInfo.TabIndex = 2;
			// 
			// lblHz
			// 
			this.lblHz.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHz.AutoSize = true;
			this.lblHz.Location = new System.Drawing.Point(145, 104);
			this.lblHz.Margin = new System.Windows.Forms.Padding(0);
			this.lblHz.Name = "lblHz";
			this.lblHz.Size = new System.Drawing.Size(20, 13);
			this.lblHz.TabIndex = 14;
			this.lblHz.Text = "Hz";
			// 
			// lblTimer
			// 
			this.lblTimer.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblTimer.AutoSize = true;
			this.lblTimer.Location = new System.Drawing.Point(0, 78);
			this.lblTimer.Margin = new System.Windows.Forms.Padding(0);
			this.lblTimer.Name = "lblTimer";
			this.lblTimer.Size = new System.Drawing.Size(36, 13);
			this.lblTimer.TabIndex = 16;
			this.lblTimer.Text = "Timer:";
			// 
			// txtTimer
			// 
			this.txtTimer.BackColor = System.Drawing.Color.White;
			this.txtTimer.Location = new System.Drawing.Point(83, 75);
			this.txtTimer.Name = "txtTimer";
			this.txtTimer.ReadOnly = true;
			this.txtTimer.Size = new System.Drawing.Size(39, 20);
			this.txtTimer.TabIndex = 17;
			// 
			// ctrlNoiseInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlNoiseInfo";
			this.Size = new System.Drawing.Size(454, 177);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpEnvelope.ResumeLayout(false);
			this.grpLengthCounter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblPeriod;
		private System.Windows.Forms.Label lblFrequency;
		private ApuViewer.ctrlEnvelopeInfo ctrlEnvelopeInfo;
		private ApuViewer.ctrlLengthCounterInfo ctrlLengthCounterInfo;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtPeriod;
		private System.Windows.Forms.TextBox txtFrequency;
		private System.Windows.Forms.Label lblShiftRegister;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOutputVolume;
		private System.Windows.Forms.TextBox txtShiftRegister;
		private System.Windows.Forms.GroupBox grpEnvelope;
		private System.Windows.Forms.GroupBox grpLengthCounter;
		private System.Windows.Forms.Label lblHz;
		private System.Windows.Forms.CheckBox chkModeFlag;
		private System.Windows.Forms.Label lblTimer;
		private System.Windows.Forms.TextBox txtTimer;
	}
}
