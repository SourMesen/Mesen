namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlBreakpoints
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
			BreakpointManager.BreakpointsChanged -= BreakpointManager_OnBreakpointChanged;
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
			this.mnuBreakpoints = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.lstBreakpoints = new Mesen.GUI.Controls.MyListView();
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLastColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mnuBreakpoints.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuBreakpoints
			// 
			this.mnuBreakpoints.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddBreakpoint,
            this.mnuEditBreakpoint,
            this.mnuRemoveBreakpoint,
            this.toolStripMenuItem1,
            this.mnuGoToLocation});
			this.mnuBreakpoints.Name = "mnuBreakpoints";
			this.mnuBreakpoints.Size = new System.Drawing.Size(153, 120);
			// 
			// mnuAddBreakpoint
			// 
			this.mnuAddBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Add;
			this.mnuAddBreakpoint.Name = "mnuAddBreakpoint";
			this.mnuAddBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.mnuAddBreakpoint.Size = new System.Drawing.Size(152, 22);
			this.mnuAddBreakpoint.Text = "Add...";
			this.mnuAddBreakpoint.Click += new System.EventHandler(this.mnuAddBreakpoint_Click);
			// 
			// mnuEditBreakpoint
			// 
			this.mnuEditBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditBreakpoint.Name = "mnuEditBreakpoint";
			this.mnuEditBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.mnuEditBreakpoint.Size = new System.Drawing.Size(152, 22);
			this.mnuEditBreakpoint.Text = "Edit";
			this.mnuEditBreakpoint.Click += new System.EventHandler(this.mnuEditBreakpoint_Click);
			// 
			// mnuRemoveBreakpoint
			// 
			this.mnuRemoveBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.mnuRemoveBreakpoint.Name = "mnuRemoveBreakpoint";
			this.mnuRemoveBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuRemoveBreakpoint.Size = new System.Drawing.Size(152, 22);
			this.mnuRemoveBreakpoint.Text = "Remove";
			this.mnuRemoveBreakpoint.Click += new System.EventHandler(this.mnuRemoveBreakpoint_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// mnuGoToLocation
			// 
			this.mnuGoToLocation.Name = "mnuGoToLocation";
			this.mnuGoToLocation.Size = new System.Drawing.Size(152, 22);
			this.mnuGoToLocation.Text = "Go to location";
			this.mnuGoToLocation.Click += new System.EventHandler(this.mnuGoToLocation_Click);
			// 
			// lstBreakpoints
			// 
			this.lstBreakpoints.CheckBoxes = true;
			this.lstBreakpoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.colLastColumn});
			this.lstBreakpoints.ContextMenuStrip = this.mnuBreakpoints;
			this.lstBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstBreakpoints.FullRowSelect = true;
			this.lstBreakpoints.GridLines = true;
			this.lstBreakpoints.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstBreakpoints.Location = new System.Drawing.Point(0, 0);
			this.lstBreakpoints.Name = "lstBreakpoints";
			this.lstBreakpoints.Size = new System.Drawing.Size(393, 101);
			this.lstBreakpoints.TabIndex = 7;
			this.lstBreakpoints.UseCompatibleStateImageBehavior = false;
			this.lstBreakpoints.View = System.Windows.Forms.View.Details;
			this.lstBreakpoints.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.lstBreakpoints_ColumnWidthChanged);
			this.lstBreakpoints.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lstBreakpoints_ColumnWidthChanging);
			this.lstBreakpoints.SelectedIndexChanged += new System.EventHandler(this.lstBreakpoints_SelectedIndexChanged);
			this.lstBreakpoints.DoubleClick += new System.EventHandler(this.lstBreakpoints_DoubleClick);
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "";
			this.columnHeader3.Width = 21;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Type";
			this.columnHeader1.Width = 70;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Address";
			this.columnHeader2.Width = 108;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Condition";
			this.columnHeader4.Width = 142;
			// 
			// colLastColumn
			// 
			this.colLastColumn.Text = "";
			this.colLastColumn.Width = 29;
			// 
			// ctrlBreakpoints
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstBreakpoints);
			this.Name = "ctrlBreakpoints";
			this.Size = new System.Drawing.Size(393, 101);
			this.mnuBreakpoints.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Mesen.GUI.Controls.MyListView lstBreakpoints;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader colLastColumn;
		private System.Windows.Forms.ContextMenuStrip mnuBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem mnuAddBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
		private System.Windows.Forms.ToolStripMenuItem mnuEditBreakpoint;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
	}
}
