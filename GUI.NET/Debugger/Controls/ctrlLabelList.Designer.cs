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
			this.lstLabels = new System.Windows.Forms.ListView();
			this.colFunctionLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFunctionAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMemoryAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
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
			this.lstLabels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstLabels.Location = new System.Drawing.Point(0, 0);
			this.lstLabels.MultiSelect = false;
			this.lstLabels.Name = "lstLabels";
			this.lstLabels.Size = new System.Drawing.Size(275, 112);
			this.lstLabels.TabIndex = 2;
			this.lstLabels.UseCompatibleStateImageBehavior = false;
			this.lstLabels.View = System.Windows.Forms.View.Details;
			this.lstLabels.DoubleClick += new System.EventHandler(this.lstLabels_DoubleClick);
			// 
			// colFunctionLabel
			// 
			this.colFunctionLabel.Text = "Label";
			this.colFunctionLabel.Width = 136;
			// 
			// colFunctionAddress
			// 
			this.colFunctionAddress.Text = "Address";
			this.colFunctionAddress.Width = 62;
			// 
			// colMemoryAddress
			// 
			this.colMemoryAddress.Text = "ROM Addr.";
			this.colMemoryAddress.Width = 71;
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAdd,
            this.mnuEdit,
            this.mnuDelete});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(153, 92);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.mnuActions_Opening);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Name = "mnuDelete";
			this.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuDelete.Size = new System.Drawing.Size(152, 22);
			this.mnuDelete.Text = "Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// mnuAdd
			// 
			this.mnuAdd.Name = "mnuAdd";
			this.mnuAdd.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.mnuAdd.Size = new System.Drawing.Size(152, 22);
			this.mnuAdd.Text = "Add";
			this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Name = "mnuEdit";
			this.mnuEdit.Size = new System.Drawing.Size(152, 22);
			this.mnuEdit.Text = "Edit";
			this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
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

		private System.Windows.Forms.ListView lstLabels;
		private System.Windows.Forms.ColumnHeader colFunctionLabel;
		private System.Windows.Forms.ColumnHeader colFunctionAddress;
		private System.Windows.Forms.ColumnHeader colMemoryAddress;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuAdd;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
	}
}
