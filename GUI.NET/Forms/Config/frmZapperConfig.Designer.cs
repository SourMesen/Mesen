namespace Mesen.GUI.Forms.Config
{
	partial class frmZapperConfig
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
			this.lblLarge = new System.Windows.Forms.Label();
			this.lblSmall = new System.Windows.Forms.Label();
			this.lblDetectRadius = new System.Windows.Forms.Label();
			this.trkRadius = new System.Windows.Forms.TrackBar();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRadius)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 56);
			this.baseConfigPanel.Size = new System.Drawing.Size(275, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblDetectRadius, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkRadius, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(275, 56);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblLarge);
			this.panel1.Controls.Add(this.lblSmall);
			this.panel1.Location = new System.Drawing.Point(124, 34);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(135, 13);
			this.panel1.TabIndex = 3;
			// 
			// lblLarge
			// 
			this.lblLarge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblLarge.Location = new System.Drawing.Point(85, 0);
			this.lblLarge.Name = "lblLarge";
			this.lblLarge.Size = new System.Drawing.Size(47, 15);
			this.lblLarge.TabIndex = 1;
			this.lblLarge.Text = "Large";
			this.lblLarge.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblSmall
			// 
			this.lblSmall.AutoSize = true;
			this.lblSmall.Location = new System.Drawing.Point(3, 0);
			this.lblSmall.Name = "lblSmall";
			this.lblSmall.Size = new System.Drawing.Size(32, 13);
			this.lblSmall.TabIndex = 0;
			this.lblSmall.Text = "Small";
			// 
			// lblDetectRadius
			// 
			this.lblDetectRadius.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDetectRadius.AutoSize = true;
			this.lblDetectRadius.Location = new System.Drawing.Point(3, 10);
			this.lblDetectRadius.Name = "lblDetectRadius";
			this.lblDetectRadius.Size = new System.Drawing.Size(118, 13);
			this.lblDetectRadius.TabIndex = 0;
			this.lblDetectRadius.Text = "Light Detection Radius:";
			// 
			// trkRadius
			// 
			this.trkRadius.LargeChange = 1;
			this.trkRadius.Location = new System.Drawing.Point(127, 3);
			this.trkRadius.Maximum = 3;
			this.trkRadius.Name = "trkRadius";
			this.trkRadius.Size = new System.Drawing.Size(132, 28);
			this.trkRadius.TabIndex = 1;
			// 
			// frmZapperConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(275, 85);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmZapperConfig";
			this.Text = "Zapper Configuration";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRadius)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblDetectRadius;
		private System.Windows.Forms.TrackBar trkRadius;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblLarge;
		private System.Windows.Forms.Label lblSmall;
	}
}