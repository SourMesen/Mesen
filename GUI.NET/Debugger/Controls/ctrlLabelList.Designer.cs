namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlLabelList
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.lstLabels = new Mesen.GUI.Controls.DoubleBufferedListView();
			this.colFunctionLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFunctionAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMemoryAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAdd,
            this.mnuEdit,
            this.mnuDelete,
            this.toolStripMenuItem1,
            this.mnuFindOccurrences});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(167, 98);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.mnuActions_Opening);
			// 
			// mnuAdd
			// 
			this.mnuAdd.Name = "mnuAdd";
			this.mnuAdd.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.mnuAdd.Size = new System.Drawing.Size(166, 22);
			this.mnuAdd.Text = "Add";
			this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Name = "mnuEdit";
			this.mnuEdit.Size = new System.Drawing.Size(166, 22);
			this.mnuEdit.Text = "Edit";
			this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Name = "mnuDelete";
			this.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDelete.Size = new System.Drawing.Size(166, 22);
			this.mnuDelete.Text = "Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuFindOccurrences
			// 
			this.mnuFindOccurrences.Name = "mnuFindOccurrences";
			this.mnuFindOccurrences.Size = new System.Drawing.Size(166, 22);
			this.mnuFindOccurrences.Text = "Find Occurrences";
			this.mnuFindOccurrences.Click += new System.EventHandler(this.mnuFindOccurrences_Click);
			// 
			// lstLabels
			// 
			this.lstLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFunctionLabel,
            this.colFunctionAddress,
            this.colMemoryAddress});
			this.lstLabels.ContextMenuStrip = this.contextMenu;
			this.lstLabels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstLabels.FullRowSelect = true;
			this.lstLabels.GridLines = true;
			this.lstLabels.HideSelection = false;
			this.lstLabels.Location = new System.Drawing.Point(0, 0);
			this.lstLabels.Name = "lstLabels";
			this.lstLabels.Size = new System.Drawing.Size(275, 112);
			this.lstLabels.TabIndex = 2;
			this.lstLabels.UseCompatibleStateImageBehavior = false;
			this.lstLabels.View = System.Windows.Forms.View.Details;
			this.lstLabels.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstLabels_ColumnClick);
			this.lstLabels.DoubleClick += new System.EventHandler(this.lstLabels_DoubleClick);
			// 
			// colFunctionLabel
			// 
			this.colFunctionLabel.Text = "Label";
			this.colFunctionLabel.Width = 136;
			// 
			// colFunctionAddress
			// 
			this.colFunctionAddress.Text = "Cpu Addr";
			this.colFunctionAddress.Width = 57;
			// 
			// colMemoryAddress
			// 
			this.colMemoryAddress.Text = "ROM Addr.";
			this.colMemoryAddress.Width = 84;
			// 
			// ctrlLabelList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstLabels);
			this.Name = "ctrlLabelList";
			this.Size = new System.Drawing.Size(275, 112);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Mesen.GUI.Controls.DoubleBufferedListView lstLabels;
		private System.Windows.Forms.ColumnHeader colFunctionLabel;
		private System.Windows.Forms.ColumnHeader colFunctionAddress;
		private System.Windows.Forms.ColumnHeader colMemoryAddress;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuAdd;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuFindOccurrences;
	}
}
