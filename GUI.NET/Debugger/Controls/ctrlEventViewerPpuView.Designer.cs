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
			this.picPicture = new System.Windows.Forms.PictureBox();
			this.tmrOverlay = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// picPicture
			// 
			this.picPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPicture.Cursor = System.Windows.Forms.Cursors.Default;
			this.picPicture.Location = new System.Drawing.Point(1, 1);
			this.picPicture.Margin = new System.Windows.Forms.Padding(0);
			this.picPicture.Name = "picPicture";
			this.picPicture.Size = new System.Drawing.Size(684, 526);
			this.picPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPicture.TabIndex = 0;
			this.picPicture.TabStop = false;
			this.picPicture.MouseLeave += new System.EventHandler(this.picPicture_MouseLeave);
			this.picPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPicture_MouseMove);
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
			this.Controls.Add(this.picPicture);
			this.Name = "ctrlEventViewerPpuView";
			this.Size = new System.Drawing.Size(686, 530);
			((System.ComponentModel.ISupportInitialize)(this.picPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPicture;
		private System.Windows.Forms.Timer tmrOverlay;
	}
}
