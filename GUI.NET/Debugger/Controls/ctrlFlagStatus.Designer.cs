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
			this.panelBorder = new System.Windows.Forms.Panel();
			this.lblLetter = new System.Windows.Forms.Label();
			this.panelBg = new System.Windows.Forms.Panel();
			this.panelBorder.SuspendLayout();
			this.panelBg.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelBorder
			// 
			this.panelBorder.BackColor = System.Drawing.Color.LightGray;
			this.panelBorder.Controls.Add(this.panelBg);
			this.panelBorder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBorder.Location = new System.Drawing.Point(0, 0);
			this.panelBorder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.panelBorder.Name = "panelBorder";
			this.panelBorder.Size = new System.Drawing.Size(20, 20);
			this.panelBorder.TabIndex = 2;
			// 
			// lblLetter
			// 
			this.lblLetter.Location = new System.Drawing.Point(1, 0);
			this.lblLetter.Margin = new System.Windows.Forms.Padding(0);
			this.lblLetter.Name = "lblLetter";
			this.lblLetter.Size = new System.Drawing.Size(16, 16);
			this.lblLetter.TabIndex = 2;
			this.lblLetter.Text = "C";
			this.lblLetter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panelBg
			// 
			this.panelBg.BackColor = System.Drawing.Color.White;
			this.panelBg.Controls.Add(this.lblLetter);
			this.panelBg.Location = new System.Drawing.Point(2, 2);
			this.panelBg.Margin = new System.Windows.Forms.Padding(0);
			this.panelBg.Name = "panelBg";
			this.panelBg.Size = new System.Drawing.Size(16, 16);
			this.panelBg.TabIndex = 3;
			// 
			// ctrlFlagStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelBorder);
			this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.Name = "ctrlFlagStatus";
			this.Size = new System.Drawing.Size(20, 20);
			this.panelBorder.ResumeLayout(false);
			this.panelBg.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBorder;
		private System.Windows.Forms.Label lblLetter;
		private System.Windows.Forms.Panel panelBg;
	}
}
