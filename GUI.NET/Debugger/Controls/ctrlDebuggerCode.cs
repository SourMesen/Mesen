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
	public partial class ctrlDebuggerCode : BaseScrollableTextboxUserControl, ICodeViewer
	{
		public delegate void AssemblerEventHandler(AssemblerEventArgs args);
		public event AssemblerEventHandler OnEditCode;

		private List<int> _lineNumbers = new List<int>(10000);
		private List<string> _lineNumberNotes = new List<string>(10000);
		private List<char> _lineMemoryType = new List<char>(10000);
		private List<int> _absoluteLineNumbers = new List<int>(10000);
		private List<string> _codeNotes = new List<string>(10000);
		private List<string> _codeLines = new List<string>(10000);
		private HashSet<int> _unexecutedAddresses = new HashSet<int>();
		private HashSet<int> _verifiedDataAddresses = new HashSet<int>();
		private HashSet<int> _speculativeCodeAddreses = new HashSet<int>();
		private Dictionary<int, string> _codeContent = new Dictionary<int, string>(10000);
		private Dictionary<int, string> _codeComments = new Dictionary<int, string>(10000);
		private Dictionary<int, string> _codeByteCode = new Dictionary<int, string>(10000);
		private List<string> _addressing = new List<string>(10000);
		private List<string> _comments = new List<string>(10000);
		private List<int> _lineIndentations = new List<int>(10000);

		private UInt32? _currentActiveAddress { get; set; } = null;

		private Point _previousLocation;
		private DebugViewInfo _config;
		private CodeTooltipManager _tooltipManager = null;
		private CodeViewerActions _codeViewerActions;

		public ctrlDebuggerCode()
		{
			InitializeComponent();
			_tooltipManager = new CodeTooltipManager(this, this.ctrlCodeViewer);

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				_codeViewerActions = new CodeViewerActions(this, false);

				ctrlFindOccurrences.Viewer = this;
				splitContainer.Panel2Collapsed = true;
			}
		}

		public void SetConfig(DebugViewInfo config)
		{
			_config = config;

			_codeViewerActions.InitMenu(config);

			if(this.ctrlCodeViewer.TextZoom != config.TextZoom) {
				this.ctrlCodeViewer.TextZoom = config.TextZoom;
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

		private Ld65DbgImporter _symbolProvider;
		public Ld65DbgImporter SymbolProvider
		{
			get { return _symbolProvider; } 
			set { _symbolProvider = value; }
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
					_tooltipManager.Code = value;
					UpdateCode();
				}
			}
		}

		public bool ShowMemoryValues
		{
			get { return this.ctrlCodeViewer.ShowMemoryValues; }
			set { this.ctrlCodeViewer.ShowMemoryValues = value; }
		}

		public ctrlScrollableTextbox CodeViewer { get { return this.ctrlCodeViewer; } }
		public CodeViewerActions CodeViewerActions { get { return _codeViewerActions; } }
		public uint? ActiveAddress { get { return _currentActiveAddress; } }

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
			if(this.ctrlCodeViewer.ShowScrollbars) {
				this.ctrlCodeViewer.ScrollbarColorProvider = new ScrollbarColorProvider(this);
			}
		}

		public void ScrollToAddress(AddressTypeInfo addressInfo, bool scrollToTop = false)
		{
			int relativeAddress = InteropEmu.DebugGetRelativeAddress((uint)addressInfo.Address, addressInfo.Type);
			if(relativeAddress >= 0) {
				this.ctrlCodeViewer.ScrollToLineNumber(relativeAddress, eHistoryType.Always , scrollToTop);
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

		private void ctrlCodeViewer_MouseLeave(object sender, EventArgs e)
		{
			_tooltipManager.Close(true);
		}
		
		private Breakpoint GetCurrentLineBreakpoint()
		{
			AddressTypeInfo addressInfo = GetAddressInfo(ctrlCodeViewer.SelectedLine);
			if(addressInfo.Address >= 0) {
				int relativeAddress = InteropEmu.DebugGetRelativeAddress((uint)addressInfo.Address, addressInfo.Type);
				return BreakpointManager.GetMatchingBreakpoint(relativeAddress, addressInfo);
			}
			return null;
		}

		private void ctrlCodeViewer_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				this.ctrlCodeViewer.ContextMenuStrip = contextMenuMargin;
			} else {
				this.ctrlCodeViewer.ContextMenuStrip = _codeViewerActions.contextMenu;
			}

			_previousLocation = e.Location;
			_tooltipManager.ProcessMouseMove(e.Location);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			_codeViewerActions.UpdateContextMenuItemVisibility();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			_codeViewerActions.ProcessMouseUp(e.Location, e.Button);
		}

		private void ctrlCodeViewer_MouseDown(object sender, MouseEventArgs e)
		{
			_tooltipManager.Close(true);

			if(e.Button == MouseButtons.Left && e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				int relativeAddress = ctrlCodeViewer.GetLineNumberAtPosition(e.Y);
				AddressTypeInfo info = GetAddressInfo(ctrlCodeViewer.GetLineIndexAtPosition(e.Y));
				BreakpointManager.ToggleBreakpoint(relativeAddress, info, false);
			}
		}

		public AddressTypeInfo GetAddressInfo(int lineNumber)
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
			_tooltipManager?.Close(true);
		}

		private void ctrlCodeViewer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(e.Location.X > this.ctrlCodeViewer.CodeMargin / 2 && e.Location.X < this.ctrlCodeViewer.CodeMargin) {
				AddressTypeInfo info = GetAddressInfo(ctrlCodeViewer.GetLineIndexAtPosition(e.Y));
				if(info.Address >= 0) {
					ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
				}
			} else{
				_codeViewerActions.ProcessMouseDoubleClick(e.Location);
			}
		}

		private void contextMenuMargin_Opening(object sender, CancelEventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			if(bp == null) {
				e.Cancel = true;
			} else {
				mnuDisableBreakpoint.Text = bp.Enabled ? "Disable breakpoint" : "Enable breakpoint";
			}
		}

		private void mnuRemoveBreakpoint_Click(object sender, EventArgs e)
		{
			BreakpointManager.RemoveBreakpoint(GetCurrentLineBreakpoint());
		}

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			BreakpointManager.EditBreakpoint(GetCurrentLineBreakpoint());
		}

		private void mnuDisableBreakpoint_Click(object sender, EventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			bp.SetEnabled(!bp.Enabled);
		}

		private void ctrlCodeViewer_TextZoomChanged(object sender, EventArgs e)
		{
			_config.TextZoom = this.ctrlCodeViewer.TextZoom;
			UpdateConfig();
		}

		public void EditSubroutine()
		{
			int currentLine = this.GetCurrentLine();
			if(currentLine != -1 && InteropEmu.DebugIsExecutionStopped()) {
				int byteLength;
				List<string> code = this.GetCode(out byteLength, ref currentLine);
				this.OnEditCode?.Invoke(new AssemblerEventArgs() { Code = string.Join(Environment.NewLine, code), StartAddress = (UInt16)currentLine, BlockLength = (UInt16)byteLength });
			}
		}

		public void EditSelectedCode()
		{
			int startAddress = this.GetCurrentLine();
			int endAddress = this.ctrlCodeViewer.LastSelectedLine;
			if(startAddress != -1 && InteropEmu.DebugIsExecutionStopped()) {
				int byteLength;
				List<string> code = this.GetCode(out byteLength, ref startAddress, endAddress);
				this.OnEditCode?.Invoke(new AssemblerEventArgs() { Code = string.Join(Environment.NewLine, code), StartAddress = (UInt16)startAddress, BlockLength = (UInt16)byteLength });
			}
		}

		public void FindAllOccurrences(string text, bool matchWholeWord, bool matchCase)
		{
			ctrlFindOccurrences.FindAllOccurrences(text, matchWholeWord, matchCase);
			this.splitContainer.Panel2Collapsed = false;
		}

		private void ctrlFindOccurrences_OnSearchResultsClosed(object sender, EventArgs e)
		{
			this.splitContainer.Panel2Collapsed = true;
		}

		public class LineStyleProvider : ctrlTextbox.ILineStyleProvider
		{
			private ctrlDebuggerCode _code;

			public LineStyleProvider(ctrlDebuggerCode code)
			{
				_code = code;
			}

			public string GetLineComment(int lineNumber)
			{
				if(_code.SymbolProvider != null && _code._config.ShowSourceAsComments) {
					AddressTypeInfo addressInfo = _code.GetAddressInfo(lineNumber);
					if(addressInfo.Type == AddressType.PrgRom) {
						return _code.SymbolProvider.GetSourceCodeLine(addressInfo.Address);
					}
				}
				return null;
			}

			public LineProperties GetLineStyle(int cpuAddress, int lineNumber)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
				LineProperties props = new LineProperties();
				AddressTypeInfo addressInfo = _code.GetAddressInfo(lineNumber);
				GetBreakpointLineProperties(props, cpuAddress, addressInfo);

				bool isActiveStatement = _code._currentActiveAddress.HasValue && _code.ctrlCodeViewer.GetLineIndex((int)_code._currentActiveAddress.Value) == lineNumber;
				if(isActiveStatement) {
					props.FgColor = Color.Black;
					props.TextBgColor = info.CodeActiveStatementColor;
					props.Symbol |= LineSymbol.Arrow;
				} else if(_code._unexecutedAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnexecutedCodeColor;
				} else if(_code._speculativeCodeAddreses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnidentifiedDataColor;
				} else if(_code._verifiedDataAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeVerifiedDataColor;
				}

				switch(_code._lineMemoryType[lineNumber]) {
					case 'P': props.AddressColor = Color.Gray; break;
					case 'W': props.AddressColor = Color.DarkBlue; break;
					case 'S': props.AddressColor = Color.DarkRed; break;
					case 'N': props.AddressColor = Color.DarkGreen; break;
				}

				return props;
			}

			public static void GetBreakpointLineProperties(LineProperties props, int cpuAddress, AddressTypeInfo addressInfo)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
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

						props.FgColor = fgColor;
						props.TextBgColor = bgColor;
						props.OutlineColor = outlineColor;
						props.Symbol = symbol;
						return;
					}
				}
			}
		}

		class ScrollbarColorProvider : IScrollbarColorProvider
		{
			private Color _nesRamColor = Color.FromArgb(213, 255, 213);
			private Dictionary<int, Color> _breakpointColors = new Dictionary<int, Color>();

			private ctrlDebuggerCode _code;
			public ScrollbarColorProvider(ctrlDebuggerCode code)
			{
				_code = code;
				DebugInfo info = ConfigManager.Config.DebugInfo;
				int len = _code._absoluteLineNumbers.Count;

				AddressTypeInfo[] addressInfo = new AddressTypeInfo[len];
				for(int i = 0; i < len; i++) {
					addressInfo[i] = _code.GetAddressInfo(i);
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

						case 'W': return Color.Lavender;
						case 'S': return Color.MistyRose;
					}
				}
				return Color.Transparent;
			}

			public frmCodePreviewTooltip GetPreview(int lineIndex)
			{
				if(lineIndex < _code._lineNumbers.Count) {
					while(lineIndex > 0 && _code._lineNumbers[lineIndex] < 0) {
						lineIndex--;
					}
					return new frmCodePreviewTooltip(lineIndex, _code._code);
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

	public class AssemblerEventArgs : EventArgs
	{
		public string Code { get; set; }
		public UInt16 StartAddress { get; set; }
		public UInt16 BlockLength { get; set; }
	}
}
