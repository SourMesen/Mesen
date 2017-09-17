namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlTilePalette
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
			this.picPaletteSelection = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSelection)).BeginInit();
			this.SuspendLayout();
			// 
			// picPaletteSelection
			// 
			this.picPaletteSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPaletteSelection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picPaletteSelection.Location = new System.Drawing.Point(0, 0);
			this.picPaletteSelection.Name = "picPaletteSelection";
			this.picPaletteSelection.Size = new System.Drawing.Size(130, 34);
			this.picPaletteSelection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPaletteSelection.TabIndex = 14;
			this.picPaletteSelection.TabStop = false;
			this.picPaletteSelection.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPaletteSelection_MouseDown);
			this.picPaletteSelection.MouseLeave += new System.EventHandler(this.picPaletteSelection_MouseLeave);
			this.picPaletteSelection.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPaletteSelection_MouseMove);
			// 
			// ctrlTilePalette
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picPaletteSelection);
			this.Name = "ctrlTilePalette";
			this.Size = new System.Drawing.Size(130, 34);
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSelection)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPaletteSelection;
	}
}
