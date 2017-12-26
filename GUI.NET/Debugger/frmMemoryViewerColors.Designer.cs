namespace Mesen.GUI.Debugger
{
	partial class frmMemoryViewerColors
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
			this.picChrDrawnByte = new System.Windows.Forms.PictureBox();
			this.lblChrDrawnByte = new System.Windows.Forms.Label();
			this.lblCodeByte = new System.Windows.Forms.Label();
			this.lblRead = new System.Windows.Forms.Label();
			this.picRead = new System.Windows.Forms.PictureBox();
			this.lblWrite = new System.Windows.Forms.Label();
			this.lblExecute = new System.Windows.Forms.Label();
			this.picCodeByte = new System.Windows.Forms.PictureBox();
			this.picWrite = new System.Windows.Forms.PictureBox();
			this.picExecute = new System.Windows.Forms.PictureBox();
			this.picDataByte = new System.Windows.Forms.PictureBox();
			this.picChrReadByte = new System.Windows.Forms.PictureBox();
			this.lblChrReadByte = new System.Windows.Forms.Label();
			this.lblDataByte = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.baseConfigPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrDrawnByte)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picCodeByte)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picExecute)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picDataByte)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrReadByte)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 122);
			this.baseConfigPanel.Size = new System.Drawing.Size(453, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.picChrDrawnByte, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblChrDrawnByte, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblCodeByte, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblRead, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.picRead, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblWrite, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblExecute, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.picCodeByte, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.picWrite, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.picExecute, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.picDataByte, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.picChrReadByte, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblChrReadByte, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblDataByte, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(453, 122);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// picChrDrawnByte
			// 
			this.picChrDrawnByte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrDrawnByte.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picChrDrawnByte.Location = new System.Drawing.Point(102, 79);
			this.picChrDrawnByte.Name = "picChrDrawnByte";
			this.picChrDrawnByte.Size = new System.Drawing.Size(32, 32);
			this.picChrDrawnByte.TabIndex = 7;
			this.picChrDrawnByte.TabStop = false;
			this.picChrDrawnByte.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblChrDrawnByte
			// 
			this.lblChrDrawnByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblChrDrawnByte.AutoSize = true;
			this.lblChrDrawnByte.Location = new System.Drawing.Point(3, 88);
			this.lblChrDrawnByte.Name = "lblChrDrawnByte";
			this.lblChrDrawnByte.Size = new System.Drawing.Size(91, 13);
			this.lblChrDrawnByte.TabIndex = 4;
			this.lblChrDrawnByte.Text = "CHR Drawn Byte:";
			// 
			// lblCodeByte
			// 
			this.lblCodeByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCodeByte.AutoSize = true;
			this.lblCodeByte.Location = new System.Drawing.Point(3, 50);
			this.lblCodeByte.Name = "lblCodeByte";
			this.lblCodeByte.Size = new System.Drawing.Size(59, 13);
			this.lblCodeByte.TabIndex = 2;
			this.lblCodeByte.Text = "Code Byte:";
			// 
			// lblRead
			// 
			this.lblRead.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRead.AutoSize = true;
			this.lblRead.Location = new System.Drawing.Point(3, 12);
			this.lblRead.Name = "lblRead";
			this.lblRead.Size = new System.Drawing.Size(36, 13);
			this.lblRead.TabIndex = 0;
			this.lblRead.Text = "Read:";
			// 
			// picRead
			// 
			this.picRead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picRead.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picRead.Location = new System.Drawing.Point(102, 3);
			this.picRead.Name = "picRead";
			this.picRead.Size = new System.Drawing.Size(32, 32);
			this.picRead.TabIndex = 5;
			this.picRead.TabStop = false;
			this.picRead.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblWrite
			// 
			this.lblWrite.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblWrite.AutoSize = true;
			this.lblWrite.Location = new System.Drawing.Point(160, 12);
			this.lblWrite.Name = "lblWrite";
			this.lblWrite.Size = new System.Drawing.Size(35, 13);
			this.lblWrite.TabIndex = 10;
			this.lblWrite.Text = "Write:";
			// 
			// lblExecute
			// 
			this.lblExecute.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExecute.AutoSize = true;
			this.lblExecute.Location = new System.Drawing.Point(317, 12);
			this.lblExecute.Name = "lblExecute";
			this.lblExecute.Size = new System.Drawing.Size(49, 13);
			this.lblExecute.TabIndex = 11;
			this.lblExecute.Text = "Execute:";
			// 
			// picCodeByte
			// 
			this.picCodeByte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picCodeByte.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picCodeByte.Location = new System.Drawing.Point(102, 41);
			this.picCodeByte.Name = "picCodeByte";
			this.picCodeByte.Size = new System.Drawing.Size(32, 32);
			this.picCodeByte.TabIndex = 6;
			this.picCodeByte.TabStop = false;
			this.picCodeByte.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picWrite
			// 
			this.picWrite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWrite.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picWrite.Location = new System.Drawing.Point(259, 3);
			this.picWrite.Name = "picWrite";
			this.picWrite.Size = new System.Drawing.Size(32, 32);
			this.picWrite.TabIndex = 8;
			this.picWrite.TabStop = false;
			this.picWrite.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picExecute
			// 
			this.picExecute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picExecute.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picExecute.Location = new System.Drawing.Point(416, 3);
			this.picExecute.Name = "picExecute";
			this.picExecute.Size = new System.Drawing.Size(32, 32);
			this.picExecute.TabIndex = 9;
			this.picExecute.TabStop = false;
			this.picExecute.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picDataByte
			// 
			this.picDataByte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picDataByte.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picDataByte.Location = new System.Drawing.Point(259, 41);
			this.picDataByte.Name = "picDataByte";
			this.picDataByte.Size = new System.Drawing.Size(32, 32);
			this.picDataByte.TabIndex = 12;
			this.picDataByte.TabStop = false;
			this.picDataByte.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picChrReadByte
			// 
			this.picChrReadByte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picChrReadByte.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picChrReadByte.Location = new System.Drawing.Point(259, 79);
			this.picChrReadByte.Name = "picChrReadByte";
			this.picChrReadByte.Size = new System.Drawing.Size(32, 32);
			this.picChrReadByte.TabIndex = 13;
			this.picChrReadByte.TabStop = false;
			this.picChrReadByte.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblChrReadByte
			// 
			this.lblChrReadByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblChrReadByte.AutoSize = true;
			this.lblChrReadByte.Location = new System.Drawing.Point(160, 88);
			this.lblChrReadByte.Name = "lblChrReadByte";
			this.lblChrReadByte.Size = new System.Drawing.Size(86, 13);
			this.lblChrReadByte.TabIndex = 3;
			this.lblChrReadByte.Text = "CHR Read Byte:";
			// 
			// lblDataByte
			// 
			this.lblDataByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDataByte.AutoSize = true;
			this.lblDataByte.Location = new System.Drawing.Point(160, 50);
			this.lblDataByte.Name = "lblDataByte";
			this.lblDataByte.Size = new System.Drawing.Size(57, 13);
			this.lblDataByte.TabIndex = 1;
			this.lblDataByte.Text = "Data Byte:";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(3, 3);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(102, 23);
			this.btnReset.TabIndex = 3;
			this.btnReset.Text = "Use default colors";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// frmMemoryViewerColors
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 151);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmMemoryViewerColors";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configure Colors...";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picChrDrawnByte)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRead)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picCodeByte)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWrite)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picExecute)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picDataByte)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picChrReadByte)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblChrDrawnByte;
		private System.Windows.Forms.Label lblCodeByte;
		private System.Windows.Forms.Label lblRead;
		private System.Windows.Forms.Label lblChrReadByte;
		private System.Windows.Forms.Label lblDataByte;
		private System.Windows.Forms.PictureBox picExecute;
		private System.Windows.Forms.PictureBox picWrite;
		private System.Windows.Forms.PictureBox picChrDrawnByte;
		private System.Windows.Forms.PictureBox picCodeByte;
		private System.Windows.Forms.PictureBox picRead;
		private System.Windows.Forms.Label lblWrite;
		private System.Windows.Forms.Label lblExecute;
		private System.Windows.Forms.PictureBox picDataByte;
		private System.Windows.Forms.PictureBox picChrReadByte;
		private System.Windows.Forms.Button btnReset;
	}
}