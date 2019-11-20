using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlEventViewerPpuView
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
			this.picViewer = new Mesen.GUI.Debugger.ctrlImagePanel();
			this.tmrOverlay = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// picPicture
			// 
			this.picViewer.Cursor = System.Windows.Forms.Cursors.Default;
			this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picViewer.GridSizeX = 0;
			this.picViewer.GridSizeY = 0;
			this.picViewer.Image = null;
			this.picViewer.ImageScale = 1;
			this.picViewer.ImageSize = new System.Drawing.Size(0, 0);
			this.picViewer.Location = new System.Drawing.Point(0, 0);
			this.picViewer.Margin = new System.Windows.Forms.Padding(0);
			this.picViewer.Name = "picViewer";
			this.picViewer.Overlay = new System.Drawing.Rectangle(0, 0, 0, 0);
			this.picViewer.Selection = new System.Drawing.Rectangle(0, 0, 0, 0);
			this.picViewer.SelectionWrapPosition = 0;
			this.picViewer.Size = new System.Drawing.Size(481, 405);
			this.picViewer.TabIndex = 0;
			this.picViewer.TabStop = false;
			this.picViewer.MouseLeave += new System.EventHandler(this.picPicture_MouseLeave);
			this.picViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picViewer_MouseMove);
			// 
			// tmrOverlay
			// 
			this.tmrOverlay.Interval = 50;
			this.tmrOverlay.Tick += new System.EventHandler(this.tmrOverlay_Tick);
			// 
			// ctrlEventViewerPpuView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picViewer);
			this.Name = "ctrlEventViewerPpuView";
			this.Size = new System.Drawing.Size(481, 405);
			this.ResumeLayout(false);

		}

		#endregion

		private ctrlImagePanel picViewer;
		private System.Windows.Forms.Timer tmrOverlay;
	}
}
