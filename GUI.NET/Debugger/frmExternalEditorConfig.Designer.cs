namespace Mesen.GUI.Debugger
{
	partial class frmExternalEditorConfig
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
			this.txtArguments = new System.Windows.Forms.TextBox();
			this.lblPath = new System.Windows.Forms.Label();
			this.lblArguments = new System.Windows.Forms.Label();
			this.lblHint = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 127);
			this.baseConfigPanel.Size = new System.Drawing.Size(488, 29);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.txtArguments, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPath, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblArguments, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblHint, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPath, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(488, 127);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// txtArguments
			// 
			this.txtArguments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtArguments.Location = new System.Drawing.Point(137, 29);
			this.txtArguments.Name = "txtArguments";
			this.txtArguments.Size = new System.Drawing.Size(348, 20);
			this.txtArguments.TabIndex = 4;
			// 
			// lblPath
			// 
			this.lblPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPath.AutoSize = true;
			this.lblPath.Location = new System.Drawing.Point(3, 6);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(79, 13);
			this.lblPath.TabIndex = 0;
			this.lblPath.Text = "Editor filename:";
			// 
			// lblArguments
			// 
			this.lblArguments.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblArguments.AutoSize = true;
			this.lblArguments.Location = new System.Drawing.Point(3, 32);
			this.lblArguments.Name = "lblArguments";
			this.lblArguments.Size = new System.Drawing.Size(128, 13);
			this.lblArguments.TabIndex = 1;
			this.lblArguments.Text = "Command line arguments:";
			// 
			// lblHint
			// 
			this.lblHint.AutoSize = true;
			this.lblHint.Location = new System.Drawing.Point(137, 57);
			this.lblHint.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(183, 65);
			this.lblHint.TabIndex = 2;
			this.lblHint.Text = "%F = the file to edit\r\n%L = the line at which to open the file\r\n\r\ne.g: for Notepa" +
    "d++:\r\n%F -n%L";
			// 
			// txtPath
			// 
			this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPath.Location = new System.Drawing.Point(137, 3);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(348, 20);
			this.txtPath.TabIndex = 3;
			// 
			// frmExternalEditorConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(488, 156);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmExternalEditorConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configure external code editor...";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtArguments;
		private System.Windows.Forms.Label lblPath;
		private System.Windows.Forms.Label lblArguments;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.TextBox txtPath;
	}
}