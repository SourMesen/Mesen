using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
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

		private string[] _contents = new string[0];
		private string[] _contentNotes = new string[0];
		private string[] _compareContents = null;
		private int[] _lineNumbers = new int[0];
		private string[] _lineNumberNotes = new string[0];
		private Dictionary<int, int> _lineNumberIndex = new Dictionary<int,int>();
		private Dictionary<int, LineProperties> _lineProperties = new Dictionary<int,LineProperties>();
		private bool _showLineNumbers = false;
		private bool _showLineInHex = false;
		private bool _showLineNumberNotes = false;
		private bool _showContentNotes = false;
		private int _cursorPosition = 0;
		private int _scrollPosition = 0;
		private string _searchString = null;
		private string _header = null;
		private Font _noteFont = null;
		private int _marginWidth = 6;

		public ctrlTextbox()
		{
			InitializeComponent();
			this.ResizeRedraw = true;
			this.DoubleBuffered = true;
		}
		
		public string[] TextLines
		{
			set
			{
				_contents = value;
				_lineNumbers = new int[_contents.Length];
				_lineNumberIndex.Clear();
				for(int i = _contents.Length - 1; i >=0; i--) {
					_lineNumbers[i] = i;
					_lineNumberIndex[i] = i;
				}
				this.Invalidate();
			}
		}

		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				_noteFont = new Font(value.FontFamily, value.Size * 0.75f);
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
					startPosition = this.CursorPosition;
					endPosition = this.CursorPosition - searchOffset;
					if(endPosition < 0) {
						endPosition = _contents.Length - 1;
					} else if(endPosition >= _contents.Length) {
						endPosition = 0;
					}

				} else {
					startPosition = this.CursorPosition + searchOffset;
					endPosition = this.CursorPosition;
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
				return _contents[_cursorPosition].ToLowerInvariant().Contains(this._searchString);
			}
		}

		public void ClearLineStyles()
		{
			_lineProperties.Clear();
			this.Invalidate();
		}

		public void SetLineColor(int lineNumber, Color? fgColor = null, Color? bgColor = null, Color? outlineColor = null, LineSymbol symbol = LineSymbol.None)
		{
			if(lineNumber != -1) {
				if(_lineNumberIndex.ContainsKey(lineNumber)) {
					LineProperties properties = new LineProperties() {
						BgColor = bgColor,
						FgColor = fgColor,
						OutlineColor = outlineColor,
						Symbol = symbol
					};

					_lineProperties[_lineNumberIndex[lineNumber]] = properties;
					this.Invalidate();
				}
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

		public void ScrollToLineIndex(int lineIndex)
		{
			if(lineIndex < this.ScrollPosition || lineIndex > this.GetLastVisibleLineIndex()) {
				//Line isn't currently visible, scroll it to the middle of the viewport
				this.ScrollPosition = lineIndex - this.GetNumberVisibleLines()/2;
			}
			this.CursorPosition = lineIndex;
		}

		public void ScrollToLineNumber(int lineNumber)
		{
			int lineIndex = this.GetLineIndex(lineNumber);
			if(lineIndex >= 0) {
				ScrollToLineIndex(lineIndex);
			}
		}

		public int CodeMargin
		{
			get
			{
				using(Graphics g = Graphics.FromHwnd(this.Handle)) {
					return this.GetMargin(g);
				}
			}
		}

		private int GetMargin(Graphics g)
		{
			return this.ShowLineNumbers ? (int)(g.MeasureString("W", this.Font).Width * _marginWidth) : 0;
		}

		public int GetLineIndexAtPosition(int yPos)
		{
			int charIndex;
			int lineIndex;
			GetCharIndex(new Point(0, yPos), out charIndex, out lineIndex);
			return lineIndex;
		}

		private bool GetCharIndex(Point position, out int charIndex, out int lineIndex)
		{
			charIndex = -1;
			using(Graphics g = Graphics.FromHwnd(this.Handle)) {
				int marginLeft = this.GetMargin(g);
				int positionX = position.X - marginLeft;
				lineIndex = this.ScrollPosition + this.GetLineAtPosition(position.Y);
				if(lineIndex > _contents.Length && _contents.Length != 0) {
					lineIndex = _contents.Length - 1;
				}

				if(positionX >= 0 && lineIndex < _contents.Length) {
					string text = _contents[lineIndex];
					int previousWidth = 0;
					for(int i = 0, len = text.Length; i < len; i++) {
						int width = (int)g.MeasureString(text.Substring(0, i+1), this.Font).Width;
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
				string text = (useCompareText && _compareContents != null) ? _compareContents[lineIndex] : _contents[lineIndex];
				List<char> wordDelimiters = new List<char>(new char[] { ' ', ',', '|', ';', '(', ')' });
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
		public int CursorPosition
		{
			get { return Math.Min(this._contents.Length - 1, Math.Max(0, _cursorPosition)); }
			set
			{ 
				_cursorPosition = Math.Max(0, Math.Min(this._contents.Length - 1, Math.Max(0, value)));
				if(_cursorPosition < this.ScrollPosition) {
					this.ScrollPosition = _cursorPosition;
				} else if(_cursorPosition > this.GetLastVisibleLineIndex()) {
					this.ScrollPosition = _cursorPosition - this.GetNumberVisibleLines() + 1;
				}
				this.Invalidate();
			}
		}

		public int CurrentLine
		{
			get { return _lineNumbers.Length > _cursorPosition ? _lineNumbers[_cursorPosition] : 0; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPosition
		{
			get { return _scrollPosition; }
			set 
			{
				value = Math.Max(0, Math.Min(value, this._contents.Length-1));
				_scrollPosition = value;
				if(this.ScrollPositionChanged != null) {
					ScrollPositionChanged(this, null);
				}
				this.Invalidate();
			}
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
				if(this.ShowLineNumberNotes || this.ShowContentNotes) {
					return (int)(this.Font.Height * 1.60);
				} else {
					return this.Font.Height - 1;
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.Focus();
			int clickedLine = this.GetLineAtPosition(e.Y);
			this.CursorPosition = this.ScrollPosition + clickedLine;
		}

		private void DrawLine(Graphics g, int currentLine, int marginLeft, int positionY)
		{
			string[] lineContent = _contents[currentLine].Split(new string[] { "||" }, StringSplitOptions.None);
			string codeString = lineContent.Length > 0 ? lineContent[0] : "";
			string addressString = lineContent.Length > 1 ? lineContent[1] : "";
			string commentString = lineContent.Length > 2 ? lineContent[2] : "";

			float codeStringLength = g.MeasureString(codeString, this.Font).Width;
			float addressStringLength = g.MeasureString(addressString, this.Font).Width;

			if(this.ShowLineNumbers) {
				//Show line number
				string lineNumber = _lineNumbers[currentLine] >= 0 ? _lineNumbers[currentLine].ToString(_showLineInHex ? "X4" : "") : "..";
				float width = g.MeasureString(lineNumber, this.Font).Width;
				g.DrawString(lineNumber, this.Font, Brushes.Gray, marginLeft - width, positionY);
				if(this.ShowLineNumberNotes) {
					width = g.MeasureString(_lineNumberNotes[currentLine], _noteFont).Width;
					g.DrawString(_lineNumberNotes[currentLine], _noteFont, Brushes.Gray, marginLeft - width, positionY+this.Font.Size+3);
				}
			}

			if(currentLine == this.CursorPosition) {
				//Highlight current line
				g.FillRectangle(Brushes.AliceBlue, marginLeft, positionY, this.ClientRectangle.Width - marginLeft, this.LineHeight);
			}

			Color textColor = Color.Black;
			if(_lineProperties.ContainsKey(currentLine)) {
				//Process background, foreground, outline color and line symbol
				LineProperties lineProperties = _lineProperties[currentLine];
				textColor = lineProperties.FgColor ?? Color.Black;

				if(lineProperties.BgColor.HasValue) {
					using(Brush bgBrush = new SolidBrush(lineProperties.BgColor.Value)) {
						g.FillRectangle(bgBrush, marginLeft + 1, positionY + 1, codeStringLength, this.LineHeight-1);
					}
				}
				if(lineProperties.OutlineColor.HasValue) {
					using(Pen outlinePen = new Pen(lineProperties.OutlineColor.Value, 1)) {
						g.DrawRectangle(outlinePen, marginLeft + 1, positionY + 1, codeStringLength, this.LineHeight-1);
					}
				}

				if(lineProperties.Symbol.HasFlag(LineSymbol.Circle)) {
					using(Brush brush = new SolidBrush(lineProperties.OutlineColor.Value)) {
						g.FillEllipse(brush, 1, positionY + 2, this.LineHeight - 3, this.LineHeight - 3);
					}
				} 
				if(lineProperties.Symbol.HasFlag(LineSymbol.CircleOutline) && lineProperties.OutlineColor.HasValue) {
					using(Pen pen = new Pen(lineProperties.OutlineColor.Value, 1)) {
						g.DrawEllipse(pen, 1, positionY + 2, this.LineHeight - 3, this.LineHeight - 3);
					}
				}
				if(lineProperties.Symbol.HasFlag(LineSymbol.Arrow)) {
					int arrowY = positionY + this.LineHeight / 2 + 1;
					using(Pen pen = new Pen(Color.Black, this.LineHeight * 0.33f)) {
						//Outline
						g.DrawLine(pen, 3, arrowY, 3 + this.LineHeight * 0.25f, arrowY);
						pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
						g.DrawLine(pen, 3 + this.LineHeight * 0.25f, arrowY, 3 + this.LineHeight * 0.75f, arrowY);

						//Fill
						pen.Width-=2f;
						pen.Color = lineProperties.BgColor.Value;
						pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
						g.DrawLine(pen, 4, arrowY, 3 + this.LineHeight * 0.25f - 1, arrowY);
						pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
						g.DrawLine(pen, 3 + this.LineHeight * 0.25f, arrowY, this.LineHeight * 0.75f + 1, arrowY);
					}
				}
			}

			using(Brush fgBrush = new SolidBrush(textColor)) {
				g.DrawString(codeString, this.Font, fgBrush, marginLeft, positionY);

				using(Brush addressBrush = new SolidBrush(Color.SteelBlue)) {
					g.DrawString(addressString, this.Font, addressBrush, marginLeft + codeStringLength, positionY);
				}
				using(Brush commentBrush = new SolidBrush(Color.DarkGreen)) {
					g.DrawString(commentString, this.Font, commentBrush, Math.Max(marginLeft + 220, marginLeft + codeStringLength + addressStringLength), positionY);
				}

				if(this.ShowContentNotes) {
					g.DrawString(_contentNotes[currentLine], _noteFont, Brushes.Gray, marginLeft, positionY + this.Font.Size+3);
				}
				this.DrawHighlightedSearchString(g, codeString, marginLeft, positionY);
				this.DrawHighlightedCompareString(g, codeString, currentLine, marginLeft, positionY);
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

					g.DrawString(sb.ToString(), new Font(this.Font, FontStyle.Bold), Brushes.Red, marginLeft, positionY);
				}
			}
		}

		private void DrawHighlightedSearchString(Graphics g, string lineText, int marginLeft, int positionY)
		{
			int searchIndex;
			if(!string.IsNullOrWhiteSpace(this._searchString) && (searchIndex = lineText.ToLowerInvariant().IndexOf(this._searchString)) >= 0) {
				//Draw colored search string
				int previousSearchIndex = -this._searchString.Length;
				string lowerCaseText = lineText.ToLowerInvariant();
				StringBuilder sb = new StringBuilder();
				StringBuilder sbBackground = new StringBuilder();
				do {
					sb.Append(string.Empty.PadLeft(searchIndex - previousSearchIndex - this._searchString.Length));
					sbBackground.Append(string.Empty.PadLeft(searchIndex - previousSearchIndex - this._searchString.Length));
					
					sb.Append(lineText.Substring(searchIndex, this._searchString.Length));
					sbBackground.Append(string.Empty.PadLeft(this._searchString.Length, '█'));

					previousSearchIndex = searchIndex;
					searchIndex = lowerCaseText.IndexOf(this._searchString, searchIndex + this._searchString.Length);
				} while(searchIndex >= 0);

				string drawSearchString = sb.ToString();
				string drawSearchStringBg = sbBackground.ToString();

				using(Brush selBrush = new SolidBrush(Color.White), selBgBrush = new SolidBrush(Color.CornflowerBlue)) {
					g.DrawString(drawSearchStringBg, this.Font, selBgBrush, marginLeft-1, positionY);
					g.DrawString(drawSearchStringBg, this.Font, selBgBrush, marginLeft+1, positionY);
					g.DrawString(drawSearchString, this.Font, selBrush, marginLeft, positionY);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			using(Brush lightGrayBrush = new SolidBrush(Color.FromArgb(240,240,240))) {
				using(Pen grayPen = new Pen(Color.LightGray)) {
					Rectangle rect = this.ClientRectangle;
					pe.Graphics.FillRectangle(Brushes.White, rect);

					int marginLeft = this.GetMargin(pe.Graphics);
					if(this.ShowLineNumbers) {
						pe.Graphics.FillRectangle(lightGrayBrush, 0, 0, marginLeft, rect.Bottom);
						pe.Graphics.DrawLine(grayPen, marginLeft, rect.Top, marginLeft, rect.Bottom);
					}

					int currentLine = this.ScrollPosition;
					int positionY = 0;

					if(!string.IsNullOrWhiteSpace(this._header)) {
						pe.Graphics.FillRectangle(lightGrayBrush, marginLeft, 0, rect.Right, this.LineHeight);
						pe.Graphics.DrawString(_header, this.Font, Brushes.Gray, marginLeft, positionY);
						positionY += this.LineHeight;
					}

					while(positionY < rect.Bottom && currentLine < _contents.Length) {
						this.DrawLine(pe.Graphics, currentLine, marginLeft, positionY);
						positionY += this.LineHeight;
						currentLine++;
					}
				}
			}
		}
	}
}
