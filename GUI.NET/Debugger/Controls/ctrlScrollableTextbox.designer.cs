namespace Mesen.GUI.Debugger
{
	partial class ctrlScrollableTextbox
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
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.ctrlTextbox = new Mesen.GUI.Debugger.ctrlTextbox();
			this.SuspendLayout();
			// 
			// vScrollBar
			// 
			this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar.LargeChange = 20;
			this.vScrollBar.Location = new System.Drawing.Point(130, 0);
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new System.Drawing.Size(18, 148);
			this.vScrollBar.TabIndex = 0;
			// 
			// ctrlTextbox
			// 
			this.ctrlTextbox.CursorPosition = -1;
			this.ctrlTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlTextbox.Font = new System.Drawing.Font("Consolas", 13F);
			this.ctrlTextbox.Location = new System.Drawing.Point(0, 0);
			this.ctrlTextbox.Name = "ctrlTextbox";
			this.ctrlTextbox.ScrollPosition = 0;
			this.ctrlTextbox.ShowLineInHex = false;
			this.ctrlTextbox.ShowLineNumbers = true;
			this.ctrlTextbox.Size = new System.Drawing.Size(130, 148);
			this.ctrlTextbox.TabIndex = 1;
			// 
			// ctrlScrollableTextbox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.ctrlTextbox);
			this.Controls.Add(this.vScrollBar);
			this.Name = "ctrlScrollableTextbox";
			this.Size = new System.Drawing.Size(148, 148);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar vScrollBar;
		private ctrlTextbox ctrlTextbox;

	}
}
