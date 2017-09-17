namespace Mesen.GUI.Controls
{
	partial class ctrlRiskyOption
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
			this.chkOption = new System.Windows.Forms.CheckBox();
			this.lblNotRecommended = new System.Windows.Forms.Label();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.chkOption);
			this.flowLayoutPanel1.Controls.Add(this.lblNotRecommended);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(197, 23);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// chkOption
			// 
			this.chkOption.AutoSize = true;
			this.chkOption.Location = new System.Drawing.Point(3, 3);
			this.chkOption.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.chkOption.Name = "chkOption";
			this.chkOption.Size = new System.Drawing.Size(86, 17);
			this.chkOption.TabIndex = 0;
			this.chkOption.Text = "Option name";
			this.chkOption.UseVisualStyleBackColor = true;
			this.chkOption.CheckedChanged += new System.EventHandler(this.chkOption_CheckedChanged);
			// 
			// lblNotRecommended
			// 
			this.lblNotRecommended.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNotRecommended.AutoSize = true;
			this.lblNotRecommended.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNotRecommended.Location = new System.Drawing.Point(89, 5);
			this.lblNotRecommended.Margin = new System.Windows.Forms.Padding(0);
			this.lblNotRecommended.Name = "lblNotRecommended";
			this.lblNotRecommended.Size = new System.Drawing.Size(98, 13);
			this.lblNotRecommended.TabIndex = 1;
			this.lblNotRecommended.Text = "(not recommended)";
			// 
			// ctrlRiskyOption
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MinimumSize = new System.Drawing.Size(0, 23);
			this.Name = "ctrlRiskyOption";
			this.Size = new System.Drawing.Size(197, 23);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.CheckBox chkOption;
		private System.Windows.Forms.Label lblNotRecommended;
	}
}
