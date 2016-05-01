namespace Mesen.GUI.Forms.Config
{
	partial class ctrlDipSwitch
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblName = new System.Windows.Forms.Label();
			this.cboDipSwitch = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblName);
			this.flowLayoutPanel1.Controls.Add(this.cboDipSwitch);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(249, 27);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// lblName
			// 
			this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(3, 7);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(55, 13);
			this.lblName.TabIndex = 1;
			this.lblName.Text = "DipSwitch";
			// 
			// cboDipSwitch
			// 
			this.cboDipSwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDipSwitch.FormattingEnabled = true;
			this.cboDipSwitch.Location = new System.Drawing.Point(64, 3);
			this.cboDipSwitch.Name = "cboDipSwitch";
			this.cboDipSwitch.Size = new System.Drawing.Size(181, 21);
			this.cboDipSwitch.TabIndex = 2;
			// 
			// ctrlDipSwitch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "ctrlDipSwitch";
			this.Size = new System.Drawing.Size(249, 27);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.ComboBox cboDipSwitch;
	}
}
