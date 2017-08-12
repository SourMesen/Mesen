namespace Mesen.GUI.Forms.Config
{
	partial class ctrlPathSelection
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.tlpPath = new System.Windows.Forms.TableLayoutPanel();
			this.txtDisabledPath = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tlpPath.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.tlpPath, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(235, 21);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// txtPath
			// 
			this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPath.Location = new System.Drawing.Point(0, 0);
			this.txtPath.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(211, 20);
			this.txtPath.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(211, 1);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(24, 20);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tlpPath.ColumnCount = 2;
			this.tlpPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
			this.tlpPath.Controls.Add(this.txtDisabledPath, 1, 0);
			this.tlpPath.Controls.Add(this.txtPath, 0, 0);
			this.tlpPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpPath.Location = new System.Drawing.Point(0, 0);
			this.tlpPath.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.tlpPath.Name = "tableLayoutPanel2";
			this.tlpPath.RowCount = 1;
			this.tlpPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpPath.Size = new System.Drawing.Size(211, 21);
			this.tlpPath.TabIndex = 1;
			// 
			// txtDisabledPath
			// 
			this.txtDisabledPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDisabledPath.Location = new System.Drawing.Point(211, 0);
			this.txtDisabledPath.Margin = new System.Windows.Forms.Padding(0);
			this.txtDisabledPath.Name = "txtDisabledPath";
			this.txtDisabledPath.Size = new System.Drawing.Size(1, 20);
			this.txtDisabledPath.TabIndex = 1;
			// 
			// ctrlPathSelection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.MaximumSize = new System.Drawing.Size(1000, 21);
			this.MinimumSize = new System.Drawing.Size(0, 21);
			this.Name = "ctrlPathSelection";
			this.Size = new System.Drawing.Size(235, 21);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tlpPath.ResumeLayout(false);
			this.tlpPath.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TableLayoutPanel tlpPath;
		private System.Windows.Forms.TextBox txtDisabledPath;
	}
}
