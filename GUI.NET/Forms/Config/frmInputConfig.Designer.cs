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
			this.tpgControllers = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSetupP4 = new System.Windows.Forms.Button();
			this.btnSetupP3 = new System.Windows.Forms.Button();
			this.lblPlayer1 = new System.Windows.Forms.Label();
			this.lblPlayer2 = new System.Windows.Forms.Label();
			this.cboPlayer4 = new System.Windows.Forms.ComboBox();
			this.cboPlayer3 = new System.Windows.Forms.ComboBox();
			this.cboPlayer1 = new System.Windows.Forms.ComboBox();
			this.lblPlayer4 = new System.Windows.Forms.Label();
			this.cboPlayer2 = new System.Windows.Forms.ComboBox();
			this.lblPlayer3 = new System.Windows.Forms.Label();
			this.btnSetupP1 = new System.Windows.Forms.Button();
			this.btnSetupP2 = new System.Windows.Forms.Button();
			this.lblNesType = new System.Windows.Forms.Label();
			this.cboConsoleType = new System.Windows.Forms.ComboBox();
			this.lblExpansionPort = new System.Windows.Forms.Label();
			this.cboExpansionPort = new System.Windows.Forms.ComboBox();
			this.chkFourScore = new System.Windows.Forms.CheckBox();
			this.tpgEmulatorKeys = new System.Windows.Forms.TabPage();
			this.tabMain.SuspendLayout();
			this.tpgControllers.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 224);
			this.baseConfigPanel.Size = new System.Drawing.Size(348, 29);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tpgControllers);
			this.tabMain.Controls.Add(this.tpgEmulatorKeys);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(348, 253);
			this.tabMain.TabIndex = 11;
			// 
			// tpgControllers
			// 
			this.tpgControllers.Controls.Add(this.tableLayoutPanel1);
			this.tpgControllers.Location = new System.Drawing.Point(4, 22);
			this.tpgControllers.Name = "tpgControllers";
			this.tpgControllers.Size = new System.Drawing.Size(340, 227);
			this.tpgControllers.TabIndex = 0;
			this.tpgControllers.Text = "Controllers";
			this.tpgControllers.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.btnSetupP4, 2, 7);
			this.tableLayoutPanel1.Controls.Add(this.btnSetupP3, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblPlayer1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblPlayer2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.cboPlayer4, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.cboPlayer3, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.cboPlayer1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblPlayer4, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.cboPlayer2, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblPlayer3, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.btnSetupP1, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnSetupP2, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblNesType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cboConsoleType, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblExpansionPort, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.cboExpansionPort, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.chkFourScore, 0, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 9;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(340, 227);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnSetupP4
			// 
			this.btnSetupP4.Location = new System.Drawing.Point(279, 171);
			this.btnSetupP4.Name = "btnSetupP4";
			this.btnSetupP4.Size = new System.Drawing.Size(56, 21);
			this.btnSetupP4.TabIndex = 12;
			this.btnSetupP4.Text = "Setup";
			this.btnSetupP4.UseVisualStyleBackColor = true;
			this.btnSetupP4.Click += new System.EventHandler(this.btnSetup_Click);
			// 
			// btnSetupP3
			// 
			this.btnSetupP3.Location = new System.Drawing.Point(279, 144);
			this.btnSetupP3.Name = "btnSetupP3";
			this.btnSetupP3.Size = new System.Drawing.Size(56, 21);
			this.btnSetupP3.TabIndex = 11;
			this.btnSetupP3.Text = "Setup";
			this.btnSetupP3.UseVisualStyleBackColor = true;
			this.btnSetupP3.Click += new System.EventHandler(this.btnSetup_Click);
			// 
			// lblPlayer1
			// 
			this.lblPlayer1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPlayer1.AutoSize = true;
			this.lblPlayer1.Location = new System.Drawing.Point(3, 44);
			this.lblPlayer1.Name = "lblPlayer1";
			this.lblPlayer1.Size = new System.Drawing.Size(48, 13);
			this.lblPlayer1.TabIndex = 0;
			this.lblPlayer1.Text = "Player 1:";
			// 
			// lblPlayer2
			// 
			this.lblPlayer2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPlayer2.AutoSize = true;
			this.lblPlayer2.Location = new System.Drawing.Point(3, 71);
			this.lblPlayer2.Name = "lblPlayer2";
			this.lblPlayer2.Size = new System.Drawing.Size(48, 13);
			this.lblPlayer2.TabIndex = 1;
			this.lblPlayer2.Text = "Player 2:";
			// 
			// cboPlayer4
			// 
			this.cboPlayer4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer4.FormattingEnabled = true;
			this.cboPlayer4.Location = new System.Drawing.Point(90, 171);
			this.cboPlayer4.Name = "cboPlayer4";
			this.cboPlayer4.Size = new System.Drawing.Size(183, 21);
			this.cboPlayer4.TabIndex = 8;
			this.cboPlayer4.SelectedIndexChanged += new System.EventHandler(this.cboPlayerController_SelectedIndexChanged);
			// 
			// cboPlayer3
			// 
			this.cboPlayer3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer3.FormattingEnabled = true;
			this.cboPlayer3.Location = new System.Drawing.Point(90, 144);
			this.cboPlayer3.Name = "cboPlayer3";
			this.cboPlayer3.Size = new System.Drawing.Size(183, 21);
			this.cboPlayer3.TabIndex = 7;
			this.cboPlayer3.SelectedIndexChanged += new System.EventHandler(this.cboPlayerController_SelectedIndexChanged);
			// 
			// cboPlayer1
			// 
			this.cboPlayer1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer1.FormattingEnabled = true;
			this.cboPlayer1.Location = new System.Drawing.Point(90, 40);
			this.cboPlayer1.Name = "cboPlayer1";
			this.cboPlayer1.Size = new System.Drawing.Size(183, 21);
			this.cboPlayer1.TabIndex = 4;
			this.cboPlayer1.SelectedIndexChanged += new System.EventHandler(this.cboPlayerController_SelectedIndexChanged);
			// 
			// lblPlayer4
			// 
			this.lblPlayer4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPlayer4.AutoSize = true;
			this.lblPlayer4.Location = new System.Drawing.Point(3, 175);
			this.lblPlayer4.Name = "lblPlayer4";
			this.lblPlayer4.Size = new System.Drawing.Size(48, 13);
			this.lblPlayer4.TabIndex = 3;
			this.lblPlayer4.Text = "Player 4:";
			// 
			// cboPlayer2
			// 
			this.cboPlayer2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer2.FormattingEnabled = true;
			this.cboPlayer2.Location = new System.Drawing.Point(90, 67);
			this.cboPlayer2.Name = "cboPlayer2";
			this.cboPlayer2.Size = new System.Drawing.Size(183, 21);
			this.cboPlayer2.TabIndex = 6;
			this.cboPlayer2.SelectedIndexChanged += new System.EventHandler(this.cboPlayerController_SelectedIndexChanged);
			// 
			// lblPlayer3
			// 
			this.lblPlayer3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPlayer3.AutoSize = true;
			this.lblPlayer3.Location = new System.Drawing.Point(3, 148);
			this.lblPlayer3.Name = "lblPlayer3";
			this.lblPlayer3.Size = new System.Drawing.Size(48, 13);
			this.lblPlayer3.TabIndex = 2;
			this.lblPlayer3.Text = "Player 3:";
			// 
			// btnSetupP1
			// 
			this.btnSetupP1.Location = new System.Drawing.Point(279, 40);
			this.btnSetupP1.Name = "btnSetupP1";
			this.btnSetupP1.Size = new System.Drawing.Size(56, 21);
			this.btnSetupP1.TabIndex = 9;
			this.btnSetupP1.Text = "Setup";
			this.btnSetupP1.UseVisualStyleBackColor = true;
			this.btnSetupP1.Click += new System.EventHandler(this.btnSetup_Click);
			// 
			// btnSetupP2
			// 
			this.btnSetupP2.Location = new System.Drawing.Point(279, 67);
			this.btnSetupP2.Name = "btnSetupP2";
			this.btnSetupP2.Size = new System.Drawing.Size(56, 21);
			this.btnSetupP2.TabIndex = 10;
			this.btnSetupP2.Text = "Setup";
			this.btnSetupP2.UseVisualStyleBackColor = true;
			this.btnSetupP2.Click += new System.EventHandler(this.btnSetup_Click);
			// 
			// lblNesType
			// 
			this.lblNesType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNesType.AutoSize = true;
			this.lblNesType.Location = new System.Drawing.Point(3, 7);
			this.lblNesType.Name = "lblNesType";
			this.lblNesType.Size = new System.Drawing.Size(75, 13);
			this.lblNesType.TabIndex = 13;
			this.lblNesType.Text = "Console Type:";
			// 
			// cboConsoleType
			// 
			this.cboConsoleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboConsoleType.FormattingEnabled = true;
			this.cboConsoleType.Items.AddRange(new object[] {
            "NES",
            "Famicom"});
			this.cboConsoleType.Location = new System.Drawing.Point(90, 3);
			this.cboConsoleType.Name = "cboConsoleType";
			this.cboConsoleType.Size = new System.Drawing.Size(109, 21);
			this.cboConsoleType.TabIndex = 14;
			this.cboConsoleType.SelectedIndexChanged += new System.EventHandler(this.cboNesType_SelectedIndexChanged);
			// 
			// lblExpansionPort
			// 
			this.lblExpansionPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblExpansionPort.AutoSize = true;
			this.lblExpansionPort.Location = new System.Drawing.Point(3, 121);
			this.lblExpansionPort.Name = "lblExpansionPort";
			this.lblExpansionPort.Size = new System.Drawing.Size(81, 13);
			this.lblExpansionPort.TabIndex = 16;
			this.lblExpansionPort.Text = "Expansion Port:";
			// 
			// cboExpansionPort
			// 
			this.cboExpansionPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboExpansionPort.FormattingEnabled = true;
			this.cboExpansionPort.Location = new System.Drawing.Point(90, 117);
			this.cboExpansionPort.Name = "cboExpansionPort";
			this.cboExpansionPort.Size = new System.Drawing.Size(183, 21);
			this.cboExpansionPort.TabIndex = 17;
			this.cboExpansionPort.SelectedIndexChanged += new System.EventHandler(this.cboExpansionPort_SelectedIndexChanged);
			// 
			// chkFourScore
			// 
			this.chkFourScore.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkFourScore.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkFourScore, 2);
			this.chkFourScore.Location = new System.Drawing.Point(3, 94);
			this.chkFourScore.Name = "chkFourScore";
			this.chkFourScore.Size = new System.Drawing.Size(151, 17);
			this.chkFourScore.TabIndex = 15;
			this.chkFourScore.Text = "Use Four Score accessory";
			this.chkFourScore.UseVisualStyleBackColor = true;
			this.chkFourScore.CheckedChanged += new System.EventHandler(this.chkFourScore_CheckedChanged);
			// 
			// tpgEmulatorKeys
			// 
			this.tpgEmulatorKeys.Location = new System.Drawing.Point(4, 22);
			this.tpgEmulatorKeys.Name = "tpgEmulatorKeys";
			this.tpgEmulatorKeys.Size = new System.Drawing.Size(340, 227);
			this.tpgEmulatorKeys.TabIndex = 4;
			this.tpgEmulatorKeys.Text = "Emulator Keys";
			this.tpgEmulatorKeys.UseVisualStyleBackColor = true;
			// 
			// frmInputConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(348, 253);
			this.Controls.Add(this.tabMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmInputConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Input Settings";
			this.Controls.SetChildIndex(this.tabMain, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tabMain.ResumeLayout(false);
			this.tpgControllers.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tpgEmulatorKeys;
		private System.Windows.Forms.TabPage tpgControllers;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblPlayer1;
		private System.Windows.Forms.Label lblPlayer2;
		private System.Windows.Forms.Label lblPlayer3;
		private System.Windows.Forms.Label lblPlayer4;
		private System.Windows.Forms.ComboBox cboPlayer1;
		private System.Windows.Forms.ComboBox cboPlayer2;
		private System.Windows.Forms.Button btnSetupP1;
		private System.Windows.Forms.Button btnSetupP2;
		private System.Windows.Forms.Label lblNesType;
		private System.Windows.Forms.ComboBox cboConsoleType;
		private System.Windows.Forms.Label lblExpansionPort;
		private System.Windows.Forms.ComboBox cboExpansionPort;
		private System.Windows.Forms.CheckBox chkFourScore;
		private System.Windows.Forms.Button btnSetupP3;
		private System.Windows.Forms.Button btnSetupP4;
		private System.Windows.Forms.ComboBox cboPlayer3;
		private System.Windows.Forms.ComboBox cboPlayer4;
	}
}