namespace Mesen.GUI.Forms.Cheats
{
	partial class frmCheatImport
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
			this.grpImportOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblGame = new System.Windows.Forms.Label();
			this.txtGameName = new System.Windows.Forms.TextBox();
			this.btnBrowseGame = new System.Windows.Forms.Button();
			this.lblCheatFile = new System.Windows.Forms.Label();
			this.txtCheatFile = new System.Windows.Forms.TextBox();
			this.btnBrowseCheat = new System.Windows.Forms.Button();
			this.baseConfigPanel = new System.Windows.Forms.Panel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnImport = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpImportOptions.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.baseConfigPanel.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpImportOptions
			// 
			this.grpImportOptions.Controls.Add(this.tableLayoutPanel2);
			this.grpImportOptions.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpImportOptions.Location = new System.Drawing.Point(0, 0);
			this.grpImportOptions.Name = "grpImportOptions";
			this.grpImportOptions.Size = new System.Drawing.Size(371, 80);
			this.grpImportOptions.TabIndex = 1;
			this.grpImportOptions.TabStop = false;
			this.grpImportOptions.Text = "Import Options";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.lblGame, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtGameName, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.btnBrowseGame, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblCheatFile, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtCheatFile, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnBrowseCheat, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(365, 61);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// lblGame
			// 
			this.lblGame.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblGame.AutoSize = true;
			this.lblGame.Location = new System.Drawing.Point(3, 37);
			this.lblGame.Name = "lblGame";
			this.lblGame.Size = new System.Drawing.Size(38, 13);
			this.lblGame.TabIndex = 0;
			this.lblGame.Text = "Game:";
			// 
			// txtGameName
			// 
			this.txtGameName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtGameName.Location = new System.Drawing.Point(66, 32);
			this.txtGameName.Name = "txtGameName";
			this.txtGameName.ReadOnly = true;
			this.txtGameName.Size = new System.Drawing.Size(229, 20);
			this.txtGameName.TabIndex = 2;
			// 
			// btnBrowseGame
			// 
			this.btnBrowseGame.AutoSize = true;
			this.btnBrowseGame.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnBrowseGame.Location = new System.Drawing.Point(301, 32);
			this.btnBrowseGame.Name = "btnBrowseGame";
			this.btnBrowseGame.Size = new System.Drawing.Size(61, 23);
			this.btnBrowseGame.TabIndex = 4;
			this.btnBrowseGame.Text = "Browse...";
			this.btnBrowseGame.UseVisualStyleBackColor = true;
			this.btnBrowseGame.Click += new System.EventHandler(this.btnBrowseGame_Click);
			// 
			// lblCheatFile
			// 
			this.lblCheatFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCheatFile.AutoSize = true;
			this.lblCheatFile.Location = new System.Drawing.Point(3, 8);
			this.lblCheatFile.Name = "lblCheatFile";
			this.lblCheatFile.Size = new System.Drawing.Size(57, 13);
			this.lblCheatFile.TabIndex = 1;
			this.lblCheatFile.Text = "Cheat File:";
			// 
			// txtCheatFile
			// 
			this.txtCheatFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCheatFile.Location = new System.Drawing.Point(66, 3);
			this.txtCheatFile.Name = "txtCheatFile";
			this.txtCheatFile.ReadOnly = true;
			this.txtCheatFile.Size = new System.Drawing.Size(229, 20);
			this.txtCheatFile.TabIndex = 3;
			// 
			// btnBrowseCheat
			// 
			this.btnBrowseCheat.AutoSize = true;
			this.btnBrowseCheat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnBrowseCheat.Location = new System.Drawing.Point(301, 3);
			this.btnBrowseCheat.Name = "btnBrowseCheat";
			this.btnBrowseCheat.Size = new System.Drawing.Size(61, 23);
			this.btnBrowseCheat.TabIndex = 5;
			this.btnBrowseCheat.Text = "Browse...";
			this.btnBrowseCheat.UseVisualStyleBackColor = true;
			this.btnBrowseCheat.Click += new System.EventHandler(this.btnBrowseCheat_Click);
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.flowLayoutPanel1);
			this.baseConfigPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 81);
			this.baseConfigPanel.Name = "baseConfigPanel";
			this.baseConfigPanel.Size = new System.Drawing.Size(371, 29);
			this.baseConfigPanel.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnImport);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(206, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(165, 29);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// btnImport
			// 
			this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImport.Location = new System.Drawing.Point(3, 3);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 1;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(84, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmCheatImport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(371, 110);
			this.Controls.Add(this.baseConfigPanel);
			this.Controls.Add(this.grpImportOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmCheatImport";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import Cheats";
			this.grpImportOptions.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.baseConfigPanel.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpImportOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox txtCheatFile;
		private System.Windows.Forms.TextBox txtGameName;
		private System.Windows.Forms.Label lblGame;
		private System.Windows.Forms.Label lblCheatFile;
		private System.Windows.Forms.Button btnBrowseGame;
		private System.Windows.Forms.Button btnBrowseCheat;
		protected System.Windows.Forms.Panel baseConfigPanel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		protected System.Windows.Forms.Button btnImport;
		protected System.Windows.Forms.Button btnCancel;
	}
}