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
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuCheats = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.tabCheatFindTool = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tabMain.SuspendLayout();
			this.tabCheats.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.contextMenuCheats.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabCheats);
			this.tabMain.Controls.Add(this.tabCheatFindTool);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Margin = new System.Windows.Forms.Padding(0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(443, 225);
			this.tabMain.TabIndex = 0;
			// 
			// tabCheats
			// 
			this.tabCheats.Controls.Add(this.tableLayoutPanel1);
			this.tabCheats.Location = new System.Drawing.Point(4, 22);
			this.tabCheats.Name = "tabCheats";
			this.tabCheats.Padding = new System.Windows.Forms.Padding(3);
			this.tabCheats.Size = new System.Drawing.Size(435, 199);
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
			this.tableLayoutPanel1.Controls.Add(this.lstCheats, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 193);
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
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.tableLayoutPanel1.SetColumnSpan(this.lstCheats, 2);
			this.lstCheats.ContextMenuStrip = this.contextMenuCheats;
			this.lstCheats.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstCheats.FullRowSelect = true;
			this.lstCheats.GridLines = true;
			this.lstCheats.Location = new System.Drawing.Point(3, 26);
			this.lstCheats.Name = "lstCheats";
			this.lstCheats.Size = new System.Drawing.Size(423, 164);
			this.lstCheats.TabIndex = 1;
			this.lstCheats.UseCompatibleStateImageBehavior = false;
			this.lstCheats.View = System.Windows.Forms.View.Details;
			this.lstCheats.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstCheats_ItemChecked);
			this.lstCheats.DoubleClick += new System.EventHandler(this.lstCheats_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Game";
			this.columnHeader1.Width = 98;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Cheat Name";
			this.columnHeader2.Width = 110;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Code";
			this.columnHeader3.Width = 142;
			// 
			// contextMenuCheats
			// 
			this.contextMenuCheats.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddCheat,
            this.mnuDeleteCheat});
			this.contextMenuCheats.Name = "contextMenuCheats";
			this.contextMenuCheats.Size = new System.Drawing.Size(160, 48);
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
			this.mnuDeleteCheat.Name = "mnuDeleteCheat";
			this.mnuDeleteCheat.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDeleteCheat.Size = new System.Drawing.Size(159, 22);
			this.mnuDeleteCheat.Text = "Delete";
			// 
			// tabCheatFindTool
			// 
			this.tabCheatFindTool.Location = new System.Drawing.Point(4, 22);
			this.tabCheatFindTool.Name = "tabCheatFindTool";
			this.tabCheatFindTool.Padding = new System.Windows.Forms.Padding(3);
			this.tabCheatFindTool.Size = new System.Drawing.Size(435, 199);
			this.tabCheatFindTool.TabIndex = 1;
			this.tabCheatFindTool.Text = "Cheat Finder";
			this.tabCheatFindTool.UseVisualStyleBackColor = true;
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(443, 225);
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
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.tabMain.ResumeLayout(false);
			this.tabCheats.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.contextMenuCheats.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabCheats;
		private System.Windows.Forms.TabPage tabCheatFindTool;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkCurrentGameOnly;
		private MyListView lstCheats;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ContextMenuStrip contextMenuCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuAddCheat;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteCheat;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
	}
}