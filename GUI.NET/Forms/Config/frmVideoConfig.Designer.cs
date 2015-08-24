namespace Mesen.GUI.Forms.Config
{
	partial class frmVideoConfig
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
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.grpCropping = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.picOverscan = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblLeft = new System.Windows.Forms.Label();
			this.nudOverscanLeft = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblTop = new System.Windows.Forms.Label();
			this.nudOverscanTop = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblBottom = new System.Windows.Forms.Label();
			this.nudOverscanBottom = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblRight = new System.Windows.Forms.Label();
			this.nudOverscanRight = new System.Windows.Forms.NumericUpDown();
			this.chkShowFps = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblFpsLimit = new System.Windows.Forms.Label();
			this.cboFpsLimit = new System.Windows.Forms.ComboBox();
			this.tlpMain.SuspendLayout();
			this.grpCropping.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).BeginInit();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanLeft)).BeginInit();
			this.flowLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanTop)).BeginInit();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanBottom)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanRight)).BeginInit();
			this.flowLayoutPanel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 308);
			this.baseConfigPanel.Size = new System.Drawing.Size(362, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.grpCropping, 0, 2);
			this.tlpMain.Controls.Add(this.chkShowFps, 0, 1);
			this.tlpMain.Controls.Add(this.flowLayoutPanel6, 0, 0);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 3;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Size = new System.Drawing.Size(362, 308);
			this.tlpMain.TabIndex = 1;
			// 
			// grpCropping
			// 
			this.grpCropping.Controls.Add(this.tableLayoutPanel1);
			this.grpCropping.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCropping.Location = new System.Drawing.Point(3, 53);
			this.grpCropping.Name = "grpCropping";
			this.grpCropping.Size = new System.Drawing.Size(356, 252);
			this.grpCropping.TabIndex = 7;
			this.grpCropping.TabStop = false;
			this.grpCropping.Text = "Video Cropping";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.picOverscan, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel5, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 19);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(277, 234);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// picOverscan
			// 
			this.picOverscan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picOverscan.Location = new System.Drawing.Point(56, 43);
			this.picOverscan.Name = "picOverscan";
			this.picOverscan.Size = new System.Drawing.Size(165, 148);
			this.picOverscan.TabIndex = 1;
			this.picOverscan.TabStop = false;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.flowLayoutPanel3.Controls.Add(this.lblLeft);
			this.flowLayoutPanel3.Controls.Add(this.nudOverscanLeft);
			this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 97);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel3.TabIndex = 1;
			// 
			// lblLeft
			// 
			this.lblLeft.AutoSize = true;
			this.lblLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblLeft.Location = new System.Drawing.Point(3, 0);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(50, 13);
			this.lblLeft.TabIndex = 0;
			this.lblLeft.Text = "Left";
			this.lblLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanLeft
			// 
			this.nudOverscanLeft.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanLeft.Name = "nudOverscanLeft";
			this.nudOverscanLeft.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanLeft.TabIndex = 2;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel4.Controls.Add(this.lblTop);
			this.flowLayoutPanel4.Controls.Add(this.nudOverscanTop);
			this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(112, 0);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel4.TabIndex = 2;
			// 
			// lblTop
			// 
			this.lblTop.AutoSize = true;
			this.lblTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTop.Location = new System.Drawing.Point(3, 0);
			this.lblTop.Name = "lblTop";
			this.lblTop.Size = new System.Drawing.Size(50, 13);
			this.lblTop.TabIndex = 0;
			this.lblTop.Text = "Top";
			this.lblTop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanTop
			// 
			this.nudOverscanTop.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanTop.Name = "nudOverscanTop";
			this.nudOverscanTop.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanTop.TabIndex = 2;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel5.Controls.Add(this.lblBottom);
			this.flowLayoutPanel5.Controls.Add(this.nudOverscanBottom);
			this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(112, 194);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel5.TabIndex = 3;
			// 
			// lblBottom
			// 
			this.lblBottom.AutoSize = true;
			this.lblBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblBottom.Location = new System.Drawing.Point(3, 0);
			this.lblBottom.Name = "lblBottom";
			this.lblBottom.Size = new System.Drawing.Size(50, 13);
			this.lblBottom.TabIndex = 0;
			this.lblBottom.Text = "Bottom";
			this.lblBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanBottom
			// 
			this.nudOverscanBottom.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanBottom.Name = "nudOverscanBottom";
			this.nudOverscanBottom.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanBottom.TabIndex = 2;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.flowLayoutPanel2.Controls.Add(this.lblRight);
			this.flowLayoutPanel2.Controls.Add(this.nudOverscanRight);
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(224, 97);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(53, 40);
			this.flowLayoutPanel2.TabIndex = 0;
			// 
			// lblRight
			// 
			this.lblRight.AutoSize = true;
			this.lblRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblRight.Location = new System.Drawing.Point(3, 0);
			this.lblRight.Name = "lblRight";
			this.lblRight.Size = new System.Drawing.Size(50, 13);
			this.lblRight.TabIndex = 0;
			this.lblRight.Text = "Right";
			this.lblRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nudOverscanRight
			// 
			this.nudOverscanRight.Location = new System.Drawing.Point(3, 16);
			this.nudOverscanRight.Name = "nudOverscanRight";
			this.nudOverscanRight.Size = new System.Drawing.Size(50, 20);
			this.nudOverscanRight.TabIndex = 1;
			// 
			// chkShowFps
			// 
			this.chkShowFps.AutoSize = true;
			this.chkShowFps.Location = new System.Drawing.Point(3, 30);
			this.chkShowFps.Name = "chkShowFps";
			this.chkShowFps.Size = new System.Drawing.Size(76, 17);
			this.chkShowFps.TabIndex = 9;
			this.chkShowFps.Text = "Show FPS";
			this.chkShowFps.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.lblFpsLimit);
			this.flowLayoutPanel6.Controls.Add(this.cboFpsLimit);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(362, 27);
			this.flowLayoutPanel6.TabIndex = 10;
			// 
			// lblFpsLimit
			// 
			this.lblFpsLimit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFpsLimit.AutoSize = true;
			this.lblFpsLimit.Location = new System.Drawing.Point(3, 7);
			this.lblFpsLimit.Name = "lblFpsLimit";
			this.lblFpsLimit.Size = new System.Drawing.Size(54, 13);
			this.lblFpsLimit.TabIndex = 0;
			this.lblFpsLimit.Text = "FPS Limit:";
			// 
			// cboFpsLimit
			// 
			this.cboFpsLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFpsLimit.FormattingEnabled = true;
			this.cboFpsLimit.Items.AddRange(new object[] {
            "Auto",
            "No Limit",
            "60",
            "50",
            "30",
            "25",
            "15",
            "12"});
			this.cboFpsLimit.Location = new System.Drawing.Point(63, 3);
			this.cboFpsLimit.Name = "cboFpsLimit";
			this.cboFpsLimit.Size = new System.Drawing.Size(89, 21);
			this.cboFpsLimit.TabIndex = 1;
			// 
			// frmVideoConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(362, 337);
			this.Controls.Add(this.tlpMain);
			this.Name = "frmVideoConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Video Options";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tlpMain, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.grpCropping.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picOverscan)).EndInit();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanLeft)).EndInit();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanTop)).EndInit();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanBottom)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOverscanRight)).EndInit();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.GroupBox grpCropping;
		private System.Windows.Forms.CheckBox chkShowFps;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label lblLeft;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label lblTop;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Label lblBottom;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblRight;
		private System.Windows.Forms.PictureBox picOverscan;
		private System.Windows.Forms.NumericUpDown nudOverscanLeft;
		private System.Windows.Forms.NumericUpDown nudOverscanTop;
		private System.Windows.Forms.NumericUpDown nudOverscanBottom;
		private System.Windows.Forms.NumericUpDown nudOverscanRight;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label lblFpsLimit;
		private System.Windows.Forms.ComboBox cboFpsLimit;
	}
}