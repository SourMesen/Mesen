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
			this.components = new System.ComponentModel.Container();
			this.picPaletteSelection = new System.Windows.Forms.PictureBox();
			this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCopyHexColor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyRgbColor = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSelection)).BeginInit();
			this.ctxMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// picPaletteSelection
			// 
			this.picPaletteSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPaletteSelection.ContextMenuStrip = this.ctxMenu;
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
			// ctxMenu
			// 
			this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyHexColor,
            this.mnuCopyRgbColor});
			this.ctxMenu.Name = "ctxMenu";
			this.ctxMenu.Size = new System.Drawing.Size(160, 70);
			this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
			// 
			// mnuCopyHexColor
			// 
			this.mnuCopyHexColor.Name = "mnuCopyHexColor";
			this.mnuCopyHexColor.Size = new System.Drawing.Size(159, 22);
			this.mnuCopyHexColor.Text = "Copy Hex Color";
			this.mnuCopyHexColor.Click += new System.EventHandler(this.mnuCopyHexColor_Click);
			// 
			// mnuCopyRgbColor
			// 
			this.mnuCopyRgbColor.Name = "mnuCopyRgbColor";
			this.mnuCopyRgbColor.Size = new System.Drawing.Size(159, 22);
			this.mnuCopyRgbColor.Text = "Copy RGB Color";
			this.mnuCopyRgbColor.Click += new System.EventHandler(this.mnuCopyRgbColor_Click);
			// 
			// ctrlTilePalette
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picPaletteSelection);
			this.Name = "ctrlTilePalette";
			this.Size = new System.Drawing.Size(130, 34);
			((System.ComponentModel.ISupportInitialize)(this.picPaletteSelection)).EndInit();
			this.ctxMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPaletteSelection;
		private System.Windows.Forms.ContextMenuStrip ctxMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyHexColor;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyRgbColor;
	}
}
