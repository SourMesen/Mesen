namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlTriangleInfo
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
			this.lblSequencePosition = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtOutputVolume = new System.Windows.Forms.TextBox();
			this.txtSequencePosition = new System.Windows.Forms.TextBox();
			this.grpLengthCounter = new System.Windows.Forms.GroupBox();
			this.ctrlLengthCounterInfo = new Mesen.GUI.Debugger.Controls.ApuViewer.ctrlLengthCounterInfo();
			this.lblHz = new System.Windows.Forms.Label();
			this.lblTimer = new System.Windows.Forms.Label();
			this.txtTimer = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpLengthCounter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
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
			this.tableLayoutPanel1.Controls.Add(this.lblSequencePosition, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtOutputVolume, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtSequencePosition, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.grpLengthCounter, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblHz, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblTimer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtTimer, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(337, 156);
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
			this.lblPeriod.Location = new System.Drawing.Point(3, 29);
			this.lblPeriod.Name = "lblPeriod";
			this.lblPeriod.Size = new System.Drawing.Size(40, 13);
			this.lblPeriod.TabIndex = 0;
			this.lblPeriod.Text = "Period:";
			// 
			// txtPeriod
			// 
			this.txtPeriod.BackColor = System.Drawing.Color.White;
			this.txtPeriod.Location = new System.Drawing.Point(102, 26);
			this.txtPeriod.Name = "txtPeriod";
			this.txtPeriod.ReadOnly = true;
			this.txtPeriod.Size = new System.Drawing.Size(39, 20);
			this.txtPeriod.TabIndex = 4;
			// 
			// lblFrequency
			// 
			this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFrequency.AutoSize = true;
			this.lblFrequency.Location = new System.Drawing.Point(3, 81);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(60, 13);
			this.lblFrequency.TabIndex = 1;
			this.lblFrequency.Text = "Frequency:";
			// 
			// txtFrequency
			// 
			this.txtFrequency.BackColor = System.Drawing.Color.White;
			this.txtFrequency.Location = new System.Drawing.Point(102, 78);
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.ReadOnly = true;
			this.txtFrequency.Size = new System.Drawing.Size(59, 20);
			this.txtFrequency.TabIndex = 5;
			// 
			// lblSequencePosition
			// 
			this.lblSequencePosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSequencePosition.AutoSize = true;
			this.lblSequencePosition.Location = new System.Drawing.Point(0, 107);
			this.lblSequencePosition.Margin = new System.Windows.Forms.Padding(0);
			this.lblSequencePosition.Name = "lblSequencePosition";
			this.lblSequencePosition.Size = new System.Drawing.Size(99, 13);
			this.lblSequencePosition.TabIndex = 6;
			this.lblSequencePosition.Text = "Sequence Position:";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 133);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Output Volume:";
			// 
			// txtOutputVolume
			// 
			this.txtOutputVolume.BackColor = System.Drawing.Color.White;
			this.txtOutputVolume.Location = new System.Drawing.Point(102, 130);
			this.txtOutputVolume.Name = "txtOutputVolume";
			this.txtOutputVolume.ReadOnly = true;
			this.txtOutputVolume.Size = new System.Drawing.Size(39, 20);
			this.txtOutputVolume.TabIndex = 8;
			// 
			// txtSequencePosition
			// 
			this.txtSequencePosition.BackColor = System.Drawing.Color.White;
			this.txtSequencePosition.Location = new System.Drawing.Point(102, 104);
			this.txtSequencePosition.Name = "txtSequencePosition";
			this.txtSequencePosition.ReadOnly = true;
			this.txtSequencePosition.Size = new System.Drawing.Size(39, 20);
			this.txtSequencePosition.TabIndex = 9;
			// 
			// grpLengthCounter
			// 
			this.grpLengthCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpLengthCounter.Controls.Add(this.ctrlLengthCounterInfo);
			this.grpLengthCounter.Location = new System.Drawing.Point(187, 3);
			this.grpLengthCounter.Name = "grpLengthCounter";
			this.tableLayoutPanel1.SetRowSpan(this.grpLengthCounter, 7);
			this.grpLengthCounter.Size = new System.Drawing.Size(126, 150);
			this.grpLengthCounter.TabIndex = 10;
			this.grpLengthCounter.TabStop = false;
			this.grpLengthCounter.Text = "Length Counter";
			// 
			// ctrlLengthCounterInfo
			// 
			this.ctrlLengthCounterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLengthCounterInfo.Location = new System.Drawing.Point(3, 16);
			this.ctrlLengthCounterInfo.Name = "ctrlLengthCounterInfo";
			this.ctrlLengthCounterInfo.Size = new System.Drawing.Size(120, 131);
			this.ctrlLengthCounterInfo.TabIndex = 2;
			// 
			// lblHz
			// 
			this.lblHz.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHz.AutoSize = true;
			this.lblHz.Location = new System.Drawing.Point(164, 81);
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
			this.lblTimer.Location = new System.Drawing.Point(3, 55);
			this.lblTimer.Name = "lblTimer";
			this.lblTimer.Size = new System.Drawing.Size(36, 13);
			this.lblTimer.TabIndex = 15;
			this.lblTimer.Text = "Timer:";
			// 
			// txtTimer
			// 
			this.txtTimer.BackColor = System.Drawing.Color.White;
			this.txtTimer.Location = new System.Drawing.Point(102, 52);
			this.txtTimer.Name = "txtTimer";
			this.txtTimer.ReadOnly = true;
			this.txtTimer.Size = new System.Drawing.Size(39, 20);
			this.txtTimer.TabIndex = 16;
			// 
			// ctrlTriangleInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlTriangleInfo";
			this.Size = new System.Drawing.Size(337, 156);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpLengthCounter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblPeriod;
		private System.Windows.Forms.Label lblFrequency;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtPeriod;
		private System.Windows.Forms.TextBox txtFrequency;
		private System.Windows.Forms.Label lblSequencePosition;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOutputVolume;
		private System.Windows.Forms.TextBox txtSequencePosition;
		private System.Windows.Forms.Label lblHz;
		private System.Windows.Forms.GroupBox grpLengthCounter;
		private ApuViewer.ctrlLengthCounterInfo ctrlLengthCounterInfo;
		private System.Windows.Forms.Label lblTimer;
		private System.Windows.Forms.TextBox txtTimer;
	}
}
