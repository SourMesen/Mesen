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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lstGameList = new System.Windows.Forms.ListView();
			this.colGameName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuGames = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuDeleteGameCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportGame = new System.Windows.Forms.ToolStripMenuItem();
			this.lstCheats = new Mesen.GUI.Controls.MyListView();
			this.colCheatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuCheats = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportSelectedCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.tsCheatActions = new Mesen.GUI.Controls.ctrlMesenToolStrip();
			this.btnAddCheat = new System.Windows.Forms.ToolStripButton();
			this.btnDelete = new System.Windows.Forms.ToolStripSplitButton();
			this.btnDeleteCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.btnDeleteGameCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.btnImport = new System.Windows.Forms.ToolStripSplitButton();
			this.btnImportCheatDB = new System.Windows.Forms.ToolStripMenuItem();
			this.btnImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.btnExport = new System.Windows.Forms.ToolStripSplitButton();
			this.btnExportAllCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.btnExportGame = new System.Windows.Forms.ToolStripMenuItem();
			this.btnExportSelectedCheats = new System.Windows.Forms.ToolStripMenuItem();
			this.tpgCheatFinder = new System.Windows.Forms.TabPage();
			this.ctrlCheatFinder = new Mesen.GUI.Forms.Cheats.ctrlCheatFinder();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.chkDisableCheats = new System.Windows.Forms.CheckBox();
			this.baseConfigPanel.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabCheats.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuGames.SuspendLayout();
			this.contextMenuCheats.SuspendLayout();
			this.tsCheatActions.SuspendLayout();
			this.tpgCheatFinder.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseConfigPanel
			// 
			this.baseConfigPanel.Controls.Add(this.chkDisableCheats);
			this.baseConfigPanel.Location = new System.Drawing.Point(0, 257);
			this.baseConfigPanel.Size = new System.Drawing.Size(616, 29);
			this.baseConfigPanel.Controls.SetChildIndex(this.chkDisableCheats, 0);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabCheats);
			this.tabMain.Controls.Add(this.tpgCheatFinder);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Margin = new System.Windows.Forms.Padding(0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(616, 257);
			this.tabMain.TabIndex = 0;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tabCheats
			// 
			this.tabCheats.Controls.Add(this.tableLayoutPanel1);
			this.tabCheats.Location = new System.Drawing.Point(4, 22);
			this.tabCheats.Name = "tabCheats";
			this.tabCheats.Padding = new System.Windows.Forms.Padding(3);
			this.tabCheats.Size = new System.Drawing.Size(608, 231);
			this.tabCheats.TabIndex = 0;
			this.tabCheats.Text = "Cheats";
			this.tabCheats.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tsCheatActions, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(602, 225);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 26);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lstGameList);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lstCheats);
			this.splitContainer1.Size = new System.Drawing.Size(596, 196);
			this.splitContainer1.SplitterDistance = 213;
			this.splitContainer1.TabIndex = 5;
			// 
			// lstGameList
			// 
			this.lstGameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGameName});
			this.lstGameList.ContextMenuStrip = this.contextMenuGames;
			this.lstGameList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstGameList.FullRowSelect = true;
			this.lstGameList.GridLines = true;
			this.lstGameList.HideSelection = false;
			this.lstGameList.Location = new System.Drawing.Point(0, 0);
			this.lstGameList.MultiSelect = false;
			this.lstGameList.Name = "lstGameList";
			this.lstGameList.Size = new System.Drawing.Size(213, 196);
			this.lstGameList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstGameList.TabIndex = 2;
			this.lstGameList.UseCompatibleStateImageBehavior = false;
			this.lstGameList.View = System.Windows.Forms.View.Details;
			this.lstGameList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstGameList_ColumnClick);
			this.lstGameList.SelectedIndexChanged += new System.EventHandler(this.lstGameList_SelectedIndexChanged);
			// 
			// colGameName
			// 
			this.colGameName.Name = "colGameName";
			this.colGameName.Text = "Game";
			this.colGameName.Width = 180;
			// 
			// contextMenuGames
			// 
			this.contextMenuGames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDeleteGameCheats,
            this.mnuExportGame});
			this.contextMenuGames.Name = "contextMenuCheats";
			this.contextMenuGames.Size = new System.Drawing.Size(132, 48);
			// 
			// mnuDeleteGameCheats
			// 
			this.mnuDeleteGameCheats.Enabled = false;
			this.mnuDeleteGameCheats.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.mnuDeleteGameCheats.Name = "mnuDeleteGameCheats";
			this.mnuDeleteGameCheats.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDeleteGameCheats.Size = new System.Drawing.Size(131, 22);
			this.mnuDeleteGameCheats.Text = "Delete";
			this.mnuDeleteGameCheats.Click += new System.EventHandler(this.btnDeleteGameCheats_Click);
			// 
			// mnuExportGame
			// 
			this.mnuExportGame.Enabled = false;
			this.mnuExportGame.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportGame.Name = "mnuExportGame";
			this.mnuExportGame.Size = new System.Drawing.Size(131, 22);
			this.mnuExportGame.Text = "Export";
			this.mnuExportGame.Click += new System.EventHandler(this.btnExportGame_Click);
			// 
			// lstCheats
			// 
			this.lstCheats.CheckBoxes = true;
			this.lstCheats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCheatName,
            this.colCode});
			this.lstCheats.ContextMenuStrip = this.contextMenuCheats;
			this.lstCheats.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstCheats.FullRowSelect = true;
			this.lstCheats.GridLines = true;
			this.lstCheats.HideSelection = false;
			this.lstCheats.Location = new System.Drawing.Point(0, 0);
			this.lstCheats.Name = "lstCheats";
			this.lstCheats.Size = new System.Drawing.Size(379, 196);
			this.lstCheats.TabIndex = 1;
			this.lstCheats.UseCompatibleStateImageBehavior = false;
			this.lstCheats.View = System.Windows.Forms.View.Details;
			this.lstCheats.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstCheats_ItemChecked);
			this.lstCheats.SelectedIndexChanged += new System.EventHandler(this.lstCheats_SelectedIndexChanged);
			this.lstCheats.DoubleClick += new System.EventHandler(this.lstCheats_DoubleClick);
			// 
			// colCheatName
			// 
			this.colCheatName.Name = "colCheatName";
			this.colCheatName.Text = "Cheat Name";
			this.colCheatName.Width = 250;
			// 
			// colCode
			// 
			this.colCode.Name = "colCode";
			this.colCode.Text = "Code";
			this.colCode.Width = 100;
			// 
			// contextMenuCheats
			// 
			this.contextMenuCheats.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddCheat,
            this.mnuDeleteCheat,
            this.mnuExportSelectedCheats});
			this.contextMenuCheats.Name = "contextMenuCheats";
			this.contextMenuCheats.Size = new System.Drawing.Size(160, 92);
			// 
			// mnuAddCheat
			// 
			this.mnuAddCheat.Image = global::Mesen.GUI.Properties.Resources.Add;
			this.mnuAddCheat.Name = "mnuAddCheat";
			this.mnuAddCheat.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.mnuAddCheat.Size = new System.Drawing.Size(159, 22);
			this.mnuAddCheat.Text = "Add cheat...";
			this.mnuAddCheat.Click += new System.EventHandler(this.mnuAddCheat_Click);
			// 
			// mnuDeleteCheat
			// 
			this.mnuDeleteCheat.Enabled = false;
			this.mnuDeleteCheat.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.mnuDeleteCheat.Name = "mnuDeleteCheat";
			this.mnuDeleteCheat.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDeleteCheat.Size = new System.Drawing.Size(159, 22);
			this.mnuDeleteCheat.Text = "Delete";
			this.mnuDeleteCheat.Click += new System.EventHandler(this.btnDeleteCheat_Click);
			// 
			// mnuExportSelectedCheats
			// 
			this.mnuExportSelectedCheats.Enabled = false;
			this.mnuExportSelectedCheats.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportSelectedCheats.Name = "mnuExportSelectedCheats";
			this.mnuExportSelectedCheats.Size = new System.Drawing.Size(159, 22);
			this.mnuExportSelectedCheats.Text = "Export";
			this.mnuExportSelectedCheats.Click += new System.EventHandler(this.btnExportSelectedCheats_Click);
			// 
			// tsCheatActions
			// 
			this.tsCheatActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddCheat,
            this.btnDelete,
            this.btnImport,
            this.btnExport});
			this.tsCheatActions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.tsCheatActions.Location = new System.Drawing.Point(0, 0);
			this.tsCheatActions.Name = "tsCheatActions";
			this.tsCheatActions.Size = new System.Drawing.Size(602, 23);
			this.tsCheatActions.TabIndex = 6;
			this.tsCheatActions.Text = "toolStrip1";
			// 
			// btnAddCheat
			// 
			this.btnAddCheat.Image = global::Mesen.GUI.Properties.Resources.Add;
			this.btnAddCheat.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddCheat.Name = "btnAddCheat";
			this.btnAddCheat.Size = new System.Drawing.Size(83, 20);
			this.btnAddCheat.Text = "Add Cheat";
			this.btnAddCheat.Click += new System.EventHandler(this.mnuAddCheat_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDeleteCheat,
            this.btnDeleteGameCheats});
			this.btnDelete.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(72, 20);
			this.btnDelete.Text = "Delete";
			this.btnDelete.ButtonClick += new System.EventHandler(this.btnDelete_ButtonClick);
			// 
			// btnDeleteCheat
			// 
			this.btnDeleteCheat.Enabled = false;
			this.btnDeleteCheat.Name = "btnDeleteCheat";
			this.btnDeleteCheat.Size = new System.Drawing.Size(256, 22);
			this.btnDeleteCheat.Text = "Delete selected cheats";
			this.btnDeleteCheat.Click += new System.EventHandler(this.btnDeleteCheat_Click);
			// 
			// btnDeleteGameCheats
			// 
			this.btnDeleteGameCheats.Name = "btnDeleteGameCheats";
			this.btnDeleteGameCheats.Size = new System.Drawing.Size(256, 22);
			this.btnDeleteGameCheats.Text = "Delete all cheats for selected game";
			this.btnDeleteGameCheats.Click += new System.EventHandler(this.btnDeleteGameCheats_Click);
			// 
			// btnImport
			// 
			this.btnImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportCheatDB,
            this.btnImportFromFile});
			this.btnImport.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 20);
			this.btnImport.Text = "Import";
			this.btnImport.ButtonClick += new System.EventHandler(this.btnImport_ButtonClick);
			// 
			// btnImportCheatDB
			// 
			this.btnImportCheatDB.Name = "btnImportCheatDB";
			this.btnImportCheatDB.Size = new System.Drawing.Size(188, 22);
			this.btnImportCheatDB.Text = "From Cheat Database";
			this.btnImportCheatDB.Click += new System.EventHandler(this.btnImportCheatDB_Click);
			// 
			// btnImportFromFile
			// 
			this.btnImportFromFile.Name = "btnImportFromFile";
			this.btnImportFromFile.Size = new System.Drawing.Size(188, 22);
			this.btnImportFromFile.Text = "From File (XML, CHT)";
			this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
			// 
			// btnExport
			// 
			this.btnExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExportAllCheats,
            this.btnExportGame,
            this.btnExportSelectedCheats});
			this.btnExport.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(72, 20);
			this.btnExport.Text = "Export";
			this.btnExport.ButtonClick += new System.EventHandler(this.btnExport_ButtonClick);
			// 
			// btnExportAllCheats
			// 
			this.btnExportAllCheats.Name = "btnExportAllCheats";
			this.btnExportAllCheats.Size = new System.Drawing.Size(157, 22);
			this.btnExportAllCheats.Text = "All Cheats";
			this.btnExportAllCheats.Click += new System.EventHandler(this.btnExportAllCheats_Click);
			// 
			// btnExportGame
			// 
			this.btnExportGame.Name = "btnExportGame";
			this.btnExportGame.Size = new System.Drawing.Size(157, 22);
			this.btnExportGame.Text = "Selected Game";
			this.btnExportGame.Click += new System.EventHandler(this.btnExportGame_Click);
			// 
			// btnExportSelectedCheats
			// 
			this.btnExportSelectedCheats.Enabled = false;
			this.btnExportSelectedCheats.Name = "btnExportSelectedCheats";
			this.btnExportSelectedCheats.Size = new System.Drawing.Size(157, 22);
			this.btnExportSelectedCheats.Text = "Selected Cheats";
			this.btnExportSelectedCheats.Click += new System.EventHandler(this.btnExportSelectedCheats_Click);
			// 
			// tpgCheatFinder
			// 
			this.tpgCheatFinder.Controls.Add(this.ctrlCheatFinder);
			this.tpgCheatFinder.Location = new System.Drawing.Point(4, 22);
			this.tpgCheatFinder.Name = "tpgCheatFinder";
			this.tpgCheatFinder.Padding = new System.Windows.Forms.Padding(3);
			this.tpgCheatFinder.Size = new System.Drawing.Size(608, 231);
			this.tpgCheatFinder.TabIndex = 1;
			this.tpgCheatFinder.Text = "Cheat Finder";
			this.tpgCheatFinder.UseVisualStyleBackColor = true;
			// 
			// ctrlCheatFinder
			// 
			this.ctrlCheatFinder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCheatFinder.Location = new System.Drawing.Point(3, 3);
			this.ctrlCheatFinder.Name = "ctrlCheatFinder";
			this.ctrlCheatFinder.Size = new System.Drawing.Size(602, 225);
			this.ctrlCheatFinder.TabIndex = 0;
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(616, 257);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// chkDisableCheats
			// 
			this.chkDisableCheats.AutoSize = true;
			this.chkDisableCheats.Location = new System.Drawing.Point(7, 7);
			this.chkDisableCheats.Name = "chkDisableCheats";
			this.chkDisableCheats.Size = new System.Drawing.Size(109, 17);
			this.chkDisableCheats.TabIndex = 3;
			this.chkDisableCheats.Text = "Disable all cheats";
			this.chkDisableCheats.UseVisualStyleBackColor = true;
			// 
			// frmCheatList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(616, 286);
			this.Controls.Add(this.tableLayoutPanel2);
			this.MinimumSize = new System.Drawing.Size(632, 324);
			this.Name = "frmCheatList";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Cheats";
			this.Controls.SetChildIndex(this.baseConfigPanel, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.baseConfigPanel.ResumeLayout(false);
			this.baseConfigPanel.PerformLayout();
			this.tabMain.ResumeLayout(false);
			this.tabCheats.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenuGames.ResumeLayout(false);
			this.contextMenuCheats.ResumeLayout(false);
			this.tsCheatActions.ResumeLayout(false);
			this.tsCheatActions.PerformLayout();
			this.tpgCheatFinder.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabCheats;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private MyListView lstCheats;
		private System.Windows.Forms.ColumnHeader colCheatName;
		private System.Windows.Forms.ColumnHeader colCode;
		private System.Windows.Forms.ContextMenuStrip contextMenuCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuAddCheat;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteCheat;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private Mesen.GUI.Controls.ctrlMesenToolStrip tsCheatActions;
		private System.Windows.Forms.ToolStripButton btnAddCheat;
		private System.Windows.Forms.CheckBox chkDisableCheats;
		private System.Windows.Forms.ListView lstGameList;
		private System.Windows.Forms.ColumnHeader colGameName;
		private System.Windows.Forms.ToolStripSplitButton btnDelete;
		private System.Windows.Forms.ToolStripMenuItem btnDeleteCheat;
		private System.Windows.Forms.ToolStripMenuItem btnDeleteGameCheats;
		private System.Windows.Forms.ContextMenuStrip contextMenuGames;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteGameCheats;
		private System.Windows.Forms.ToolStripSplitButton btnExport;
		private System.Windows.Forms.ToolStripMenuItem btnExportAllCheats;
		private System.Windows.Forms.ToolStripMenuItem btnExportGame;
		private System.Windows.Forms.ToolStripMenuItem btnExportSelectedCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuExportSelectedCheats;
		private System.Windows.Forms.ToolStripMenuItem mnuExportGame;
		private System.Windows.Forms.ToolStripSplitButton btnImport;
		private System.Windows.Forms.ToolStripMenuItem btnImportCheatDB;
		private System.Windows.Forms.ToolStripMenuItem btnImportFromFile;
		private System.Windows.Forms.TabPage tpgCheatFinder;
		private ctrlCheatFinder ctrlCheatFinder;
	}
}