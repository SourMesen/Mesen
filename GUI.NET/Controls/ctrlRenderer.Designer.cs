namespace Mesen.GUI.Controls
{
	partial class ctrlRenderer
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
			this.tmrMouse = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// tmrMouse
			// 
			this.tmrMouse.Interval = 3000;
			this.tmrMouse.Tick += new System.EventHandler(this.tmrMouse_Tick);
			// 
			// ctrlRenderer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "ctrlRenderer";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseDown);
			this.MouseLeave += new System.EventHandler(this.ctrlRenderer_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer tmrMouse;
	}
}
