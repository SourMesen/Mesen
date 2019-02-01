namespace Mesen.GUI.Debugger
{
	partial class frmWatchWindow
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
			this.ctrlWatch = new Mesen.GUI.Debugger.ctrlWatch();
			this.picWatchHelp = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).BeginInit();
			this.SuspendLayout();
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(0, 0);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(317, 322);
			this.ctrlWatch.TabIndex = 0;
			// 
			// picWatchHelp
			// 
			this.picWatchHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picWatchHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picWatchHelp.Location = new System.Drawing.Point(297, 4);
			this.picWatchHelp.Name = "picWatchHelp";
			this.picWatchHelp.Size = new System.Drawing.Size(16, 16);
			this.picWatchHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWatchHelp.TabIndex = 2;
			this.picWatchHelp.TabStop = false;
			// 
			// frmWatchWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 322);
			this.Controls.Add(this.picWatchHelp);
			this.Controls.Add(this.ctrlWatch);
			this.MinimumSize = new System.Drawing.Size(248, 137);
			this.Name = "frmWatchWindow";
			this.Text = "Watch Window";
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private ctrlWatch ctrlWatch;
		private System.Windows.Forms.PictureBox picWatchHelp;
	}
}