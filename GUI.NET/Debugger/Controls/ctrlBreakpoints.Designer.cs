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
			this.contextMenuBreakpoints = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.lstBreakpoints = new Mesen.GUI.Controls.MyListView();
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLastColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuBreakpoints.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuBreakpoints
			// 
			this.contextMenuBreakpoints.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddBreakpoint,
            this.mnuRemoveBreakpoint,
            this.mnuGoToLocation});
			this.contextMenuBreakpoints.Name = "contextMenuWatch";
			this.contextMenuBreakpoints.Size = new System.Drawing.Size(153, 92);
			this.contextMenuBreakpoints.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuBreakpoints_Opening);
			// 
			// mnuAddBreakpoint
			// 
			this.mnuAddBreakpoint.Name = "mnuAddBreakpoint";
			this.mnuAddBreakpoint.Size = new System.Drawing.Size(152, 22);
			this.mnuAddBreakpoint.Text = "Add...";
			this.mnuAddBreakpoint.Click += new System.EventHandler(this.mnuAddBreakpoint_Click);
			// 
			// mnuRemoveBreakpoint
			// 
			this.mnuRemoveBreakpoint.Name = "mnuRemoveBreakpoint";
			this.mnuRemoveBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.mnuRemoveBreakpoint.Size = new System.Drawing.Size(152, 22);
			this.mnuRemoveBreakpoint.Text = "Remove";
			this.mnuRemoveBreakpoint.Click += new System.EventHandler(this.mnuRemoveBreakpoint_Click);
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
			this.lstBreakpoints.ContextMenuStrip = this.contextMenuBreakpoints;
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
			this.columnHeader2.Width = 70;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Condition";
			this.columnHeader4.Width = 100;
			// 
			// colLastColumn
			// 
			this.colLastColumn.Text = "";
			// 
			// mnuGoToLocation
			// 
			this.mnuGoToLocation.Name = "mnuGoToLocation";
			this.mnuGoToLocation.Size = new System.Drawing.Size(152, 22);
			this.mnuGoToLocation.Text = "Go to location";
			this.mnuGoToLocation.Click += new System.EventHandler(this.mnuGoToLocation_Click);
			// 
			// ctrlBreakpoints
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstBreakpoints);
			this.Name = "ctrlBreakpoints";
			this.Size = new System.Drawing.Size(393, 101);
			this.contextMenuBreakpoints.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Mesen.GUI.Controls.MyListView lstBreakpoints;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader colLastColumn;
		private System.Windows.Forms.ContextMenuStrip contextMenuBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem mnuAddBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
	}
}
