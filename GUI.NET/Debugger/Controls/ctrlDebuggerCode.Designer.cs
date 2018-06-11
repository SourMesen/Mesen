namespace Mesen.GUI.Debugger
{
	partial class ctrlDebuggerCode
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
			this.ctrlCodeViewer = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
			this.contextMenuMargin = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuEditBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.ctrlFindOccurrences = new Mesen.GUI.Debugger.Controls.ctrlFindOccurrences();
			this.contextMenuMargin.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctrlCodeViewer
			// 
			this.ctrlCodeViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlCodeViewer.CodeHighlightingEnabled = true;
			this.ctrlCodeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCodeViewer.HideSelection = false;
			this.ctrlCodeViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlCodeViewer.Name = "ctrlCodeViewer";
			this.ctrlCodeViewer.ShowCompactPrgAddresses = false;
			this.ctrlCodeViewer.ShowContentNotes = false;
			this.ctrlCodeViewer.ShowLineNumberNotes = false;
			this.ctrlCodeViewer.ShowMemoryValues = false;
			this.ctrlCodeViewer.ShowScrollbars = true;
			this.ctrlCodeViewer.ShowSingleContentLineNotes = true;
			this.ctrlCodeViewer.ShowSingleLineLineNumberNotes = false;
			this.ctrlCodeViewer.Size = new System.Drawing.Size(479, 150);
			this.ctrlCodeViewer.TabIndex = 1;
			this.ctrlCodeViewer.ScrollPositionChanged += new System.EventHandler(this.ctrlCodeViewer_ScrollPositionChanged);
			this.ctrlCodeViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseUp);
			this.ctrlCodeViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseMove);
			this.ctrlCodeViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDown);
			this.ctrlCodeViewer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDoubleClick);
			this.ctrlCodeViewer.MouseLeave += new System.EventHandler(this.ctrlCodeViewer_MouseLeave);
			this.ctrlCodeViewer.TextZoomChanged += new System.EventHandler(this.ctrlCodeViewer_TextZoomChanged);
			// 
			// contextMenuMargin
			// 
			this.contextMenuMargin.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditBreakpoint,
            this.mnuDisableBreakpoint,
            this.mnuRemoveBreakpoint});
			this.contextMenuMargin.Name = "contextMenuMargin";
			this.contextMenuMargin.Size = new System.Drawing.Size(178, 70);
			this.contextMenuMargin.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuMargin_Opening);
			// 
			// mnuEditBreakpoint
			// 
			this.mnuEditBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditBreakpoint.Name = "mnuEditBreakpoint";
			this.mnuEditBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuEditBreakpoint.Text = "Edit breakpoint";
			this.mnuEditBreakpoint.Click += new System.EventHandler(this.mnuEditBreakpoint_Click);
			// 
			// mnuDisableBreakpoint
			// 
			this.mnuDisableBreakpoint.Image = global::Mesen.GUI.Properties.Resources.BreakpointEnableDisable;
			this.mnuDisableBreakpoint.Name = "mnuDisableBreakpoint";
			this.mnuDisableBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuDisableBreakpoint.Text = "Disable breakpoint";
			this.mnuDisableBreakpoint.Click += new System.EventHandler(this.mnuDisableBreakpoint_Click);
			// 
			// mnuRemoveBreakpoint
			// 
			this.mnuRemoveBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.mnuRemoveBreakpoint.Name = "mnuRemoveBreakpoint";
			this.mnuRemoveBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuRemoveBreakpoint.Text = "Remove breakpoint";
			this.mnuRemoveBreakpoint.Click += new System.EventHandler(this.mnuRemoveBreakpoint_Click);
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.ctrlCodeViewer);
			this.splitContainer.Panel1MinSize = 150;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.ctrlFindOccurrences);
			this.splitContainer.Size = new System.Drawing.Size(479, 318);
			this.splitContainer.SplitterDistance = 150;
			this.splitContainer.TabIndex = 12;
			// 
			// ctrlFindOccurrences
			// 
			this.ctrlFindOccurrences.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFindOccurrences.Location = new System.Drawing.Point(0, 0);
			this.ctrlFindOccurrences.Name = "ctrlFindOccurrences";
			this.ctrlFindOccurrences.Size = new System.Drawing.Size(479, 164);
			this.ctrlFindOccurrences.TabIndex = 0;
			this.ctrlFindOccurrences.Viewer = null;
			this.ctrlFindOccurrences.OnSearchResultsClosed += new System.EventHandler(this.ctrlFindOccurrences_OnSearchResultsClosed);
			// 
			// ctrlDebuggerCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer);
			this.Name = "ctrlDebuggerCode";
			this.Size = new System.Drawing.Size(479, 318);
			this.contextMenuMargin.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Mesen.GUI.Debugger.ctrlScrollableTextbox ctrlCodeViewer;
		private System.Windows.Forms.ContextMenuStrip contextMenuMargin;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuEditBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableBreakpoint;
		private System.Windows.Forms.SplitContainer splitContainer;
		private Controls.ctrlFindOccurrences ctrlFindOccurrences;
	}
}
