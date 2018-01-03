namespace Mesen.GUI.Debugger
{
	partial class frmDebuggerColors
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
			this.picActiveStatement = new System.Windows.Forms.PictureBox();
			this.lblActiveStatement = new System.Windows.Forms.Label();
			this.lblExecBreakpoint = new System.Windows.Forms.Label();
			this.lblUnidentifiedData = new System.Windows.Forms.Label();
			this.picExecBreakpoint = new System.Windows.Forms.PictureBox();
			this.picUnidentifiedData = new System.Windows.Forms.PictureBox();
			this.picWriteBreakpoint = new System.Windows.Forms.PictureBox();
			this.lblWriteBreakpoint = new System.Windows.Forms.Label();
			this.lblUnexecutedCode = new System.Windows.Forms.Label();
			this.lblVerifiedData = new System.Windows.Forms.Label();
			this.picUnexecutedCode = new System.Windows.Forms.PictureBox();
			this.picVerifiedData = new System.Windows.Forms.PictureBox();
			this.lblReadBreakpoint = new System.Windows.Forms.Label();
			this.picReadBreakpoint = new System.Windows.Forms.PictureBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.baseConfigPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picActiveStatement)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picExecBreakpoint)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picUnidentifiedData)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWriteBreakpoint)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picUnexecutedCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picVerifiedData)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picReadBreakpoint)).BeginInit();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.btnReset);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 116);
			this.baseConfigPanel.Size = new System.Drawing.Size(555, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.btnReset, 0);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.picActiveStatement, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblActiveStatement, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblExecBreakpoint, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblUnidentifiedData, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.picExecBreakpoint, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.picUnidentifiedData, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.picWriteBreakpoint, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblWriteBreakpoint, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblUnexecutedCode, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblVerifiedData, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.picUnexecutedCode, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.picVerifiedData, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblReadBreakpoint, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.picReadBreakpoint, 7, 1);
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
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(555, 145);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// picActiveStatement
			// 
			this.picActiveStatement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picActiveStatement.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picActiveStatement.Location = new System.Drawing.Point(136, 79);
			this.picActiveStatement.Name = "picActiveStatement";
			this.picActiveStatement.Size = new System.Drawing.Size(32, 32);
			this.picActiveStatement.TabIndex = 7;
			this.picActiveStatement.TabStop = false;
			this.picActiveStatement.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblActiveStatement
			// 
			this.lblActiveStatement.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblActiveStatement.AutoSize = true;
			this.lblActiveStatement.Location = new System.Drawing.Point(3, 88);
			this.lblActiveStatement.Name = "lblActiveStatement";
			this.lblActiveStatement.Size = new System.Drawing.Size(91, 13);
			this.lblActiveStatement.TabIndex = 4;
			this.lblActiveStatement.Text = "Active Statement:";
			// 
			// lblExecBreakpoint
			// 
			this.lblExecBreakpoint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExecBreakpoint.AutoSize = true;
			this.lblExecBreakpoint.Location = new System.Drawing.Point(3, 50);
			this.lblExecBreakpoint.Name = "lblExecBreakpoint";
			this.lblExecBreakpoint.Size = new System.Drawing.Size(88, 13);
			this.lblExecBreakpoint.TabIndex = 2;
			this.lblExecBreakpoint.Text = "Exec Breakpoint:";
			// 
			// lblUnidentifiedData
			// 
			this.lblUnidentifiedData.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblUnidentifiedData.AutoSize = true;
			this.lblUnidentifiedData.Location = new System.Drawing.Point(194, 12);
			this.lblUnidentifiedData.Name = "lblUnidentifiedData";
			this.lblUnidentifiedData.Size = new System.Drawing.Size(122, 13);
			this.lblUnidentifiedData.TabIndex = 10;
			this.lblUnidentifiedData.Text = "Unidentified Data/Code:";
			// 
			// picExecBreakpoint
			// 
			this.picExecBreakpoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picExecBreakpoint.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picExecBreakpoint.Location = new System.Drawing.Point(136, 41);
			this.picExecBreakpoint.Name = "picExecBreakpoint";
			this.picExecBreakpoint.Size = new System.Drawing.Size(32, 32);
			this.picExecBreakpoint.TabIndex = 6;
			this.picExecBreakpoint.TabStop = false;
			this.picExecBreakpoint.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picUnidentifiedData
			// 
			this.picUnidentifiedData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picUnidentifiedData.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picUnidentifiedData.Location = new System.Drawing.Point(327, 3);
			this.picUnidentifiedData.Name = "picUnidentifiedData";
			this.picUnidentifiedData.Size = new System.Drawing.Size(32, 32);
			this.picUnidentifiedData.TabIndex = 8;
			this.picUnidentifiedData.TabStop = false;
			this.picUnidentifiedData.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picWriteBreakpoint
			// 
			this.picWriteBreakpoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picWriteBreakpoint.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picWriteBreakpoint.Location = new System.Drawing.Point(327, 41);
			this.picWriteBreakpoint.Name = "picWriteBreakpoint";
			this.picWriteBreakpoint.Size = new System.Drawing.Size(32, 32);
			this.picWriteBreakpoint.TabIndex = 12;
			this.picWriteBreakpoint.TabStop = false;
			this.picWriteBreakpoint.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblWriteBreakpoint
			// 
			this.lblWriteBreakpoint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblWriteBreakpoint.AutoSize = true;
			this.lblWriteBreakpoint.Location = new System.Drawing.Point(194, 50);
			this.lblWriteBreakpoint.Name = "lblWriteBreakpoint";
			this.lblWriteBreakpoint.Size = new System.Drawing.Size(89, 13);
			this.lblWriteBreakpoint.TabIndex = 1;
			this.lblWriteBreakpoint.Text = "Write Breakpoint:";
			// 
			// lblUnexecutedCode
			// 
			this.lblUnexecutedCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblUnexecutedCode.AutoSize = true;
			this.lblUnexecutedCode.Location = new System.Drawing.Point(385, 12);
			this.lblUnexecutedCode.Name = "lblUnexecutedCode";
			this.lblUnexecutedCode.Size = new System.Drawing.Size(96, 13);
			this.lblUnexecutedCode.TabIndex = 0;
			this.lblUnexecutedCode.Text = "Unexecuted Code:";
			// 
			// lblVerifiedData
			// 
			this.lblVerifiedData.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblVerifiedData.AutoSize = true;
			this.lblVerifiedData.Location = new System.Drawing.Point(3, 12);
			this.lblVerifiedData.Name = "lblVerifiedData";
			this.lblVerifiedData.Size = new System.Drawing.Size(71, 13);
			this.lblVerifiedData.TabIndex = 11;
			this.lblVerifiedData.Text = "Verified Data:";
			// 
			// picUnexecutedCode
			// 
			this.picUnexecutedCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picUnexecutedCode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picUnexecutedCode.Location = new System.Drawing.Point(518, 3);
			this.picUnexecutedCode.Name = "picUnexecutedCode";
			this.picUnexecutedCode.Size = new System.Drawing.Size(32, 32);
			this.picUnexecutedCode.TabIndex = 5;
			this.picUnexecutedCode.TabStop = false;
			this.picUnexecutedCode.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// picVerifiedData
			// 
			this.picVerifiedData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picVerifiedData.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picVerifiedData.Location = new System.Drawing.Point(136, 3);
			this.picVerifiedData.Name = "picVerifiedData";
			this.picVerifiedData.Size = new System.Drawing.Size(32, 32);
			this.picVerifiedData.TabIndex = 9;
			this.picVerifiedData.TabStop = false;
			this.picVerifiedData.Click += new System.EventHandler(this.picColorPicker_Click);
			// 
			// lblReadBreakpoint
			// 
			this.lblReadBreakpoint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblReadBreakpoint.AutoSize = true;
			this.lblReadBreakpoint.Location = new System.Drawing.Point(385, 50);
			this.lblReadBreakpoint.Name = "lblReadBreakpoint";
			this.lblReadBreakpoint.Size = new System.Drawing.Size(90, 13);
			this.lblReadBreakpoint.TabIndex = 16;
			this.lblReadBreakpoint.Text = "Read Breakpoint:";
			// 
			// picReadBreakpoint
			// 
			this.picReadBreakpoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picReadBreakpoint.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picReadBreakpoint.Location = new System.Drawing.Point(518, 41);
			this.picReadBreakpoint.Name = "picReadBreakpoint";
			this.picReadBreakpoint.Size = new System.Drawing.Size(32, 32);
			this.picReadBreakpoint.TabIndex = 17;
			this.picReadBreakpoint.TabStop = false;
			this.picReadBreakpoint.Click += new System.EventHandler(this.picColorPicker_Click);
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
			// frmDebuggerColors
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 145);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmDebuggerColors";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configure Colors...";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picActiveStatement)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picExecBreakpoint)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picUnidentifiedData)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWriteBreakpoint)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picUnexecutedCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picVerifiedData)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picReadBreakpoint)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblActiveStatement;
		private System.Windows.Forms.Label lblExecBreakpoint;
		private System.Windows.Forms.Label lblUnexecutedCode;
		private System.Windows.Forms.Label lblWriteBreakpoint;
		private System.Windows.Forms.PictureBox picVerifiedData;
		private System.Windows.Forms.PictureBox picUnidentifiedData;
		private System.Windows.Forms.PictureBox picActiveStatement;
		private System.Windows.Forms.PictureBox picExecBreakpoint;
		private System.Windows.Forms.PictureBox picUnexecutedCode;
		private System.Windows.Forms.Label lblUnidentifiedData;
		private System.Windows.Forms.Label lblVerifiedData;
		private System.Windows.Forms.PictureBox picWriteBreakpoint;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label lblReadBreakpoint;
		private System.Windows.Forms.PictureBox picReadBreakpoint;
	}
}