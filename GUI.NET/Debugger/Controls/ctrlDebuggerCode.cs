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

		private UInt32? _currentActiveAddress { get; set; } = null;

		private DebugViewInfo _config;
		private CodeTooltipManager _tooltipManager = null;
		private CodeViewerActions _codeViewerActions;

		public ctrlDebuggerCode()
		{
			InitializeComponent();
			_tooltipManager = new CodeTooltipManager(this, this.ctrlCodeViewer);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				_codeViewerActions = new CodeViewerActions(this, false);

				ctrlFindOccurrences.Viewer = this;
				splitContainer.Panel2Collapsed = true;

				this.SymbolProvider = DebugWorkspaceManager.SymbolProvider;
				DebugWorkspaceManager.SymbolProviderChanged += UpdateSymbolProvider;
			}
		}

		private void UpdateSymbolProvider(Ld65DbgImporter symbolProvider)
		{
			this.SymbolProvider = symbolProvider;
		}

		public void SetConfig(DebugViewInfo config, bool disableActions = false)
		{
			_config = config;
			if(!disableActions) {
				_codeViewerActions.InitMenu(config);
			}

			if(this.ctrlCodeViewer.TextZoom != config.TextZoom) {
				this.ctrlCodeViewer.TextZoom = config.TextZoom;
			}
		}

		public void SetMessage(TextboxMessageInfo message)
		{
			this.ctrlCodeViewer.SetMessage(message);
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

		private CodeInfo _code = new CodeInfo("");

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CodeInfo Code
		{
			get { return _code; }
			set
			{
				_code = value;
				_tooltipManager.Code = value;
				UpdateCode();
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
			_code.InitAssemblerValues();

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
				if(_code.CodeContent.TryGetValue(i, out code)) {
					code = code.Split('\x2')[0].Trim();

					if(code.StartsWith("--") || code.StartsWith("__")) {
						//Stop adding code when we find a new section (new function, data blocks, etc.)
						break;
					}

					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)i, info);
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
						code = ".byte " + string.Join(",", _code.CodeByteCode[i].Split(' '));
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
					
					int length = _code.CodeByteCode[i].Count(c => c == ' ') + 1;
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

		private void UpdateCode()
		{
			int centerLineIndex = ctrlCodeViewer.GetLineIndexAtPosition(0) + ctrlCodeViewer.GetNumberVisibleLines() / 2;
			int centerLineAddress;
			int scrollOffset = -1;
			do {
				//Save the address at the center of the debug view
				centerLineAddress = ctrlCodeViewer.GetLineNumber(centerLineIndex);
				centerLineIndex--;
				scrollOffset++;
			} while(centerLineAddress < 0 && centerLineIndex > 0);


			ctrlCodeViewer.LineIndentations = _code.LineIndentations;
			ctrlCodeViewer.Addressing = _code.Addressing;
			ctrlCodeViewer.Comments = _code.Comments;

			ctrlCodeViewer.LineNumbers = _code.LineNumbers;
			ctrlCodeViewer.TextLineNotes = _code.CodeNotes;
			ctrlCodeViewer.LineNumberNotes = _code.LineNumberNotes;
			ctrlCodeViewer.TextLines = _code.CodeLines;
			
			if(centerLineAddress >= 0) {
				//Scroll to the same address as before, to prevent the code view from changing due to setting or banking changes, etc.
				int lineIndex = ctrlCodeViewer.GetLineIndex(centerLineAddress) + scrollOffset;
				ctrlCodeViewer.ScrollToLineIndex(lineIndex, eHistoryType.None, false, true);
			}
		}
		
		private void ctrlCodeViewer_MouseLeave(object sender, EventArgs e)
		{
			_tooltipManager.Close();
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

			_tooltipManager.ProcessMouseMove(e.Location);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			_codeViewerActions.UpdateContextMenuItemVisibility(_codeViewerActions.contextMenu.Items);
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			_codeViewerActions.ProcessMouseUp(e.Location, e.Button);
		}

		private void ctrlCodeViewer_MouseDown(object sender, MouseEventArgs e)
		{
			_tooltipManager.Close();

			if(e.Button == MouseButtons.Left && e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				int lineIndex = ctrlCodeViewer.GetLineIndexAtPosition(e.Y);
				int relativeAddress = ctrlCodeViewer.GetLineNumber(lineIndex);
				if(relativeAddress < 0 && ctrlCodeViewer.LineCount > lineIndex + 1) {
					//Current line has no address, try using the next line instead.
					//(Used when trying to set a breakpoint on a row containing only a label)
					lineIndex++;
					relativeAddress = ctrlCodeViewer.GetLineNumber(lineIndex);
				}

				AddressTypeInfo info = GetAddressInfo(lineIndex);
				BreakpointManager.ToggleBreakpoint(relativeAddress, info, false);
			}
		}

		public AddressTypeInfo GetAddressInfo(int lineNumber)
		{
			AddressTypeInfo info = new AddressTypeInfo();
			SetAddressInfo(info, lineNumber);
			return info;
		}

		private void SetAddressInfo(AddressTypeInfo info, int lineNumber)
		{
			if(lineNumber < this._code.AbsoluteLineNumbers.Length) {
				info.Address = this._code.AbsoluteLineNumbers[lineNumber];
				switch(this._code.LineMemoryType[lineNumber]) {
					case 'P': info.Type = AddressType.PrgRom; break;
					case 'W': info.Type = AddressType.WorkRam; break;
					case 'S': info.Type = AddressType.SaveRam; break;
					case 'N': info.Type = AddressType.InternalRam; break;
				}
			}
		}

		private void ctrlCodeViewer_ScrollPositionChanged(object sender, EventArgs e)
		{
			_tooltipManager?.Close();
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
			ConfigManager.ApplyChanges();
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
				if(_code.SymbolProvider != null && _code._config?.ShowSourceAsComments == true) {
					AddressTypeInfo addressInfo = _code.GetAddressInfo(lineNumber);
					if(addressInfo.Type == AddressType.PrgRom) {
						return _code.SymbolProvider.GetSourceCodeLine(addressInfo.Address);
					}
				}
				return null;
			}

			public static void ConfigureActiveStatement(LineProperties props)
			{
				props.FgColor = Color.Black;
				props.TextBgColor = ConfigManager.Config.DebugInfo.CodeActiveStatementColor;
				props.Symbol |= LineSymbol.Arrow;

				if(ConfigManager.Config.DebugInfo.ShowInstructionProgression) {
					InstructionProgress state = new InstructionProgress();
					InteropEmu.DebugGetInstructionProgress(ref state);

					LineProgress progress = new LineProgress();
					progress.Current = (int)state.OpCycle;
					progress.Maxixum = frmOpCodeTooltip.OpCycles[state.OpCode];
					switch(state.OpMemoryOperationType) {
						case InteropMemoryOperationType.DummyRead: progress.Color = Color.FromArgb(184, 160, 255); progress.Text = "DR"; break;
						case InteropMemoryOperationType.DummyWrite: progress.Color = Color.FromArgb(255, 245, 137); progress.Text = "DW"; break;
						case InteropMemoryOperationType.Read: progress.Color = Color.FromArgb(150, 176, 255); progress.Text = "R"; break;
						case InteropMemoryOperationType.Write: progress.Color = Color.FromArgb(255, 171, 150); progress.Text = "W"; break;
						default: progress.Color = Color.FromArgb(143, 255, 173); progress.Text = "X"; break;
					}
					props.Progress = progress;
				}
			}

			public LineProperties GetLineStyle(int cpuAddress, int lineNumber)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
				LineProperties props = new LineProperties();
				AddressTypeInfo addressInfo = _code.GetAddressInfo(lineNumber);
				GetBreakpointLineProperties(props, cpuAddress, addressInfo);

				bool isActiveStatement = _code._currentActiveAddress.HasValue && _code.ctrlCodeViewer.GetLineIndex((int)_code._currentActiveAddress.Value) == lineNumber;
				if(isActiveStatement) {
					ConfigureActiveStatement(props);
				} else if(_code._code.UnexecutedAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnexecutedCodeColor;
				} else if(_code._code.SpeculativeCodeAddreses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeUnidentifiedDataColor;
				} else if(_code._code.VerifiedDataAddresses.Contains(lineNumber)) {
					props.LineBgColor = info.CodeVerifiedDataColor;
				}

				switch(_code._code.LineMemoryType[lineNumber]) {
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
					if(breakpoint.Matches(cpuAddress, addressInfo)) {
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
				int len = _code._code.AbsoluteLineNumbers.Length;

				AddressTypeInfo addressInfo = new AddressTypeInfo();

				Breakpoint[] breakpoints = BreakpointManager.Breakpoints.ToArray();
				for(int i = 0; i < len; i++) {
					_code.SetAddressInfo(addressInfo, i);
					for(int j = 0; j < breakpoints.Length; j++) {
						if(breakpoints[j].Matches(_code._code.LineNumbers[i], addressInfo)) {
							Color bpColor = breakpoints[j].BreakOnExec ? info.CodeExecBreakpointColor : (breakpoints[j].BreakOnWrite ? info.CodeWriteBreakpointColor : info.CodeReadBreakpointColor);
							_breakpointColors[i] = bpColor;
							break;
						}
					}
				}
			}

			public Color GetBackgroundColor(float position)
			{
				DebugInfo info = ConfigManager.Config.DebugInfo;
				int lineIndex = (int)((_code._code.LineMemoryType.Length - 1) * position);

				if(lineIndex < _code._code.LineMemoryType.Length) {
					switch(_code._code.LineMemoryType[lineIndex]) {
						case 'N': return _nesRamColor;
						case 'P':
							if(_code._code.UnexecutedAddresses.Contains(lineIndex)) {
								return info.CodeUnexecutedCodeColor;
							} else if(_code._code.VerifiedDataAddresses.Contains(lineIndex)) {
								return info.CodeVerifiedDataColor;
							} else if(_code._code.SpeculativeCodeAddreses.Contains(lineIndex)) {
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
				if(lineIndex < _code._code.LineNumbers.Length) {
					while(lineIndex > 0 && _code._code.LineNumbers[lineIndex] < 0) {
						lineIndex--;
					}
					return new frmCodePreviewTooltip(_code.FindForm(), lineIndex, _code._code);
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
				int lineIndex = (int)((_code._code.LineMemoryType.Length - 1) * position);

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
