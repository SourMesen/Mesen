namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlHexViewer
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlDataViewer = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblNumberOfColumns = new System.Windows.Forms.Label();
			this.cboNumberColumns = new System.Windows.Forms.ComboBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.ctrlDataViewer, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(191, 109);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// ctrlDataViewer
			// 
			this.ctrlDataViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlDataViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDataViewer.FontSize = 13F;
			this.ctrlDataViewer.Location = new System.Drawing.Point(3, 30);
			this.ctrlDataViewer.Name = "ctrlDataViewer";
			this.ctrlDataViewer.Size = new System.Drawing.Size(185, 76);
			this.ctrlDataViewer.TabIndex = 0;
			this.ctrlDataViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlDataViewer_MouseMove);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.lblNumberOfColumns);
			this.flowLayoutPanel1.Controls.Add(this.cboNumberColumns);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(27, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(164, 27);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// lblNumberOfColumns
			// 
			this.lblNumberOfColumns.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNumberOfColumns.AutoSize = true;
			this.lblNumberOfColumns.Location = new System.Drawing.Point(3, 7);
			this.lblNumberOfColumns.Name = "lblNumberOfColumns";
			this.lblNumberOfColumns.Size = new System.Drawing.Size(102, 13);
			this.lblNumberOfColumns.TabIndex = 0;
			this.lblNumberOfColumns.Text = "Number of Columns:";
			// 
			// cboNumberColumns
			// 
			this.cboNumberColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboNumberColumns.FormattingEnabled = true;
			this.cboNumberColumns.Items.AddRange(new object[] {
            "4",
            "8",
            "16",
            "32",
            "64"});
			this.cboNumberColumns.Location = new System.Drawing.Point(111, 3);
			this.cboNumberColumns.Name = "cboNumberColumns";
			this.cboNumberColumns.Size = new System.Drawing.Size(50, 21);
			this.cboNumberColumns.TabIndex = 1;
			this.cboNumberColumns.SelectedIndexChanged += new System.EventHandler(this.cboNumberColumns_SelectedIndexChanged);
			// 
			// ctrlHexViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlHexViewer";
			this.Size = new System.Drawing.Size(191, 109);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private ctrlScrollableTextbox ctrlDataViewer;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblNumberOfColumns;
		private System.Windows.Forms.ComboBox cboNumberColumns;
		private System.Windows.Forms.ToolTip toolTip;
	}
}
