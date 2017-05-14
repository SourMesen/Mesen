namespace Mesen.GUI.Forms
{
	partial class frmAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.labelProductName = new System.Windows.Forms.Label();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblWebsite = new System.Windows.Forms.Label();
			this.lblLink = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.picDonate = new System.Windows.Forms.PictureBox();
			this.lblDonate = new System.Windows.Forms.Label();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picDonate)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.flowLayoutPanel1, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Controls.Add(this.flowLayoutPanel2, 0, 6);
			this.tableLayoutPanel.Controls.Add(this.picDonate, 0, 5);
			this.tableLayoutPanel.Controls.Add(this.lblDonate, 0, 4);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(5, 5);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 7;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(337, 134);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// logoPictureBox
			// 
			this.logoPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.logoPictureBox.Image = global::Mesen.GUI.Properties.Resources.MesenLogo;
			this.logoPictureBox.Location = new System.Drawing.Point(10, 0);
			this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 4);
			this.logoPictureBox.Size = new System.Drawing.Size(64, 65);
			this.logoPictureBox.TabIndex = 12;
			this.logoPictureBox.TabStop = false;
			// 
			// labelProductName
			// 
			this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProductName.Location = new System.Drawing.Point(90, 0);
			this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new System.Drawing.Size(244, 17);
			this.labelProductName.TabIndex = 19;
			this.labelProductName.Text = "Mesen";
			this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCopyright.Location = new System.Drawing.Point(90, 34);
			this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(244, 17);
			this.labelCopyright.TabIndex = 21;
			this.labelCopyright.Text = "© 2017 M. Bibaud (aka Sour)";
			this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblWebsite);
			this.flowLayoutPanel1.Controls.Add(this.lblLink);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(84, 51);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(236, 18);
			this.flowLayoutPanel1.TabIndex = 26;
			// 
			// lblWebsite
			// 
			this.lblWebsite.AutoSize = true;
			this.lblWebsite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWebsite.Location = new System.Drawing.Point(6, 0);
			this.lblWebsite.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.lblWebsite.Name = "lblWebsite";
			this.lblWebsite.Size = new System.Drawing.Size(61, 16);
			this.lblWebsite.TabIndex = 25;
			this.lblWebsite.Text = "Website:";
			this.lblWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLink
			// 
			this.lblLink.AutoSize = true;
			this.lblLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLink.ForeColor = System.Drawing.Color.Blue;
			this.lblLink.Location = new System.Drawing.Point(67, 2);
			this.lblLink.Margin = new System.Windows.Forms.Padding(0, 2, 3, 0);
			this.lblLink.Name = "lblLink";
			this.lblLink.Size = new System.Drawing.Size(80, 13);
			this.lblLink.TabIndex = 26;
			this.lblLink.Text = "www.mesen.ca";
			this.lblLink.Click += new System.EventHandler(this.lblLink_Click);
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(90, 17);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(146, 17);
			this.labelVersion.TabIndex = 0;
			this.labelVersion.Text = "Version: 0.9.0 (Beta)";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(259, 108);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "&OK";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 137);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(64, 1);
			this.flowLayoutPanel2.TabIndex = 27;
			// 
			// picDonate
			// 
			this.picDonate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picDonate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picDonate.Image = ((System.Drawing.Image)(resources.GetObject("picDonate.Image")));
			this.picDonate.Location = new System.Drawing.Point(3, 108);
			this.picDonate.Name = "picDonate";
			this.picDonate.Size = new System.Drawing.Size(78, 22);
			this.picDonate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picDonate.TabIndex = 29;
			this.picDonate.TabStop = false;
			this.picDonate.Click += new System.EventHandler(this.picDonate_Click);
			// 
			// lblDonate
			// 
			this.tableLayoutPanel.SetColumnSpan(this.lblDonate, 2);
			this.lblDonate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDonate.Location = new System.Drawing.Point(0, 75);
			this.lblDonate.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.lblDonate.Name = "lblDonate";
			this.lblDonate.Size = new System.Drawing.Size(337, 30);
			this.lblDonate.TabIndex = 30;
			this.lblDonate.Text = "If you want to support Mesen, please consider donating.\r\nThank you for your suppo" +
    "rt!";
			// 
			// frmAbout
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(347, 144);
			this.Controls.Add(this.tableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About - Mesen";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picDonate)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.Label labelProductName;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblWebsite;
		private System.Windows.Forms.Label lblLink;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.PictureBox picDonate;
		private System.Windows.Forms.Label lblDonate;
	}
}
