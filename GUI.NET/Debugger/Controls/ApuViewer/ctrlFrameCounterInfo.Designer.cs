namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlFrameCounterInfo
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
			this.lblCurrentStep = new System.Windows.Forms.Label();
			this.txtCurrentStep = new System.Windows.Forms.TextBox();
			this.chkIrqEnabled = new System.Windows.Forms.CheckBox();
			this.chkFiveStepMode = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.lblCurrentStep, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtCurrentStep, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkIrqEnabled, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkFiveStepMode, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(125, 76);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblCurrentStep
			// 
			this.lblCurrentStep.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCurrentStep.AutoSize = true;
			this.lblCurrentStep.Location = new System.Drawing.Point(0, 52);
			this.lblCurrentStep.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.lblCurrentStep.Name = "lblCurrentStep";
			this.lblCurrentStep.Size = new System.Drawing.Size(69, 13);
			this.lblCurrentStep.TabIndex = 0;
			this.lblCurrentStep.Text = "Current Step:";
			// 
			// txtCurrentStep
			// 
			this.txtCurrentStep.BackColor = System.Drawing.Color.White;
			this.txtCurrentStep.Location = new System.Drawing.Point(72, 49);
			this.txtCurrentStep.Name = "txtCurrentStep";
			this.txtCurrentStep.ReadOnly = true;
			this.txtCurrentStep.Size = new System.Drawing.Size(39, 20);
			this.txtCurrentStep.TabIndex = 4;
			// 
			// chkIrqEnabled
			// 
			this.chkIrqEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkIrqEnabled.AutoCheck = false;
			this.chkIrqEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkIrqEnabled, 2);
			this.chkIrqEnabled.Location = new System.Drawing.Point(3, 26);
			this.chkIrqEnabled.Name = "chkIrqEnabled";
			this.chkIrqEnabled.Size = new System.Drawing.Size(87, 17);
			this.chkIrqEnabled.TabIndex = 3;
			this.chkIrqEnabled.Text = "IRQ Enabled";
			this.chkIrqEnabled.UseVisualStyleBackColor = true;
			// 
			// chkFiveStepMode
			// 
			this.chkFiveStepMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkFiveStepMode.AutoCheck = false;
			this.chkFiveStepMode.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkFiveStepMode, 2);
			this.chkFiveStepMode.Location = new System.Drawing.Point(3, 3);
			this.chkFiveStepMode.Name = "chkFiveStepMode";
			this.chkFiveStepMode.Size = new System.Drawing.Size(85, 17);
			this.chkFiveStepMode.TabIndex = 15;
			this.chkFiveStepMode.Text = "5-step Mode";
			this.chkFiveStepMode.UseVisualStyleBackColor = true;
			// 
			// ctrlFrameCounterInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlFrameCounterInfo";
			this.Size = new System.Drawing.Size(125, 76);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblCurrentStep;
		private System.Windows.Forms.CheckBox chkIrqEnabled;
		private System.Windows.Forms.TextBox txtCurrentStep;
		private System.Windows.Forms.CheckBox chkFiveStepMode;
	}
}
