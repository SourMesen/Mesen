using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mesen.GUI.Debugger.Ld65DbgImporter;
using System.IO;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlSourceViewer : BaseControl, ICodeViewer
	{
		private UInt32? _currentActiveAddress { get; set; } = null;
		private CodeTooltipManager _tooltipManager = null;
		private CodeViewerActions _codeViewerActions;
		private DebugViewInfo _config;
		private Ld65DbgImporter.FileInfo _selectedFile = null;
		
		public ctrlSourceViewer()
		{
			InitializeComponent();
			_tooltipManager = new CodeTooltipManager(this, this.ctrlCodeViewer);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				_codeViewerActions = new CodeViewerActions(this, true);

				this.SymbolProvider = DebugWorkspaceManager.SymbolProvider;
				DebugWorkspaceManager.SymbolProviderChanged += UpdateSymbolProvider;
			}
		}

		private void UpdateSymbolProvider(Ld65DbgImporter symbolProvider)
		{
			this.SymbolProvider = symbolProvider;
			if(symbolProvider == null && this.Visible) {
				_codeViewerActions.SwitchView();
			}
		}

		public new void Focus()
		{
			base.Focus();
			this.ctrlCodeViewer.Focus();
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

		private List<string> _lineNumberNotes = new List<string>();
		private void UpdateCode()
		{
			if(_symbolProvider == null || CurrentFile == null) {
				return;
			}

			List<int> indents = new List<int>();
			List<string> addressing = new List<string>();
			List<string> comments = new List<string>();
			List<int> lineNumbers = new List<int>();
			List<string> lineNotes = new List<string>();
			_lineNumberNotes = new List<string>();
			List<string> codeLines = new List<string>();

			bool isC = CurrentFile.Name.EndsWith(".h") || CurrentFile.Name.EndsWith(".c");
			int index = 0;
			foreach(string line in CurrentFile.Data) {
				string l = line.Replace("\t", "  ");

				addressing.Add("");

				int prgAddress = _symbolProvider.GetPrgAddress(CurrentFile.ID, index);
				int relativeAddress = InteropEmu.DebugGetRelativeAddress((uint)prgAddress, AddressType.PrgRom);
				lineNumbers.Add(relativeAddress);
				lineNotes.Add("");
				_lineNumberNotes.Add(prgAddress >= 0 ? prgAddress.ToString("X4") : "");

				string trimmed = l.TrimStart();
				int margin = (l.Length - trimmed.Length) * 10;
				indents.Add(margin);

				int commentIndex;
				if(isC) {
					commentIndex = trimmed.IndexOf("//");
				} else {
					commentIndex = trimmed.IndexOfAny(new char[] { ';', '.' });
				}

				if(commentIndex >= 0) {
					comments.Add(trimmed.Substring(commentIndex));
					codeLines.Add(trimmed.Substring(0, commentIndex));
				} else {
					comments.Add("");
					codeLines.Add(trimmed);
				}
				index++;
			}

			ctrlCodeViewer.CodeHighlightingEnabled = !isC;
			ctrlCodeViewer.LineIndentations = indents.ToArray();
			ctrlCodeViewer.Addressing = addressing.ToArray();
			ctrlCodeViewer.Comments = comments.ToArray();

			ctrlCodeViewer.LineNumbers = lineNumbers.ToArray();
			ctrlCodeViewer.TextLineNotes = lineNotes.ToArray();
			ctrlCodeViewer.LineNumberNotes = _lineNumberNotes.ToArray();
			ctrlCodeViewer.TextLines = codeLines.ToArray();

			this.RefreshViewer();
		}

		public void RefreshViewer()
		{
			if(_symbolProvider != null) {
				ctrlCodeViewer.ScrollbarColorProvider = new ScrollbarColorProvider(this);
				ctrlCodeViewer.StyleProvider = new LineStyleProvider(this);
			} else {
				ctrlCodeViewer.ScrollbarColorProvider = null;
				ctrlCodeViewer.StyleProvider = null;
			}
		}

		Ld65DbgImporter _symbolProvider;
		public Ld65DbgImporter SymbolProvider
		{
			get { return _symbolProvider; }
			set
			{
				if(_symbolProvider != value) {
					_symbolProvider = value;

					cboFile.BeginUpdate();
					cboFile.Items.Clear();
					cboFile.Sorted = false;
					if(_symbolProvider != null) {
						foreach(Ld65DbgImporter.FileInfo file in _symbolProvider.Files.Values) {
							if(file.Data != null && file.Data.Length > 0 && !file.Name.ToLower().EndsWith(".chr")) {
								cboFile.Items.Add(file);
							}
						}
					}
					cboFile.Sorted = true;
					cboFile.EndUpdate();

					if(cboFile.Items.Count > 0) {
						cboFile.SelectedIndex = 0;
					}

					_tooltipManager.SymbolProvider = value;
				}
			}
		}

		public Ld65DbgImporter.FileInfo CurrentFile
		{
			get { return (Ld65DbgImporter.FileInfo)_selectedFile; }
			set
			{
				cboFile.SelectedItem = value;
				_selectedFile = value;
			}
		}

		public bool HideFileDropdown { set { cboFile.Visible = !value; lblFile.Visible = !value; } }

		public ctrlScrollableTextbox CodeViewer { get { return this.ctrlCodeViewer; } }
		public CodeViewerActions CodeViewerActions { get { return _codeViewerActions; } }
		public UInt32? ActiveAddress { get { return _currentActiveAddress; } }

		private void mnuToggleBreakpoint_Click(object sender, EventArgs e)
		{
			ToggleBreakpoint();
		}

		public AddressTypeInfo GetAddressInfo(int lineIndex)
		{
			return new AddressTypeInfo() {
				Address = _symbolProvider?.GetPrgAddress(CurrentFile.ID, lineIndex) ?? -1,
				Type = AddressType.PrgRom
			};
		}

		public void SetActiveAddress(UInt32 address)
		{
			_currentActiveAddress = address;
			this.UpdateCode();
		}

		public void SelectActiveAddress(UInt32 address)
		{
			if(_symbolProvider == null) {
				return;
			}

			_currentActiveAddress = address;

			ScrollToLineNumber((int)address);
			this.RefreshViewer();
		}

		public void ClearActiveAddress()
		{
			_currentActiveAddress = null;
			this.RefreshViewer();
		}

		private void cboFile_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(cboFile.SelectedIndex >= 0) {
				_selectedFile = cboFile.SelectedItem as Ld65DbgImporter.FileInfo;
				this.ctrlCodeViewer.ScrollToLineIndex(0, eHistoryType.Always, true);
				this.UpdateCode();
			} else {
				_selectedFile = null;
			}
		}

		public void ToggleBreakpoint()
		{
			AddressTypeInfo info = GetAddressInfo(ctrlCodeViewer.SelectedLine);
			if(info.Address >= 0) {
				BreakpointManager.ToggleBreakpoint(-1, info, false);
			} else {
				//Current line has no address, try using the next line instead.
				//(Used when trying to set a breakpoint on a row containing only a label)
				info = GetAddressInfo(ctrlCodeViewer.SelectedLine + 1);
				if(info.Address >= 0) {
					BreakpointManager.ToggleBreakpoint(-1, info, false);
				}
			}
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

		private void ctrlCodeViewer_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left && e.Location.X < this.ctrlCodeViewer.CodeMargin / 4) {
				ToggleBreakpoint();
			}
		}

		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			_codeViewerActions.ProcessMouseUp(e.Location, e.Button);
		}

		private void ctrlCodeViewer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			_codeViewerActions.ProcessMouseDoubleClick(e.Location);
		}

		private void ctrlCodeViewer_TextZoomChanged(object sender, EventArgs e)
		{
			_config.TextZoom = this.ctrlCodeViewer.TextZoom;
			ConfigManager.ApplyChanges();
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

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			if(bp != null) {
				BreakpointManager.EditBreakpoint(bp);
			}
		}

		private void mnuDisableBreakpoint_Click(object sender, EventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			bp.SetEnabled(!bp.Enabled);
		}

		private void mnuRemoveBreakpoint_Click(object sender, EventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			if(bp != null) {
				BreakpointManager.RemoveBreakpoint(bp);
			}
		}

		private void contextMenuMargin_Opening(object sender, CancelEventArgs e)
		{
			Breakpoint bp = GetCurrentLineBreakpoint();
			if(bp != null) {
				mnuDisableBreakpoint.Text = bp.Enabled ? "Disable breakpoint" : "Enable breakpoint";
			} else {
				e.Cancel = true;
			}
		}

		private void ctrlCodeViewer_MouseLeave(object sender, EventArgs e)
		{
			_tooltipManager.Close();
		}

		private void ctrlCodeViewer_ScrollPositionChanged(object sender, EventArgs e)
		{
			_tooltipManager?.Close();
		}

		public void ScrollToFileLine(string filename, int lineNumber)
		{
			foreach(Ld65DbgImporter.FileInfo fileInfo in cboFile.Items) {
				if(fileInfo.Name == filename) {
					cboFile.SelectedItem = fileInfo;
					ctrlCodeViewer.ScrollToLineIndex(lineNumber);
					break;
				}
			}
		}

		public void ScrollToAddress(AddressTypeInfo addressInfo, bool scrollToTop = false)
		{
			if(addressInfo.Address >= 0 && addressInfo.Type == AddressType.PrgRom) {
				LineInfo line = _symbolProvider?.GetSourceCodeLineInfo(addressInfo.Address);
				if(line != null) {
					foreach(Ld65DbgImporter.FileInfo fileInfo in cboFile.Items) {
						if(fileInfo.ID == line.FileID) {
							cboFile.SelectedItem = fileInfo;
							break;
						}
					}
					ctrlCodeViewer.ScrollToLineIndex(line.LineNumber, eHistoryType.Always, scrollToTop);
				}
			}
		}

		private bool CurrentFileContainsAddress(int cpuAddress)
		{
			if(CurrentFile == null) {
				return false;
			}

			AddressTypeInfo addressInfo = new AddressTypeInfo();
			InteropEmu.DebugGetAbsoluteAddressAndType((uint)cpuAddress, addressInfo);
			if(addressInfo.Address >= 0 && addressInfo.Type == AddressType.PrgRom) {
				LineInfo line = _symbolProvider?.GetSourceCodeLineInfo(addressInfo.Address);
				return CurrentFile.ID == line?.FileID;
			}
			return false;
		}

		public void ScrollToLineNumber(int lineNumber, bool scrollToTop = false)
		{
			AddressTypeInfo addressInfo = new AddressTypeInfo();
			InteropEmu.DebugGetAbsoluteAddressAndType((uint)lineNumber, addressInfo);
			ScrollToAddress(addressInfo, scrollToTop);
		}

		public void EditSubroutine()
		{
			//Not supported
		}

		public void EditSelectedCode()
		{
			//Not supported
		}

		public void FindAllOccurrences(string text, bool matchWholeWord, bool matchCase)
		{
			//Not supported (yet?)
		}

		#region Scrollbar / Code highlighting

		class LineStyleProvider : ctrlTextbox.ILineStyleProvider
		{
			private ctrlSourceViewer _viewer;

			public LineStyleProvider(ctrlSourceViewer viewer)
			{
				_viewer = viewer;
			}

			public string GetLineComment(int lineNumber)
			{
				return null;
			}

			public LineProperties GetLineStyle(int cpuAddress, int lineIndex)
			{
				LineProperties props = new LineProperties();

				int nextLineIndex = lineIndex + 1;
				int nextCpuAddress;
				do {
					nextCpuAddress = _viewer.CodeViewer.GetLineNumber(nextLineIndex);
					nextLineIndex++;
				} while(nextCpuAddress < 0);

				bool isActiveStatement = (
					cpuAddress >= 0 &&
					_viewer._currentActiveAddress.HasValue && (
						(_viewer._currentActiveAddress >= cpuAddress && _viewer._currentActiveAddress < nextCpuAddress && nextCpuAddress > cpuAddress) ||
						(_viewer._currentActiveAddress == cpuAddress && nextCpuAddress < cpuAddress)
					)
				);

				int prgAddress = _viewer._symbolProvider?.GetPrgAddress(_viewer.CurrentFile.ID, lineIndex) ?? -1;

				if(prgAddress >= 0) {
					AddressTypeInfo addressInfo = new AddressTypeInfo();
					addressInfo.Address = prgAddress;
					addressInfo.Type = AddressType.PrgRom;

					ctrlDebuggerCode.LineStyleProvider.GetBreakpointLineProperties(props, cpuAddress, addressInfo);
				}

				if(isActiveStatement) {
					ctrlDebuggerCode.LineStyleProvider.ConfigureActiveStatement(props);
				}

				return props;
			}
		}

		class ScrollbarColorProvider : IScrollbarColorProvider
		{
			private Dictionary<int, Color> _breakpointColors = new Dictionary<int, Color>();

			private ctrlSourceViewer _viewer;
			public ScrollbarColorProvider(ctrlSourceViewer viewer)
			{
				_viewer = viewer;

				DebugInfo info = ConfigManager.Config.DebugInfo;
				int len = viewer.ctrlCodeViewer.LineCount;

				int[] relativeAddresses = new int[len];
				AddressTypeInfo[] addressInfo = new AddressTypeInfo[len];
				for(int i = 0; i < len; i++) {
					addressInfo[i] = _viewer.GetAddressInfo(i);
					if(addressInfo[i].Address >= 0) {
						relativeAddresses[i] = InteropEmu.DebugGetRelativeAddress((uint)addressInfo[i].Address, AddressType.PrgRom);
					} else {
						relativeAddresses[i] = -1;
					}
				}

				foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
					for(int i = 0; i < len; i++) {
						if(breakpoint.Matches(relativeAddresses[i], addressInfo[i])) {
							Color bpColor = breakpoint.BreakOnExec ? info.CodeExecBreakpointColor : (breakpoint.BreakOnWrite ? info.CodeWriteBreakpointColor : info.CodeReadBreakpointColor);
							_breakpointColors[i] = bpColor;
						}
					}
				}
			}

			public Color GetBackgroundColor(float position)
			{
				return Color.Transparent;
			}

			public frmCodePreviewTooltip GetPreview(int lineIndex)
			{
				if(lineIndex < _viewer.CodeViewer.LineCount) {
					while(lineIndex > 0 && _viewer.CodeViewer.GetLineNumber(lineIndex) < 0) {
						lineIndex--;
					}
					return new frmCodePreviewTooltip(_viewer.FindForm(), lineIndex, null, _viewer.SymbolProvider, _viewer.CurrentFile);
				} else {
					return null;
				}
			}

			public int GetActiveLine()
			{
				if(_viewer._currentActiveAddress.HasValue && _viewer.CurrentFileContainsAddress((int)_viewer._currentActiveAddress.Value)) {
					return _viewer.ctrlCodeViewer.GetLineIndex((int)_viewer._currentActiveAddress.Value);
				} else {
					return -1;
				}
			}

			public int GetSelectedLine()
			{
				return _viewer.ctrlCodeViewer.SelectedLine;
			}

			public Color GetMarkerColor(float position, int linesPerPixel)
			{
				int lineIndex = (int)((_viewer.ctrlCodeViewer.LineCount - 1) * position);

				Color bpColor;
				for(int i = 0; i < linesPerPixel; i++) {
					if(_breakpointColors.TryGetValue(lineIndex + i, out bpColor)) {
						return bpColor;
					}
				}
				return Color.Transparent;
			}
		}
		#endregion
	}
}
