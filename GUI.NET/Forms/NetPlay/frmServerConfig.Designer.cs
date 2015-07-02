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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServerConfig));
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.chkPublicServer = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblServerName = new System.Windows.Forms.Label();
			this.txtServerName = new System.Windows.Forms.TextBox();
			this.chkSpectator = new System.Windows.Forms.CheckBox();
			this.lblMaxPlayers = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.nudNbPlayers = new System.Windows.Forms.NumericUpDown();
			this.tlpMain.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNbPlayers)).BeginInit();
			this.SuspendLayout();
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.txtPort, 1, 1);
			this.tlpMain.Controls.Add(this.lblPort, 0, 1);
			this.tlpMain.Controls.Add(this.chkPublicServer, 0, 4);
			this.tlpMain.Controls.Add(this.flowLayoutPanel1, 0, 6);
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
			this.tlpMain.Size = new System.Drawing.Size(302, 190);
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
			// flowLayoutPanel1
			// 
			this.tlpMain.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Controls.Add(this.btnOK);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 161);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(302, 29);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(224, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(143, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "Start Server";
			this.btnOK.UseVisualStyleBackColor = true;
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
			this.txtPassword.Location = new System.Drawing.Point(128, 55);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(171, 20);
			this.txtPassword.TabIndex = 10;
			this.txtPassword.UseSystemPasswordChar = true;
			this.txtPassword.TextChanged += new System.EventHandler(this.Field_ValueChanged);
			// 
			// nudNbPlayers
			// 
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
			this.nudNbPlayers.Size = new System.Drawing.Size(39, 20);
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
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(302, 190);
			this.Controls.Add(this.tlpMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmServerConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Server Configuration";
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudNbPlayers)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblServerName;
		private System.Windows.Forms.Label lblMaxPlayers;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.CheckBox chkSpectator;
		private System.Windows.Forms.NumericUpDown nudNbPlayers;
		private System.Windows.Forms.CheckBox chkPublicServer;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;
	}
}