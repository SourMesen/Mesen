using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	[Flags]
	public enum LineSymbol
	{
		None = 0,
		Circle = 1,
		CircleOutline = 2,
		Arrow = 4,
	}

	public enum eHistoryType
	{
		Always,
		OnScroll,
		None
	}

	public class LineProperties
	{
		public Color? BgColor;
		public Color? FgColor;
		public Color? OutlineColor;
		public LineSymbol Symbol;
	}

	public partial class ctrlTextbox : Control
	{
		public event EventHandler ScrollPositionChanged;
		private bool _disableScrollPositionChangedEvent;

		private const float HorizontalScrollFactor = 8;
		private const int CommentSpacingCharCount = 25;

		private TextboxHistory _history = new TextboxHistory();

		private string[] _contents = new string[0];
		private string[] _contentNotes = new string[0];
		private string[] _compareContents = null;
		private int[] _lineNumbers = new int[0];
		private int[] _lineMargins = new int[0];		
		private string[] _lineNumberNotes = new string[0];
		private Dictionary<int, int> _lineNumberIndex = new Dictionary<int,int>();
		private bool _showLineNumbers = false;
		private bool _showLineInHex = false;
		private bool _showLineNumberNotes = false;
		private bool _showSingleLineLineNumberNotes = false;
		private bool _showContentNotes = false;
		private bool _showSingleLineContentNotes = true;
		private int _selectionStart = 0;
		private int _selectionLength = 0;
		private int _scrollPosition = 0;
		private int _horizontalScrollPosition = 0;
		private string _searchString = null;
		private string _header = null;
		private Font _noteFont = null;
		private int _marginWidth = 9;
		private int _extendedMarginWidth = 13;
		private float _maxLineWidth = 0;
		private int _maxLineWidthIndex = 0;

		public ctrlTextbox()
		{
			InitializeComponent();
			this.ResizeRedraw = true;
			this.DoubleBuffered = true;
		}
		
		public string[] Comments;
		public string[] Addressing;
		public int[] LineIndentations;

		public string[] TextLines
		{
			set
			{
				int maxLength = 0;

				_maxLineWidthIndex = 0;
				
				_contents = value;
				for(int i = 0, len = value.Length; i < len; i++) {
					int length = _contents[i].Length + (Addressing != null ? Addressing[i].Length : 0);
					if(Comments?[i].Length > 0) {
						length = Math.Max(length, length > 0 ? CommentSpacingCharCount : 0) + Comments[i].Length;
					}
					if(length > maxLength) {
						maxLength = length;
						_maxLineWidthIndex = i;
					}
				}

				UpdateHorizontalScrollWidth();

				if(_lineNumbers.Length != _contents.Length) {
					_lineNumbers = new int[_contents.Length];
					_lineNumberIndex.Clear();
					for(int i = _contents.Length - 1; i >=0; i--) {
						_lineNumbers[i] = i;
						_lineNumberIndex[i] = i;
					}
				}
				this.Invalidate();
			}
		}

		//Cache Font.Height value because accessing it is slow
		private new int FontHeight { get; set; }

		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				this.FontHeight = value.Height;
				_noteFont = new Font(value.FontFamily, value.Size * 0.75f);
				UpdateHorizontalScrollWidth();
			}
		}

		public bool ShowSingleContentLineNotes
		{
			get { return _showSingleLineContentNotes; }
			set
			{
				_showSingleLineContentNotes = value;
				this.Invalidate();
			}
		}

		public bool ShowContentNotes
		{
			get { return _showContentNotes; }
			set 
			{
				_showContentNotes = value;
				this.Invalidate();
			}
		}

		public bool ShowSingleLineLineNumberNotes
		{
			get { return _showSingleLineLineNumberNotes; }
			set
			{
				_showSingleLineLineNumberNotes = value;
				this.Invalidate();
			}
		}

		public bool ShowLineNumberNotes
		{
			get { return this._showLineNumberNotes; }
			set 
			{ 
				this._showLineNumberNotes = value;
				this.Invalidate();
			}
		}

		public string[] TextLineNotes
		{
			set
			{
				this._contentNotes = value;
				this.Invalidate();
			}
		}

		public string[] CompareLines
		{
			set
			{
				_compareContents = value;
			}
		}

		public int LineCount
		{
			get
			{
				return _contents.Length;
			}
		}

		public int[] LineNumbers
		{
			set
			{
				_lineNumbers = value;
				_lineNumberIndex.Clear();
				int i = 0;
				foreach(int line in _lineNumbers) {
					_lineNumberIndex[line] = i;
					i++;
				}
				this.Invalidate();
			}
		}

		public string[] LineNumberNotes
		{
			set
			{
				_lineNumberNotes = value;
				this.Invalidate();
			}
		}

		public string Header
		{
			set
			{
				this._header = value;
				this.Invalidate();
			}
		}

		public int MarginWidth
		{
			set
			{
				this._marginWidth = value;
				this.Invalidate();
			}
		}

		public List<Tuple<int, int, string>> FindAllOccurrences(string text, bool matchWholeWord, bool matchCase)
		{
			List<Tuple<int, int, string>> result = new List<Tuple<int, int, string>>();
			string regex;
			if(matchWholeWord) {
				regex = $"[^0-9a-zA-Z_#@]+{Regex.Escape(text)}[^0-9a-zA-Z_#@]+";
			} else {
				regex = Regex.Escape(text);
			}

			for(int i = 0, len = _contents.Length; i < len; i++) {
				string line = _contents[i] + Addressing?[i] + (Comments != null ? ("\t" + Comments[i]) : null);
				if(Regex.IsMatch(line, regex, matchCase ? RegexOptions.None : RegexOptions.IgnoreCase)) {
					if(line.StartsWith("__") && line.EndsWith("__")) {
						line = "Block: " + line.Substring(2, line.Length - 4);
					}

					if(line.StartsWith("--") && line.EndsWith("--")) {
						continue;
					}

					int j = i;
					while(j < _lineNumbers.Length && _lineNumbers[j] < 0) {
						j++;
					}

					var searchResult = new Tuple<int, int, string>(_lineNumbers[j], i, line);
					result.Add(searchResult);
				}
			}
			return result;
		}

		public bool Search(string searchString, bool searchBackwards, bool isNewSearch)
		{
			if(string.IsNullOrWhiteSpace(searchString)) {
				this._searchString = null;
				this.Invalidate();
				return true;
			} else {
				int startPosition;
				int endPosition;

				this._searchString = searchString.ToLowerInvariant();
				int searchOffset = (searchBackwards ? -1 : 1);
				if(isNewSearch) {
					startPosition = this.SelectionStart;
					endPosition = this.SelectionStart - searchOffset;
					if(endPosition < 0) {
						endPosition = _contents.Length - 1;
					} else if(endPosition >= _contents.Length) {
						endPosition = 0;
					}

				} else {
					startPosition = this.SelectionStart + searchOffset;
					endPosition = this.SelectionStart;
					if(startPosition < 0) {
						startPosition = _contents.Length - 1;
					} else if(startPosition >= _contents.Length) {
						startPosition = 0;
					}
				}

				for(int i = startPosition; i != endPosition; i += searchOffset) {
					string line = _contents[i].ToLowerInvariant();
					if(line.Contains(this._searchString)) {
						this.ScrollToLineIndex(i);
						this.Invalidate();
						return true;
					}

					//Continue search from start/end of document
					if(!searchBackwards && i == this._contents.Length - 1) {
						i = 0;
					} else if(searchBackwards && i == 0) {
						i = this._contents.Length - 1;
					}
				}
				this.Invalidate();
				return _contents[_selectionStart].ToLowerInvariant().Contains(this._searchString);
			}
		}

		public interface ILineStyleProvider
		{
			LineProperties GetLineStyle(int cpuAddress);
		}

		private ILineStyleProvider _styleProvider;
		public ILineStyleProvider StyleProvider
		{
			get { return _styleProvider; }
			set
			{
				_styleProvider = value;
				this.Invalidate();
			}
		}

		public LineProperties GetLineStyle(int lineNumber)
		{
			if(StyleProvider != null && _lineNumbers[lineNumber] >= 0) {
				return StyleProvider.GetLineStyle(_lineNumbers[lineNumber]);
			} else {
				return null;
			}
		}

		public int GetLineIndex(int lineNumber)
		{
			if(_lineNumberIndex.ContainsKey(lineNumber)) {
				return _lineNumberIndex[lineNumber];
			} else {
				foreach(int line in _lineNumbers) {
					if(line > lineNumber) {
						return Math.Max(0, GetLineIndex(line) - 1);
					}
				}
			}
			return -1;
		}

		public int GetLineNumber(int lineIndex)
		{
			if(_lineNumbers.Length <= lineIndex) {
				return 0;
			} else {
				return _lineNumbers[lineIndex];
			}
		}

		public void ScrollToLineIndex(int lineIndex, eHistoryType historyType = eHistoryType.Always)
		{
			if(this.SelectionStart != lineIndex) {
				bool scrolled = false;
				if(lineIndex < this.ScrollPosition || lineIndex > this.GetLastVisibleLineIndex()) {
					//Line isn't currently visible, scroll it to the middle of the viewport
					this.ScrollPosition = lineIndex - this.GetNumberVisibleLines()/2;
					scrolled = true;
				}

				if(historyType == eHistoryType.Always || scrolled && historyType == eHistoryType.OnScroll) {
					_history.AddHistory(this.SelectionStart);
				}
				this.SelectionStart = lineIndex;
				this.SelectionLength = 0;
				if(historyType == eHistoryType.Always || scrolled && historyType == eHistoryType.OnScroll) {
					_history.AddHistory(this.SelectionStart);
				}
			}
		}

		public void ScrollToLineNumber(int lineNumber, eHistoryType historyType = eHistoryType.Always)
		{
			int lineIndex = this.GetLineIndex(lineNumber);
			if(lineIndex >= 0) {
				ScrollToLineIndex(lineIndex, historyType);
			}
		}

		public int CodeMargin
		{
			get
			{
				using(Graphics g = Graphics.FromHwnd(this.Handle)) {
					return this.GetMargin(g, false);
				}
			}
		}

		private int GetMargin(Graphics g, bool getExtendedMargin)
		{
			int marginWidth = getExtendedMargin && this.ShowContentNotes && this.ShowSingleContentLineNotes ? _marginWidth + _extendedMarginWidth : _marginWidth;
			return (this.ShowLineNumbers ? (int)(g.MeasureString("".PadLeft(marginWidth, 'W'), this.Font, int.MaxValue, StringFormat.GenericTypographic).Width) : 0) - 1;
		}

		public int GetLineIndexAtPosition(int yPos)
		{
			int charIndex;
			int lineIndex;
			GetCharIndex(new Point(0, yPos), out charIndex, out lineIndex);
			return lineIndex;
		}

		private string GetFullWidthString(int lineIndex)
		{
			string text = _contents[lineIndex] + Addressing?[lineIndex];
			if(Comments?[lineIndex].Length > 0) {
				return text.PadRight(text.Length > 0 ? CommentSpacingCharCount : 0) + Comments[lineIndex];
			}
			return text;
		}

		private bool GetCharIndex(Point position, out int charIndex, out int lineIndex)
		{
			charIndex = -1;
			using(Graphics g = Graphics.FromHwnd(this.Handle)) {
				int marginLeft = this.GetMargin(g, true);
				int positionX = position.X - marginLeft;
				lineIndex = this.ScrollPosition + this.GetLineAtPosition(position.Y);
				if(lineIndex > _contents.Length && _contents.Length != 0) {
					lineIndex = _contents.Length - 1;
				}

				if(positionX >= 0 && lineIndex < _contents.Length) {
					string text = this.GetFullWidthString(lineIndex);
					//Adjust background color highlights based on number of spaces in front of content
					positionX -= (LineIndentations != null ? LineIndentations[lineIndex] : 0);

					int previousWidth = 0;
					for(int i = 0, len = text.Length; i < len; i++) {
						int width = (int)g.MeasureString(text.Substring(0, i+1), this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;
						if(width >= positionX && previousWidth <= positionX) {
							charIndex = i;
							return true;
						}
						previousWidth = width;
					}
				}
			}
			return false;
		}

		public string GetWordUnderLocation(Point position, bool useCompareText = false)
		{
			int charIndex; 
			int lineIndex;
			if(this.GetCharIndex(position, out charIndex, out lineIndex)) {
				string text = (useCompareText && _compareContents != null) ? _compareContents[lineIndex] : this.GetFullWidthString(lineIndex);
				List<char> wordDelimiters = new List<char>(new char[] { ' ', ',', '|', ';', '(', ')', '.', '-', ':' });
				if(wordDelimiters.Contains(text[charIndex])) {
					return string.Empty;
				} else {
					int endIndex = text.IndexOfAny(wordDelimiters.ToArray(), charIndex);
					if(endIndex == -1) {
						endIndex = text.Length;
					}
					int startIndex = text.LastIndexOfAny(wordDelimiters.ToArray(), charIndex);
					return text.Substring(startIndex + 1, endIndex - startIndex - 1);
				}
			}
			return string.Empty;
		}

		private int GetLineAtPosition(int yPos)
		{
			if(!string.IsNullOrWhiteSpace(this._header)) {
				yPos -= this.LineHeight;
			}
			return Math.Max(0, yPos / this.LineHeight);
		}

		private int GetLastVisibleLineIndex()
		{
			return this.ScrollPosition + this.GetNumberVisibleLines() - 1;
		}

		public int GetNumberVisibleLines()
		{
			Rectangle rect = this.ClientRectangle;
			return this.GetLineAtPosition(rect.Height);
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionStart
		{
			get { return Math.Min(this._contents.Length - 1, Math.Max(0, _selectionStart)); }
			set
			{
				int selectionStart = Math.Max(0, Math.Min(this._contents.Length - 1, Math.Max(0, value)));
				
				_selectionStart = selectionStart;

				if(this.SelectionLength == 0) {
					this.SelectedLine = this.SelectionStart;
				}

				this.Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionLength
		{
			get { return (this.SelectionStart + _selectionLength) > this._contents.Length - 1 ? this._contents.Length - this.SelectionStart - 1 : _selectionLength; }
			set
			{
				_selectionLength = value;

				if(this.SelectionStart + _selectionLength > this._contents.Length - 1) {
					_selectionLength = this._contents.Length - this.SelectionStart - 1;
				}

				if(value == 0) {
					this.SelectedLine = this.SelectionStart;
				}

				this.Invalidate();
			}
		}
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedLine
		{
			get { return this._selectedLine; }
			set
			{
				this._selectedLine = value;
				if(_selectedLine < this.ScrollPosition) {
					this.ScrollPosition = _selectedLine;
				} else if(_selectedLine > this.GetLastVisibleLineIndex()) {
					this.ScrollPosition = _selectedLine - this.GetNumberVisibleLines() + 1;
				}
				this.Invalidate();
			}
		}
		private int _selectedLine = 0;

		public void MoveSelectionDown(int lines = 1)
		{
			_disableScrollPositionChangedEvent = true;
			while(lines > 0) {
				bool singleLineSelection = this.SelectionLength == 0;

				if(singleLineSelection) {
					if(this.SelectionStart + this.SelectionLength >= this._contents.Length - 1) {
						//End of document reached
						break;
					}
					this.SelectedLine = this.SelectionStart + 1;
					this.SelectionLength++;
				} else if(this.SelectionStart + this.SelectionLength == this.SelectedLine) {
					if(this.SelectionStart + this.SelectionLength >= this._contents.Length - 1) {
						//End of document reached
						break;
					}
					this.SelectedLine++;
					this.SelectionLength++;
				} else {
					this.SelectionStart++;
					this.SelectedLine++;
					this.SelectionLength--;
				}
				lines--;
			}
			_disableScrollPositionChangedEvent = false;
			ScrollPositionChanged?.Invoke(this, null);
		}

		public void MoveSelectionUp(int lines = 1)
		{
			_disableScrollPositionChangedEvent = true;
			while(lines > 0) {
				bool singleLineSelection = this.SelectionLength == 0;

				if(singleLineSelection) {
					if(this.SelectionStart == 0) {
						//Top of document reached
						break;
					}
					this.SelectionStart--;
					this.SelectedLine = this.SelectionStart;
					this.SelectionLength++;
				} else if(this.SelectionStart == this.SelectedLine) {
					if(this.SelectionStart == 0) {
						//Top of document reached
						break;
					}
					this.SelectionStart--;
					this.SelectedLine--;
					this.SelectionLength++;
				} else {
					this.SelectedLine--;
					this.SelectionLength--;
				}
				lines--;
			}
			_disableScrollPositionChangedEvent = false;
			ScrollPositionChanged?.Invoke(this, null);
		}

		public int CurrentLine
		{
			get { return _lineNumbers.Length > _selectionStart ? _lineNumbers[_selectionStart] : 0; }
		}

		public int LastSelectedLine
		{
			get { return _lineNumbers.Length > _selectionStart + this.SelectionLength ? _lineNumbers[_selectionStart + this.SelectionLength] : 0; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPosition
		{
			get { return _scrollPosition; }
			set 
			{
				value = Math.Max(0, Math.Min(value, this._contents.Length-this.GetNumberVisibleLines()));
				_scrollPosition = value;
				if(!_disableScrollPositionChangedEvent && this.ScrollPositionChanged != null) {
					ScrollPositionChanged(this, null);
				}
				this.Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int HorizontalScrollPosition
		{
			get { return _horizontalScrollPosition; }
			set
			{
				_horizontalScrollPosition = value;
				if(!_disableScrollPositionChangedEvent && this.ScrollPositionChanged != null) {
					ScrollPositionChanged(this, null);
				}
				this.Invalidate();
			}
		}

		public int HorizontalScrollWidth { get; set; } = 0;

		private void UpdateHorizontalScrollWidth()
		{
			if(_contents.Length > _maxLineWidthIndex) {
				using(Graphics g = this.CreateGraphics()) {
					_maxLineWidth = (LineIndentations != null ? LineIndentations[_maxLineWidthIndex] : 0) + g.MeasureString(GetFullWidthString(_maxLineWidthIndex), this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;
					HorizontalScrollWidth = (int)(Math.Max(0, HorizontalScrollFactor + _maxLineWidth - (this.Width - GetMargin(g, true))) / HorizontalScrollFactor);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.ScrollPosition = this.ScrollPosition;
			UpdateHorizontalScrollWidth();
			ScrollPositionChanged?.Invoke(this, e);
		}

		public bool ShowLineNumbers
		{
			get { return _showLineNumbers; }
			set { _showLineNumbers = value; }
		}

		public bool ShowLineInHex
		{
			get { return _showLineInHex; }
			set { _showLineInHex = value; }
		}

		private int LineHeight
		{
			get 
			{
				if(this.ShowLineNumberNotes && !this.ShowSingleLineLineNumberNotes || this.ShowContentNotes && !this.ShowSingleContentLineNotes) {
					return (int)(this.FontHeight * 1.60);
				} else {
					return this.FontHeight - 1;
				}
			}
		}

		int _clickedLine;
		bool _mouseDragging;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.Focus();

			if(e.Button == MouseButtons.XButton1) {
				this.NavigateBackward();
			} else if(e.Button == MouseButtons.XButton2) {
				this.NavigateForward();
			} else {
				_clickedLine = this.ScrollPosition + this.GetLineAtPosition(e.Y);

				if(e.Button == MouseButtons.Right) {
					if(_clickedLine >= this.SelectionStart && _clickedLine <= this.SelectionStart + this.SelectionLength) {
						//Right-clicking on selection should not change it
						return;
					}
				}

				if(Control.ModifierKeys.HasFlag(Keys.Shift)) {
					if(_clickedLine > this.SelectedLine) {
						MoveSelectionDown(_clickedLine - this.SelectedLine);
					} else {
						MoveSelectionUp(this.SelectedLine - _clickedLine);
					}
				} else {
					_mouseDragging = true;
					this.SelectedLine = _clickedLine;
					this.SelectionStart = _clickedLine;
					this.SelectionLength = 0;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_mouseDragging = false;
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(_mouseDragging) {
				int lineUnderMouse = this.ScrollPosition + this.GetLineAtPosition(e.Y);
				this.SelectedLine = lineUnderMouse;
				this.SelectedLine = lineUnderMouse;
				if(lineUnderMouse > _clickedLine) {
					this.SelectionLength = lineUnderMouse - _clickedLine;
				} else {
					this.SelectedLine = lineUnderMouse;
					this.SelectionStart = lineUnderMouse;
					this.SelectionLength = _clickedLine - lineUnderMouse;
				}
			}
			base.OnMouseMove(e);
		}

		public void CopySelection()
		{
			StringBuilder sb = new StringBuilder();
			for(int i = this.SelectionStart, end = this.SelectionStart + this.SelectionLength; i <= end; i++) {
				string indent = "";
				if(LineIndentations != null) {
					indent = "".PadLeft(LineIndentations[i] / 10);
				}

				string codeString = _contents[i].Trim();
				if(codeString.StartsWith("__") || codeString.StartsWith("--")) {
					codeString = "--------" + codeString.Substring(2, codeString.Length - 4) + "--------";
				}

				string commentString = Comments?[i].Trim() ?? "";
				int padding = Math.Max(CommentSpacingCharCount, codeString.Length);
				if(codeString.Length == 0) {
					padding = 0;
				}

				codeString = codeString.PadRight(padding);
				sb.AppendLine(indent + codeString + commentString);
			}
			Clipboard.SetText(sb.ToString());
		}

		public void NavigateForward()
		{
			this.ScrollToLineIndex(_history.GoForward(), eHistoryType.None);
		}

		public void NavigateBackward()
		{
			this.ScrollToLineIndex(_history.GoBack(), eHistoryType.None);
		}

		private void DrawLine(Graphics g, int currentLine, int marginLeft, int positionY, int lineHeight)
		{
			string codeString = _contents[currentLine];
			string addressString = this.Addressing?[currentLine];
			string commentString = this.Comments?[currentLine];

			float codeStringLength = g.MeasureString(codeString, this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;
			float addressStringLength = g.MeasureString(addressString, this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;

			if(currentLine >= this.SelectionStart && currentLine <= this.SelectionStart + this.SelectionLength) {
				//Highlight current line
				using(Brush brush = new SolidBrush(Color.FromArgb(230, 238, 255))) {
					int offset = currentLine -  1 == this.SelectedLine ? 1 : 0;
					g.FillRectangle(brush, marginLeft, positionY + offset, Math.Max(_maxLineWidth, this.ClientRectangle.Width), lineHeight - offset);
				}
				if(currentLine == this.SelectedLine) {
					g.DrawRectangle(Pens.Blue, marginLeft + 1, positionY+1, Math.Max(_maxLineWidth, this.ClientRectangle.Width - marginLeft) - 1, lineHeight);
				}
			}

			//Adjust background color highlights based on number of spaces in front of content
			marginLeft += (LineIndentations != null ? LineIndentations[currentLine] : 0);

			Color textColor = Color.Black;
			LineProperties lineProperties = GetLineStyle(currentLine);
			if(lineProperties != null) {
				//Process background, foreground, outline color and line symbol
				textColor = lineProperties.FgColor ?? Color.Black;

				if(lineProperties.BgColor.HasValue) {
					using(Brush bgBrush = new SolidBrush(lineProperties.BgColor.Value)) {
						g.FillRectangle(bgBrush, marginLeft, positionY + 1, codeStringLength, lineHeight-1);
					}
				}
				if(lineProperties.OutlineColor.HasValue) {
					using(Pen outlinePen = new Pen(lineProperties.OutlineColor.Value, 1)) {
						g.DrawRectangle(outlinePen, marginLeft, positionY + 1, codeStringLength, lineHeight-1);
					}
				}
			}

			this.DrawLineText(g, currentLine, marginLeft, positionY, codeString, addressString, commentString, codeStringLength, addressStringLength, textColor, lineHeight);
		}

		private void DrawLineNumber(Graphics g, int currentLine, int marginLeft, int positionY)
		{
			if(this.ShowLineNumberNotes && this.ShowSingleLineLineNumberNotes) {
				//Display line note instead of line number
				string lineNumber;
				if(string.IsNullOrEmpty(_lineNumberNotes[currentLine])) {
					lineNumber = _lineNumbers[currentLine] >= 0 ? _lineNumbers[currentLine].ToString(_showLineInHex ? "X4" : "") : "..";
				} else {
					lineNumber = _lineNumberNotes[currentLine];
				}
				float width = g.MeasureString(lineNumber, this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;
				g.DrawString(lineNumber, this.Font, Brushes.Gray, marginLeft - width, positionY, StringFormat.GenericTypographic);
			} else {
				//Display line number
				string lineNumber = _lineNumbers[currentLine] >= 0 ? _lineNumbers[currentLine].ToString(_showLineInHex ? "X4" : "") : "..";
				float width = g.MeasureString(lineNumber, this.Font, int.MaxValue, StringFormat.GenericTypographic).Width;
				g.DrawString(lineNumber, this.Font, Brushes.Gray, marginLeft - width, positionY, StringFormat.GenericTypographic);

				if(this.ShowLineNumberNotes && !this.ShowSingleLineLineNumberNotes) {
					//Display line note below line number
					width = g.MeasureString(_lineNumberNotes[currentLine], _noteFont, int.MaxValue, StringFormat.GenericTypographic).Width;
					g.DrawString(_lineNumberNotes[currentLine], _noteFont, Brushes.Gray, marginLeft - width, positionY+this.Font.Size+3, StringFormat.GenericTypographic);
				}
			}
		}

		private void DrawLineText(Graphics g, int currentLine, int marginLeft, int positionY, string codeString, string addressString, string commentString, float codeStringLength, float addressStringLength, Color textColor, int lineHeight)
		{
			using(Brush fgBrush = new SolidBrush(textColor)) {
				if(codeString.StartsWith("--") && codeString.EndsWith("--")) {
					//Draw block start
					g.TranslateTransform(HorizontalScrollPosition * HorizontalScrollFactor, 0);
					string text = codeString.Substring(2, codeString.Length - 4);
					float textLength = g.MeasureString(text, this._noteFont, int.MaxValue, StringFormat.GenericTypographic).Width;
					g.DrawString(text, this._noteFont, fgBrush, (marginLeft + this.Width - textLength) / 2, positionY, StringFormat.GenericTypographic);
					g.DrawLine(Pens.Black, marginLeft, positionY+lineHeight-2, marginLeft+this.Width, positionY+lineHeight-2);
					g.TranslateTransform(-HorizontalScrollPosition * HorizontalScrollFactor, 0);
				} else if(codeString.StartsWith("__") && codeString.EndsWith("__")) {
					//Draw block end
					g.TranslateTransform(HorizontalScrollPosition * HorizontalScrollFactor, 0);
					string text = codeString.Substring(2, codeString.Length - 4);
					float textLength = g.MeasureString(text, this._noteFont, int.MaxValue, StringFormat.GenericTypographic).Width;
					g.DrawString(text, this._noteFont, fgBrush, (marginLeft + this.Width - textLength) / 2, positionY + 4, StringFormat.GenericTypographic);
					g.DrawLine(Pens.Black, marginLeft, positionY+2, marginLeft+this.Width, positionY+2);
					g.TranslateTransform(-HorizontalScrollPosition * HorizontalScrollFactor, 0);
				} else {
					//Draw line content
					g.DrawString(codeString, this.Font, fgBrush, marginLeft, positionY, StringFormat.GenericTypographic);

					if(!string.IsNullOrWhiteSpace(addressString)) {
						using(Brush addressBrush = new SolidBrush(Color.SteelBlue)) {
							g.DrawString(addressString, this.Font, addressBrush, marginLeft + codeStringLength, positionY, StringFormat.GenericTypographic);
						}
					}

					if(!string.IsNullOrWhiteSpace(commentString)) {
						using(Brush commentBrush = new SolidBrush(Color.DarkGreen)) {
							int padding = Math.Max(CommentSpacingCharCount, codeString.Length + addressString.Length);
							if(codeString.Length == 0 && addressString.Length == 0) {
								//Draw comment left-aligned, next to the margin when there is no code on the line
								padding = 0;
							}
							g.DrawString(commentString.PadLeft(padding+commentString.Length), this.Font, commentBrush, marginLeft, positionY, StringFormat.GenericTypographic);
						}
					}

					if(this.ShowContentNotes && !this.ShowSingleContentLineNotes) {
						g.DrawString(_contentNotes[currentLine], _noteFont, Brushes.Gray, marginLeft, positionY + this.Font.Size+3, StringFormat.GenericTypographic);
					}
					this.DrawHighlightedSearchString(g, codeString, marginLeft, positionY);
					this.DrawHighlightedCompareString(g, codeString, currentLine, marginLeft, positionY);
				}
			}
		}

		private void DrawLineSymbols(Graphics g, int positionY, LineProperties lineProperties, int lineHeight)
		{
			if(lineProperties.Symbol.HasFlag(LineSymbol.Circle)) {
				using(Brush brush = new SolidBrush(lineProperties.OutlineColor.Value)) {
					g.FillEllipse(brush, 3, positionY + 4, lineHeight - 3, lineHeight - 3);
				}
			}
			if(lineProperties.Symbol.HasFlag(LineSymbol.CircleOutline) && lineProperties.OutlineColor.HasValue) {
				using(Pen pen = new Pen(lineProperties.OutlineColor.Value, 1)) {
					g.DrawEllipse(pen, 3, positionY + 4, lineHeight - 3, lineHeight - 3);
				}
			}
			if(lineProperties.Symbol.HasFlag(LineSymbol.Arrow)) {
				int arrowY = positionY + lineHeight / 2 + 1;
				if(Program.IsMono) {
					using(Brush brush = new SolidBrush(lineProperties.BgColor.Value)) {
						g.FillRectangle(brush, 1, arrowY - lineHeight * 0.25f / 2, lineHeight - 1, lineHeight * 0.35f); 
					}
					g.DrawRectangle(Pens.Black, 1, arrowY - lineHeight * 0.25f / 2, lineHeight - 1, lineHeight * 0.35f); 
				} else {
					using(Pen pen = new Pen(Color.Black, lineHeight * 0.33f)) {
						//Outline
						g.DrawLine(pen, 3, arrowY, 3 + lineHeight * 0.25f, arrowY);
						pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
						g.DrawLine(pen, 3 + lineHeight * 0.25f, arrowY, 3 + lineHeight * 0.75f, arrowY);

						//Fill
						pen.Width-=2f;
						pen.Color = lineProperties.BgColor.Value;
						pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
						g.DrawLine(pen, 4, arrowY, 3 + lineHeight * 0.25f - 1, arrowY);
						pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
						g.DrawLine(pen, 3 + lineHeight * 0.25f, arrowY, lineHeight * 0.75f + 1, arrowY);
					}
				}
			}
		}

		private void DrawHighlightedCompareString(Graphics g, string lineText, int currentLine, int marginLeft, int positionY)
		{
			if(_compareContents != null && _compareContents.Length > currentLine) {
				string compareText = _compareContents[currentLine];

				if(compareText != lineText) {
					StringBuilder sb = new StringBuilder();
					for(int i = 0, len = lineText.Length; i < len; i++) {
						if(lineText[i] == compareText[i]) {
							sb.Append(" ");
						} else {
							sb.Append(lineText[i]);
						}
					}

					g.DrawString(sb.ToString(), new Font(this.Font, FontStyle.Bold), Brushes.Red, marginLeft, positionY, StringFormat.GenericTypographic);
				}
			}
		}

		private void DrawHighlightedSearchString(Graphics g, string lineText, int marginLeft, int positionY)
		{
			int searchIndex;
			if(!string.IsNullOrWhiteSpace(this._searchString) && (searchIndex = lineText.ToLowerInvariant().IndexOf(this._searchString)) >= 0) {
				//Draw colored search string
				string lowerCaseText = lineText.ToLowerInvariant();

				Action<bool> draw = (bool forBackground) => {
					int index = searchIndex;
					do {
						string padding = string.Empty.PadLeft(index, 'A');
						string highlightedText = lineText.Substring(index, this._searchString.Length);
						index = lowerCaseText.IndexOf(this._searchString, index + this._searchString.Length);

						SizeF size = g.MeasureString(highlightedText, this.Font, int.MaxValue, StringFormat.GenericTypographic);
						SizeF offsetSize = g.MeasureString(padding, this.Font, int.MaxValue, StringFormat.GenericTypographic);
						if(forBackground) {
							g.FillRectangle(Brushes.CornflowerBlue, marginLeft + offsetSize.Width, positionY + 1, size.Width + 1, size.Height - 2);
						} else {
							g.DrawString(highlightedText, this.Font, Brushes.White, marginLeft + offsetSize.Width + 1, positionY, StringFormat.GenericTypographic);
						}
					} while(index >= 0);
				};

				draw(true);
				draw(false);
			}
		}

		private void DrawMargin(Graphics g, int currentLine, int marginLeft, int regularMargin, int positionY, int lineHeight)
		{
			if(this.ShowLineNumbers) {
				//Show line number
				this.DrawLineNumber(g, currentLine, regularMargin, positionY);
			}
			if(this.ShowContentNotes && this.ShowSingleContentLineNotes) {
				g.DrawString(_contentNotes[currentLine], this.Font, Brushes.Gray, regularMargin + 6, positionY, StringFormat.GenericTypographic);
			}

			//Adjust background color highlights based on number of spaces in front of content
			marginLeft += (LineIndentations != null ? LineIndentations[currentLine] : 0);

			LineProperties lineProperties = GetLineStyle(currentLine);
			if(lineProperties != null) {
				this.DrawLineSymbols(g, positionY, lineProperties, lineHeight);
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			int lineHeight = this.LineHeight;
			pe.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			Rectangle rect = this.ClientRectangle;
			pe.Graphics.FillRectangle(Brushes.White, rect);

			pe.Graphics.TranslateTransform(-HorizontalScrollPosition * HorizontalScrollFactor, 0);

			int marginLeft = this.GetMargin(pe.Graphics, true);
			int regularMargin = this.GetMargin(pe.Graphics, false);
			int currentLine = this.ScrollPosition;
			int positionY = 0;

			if(!string.IsNullOrWhiteSpace(this._header)) {
				using(Brush lightGrayBrush = new SolidBrush(Color.FromArgb(240, 240, 240))) {
					pe.Graphics.FillRectangle(lightGrayBrush, marginLeft, 0, Math.Max(_maxLineWidth, rect.Right), lineHeight);
				}
				pe.Graphics.DrawString(_header, this.Font, Brushes.Gray, marginLeft, positionY);
				positionY += lineHeight;
			}

			while(positionY < rect.Bottom && currentLine < _contents.Length) {
				this.DrawLine(pe.Graphics, currentLine, marginLeft, positionY, lineHeight);
				positionY += lineHeight;
				currentLine++;
			}

			pe.Graphics.TranslateTransform(HorizontalScrollPosition * HorizontalScrollFactor, 0);

			if(this.ShowLineNumbers) {
				using(Brush brush = new SolidBrush(Color.FromArgb(235, 235, 235))) {
					pe.Graphics.FillRectangle(brush, 0, 0, regularMargin, rect.Bottom);
				}
				using(Brush brush = new SolidBrush(Color.FromArgb(251, 251, 251))) {
					pe.Graphics.FillRectangle(brush, regularMargin, 0, marginLeft - regularMargin, rect.Bottom);
				}

				using(Pen pen = new Pen(Color.LightGray)) {
					pe.Graphics.DrawLine(pen, regularMargin, rect.Top, regularMargin, rect.Bottom);
					pe.Graphics.DrawLine(pen, marginLeft, rect.Top, marginLeft, rect.Bottom);
				}
			}

			currentLine = this.ScrollPosition;
			positionY = string.IsNullOrWhiteSpace(this._header) ? 0 : lineHeight;
			while(positionY < rect.Bottom && currentLine < _contents.Length) {
				this.DrawMargin(pe.Graphics, currentLine, marginLeft, regularMargin, positionY, lineHeight);
				positionY += lineHeight;
				currentLine++;
			}
		}
	}
}
