namespace Mesen.GUI.Forms.Config
{
	partial class frmVsGameConfig
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
			this.cboGame = new System.Windows.Forms.ComboBox();
			this.lblGame = new System.Windows.Forms.Label();
			this.lblPpuModel = new System.Windows.Forms.Label();
			this.cboPpuModel = new System.Windows.Forms.ComboBox();
			this.grpDipSwitches = new System.Windows.Forms.GroupBox();
			this.tlpMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 295);
			this.baseConfigPanel.Size = new System.Drawing.Size(305, 29);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.cboGame, 1, 0);
			this.tlpMain.Controls.Add(this.lblGame, 0, 0);
			this.tlpMain.Controls.Add(this.lblPpuModel, 0, 1);
			this.tlpMain.Controls.Add(this.cboPpuModel, 1, 1);
			this.tlpMain.Controls.Add(this.grpDipSwitches, 0, 2);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 3;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(305, 324);
			this.tlpMain.TabIndex = 0;
			// 
			// cboGame
			// 
			this.cboGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGame.FormattingEnabled = true;
			this.cboGame.Location = new System.Drawing.Point(73, 3);
			this.cboGame.Name = "cboGame";
			this.cboGame.Size = new System.Drawing.Size(194, 21);
			this.cboGame.TabIndex = 5;
			this.cboGame.SelectedIndexChanged += new System.EventHandler(this.cboGame_SelectedIndexChanged);
			// 
			// lblGame
			// 
			this.lblGame.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblGame.AutoSize = true;
			this.lblGame.Location = new System.Drawing.Point(3, 7);
			this.lblGame.Name = "lblGame";
			this.lblGame.Size = new System.Drawing.Size(38, 13);
			this.lblGame.TabIndex = 4;
			this.lblGame.Text = "Game:";
			// 
			// lblPpuModel
			// 
			this.lblPpuModel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPpuModel.AutoSize = true;
			this.lblPpuModel.Location = new System.Drawing.Point(3, 34);
			this.lblPpuModel.Name = "lblPpuModel";
			this.lblPpuModel.Size = new System.Drawing.Size(64, 13);
			this.lblPpuModel.TabIndex = 1;
			this.lblPpuModel.Text = "PPU Model:";
			// 
			// cboPpuModel
			// 
			this.cboPpuModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPpuModel.FormattingEnabled = true;
			this.cboPpuModel.Location = new System.Drawing.Point(73, 30);
			this.cboPpuModel.Name = "cboPpuModel";
			this.cboPpuModel.Size = new System.Drawing.Size(194, 21);
			this.cboPpuModel.TabIndex = 2;
			// 
			// grpDipSwitches
			// 
			this.tlpMain.SetColumnSpan(this.grpDipSwitches, 2);
			this.grpDipSwitches.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpDipSwitches.Location = new System.Drawing.Point(3, 57);
			this.grpDipSwitches.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
			this.grpDipSwitches.Name = "grpDipSwitches";
			this.grpDipSwitches.Size = new System.Drawing.Size(299, 237);
			this.grpDipSwitches.TabIndex = 3;
			this.grpDipSwitches.TabStop = false;
			this.grpDipSwitches.Text = "DIP Switches";
			// 
			// frmVsGameConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(305, 324);
			this.Controls.Add(this.tlpMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmVsGameConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Game Configuration";
			this.Controls.SetChildIndex(this.tlpMain, 0);
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.Label lblPpuModel;
		private System.Windows.Forms.ComboBox cboPpuModel;
		private System.Windows.Forms.GroupBox grpDipSwitches;
		private System.Windows.Forms.ComboBox cboGame;
		private System.Windows.Forms.Label lblGame;
	}
}