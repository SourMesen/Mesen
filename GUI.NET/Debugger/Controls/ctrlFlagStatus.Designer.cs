namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlFlagStatus
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
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblLetter = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.LightGray;
			this.panel2.Controls.Add(this.lblLetter);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(20, 20);
			this.panel2.TabIndex = 2;
			// 
			// lblLetter
			// 
			this.lblLetter.BackColor = System.Drawing.Color.White;
			this.lblLetter.Location = new System.Drawing.Point(2, 2);
			this.lblLetter.Name = "lblLetter";
			this.lblLetter.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.lblLetter.Size = new System.Drawing.Size(16, 16);
			this.lblLetter.TabIndex = 2;
			this.lblLetter.Text = "C";
			this.lblLetter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ctrlFlagStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "ctrlFlagStatus";
			this.Size = new System.Drawing.Size(20, 20);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblLetter;
	}
}
