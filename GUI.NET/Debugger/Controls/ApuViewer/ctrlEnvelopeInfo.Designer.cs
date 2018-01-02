namespace Mesen.GUI.Debugger.Controls.ApuViewer
{
	partial class ctrlEnvelopeInfo
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
			this.txtDivider = new System.Windows.Forms.TextBox();
			this.txtCounter = new System.Windows.Forms.TextBox();
			this.lblCounter = new System.Windows.Forms.Label();
			this.lblDivider = new System.Windows.Forms.Label();
			this.chkStart = new System.Windows.Forms.CheckBox();
			this.chkConstantVolume = new System.Windows.Forms.CheckBox();
			this.lblVolume = new System.Windows.Forms.Label();
			this.txtVolume = new System.Windows.Forms.TextBox();
			this.chkLoop = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.txtDivider, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtCounter, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblCounter, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblDivider, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.chkStart, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkConstantVolume, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblVolume, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtVolume, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.chkLoop, 0, 1);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(110, 147);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// txtDivider
			// 
			this.txtDivider.BackColor = System.Drawing.Color.White;
			this.txtDivider.Location = new System.Drawing.Point(56, 98);
			this.txtDivider.Name = "txtDivider";
			this.txtDivider.ReadOnly = true;
			this.txtDivider.Size = new System.Drawing.Size(40, 20);
			this.txtDivider.TabIndex = 5;
			// 
			// txtCounter
			// 
			this.txtCounter.BackColor = System.Drawing.Color.White;
			this.txtCounter.Location = new System.Drawing.Point(56, 72);
			this.txtCounter.Name = "txtCounter";
			this.txtCounter.ReadOnly = true;
			this.txtCounter.Size = new System.Drawing.Size(40, 20);
			this.txtCounter.TabIndex = 4;
			// 
			// lblCounter
			// 
			this.lblCounter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCounter.AutoSize = true;
			this.lblCounter.Location = new System.Drawing.Point(3, 75);
			this.lblCounter.Name = "lblCounter";
			this.lblCounter.Size = new System.Drawing.Size(47, 13);
			this.lblCounter.TabIndex = 2;
			this.lblCounter.Text = "Counter:";
			// 
			// lblDivider
			// 
			this.lblDivider.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDivider.AutoSize = true;
			this.lblDivider.Location = new System.Drawing.Point(3, 101);
			this.lblDivider.Name = "lblDivider";
			this.lblDivider.Size = new System.Drawing.Size(43, 13);
			this.lblDivider.TabIndex = 1;
			this.lblDivider.Text = "Divider:";
			// 
			// chkStart
			// 
			this.chkStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkStart.AutoCheck = false;
			this.chkStart.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkStart, 2);
			this.chkStart.Location = new System.Drawing.Point(0, 3);
			this.chkStart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.chkStart.Name = "chkStart";
			this.chkStart.Size = new System.Drawing.Size(71, 17);
			this.chkStart.TabIndex = 6;
			this.chkStart.Text = "Start Flag";
			this.chkStart.UseVisualStyleBackColor = true;
			// 
			// chkConstantVolume
			// 
			this.chkConstantVolume.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkConstantVolume.AutoCheck = false;
			this.chkConstantVolume.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkConstantVolume, 2);
			this.chkConstantVolume.Location = new System.Drawing.Point(0, 49);
			this.chkConstantVolume.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.chkConstantVolume.Name = "chkConstantVolume";
			this.chkConstantVolume.Size = new System.Drawing.Size(106, 17);
			this.chkConstantVolume.TabIndex = 8;
			this.chkConstantVolume.Text = "Constant Volume";
			this.chkConstantVolume.UseVisualStyleBackColor = true;
			// 
			// lblVolume
			// 
			this.lblVolume.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVolume.AutoSize = true;
			this.lblVolume.Location = new System.Drawing.Point(3, 127);
			this.lblVolume.Name = "lblVolume";
			this.lblVolume.Size = new System.Drawing.Size(45, 13);
			this.lblVolume.TabIndex = 9;
			this.lblVolume.Text = "Volume:";
			// 
			// txtVolume
			// 
			this.txtVolume.BackColor = System.Drawing.Color.White;
			this.txtVolume.Location = new System.Drawing.Point(56, 124);
			this.txtVolume.Name = "txtVolume";
			this.txtVolume.ReadOnly = true;
			this.txtVolume.Size = new System.Drawing.Size(40, 20);
			this.txtVolume.TabIndex = 10;
			// 
			// chkLoop
			// 
			this.chkLoop.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkLoop.AutoCheck = false;
			this.chkLoop.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkLoop, 2);
			this.chkLoop.Location = new System.Drawing.Point(0, 26);
			this.chkLoop.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.chkLoop.Name = "chkLoop";
			this.chkLoop.Size = new System.Drawing.Size(50, 17);
			this.chkLoop.TabIndex = 7;
			this.chkLoop.Text = "Loop";
			this.chkLoop.UseVisualStyleBackColor = true;
			// 
			// ctrlEnvelopeInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ctrlEnvelopeInfo";
			this.Size = new System.Drawing.Size(110, 147);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtDivider;
		private System.Windows.Forms.TextBox txtCounter;
		private System.Windows.Forms.Label lblCounter;
		private System.Windows.Forms.Label lblDivider;
		private System.Windows.Forms.CheckBox chkStart;
		private System.Windows.Forms.CheckBox chkLoop;
		private System.Windows.Forms.CheckBox chkConstantVolume;
		private System.Windows.Forms.Label lblVolume;
		private System.Windows.Forms.TextBox txtVolume;
	}
}
