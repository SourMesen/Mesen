using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.NetPlay
{
	partial class frmServerConfig
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
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.chkPublicServer = new System.Windows.Forms.CheckBox();
			this.lblServerName = new System.Windows.Forms.Label();
			this.txtServerName = new System.Windows.Forms.TextBox();
			this.chkSpectator = new System.Windows.Forms.CheckBox();
			this.lblMaxPlayers = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.nudNbPlayers = new MesenNumericUpDown();
			this.tlpMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 161);
			this.baseConfigPanel.Size = new System.Drawing.Size(302, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.txtPort, 1, 1);
			this.tlpMain.Controls.Add(this.lblPort, 0, 1);
			this.tlpMain.Controls.Add(this.chkPublicServer, 0, 4);
			this.tlpMain.Controls.Add(this.lblServerName, 0, 0);
			this.tlpMain.Controls.Add(this.txtServerName, 1, 0);
			this.tlpMain.Controls.Add(this.chkSpectator, 0, 5);
			this.tlpMain.Controls.Add(this.lblMaxPlayers, 0, 3);
			this.tlpMain.Controls.Add(this.lblPassword, 0, 2);
			this.tlpMain.Controls.Add(this.txtPassword, 1, 2);
			this.tlpMain.Controls.Add(this.nudNbPlayers, 1, 3);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 7;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(302, 161);
			this.tlpMain.TabIndex = 1;
			// 
			// txtPort
			// 
			this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPort.Location = new System.Drawing.Point(128, 29);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(171, 20);
			this.txtPort.TabIndex = 13;
			this.txtPort.TextChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// lblPort
			// 
			this.lblPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(3, 32);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 12;
			this.lblPort.Text = "Port:";
			// 
			// chkPublicServer
			// 
			this.chkPublicServer.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkPublicServer, 2);
			this.chkPublicServer.Enabled = false;
			this.chkPublicServer.Location = new System.Drawing.Point(3, 107);
			this.chkPublicServer.Name = "chkPublicServer";
			this.chkPublicServer.Size = new System.Drawing.Size(87, 17);
			this.chkPublicServer.TabIndex = 11;
			this.chkPublicServer.Text = "Public server";
			this.chkPublicServer.UseVisualStyleBackColor = true;
			this.chkPublicServer.CheckedChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// lblServerName
			// 
			this.lblServerName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblServerName.AutoSize = true;
			this.lblServerName.Location = new System.Drawing.Point(3, 6);
			this.lblServerName.Name = "lblServerName";
			this.lblServerName.Size = new System.Drawing.Size(70, 13);
			this.lblServerName.TabIndex = 3;
			this.lblServerName.Text = "Server name:";
			// 
			// txtServerName
			// 
			this.txtServerName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtServerName.Location = new System.Drawing.Point(128, 3);
			this.txtServerName.Name = "txtServerName";
			this.txtServerName.Size = new System.Drawing.Size(171, 20);
			this.txtServerName.TabIndex = 5;
			this.txtServerName.TextChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// chkSpectator
			// 
			this.chkSpectator.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.chkSpectator, 2);
			this.chkSpectator.Enabled = false;
			this.chkSpectator.Location = new System.Drawing.Point(3, 130);
			this.chkSpectator.Name = "chkSpectator";
			this.chkSpectator.Size = new System.Drawing.Size(103, 17);
			this.chkSpectator.TabIndex = 7;
			this.chkSpectator.Text = "Allow spectators";
			this.chkSpectator.UseVisualStyleBackColor = true;
			this.chkSpectator.CheckedChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// lblMaxPlayers
			// 
			this.lblMaxPlayers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMaxPlayers.AutoSize = true;
			this.lblMaxPlayers.Location = new System.Drawing.Point(3, 84);
			this.lblMaxPlayers.Name = "lblMaxPlayers";
			this.lblMaxPlayers.Size = new System.Drawing.Size(119, 13);
			this.lblMaxPlayers.TabIndex = 4;
			this.lblMaxPlayers.Text = "Max. number of players:";
			// 
			// lblPassword
			// 
			this.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(3, 58);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 9;
			this.lblPassword.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPassword.Enabled = false;
			this.txtPassword.Location = new System.Drawing.Point(128, 55);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(171, 20);
			this.txtPassword.TabIndex = 10;
			this.txtPassword.UseSystemPasswordChar = true;
			this.txtPassword.TextChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// nudNbPlayers
			// 
			this.nudNbPlayers.Enabled = false;
			this.nudNbPlayers.Location = new System.Drawing.Point(128, 81);
			this.nudNbPlayers.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudNbPlayers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudNbPlayers.Name = "nudNbPlayers";
			this.nudNbPlayers.Size = new System.Drawing.Size(35, 20);
			this.nudNbPlayers.TabIndex = 8;
			this.nudNbPlayers.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudNbPlayers.ValueChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// frmServerConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 190);
			this.Controls.Add(this.tlpMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmServerConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Server Configuration";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tlpMain, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.Label lblServerName;
		private System.Windows.Forms.Label lblMaxPlayers;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.CheckBox chkSpectator;
		private MesenNumericUpDown nudNbPlayers;
		private System.Windows.Forms.CheckBox chkPublicServer;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;
	}
}