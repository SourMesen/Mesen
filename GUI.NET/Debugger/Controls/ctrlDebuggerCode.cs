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
		public delegate void AddressEventHandler(ctrlDebuggerCode sender, AddressEventArgs args);
		public delegate void WatchEventHandler(WatchEventArgs args);
		public delegate void AssemblerEventHandler(AssemblerEventArgs args);
		public event AssemblerEventHandler OnEditCode;
		public event AddressEventHandler OnSetNextStatement;
		public event AddressEventHandler OnScrollToAddress;
		private DebugViewInfo _config;

		List<int> _lineNumbers = new List<int>(10000);
		List<string> _lineNumberNotes = new List<string>(10000);
		List<char> _lineMemoryType = new List<char>(10000);
		List<int> _absoluteLineNumbers = new List<int>(10000);
		List<string> _codeNotes = new List<string>(10000);
		List<string> _codeLines = new List<string>(10000);
		private HashSet<int> _unexecutedAddresses = new HashSet<int>();
		private HashSet<int> _verifiedDataAddresses = new HashSet<int>();
		private HashSet<int> _speculativeCodeAddreses = new HashSet<int>();
		Dictionary<int, string> _codeContent = new Dictionary<int, string>(10000);
		Dictionary<int, string> _codeComments = new Dictionary<int, string>(10000);
		Dictionary<int, string> _codeByteCode = new Dictionary<int, string>(10000);
		List<string> _addressing = new List<string>(10000);
		List<string> _comments = new List<string>(10000);
		List<int> _lineIndentations = new List<int>(10000);

		private UInt32? _currentActiveAddress { get; set; } = null;

		private Form _codeTooltip = null;

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

			if(this.TextZoom != config.TextZoom) {
				this.TextZoom = config.TextZoom;
			}
		}

		private void UpdateConfig()
		{
			this.SetConfig(_config);
			ConfigManager.ApplyChanges();
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

		public bool ShowScrollbars
		{
			get { return this.ctrlCodeViewer.ShowScrollbars; }
			set { this.ctrlCodeViewer.ShowScrollbars = value; }
		}

		public bool ShowMemoryValues
		{
			get { return this.ctrlCodeViewer.ShowMemoryValues; }
			set { this.ctrlCodeViewer.ShowMemoryValues = value; }
		}

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
			this.ctrlCodeViewer.StyleProvider = new LineStyleProvider(this);
			if(this.ShowScrollbars) {
				this.ctrlCodeViewer.ScrollbarColorProvider = new ScrollbarColorProvider(this);
			}
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

			result.Add("");
			return result;
		}

		public bool UpdateCode(bool forceUpdate = false)
		{
			if(_codeChanged || forceUpdate) {
				int centerLineIndex = ctrlCodeViewer.GetLineIndexAtPosition(this.Height / 2);
				int centerLineAddress;
				do {
					//Save the address at the center of the debug view
					centerLineAddress = ctrlCodeViewer.GetLineNumber(centerLineIndex);
					centerLineIndex--;
				} while(centerLineAddress < 0 && centerLineIndex > 0);

				_lineNumbers.Clear();
				_codeContent.Clear();
				_codeComments.Clear();
				_codeByteCode.Clear();
				_unexecutedAddresses.Clear();
				_speculativeCodeAddreses.Clear();
				_verifiedDataAddresses.Clear();
				_lineMemoryType.Clear();
				_absoluteLineNumbers.Clear();

				string[] token = new string[8];
				int tokenIndex = 0;
				int startPos = 0;
				int endPos = 0;
				int lineNumber = 0;

				Action readToken = () => {
					endPos = _code.IndexOf('\x1', endPos) + 1;
					token[tokenIndex++] = _code.Substring(startPos, endPos - startPos - 1);
					startPos = endPos;
				};
			
				Action readLine = () => {
					tokenIndex = 0;
					readToken(); readToken(); readToken(); readToken(); readToken();  readToken(); readToken(); readToken();
				};

				Func<bool> processLine = () => {
					readLine();

					int relativeAddress = ParseHexAddress(token[1]);

					//Flags:
					//1: Executed code
					//2: Speculative Code
					//4: Indented line
					if(token[0] == "2") {
						_speculativeCodeAddreses.Add(lineNumber);
						_lineIndentations.Add(0);
					} else if(token[0] == "4") {
						_unexecutedAddresses.Add(lineNumber);
						_lineIndentations.Add(20);
					} else if(token[0] == "6") {
						_speculativeCodeAddreses.Add(lineNumber);
						_lineIndentations.Add(20);
					} else if(token[0] == "5") {
						_lineIndentations.Add(20);
					} else if(token[0] == "8") {
						_verifiedDataAddresses.Add(lineNumber);
						_lineIndentations.Add(0);
					} else if(token[0] == "9") {
						_verifiedDataAddresses.Add(lineNumber);
						_lineIndentations.Add(20);
					} else {
						_lineIndentations.Add(0);
					}

					_lineNumbers.Add(relativeAddress);
					_lineMemoryType.Add(token[2][0]);
					_lineNumberNotes.Add(string.IsNullOrWhiteSpace(token[3]) ? "" : (token[3].Length > 5 ? token[3].TrimStart('0').PadLeft(4, '0') : token[3]));
					_absoluteLineNumbers.Add(this.ParseHexAddress(token[3]));
					_codeNotes.Add(token[4]);
					_codeLines.Add(token[5]);

					_addressing.Add(token[6]);
					_comments.Add(token[7]);

					//Used by assembler
					_codeByteCode[relativeAddress] = token[4];
					_codeContent[relativeAddress] = token[5];
					_codeComments[relativeAddress] = token[7];

					lineNumber++;

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
				_lineNumberNotes.Clear();
				_codeNotes.Clear();
				_codeLines.Clear();
				_addressing.Clear();
				_comments.Clear();
				_lineIndentations.Clear();

				_codeChanged = false;
				UpdateLineColors();

				if(centerLineAddress >= 0) {
					//Scroll to the same address as before, to prevent the code view from changing due to setting or banking changes, etc.
					ctrlCodeViewer.ScrollToLineNumber(centerLineAddress, eHistoryType.None, false);
				}

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
		private int _hoverLastLineAddress = -1;

		private void ShowTooltip(string word, Dictionary<string, string> values, int address, int lineAddress)
		{
			if(_hoverLastWord != word || _hoverLastLineAddress != lineAddress || _codeTooltip == null) {
				if(!_preventCloseTooltip && _codeTooltip != null) {
					_codeTooltip.Close();
					_codeTooltip = null;
				}

				if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
					_codeTooltip = new frmOpCodeTooltip(word, lineAddress);
				} else {
					bool isPrgRom = false;
					if(address >= 0 && ConfigManager.Config.DebugInfo.ShowCodePreview) {
						AddressTypeInfo addressInfo = new AddressTypeInfo();
						InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)address, ref addressInfo);
						isPrgRom = addressInfo.Type == AddressType.PrgRom;
					}

					_codeTooltip = new frmCodeTooltip(values, isPrgRom ? address : -1, isPrgRom ? _code : null);
				}
				_codeTooltip.Left = Cursor.Position.X + 10;
				_codeTooltip.Top = Cursor.Position.Y + 10;
				_codeTooltip.Show(this);
			}
			_codeTooltip.Left = Cursor.Position.X + 10;
			_codeTooltip.Top = Cursor.Position.Y + 10;

			_preventCloseTooltip = true;
			_hoverLastWord = word;
			_hoverLastLineAddress = lineAddress;
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
					} else if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
						ShowTooltip(word, null, -1, ctrlCodeViewer.GetLineNumberAtPosition(e.Y));
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

			ShowTooltip(word, values, (int)address, -1);
		}

		private void DisplayLabelTooltip(string word, CodeLabel label)
		{
			Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress+1) << 8)) : (UInt16)0;
			int address = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
			var values = new Dictionary<string, string>() {
							{ "Label", label.Label },
							{ "Address", "$" + address.ToString("X4") },
							{ "Value", (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a") },
						};

			if(!string.IsNullOrWhiteSpace(label.Comment)) {
				values["Comment"] = label.Comment;
			}

			ShowTooltip(word, values, address, -1);
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

		protected override bool ProcessKeyMessage(ref Message m)
		{
			this.UpdateContextMenuItemVisibility(mnuAddToWatch.Visible);
			return base.ProcessKeyMessage(ref m);
		}

		public void UpdateContextMenuItemVisibility(bool visible)
		{
			mnuUndoPrgChrEdit.Enabled = InteropEmu.DebugHasUndoHistory();
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
					if(ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Alt)) {
						ShowInSplitView();
					} else if(ModifierKeys.HasFlag(Keys.Control)) {
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

			int relativeAddress = ctrlCodeViewer.GetLineNumberAtPosition(e.Y);
			AddressTypeInfo info = GetLineAddressTypeInfo(ctrlCodeViewer.GetLineIndexAtPosition(e.Y));
			_lineBreakpoint = BreakpointManager.GetMatchingBreakpoint(relativeAddress, info);

			if(e.Button == MouseButtons.Left && e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				BreakpointManager.ToggleBreakpoint(relativeAddress, info, false);
			}
		}

		private AddressTypeInfo GetLineAddressTypeInfo(int lineNumber)
		{
			AddressTypeInfo info = new AddressTypeInfo();
			info.Address = this._absoluteLineNumbers[lineNumber];
			switch(this._lineMemoryType[lineNumber]) {
				case 'P': info.Type = AddressType.PrgRom; break;
				case 'W': info.Type = AddressType.WorkRam; break;
				case 'S': info.Type = AddressType.SaveRam; break;
				case 'N': info.Type = AddressType.InternalRam; break;
			}
			return info;
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
				AddressTypeInfo info = GetLineAddressTypeInfo(ctrlCodeViewer.GetLineIndexAtPosition(e.Y));
				if(info.Address >= 0) {
					ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
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

		private void contextMenuCode_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			mnuEditSelectedCode.Enabled = true;
			mnuEditSubroutine.Enabled = true;
		}

		private void mnuShowNextStatement_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.ScrollToLineNumber((int)_currentActiveAddress.Value);
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

		private void mnuShowInSplitView_Click(object sender, EventArgs e)
		{
			ShowInSplitView();
		}

		private void ShowInSplitView()
		{
			this.OnScrollToAddress?.Invoke(this, new AddressEventArgs() { Address = (UInt32)_lastClickedAddress });
		}

		private void AddWatch()
		{
			WatchManager.AddWatch(_newWatchValue);
		}

		private void mnuSetNextStatement_Click(object sender, EventArgs e)
		{
			this.OnSetNextStatement?.Invoke(this, new AddressEventArgs() { Address = (UInt32)this.ctrlCodeViewer.CurrentLine });
		}

		private void ctrlCodeViewer_TextZoomChanged(object sender, EventArgs e)
		{
			_config.TextZoom = this.TextZoom;
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
			this.ToggleBreakpoint(false);
		}

		public void ToggleBreakpoint(bool toggleEnabledFlag)
		{
			int relativeAddress = ctrlCodeViewer.CurrentLine;
			AddressTypeInfo info = GetLineAddressTypeInfo(ctrlCodeViewer.SelectedLine);

			BreakpointManager.ToggleBreakpoint(relativeAddress, info, toggleEnabledFlag);
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

		private void GetSelectedAddressRange(out int start, out int end, out string range)
		{
			int firstLineOfSelection = this.ctrlCodeViewer.SelectionStart;
			while(this.ctrlCodeViewer.GetLineNumber(firstLineOfSelection) < 0) {
				firstLineOfSelection++;
			}
			int firstLineAfterSelection = this.ctrlCodeViewer.SelectionStart + this.ctrlCodeViewer.SelectionLength + 1;
			while(this.ctrlCodeViewer.GetLineNumber(firstLineAfterSelection) < 0) {
				firstLineAfterSelection++;
			}
			start = this.ctrlCodeViewer.GetLineNumber(firstLineOfSelection);
			end = this.ctrlCodeViewer.GetLineNumber(firstLineAfterSelection) - 1;

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

		class LineStyleProvider : ctrlTextbox.ILineStyleProvider
		{
			private ctrlDebuggerCode _code;

			public LineStyleProvider(ctrlDebuggerCode code)
			{
				_code = code;
			}

			public LineProperties GetLineStyle(int cpuAddress, int lineNumber)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
				LineProperties props = new LineProperties();
				bool isActiveStatement = _code._currentActiveAddress.HasValue && _code.ctrlCodeViewer.GetLineIndex((int)_code._currentActiveAddress.Value) == lineNumber;
				if(isActiveStatement) {
					props.TextBgColor = info.CodeActiveStatementColor;
					props.Symbol = LineSymbol.Arrow;
				} else if(_code._unexecutedAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnexecutedCodeColor;
				} else if(_code._speculativeCodeAddreses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnidentifiedDataColor;
				} else if(_code._verifiedDataAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeVerifiedDataColor;
				}

				AddressTypeInfo addressInfo = _code.GetLineAddressTypeInfo(lineNumber);

				foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
					if(breakpoint.Matches(cpuAddress, ref addressInfo)) {
						Color fgColor = Color.White;
						Color? bgColor = null;
						Color bpColor = breakpoint.BreakOnExec ? info.CodeExecBreakpointColor : (breakpoint.BreakOnWrite ? info.CodeWriteBreakpointColor : info.CodeReadBreakpointColor);
						Color outlineColor = bpColor;
						LineSymbol symbol;
						if(breakpoint.Enabled) {
							bgColor = bpColor;
							symbol = LineSymbol.Circle;
						} else {
							fgColor = Color.Black;
							symbol = LineSymbol.CircleOutline;
						}

						if(breakpoint.MarkEvent) {
							symbol |= LineSymbol.Mark;
						}

						if(!string.IsNullOrWhiteSpace(breakpoint.Condition)) {
							symbol |= LineSymbol.Plus;
						}

						if(isActiveStatement) {
							fgColor = Color.Black;
							bgColor = Color.Yellow;
							symbol |= LineSymbol.Arrow;
						}

						if(props == null) {
							props = new LineProperties();
						}
						props.FgColor = fgColor;
						props.TextBgColor = bgColor;
						props.OutlineColor = outlineColor;
						props.Symbol = symbol;
						break;
					}
				}

				switch(_code._lineMemoryType[lineNumber]) {
					case 'P': props.AddressColor = Color.Gray; break;
					case 'W': props.AddressColor = Color.DarkBlue; break;
					case 'S': props.AddressColor = Color.DarkRed; break;
					case 'N': props.AddressColor = Color.DarkGreen; break;
				}

				return props;
			}
		}

		class ScrollbarColorProvider : IScrollbarColorProvider
		{
			private Color _nesRamColor = Color.FromArgb(163, 222, 171);
			private Dictionary<int, Color> _breakpointColors = new Dictionary<int, Color>();

			private ctrlDebuggerCode _code;
			public ScrollbarColorProvider(ctrlDebuggerCode code)
			{
				_code = code;
				DebugInfo info = ConfigManager.Config.DebugInfo;
				int len = _code._absoluteLineNumbers.Count;

				AddressTypeInfo[] addressInfo = new AddressTypeInfo[len];
				for(int i = 0; i < len; i++) {
					addressInfo[i] = _code.GetLineAddressTypeInfo(i);
				}

				foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
					for(int i = 0; i < len; i++) {
						if(breakpoint.Matches(_code._lineNumbers[i], ref addressInfo[i])) {
							Color bpColor = breakpoint.BreakOnExec ? info.CodeExecBreakpointColor : (breakpoint.BreakOnWrite ? info.CodeWriteBreakpointColor : info.CodeReadBreakpointColor);
							_breakpointColors[i] = bpColor;
						}
					}
				}
			}

			public Color GetBackgroundColor(float position)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
				int lineIndex = (int)((_code._lineMemoryType.Count - 1) * position);

				if(lineIndex < _code._lineMemoryType.Count) {
					switch(_code._lineMemoryType[lineIndex]) {
						case 'N': return _nesRamColor;
						case 'P':
							if(_code._unexecutedAddresses.Contains(lineIndex)) {
								return info.CodeUnexecutedCodeColor;
							} else if(_code._verifiedDataAddresses.Contains(lineIndex)) {
								return info.CodeVerifiedDataColor;
							} else if(_code._speculativeCodeAddreses.Contains(lineIndex)) {
								return info.CodeUnidentifiedDataColor;
							}
							return Color.White;

						case 'W': return Color.LightSteelBlue;
						case 'S': return Color.LightCoral;
					}
				}
				return Color.Transparent;
			}

			public frmCodeTooltip GetPreview(int lineIndex)
			{
				if(lineIndex < _code._lineNumbers.Count) {
					int cpuAddress = -1;
					do {
						cpuAddress = _code._lineNumbers[lineIndex];
						lineIndex--;
					} while(cpuAddress < 0 && lineIndex >= 0);

					frmCodeTooltip frm = new frmCodeTooltip(new Dictionary<string, string>(), cpuAddress, _code._code);
					return frm;
				} else {
					return null;
				}
			}

			public int GetActiveLine()
			{
				if(_code._currentActiveAddress.HasValue) {
					return _code.ctrlCodeViewer.GetLineIndex((int)_code._currentActiveAddress.Value);
				} else {
					return -1;
				}
			}

			public int GetSelectedLine()
			{
				return _code.ctrlCodeViewer.SelectedLine;
			}

			public Color GetMarkerColor(float position, int linesPerPixel)
			{
				int lineIndex = (int)((_code._lineMemoryType.Count - 1) * position);

				Color bpColor;
				for(int i = 0; i < linesPerPixel; i++) {
					if(_breakpointColors.TryGetValue(lineIndex + i, out bpColor)) {
						return bpColor;
					}
				}
				return Color.Transparent;
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
