using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using System.Globalization;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class CodeViewerActions : UserControl
	{
		public event SetNextStatementEventHandler OnSetNextStatement;
		public event ShowInSplitViewEventHandler OnShowInSplitView;
		public event SwitchToSourceEventHandler OnSwitchView;

		private int _lastClickedAddress = Int32.MaxValue;
		private Ld65DbgImporter.SymbolInfo _lastClickedSymbol = null;
		private string _newWatchValue = string.Empty;
		private string _lastWord = string.Empty;
		private Point _lastLocation = Point.Empty;
		private DebugViewInfo _config;

		public ICodeViewer Viewer { get; set; }
		public bool SourceView { get; set; }

		public CodeViewerActions()
		{
			InitializeComponent();
		}

		public CodeViewerActions(ICodeViewer viewer, bool isSourceView) : this()
		{
			Viewer = viewer;
			SourceView = isSourceView;

			this.InitShortcuts();
		}

		private void InitShortcuts()
		{
			Control parent = (Control)Viewer;
			mnuEditInMemoryViewer.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_EditInMemoryViewer));
			mnuEditLabel.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_EditLabel));
			mnuSetNextStatement.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_SetNextStatement));
			mnuShowNextStatement.InitShortcut(parent, nameof(DebuggerShortcutsConfig.GoToProgramCounter));
			mnuToggleBreakpoint.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_ToggleBreakpoint));

			mnuUndoPrgChrEdit.InitShortcut(parent, nameof(DebuggerShortcutsConfig.Undo));
			mnuCopySelection.InitShortcut(parent, nameof(DebuggerShortcutsConfig.Copy));

			mnuSwitchView.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_SwitchView));

			if(!SourceView) {
				mnuNavigateBackward.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_NavigateBack));
				mnuNavigateForward.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_NavigateForward));

				mnuEditSelectedCode.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_EditSelectedCode));
				mnuEditSubroutine.InitShortcut(parent, nameof(DebuggerShortcutsConfig.CodeWindow_EditSubroutine));

				mnuMarkAsCode.InitShortcut(parent, nameof(DebuggerShortcutsConfig.MarkAsCode));
				mnuMarkAsData.InitShortcut(parent, nameof(DebuggerShortcutsConfig.MarkAsData));
				mnuMarkAsUnidentifiedData.InitShortcut(parent, nameof(DebuggerShortcutsConfig.MarkAsUnidentified));
			}
		}

		public void InitMenu(DebugViewInfo config)
		{
			_config = config;
			mnuPrgShowInline.Checked = false;
			mnuPrgAddressReplace.Checked = false;
			mnuPrgAddressBelow.Checked = false;
			mnuHidePrgAddresses.Checked = false;

			mnuShowByteCodeOnLeft.Checked = false;
			mnuShowByteCodeBelow.Checked = false;
			mnuHideByteCode.Checked = false;

			mnuShowSourceAsComments.Checked = config.ShowSourceAsComments;

			switch(config.ByteCodePosition) {
				case ByteCodePosition.Left:
					Viewer.CodeViewer.ShowContentNotes = true;
					Viewer.CodeViewer.ShowSingleContentLineNotes = true;
					this.mnuShowByteCodeOnLeft.Checked = true;
					break;

				case ByteCodePosition.Below:
					Viewer.CodeViewer.ShowContentNotes = true;
					Viewer.CodeViewer.ShowSingleContentLineNotes = false;
					this.mnuShowByteCodeBelow.Checked = true;
					break;

				case ByteCodePosition.Hidden:
					Viewer.CodeViewer.ShowContentNotes = false;
					Viewer.CodeViewer.ShowSingleContentLineNotes = false;
					this.mnuHideByteCode.Checked = true;
					break;
			}

			switch(config.PrgAddressPosition) {
				case PrgAddressPosition.Inline:
					Viewer.CodeViewer.ShowCompactPrgAddresses = true;
					Viewer.CodeViewer.ShowLineNumberNotes = false;
					Viewer.CodeViewer.ShowSingleLineLineNumberNotes = false;
					this.mnuPrgShowInline.Checked = true;
					break;

				case PrgAddressPosition.Replace:
					Viewer.CodeViewer.ShowCompactPrgAddresses = false;
					Viewer.CodeViewer.ShowLineNumberNotes = true;
					Viewer.CodeViewer.ShowSingleLineLineNumberNotes = true;
					this.mnuPrgAddressReplace.Checked = true;
					break;

				case PrgAddressPosition.Below:
					Viewer.CodeViewer.ShowCompactPrgAddresses = false;
					Viewer.CodeViewer.ShowLineNumberNotes = true;
					Viewer.CodeViewer.ShowSingleLineLineNumberNotes = false;
					this.mnuPrgAddressBelow.Checked = true;
					break;

				case PrgAddressPosition.Hidden:
					Viewer.CodeViewer.ShowCompactPrgAddresses = false;
					Viewer.CodeViewer.ShowLineNumberNotes = false;
					Viewer.CodeViewer.ShowSingleLineLineNumberNotes = false;
					this.mnuHidePrgAddresses.Checked = true;
					break;
			}
		}

		private void UpdateConfig()
		{
			this.InitMenu(_config);
			ConfigManager.ApplyChanges();
		}

		private void contextMenuCode_Opening(object sender, CancelEventArgs e)
		{
			UpdateContextMenuItemVisibility(contextMenu.Items, true);

			int startAddress, endAddress;
			string range;
			GetSelectedAddressRange(out startAddress, out endAddress, out range);
			mnuMarkSelectionAs.Enabled = startAddress >= 0 && endAddress >= 0 && startAddress <= endAddress;
			if(mnuMarkSelectionAs.Enabled) {
				mnuMarkSelectionAs.Text = "Mark selection as... (" + range + ")";
			} else {
				mnuMarkSelectionAs.Text = "Mark selection as...";
			}
		}

		private void GetSelectedAddressRange(out int start, out int end, out string range)
		{
			int firstLineOfSelection = Viewer.CodeViewer.SelectionStart;
			while(Viewer.CodeViewer.GetLineNumber(firstLineOfSelection) < 0) {
				firstLineOfSelection++;
			}
			int firstLineAfterSelection = Viewer.CodeViewer.SelectionStart + Viewer.CodeViewer.SelectionLength + 1;
			while(Viewer.CodeViewer.GetLineNumber(firstLineAfterSelection) < 0) {
				firstLineAfterSelection++;
			}
			start = Viewer.CodeViewer.GetLineNumber(firstLineOfSelection);
			end = Viewer.CodeViewer.GetLineNumber(firstLineAfterSelection) - 1;

			range = "";
			if(start >= 0 && end >= 0) {
				range = $"${start.ToString("X4")} - ${end.ToString("X4")}";
				start = InteropEmu.DebugGetAbsoluteAddress((UInt32)start);
				end = InteropEmu.DebugGetAbsoluteAddress((UInt32)end);
			}
		}

		private void MarkSelectionAs(CdlPrgFlags type)
		{
			int startAddress, endAddress;
			string range;
			GetSelectedAddressRange(out startAddress, out endAddress, out range);

			if(startAddress >= 0 && endAddress >= 0 && startAddress <= endAddress) {
				InteropEmu.DebugMarkPrgBytesAs((UInt32)startAddress, (UInt32)endAddress, type);

				frmDebugger debugger = DebugWindowManager.GetDebugger();
				if(debugger != null) {
					debugger.UpdateDebugger(false);
				}
			}
		}
		
		private void mnuMarkAsCode_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(CdlPrgFlags.Code);
		}

		private void mnuMarkAsData_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(CdlPrgFlags.Data);
		}

		private void mnuMarkAsUnidentifiedData_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(CdlPrgFlags.None);
		}

		private void mnuGoToLocation_Click(object sender, EventArgs e)
		{
			GoToLocation();
		}

		private void GoToLocation()
		{
			if(_lastClickedSymbol != null && Viewer is ctrlSourceViewer) {
				Ld65DbgImporter.DefinitionInfo def = Viewer.SymbolProvider.GetSymbolDefinition(_lastClickedSymbol);
				if(def != null) {
					((ctrlSourceViewer)Viewer).ScrollToFileLine(def.FileName, def.Line);
					return;
				}
			}

			if(_lastClickedAddress >= 0) {
				Viewer.ScrollToLineNumber((int)_lastClickedAddress);
			}
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			AddWatch();
		}

		private void AddWatch()
		{
			WatchManager.AddWatch(_newWatchValue);
		}

		private void mnuShowInSplitView_Click(object sender, EventArgs e)
		{
			ShowInSplitView();
		}

		private void ShowInSplitView()
		{
			if(_lastClickedAddress >= 0) {
				this.OnShowInSplitView?.Invoke(Viewer, new AddressEventArgs() { Address = (UInt32)_lastClickedAddress });
			} else if(_lastClickedSymbol != null) {
				AddressTypeInfo info = Viewer.SymbolProvider.GetSymbolAddressInfo(_lastClickedSymbol);
				if(info != null && info.Address >= 0) {
					int relAddress = InteropEmu.DebugGetRelativeAddress((UInt32)info.Address, info.Type);
					this.OnShowInSplitView?.Invoke(Viewer, new AddressEventArgs() { Address = (UInt32)relAddress });
				}
			}
		}
		
		private void mnuEditLabel_Click(object sender, EventArgs e)
		{
			if(UpdateContextMenu(_lastLocation)) {
				if(_lastClickedAddress >= 0) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)_lastClickedAddress, info);
					if(info.Address >= 0) {
						ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
					}
				} else if(_lastClickedSymbol != null) {
					AddressTypeInfo info = Viewer.SymbolProvider.GetSymbolAddressInfo(_lastClickedSymbol);
					if(info != null && info.Address >= 0) {
						ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
					}
				}
			}
		}

		private void mnuNavigateForward_Click(object sender, EventArgs e)
		{
			Viewer.CodeViewer.NavigateForward();
		}

		private void mnuNavigateBackward_Click(object sender, EventArgs e)
		{
			Viewer.CodeViewer.NavigateBackward();
		}
		
		private void mnuToggleBreakpoint_Click(object sender, EventArgs e)
		{
			this.ToggleBreakpoint(false);
		}

		public void ToggleBreakpoint(bool toggleEnabledFlag)
		{
			AddressTypeInfo info = Viewer.GetAddressInfo(Viewer.CodeViewer.SelectedLine);
			if(info.Address >= 0) {
				BreakpointManager.ToggleBreakpoint(-1, info, toggleEnabledFlag);
			} else {
				//Current line has no address, try using the next line instead.
				//(Used when trying to set a breakpoint on a row containing only a label)
				info = Viewer.GetAddressInfo(Viewer.CodeViewer.SelectedLine + 1);
				if(info.Address >= 0) {
					BreakpointManager.ToggleBreakpoint(-1, info, toggleEnabledFlag);
				}
			}
		}

		private void mnuShowByteCodeOnLeft_Click(object sender, EventArgs e)
		{
			_config.ByteCodePosition = ByteCodePosition.Left;
			this.UpdateConfig();
		}

		private void mnuShowByteCodeBelow_Click(object sender, EventArgs e)
		{
			_config.ByteCodePosition = ByteCodePosition.Below;
			this.UpdateConfig();
		}

		private void mnuHideByteCode_Click(object sender, EventArgs e)
		{
			_config.ByteCodePosition = ByteCodePosition.Hidden;
			this.UpdateConfig();
		}

		private void mnuShowSourceAsComments_Click(object sender, EventArgs e)
		{
			_config.ShowSourceAsComments = mnuShowSourceAsComments.Checked;
			this.UpdateConfig();
		}

		private void mnuShowInlineCompactDisplay_Click(object sender, EventArgs e)
		{
			_config.PrgAddressPosition = PrgAddressPosition.Inline;
			this.UpdateConfig();
		}

		private void mnuReplaceCpuAddress_Click(object sender, EventArgs e)
		{
			_config.PrgAddressPosition = PrgAddressPosition.Replace;
			this.UpdateConfig();
		}

		private void mnuBelowCpuAddress_Click(object sender, EventArgs e)
		{
			_config.PrgAddressPosition = PrgAddressPosition.Below;
			this.UpdateConfig();
		}

		private void mnuHidePrgAddresses_Click(object sender, EventArgs e)
		{
			_config.PrgAddressPosition = PrgAddressPosition.Hidden;
			this.UpdateConfig();
		}

		private void mnuCopySelection_Click(object sender, EventArgs e)
		{
			Viewer.CodeViewer.CopySelection(ConfigManager.Config.DebugInfo.CopyAddresses, ConfigManager.Config.DebugInfo.CopyByteCode, ConfigManager.Config.DebugInfo.CopyComments);
		}
		
		private void mnuShowNextStatement_Click(object sender, EventArgs e)
		{
			this.ScrollToActiveAddress();
		}
		
		public void ScrollToActiveAddress()
		{
			if(Viewer.ActiveAddress.HasValue) {
				Viewer.ScrollToLineNumber((int)Viewer.ActiveAddress.Value);
			}
		}

		private void mnuShowLineNotes_Click(object sender, EventArgs e)
		{
			Viewer.CodeViewer.ShowLineNumberNotes = this.mnuShowLineNotes.Checked;
			this.UpdateConfig();
		}

		private void mnuFindOccurrences_Click(object sender, EventArgs e)
		{
			Viewer.FindAllOccurrences(_lastWord, true, true);
		}

		private void mnuUndoPrgChrEdit_Click(object sender, EventArgs e)
		{
			if(InteropEmu.DebugHasUndoHistory()) {
				InteropEmu.DebugPerformUndo();
				frmDebugger debugger = DebugWindowManager.GetDebugger();
				if(debugger != null) {
					debugger.UpdateDebugger(false);
				}
			}
		}
		
		private void mnuSetNextStatement_Click(object sender, EventArgs e)
		{
			this.OnSetNextStatement?.Invoke(new AddressEventArgs() { Address = (UInt32)Viewer.CodeViewer.CurrentLine });
		}

		private void mnuEditSubroutine_Click(object sender, EventArgs e)
		{
			Viewer.EditSubroutine();
		}

		private void mnuEditSelectedCode_Click(object sender, EventArgs e)
		{
			Viewer.EditSelectedCode();
		}

		private void mnuEditInMemoryViewer_Click(object sender, EventArgs e)
		{
			if(UpdateContextMenu(_lastLocation)) {
				if(_lastClickedAddress >= 0) {
					DebugWindowManager.OpenMemoryViewer(_lastClickedAddress, DebugMemoryType.CpuMemory);
				} else {
					AddressTypeInfo info = Viewer.SymbolProvider.GetSymbolAddressInfo(_lastClickedSymbol);
					if(info != null && info.Address >= 0) {
						DebugWindowManager.OpenMemoryViewer(info.Address, info.Type.ToMemoryType());
					}
				}
			}
		}

		private void mnuSwitchView_Click(object sender, EventArgs e)
		{
			if(Viewer.SymbolProvider != null) {
				this.SwitchView();
			}
		}

		public void SwitchView()
		{
			this.OnSwitchView?.Invoke(Viewer);
		}

		public void ProcessMouseUp(Point location, MouseButtons button)
		{
			if(UpdateContextMenu(location)) {
				if(button == MouseButtons.Left) {
					if(ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Alt)) {
						ShowInSplitView();
					} else if(ModifierKeys.HasFlag(Keys.Control)) {
						AddWatch();
					} else if(ModifierKeys.HasFlag(Keys.Alt)) {
						Viewer.FindAllOccurrences(_lastWord, true, true);
					}
				}
			}
		}

		public void ProcessMouseDoubleClick(Point location)
		{
			if(UpdateContextMenu(location) && mnuGoToLocation.Enabled) {
				GoToLocation();
			}
		}

		private void contextMenuCode_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			mnuEditSelectedCode.Enabled = true;
			mnuEditSubroutine.Enabled = true;
		}

		public void UpdateContextMenuItemVisibility(ToolStripItemCollection items, bool? visible = null)
		{
			items[nameof(mnuUndoPrgChrEdit)].Enabled = InteropEmu.DebugHasUndoHistory();
			items[nameof(mnuShowNextStatement)].Enabled = Viewer.ActiveAddress.HasValue;
			items[nameof(mnuSetNextStatement)].Enabled = Viewer.ActiveAddress.HasValue;
			items[nameof(mnuEditSelectedCode)].Enabled = items[nameof(mnuEditSubroutine)].Enabled = InteropEmu.DebugIsExecutionStopped() && Viewer.CodeViewer.CurrentLine >= 0;

			if(SourceView) {
				items[nameof(mnuMarkSelectionAs)].Visible = false;
				items[nameof(mnuShowCodeNotes)].Visible = false;

				items[nameof(mnuFindOccurrences)].Visible = false;
				items[nameof(mnuEditSubroutine)].Visible = false;
				items[nameof(mnuEditSelectedCode)].Visible = false;
				items[nameof(mnuNavigateForward)].Visible = false;
				items[nameof(mnuNavigateBackward)].Visible = false;
				items[nameof(mnuEditLabel)].Visible = false;
				items[nameof(sepNavigation)].Visible = false;
				items[nameof(mnuShowSourceAsComments)].Visible = false;
			}

			bool hasSymbolProvider = Viewer.SymbolProvider != null;
			items[nameof(mnuShowSourceAsComments)].Visible = hasSymbolProvider;
			items[nameof(mnuSwitchView)].Visible = hasSymbolProvider;
			items[nameof(sepSwitchView)].Visible = hasSymbolProvider;
		}

		private bool UpdateContextMenu(Point mouseLocation)
		{
			_lastLocation = mouseLocation;

			UpdateContextMenuItemVisibility(contextMenu.Items, true);

			mnuSwitchView.Text = SourceView ? "Switch to Disassembly View" : "Switch to Source View";

			string word = Viewer.CodeViewer.GetWordUnderLocation(mouseLocation);
			Ld65DbgImporter.SymbolInfo symbol = null;
			CodeLabel codeLabel = null;

			if(!word.StartsWith("$")) {
				codeLabel = LabelManager.GetLabel(word);

				if(Viewer.SymbolProvider != null) {
					int rangeStart, rangeEnd;
					if(Viewer.CodeViewer.GetNoteRangeAtLocation(mouseLocation.Y, out rangeStart, out rangeEnd)) {
						symbol = Viewer.SymbolProvider.GetSymbol(word, rangeStart, rangeEnd);
						if(symbol?.SegmentID == null) {
							symbol = null;
						}
					}
				}
			}

			if(word.StartsWith("$") || codeLabel != null || symbol != null) {
				//Cursor is on a numeric value or label
				_lastWord = word;

				if(word.StartsWith("$")) {
					//CPU Address
					_lastClickedAddress = Int32.Parse(word.Substring(1), NumberStyles.AllowHexSpecifier);
					_lastClickedSymbol = null;
					_newWatchValue = "[$" + _lastClickedAddress.ToString("X") + "]";
				} else if(symbol != null) {
					//Symbol
					_lastClickedAddress = -1;
					_lastClickedSymbol = symbol;
					_newWatchValue = "[" + word + "]";
				} else if(codeLabel != null) {
					//Label
					_lastClickedAddress = (Int32)InteropEmu.DebugGetRelativeAddress(codeLabel.Address, codeLabel.AddressType);
					_lastClickedSymbol = null;
					_newWatchValue = "[" + word + "]";
				}

				mnuGoToLocation.Enabled = true;
				mnuGoToLocation.Text = $"Go to Location ({word})";

				mnuShowInSplitView.Enabled = true;
				mnuShowInSplitView.Text = $"Show in Split View ({word})";

				mnuAddToWatch.Enabled = true;
				mnuAddToWatch.Text = $"Add to Watch ({word})";

				mnuFindOccurrences.Enabled = true;
				mnuFindOccurrences.Text = $"Find Occurrences ({word})";

				mnuEditLabel.Enabled = true;
				mnuEditLabel.Text = $"Edit Label ({word})";

				mnuEditInMemoryViewer.Enabled = true;
				mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer ({word})";

				return true;
			} else {
				mnuGoToLocation.Enabled = false;
				mnuGoToLocation.Text = "Go to Location";
				mnuShowInSplitView.Enabled = false;
				mnuShowInSplitView.Text = "Show in Split View";
				mnuAddToWatch.Enabled = false;
				mnuAddToWatch.Text = "Add to Watch";
				mnuFindOccurrences.Enabled = false;
				mnuFindOccurrences.Text = "Find Occurrences";
				mnuEditLabel.Enabled = false;
				mnuEditLabel.Text = "Edit Label";
				mnuEditInMemoryViewer.Enabled = false;
				mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer";

				_lastClickedSymbol = null;
				if(mouseLocation.X < Viewer.CodeViewer.CodeMargin) {
					_lastClickedAddress = Viewer.CodeViewer.GetLineNumberAtPosition(mouseLocation.Y);
				} else {
					_lastClickedAddress = Viewer.CodeViewer.LastSelectedLine;
				}

				if(_lastClickedAddress >= 0) {
					//Cursor is in the margin, over an address label					
					string address = $"${_lastClickedAddress.ToString("X4")}";
					_newWatchValue = $"[{address}]";
					_lastWord = address;

					mnuShowInSplitView.Enabled = true;
					mnuShowInSplitView.Text = $"Show in Split View ({address})";
					mnuAddToWatch.Enabled = true;
					mnuAddToWatch.Text = $"Add to Watch ({address})";
					mnuFindOccurrences.Enabled = true;
					mnuFindOccurrences.Text = $"Find Occurrences ({address})";
					mnuEditLabel.Enabled = true;
					mnuEditLabel.Text = $"Edit Label ({address})";
					mnuEditInMemoryViewer.Enabled = true;
					mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer ({address})";
					return true;
				}

				return false;
			}
		}
	}
}
