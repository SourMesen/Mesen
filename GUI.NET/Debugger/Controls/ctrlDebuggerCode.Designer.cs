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
			this.contextMenuCode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSetNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowLineNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCodeNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditLabel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddToWatch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNavigateBackward = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNavigateForward = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ctrlCodeViewer = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
			this.contextMenuMargin = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuDisableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.tlpSearchResults = new System.Windows.Forms.TableLayoutPanel();
			this.lstSearchResult = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.picCloseOccurrenceList = new System.Windows.Forms.PictureBox();
			this.lblSearchResult = new System.Windows.Forms.Label();
			this.contextMenuCode.SuspendLayout();
			this.contextMenuMargin.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.tlpSearchResults.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCloseOccurrenceList)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenuCode
			// 
			this.contextMenuCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNextStatement,
            this.mnuSetNextStatement,
            this.toolStripMenuItem1,
            this.mnuShowLineNotes,
            this.mnuShowCodeNotes,
            this.toolStripMenuItem2,
            this.mnuEditLabel,
            this.toolStripMenuItem4,
            this.mnuGoToLocation,
            this.mnuAddToWatch,
            this.mnuFindOccurrences,
            this.toolStripMenuItem3,
            this.mnuNavigateBackward,
            this.mnuNavigateForward});
			this.contextMenuCode.Name = "contextMenuWatch";
			this.contextMenuCode.Size = new System.Drawing.Size(259, 248);
			this.contextMenuCode.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuCode_Opening);
			// 
			// mnuShowNextStatement
			// 
			this.mnuShowNextStatement.Name = "mnuShowNextStatement";
			this.mnuShowNextStatement.ShortcutKeyDisplayString = "Alt+*";
			this.mnuShowNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Multiply)));
			this.mnuShowNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuShowNextStatement.Text = "Show Next Statement";
			this.mnuShowNextStatement.Click += new System.EventHandler(this.mnuShowNextStatement_Click);
			// 
			// mnuSetNextStatement
			// 
			this.mnuSetNextStatement.Name = "mnuSetNextStatement";
			this.mnuSetNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F10)));
			this.mnuSetNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuSetNextStatement.Text = "Set Next Statement";
			this.mnuSetNextStatement.Click += new System.EventHandler(this.mnuSetNextStatement_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuShowLineNotes
			// 
			this.mnuShowLineNotes.CheckOnClick = true;
			this.mnuShowLineNotes.Name = "mnuShowLineNotes";
			this.mnuShowLineNotes.Size = new System.Drawing.Size(258, 22);
			this.mnuShowLineNotes.Text = "Show PRG Addresses";
			this.mnuShowLineNotes.Click += new System.EventHandler(this.mnuShowLineNotes_Click);
			// 
			// mnuShowCodeNotes
			// 
			this.mnuShowCodeNotes.CheckOnClick = true;
			this.mnuShowCodeNotes.Name = "mnuShowCodeNotes";
			this.mnuShowCodeNotes.Size = new System.Drawing.Size(258, 22);
			this.mnuShowCodeNotes.Text = "Show Byte Code";
			this.mnuShowCodeNotes.Click += new System.EventHandler(this.mnuShowCodeNotes_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuEditLabel
			// 
			this.mnuEditLabel.Name = "mnuEditLabel";
			this.mnuEditLabel.Size = new System.Drawing.Size(258, 22);
			this.mnuEditLabel.Text = "Edit Label";
			this.mnuEditLabel.Click += new System.EventHandler(this.mnuEditLabel_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuGoToLocation
			// 
			this.mnuGoToLocation.Name = "mnuGoToLocation";
			this.mnuGoToLocation.Size = new System.Drawing.Size(258, 22);
			this.mnuGoToLocation.Text = "Go To Location";
			this.mnuGoToLocation.Click += new System.EventHandler(this.mnuGoToLocation_Click);
			// 
			// mnuAddToWatch
			// 
			this.mnuAddToWatch.Name = "mnuAddToWatch";
			this.mnuAddToWatch.Size = new System.Drawing.Size(258, 22);
			this.mnuAddToWatch.Text = "Add to Watch";
			this.mnuAddToWatch.Click += new System.EventHandler(this.mnuAddToWatch_Click);
			// 
			// mnuFindOccurrences
			// 
			this.mnuFindOccurrences.Name = "mnuFindOccurrences";
			this.mnuFindOccurrences.Size = new System.Drawing.Size(258, 22);
			this.mnuFindOccurrences.Text = "Find Occurrences";
			this.mnuFindOccurrences.Click += new System.EventHandler(this.mnuFindOccurrences_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuNavigateBackward
			// 
			this.mnuNavigateBackward.Image = global::Mesen.GUI.Properties.Resources.PreviousArrow;
			this.mnuNavigateBackward.Name = "mnuNavigateBackward";
			this.mnuNavigateBackward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
			this.mnuNavigateBackward.Size = new System.Drawing.Size(258, 22);
			this.mnuNavigateBackward.Text = "Navigate Backward";
			this.mnuNavigateBackward.Click += new System.EventHandler(this.mnuNavigateBackward_Click);
			// 
			// mnuNavigateForward
			// 
			this.mnuNavigateForward.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuNavigateForward.Name = "mnuNavigateForward";
			this.mnuNavigateForward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
			this.mnuNavigateForward.Size = new System.Drawing.Size(258, 22);
			this.mnuNavigateForward.Text = "Navigate Forward";
			this.mnuNavigateForward.Click += new System.EventHandler(this.mnuNavigateForward_Click);
			// 
			// ctrlCodeViewer
			// 
			this.ctrlCodeViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlCodeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCodeViewer.FontSize = 13F;
			this.ctrlCodeViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlCodeViewer.Name = "ctrlCodeViewer";
			this.ctrlCodeViewer.ShowContentNotes = false;
			this.ctrlCodeViewer.ShowLineNumberNotes = false;
			this.ctrlCodeViewer.Size = new System.Drawing.Size(479, 150);
			this.ctrlCodeViewer.TabIndex = 1;
			this.ctrlCodeViewer.ScrollPositionChanged += new System.EventHandler(this.ctrlCodeViewer_ScrollPositionChanged);
			this.ctrlCodeViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseUp);
			this.ctrlCodeViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseMove);
			this.ctrlCodeViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDown);
			this.ctrlCodeViewer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseDoubleClick);
			this.ctrlCodeViewer.FontSizeChanged += new System.EventHandler(this.ctrlCodeViewer_FontSizeChanged);
			// 
			// contextMenuMargin
			// 
			this.contextMenuMargin.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisableBreakpoint,
            this.mnuRemoveBreakpoint,
            this.mnuEditBreakpoint});
			this.contextMenuMargin.Name = "contextMenuMargin";
			this.contextMenuMargin.Size = new System.Drawing.Size(178, 70);
			this.contextMenuMargin.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuMargin_Opening);
			// 
			// mnuDisableBreakpoint
			// 
			this.mnuDisableBreakpoint.Name = "mnuDisableBreakpoint";
			this.mnuDisableBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuDisableBreakpoint.Text = "Disable breakpoint";
			this.mnuDisableBreakpoint.Click += new System.EventHandler(this.mnuDisableBreakpoint_Click);
			// 
			// mnuRemoveBreakpoint
			// 
			this.mnuRemoveBreakpoint.Name = "mnuRemoveBreakpoint";
			this.mnuRemoveBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuRemoveBreakpoint.Text = "Remove breakpoint";
			this.mnuRemoveBreakpoint.Click += new System.EventHandler(this.mnuRemoveBreakpoint_Click);
			// 
			// mnuEditBreakpoint
			// 
			this.mnuEditBreakpoint.Name = "mnuEditBreakpoint";
			this.mnuEditBreakpoint.Size = new System.Drawing.Size(177, 22);
			this.mnuEditBreakpoint.Text = "Edit breakpoint";
			this.mnuEditBreakpoint.Click += new System.EventHandler(this.mnuEditBreakpoint_Click);
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
			this.splitContainer.Panel2.Controls.Add(this.tlpSearchResults);
			this.splitContainer.Size = new System.Drawing.Size(479, 318);
			this.splitContainer.SplitterDistance = 150;
			this.splitContainer.TabIndex = 12;
			// 
			// tlpSearchResults
			// 
			this.tlpSearchResults.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpSearchResults.ColumnCount = 1;
			this.tlpSearchResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSearchResults.Controls.Add(this.lstSearchResult, 0, 1);
			this.tlpSearchResults.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tlpSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpSearchResults.Location = new System.Drawing.Point(0, 0);
			this.tlpSearchResults.Name = "tlpSearchResults";
			this.tlpSearchResults.RowCount = 2;
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpSearchResults.Size = new System.Drawing.Size(479, 164);
			this.tlpSearchResults.TabIndex = 12;
			// 
			// lstSearchResult
			// 
			this.lstSearchResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lstSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSearchResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstSearchResult.FullRowSelect = true;
			this.lstSearchResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstSearchResult.Location = new System.Drawing.Point(1, 24);
			this.lstSearchResult.Margin = new System.Windows.Forms.Padding(0);
			this.lstSearchResult.Name = "lstSearchResult";
			this.lstSearchResult.Size = new System.Drawing.Size(477, 139);
			this.lstSearchResult.TabIndex = 9;
			this.lstSearchResult.UseCompatibleStateImageBehavior = false;
			this.lstSearchResult.View = System.Windows.Forms.View.Details;
			this.lstSearchResult.SizeChanged += new System.EventHandler(this.lstSearchResult_SizeChanged);
			this.lstSearchResult.DoubleClick += new System.EventHandler(this.lstSearchResult_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "";
			this.columnHeader2.Width = 160;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.picCloseOccurrenceList, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSearchResult, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(477, 22);
			this.tableLayoutPanel2.TabIndex = 11;
			// 
			// picCloseOccurrenceList
			// 
			this.picCloseOccurrenceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.picCloseOccurrenceList.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picCloseOccurrenceList.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.picCloseOccurrenceList.Location = new System.Drawing.Point(458, 3);
			this.picCloseOccurrenceList.Name = "picCloseOccurrenceList";
			this.picCloseOccurrenceList.Size = new System.Drawing.Size(16, 16);
			this.picCloseOccurrenceList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picCloseOccurrenceList.TabIndex = 10;
			this.picCloseOccurrenceList.TabStop = false;
			this.picCloseOccurrenceList.Click += new System.EventHandler(this.picCloseOccurrenceList_Click);
			// 
			// lblSearchResult
			// 
			this.lblSearchResult.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSearchResult.AutoSize = true;
			this.lblSearchResult.Location = new System.Drawing.Point(3, 4);
			this.lblSearchResult.Name = "lblSearchResult";
			this.lblSearchResult.Size = new System.Drawing.Size(95, 13);
			this.lblSearchResult.TabIndex = 11;
			this.lblSearchResult.Text = "Search results for: ";
			// 
			// ctrlDebuggerCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer);
			this.Name = "ctrlDebuggerCode";
			this.Size = new System.Drawing.Size(479, 318);
			this.contextMenuCode.ResumeLayout(false);
			this.contextMenuMargin.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.tlpSearchResults.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCloseOccurrenceList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowNextStatement;
		private System.Windows.Forms.ToolStripMenuItem mnuSetNextStatement;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
		private System.Windows.Forms.ToolStripMenuItem mnuAddToWatch;
		private Mesen.GUI.Debugger.ctrlScrollableTextbox ctrlCodeViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuShowLineNotes;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCodeNotes;
		private System.Windows.Forms.ContextMenuStrip contextMenuMargin;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuEditBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuDisableBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuFindOccurrences;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TableLayoutPanel tlpSearchResults;
		private System.Windows.Forms.ListView lstSearchResult;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.PictureBox picCloseOccurrenceList;
		private System.Windows.Forms.Label lblSearchResult;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuNavigateForward;
		private System.Windows.Forms.ToolStripMenuItem mnuNavigateBackward;
		private System.Windows.Forms.ToolStripMenuItem mnuEditLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
	}
}
