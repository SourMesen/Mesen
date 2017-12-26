namespace Mesen.GUI.Debugger
{
	partial class frmAssemblerColors
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
			this.picComment = new System.Windows.Forms.PictureBox();
			this.picAddress = new System.Windows.Forms.PictureBox();
			this.picImmediate = new System.Windows.Forms.PictureBox();
			this.picLabelDefinition = new System.Windows.Forms.PictureBox();
			this.lblImmediate = new System.Windows.Forms.Label();
			this.lblLabelDefinition = new System.Windows.Forms.Label();
			this.lblOpcode = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblComment = new System.Windows.Forms.Label();
			this.picOpcode = new System.Windows.Forms.PictureBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.baseConfigPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picComment)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAddress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picImmediate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLabelDefinition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picOpcode)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 210);
			this.baseConfigPanel.Size = new System.Drawing.Size(310, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.picComment, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.picAddress, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.picImmediate, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.picLabelDefinition, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblImmediate, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblLabelDefinition, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblOpcode, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.picOpcode, 1, 0);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 210);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// picComment
			// 
			this.picComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picComment.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picComment.Location = new System.Drawing.Point(97, 155);
			this.picComment.Name = "picComment";
			this.picComment.Size = new System.Drawing.Size(32, 32);
			this.picComment.TabIndex = 9;
			this.picComment.TabStop = false;
			this.picComment.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picAddress
			// 
			this.picAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picAddress.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picAddress.Location = new System.Drawing.Point(97, 117);
			this.picAddress.Name = "picAddress";
			this.picAddress.Size = new System.Drawing.Size(32, 32);
			this.picAddress.TabIndex = 8;
			this.picAddress.TabStop = false;
			this.picAddress.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picImmediate
			// 
			this.picImmediate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picImmediate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picImmediate.Location = new System.Drawing.Point(97, 79);
			this.picImmediate.Name = "picImmediate";
			this.picImmediate.Size = new System.Drawing.Size(32, 32);
			this.picImmediate.TabIndex = 7;
			this.picImmediate.TabStop = false;
			this.picImmediate.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picLabelDefinition
			// 
			this.picLabelDefinition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picLabelDefinition.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picLabelDefinition.Location = new System.Drawing.Point(97, 41);
			this.picLabelDefinition.Name = "picLabelDefinition";
			this.picLabelDefinition.Size = new System.Drawing.Size(32, 32);
			this.picLabelDefinition.TabIndex = 6;
			this.picLabelDefinition.TabStop = false;
			this.picLabelDefinition.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblImmediate
			// 
			this.lblImmediate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblImmediate.AutoSize = true;
			this.lblImmediate.Location = new System.Drawing.Point(3, 88);
			this.lblImmediate.Name = "lblImmediate";
			this.lblImmediate.Size = new System.Drawing.Size(88, 13);
			this.lblImmediate.TabIndex = 4;
			this.lblImmediate.Text = "Immediate Value:";
			// 
			// lblLabelDefinition
			// 
			this.lblLabelDefinition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLabelDefinition.AutoSize = true;
			this.lblLabelDefinition.Location = new System.Drawing.Point(3, 50);
			this.lblLabelDefinition.Name = "lblLabelDefinition";
			this.lblLabelDefinition.Size = new System.Drawing.Size(83, 13);
			this.lblLabelDefinition.TabIndex = 2;
			this.lblLabelDefinition.Text = "Label Definition:";
			// 
			// lblOpcode
			// 
			this.lblOpcode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblOpcode.AutoSize = true;
			this.lblOpcode.Location = new System.Drawing.Point(3, 12);
			this.lblOpcode.Name = "lblOpcode";
			this.lblOpcode.Size = new System.Drawing.Size(48, 13);
			this.lblOpcode.TabIndex = 0;
			this.lblOpcode.Text = "Opcode:";
			// 
			// lblAddress
			// 
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 126);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(77, 13);
			this.lblAddress.TabIndex = 3;
			this.lblAddress.Text = "Address value:";
			// 
			// lblComment
			// 
			this.lblComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblComment.AutoSize = true;
			this.lblComment.Location = new System.Drawing.Point(3, 164);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(51, 13);
			this.lblComment.TabIndex = 1;
			this.lblComment.Text = "Comment";
			// 
			// picOpcode
			// 
			this.picOpcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picOpcode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picOpcode.Location = new System.Drawing.Point(97, 3);
			this.picOpcode.Name = "picOpcode";
			this.picOpcode.Size = new System.Drawing.Size(32, 32);
			this.picOpcode.TabIndex = 5;
			this.picOpcode.TabStop = false;
			this.picOpcode.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(3, 3);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(102, 23);
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "Use default colors";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// frmAssemblerColors
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 239);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmAssemblerColors";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configure Colors...";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picComment)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAddress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picImmediate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLabelDefinition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picOpcode)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblImmediate;
		private System.Windows.Forms.Label lblLabelDefinition;
		private System.Windows.Forms.Label lblOpcode;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.Label lblComment;
		private System.Windows.Forms.PictureBox picComment;
		private System.Windows.Forms.PictureBox picAddress;
		private System.Windows.Forms.PictureBox picImmediate;
		private System.Windows.Forms.PictureBox picLabelDefinition;
		private System.Windows.Forms.PictureBox picOpcode;
		private System.Windows.Forms.Button btnReset;
	}
}