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
			// frmWatchWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 322);
			this.Controls.Add(this.ctrlWatch);
			this.MinimumSize = new System.Drawing.Size(248, 137);
			this.Name = "frmWatchWindow";
			this.Text = "Watch Window";
			this.ResumeLayout(false);

		}

		#endregion

		private ctrlWatch ctrlWatch;
	}
}