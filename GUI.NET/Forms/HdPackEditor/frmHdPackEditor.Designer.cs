namespace Mesen.GUI.Forms.HdPackEditor
{
	partial class frmHdPackEditor
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grpPreview = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.picBankPreview = new System.Windows.Forms.PictureBox();
			this.cboBank = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cboChrBankSize = new System.Windows.Forms.ComboBox();
			this.lblBankSize = new System.Windows.Forms.Label();
			this.lblScale = new System.Windows.Forms.Label();
			this.cboScale = new System.Windows.Forms.ComboBox();
			this.chkSortByFrequency = new System.Windows.Forms.CheckBox();
			this.chkLargeSprites = new System.Windows.Forms.CheckBox();
			this.chkGroupBlankTiles = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblFolder = new System.Windows.Forms.Label();
			this.txtSaveFolder = new System.Windows.Forms.TextBox();
			this.btnSelectFolder = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnStartRecording = new System.Windows.Forms.Button();
			this.btnStopRecording = new System.Windows.Forms.Button();
			this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.grpPreview.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBankPreview)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.grpPreview, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(612, 376);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// grpPreview
			// 
			this.grpPreview.Controls.Add(this.tableLayoutPanel3);
			this.grpPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpPreview.Location = new System.Drawing.Point(340, 31);
			this.grpPreview.Name = "grpPreview";
			this.grpPreview.Size = new System.Drawing.Size(269, 310);
			this.grpPreview.TabIndex = 0;
			this.grpPreview.TabStop = false;
			this.grpPreview.Text = "CHR Bank Preview";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.picBankPreview, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.cboBank, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(263, 291);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// picBankPreview
			// 
			this.tableLayoutPanel3.SetColumnSpan(this.picBankPreview, 2);
			this.picBankPreview.Location = new System.Drawing.Point(3, 30);
			this.picBankPreview.Name = "picBankPreview";
			this.picBankPreview.Size = new System.Drawing.Size(257, 258);
			this.picBankPreview.TabIndex = 0;
			this.picBankPreview.TabStop = false;
			// 
			// cboBank
			// 
			this.cboBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBank.FormattingEnabled = true;
			this.cboBank.Location = new System.Drawing.Point(70, 3);
			this.cboBank.Name = "cboBank";
			this.cboBank.Size = new System.Drawing.Size(121, 21);
			this.cboBank.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "CHR Bank:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 31);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(331, 310);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.cboChrBankSize, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblBankSize, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblScale, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.cboScale, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.chkSortByFrequency, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkLargeSprites, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkGroupBlankTiles, 0, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(325, 291);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// cboChrBankSize
			// 
			this.cboChrBankSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChrBankSize.FormattingEnabled = true;
			this.cboChrBankSize.Items.AddRange(new object[] {
            "1 KB",
            "2 KB",
            "4 KB"});
			this.cboChrBankSize.Location = new System.Drawing.Point(93, 30);
			this.cboChrBankSize.Name = "cboChrBankSize";
			this.cboChrBankSize.Size = new System.Drawing.Size(121, 21);
			this.cboChrBankSize.TabIndex = 10;
			// 
			// lblBankSize
			// 
			this.lblBankSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBankSize.AutoSize = true;
			this.lblBankSize.Location = new System.Drawing.Point(3, 34);
			this.lblBankSize.Name = "lblBankSize";
			this.lblBankSize.Size = new System.Drawing.Size(84, 13);
			this.lblBankSize.TabIndex = 9;
			this.lblBankSize.Text = "CHR Bank Size:";
			// 
			// lblScale
			// 
			this.lblScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblScale.AutoSize = true;
			this.lblScale.Location = new System.Drawing.Point(3, 7);
			this.lblScale.Name = "lblScale";
			this.lblScale.Size = new System.Drawing.Size(64, 13);
			this.lblScale.TabIndex = 4;
			this.lblScale.Text = "Scale/Filter:";
			// 
			// cboScale
			// 
			this.cboScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboScale.FormattingEnabled = true;
			this.cboScale.Location = new System.Drawing.Point(93, 3);
			this.cboScale.Name = "cboScale";
			this.cboScale.Size = new System.Drawing.Size(121, 21);
			this.cboScale.TabIndex = 5;
			// 
			// chkSortByFrequency
			// 
			this.chkSortByFrequency.AutoSize = true;
			this.chkSortByFrequency.Checked = true;
			this.chkSortByFrequency.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.chkSortByFrequency, 2);
			this.chkSortByFrequency.Location = new System.Drawing.Point(3, 57);
			this.chkSortByFrequency.Name = "chkSortByFrequency";
			this.chkSortByFrequency.Size = new System.Drawing.Size(173, 17);
			this.chkSortByFrequency.TabIndex = 3;
			this.chkSortByFrequency.Text = "Sort pages by usage frequency";
			this.chkSortByFrequency.UseVisualStyleBackColor = true;
			// 
			// chkLargeSprites
			// 
			this.chkLargeSprites.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.chkLargeSprites, 2);
			this.chkLargeSprites.Location = new System.Drawing.Point(3, 103);
			this.chkLargeSprites.Name = "chkLargeSprites";
			this.chkLargeSprites.Size = new System.Drawing.Size(163, 17);
			this.chkLargeSprites.TabIndex = 8;
			this.chkLargeSprites.Text = "Use 8x16 sprite display mode";
			this.chkLargeSprites.UseVisualStyleBackColor = true;
			// 
			// chkGroupBlankTiles
			// 
			this.chkGroupBlankTiles.AutoSize = true;
			this.chkGroupBlankTiles.Checked = true;
			this.chkGroupBlankTiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.chkGroupBlankTiles, 2);
			this.chkGroupBlankTiles.Location = new System.Drawing.Point(3, 80);
			this.chkGroupBlankTiles.Name = "chkGroupBlankTiles";
			this.chkGroupBlankTiles.Size = new System.Drawing.Size(105, 17);
			this.chkGroupBlankTiles.TabIndex = 11;
			this.chkGroupBlankTiles.Text = "Group blank tiles";
			this.chkGroupBlankTiles.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.lblFolder);
			this.flowLayoutPanel1.Controls.Add(this.txtSaveFolder);
			this.flowLayoutPanel1.Controls.Add(this.btnSelectFolder);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(612, 28);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// lblFolder
			// 
			this.lblFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFolder.AutoSize = true;
			this.lblFolder.Location = new System.Drawing.Point(3, 8);
			this.lblFolder.Name = "lblFolder";
			this.lblFolder.Size = new System.Drawing.Size(67, 13);
			this.lblFolder.TabIndex = 0;
			this.lblFolder.Text = "Save Folder:";
			// 
			// txtSaveFolder
			// 
			this.txtSaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtSaveFolder.Location = new System.Drawing.Point(76, 4);
			this.txtSaveFolder.Name = "txtSaveFolder";
			this.txtSaveFolder.ReadOnly = true;
			this.txtSaveFolder.Size = new System.Drawing.Size(443, 20);
			this.txtSaveFolder.TabIndex = 1;
			this.txtSaveFolder.TabStop = false;
			// 
			// btnSelectFolder
			// 
			this.btnSelectFolder.AutoSize = true;
			this.btnSelectFolder.Location = new System.Drawing.Point(525, 3);
			this.btnSelectFolder.Name = "btnSelectFolder";
			this.btnSelectFolder.Size = new System.Drawing.Size(80, 23);
			this.btnSelectFolder.TabIndex = 8;
			this.btnSelectFolder.Text = "Select Folder";
			this.btnSelectFolder.UseVisualStyleBackColor = true;
			this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel2.Controls.Add(this.btnStartRecording);
			this.flowLayoutPanel2.Controls.Add(this.btnStopRecording);
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(348, 350);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(264, 26);
			this.flowLayoutPanel2.TabIndex = 7;
			// 
			// btnStartRecording
			// 
			this.btnStartRecording.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartRecording.AutoSize = true;
			this.btnStartRecording.Image = global::Mesen.GUI.Properties.Resources.Record;
			this.btnStartRecording.Location = new System.Drawing.Point(154, 3);
			this.btnStartRecording.Name = "btnStartRecording";
			this.btnStartRecording.Size = new System.Drawing.Size(107, 23);
			this.btnStartRecording.TabIndex = 6;
			this.btnStartRecording.Text = "Start Recording";
			this.btnStartRecording.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStartRecording.UseVisualStyleBackColor = true;
			this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
			// 
			// btnStopRecording
			// 
			this.btnStopRecording.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStopRecording.AutoSize = true;
			this.btnStopRecording.Image = global::Mesen.GUI.Properties.Resources.Stop;
			this.btnStopRecording.Location = new System.Drawing.Point(41, 3);
			this.btnStopRecording.Name = "btnStopRecording";
			this.btnStopRecording.Size = new System.Drawing.Size(107, 23);
			this.btnStopRecording.TabIndex = 7;
			this.btnStopRecording.Text = "Stop Recording";
			this.btnStopRecording.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStopRecording.UseVisualStyleBackColor = true;
			this.btnStopRecording.Visible = false;
			this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
			// 
			// tmrRefresh
			// 
			this.tmrRefresh.Interval = 200;
			this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
			// 
			// frmHdPackEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(612, 376);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmHdPackEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "HD Pack Builder";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpPreview.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBankPreview)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox grpPreview;
		private System.Windows.Forms.PictureBox picBankPreview;
		private System.Windows.Forms.Timer tmrRefresh;
		private System.Windows.Forms.ComboBox cboBank;
		private System.Windows.Forms.CheckBox chkSortByFrequency;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblScale;
		private System.Windows.Forms.ComboBox cboScale;
		private System.Windows.Forms.Button btnStartRecording;
		private System.Windows.Forms.CheckBox chkLargeSprites;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblFolder;
		private System.Windows.Forms.TextBox txtSaveFolder;
		private System.Windows.Forms.Button btnSelectFolder;
		private System.Windows.Forms.ComboBox cboChrBankSize;
		private System.Windows.Forms.Label lblBankSize;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button btnStopRecording;
		private System.Windows.Forms.CheckBox chkGroupBlankTiles;
	}
}