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
		public delegate void AssemblerEventHandler(AssemblerEventArgs args);
		public event AssemblerEventHandler OnEditCode;
		public event AddressEventHandler OnSetNextStatement;
		private DebugViewInfo _config;

		List<int> _lineNumbers = new List<int>(10000);
		List<string> _lineNumberNotes = new List<string>(10000);
		List<string> _codeNotes = new List<string>(10000);
		List<string> _codeLines = new List<string>(10000);
		private HashSet<int> _unexecutedAddresses = new HashSet<int>();
		private HashSet<int> _speculativeCodeAddreses = new HashSet<int>();
		Dictionary<int, string> _codeContent = new Dictionary<int, string>(10000);
		Dictionary<int, string> _codeComments = new Dictionary<int, string>(10000);
		Dictionary<int, string> _codeByteCode = new Dictionary<int, string>(10000);
		List<string> _addressing = new List<string>(10000);
		List<string> _comments = new List<string>(10000);
		List<int> _lineIndentations = new List<int>(10000);

		private UInt32? _currentActiveAddress { get; set; } = null;

		private frmCodeTooltip _codeTooltip = null;

		public ctrlDebuggerCode()
		{
			InitializeComponent();
			this.lstSearchResult.Font = new System.Drawing.Font(BaseControl.MonospaceFontFamily, 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));			
			splitContainer.Panel2Collapsed = true;
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<ToolStripItem> ContextMenuItems
		{
			get
			{
				List<ToolStripItem> items = new List<ToolStripItem>();
				foreach(ToolStripItem item in this.contextMenuCode.Items) {
					items.Add(item);
				}
				return items;
			}

			set
			{
				this.contextMenuCode.Items.AddRange(value.ToArray());
			}
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
				if(value != null) {
					_codeChanged = true;
					_code = value;
					UpdateCode();
				}
			}
		}

		public void SelectActiveAddress(UInt32 address)
		{
			this.SetActiveAddress(address);
			this.ctrlCodeViewer.ScrollToLineNumber((int)address, eHistoryType.OnScroll);
		}

		public void SetActiveAddress(UInt32 address)
		{
			_currentActiveAddress = address;
			this.UpdateLineColors();
		}
		
		public void ClearActiveAddress()
		{
			_currentActiveAddress = null;
			this.UpdateLineColors();
		}

		public void UpdateLineColors()
		{
			this.ctrlCodeViewer.StyleProvider = new LineStyleProvider(this);
		}

		public List<string> GetCode(out int byteLength, ref int startAddress, int endAddress = -1)
		{
			List<string> result = new List<string>();
			byteLength = 0;

			if(endAddress == -1) {
				//When no end address is specified, find the start of the function based on startAddress
				int address = InteropEmu.DebugFindSubEntryPoint((UInt16)startAddress);
				if(address != -1) {
					startAddress = address;
				}
			}

			for(int i = startAddress; (i <= endAddress || endAddress == -1) && endAddress < 65536; ) {
				string code;
				if(_codeContent.TryGetValue(i, out code)) {
					code = code.Split('\x2')[0].Trim();

					if(code.StartsWith("--") || code.StartsWith("__")) {
						//Stop adding code when we find a new section (new function, data blocks, etc.)
						break;
					}

					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)i, ref info);
					CodeLabel codeLabel = info.Address >= 0 ? LabelManager.GetLabel((UInt32)info.Address, AddressType.PrgRom) : null;
					string comment = codeLabel?.Comment;
					string label = codeLabel?.Label;

					bool addPadding = true;
					if(code.StartsWith("STP*") || code.StartsWith("NOP*")) {
						//Transform unofficial opcodes that can't be reassembled properly into .byte statements
						if(comment != null) {
							comment.Insert(0, code + " - ");
						} else {
							comment = code;
						}
						code = ".byte " + string.Join(",", _codeByteCode[i].Split(' '));
						addPadding = false;
					}

					if(!string.IsNullOrWhiteSpace(comment) && comment.Contains("\n")) {
						result.AddRange(comment.Replace("\r", "").Split('\n').Select(cmt => ";" + cmt));
						comment = null;
					}
					if(!string.IsNullOrWhiteSpace(label)) {
						result.Add(label + ":");
					}
					result.Add((addPadding ? "  " : "") + code + (!string.IsNullOrWhiteSpace(comment) ? (" ;" + comment) : ""));
					
					int length = _codeByteCode[i].Count(c => c == ' ') + 1;
					byteLength += length;
					i += length;

					if(endAddress == -1 && (string.Compare(code, "RTI", true) == 0 || string.Compare(code, "RTS", true) == 0)) {
						break;
					}
				} else {
					break;
				}
			}

			return result;
		}

		public bool UpdateCode(bool forceUpdate = false)
		{
			if(_codeChanged || forceUpdate) {
				_codeContent.Clear();
				_codeComments.Clear();
				_codeByteCode.Clear();
				_unexecutedAddresses.Clear();
				_speculativeCodeAddreses.Clear();

				string[] token = new string[7];
				int tokenIndex = 0;
				int startPos = 0;
				int endPos = 0;

				Action readToken = () => {
					endPos = _code.IndexOf('\x1', endPos) + 1;
					token[tokenIndex++] = _code.Substring(startPos, endPos - startPos - 1);
					startPos = endPos;
				};
			
				Action readLine = () => {
					tokenIndex = 0;
					readToken(); readToken(); readToken(); readToken(); readToken();  readToken();  readToken();
				};

				Func<bool> processLine = () => {
					readLine();

					int relativeAddress = ParseHexAddress(token[1]);

					//Flags:
					//1: Executed code
					//2: Speculative Code
					//4: Indented line
					if(token[0] == "4") {
						_unexecutedAddresses.Add(relativeAddress);
						_lineIndentations.Add(20);
					} else if(token[0] == "6") {
						_speculativeCodeAddreses.Add(relativeAddress);
						_lineIndentations.Add(20);
					} else if(token[0] == "5") {
						_lineIndentations.Add(20);
					} else {
						_lineIndentations.Add(0);
					}

					_lineNumbers.Add(relativeAddress);
					_lineNumberNotes.Add(string.IsNullOrWhiteSpace(token[2]) ? "" : (token[2].Length > 5 ? token[2].TrimStart('0').PadLeft(4, '0') : token[2]));
					_codeNotes.Add(token[3]);
					_codeLines.Add(token[4]);

					_addressing.Add(token[5]);
					_comments.Add(token[6]);

					//Used by assembler
					_codeByteCode[relativeAddress] = token[3];
					_codeContent[relativeAddress] = token[4];
					_codeComments[relativeAddress] = token[6];

					return endPos < _code.Length;
				};

				while(processLine());

				ctrlCodeViewer.LineIndentations = _lineIndentations.ToArray();
				ctrlCodeViewer.Addressing = _addressing.ToArray();
				ctrlCodeViewer.Comments = _comments.ToArray();

				ctrlCodeViewer.LineNumbers = _lineNumbers.ToArray();
				ctrlCodeViewer.TextLineNotes = _codeNotes.ToArray();
				ctrlCodeViewer.LineNumberNotes = _lineNumberNotes.ToArray();
				ctrlCodeViewer.TextLines = _codeLines.ToArray();

				//These are all temporary and can be cleared right away
				_lineNumbers.Clear();
				_lineNumberNotes.Clear();
				_codeNotes.Clear();
				_codeLines.Clear();
				_addressing.Clear();
				_comments.Clear();
				_lineIndentations.Clear();

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
			byte byteValue = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address);
			UInt16 wordValue = (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address+1) << 8));

			var values = new Dictionary<string, string>() {
								{ "Address", "$" + address.ToString("X4") },
								{ "Value", $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" }
							};

			ShowTooltip(word, values);
		}

		private void DisplayLabelTooltip(string word, CodeLabel label)
		{
			Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress+1) << 8)) : (UInt16)0;
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
			UpdateContextMenuItemVisibility(true);

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

				mnuEditInMemoryViewer.Enabled = true;
				mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer ({word})";

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
				mnuEditInMemoryViewer.Enabled = false;
				mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer";


				if(mouseLocation.X < this.ctrlCodeViewer.CodeMargin) {
					_lastClickedAddress = ctrlCodeViewer.GetLineNumberAtPosition(mouseLocation.Y);
				} else {
					_lastClickedAddress = ctrlCodeViewer.LastSelectedLine;
				}

				if(_lastClickedAddress >= 0) {
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
					mnuEditInMemoryViewer.Enabled = true;
					mnuEditInMemoryViewer.Text = $"Edit in Memory Viewer ({address})";
					return true;
				}

				return false;
			}
		}

		public void UpdateContextMenuItemVisibility(bool visible)
		{
			mnuShowNextStatement.Enabled = _currentActiveAddress.HasValue;
			mnuSetNextStatement.Enabled = _currentActiveAddress.HasValue;
			mnuEditSelectedCode.Enabled = mnuEditSubroutine.Enabled = InteropEmu.DebugIsExecutionStopped() && ctrlCodeViewer.CurrentLine >= 0;

			mnuAddToWatch.Visible = visible;
			mnuFindOccurrences.Visible = visible;
			mnuEditLabel.Visible = visible;
			mnuGoToLocation.Visible = visible;
			mnuToggleBreakpoint.Visible = visible;
			sepAddToWatch.Visible = visible;
			sepEditLabel.Visible = visible;
		}

		int _lastClickedAddress = Int32.MaxValue;
		string _newWatchValue = string.Empty;
		string _lastWord = string.Empty;
		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			if(UpdateContextMenu(e.Location)) {
				if(e.Button == MouseButtons.Left) {
					if(ModifierKeys.HasFlag(Keys.Control)) {
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

			if(e.Button == MouseButtons.Left && e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				BreakpointManager.ToggleBreakpoint(address, false);
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
			if(e.Location.X > this.ctrlCodeViewer.CodeMargin / 2 && e.Location.X < this.ctrlCodeViewer.CodeMargin) {
				int relativeAddress = ctrlCodeViewer.GetLineNumberAtPosition(e.Y);
				if(relativeAddress >= 0) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)relativeAddress, ref info);

					if(info.Address >= 0) {
						ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
					}
				}
			} else if(UpdateContextMenu(e.Location) && mnuGoToLocation.Enabled) {
				GoToLocation();
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
			UpdateContextMenuItemVisibility(true);
		}
		
		private void contextMenuCode_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			mnuEditSelectedCode.Enabled = true;
			mnuEditSubroutine.Enabled = true;
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
			WatchManager.AddWatch(_newWatchValue);
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

		private void mnuEditSubroutine_Click(object sender, EventArgs e)
		{
			int currentLine = this.GetCurrentLine();
			if(currentLine != -1 && InteropEmu.DebugIsExecutionStopped()) {
				int byteLength;
				List<string> code = this.GetCode(out byteLength, ref currentLine);
				this.OnEditCode?.Invoke(new AssemblerEventArgs() { Code = string.Join(Environment.NewLine, code), StartAddress = (UInt16)currentLine, BlockLength = (UInt16)byteLength });
			}
		}

		private void mnuEditSelectedCode_Click(object sender, EventArgs e)
		{
			int startAddress = this.GetCurrentLine();
			int endAddress = this.ctrlCodeViewer.LastSelectedLine;
			if(startAddress != -1 && InteropEmu.DebugIsExecutionStopped()) {
				int byteLength;
				List<string> code = this.GetCode(out byteLength, ref startAddress, endAddress);
				this.OnEditCode?.Invoke(new AssemblerEventArgs() { Code = string.Join(Environment.NewLine, code), StartAddress = (UInt16)startAddress, BlockLength = (UInt16)byteLength });
			}
		}


		class LineStyleProvider : ctrlTextbox.ILineStyleProvider
		{
			private Color _unexecutedColor = Color.FromArgb(183, 229, 190);
			private Color _speculativeColor = Color.FromArgb(240, 220, 220);
			private Color _execBpColor = Color.FromArgb(140, 40, 40);
			private Color _writeBpColor = Color.FromArgb(40, 120, 80);
			private Color _readBpColor = Color.FromArgb(40, 40, 200);

			private ctrlDebuggerCode _code;

			public LineStyleProvider(ctrlDebuggerCode code)
			{
				_code = code;
			}

			public LineProperties GetLineStyle(int cpuAddress)
			{
				foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
					if(breakpoint.Matches(cpuAddress)) {
						Color? fgColor = Color.White;
						Color? bgColor = null;
						Color bpColor = breakpoint.BreakOnExec ? _execBpColor : (breakpoint.BreakOnWrite ? _writeBpColor : _readBpColor);
						Color? outlineColor = bpColor;
						LineSymbol symbol;
						if(breakpoint.Enabled) {
							bgColor = bpColor;
							symbol = LineSymbol.Circle;
						} else {
							fgColor = Color.Black;
							symbol = LineSymbol.CircleOutline;
						}

						if(_code._currentActiveAddress.HasValue && breakpoint.Matches((int)_code._currentActiveAddress.Value)) {
							fgColor = Color.Black;
							bgColor = Color.Yellow;
							symbol |= LineSymbol.Arrow;
						} else if(_code._unexecutedAddresses.Contains((Int32)breakpoint.Address)) {
							fgColor = Color.Black;
							bgColor =  _unexecutedColor;
						} else if(_code._speculativeCodeAddreses.Contains((Int32)breakpoint.Address)) {
							fgColor = Color.Black;
							bgColor =  _speculativeColor;
						}

						return new LineProperties() { FgColor = fgColor, BgColor = bgColor, OutlineColor = outlineColor, Symbol = symbol };
					}
				}

				if(_code._currentActiveAddress.HasValue && cpuAddress == _code._currentActiveAddress) {
					return new LineProperties() { FgColor = Color.Black, BgColor = Color.Yellow, OutlineColor = null, Symbol = LineSymbol.Arrow };
				} else if(ConfigManager.Config.DebugInfo.HighlightUnexecutedCode && _code._unexecutedAddresses.Contains(cpuAddress)) {
					return new LineProperties() { FgColor = null, BgColor = _unexecutedColor, OutlineColor = null, Symbol = LineSymbol.None };
				} else if(_code._speculativeCodeAddreses.Contains(cpuAddress)) {
					return new LineProperties() { FgColor = null, BgColor = _speculativeColor, OutlineColor = null, Symbol = LineSymbol.None };
				}
				return null;
			}
		}

		private void copySelectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.CopySelection();
		}

		private void mnuEditInMemoryViewer_Click(object sender, EventArgs e)
		{
			if(UpdateContextMenu(_previousLocation)) {
				DebugWindowManager.OpenMemoryViewer(_lastClickedAddress);
			}
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

	public class AssemblerEventArgs : EventArgs
	{
		public string Code { get; set; }
		public UInt16 StartAddress { get; set; }
		public UInt16 BlockLength { get; set; }
	}
}
