namespace Mesen.GUI.Debugger
{
	partial class ctrlPaletteDisplay
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
			this.picPalette = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).BeginInit();
			this.SuspendLayout();
			// 
			// picPalette
			// 
			this.picPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPalette.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPalette.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picPalette.Location = new System.Drawing.Point(0, 0);
			this.picPalette.Margin = new System.Windows.Forms.Padding(1);
			this.picPalette.Name = "picPalette";
			this.picPalette.Size = new System.Drawing.Size(338, 338);
			this.picPalette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPalette.TabIndex = 1;
			this.picPalette.TabStop = false;
			this.picPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
			// 
			// ctrlPaletteDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picPalette);
			this.Name = "ctrlPaletteDisplay";
			this.Size = new System.Drawing.Size(338, 338);
			((System.ComponentModel.ISupportInitialize)(this.picPalette)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPalette;
	}
}
