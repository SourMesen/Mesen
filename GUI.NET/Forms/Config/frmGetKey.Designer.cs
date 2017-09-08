namespace Mesen.GUI.Forms.Config
{
	partial class frmGetKey
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblCurrentKeys = new System.Windows.Forms.Label();
			this.lblSetKeyMessage = new System.Windows.Forms.Label();
			this.tmrCheckKey = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblSetKeyMessage);
			this.groupBox1.Controls.Add(this.lblCurrentKeys);
			this.groupBox1.Location = new System.Drawing.Point(4, -1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(369, 102);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// lblCurrentKeys
			// 
			this.lblCurrentKeys.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblCurrentKeys.Location = new System.Drawing.Point(3, 55);
			this.lblCurrentKeys.Name = "lblCurrentKeys";
			this.lblCurrentKeys.Size = new System.Drawing.Size(363, 44);
			this.lblCurrentKeys.TabIndex = 1;
			this.lblCurrentKeys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSetKeyMessage
			// 
			this.lblSetKeyMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblSetKeyMessage.Location = new System.Drawing.Point(3, 16);
			this.lblSetKeyMessage.Name = "lblSetKeyMessage";
			this.lblSetKeyMessage.Size = new System.Drawing.Size(363, 39);
			this.lblSetKeyMessage.TabIndex = 0;
			this.lblSetKeyMessage.Text = "Press any key on your keyboard or controller to set a new binding.";
			this.lblSetKeyMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tmrCheckKey
			// 
			this.tmrCheckKey.Enabled = true;
			this.tmrCheckKey.Interval = 10;
			this.tmrCheckKey.Tick += new System.EventHandler(this.tmrCheckKey_Tick);
			// 
			// frmGetKey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(377, 104);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmGetKey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set key binding...";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblSetKeyMessage;
		private System.Windows.Forms.Timer tmrCheckKey;
		private System.Windows.Forms.Label lblCurrentKeys;
	}
}