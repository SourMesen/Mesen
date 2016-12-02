namespace Mesen.GUI.Debugger
{
	partial class frmFindOccurrences
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
			this.lblAddress = new System.Windows.Forms.Label();
			this.txtSearchString = new System.Windows.Forms.TextBox();
			this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 73);
			this.baseConfigPanel.Size = new System.Drawing.Size(251, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtSearchString, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkMatchWholeWord, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkMatchCase, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(251, 102);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblAddress
			// 
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 6);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(44, 13);
			this.lblAddress.TabIndex = 0;
			this.lblAddress.Text = "Search:";
			// 
			// txtSearchString
			// 
			this.txtSearchString.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSearchString.Location = new System.Drawing.Point(53, 3);
			this.txtSearchString.MaxLength = 255;
			this.txtSearchString.Name = "txtSearchString";
			this.txtSearchString.Size = new System.Drawing.Size(195, 20);
			this.txtSearchString.TabIndex = 1;
			// 
			// chkMatchWholeWord
			// 
			this.chkMatchWholeWord.AutoSize = true;
			this.chkMatchWholeWord.Location = new System.Drawing.Point(53, 52);
			this.chkMatchWholeWord.Name = "chkMatchWholeWord";
			this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
			this.chkMatchWholeWord.TabIndex = 2;
			this.chkMatchWholeWord.Text = "Match whole word";
			this.chkMatchWholeWord.UseVisualStyleBackColor = true;
			// 
			// chkMatchCase
			// 
			this.chkMatchCase.AutoSize = true;
			this.chkMatchCase.Location = new System.Drawing.Point(53, 29);
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
			this.chkMatchCase.TabIndex = 3;
			this.chkMatchCase.Text = "Match case";
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// frmFindOccurrences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(251, 102);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmFindOccurrences";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Find All Occurrences";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.TextBox txtSearchString;
		private System.Windows.Forms.CheckBox chkMatchWholeWord;
		private System.Windows.Forms.CheckBox chkMatchCase;
	}
}