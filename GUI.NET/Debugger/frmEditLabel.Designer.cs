namespace Mesen.GUI.Debugger
{
	partial class frmEditLabel
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
			this.txtComment = new System.Windows.Forms.TextBox();
			this.lblLabel = new System.Windows.Forms.Label();
			this.lblComment = new System.Windows.Forms.Label();
			this.txtLabel = new System.Windows.Forms.TextBox();
			this.lblRegion = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.cboRegion = new System.Windows.Forms.ComboBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Size = new System.Drawing.Size(377, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.txtComment, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblLabel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtLabel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblRegion, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cboRegion, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(377, 233);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// txtComment
			// 
			this.txtComment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtComment.Location = new System.Drawing.Point(63, 82);
			this.txtComment.Multiline = true;
			this.txtComment.Name = "txtComment";
			this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtComment.Size = new System.Drawing.Size(311, 148);
			this.txtComment.TabIndex = 3;
			// 
			// lblLabel
			// 
			this.lblLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLabel.AutoSize = true;
			this.lblLabel.Location = new System.Drawing.Point(3, 59);
			this.lblLabel.Name = "lblLabel";
			this.lblLabel.Size = new System.Drawing.Size(36, 13);
			this.lblLabel.TabIndex = 0;
			this.lblLabel.Text = "Label:";
			// 
			// lblComment
			// 
			this.lblComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblComment.AutoSize = true;
			this.lblComment.Location = new System.Drawing.Point(3, 149);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(54, 13);
			this.lblComment.TabIndex = 1;
			this.lblComment.Text = "Comment:";
			// 
			// txtLabel
			// 
			this.txtLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLabel.Location = new System.Drawing.Point(63, 56);
			this.txtLabel.Name = "txtLabel";
			this.txtLabel.Size = new System.Drawing.Size(311, 20);
			this.txtLabel.TabIndex = 2;
			// 
			// lblRegion
			// 
			this.lblRegion.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRegion.AutoSize = true;
			this.lblRegion.Location = new System.Drawing.Point(3, 7);
			this.lblRegion.Name = "lblRegion";
			this.lblRegion.Size = new System.Drawing.Size(44, 13);
			this.lblRegion.TabIndex = 4;
			this.lblRegion.Text = "Region:";
			// 
			// lblAddress
			// 
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(3, 33);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(48, 13);
			this.lblAddress.TabIndex = 5;
			this.lblAddress.Text = "Address:";
			// 
			// cboRegion
			// 
			this.cboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRegion.FormattingEnabled = true;
			this.cboRegion.Location = new System.Drawing.Point(63, 3);
			this.cboRegion.Name = "cboRegion";
			this.cboRegion.Size = new System.Drawing.Size(121, 21);
			this.cboRegion.TabIndex = 6;
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(63, 30);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(57, 20);
			this.txtAddress.TabIndex = 7;
			// 
			// frmEditLabel
			// 
			this.AcceptButton = null;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(377, 262);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmEditLabel";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Label";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtComment;
		private System.Windows.Forms.Label lblLabel;
		private System.Windows.Forms.Label lblComment;
		private System.Windows.Forms.TextBox txtLabel;
		private System.Windows.Forms.Label lblRegion;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.ComboBox cboRegion;
		private System.Windows.Forms.TextBox txtAddress;
	}
}