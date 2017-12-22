namespace Mesen.GUI.Forms.Config
{
	partial class frmMouseConfig
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblHigh = new System.Windows.Forms.Label();
			this.lblLow = new System.Windows.Forms.Label();
			this.lblSensitivity = new System.Windows.Forms.Label();
			this.trkSensitivity = new System.Windows.Forms.TrackBar();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkSensitivity)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 56);
			this.baseConfigPanel.Size = new System.Drawing.Size(250, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblSensitivity, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkSensitivity, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 56);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblHigh);
			this.panel1.Controls.Add(this.lblLow);
			this.panel1.Location = new System.Drawing.Point(63, 34);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(178, 13);
			this.panel1.TabIndex = 3;
			// 
			// lblHigh
			// 
			this.lblHigh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHigh.Location = new System.Drawing.Point(128, 0);
			this.lblHigh.Name = "lblHigh";
			this.lblHigh.Size = new System.Drawing.Size(47, 15);
			this.lblHigh.TabIndex = 1;
			this.lblHigh.Text = "High";
			this.lblHigh.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblLow
			// 
			this.lblLow.AutoSize = true;
			this.lblLow.Location = new System.Drawing.Point(3, 0);
			this.lblLow.Name = "lblLow";
			this.lblLow.Size = new System.Drawing.Size(27, 13);
			this.lblLow.TabIndex = 0;
			this.lblLow.Text = "Low";
			// 
			// lblSensitivity
			// 
			this.lblSensitivity.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSensitivity.AutoSize = true;
			this.lblSensitivity.Location = new System.Drawing.Point(3, 10);
			this.lblSensitivity.Name = "lblSensitivity";
			this.lblSensitivity.Size = new System.Drawing.Size(57, 13);
			this.lblSensitivity.TabIndex = 0;
			this.lblSensitivity.Text = "Sensitivity:";
			// 
			// trkSensitivity
			// 
			this.trkSensitivity.LargeChange = 1;
			this.trkSensitivity.Location = new System.Drawing.Point(66, 3);
			this.trkSensitivity.Maximum = 5;
			this.trkSensitivity.Name = "trkSensitivity";
			this.trkSensitivity.Size = new System.Drawing.Size(178, 28);
			this.trkSensitivity.TabIndex = 1;
			// 
			// frmMouseConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(250, 85);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmMouseConfig";
			this.Text = "Mouse Configuration";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkSensitivity)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblSensitivity;
		private System.Windows.Forms.TrackBar trkSensitivity;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblHigh;
		private System.Windows.Forms.Label lblLow;
	}
}