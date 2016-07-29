using Mesen.GUI.Controls;
namespace Mesen.GUI.Forms.Cheats
{
	partial class frmCheatList
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
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabCheats = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkCurrentGameOnly = new System.Windows.Forms.CheckBox();
			this.lstCheats = new Mesen.GUI.Controls.MyListView();
			this.colGameName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCheatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuCheats = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAddCheat = new System.Windows.Forms.Button();
			this.btnDeleteCheat = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tabMain.SuspendLayout();
			this.tabCheats.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.contextMenuCheats.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 226);
			this.baseConfigPanel.Size = new System.Drawing.Size(443, 29);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabCheats);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Margin = new System.Windows.Forms.Padding(0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(443, 226);
			this.tabMain.TabIndex = 0;
			// 
			// tabCheats
			// 
			this.tabCheats.Controls.Add(this.tableLayoutPanel1);
			this.tabCheats.Location = new System.Drawing.Point(4, 22);
			this.tabCheats.Name = "tabCheats";
			this.tabCheats.Padding = new System.Windows.Forms.Padding(3);
			this.tabCheats.Size = new System.Drawing.Size(435, 200);
			this.tabCheats.TabIndex = 0;
			this.tabCheats.Text = "Cheats";
			this.tabCheats.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.chkCurrentGameOnly, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lstCheats, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 194);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkCurrentGameOnly
			// 
			this.chkCurrentGameOnly.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkCurrentGameOnly, 2);
			this.chkCurrentGameOnly.Location = new System.Drawing.Point(3, 3);
			this.chkCurrentGameOnly.Name = "chkCurrentGameOnly";
			this.chkCurrentGameOnly.Size = new System.Drawing.Size(208, 17);
			this.chkCurrentGameOnly.TabIndex = 0;
			this.chkCurrentGameOnly.Text = "Only show cheats for the current game";
			this.chkCurrentGameOnly.UseVisualStyleBackColor = true;
			this.chkCurrentGameOnly.CheckedChanged += new System.EventHandler(this.chkCurrentGameOnly_CheckedChanged);
			// 
			// lstCheats
			// 
			this.lstCheats.CheckBoxes = true;
			this.lstCheats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGameName,
            this.colCheatName,
            this.colCode});
			this.tableLayoutPanel1.SetColumnSpan(this.lstCheats, 2);
			this.lstCheats.ContextMenuStrip = this.contextMenuCheats;
			this.lstCheats.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstCheats.FullRowSelect = true;
			this.lstCheats.Location = new System.Drawing.Point(3, 55);
			this.lstCheats.Name = "lstCheats";
			this.lstCheats.Size = new System.Drawing.Size(423, 136);
			this.lstCheats.TabIndex = 1;
			this.lstCheats.UseCompatibleStateImageBehavior = false;
			this.lstCheats.View = System.Windows.Forms.View.Details;
			this.lstCheats.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstCheats_ItemChecked);
			this.lstCheats.SelectedIndexChanged += new System.EventHandler(this.lstCheats_SelectedIndexChanged);
			this.lstCheats.DoubleClick += new System.EventHandler(this.lstCheats_DoubleClick);
			// 
			// colGameName
			// 
			this.colGameName.Name = "colGameName";
			this.colGameName.Text = "Game";
			this.colGameName.Width = 98;
			// 
			// colCheatName
			// 
			this.colCheatName.Name = "colCheatName";
			this.colCheatName.Text = "Cheat Name";
			this.colCheatName.Width = 110;
			// 
			// colCode
			// 
			this.colCode.Name = "colCode";
			this.colCode.Text = "Code";
			this.colCode.Width = 142;
			// 
			// contextMenuCheats
			// 
			this.contextMenuCheats.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddCheat,
            this.mnuDeleteCheat});
			this.contextMenuCheats.Name = "contextMenuCheats";
			this.contextMenuCheats.Size = new System.Drawing.Size(160, 70);
			// 
			// mnuAddCheat
			// 
			this.mnuAddCheat.Name = "mnuAddCheat";
			this.mnuAddCheat.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.mnuAddCheat.Size = new System.Drawing.Size(159, 22);
			this.mnuAddCheat.Text = "Add cheat...";
			this.mnuAddCheat.Click += new System.EventHandler(this.mnuAddCheat_Click);
			// 
			// mnuDeleteCheat
			// 
			this.mnuDeleteCheat.Enabled = false;
			this.mnuDeleteCheat.Name = "mnuDeleteCheat";
			this.mnuDeleteCheat.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDeleteCheat.Size = new System.Drawing.Size(159, 22);
			this.mnuDeleteCheat.Text = "Delete";
			this.mnuDeleteCheat.Click += new System.EventHandler(this.mnuDeleteCheat_Click);
			// 
			// flowLayoutPanel2
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 2);
			this.flowLayoutPanel2.Controls.Add(this.btnAddCheat);
			this.flowLayoutPanel2.Controls.Add(this.btnDeleteCheat);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 23);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(429, 29);
			this.flowLayoutPanel2.TabIndex = 3;
			// 
			// btnAddCheat
			// 
			this.btnAddCheat.AutoSize = true;
			this.btnAddCheat.Location = new System.Drawing.Point(3, 3);
			this.btnAddCheat.Name = "btnAddCheat";
			this.btnAddCheat.Size = new System.Drawing.Size(75, 23);
			this.btnAddCheat.TabIndex = 2;
			this.btnAddCheat.Text = "Add Cheat";
			this.btnAddCheat.UseVisualStyleBackColor = true;
			this.btnAddCheat.Click += new System.EventHandler(this.mnuAddCheat_Click);
			// 
			// btnDeleteCheat
			// 
			this.btnDeleteCheat.AutoSize = true;
			this.btnDeleteCheat.Enabled = false;
			this.btnDeleteCheat.Location = new System.Drawing.Point(84, 3);
			this.btnDeleteCheat.Name = "btnDeleteCheat";
			this.btnDeleteCheat.Size = new System.Drawing.Size(129, 23);
			this.btnDeleteCheat.TabIndex = 3;
			this.btnDeleteCheat.Text = "Delete Selected Cheats";
			this.btnDeleteCheat.UseVisualStyleBackColor = true;
			this.btnDeleteCheat.Click += new System.EventHandler(this.btnDeleteCheat_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.tabMain, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(443, 226);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// frmCheatList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(443, 255);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Name = "frmCheatList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Cheats";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.tabMain.ResumeLayout(false);
			this.tabCheats.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.contextMenuCheats.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabCheats;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkCurrentGameOnly;
		private MyListView lstCheats;
		private System.Windows.Forms.ColumnHeader colGameName;
		private System.Windows.Forms.ColumnHeader colCheatName;
		private System.Windows.Forms.ColumnHeader colCode;
		private System.Windows.Forms.ContextMenuStrip contextMenuCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuAddCheat;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteCheat;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button btnAddCheat;
		private System.Windows.Forms.Button btnDeleteCheat;
	}
}