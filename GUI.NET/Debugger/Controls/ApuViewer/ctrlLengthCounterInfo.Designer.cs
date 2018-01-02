namespace Mesen.GUI.Debugger.Controls.ApuViewer
{
	partial class ctrlLengthCounterInfo
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
			this.txtReloadValue = new System.Windows.Forms.TextBox();
			this.txtCounter = new System.Windows.Forms.TextBox();
			this.lblCounter = new System.Windows.Forms.Label();
			this.lblReloadValue = new System.Windows.Forms.Label();
			this.chkHalt = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.txtReloadValue, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtCounter, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblCounter, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblReloadValue, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkHalt, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(127, 77);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// txtReloadValue
			// 
			this.txtReloadValue.BackColor = System.Drawing.Color.White;
			this.txtReloadValue.Location = new System.Drawing.Point(77, 52);
			this.txtReloadValue.Name = "txtReloadValue";
			this.txtReloadValue.ReadOnly = true;
			this.txtReloadValue.Size = new System.Drawing.Size(40, 20);
			this.txtReloadValue.TabIndex = 5;
			// 
			// txtCounter
			// 
			this.txtCounter.BackColor = System.Drawing.Color.White;
			this.txtCounter.Location = new System.Drawing.Point(77, 26);
			this.txtCounter.Name = "txtCounter";
			this.txtCounter.ReadOnly = true;
			this.txtCounter.Size = new System.Drawing.Size(40, 20);
			this.txtCounter.TabIndex = 4;
			// 
			// lblCounter
			// 
			this.lblCounter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCounter.AutoSize = true;
			this.lblCounter.Location = new System.Drawing.Point(0, 29);
			this.lblCounter.Margin = new System.Windows.Forms.Padding(0);
			this.lblCounter.Name = "lblCounter";
			this.lblCounter.Size = new System.Drawing.Size(47, 13);
			this.lblCounter.TabIndex = 2;
			this.lblCounter.Text = "Counter:";
			// 
			// lblReloadValue
			// 
			this.lblReloadValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblReloadValue.AutoSize = true;
			this.lblReloadValue.Location = new System.Drawing.Point(0, 55);
			this.lblReloadValue.Margin = new System.Windows.Forms.Padding(0);
			this.lblReloadValue.Name = "lblReloadValue";
			this.lblReloadValue.Size = new System.Drawing.Size(74, 13);
			this.lblReloadValue.TabIndex = 1;
			this.lblReloadValue.Text = "Reload Value:";
			// 
			// chkHalt
			// 
			this.chkHalt.AutoCheck = false;
			this.chkHalt.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkHalt, 2);
			this.chkHalt.Location = new System.Drawing.Point(3, 3);
			this.chkHalt.Name = "chkHalt";
			this.chkHalt.Size = new System.Drawing.Size(45, 17);
			this.chkHalt.TabIndex = 6;
			this.chkHalt.Text = "Halt";
			this.chkHalt.UseVisualStyleBackColor = true;
			// 
			// ctrlLengthCounterInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlLengthCounterInfo";
			this.Size = new System.Drawing.Size(127, 77);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtReloadValue;
		private System.Windows.Forms.TextBox txtCounter;
		private System.Windows.Forms.Label lblCounter;
		private System.Windows.Forms.Label lblReloadValue;
		private System.Windows.Forms.CheckBox chkHalt;
	}
}
