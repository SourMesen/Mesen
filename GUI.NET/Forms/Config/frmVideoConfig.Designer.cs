namespace Mesen.GUI.Forms.Config
{
	partial class frmVideoConfig
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblHost = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtHost = new System.Windows.Forms.TextBox();
			this.chkSpectator = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.txtPort, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblHost, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblPort, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtHost, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkSpectator, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 175);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// txtPort
			// 
			this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPort.Location = new System.Drawing.Point(41, 29);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(240, 20);
			this.txtPort.TabIndex = 6;
			this.txtPort.Text = "8888";
			// 
			// lblHost
			// 
			this.lblHost.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHost.AutoSize = true;
			this.lblHost.Location = new System.Drawing.Point(3, 6);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(32, 13);
			this.lblHost.TabIndex = 3;
			this.lblHost.Text = "Host:";
			// 
			// lblPort
			// 
			this.lblPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(3, 32);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 4;
			this.lblPort.Text = "Port:";
			// 
			// txtHost
			// 
			this.txtHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtHost.Location = new System.Drawing.Point(41, 3);
			this.txtHost.Name = "txtHost";
			this.txtHost.Size = new System.Drawing.Size(240, 20);
			this.txtHost.TabIndex = 5;
			this.txtHost.Text = "127.0.0.1";
			// 
			// chkSpectator
			// 
			this.chkSpectator.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkSpectator, 2);
			this.chkSpectator.Enabled = false;
			this.chkSpectator.Location = new System.Drawing.Point(3, 55);
			this.chkSpectator.Name = "chkSpectator";
			this.chkSpectator.Size = new System.Drawing.Size(106, 17);
			this.chkSpectator.TabIndex = 7;
			this.chkSpectator.Text = "Join as spectator";
			this.chkSpectator.UseVisualStyleBackColor = true;
			// 
			// frmVideoConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmVideoConfig";
			this.Text = "Video Options";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtHost;
		private System.Windows.Forms.CheckBox chkSpectator;
	}
}