using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Debugger.Controls;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlDebuggerCode : BaseScrollableTextboxUserControl
	{
		public delegate void AddressEventHandler(AddressEventArgs args);
		public delegate void WatchEventHandler(WatchEventArgs args);
		public event WatchEventHandler OnWatchAdded;
		public event AddressEventHandler OnSetNextStatement;
		private DebugViewInfo _config;
		private HashSet<int> _unexecutedAddresses = new HashSet<int>();
		private HashSet<int> _speculativeCodeAddreses = new HashSet<int>();
		private Color _unexecutedColor = Color.FromArgb(183, 229, 190);
		private Color _speculativeColor = Color.FromArgb(240, 220, 220);

		private frmCodeTooltip _codeTooltip = null;

		public ctrlDebuggerCode()
		{
			InitializeComponent();
			this.lstSearchResult.Font = new System.Drawing.Font(BaseControl.MonospaceFontFamily, 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));			
			splitContainer.Panel2Collapsed = true;
		}

		public void SetConfig(DebugViewInfo config)
		{
			_config = config;

			mnuPrgAddressReplace.Checked = false;
			mnuPrgAddressBelow.Checked = false;
			mnuHidePrgAddresses.Checked = false;

			mnuShowByteCodeOnLeft.Checked = false;
			mnuShowByteCodeBelow.Checked = false;
			mnuHideByteCode.Checked = false;

			switch(config.ByteCodePosition) {
				case ByteCodePosition.Left:
					this.ctrlCodeViewer.ShowContentNotes = true;
					this.ctrlCodeViewer.ShowSingleContentLineNotes = true;
					this.mnuShowByteCodeOnLeft.Checked = true;
					break;

				case ByteCodePosition.Below:
					this.ctrlCodeViewer.ShowContentNotes = true;
					this.ctrlCodeViewer.ShowSingleContentLineNotes = false;
					this.mnuShowByteCodeBelow.Checked = true;
					break;

				case ByteCodePosition.Hidden:
					this.ctrlCodeViewer.ShowContentNotes = false;
					this.ctrlCodeViewer.ShowSingleContentLineNotes = false;
					this.mnuHideByteCode.Checked = true;
					break;
			}

			switch(config.PrgAddressPosition) {
				case PrgAddressPosition.Replace:
					this.ctrlCodeViewer.ShowLineNumberNotes = true;
					this.ctrlCodeViewer.ShowSingleLineLineNumberNotes = true;
					this.mnuPrgAddressReplace.Checked = true;
					break;

				case PrgAddressPosition.Below:
					this.ctrlCodeViewer.ShowLineNumberNotes = true;
					this.ctrlCodeViewer.ShowSingleLineLineNumberNotes = false;
					this.mnuPrgAddressBelow.Checked = true;
					break;

				case PrgAddressPosition.Hidden:
					this.ctrlCodeViewer.ShowLineNumberNotes = false;
					this.ctrlCodeViewer.ShowSingleLineLineNumberNotes = false;
					this.mnuHidePrgAddresses.Checked = true;
					break;
			}

			if(this.FontSize != config.FontSize) {
				this.FontSize = config.FontSize;
			}
		}

		private void UpdateConfig()
		{
			this.SetConfig(_config);
			ConfigManager.ApplyChanges();
		}

		public override float FontSize
		{
			get { return base.FontSize; }
			set { base.FontSize=value; UpdateConfig(); }
		}

		protected override ctrlScrollableTextbox ScrollableTextbox
		{
			get { return this.ctrlCodeViewer; }
		}

		private string _code;
		private bool _codeChanged;
		public string Code
		{
			get { return _code; }
			set
			{
				if(value != _code) {
					_codeChanged = true;
					_code = value;
					UpdateCode();
				}
			}
		}

		private UInt32? _currentActiveAddress = null;
		public void SelectActiveAddress(UInt32 address)
		{
			this.SetActiveAddress(address);
			this.ctrlCodeViewer.ScrollToLineNumber((int)address, eHistoryType.OnScroll);
		}

		public void SetActiveAddress(UInt32 address)
		{
			_currentActiveAddress = address;
		}
		
		public void ClearActiveAddress()
		{
			_currentActiveAddress = null;
		}

		public void UpdateLineColors()
		{
			this.ctrlCodeViewer.BeginUpdate();

			this.ctrlCodeViewer.ClearLineStyles();

			if(_currentActiveAddress.HasValue) {
				this.ctrlCodeViewer.SetLineColor((int)_currentActiveAddress, Color.Black, Color.Yellow, null, LineSymbol.Arrow);
			}

			if(ConfigManager.Config.DebugInfo.HighlightUnexecutedCode) {
				foreach(int relativeAddress in _unexecutedAddresses) {
					this.ctrlCodeViewer.SetLineColor(relativeAddress, null, _unexecutedColor);
				}
			}

			foreach(int relativeAddress in _speculativeCodeAddreses) {
				this.ctrlCodeViewer.SetLineColor(relativeAddress, null, _speculativeColor);
			}			

			this.HighlightBreakpoints();

			this.ctrlCodeViewer.EndUpdate();
		}

		public bool UpdateCode(bool forceUpdate = false)
		{
			if(_codeChanged || forceUpdate) {
				List<int> lineNumbers = new List<int>();
				List<string> lineNumberNotes = new List<string>();
				List<string> codeNotes = new List<string>();
				List<string> codeLines = new List<string>();
				_unexecutedAddresses = new HashSet<int>();
				_speculativeCodeAddreses = new HashSet<int>();
				
				int index = -1;
				int previousIndex = -1;
				while((index = _code.IndexOf('\n', index + 1)) >= 0) {
					string line = _code.Substring(previousIndex + 1, index - previousIndex - 1);
					string[] lineParts = line.Split('\x1');

					if(lineParts.Length >= 5) {
						int relativeAddress = ParseHexAddress(lineParts[1]);

						if(lineParts[0] == "0" && lineParts[4].StartsWith("  ")) {
							_unexecutedAddresses.Add(relativeAddress);
						} else if(lineParts[0] == "2" && lineParts[4].StartsWith("  ")) {
							_speculativeCodeAddreses.Add(relativeAddress);
						}

						lineNumbers.Add(relativeAddress);
						lineNumberNotes.Add(string.IsNullOrWhiteSpace(lineParts[2]) ? "" : lineParts[2].TrimStart('0').PadLeft(4, '0'));
						codeNotes.Add(lineParts[3]);
						codeLines.Add(lineParts[4]);
					}

					previousIndex = index;
				}

				ctrlCodeViewer.TextLines = codeLines.ToArray();
				ctrlCodeViewer.LineNumbers = lineNumbers.ToArray();
				ctrlCodeViewer.TextLineNotes = codeNotes.ToArray();
				ctrlCodeViewer.LineNumberNotes = lineNumberNotes.ToArray();

				_codeChanged = false;
				UpdateLineColors();
				return true;
			}
			UpdateLineColors();
			return false;
		}

		private int ParseHexAddress(string hexAddress)
		{
			if(string.IsNullOrWhiteSpace(hexAddress)) {
				return -1;
			} else {
				return (int)UInt32.Parse(hexAddress, System.Globalization.NumberStyles.AllowHexSpecifier);
			}
		}

		private void HighlightBreakpoints()
		{
			foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
				Color? fgColor = Color.White;
				Color? bgColor = null;
				Color? outlineColor = Color.FromArgb(140, 40, 40);
				LineSymbol symbol;
				if(breakpoint.Enabled) {
					bgColor = Color.FromArgb(140, 40, 40);
					symbol = LineSymbol.Circle;
				} else {
					fgColor = Color.Black;
					symbol = LineSymbol.CircleOutline;
				}

				if(breakpoint.Address == (UInt32)(_currentActiveAddress.HasValue ? (int)_currentActiveAddress.Value : -1)) {
					fgColor = Color.Black;
					bgColor = Color.Yellow;
					symbol |= LineSymbol.Arrow;
				} else if(_unexecutedAddresses.Contains((Int32)breakpoint.Address)) {
					fgColor = Color.Black;
					bgColor = _unexecutedColor;
				} else if(_speculativeCodeAddreses.Contains((Int32)breakpoint.Address)) {
					fgColor = Color.Black;
					bgColor = _speculativeColor;
				}

				ctrlCodeViewer.SetLineColor((int)breakpoint.Address, fgColor, bgColor, outlineColor, symbol);
			}
		}

		#region Events
		private Point _previousLocation;
		private bool _preventCloseTooltip = false;
		private string _hoverLastWord = "";

		private void ShowTooltip(string word, Dictionary<string, string> values)
		{
			if(_hoverLastWord != word || _codeTooltip == null) {
				if(!_preventCloseTooltip && _codeTooltip != null) {
					_codeTooltip.Close();
					_codeTooltip = null;
				}
				_codeTooltip = new frmCodeTooltip(values);
				_codeTooltip.Left = Cursor.Position.X + 10;
				_codeTooltip.Top = Cursor.Position.Y + 10;
				_codeTooltip.Show(this);
			}
			_codeTooltip.Left = Cursor.Position.X + 10;
			_codeTooltip.Top = Cursor.Position.Y + 10;

			_preventCloseTooltip = true;
			_hoverLastWord = word;
		}
		
		private void ctrlCodeViewer_MouseLeave(object sender, EventArgs e)
		{
			if(_codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}
		}

		private void ctrlCodeViewer_MouseMove(object sender, MouseEventArgs e)
		{
			//Always enable to allow F2 shortcut
			mnuEditLabel.Enabled = true;

			if(e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				this.ctrlCodeViewer.ContextMenuStrip = contextMenuMargin;
			} else {
				this.ctrlCodeViewer.ContextMenuStrip = contextMenuCode;
			}

			if(_previousLocation != e.Location) {
				if(!_preventCloseTooltip && _codeTooltip != null) {
					_codeTooltip.Close();
					_codeTooltip = null;
				}
				_preventCloseTooltip = false;

				string word = GetWordUnderLocation(e.Location);
				if(word.StartsWith("$")) {
					try {
						UInt32 address = UInt32.Parse(word.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);

						AddressTypeInfo info = new AddressTypeInfo();
						InteropEmu.DebugGetAbsoluteAddressAndType(address, ref info);

						if(info.Address >= 0) {
							CodeLabel label = LabelManager.GetLabel((UInt32)info.Address, info.Type);
							if(label == null) {
								DisplayAddressTooltip(word, address);
							} else {
								DisplayLabelTooltip(word, label);
							}
						} else {
							DisplayAddressTooltip(word, address);
						}
					} catch { }
				} else {
					CodeLabel label = LabelManager.GetLabel(word);

					if(label != null) {
						DisplayLabelTooltip(word, label);
					}
				}
				_previousLocation = e.Location;
			}
		}

		private void DisplayAddressTooltip(string word, UInt32 address)
		{
			byte byteValue = InteropEmu.DebugGetMemoryValue(address);
			UInt16 wordValue = (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(address+1) << 8));

			var values = new Dictionary<string, string>() {
								{ "Address", "$" + address.ToString("X4") },
								{ "Value", $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" }
							};

			ShowTooltip(word, values);
		}

		private void DisplayLabelTooltip(string word, CodeLabel label)
		{
			Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue((UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue((UInt32)relativeAddress+1) << 8)) : (UInt16)0;
			var values = new Dictionary<string, string>() {
							{ "Label", label.Label },
							{ "Address", "$" + InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType).ToString("X4") },
							{ "Value", (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a") },
						};

			if(!string.IsNullOrWhiteSpace(label.Comment)) {
				values["Comment"] = label.Comment;
			}

			ShowTooltip(word, values);
		}

		private bool UpdateContextMenu(Point mouseLocation)
		{
			string word = GetWordUnderLocation(mouseLocation);
			if(word.StartsWith("$") || LabelManager.GetLabel(word) != null) {
				//Cursor is on a numeric value or label
				_lastWord = word;

				if(word.StartsWith("$")) {
					_lastClickedAddress = Int32.Parse(word.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);
					_newWatchValue = "[$" + _lastClickedAddress.ToString("X") + "]";
				} else {
					_lastClickedAddress = (Int32)InteropEmu.DebugGetRelativeAddress(LabelManager.GetLabel(word).Address, LabelManager.GetLabel(word).AddressType);
					_newWatchValue = "[" + word + "]";
				}

				mnuGoToLocation.Enabled = true;
				mnuGoToLocation.Text = $"Go to Location ({word})";

				mnuAddToWatch.Enabled = true;
				mnuAddToWatch.Text = $"Add to Watch ({word})";

				mnuFindOccurrences.Enabled = true;
				mnuFindOccurrences.Text = $"Find Occurrences ({word})";

				mnuEditLabel.Enabled = true;
				mnuEditLabel.Text = $"Edit Label ({word})";

				return true;
			} else {
				mnuGoToLocation.Enabled = false;
				mnuGoToLocation.Text = "Go to Location";
				mnuAddToWatch.Enabled = false;
				mnuAddToWatch.Text = "Add to Watch";
				mnuFindOccurrences.Enabled = false;
				mnuFindOccurrences.Text = "Find Occurrences";
				mnuEditLabel.Enabled = false;
				mnuEditLabel.Text = "Edit Label";

				_lastClickedAddress = ctrlCodeViewer.GetLineNumberAtPosition(mouseLocation.Y);
				if(mouseLocation.X < this.ctrlCodeViewer.CodeMargin && _lastClickedAddress >= 0) {
					//Cursor is in the margin, over an address label					
					string address = $"${_lastClickedAddress.ToString("X4")}";
					_newWatchValue = $"[{address}]";
					_lastWord = address;

					mnuAddToWatch.Enabled = true;
					mnuAddToWatch.Text = $"Add to Watch ({address})";
					mnuFindOccurrences.Enabled = true;
					mnuFindOccurrences.Text = $"Find Occurrences ({address})";
					mnuEditLabel.Enabled = true;
					mnuEditLabel.Text = $"Edit Label ({address})";

					return true;
				}

				return false;
			}
		}

		int _lastClickedAddress = Int32.MaxValue;
		string _newWatchValue = string.Empty;
		string _lastWord = string.Empty;
		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			if(UpdateContextMenu(e.Location)) {
				if(e.Button == MouseButtons.Left) {
					if(ModifierKeys.HasFlag(Keys.Control)) {
						GoToLocation();
					} else if(ModifierKeys.HasFlag(Keys.Shift)) {
						AddWatch();
					} else if(ModifierKeys.HasFlag(Keys.Alt)) {
						FindAllOccurrences(_lastWord, true, true);
					}
				}
			}
		}

		Breakpoint _lineBreakpoint = null;
		private void ctrlCodeViewer_MouseDown(object sender, MouseEventArgs e)
		{
			if(_codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}

			int address = ctrlCodeViewer.GetLineNumberAtPosition(e.Y);
			_lineBreakpoint = BreakpointManager.GetMatchingBreakpoint(address);

			if(e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				if(e.Button == System.Windows.Forms.MouseButtons.Left) {
					if(_lineBreakpoint == null) {
						Breakpoint bp = new Breakpoint();
						bp.Address = (UInt32)address;
						bp.BreakOnExec = true;
						BreakpointManager.AddBreakpoint(bp);
					} else {
						BreakpointManager.RemoveBreakpoint(_lineBreakpoint);
					}
				}
			}
		}

		private void ctrlCodeViewer_ScrollPositionChanged(object sender, EventArgs e)
		{
			if(_codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}
		}

		private void ctrlCodeViewer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int relativeAddress = ctrlCodeViewer.GetLineNumberAtPosition(e.Y);

			if(relativeAddress >= 0 && e.Location.X > this.ctrlCodeViewer.CodeMargin / 2 && e.Location.X < this.ctrlCodeViewer.CodeMargin) {
				AddressTypeInfo info = new AddressTypeInfo();
				InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)relativeAddress, ref info);

				if(info.Address >= 0) {
					ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
				}
			}
		}

		#region Context Menu

		private void contextMenuMargin_Opening(object sender, CancelEventArgs e)
		{
			if(_lineBreakpoint == null) {
				e.Cancel = true;
			} else {
				mnuDisableBreakpoint.Text = _lineBreakpoint.Enabled ? "Disable breakpoint" : "Enable breakpoint";
			}
		}

		private void mnuRemoveBreakpoint_Click(object sender, EventArgs e)
		{
			BreakpointManager.RemoveBreakpoint(_lineBreakpoint);
		}

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			BreakpointManager.EditBreakpoint(_lineBreakpoint);
		}

		private void mnuDisableBreakpoint_Click(object sender, EventArgs e)
		{
			_lineBreakpoint.SetEnabled(!_lineBreakpoint.Enabled);
		}

		private void contextMenuCode_Opening(object sender, CancelEventArgs e)
		{
			mnuShowNextStatement.Enabled = _currentActiveAddress.HasValue;
			mnuSetNextStatement.Enabled = _currentActiveAddress.HasValue;
		}

		private void mnuShowNextStatement_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.ScrollToLineNumber((int)_currentActiveAddress.Value);
		}

		private void mnuShowOnlyDisassembledCode_Click(object sender, EventArgs e)
		{
			UpdateCode(true);
			if(_currentActiveAddress.HasValue) {
				SelectActiveAddress(_currentActiveAddress.Value);
			}
			this.UpdateConfig();
		}
		
		private void mnuShowLineNotes_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.ShowLineNumberNotes = this.mnuShowLineNotes.Checked;
			this.UpdateConfig();
		}
		
		private void mnuGoToLocation_Click(object sender, EventArgs e)
		{
			GoToLocation();
		}

		private void mnuFindOccurrences_Click(object sender, EventArgs e)
		{
			this.FindAllOccurrences(_lastWord, true, true);
		}

		public void FindAllOccurrences(string text, bool matchWholeWord, bool matchCase)
		{
			this.lstSearchResult.Items.Clear();
			foreach(Tuple<int, int, string> searchResult in this.ctrlCodeViewer.FindAllOccurrences(text, matchWholeWord, matchCase)) {
				var item = this.lstSearchResult.Items.Add(searchResult.Item1.ToString("X4"));
				item.Tag = searchResult.Item2;
				item.SubItems.Add(searchResult.Item3);
				item.SubItems.Add("");
			}
			this.lblSearchResult.Text = $"Search results for: {text} ({this.lstSearchResult.Items.Count} results)";
			this.lstSearchResult.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			this.lstSearchResult.Columns[0].Width += 20;
			this.lstSearchResult.Columns[1].Width = Math.Max(this.lstSearchResult.Columns[1].Width, this.lstSearchResult.Width - this.lstSearchResult.Columns[0].Width - 24);
			this.splitContainer.Panel2Collapsed = false;
		}
		
		private void lstSearchResult_SizeChanged(object sender, EventArgs e)
		{
			this.lstSearchResult.SizeChanged -= lstSearchResult_SizeChanged;
			this.lstSearchResult.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			this.lstSearchResult.Columns[1].Width = Math.Max(this.lstSearchResult.Columns[1].Width, this.lstSearchResult.Width - this.lstSearchResult.Columns[0].Width - 24);
			this.lstSearchResult.SizeChanged += lstSearchResult_SizeChanged;
		}

		private void picCloseOccurrenceList_Click(object sender, EventArgs e)
		{
			this.splitContainer.Panel2Collapsed = true;
		}
		
		private void lstSearchResult_DoubleClick(object sender, EventArgs e)
		{
			if(lstSearchResult.SelectedItems.Count > 0) {
				int lineIndex = (int)lstSearchResult.SelectedItems[0].Tag;
				this.ctrlCodeViewer.ScrollToLineIndex(lineIndex);
			}
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			AddWatch();
		}

		private void GoToLocation()
		{
			this.ctrlCodeViewer.ScrollToLineNumber((int)_lastClickedAddress);
		}

		private void AddWatch()
		{
			if(this.OnWatchAdded != null) {
				this.OnWatchAdded(new WatchEventArgs() { WatchValue = _newWatchValue });
			}
		}

		private void mnuSetNextStatement_Click(object sender, EventArgs e)
		{
			if(this.OnSetNextStatement != null) {
				this.OnSetNextStatement(new AddressEventArgs() { Address = (UInt32)this.ctrlCodeViewer.CurrentLine });
			}
		}

		private void ctrlCodeViewer_FontSizeChanged(object sender, EventArgs e)
		{
			_config.FontSize = this.FontSize;
			UpdateConfig();
		}

		private void mnuEditLabel_Click(object sender, EventArgs e)
		{
			if(UpdateContextMenu(_previousLocation)) {
				AddressTypeInfo info = new AddressTypeInfo();
				InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)_lastClickedAddress, ref info);
				if(info.Address >= 0) {
					ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
				}
			}
		}

		private void mnuNavigateForward_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.NavigateForward();
		}

		private void mnuNavigateBackward_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.NavigateBackward();
		}

		private void mnuToggleBreakpoint_Click(object sender, EventArgs e)
		{
			BreakpointManager.ToggleBreakpoint(this.ctrlCodeViewer.CurrentLine, false);
		}

		#endregion

		#endregion

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
	}

	public class WatchEventArgs : EventArgs
	{
		public string WatchValue { get; set; }
	}

	public class AddressEventArgs : EventArgs
	{
		public UInt32 Address { get; set; }
	}
}
