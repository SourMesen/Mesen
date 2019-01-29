namespace Mesen.GUI.Debugger
{
	partial class ctrlWatch
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
			WatchManager.WatchChanged -= WatchManager_WatchChanged;
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
			this.lstWatch = new Mesen.GUI.Controls.DoubleBufferedListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuWatch = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuRemoveWatch = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditInMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuViewInDisassembly = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHexDisplay = new System.Windows.Forms.ToolStripMenuItem();
			this.txtEdit = new System.Windows.Forms.TextBox();
			this.contextMenuWatch.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstWatch
			// 
			this.lstWatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
			this.lstWatch.ContextMenuStrip = this.contextMenuWatch;
			this.lstWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstWatch.FullRowSelect = true;
			this.lstWatch.GridLines = true;
			this.lstWatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstWatch.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.lstWatch.Location = new System.Drawing.Point(0, 0);
			this.lstWatch.Name = "lstWatch";
			this.lstWatch.Size = new System.Drawing.Size(378, 112);
			this.lstWatch.TabIndex = 6;
			this.lstWatch.UseCompatibleStateImageBehavior = false;
			this.lstWatch.View = System.Windows.Forms.View.Details;
			this.lstWatch.SelectedIndexChanged += new System.EventHandler(this.lstWatch_SelectedIndexChanged);
			this.lstWatch.Click += new System.EventHandler(this.lstWatch_Click);
			this.lstWatch.DoubleClick += new System.EventHandler(this.lstWatch_DoubleClick);
			this.lstWatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstWatch_KeyPress);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 180;
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 110;
			// 
			// contextMenuWatch
			// 
			this.contextMenuWatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemoveWatch,
            this.toolStripMenuItem1,
            this.mnuEditInMemoryViewer,
            this.mnuViewInDisassembly,
            this.toolStripMenuItem2,
            this.mnuHexDisplay});
			this.contextMenuWatch.Name = "contextMenuWatch";
			this.contextMenuWatch.Size = new System.Drawing.Size(194, 104);
			this.contextMenuWatch.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuWatch_Opening);
			// 
			// mnuRemoveWatch
			// 
			this.mnuRemoveWatch.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.mnuRemoveWatch.Name = "mnuRemoveWatch";
			this.mnuRemoveWatch.Size = new System.Drawing.Size(193, 22);
			this.mnuRemoveWatch.Text = "Remove";
			this.mnuRemoveWatch.Click += new System.EventHandler(this.mnuRemoveWatch_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuEditInMemoryViewer
			// 
			this.mnuEditInMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuEditInMemoryViewer.Name = "mnuEditInMemoryViewer";
			this.mnuEditInMemoryViewer.Size = new System.Drawing.Size(193, 22);
			this.mnuEditInMemoryViewer.Text = "Edit in Memory Viewer";
			this.mnuEditInMemoryViewer.Click += new System.EventHandler(this.mnuEditInMemoryViewer_Click);
			// 
			// mnuViewInDisassembly
			// 
			this.mnuViewInDisassembly.Image = global::Mesen.GUI.Properties.Resources.Bug;
			this.mnuViewInDisassembly.Name = "mnuViewInDisassembly";
			this.mnuViewInDisassembly.Size = new System.Drawing.Size(193, 22);
			this.mnuViewInDisassembly.Text = "View in disassembly";
			this.mnuViewInDisassembly.Click += new System.EventHandler(this.mnuViewInDisassembly_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuHexDisplay
			// 
			this.mnuHexDisplay.Checked = true;
			this.mnuHexDisplay.CheckOnClick = true;
			this.mnuHexDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuHexDisplay.Name = "mnuHexDisplay";
			this.mnuHexDisplay.Size = new System.Drawing.Size(193, 22);
			this.mnuHexDisplay.Text = "Hexadecimal Display";
			this.mnuHexDisplay.Click += new System.EventHandler(this.mnuHexDisplay_Click);
			// 
			// txtEdit
			// 
			this.txtEdit.AcceptsReturn = true;
			this.txtEdit.Location = new System.Drawing.Point(3, 24);
			this.txtEdit.Name = "txtEdit";
			this.txtEdit.Size = new System.Drawing.Size(177, 20);
			this.txtEdit.TabIndex = 7;
			this.txtEdit.Visible = false;
			this.txtEdit.Leave += new System.EventHandler(this.txtEdit_Leave);
			// 
			// ctrlWatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtEdit);
			this.Controls.Add(this.lstWatch);
			this.Name = "ctrlWatch";
			this.Size = new System.Drawing.Size(378, 112);
			this.contextMenuWatch.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Mesen.GUI.Controls.DoubleBufferedListView lstWatch;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.ContextMenuStrip contextMenuWatch;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveWatch;
		private System.Windows.Forms.ToolStripMenuItem mnuHexDisplay;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuEditInMemoryViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuViewInDisassembly;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.TextBox txtEdit;
	}
}
