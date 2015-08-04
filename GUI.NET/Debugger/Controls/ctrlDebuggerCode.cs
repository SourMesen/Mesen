using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlDebuggerCode : UserControl
	{
		public delegate void WatchAddedEventHandler(WatchAddedEventArgs args);
		public event WatchAddedEventHandler OnWatchAdded;

		public ctrlDebuggerCode()
		{
			InitializeComponent();
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

		[DefaultValue(13F)]
		public float FontSize
		{
			get { return this.ctrlCodeViewer.FontSize; }
			set { this.ctrlCodeViewer.FontSize = value; }
		}

		private UInt32? _currentActiveAddress = null;
		public void SelectActiveAddress(UInt32 address)
		{
			this.SetActiveAddress(address);
			this.ctrlCodeViewer.ScrollToLineNumber((int)address);
		}

		public void SetActiveAddress(UInt32 address)
		{
			//Set line background to yellow
			this.ctrlCodeViewer.ClearLineStyles();
			this.ctrlCodeViewer.SetLineColor((int)address, Color.Black, Color.Yellow, null, LineSymbol.Arrow);
			_currentActiveAddress = address;
		}
		
		public void ClearActiveAddress()
		{
			_currentActiveAddress = null;
		}
		
		public string GetWordUnderLocation(Point position)
		{
			return this.ctrlCodeViewer.GetWordUnderLocation(position);
		}

		public void OpenSearchBox()
		{
			this.ctrlCodeViewer.OpenSearchBox();
		}

		public void FindNext()
		{
			this.ctrlCodeViewer.FindNext();
		}

		public void FindPrevious()
		{
			this.ctrlCodeViewer.FindPrevious();
		}
		
		public bool UpdateCode(bool forceUpdate = false)
		{
			if(_codeChanged || forceUpdate) {
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();
				this.ctrlCodeViewer.ClearLineStyles();

				List<int> lineNumbers = new List<int>();
				List<string> codeLines = new List<string>();
				bool diassembledCodeOnly = mnuShowOnlyDisassembledCode.Checked;
				bool skippingCode = false;
				foreach(string line in _code.Split('\n')) {
					string[] lineParts = line.Split(':');
					if(skippingCode && (lineParts.Length != 2 || lineParts[1][0] != '.')) {
						lineNumbers.Add(-1);
						codeLines.Add("[code not disassembled]");

						int previousAddress = lineParts[0].Length > 0 ? (int)ParseHexAddress(lineParts[0])-1 : 0xFFFF;
						lineNumbers.Add(previousAddress);
						codeLines.Add("[code not disassembled]");

						skippingCode = false;
					}

					if(lineParts.Length == 2) {
						if(diassembledCodeOnly && lineParts[1][0] == '.') {
							if(!skippingCode) {
								lineNumbers.Add((int)ParseHexAddress(lineParts[0]));
								codeLines.Add("[code not disassembled]");
								skippingCode = true;
							}
						} else {
							lineNumbers.Add((int)ParseHexAddress(lineParts[0]));
							codeLines.Add(lineParts[1]);
						}
					}
				}

				ctrlCodeViewer.TextLines = codeLines.ToArray();
				ctrlCodeViewer.LineNumbers = lineNumbers.ToArray();
				sw.Stop();
				_codeChanged = false;
				return true;
			}
			return false;
		}

		private int GetAddressLine(UInt32 address)
		{
			int attempts = 8;
			do {
				int lineIndex = this.ctrlCodeViewer.GetLineIndex((int)address);
				if(lineIndex >= 0) {
					return lineIndex;
				}
				address--;
				attempts--;
			} while(attempts > 0);
			return -1;
		}

		private UInt32 ParseHexAddress(string hexAddress)
		{
			return UInt32.Parse(hexAddress, System.Globalization.NumberStyles.AllowHexSpecifier);
		}

		public void HighlightBreakpoints(List<Breakpoint> breakpoints)
		{
			ctrlCodeViewer.ClearLineStyles();
			if(_currentActiveAddress.HasValue) {
				SetActiveAddress(_currentActiveAddress.Value);
			}
			foreach(Breakpoint breakpoint in breakpoints) {
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
				}

				ctrlCodeViewer.SetLineColor((int)breakpoint.Address, fgColor, bgColor, outlineColor, symbol);
			}
		}

		public int GetCurrentLine()
		{
			return this.ctrlCodeViewer.CurrentLine;
		}

		public void GoToAddress()
		{
			GoToAddress address = new GoToAddress();
			if(_currentActiveAddress.HasValue) {
				address.Address = _currentActiveAddress.Value;
			}

			frmGoToLine frm = new frmGoToLine(address);
			frm.StartPosition = FormStartPosition.Manual;
			Point topLeft = this.PointToScreen(new Point(0, 0));
			frm.Location = new Point(topLeft.X + (this.Width - frm.Width) / 2, topLeft.Y + (this.Height - frm.Height) / 2);
			if(frm.ShowDialog() == DialogResult.OK) {
				ctrlCodeViewer.ScrollToLineNumber((int)address.Address);
			}
		}

		#region Events
		private Point _previousLocation;
		private void ctrlCodeViewer_MouseMove(object sender, MouseEventArgs e)
		{
			if(_previousLocation != e.Location) {
				string word = GetWordUnderLocation(e.Location);
				if(word.StartsWith("$")) {
					UInt32 address = UInt32.Parse(word.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);
					Byte memoryValue = InteropEmu.DebugGetMemoryValue(address);
					string valueText = "$" + memoryValue.ToString("X");
					toolTip.Show(valueText, ctrlCodeViewer, e.Location.X + 5, e.Location.Y + 5, 3000);
				}
				_previousLocation = e.Location;
			}
		}

		UInt32 _lastClickedAddress = UInt32.MaxValue;
		private void ctrlCodeViewer_MouseUp(object sender, MouseEventArgs e)
		{
			string word = GetWordUnderLocation(e.Location);
			if(word.StartsWith("$")) {
				_lastClickedAddress = UInt32.Parse(word.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);
				mnuGoToLocation.Enabled = true;
				mnuGoToLocation.Text = "Go to Location (" + word + ")";

				mnuAddToWatch.Enabled = true;
				mnuAddToWatch.Text = "Add to Watch (" + word + ")";
			} else {
				mnuGoToLocation.Enabled = false;
				mnuGoToLocation.Text = "Go to Location";
				mnuAddToWatch.Enabled = false;
				mnuAddToWatch.Text = "Add to Watch";
			}
		}

		#region Context Menu
		private void contextMenuCode_Opening(object sender, CancelEventArgs e)
		{
			mnuShowNextStatement.Enabled = _currentActiveAddress.HasValue;
			mnuSetNextStatement.Enabled = false;
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
		}

		private void mnuGoToLocation_Click(object sender, EventArgs e)
		{
			this.ctrlCodeViewer.ScrollToLineNumber((int)_lastClickedAddress);
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			if(this.OnWatchAdded != null) {
				this.OnWatchAdded(new WatchAddedEventArgs() { Address = _lastClickedAddress});
			}
		}
		#endregion

		#endregion
	}

	public class WatchAddedEventArgs : EventArgs
	{
		public UInt32 Address { get; set; }
	}
}
