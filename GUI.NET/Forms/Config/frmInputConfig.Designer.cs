namespace Mesen.GUI.Forms.Config
{
	partial class frmInputConfig
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
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tpgPort1 = new System.Windows.Forms.TabPage();
			this.ctrlInputPortConfig1 = new Mesen.GUI.Forms.Config.ctrlInputPortConfig();
			this.tpgPort2 = new System.Windows.Forms.TabPage();
			this.ctrlInputPortConfig2 = new Mesen.GUI.Forms.Config.ctrlInputPortConfig();
			this.tpgPort3 = new System.Windows.Forms.TabPage();
			this.ctrlInputPortConfig3 = new Mesen.GUI.Forms.Config.ctrlInputPortConfig();
			this.tpgPort4 = new System.Windows.Forms.TabPage();
			this.ctrlInputPortConfig4 = new Mesen.GUI.Forms.Config.ctrlInputPortConfig();
			this.tpgEmulatorKeys = new System.Windows.Forms.TabPage();
			this.tabMain.SuspendLayout();
			this.tpgPort1.SuspendLayout();
			this.tpgPort2.SuspendLayout();
			this.tpgPort3.SuspendLayout();
			this.tpgPort4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgPort1);
			this.tabMain.Controls.Add(this.tpgPort2);
			this.tabMain.Controls.Add(this.tpgPort3);
			this.tabMain.Controls.Add(this.tpgPort4);
			this.tabMain.Controls.Add(this.tpgEmulatorKeys);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(633, 376);
			this.tabMain.TabIndex = 11;
			// 
			// tpgPort1
			// 
			this.tpgPort1.Controls.Add(this.ctrlInputPortConfig1);
			this.tpgPort1.Location = new System.Drawing.Point(4, 22);
			this.tpgPort1.Name = "tpgPort1";
			this.tpgPort1.Size = new System.Drawing.Size(625, 350);
			this.tpgPort1.TabIndex = 0;
			this.tpgPort1.Text = "Port 1";
			this.tpgPort1.UseVisualStyleBackColor = true;
			// 
			// ctrlInputPortConfig1
			// 
			this.ctrlInputPortConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlInputPortConfig1.Location = new System.Drawing.Point(0, 0);
			this.ctrlInputPortConfig1.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlInputPortConfig1.Name = "ctrlInputPortConfig1";
			this.ctrlInputPortConfig1.Size = new System.Drawing.Size(625, 350);
			this.ctrlInputPortConfig1.TabIndex = 0;
			// 
			// tpgPort2
			// 
			this.tpgPort2.Controls.Add(this.ctrlInputPortConfig2);
			this.tpgPort2.Location = new System.Drawing.Point(4, 22);
			this.tpgPort2.Name = "tpgPort2";
			this.tpgPort2.Size = new System.Drawing.Size(625, 359);
			this.tpgPort2.TabIndex = 1;
			this.tpgPort2.Text = "Port 2";
			this.tpgPort2.UseVisualStyleBackColor = true;
			// 
			// ctrlInputPortConfig2
			// 
			this.ctrlInputPortConfig2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlInputPortConfig2.Location = new System.Drawing.Point(0, 0);
			this.ctrlInputPortConfig2.Name = "ctrlInputPortConfig2";
			this.ctrlInputPortConfig2.Size = new System.Drawing.Size(625, 359);
			this.ctrlInputPortConfig2.TabIndex = 1;
			// 
			// tpgPort3
			// 
			this.tpgPort3.Controls.Add(this.ctrlInputPortConfig3);
			this.tpgPort3.Location = new System.Drawing.Point(4, 22);
			this.tpgPort3.Name = "tpgPort3";
			this.tpgPort3.Size = new System.Drawing.Size(625, 359);
			this.tpgPort3.TabIndex = 2;
			this.tpgPort3.Text = "Port 3";
			this.tpgPort3.UseVisualStyleBackColor = true;
			// 
			// ctrlInputPortConfig3
			// 
			this.ctrlInputPortConfig3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlInputPortConfig3.Location = new System.Drawing.Point(0, 0);
			this.ctrlInputPortConfig3.Name = "ctrlInputPortConfig3";
			this.ctrlInputPortConfig3.Size = new System.Drawing.Size(625, 359);
			this.ctrlInputPortConfig3.TabIndex = 1;
			// 
			// tpgPort4
			// 
			this.tpgPort4.Controls.Add(this.ctrlInputPortConfig4);
			this.tpgPort4.Location = new System.Drawing.Point(4, 22);
			this.tpgPort4.Name = "tpgPort4";
			this.tpgPort4.Size = new System.Drawing.Size(625, 359);
			this.tpgPort4.TabIndex = 3;
			this.tpgPort4.Text = "Port 4";
			this.tpgPort4.UseVisualStyleBackColor = true;
			// 
			// ctrlInputPortConfig4
			// 
			this.ctrlInputPortConfig4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlInputPortConfig4.Location = new System.Drawing.Point(0, 0);
			this.ctrlInputPortConfig4.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlInputPortConfig4.Name = "ctrlInputPortConfig4";
			this.ctrlInputPortConfig4.Size = new System.Drawing.Size(625, 359);
			this.ctrlInputPortConfig4.TabIndex = 1;
			// 
			// tpgEmulatorKeys
			// 
			this.tpgEmulatorKeys.Location = new System.Drawing.Point(4, 22);
			this.tpgEmulatorKeys.Name = "tpgEmulatorKeys";
			this.tpgEmulatorKeys.Size = new System.Drawing.Size(625, 359);
			this.tpgEmulatorKeys.TabIndex = 4;
			this.tpgEmulatorKeys.Text = "Emulator Keys";
			this.tpgEmulatorKeys.UseVisualStyleBackColor = true;
			// 
			// frmInputConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 406);
			this.Controls.Add(this.tabMain);
			this.Name = "frmInputConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Input Settings";
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.tabMain.ResumeLayout(false);
			this.tpgPort1.ResumeLayout(false);
			this.tpgPort2.ResumeLayout(false);
			this.tpgPort3.ResumeLayout(false);
			this.tpgPort4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgPort2;
		private System.Windows.Forms.TabPage tpgPort3;
		private System.Windows.Forms.TabPage tpgPort4;
		private System.Windows.Forms.TabPage tpgEmulatorKeys;
		private System.Windows.Forms.TabPage tpgPort1;
		private ctrlInputPortConfig ctrlInputPortConfig1;
		private ctrlInputPortConfig ctrlInputPortConfig2;
		private ctrlInputPortConfig ctrlInputPortConfig3;
		private ctrlInputPortConfig ctrlInputPortConfig4;

	}
}