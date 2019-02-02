namespace Mesen.GUI.Debugger.Controls
{
	partial class CodeViewerActions
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
			this.mnuMarkSelectionAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMarkAsCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMarkAsData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMarkAsUnidentifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPerfTracker = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPerfTrackerFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPerfTrackerCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPerfTrackerTextOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPerfTrackerDisabled = new System.Windows.Forms.ToolStripMenuItem();
			this.sepMarkSelectionAs = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditSourceFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditSelectedCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditSubroutine = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUndoPrgChrEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopySelection = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSetNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowCodeNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowByteCodeOnLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowByteCodeBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHideByteCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowLineNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrgShowInline = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrgAddressReplace = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrgAddressBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHidePrgAddresses = new System.Windows.Forms.ToolStripMenuItem();
			this.sepEditLabel = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditLabel = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditInMemoryViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.sepAddToWatch = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAddToWatch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowInSplitView = new System.Windows.Forms.ToolStripMenuItem();
			this.sepNavigation = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNavigateBackward = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNavigateForward = new System.Windows.Forms.ToolStripMenuItem();
			this.sepSwitchView = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSwitchView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowSourceAsComments = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMarkSelectionAs,
            this.toolStripMenuItem3,
            this.mnuPerfTracker,
            this.sepMarkSelectionAs,
            this.mnuEditSourceFile,
            this.mnuEditSelectedCode,
            this.mnuEditSubroutine,
            this.mnuUndoPrgChrEdit,
            this.mnuCopySelection,
            this.toolStripMenuItem7,
            this.mnuShowNextStatement,
            this.mnuSetNextStatement,
            this.toolStripMenuItem1,
            this.mnuShowCodeNotes,
            this.mnuShowLineNotes,
            this.sepEditLabel,
            this.mnuEditLabel,
            this.mnuEditInMemoryViewer,
            this.mnuToggleBreakpoint,
            this.sepAddToWatch,
            this.mnuAddToWatch,
            this.mnuFindOccurrences,
            this.toolStripMenuItem2,
            this.mnuGoToLocation,
            this.mnuShowInSplitView,
            this.sepNavigation,
            this.mnuNavigateBackward,
            this.mnuNavigateForward,
            this.sepSwitchView,
            this.mnuSwitchView,
            this.mnuShowSourceAsComments});
			this.contextMenu.Name = "contextMenuWatch";
			this.contextMenu.Size = new System.Drawing.Size(254, 564);
			this.contextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuCode_Closed);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuCode_Opening);
			// 
			// mnuMarkSelectionAs
			// 
			this.mnuMarkSelectionAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMarkAsCode,
            this.mnuMarkAsData,
            this.mnuMarkAsUnidentifiedData});
			this.mnuMarkSelectionAs.Name = "mnuMarkSelectionAs";
			this.mnuMarkSelectionAs.Size = new System.Drawing.Size(253, 22);
			this.mnuMarkSelectionAs.Text = "Mark selection as...";
			// 
			// mnuMarkAsCode
			// 
			this.mnuMarkAsCode.Image = global::Mesen.GUI.Properties.Resources.Accept;
			this.mnuMarkAsCode.Name = "mnuMarkAsCode";
			this.mnuMarkAsCode.Size = new System.Drawing.Size(199, 22);
			this.mnuMarkAsCode.Text = "Verified Code";
			this.mnuMarkAsCode.Click += new System.EventHandler(this.mnuMarkAsCode_Click);
			// 
			// mnuMarkAsData
			// 
			this.mnuMarkAsData.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuMarkAsData.Name = "mnuMarkAsData";
			this.mnuMarkAsData.Size = new System.Drawing.Size(199, 22);
			this.mnuMarkAsData.Text = "Verified Data";
			this.mnuMarkAsData.Click += new System.EventHandler(this.mnuMarkAsData_Click);
			// 
			// mnuMarkAsUnidentifiedData
			// 
			this.mnuMarkAsUnidentifiedData.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.mnuMarkAsUnidentifiedData.Name = "mnuMarkAsUnidentifiedData";
			this.mnuMarkAsUnidentifiedData.Size = new System.Drawing.Size(199, 22);
			this.mnuMarkAsUnidentifiedData.Text = "Unidentified Code/Data";
			this.mnuMarkAsUnidentifiedData.Click += new System.EventHandler(this.mnuMarkAsUnidentifiedData_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuPerfTracker
			// 
			this.mnuPerfTracker.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPerfTrackerFullscreen,
            this.mnuPerfTrackerCompact,
            this.mnuPerfTrackerTextOnly,
            this.toolStripMenuItem4,
            this.mnuPerfTrackerDisabled});
			this.mnuPerfTracker.Image = global::Mesen.GUI.Properties.Resources.PerfTracker;
			this.mnuPerfTracker.Name = "mnuPerfTracker";
			this.mnuPerfTracker.Size = new System.Drawing.Size(253, 22);
			this.mnuPerfTracker.Text = "Performance Tracker";
			this.mnuPerfTracker.DropDownOpening += new System.EventHandler(this.mnuPerfTracker_DropDownOpening);
			// 
			// mnuPerfTrackerFullscreen
			// 
			this.mnuPerfTrackerFullscreen.Name = "mnuPerfTrackerFullscreen";
			this.mnuPerfTrackerFullscreen.Size = new System.Drawing.Size(152, 22);
			this.mnuPerfTrackerFullscreen.Text = "Fullscreen";
			this.mnuPerfTrackerFullscreen.Click += new System.EventHandler(this.mnuPerfTrackerFullscreen_Click);
			// 
			// mnuPerfTrackerCompact
			// 
			this.mnuPerfTrackerCompact.Name = "mnuPerfTrackerCompact";
			this.mnuPerfTrackerCompact.Size = new System.Drawing.Size(152, 22);
			this.mnuPerfTrackerCompact.Text = "Compact";
			this.mnuPerfTrackerCompact.Click += new System.EventHandler(this.mnuPerfTrackerCompact_Click);
			// 
			// mnuPerfTrackerTextOnly
			// 
			this.mnuPerfTrackerTextOnly.Name = "mnuPerfTrackerTextOnly";
			this.mnuPerfTrackerTextOnly.Size = new System.Drawing.Size(152, 22);
			this.mnuPerfTrackerTextOnly.Text = "Text-only";
			this.mnuPerfTrackerTextOnly.Click += new System.EventHandler(this.mnuPerfTrackerTextOnly_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
			// 
			// mnuPerfTrackerDisabled
			// 
			this.mnuPerfTrackerDisabled.Checked = true;
			this.mnuPerfTrackerDisabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuPerfTrackerDisabled.Name = "mnuPerfTrackerDisabled";
			this.mnuPerfTrackerDisabled.Size = new System.Drawing.Size(152, 22);
			this.mnuPerfTrackerDisabled.Text = "Disabled";
			this.mnuPerfTrackerDisabled.Click += new System.EventHandler(this.mnuPerfTrackerDisabled_Click);
			// 
			// sepMarkSelectionAs
			// 
			this.sepMarkSelectionAs.Name = "sepMarkSelectionAs";
			this.sepMarkSelectionAs.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuEditSourceFile
			// 
			this.mnuEditSourceFile.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditSourceFile.Name = "mnuEditSourceFile";
			this.mnuEditSourceFile.Size = new System.Drawing.Size(253, 22);
			this.mnuEditSourceFile.Text = "Edit Source File";
			this.mnuEditSourceFile.Click += new System.EventHandler(this.mnuEditSourceFile_Click);
			// 
			// mnuEditSelectedCode
			// 
			this.mnuEditSelectedCode.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditSelectedCode.Name = "mnuEditSelectedCode";
			this.mnuEditSelectedCode.Size = new System.Drawing.Size(253, 22);
			this.mnuEditSelectedCode.Text = "Edit Selected Code";
			this.mnuEditSelectedCode.Click += new System.EventHandler(this.mnuEditSelectedCode_Click);
			// 
			// mnuEditSubroutine
			// 
			this.mnuEditSubroutine.Image = global::Mesen.GUI.Properties.Resources.Edit;
			this.mnuEditSubroutine.Name = "mnuEditSubroutine";
			this.mnuEditSubroutine.Size = new System.Drawing.Size(253, 22);
			this.mnuEditSubroutine.Text = "Edit Subroutine";
			this.mnuEditSubroutine.Click += new System.EventHandler(this.mnuEditSubroutine_Click);
			// 
			// mnuUndoPrgChrEdit
			// 
			this.mnuUndoPrgChrEdit.Image = global::Mesen.GUI.Properties.Resources.Undo;
			this.mnuUndoPrgChrEdit.Name = "mnuUndoPrgChrEdit";
			this.mnuUndoPrgChrEdit.Size = new System.Drawing.Size(253, 22);
			this.mnuUndoPrgChrEdit.Text = "Undo PRG/CHR Edit";
			this.mnuUndoPrgChrEdit.Click += new System.EventHandler(this.mnuUndoPrgChrEdit_Click);
			// 
			// mnuCopySelection
			// 
			this.mnuCopySelection.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCopySelection.Name = "mnuCopySelection";
			this.mnuCopySelection.Size = new System.Drawing.Size(253, 22);
			this.mnuCopySelection.Text = "Copy Selection";
			this.mnuCopySelection.Click += new System.EventHandler(this.mnuCopySelection_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuShowNextStatement
			// 
			this.mnuShowNextStatement.Name = "mnuShowNextStatement";
			this.mnuShowNextStatement.ShortcutKeyDisplayString = "";
			this.mnuShowNextStatement.Size = new System.Drawing.Size(253, 22);
			this.mnuShowNextStatement.Text = "Show Next Statement";
			this.mnuShowNextStatement.Click += new System.EventHandler(this.mnuShowNextStatement_Click);
			// 
			// mnuSetNextStatement
			// 
			this.mnuSetNextStatement.Name = "mnuSetNextStatement";
			this.mnuSetNextStatement.Size = new System.Drawing.Size(253, 22);
			this.mnuSetNextStatement.Text = "Set Next Statement";
			this.mnuSetNextStatement.Click += new System.EventHandler(this.mnuSetNextStatement_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuShowCodeNotes
			// 
			this.mnuShowCodeNotes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowByteCodeOnLeft,
            this.mnuShowByteCodeBelow,
            this.toolStripMenuItem5,
            this.mnuHideByteCode});
			this.mnuShowCodeNotes.Name = "mnuShowCodeNotes";
			this.mnuShowCodeNotes.Size = new System.Drawing.Size(253, 22);
			this.mnuShowCodeNotes.Text = "Byte Code Display";
			// 
			// mnuShowByteCodeOnLeft
			// 
			this.mnuShowByteCodeOnLeft.Name = "mnuShowByteCodeOnLeft";
			this.mnuShowByteCodeOnLeft.Size = new System.Drawing.Size(130, 22);
			this.mnuShowByteCodeOnLeft.Text = "On the left";
			this.mnuShowByteCodeOnLeft.Click += new System.EventHandler(this.mnuShowByteCodeOnLeft_Click);
			// 
			// mnuShowByteCodeBelow
			// 
			this.mnuShowByteCodeBelow.Name = "mnuShowByteCodeBelow";
			this.mnuShowByteCodeBelow.Size = new System.Drawing.Size(130, 22);
			this.mnuShowByteCodeBelow.Text = "Below";
			this.mnuShowByteCodeBelow.Click += new System.EventHandler(this.mnuShowByteCodeBelow_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(127, 6);
			// 
			// mnuHideByteCode
			// 
			this.mnuHideByteCode.Name = "mnuHideByteCode";
			this.mnuHideByteCode.Size = new System.Drawing.Size(130, 22);
			this.mnuHideByteCode.Text = "Hidden";
			this.mnuHideByteCode.Click += new System.EventHandler(this.mnuHideByteCode_Click);
			// 
			// mnuShowLineNotes
			// 
			this.mnuShowLineNotes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPrgShowInline,
            this.mnuPrgAddressReplace,
            this.mnuPrgAddressBelow,
            this.toolStripMenuItem6,
            this.mnuHidePrgAddresses});
			this.mnuShowLineNotes.Name = "mnuShowLineNotes";
			this.mnuShowLineNotes.Size = new System.Drawing.Size(253, 22);
			this.mnuShowLineNotes.Text = "PRG Address Display";
			// 
			// mnuPrgShowInline
			// 
			this.mnuPrgShowInline.Name = "mnuPrgShowInline";
			this.mnuPrgShowInline.Size = new System.Drawing.Size(196, 22);
			this.mnuPrgShowInline.Text = "Inline Compact Display";
			this.mnuPrgShowInline.Click += new System.EventHandler(this.mnuShowInlineCompactDisplay_Click);
			// 
			// mnuPrgAddressReplace
			// 
			this.mnuPrgAddressReplace.Name = "mnuPrgAddressReplace";
			this.mnuPrgAddressReplace.Size = new System.Drawing.Size(196, 22);
			this.mnuPrgAddressReplace.Text = "Replace CPU address";
			this.mnuPrgAddressReplace.Click += new System.EventHandler(this.mnuReplaceCpuAddress_Click);
			// 
			// mnuPrgAddressBelow
			// 
			this.mnuPrgAddressBelow.Name = "mnuPrgAddressBelow";
			this.mnuPrgAddressBelow.Size = new System.Drawing.Size(196, 22);
			this.mnuPrgAddressBelow.Text = "Below CPU address";
			this.mnuPrgAddressBelow.Click += new System.EventHandler(this.mnuBelowCpuAddress_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(193, 6);
			// 
			// mnuHidePrgAddresses
			// 
			this.mnuHidePrgAddresses.Name = "mnuHidePrgAddresses";
			this.mnuHidePrgAddresses.Size = new System.Drawing.Size(196, 22);
			this.mnuHidePrgAddresses.Text = "Hidden";
			this.mnuHidePrgAddresses.Click += new System.EventHandler(this.mnuHidePrgAddresses_Click);
			// 
			// sepEditLabel
			// 
			this.sepEditLabel.Name = "sepEditLabel";
			this.sepEditLabel.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuEditLabel
			// 
			this.mnuEditLabel.Image = global::Mesen.GUI.Properties.Resources.EditLabel;
			this.mnuEditLabel.Name = "mnuEditLabel";
			this.mnuEditLabel.Size = new System.Drawing.Size(253, 22);
			this.mnuEditLabel.Text = "Edit Label";
			this.mnuEditLabel.Click += new System.EventHandler(this.mnuEditLabel_Click);
			// 
			// mnuEditInMemoryViewer
			// 
			this.mnuEditInMemoryViewer.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuEditInMemoryViewer.Name = "mnuEditInMemoryViewer";
			this.mnuEditInMemoryViewer.Size = new System.Drawing.Size(253, 22);
			this.mnuEditInMemoryViewer.Text = "Edit in Memory Viewer";
			this.mnuEditInMemoryViewer.Click += new System.EventHandler(this.mnuEditInMemoryViewer_Click);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Breakpoint;
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.ShortcutKeyDisplayString = "";
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(253, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			this.mnuToggleBreakpoint.Click += new System.EventHandler(this.mnuToggleBreakpoint_Click);
			// 
			// sepAddToWatch
			// 
			this.sepAddToWatch.Name = "sepAddToWatch";
			this.sepAddToWatch.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuAddToWatch
			// 
			this.mnuAddToWatch.Name = "mnuAddToWatch";
			this.mnuAddToWatch.ShortcutKeyDisplayString = "Ctrl+Click";
			this.mnuAddToWatch.Size = new System.Drawing.Size(253, 22);
			this.mnuAddToWatch.Text = "Add to Watch";
			this.mnuAddToWatch.Click += new System.EventHandler(this.mnuAddToWatch_Click);
			// 
			// mnuFindOccurrences
			// 
			this.mnuFindOccurrences.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFindOccurrences.Name = "mnuFindOccurrences";
			this.mnuFindOccurrences.ShortcutKeyDisplayString = "Alt+Click";
			this.mnuFindOccurrences.Size = new System.Drawing.Size(253, 22);
			this.mnuFindOccurrences.Text = "Find Occurrences";
			this.mnuFindOccurrences.Click += new System.EventHandler(this.mnuFindOccurrences_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuGoToLocation
			// 
			this.mnuGoToLocation.Name = "mnuGoToLocation";
			this.mnuGoToLocation.ShortcutKeyDisplayString = "Double Click";
			this.mnuGoToLocation.Size = new System.Drawing.Size(253, 22);
			this.mnuGoToLocation.Text = "Go to Location";
			this.mnuGoToLocation.Click += new System.EventHandler(this.mnuGoToLocation_Click);
			// 
			// mnuShowInSplitView
			// 
			this.mnuShowInSplitView.Image = global::Mesen.GUI.Properties.Resources.SplitView;
			this.mnuShowInSplitView.Name = "mnuShowInSplitView";
			this.mnuShowInSplitView.ShortcutKeyDisplayString = "Ctrl+Alt+Click";
			this.mnuShowInSplitView.Size = new System.Drawing.Size(253, 22);
			this.mnuShowInSplitView.Text = "Show in Split View";
			this.mnuShowInSplitView.Click += new System.EventHandler(this.mnuShowInSplitView_Click);
			// 
			// sepNavigation
			// 
			this.sepNavigation.Name = "sepNavigation";
			this.sepNavigation.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuNavigateBackward
			// 
			this.mnuNavigateBackward.Image = global::Mesen.GUI.Properties.Resources.NavigateBack;
			this.mnuNavigateBackward.Name = "mnuNavigateBackward";
			this.mnuNavigateBackward.Size = new System.Drawing.Size(253, 22);
			this.mnuNavigateBackward.Text = "Navigate Backward";
			this.mnuNavigateBackward.Click += new System.EventHandler(this.mnuNavigateBackward_Click);
			// 
			// mnuNavigateForward
			// 
			this.mnuNavigateForward.Image = global::Mesen.GUI.Properties.Resources.NavigateForward;
			this.mnuNavigateForward.Name = "mnuNavigateForward";
			this.mnuNavigateForward.Size = new System.Drawing.Size(253, 22);
			this.mnuNavigateForward.Text = "Navigate Forward";
			this.mnuNavigateForward.Click += new System.EventHandler(this.mnuNavigateForward_Click);
			// 
			// sepSwitchView
			// 
			this.sepSwitchView.Name = "sepSwitchView";
			this.sepSwitchView.Size = new System.Drawing.Size(250, 6);
			// 
			// mnuSwitchView
			// 
			this.mnuSwitchView.Image = global::Mesen.GUI.Properties.Resources.SwitchView;
			this.mnuSwitchView.Name = "mnuSwitchView";
			this.mnuSwitchView.Size = new System.Drawing.Size(253, 22);
			this.mnuSwitchView.Text = "Switch to Source View";
			this.mnuSwitchView.Click += new System.EventHandler(this.mnuSwitchView_Click);
			// 
			// mnuShowSourceAsComments
			// 
			this.mnuShowSourceAsComments.CheckOnClick = true;
			this.mnuShowSourceAsComments.Name = "mnuShowSourceAsComments";
			this.mnuShowSourceAsComments.Size = new System.Drawing.Size(253, 22);
			this.mnuShowSourceAsComments.Text = "Show source code as comments";
			this.mnuShowSourceAsComments.Click += new System.EventHandler(this.mnuShowSourceAsComments_Click);
			// 
			// CodeViewerActions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "CodeViewerActions";
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolStripMenuItem mnuMarkSelectionAs;
		private System.Windows.Forms.ToolStripMenuItem mnuMarkAsCode;
		private System.Windows.Forms.ToolStripMenuItem mnuMarkAsData;
		private System.Windows.Forms.ToolStripMenuItem mnuMarkAsUnidentifiedData;
		private System.Windows.Forms.ToolStripSeparator sepMarkSelectionAs;
		private System.Windows.Forms.ToolStripMenuItem mnuEditSelectedCode;
		private System.Windows.Forms.ToolStripMenuItem mnuEditSubroutine;
		private System.Windows.Forms.ToolStripMenuItem mnuUndoPrgChrEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuCopySelection;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mnuShowNextStatement;
		private System.Windows.Forms.ToolStripMenuItem mnuSetNextStatement;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCodeNotes;
		private System.Windows.Forms.ToolStripMenuItem mnuShowByteCodeOnLeft;
		private System.Windows.Forms.ToolStripMenuItem mnuShowByteCodeBelow;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuHideByteCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowLineNotes;
		private System.Windows.Forms.ToolStripMenuItem mnuPrgShowInline;
		private System.Windows.Forms.ToolStripMenuItem mnuPrgAddressReplace;
		private System.Windows.Forms.ToolStripMenuItem mnuPrgAddressBelow;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mnuHidePrgAddresses;
		private System.Windows.Forms.ToolStripSeparator sepEditLabel;
		private System.Windows.Forms.ToolStripMenuItem mnuEditLabel;
		private System.Windows.Forms.ToolStripMenuItem mnuEditInMemoryViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuToggleBreakpoint;
		private System.Windows.Forms.ToolStripSeparator sepAddToWatch;
		private System.Windows.Forms.ToolStripMenuItem mnuAddToWatch;
		private System.Windows.Forms.ToolStripMenuItem mnuFindOccurrences;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInSplitView;
		private System.Windows.Forms.ToolStripSeparator sepNavigation;
		private System.Windows.Forms.ToolStripMenuItem mnuNavigateBackward;
		private System.Windows.Forms.ToolStripMenuItem mnuNavigateForward;
		private System.Windows.Forms.ToolStripSeparator sepSwitchView;
		private System.Windows.Forms.ToolStripMenuItem mnuSwitchView;
		public System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuShowSourceAsComments;
		private System.Windows.Forms.ToolStripMenuItem mnuEditSourceFile;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuPerfTracker;
		private System.Windows.Forms.ToolStripMenuItem mnuPerfTrackerFullscreen;
		private System.Windows.Forms.ToolStripMenuItem mnuPerfTrackerCompact;
		private System.Windows.Forms.ToolStripMenuItem mnuPerfTrackerTextOnly;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuPerfTrackerDisabled;
	}
}
