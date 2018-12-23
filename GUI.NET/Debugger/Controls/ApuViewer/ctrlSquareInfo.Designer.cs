namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlSquareInfo
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
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblPeriod = new System.Windows.Forms.Label();
			this.txtPeriod = new System.Windows.Forms.TextBox();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.txtFrequency = new System.Windows.Forms.TextBox();
			this.lblDuty = new System.Windows.Forms.Label();
			this.lblDutyPosition = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtOutputVolume = new System.Windows.Forms.TextBox();
			this.txtDutyPosition = new System.Windows.Forms.TextBox();
			this.grpEnvelope = new System.Windows.Forms.GroupBox();
			this.grpSweep = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkSweepEnabled = new System.Windows.Forms.CheckBox();
			this.lblSweepShift = new System.Windows.Forms.Label();
			this.lblSweepPeriod = new System.Windows.Forms.Label();
			this.chkSweepNegate = new System.Windows.Forms.CheckBox();
			this.txtSweepPeriod = new System.Windows.Forms.TextBox();
			this.txtSweepShift = new System.Windows.Forms.TextBox();
			this.txtDuty = new System.Windows.Forms.TextBox();
			this.grpLengthCounter = new System.Windows.Forms.GroupBox();
			this.lblHz = new System.Windows.Forms.Label();
			this.lblTimer = new System.Windows.Forms.Label();
			this.txtTimer = new System.Windows.Forms.TextBox();
			this.ctrlEnvelopeInfo = new Mesen.GUI.Debugger.Controls.ApuViewer.ctrlEnvelopeInfo();
			this.ctrlLengthCounterInfo = new Mesen.GUI.Debugger.Controls.ApuViewer.ctrlLengthCounterInfo();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpEnvelope.SuspendLayout();
			this.grpSweep.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpLengthCounter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 7;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkEnabled, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblPeriod, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPeriod, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblFrequency, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtFrequency, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblDuty, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblDutyPosition, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtOutputVolume, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtDutyPosition, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.grpEnvelope, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpSweep, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtDuty, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.grpLengthCounter, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblHz, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblTimer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtTimer, 1, 2);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(526, 180);
			this.tableLayoutPanel1.TabIndex = 0;
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
			this.lblPeriod.Location = new System.Drawing.Point(0, 29);
			this.lblPeriod.Margin = new System.Windows.Forms.Padding(0);
			this.lblPeriod.Name = "lblPeriod";
			this.lblPeriod.Size = new System.Drawing.Size(40, 13);
			this.lblPeriod.TabIndex = 0;
			this.lblPeriod.Text = "Period:";
			// 
			// txtPeriod
			// 
			this.txtPeriod.BackColor = System.Drawing.Color.White;
			this.txtPeriod.Location = new System.Drawing.Point(83, 26);
			this.txtPeriod.Name = "txtPeriod";
			this.txtPeriod.ReadOnly = true;
			this.txtPeriod.Size = new System.Drawing.Size(39, 20);
			this.txtPeriod.TabIndex = 4;
			// 
			// lblFrequency
			// 
			this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFrequency.AutoSize = true;
			this.lblFrequency.Location = new System.Drawing.Point(0, 81);
			this.lblFrequency.Margin = new System.Windows.Forms.Padding(0);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(60, 13);
			this.lblFrequency.TabIndex = 1;
			this.lblFrequency.Text = "Frequency:";
			// 
			// txtFrequency
			// 
			this.txtFrequency.BackColor = System.Drawing.Color.White;
			this.txtFrequency.Location = new System.Drawing.Point(83, 78);
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.ReadOnly = true;
			this.txtFrequency.Size = new System.Drawing.Size(59, 20);
			this.txtFrequency.TabIndex = 5;
			// 
			// lblDuty
			// 
			this.lblDuty.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDuty.AutoSize = true;
			this.lblDuty.Location = new System.Drawing.Point(0, 107);
			this.lblDuty.Margin = new System.Windows.Forms.Padding(0);
			this.lblDuty.Name = "lblDuty";
			this.lblDuty.Size = new System.Drawing.Size(32, 13);
			this.lblDuty.TabIndex = 2;
			this.lblDuty.Text = "Duty:";
			// 
			// lblDutyPosition
			// 
			this.lblDutyPosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDutyPosition.AutoSize = true;
			this.lblDutyPosition.Location = new System.Drawing.Point(0, 133);
			this.lblDutyPosition.Margin = new System.Windows.Forms.Padding(0);
			this.lblDutyPosition.Name = "lblDutyPosition";
			this.lblDutyPosition.Size = new System.Drawing.Size(72, 13);
			this.lblDutyPosition.TabIndex = 6;
			this.lblDutyPosition.Text = "Duty Position:";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 159);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Output Volume:";
			// 
			// txtOutputVolume
			// 
			this.txtOutputVolume.BackColor = System.Drawing.Color.White;
			this.txtOutputVolume.Location = new System.Drawing.Point(83, 156);
			this.txtOutputVolume.Name = "txtOutputVolume";
			this.txtOutputVolume.ReadOnly = true;
			this.txtOutputVolume.Size = new System.Drawing.Size(39, 20);
			this.txtOutputVolume.TabIndex = 8;
			// 
			// txtDutyPosition
			// 
			this.txtDutyPosition.BackColor = System.Drawing.Color.White;
			this.txtDutyPosition.Location = new System.Drawing.Point(83, 130);
			this.txtDutyPosition.Name = "txtDutyPosition";
			this.txtDutyPosition.ReadOnly = true;
			this.txtDutyPosition.Size = new System.Drawing.Size(39, 20);
			this.txtDutyPosition.TabIndex = 9;
			// 
			// grpEnvelope
			// 
			this.grpEnvelope.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpEnvelope.Controls.Add(this.ctrlEnvelopeInfo);
			this.grpEnvelope.Location = new System.Drawing.Point(267, 3);
			this.grpEnvelope.Name = "grpEnvelope";
			this.tableLayoutPanel1.SetRowSpan(this.grpEnvelope, 7);
			this.grpEnvelope.Size = new System.Drawing.Size(116, 173);
			this.grpEnvelope.TabIndex = 12;
			this.grpEnvelope.TabStop = false;
			this.grpEnvelope.Text = "Envelope";
			// 
			// grpSweep
			// 
			this.grpSweep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpSweep.Controls.Add(this.tableLayoutPanel2);
			this.grpSweep.Location = new System.Drawing.Point(168, 3);
			this.grpSweep.Name = "grpSweep";
			this.tableLayoutPanel1.SetRowSpan(this.grpSweep, 7);
			this.grpSweep.Size = new System.Drawing.Size(93, 173);
			this.grpSweep.TabIndex = 11;
			this.grpSweep.TabStop = false;
			this.grpSweep.Text = "Sweep Unit";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.chkSweepEnabled, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSweepShift, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblSweepPeriod, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkSweepNegate, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtSweepPeriod, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtSweepShift, 1, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(87, 154);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// chkSweepEnabled
			// 
			this.chkSweepEnabled.AutoCheck = false;
			this.chkSweepEnabled.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkSweepEnabled, 2);
			this.chkSweepEnabled.Location = new System.Drawing.Point(3, 3);
			this.chkSweepEnabled.Name = "chkSweepEnabled";
			this.chkSweepEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkSweepEnabled.TabIndex = 6;
			this.chkSweepEnabled.Text = "Enabled";
			this.chkSweepEnabled.UseVisualStyleBackColor = true;
			// 
			// lblSweepShift
			// 
			this.lblSweepShift.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSweepShift.AutoSize = true;
			this.lblSweepShift.Location = new System.Drawing.Point(0, 78);
			this.lblSweepShift.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.lblSweepShift.Name = "lblSweepShift";
			this.lblSweepShift.Size = new System.Drawing.Size(31, 13);
			this.lblSweepShift.TabIndex = 7;
			this.lblSweepShift.Text = "Shift:";
			// 
			// lblSweepPeriod
			// 
			this.lblSweepPeriod.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSweepPeriod.AutoSize = true;
			this.lblSweepPeriod.Location = new System.Drawing.Point(0, 52);
			this.lblSweepPeriod.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.lblSweepPeriod.Name = "lblSweepPeriod";
			this.lblSweepPeriod.Size = new System.Drawing.Size(40, 13);
			this.lblSweepPeriod.TabIndex = 1;
			this.lblSweepPeriod.Text = "Period:";
			// 
			// chkSweepNegate
			// 
			this.chkSweepNegate.AutoCheck = false;
			this.chkSweepNegate.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkSweepNegate, 2);
			this.chkSweepNegate.Location = new System.Drawing.Point(3, 26);
			this.chkSweepNegate.Name = "chkSweepNegate";
			this.chkSweepNegate.Size = new System.Drawing.Size(61, 17);
			this.chkSweepNegate.TabIndex = 8;
			this.chkSweepNegate.Text = "Negate";
			this.chkSweepNegate.UseVisualStyleBackColor = true;
			// 
			// txtSweepPeriod
			// 
			this.txtSweepPeriod.BackColor = System.Drawing.Color.White;
			this.txtSweepPeriod.Location = new System.Drawing.Point(43, 49);
			this.txtSweepPeriod.Name = "txtSweepPeriod";
			this.txtSweepPeriod.ReadOnly = true;
			this.txtSweepPeriod.Size = new System.Drawing.Size(39, 20);
			this.txtSweepPeriod.TabIndex = 5;
			// 
			// txtSweepShift
			// 
			this.txtSweepShift.BackColor = System.Drawing.Color.White;
			this.txtSweepShift.Location = new System.Drawing.Point(43, 75);
			this.txtSweepShift.Name = "txtSweepShift";
			this.txtSweepShift.ReadOnly = true;
			this.txtSweepShift.Size = new System.Drawing.Size(39, 20);
			this.txtSweepShift.TabIndex = 9;
			// 
			// txtDuty
			// 
			this.txtDuty.BackColor = System.Drawing.Color.White;
			this.txtDuty.Location = new System.Drawing.Point(83, 104);
			this.txtDuty.Name = "txtDuty";
			this.txtDuty.ReadOnly = true;
			this.txtDuty.Size = new System.Drawing.Size(39, 20);
			this.txtDuty.TabIndex = 13;
			// 
			// grpLengthCounter
			// 
			this.grpLengthCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpLengthCounter.Controls.Add(this.ctrlLengthCounterInfo);
			this.grpLengthCounter.Location = new System.Drawing.Point(389, 3);
			this.grpLengthCounter.Name = "grpLengthCounter";
			this.tableLayoutPanel1.SetRowSpan(this.grpLengthCounter, 7);
			this.grpLengthCounter.Size = new System.Drawing.Size(130, 173);
			this.grpLengthCounter.TabIndex = 10;
			this.grpLengthCounter.TabStop = false;
			this.grpLengthCounter.Text = "Length Counter";
			// 
			// lblHz
			// 
			this.lblHz.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHz.AutoSize = true;
			this.lblHz.Location = new System.Drawing.Point(145, 81);
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
			this.lblTimer.Location = new System.Drawing.Point(0, 55);
			this.lblTimer.Margin = new System.Windows.Forms.Padding(0);
			this.lblTimer.Name = "lblTimer";
			this.lblTimer.Size = new System.Drawing.Size(36, 13);
			this.lblTimer.TabIndex = 15;
			this.lblTimer.Text = "Timer:";
			// 
			// txtTimer
			// 
			this.txtTimer.BackColor = System.Drawing.Color.White;
			this.txtTimer.Location = new System.Drawing.Point(83, 52);
			this.txtTimer.Name = "txtTimer";
			this.txtTimer.ReadOnly = true;
			this.txtTimer.Size = new System.Drawing.Size(39, 20);
			this.txtTimer.TabIndex = 16;
			// 
			// ctrlEnvelopeInfo
			// 
			this.ctrlEnvelopeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlEnvelopeInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlEnvelopeInfo.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlEnvelopeInfo.Name = "ctrlEnvelopeInfo";
			this.ctrlEnvelopeInfo.Size = new System.Drawing.Size(110, 154);
			this.ctrlEnvelopeInfo.TabIndex = 1;
			// 
			// ctrlLengthCounterInfo
			// 
			this.ctrlLengthCounterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLengthCounterInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlLengthCounterInfo.Name = "ctrlLengthCounterInfo";
			this.ctrlLengthCounterInfo.Size = new System.Drawing.Size(124, 154);
			this.ctrlLengthCounterInfo.TabIndex = 2;
			// 
			// ctrlSquareInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlSquareInfo";
			this.Size = new System.Drawing.Size(526, 180);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpEnvelope.ResumeLayout(false);
			this.grpSweep.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpLengthCounter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblPeriod;
		private System.Windows.Forms.Label lblFrequency;
		private ApuViewer.ctrlEnvelopeInfo ctrlEnvelopeInfo;
		private ApuViewer.ctrlLengthCounterInfo ctrlLengthCounterInfo;
		private System.Windows.Forms.Label lblDuty;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtPeriod;
		private System.Windows.Forms.TextBox txtFrequency;
		private System.Windows.Forms.Label lblDutyPosition;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOutputVolume;
		private System.Windows.Forms.TextBox txtDutyPosition;
		private System.Windows.Forms.GroupBox grpEnvelope;
		private System.Windows.Forms.GroupBox grpLengthCounter;
		private System.Windows.Forms.GroupBox grpSweep;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblSweepPeriod;
		private System.Windows.Forms.TextBox txtSweepPeriod;
		private System.Windows.Forms.CheckBox chkSweepEnabled;
		private System.Windows.Forms.Label lblSweepShift;
		private System.Windows.Forms.CheckBox chkSweepNegate;
		private System.Windows.Forms.TextBox txtSweepShift;
		private System.Windows.Forms.TextBox txtDuty;
		private System.Windows.Forms.Label lblHz;
		private System.Windows.Forms.Label lblTimer;
		private System.Windows.Forms.TextBox txtTimer;
	}
}
