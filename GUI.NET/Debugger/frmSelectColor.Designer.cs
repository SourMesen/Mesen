namespace Mesen.GUI.Debugger
{
	partial class frmSelectColor
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
			this.ctrlPaletteDisplay = new Mesen.GUI.Debugger.ctrlPaletteDisplay();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblSelectColor = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctrlPaletteDisplay
			// 
			this.ctrlPaletteDisplay.Location = new System.Drawing.Point(6, 19);
			this.ctrlPaletteDisplay.Name = "ctrlPaletteDisplay";
			this.ctrlPaletteDisplay.Size = new System.Drawing.Size(338, 338);
			this.ctrlPaletteDisplay.TabIndex = 2;
			this.ctrlPaletteDisplay.ColorClick += new Mesen.GUI.Debugger.ctrlPaletteDisplay.PaletteClickHandler(this.ctrlPaletteDisplay_ColorClick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.ctrlPaletteDisplay, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblSelectColor, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 363);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// lblSelectColor
			// 
			this.lblSelectColor.AutoSize = true;
			this.lblSelectColor.Location = new System.Drawing.Point(6, 3);
			this.lblSelectColor.Name = "lblSelectColor";
			this.lblSelectColor.Size = new System.Drawing.Size(134, 13);
			this.lblSelectColor.TabIndex = 3;
			this.lblSelectColor.Text = "Click to select a new color:";
			// 
			// frmSelectColor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(350, 363);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmSelectColor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Color...";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private ctrlPaletteDisplay ctrlPaletteDisplay;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblSelectColor;
	}
}