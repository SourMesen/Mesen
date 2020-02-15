namespace Mesen.GUI.Debugger
{
	partial class frmProfiler
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
			this.components = new System.ComponentModel.Container();
			this.ctrlProfiler = new Mesen.GUI.Debugger.Controls.ctrlProfiler();
			this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// ctrlProfiler
			// 
			this.ctrlProfiler.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlProfiler.Location = new System.Drawing.Point(0, 0);
			this.ctrlProfiler.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlProfiler.Name = "ctrlProfiler";
			this.ctrlProfiler.Size = new System.Drawing.Size(665, 385);
			this.ctrlProfiler.TabIndex = 1;
			// 
			// tmrRefresh
			// 
			this.tmrRefresh.Interval = 300;
			this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
			// 
			// frmProfiler
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(665, 385);
			this.Controls.Add(this.ctrlProfiler);
			this.Name = "frmProfiler";
			this.Text = "Performance Profiler";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.ctrlProfiler ctrlProfiler;
		private System.Windows.Forms.Timer tmrRefresh;
	}
}