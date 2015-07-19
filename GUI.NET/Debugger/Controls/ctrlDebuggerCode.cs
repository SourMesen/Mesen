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

			txtCode.SyncedTextbox = txtCodeMargin;
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
				}
			}
		}

		private UInt32? _currentActiveAddress = null;
		public void SelectActiveAddress(UInt32 address)
		{
			txtCode.BeginUpdate();
			txtCodeMargin.BeginUpdate();

			if(!UpdateCode()) {
				RemoveActiveHighlight();
			}

			int lineIndex = GetAddressLine(address);
			if(lineIndex >= 0) {
				if(!IsLineVisible(lineIndex)) {
					//Scroll active line to middle of viewport if it wasn't already visible
					ScrollToLine(lineIndex);
				}
				//Set line background to yellow
				HighlightLine(lineIndex, Color.Yellow, Color.Black);
			}
			txtCode.EndUpdate();
			txtCodeMargin.EndUpdate();

			_currentActiveAddress = address;
		}

		public void RemoveActiveHighlight()
		{
			//Didn't update code, need to remove previous highlighting
			if(_currentActiveAddress.HasValue) {
				int lineIndex = GetAddressLine(_currentActiveAddress.Value);
				if(lineIndex != -1) {
					HighlightLine(lineIndex, Color.White, Color.Black);
				}
			}
		}

		public int GetCurrentLine()
		{
			int cursorPos = txtCode.GetFirstCharIndexOfCurrentLine();
			return txtCode.GetLineFromCharIndex(cursorPos);
		}

		public string GetWordUnderLocation(Point position)
		{
			int charIndex = txtCode.GetCharIndexFromPosition(position);

			if(txtCode.Text.Length > charIndex) {
				List<char> wordDelimiters = new List<char>(new char[] { ' ', '\r', '\n', ',' });
				if(wordDelimiters.Contains(txtCode.Text[charIndex])) {
					return string.Empty;
				} else {
					int endIndex = txtCode.Text.IndexOfAny(wordDelimiters.ToArray(), charIndex);
					int startIndex = txtCode.Text.LastIndexOfAny(wordDelimiters.ToArray(), charIndex);
					return txtCode.Text.Substring(startIndex + 1, endIndex - startIndex - 1);
				}
			} else {
				return string.Empty;
			}
		}
		
		private int GetLineCount()
		{
			return (int)((float)txtCodeMargin.Height / (txtCodeMargin.Font.GetHeight() + 1)) - 1;
		}

		private void ScrollToAddress(UInt32 address)
		{
			int lineIndex = GetAddressLine(address);
			if(lineIndex != -1 && !IsLineVisible(lineIndex)) {
				ScrollToLine(lineIndex);
			}
		}

		private void ScrollToLine(int lineIndex)
		{
			lineIndex = Math.Max(0, lineIndex - GetLineCount()/2);
			txtCodeMargin.SelectionStart = txtCodeMargin.GetFirstCharIndexFromLine(lineIndex);
			txtCodeMargin.ScrollToCaret();
			txtCode.SelectionStart = txtCode.GetFirstCharIndexFromLine(lineIndex);
			txtCode.ScrollToCaret();
		}

		private bool IsLineVisible(int lineIndex)
		{
			int firstLine = txtCodeMargin.GetLineFromCharIndex(txtCodeMargin.GetCharIndexFromPosition(new Point(0, 0)));
			return lineIndex >= firstLine && lineIndex <= firstLine+GetLineCount();
		}

		public bool UpdateCode(bool forceUpdate = false)
		{
			if(_codeChanged || forceUpdate) {
				StringBuilder lineNumbers = new StringBuilder();
				StringBuilder codeString = new StringBuilder();
				bool diassembledCodeOnly = mnuShowOnlyDisassembledCode.Checked;
				bool skippingCode = false;
				foreach(string line in _code.Split('\n')) {
					string[] lineParts = line.Split(':');
					if(skippingCode && (lineParts.Length != 2 || lineParts[1][0] != '.')) {
						lineNumbers.AppendLine(" .. ");
						codeString.AppendLine("[code not disassembled]");

						UInt32 previousAddress = lineParts[0].Length > 0 ? ParseHexAddress(lineParts[0])-1 : 0xFFFF;
						lineNumbers.AppendLine(previousAddress.ToString("X"));
						codeString.AppendLine("[code not disassembled]");

						skippingCode = false;
					}

					if(lineParts.Length == 2) {
						if(diassembledCodeOnly && lineParts[1][0] == '.') {
							if(!skippingCode) {
								lineNumbers.AppendLine(lineParts[0]);
								codeString.AppendLine("[code not disassembled]");
								skippingCode = true;
							}
						} else {
							lineNumbers.AppendLine(lineParts[0]);
							codeString.AppendLine(lineParts[1]);
						}
					}
				}
				txtCode.Text = codeString.ToString();
				txtCodeMargin.Text = lineNumbers.ToString();
				txtCodeMargin.SelectAll();
				txtCodeMargin.SelectionAlignment = HorizontalAlignment.Right;
				txtCodeMargin.SelectionLength = 0;
				_codeChanged = false;
				return true;
			}
			return false;
		}

		private int GetAddressLine(UInt32 address)
		{
			int attempts = 8;
			do {
				int pos = txtCodeMargin.Text.IndexOf(address.ToString("x"), StringComparison.InvariantCultureIgnoreCase);
				if(pos >= 0) {
					return txtCodeMargin.GetLineFromCharIndex(pos);
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

		private UInt32 GetLineAddress(int lineIndex)
		{
			int lineStartIndex = txtCodeMargin.GetFirstCharIndexFromLine(lineIndex);
			txtCodeMargin.SelectionStart = lineStartIndex;
			txtCodeMargin.SelectionLength = 4;
			return ParseHexAddress(txtCodeMargin.SelectedText);
		}

		public void HighlightLine(int lineIndex, Color bgColor, Color fgColor)
		{
			int lineStartIndex = txtCodeMargin.GetFirstCharIndexFromLine(lineIndex);
			txtCodeMargin.SelectionStart = lineStartIndex;
			txtCodeMargin.SelectionLength = 4;
			txtCodeMargin.SelectionBackColor = bgColor;
			txtCodeMargin.SelectionColor = fgColor;
			txtCodeMargin.SelectionLength = 0;

			lineStartIndex = txtCode.GetFirstCharIndexFromLine(lineIndex);
			txtCode.SelectionStart = lineStartIndex;
			txtCode.SelectionLength = txtCode.GetFirstCharIndexFromLine(lineIndex+1) - lineStartIndex;
			txtCode.SelectionBackColor = bgColor;
			txtCode.SelectionColor = fgColor;
			txtCode.SelectionLength = 0;
		}

		public void ToggleBreakpoint()
		{
			HighlightLine(GetCurrentLine(), Color.FromArgb(158, 84, 94), Color.White);
			InteropEmu.DebugAddBreakpoint(BreakpointType.Execute, GetLineAddress(GetCurrentLine()), false);
		}

		#region Events
		private Point _previousLocation;
		private void txtCode_MouseMove(object sender, MouseEventArgs e)
		{
			if(_previousLocation != e.Location) {
				string word = GetWordUnderLocation(e.Location);
				if(word.StartsWith("$")) {
					UInt32 address = UInt32.Parse(word.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);
					Byte memoryValue = InteropEmu.DebugGetMemoryValue(address);
					string valueText = "$" + memoryValue.ToString("X");
					toolTip.Show(valueText, txtCode, e.Location.X + 5, e.Location.Y + 5, 3000);
				}
				_previousLocation = e.Location;
			}
		}

		UInt32 _lastClickedAddress = UInt32.MaxValue;
		private void txtCode_MouseUp(object sender, MouseEventArgs e)
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

		private void txtCodeMargin_Enter(object sender, EventArgs e)
		{
			txtCode.Focus();
		}

		#region Context Menu
		private void contextMenuCode_Opening(object sender, CancelEventArgs e)
		{
			mnuShowNextStatement.Enabled = _currentActiveAddress.HasValue;
			mnuSetNextStatement.Enabled = false;
		}

		private void mnuShowNextStatement_Click(object sender, EventArgs e)
		{
			ScrollToAddress(_currentActiveAddress.Value);
		}

		private void mnuShowOnlyDisassembledCode_Click(object sender, EventArgs e)
		{
			UpdateCode(true);
		}

		private void mnuGoToLocation_Click(object sender, EventArgs e)
		{
			ScrollToAddress(_lastClickedAddress);
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
