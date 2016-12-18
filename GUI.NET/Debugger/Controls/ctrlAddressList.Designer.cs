namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlAddressList
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
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.ctrlDataViewer, 0, 1);
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
			this.ctrlDataViewer.Location = new System.Drawing.Point(3, 3);
			this.ctrlDataViewer.Name = "ctrlDataViewer";
			this.ctrlDataViewer.ShowContentNotes = false;
			this.ctrlDataViewer.ShowLineNumberNotes = false;
			this.ctrlDataViewer.Size = new System.Drawing.Size(185, 103);
			this.ctrlDataViewer.TabIndex = 0;
			// 
			// ctrlAddressList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlAddressList";
			this.Size = new System.Drawing.Size(191, 109);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private ctrlScrollableTextbox ctrlDataViewer;
		private System.Windows.Forms.ToolTip toolTip;
	}
}
