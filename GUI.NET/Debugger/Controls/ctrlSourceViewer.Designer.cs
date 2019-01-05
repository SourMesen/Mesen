namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlSourceViewer
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
			if(_codeViewerActions != null) {
				_codeViewerActions.Dispose();
				_codeViewerActions = null;
			}
			if(_tooltipManager != null) {
				_tooltipManager.Dispose();
				_tooltipManager = null;
			}
			DebugWorkspaceManager.SymbolProviderChanged -= UpdateSymbolProvider;
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblFile = new System.Windows.Forms.Label();
			this.cboFile = new System.Windows.Forms.ComboBox();
			this.ctrlCodeViewer = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
			this.contextMenuMargin = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuEditBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.contextMenuMargin.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblFile, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cboFile, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlCodeViewer, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(463, 275);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblFile
			// 
			this.lblFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblFile.AutoSize = true;
			this.lblFile.Location = new System.Drawing.Point(3, 7);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(26, 13);
			this.lblFile.TabIndex = 0;
			this.lblFile.Text = "File:";
			// 
			// cboFile
			// 
			this.cboFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFile.FormattingEnabled = true;
			this.cboFile.Location = new System.Drawing.Point(35, 3);
			this.cboFile.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.cboFile.Name = "cboFile";
			this.cboFile.Size = new System.Drawing.Size(428, 21);
			this.cboFile.TabIndex = 1;
			this.cboFile.SelectedIndexChanged += new System.EventHandler(this.cboFile_SelectedIndexChanged);
			// 
			// ctrlCodeViewer
			// 
			this.ctrlCodeViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlCodeViewer.CodeHighlightingEnabled = false;
			this.tableLayoutPanel1.SetColumnSpan(this.ctrlCodeViewer, 2);
			this.ctrlCodeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCodeViewer.HideSelection = false;
			this.ctrlCodeViewer.Location = new System.Drawing.Point(0, 27);
			this.ctrlCodeViewer.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlCodeViewer.Name = "ctrlCodeViewer";
			this.ctrlCodeViewer.ShowCompactPrgAddresses = false;
			this.ctrlCodeViewer.ShowContentNotes = false;
			this.ctrlCodeViewer.ShowLineNumberNotes = false;
			this.ctrlCodeViewer.ShowMemoryValues = false;
			this.ctrlCodeViewer.ShowScrollbars = true;
			this.ctrlCodeViewer.ShowSingleContentLineNotes = true;
			this.ctrlCodeViewer.ShowSingleLineLineNumberNotes = false;
			this.ctrlCodeViewer.Size = new System.Drawing.Size(463, 248);
			this.ctrlCodeViewer.TabIndex = 2;
			this.ctrlCodeViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseUp);
			this.ctrlCodeViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseMove);
			this.ctrlCodeViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDown);
			this.ctrlCodeViewer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDoubleClick);
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
			// ctrlSourceViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlSourceViewer";
			this.Size = new System.Drawing.Size(463, 275);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.contextMenuMargin.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.ComboBox cboFile;
		private ctrlScrollableTextbox ctrlCodeViewer;
		private System.Windows.Forms.ContextMenuStrip contextMenuMargin;
		private System.Windows.Forms.ToolStripMenuItem mnuEditBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
	}
}
