using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
		private int[] _lineNumbers = new int[0];
		private Dictionary<int, int> _lineNumberIndex = new Dictionary<int,int>();
		private Dictionary<int, LineProperties> _lineProperties = new Dictionary<int,LineProperties>();
		private bool _showLineNumbers = false;
		private bool _showLineInHex = false;
		private int _cursorPosition = 0;
		private int _scrollPosition = 0;
		private string _searchString = null;

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

		public int LineCount
		{
			get
			{
				return _contents.Length;
			}
		}

		public int[] CustomLineNumbers
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
					if(line.Contains(searchString)) {
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
			return _lineNumbers[lineIndex];
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

		private int GetMargin(Graphics g)
		{
			return this.ShowLineNumbers ? (int)(g.MeasureString("W", this.Font).Width * 6) : 0;
		}
		
		public string GetWordUnderLocation(Point position)
		{
			using(Graphics g = Graphics.FromHwnd(this.Handle)) {
				int marginLeft = this.GetMargin(g);
				int positionX = position.X - marginLeft;
				int lineOffset = position.Y / this.LineHeight;
				if(positionX >= 0 && this.ScrollPosition + lineOffset < _contents.Length) {
					string text = _contents[this.ScrollPosition + lineOffset];
					int charIndex = -1;
					int previousWidth = 0;
					for(int i = 0, len = text.Length; i < len; i++) {
						int width = (int)g.MeasureString(text.Substring(0, i+1), this.Font).Width;
						if(width >= positionX && previousWidth <= positionX) {
							charIndex = i;
							break;
						}
						previousWidth = width;
					}

					if(charIndex >= 0) {
						List<char> wordDelimiters = new List<char>(new char[] { ' ', ',' });
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
				}
			}
			return string.Empty;
		}

		private int GetLastVisibleLineIndex()
		{
			return this.ScrollPosition + this.GetNumberVisibleLines() - 1;
		}

		private int GetNumberVisibleLines()
		{
			Rectangle rect = this.ClientRectangle;
			return rect.Height / this.LineHeight;
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public int CursorPosition
		{
			get { return _cursorPosition; }
			set
			{ 
				_cursorPosition = Math.Min(this._contents.Length - 1, Math.Max(0, value));
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
			get { return _lineNumbers[_cursorPosition]; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
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
			get { return this.Font.Height - 1; }
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Focus();
			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if(e.Button == System.Windows.Forms.MouseButtons.Left) {
				int clickedLine = e.Y / this.LineHeight;
				this.CursorPosition = this.ScrollPosition + clickedLine;
			}
			base.OnMouseClick(e);
		}

		private void DrawLine(Graphics g, int currentLine, int marginLeft, int positionY)
		{
			if(this.ShowLineNumbers) {
				//Show line number
				string lineNumber = _lineNumbers[currentLine] >= 0 ? _lineNumbers[currentLine].ToString(_showLineInHex ? "X4" : "") : "..";
				float width = g.MeasureString(lineNumber, this.Font).Width;
				g.DrawString(lineNumber, this.Font, Brushes.Gray, marginLeft - width, positionY);
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

				float stringLength = g.MeasureString(_contents[currentLine], this.Font).Width;

				if(lineProperties.BgColor.HasValue) {
					using(Brush bgBrush = new SolidBrush(lineProperties.BgColor.Value)) {
						g.FillRectangle(bgBrush, marginLeft + 1, positionY + 1, stringLength, this.LineHeight-1);
					}
				}
				if(lineProperties.OutlineColor.HasValue) {
					using(Pen outlinePen = new Pen(lineProperties.OutlineColor.Value, 1)) {
						g.DrawRectangle(outlinePen, marginLeft + 1, positionY + 1, stringLength, this.LineHeight-1);
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
				int searchIndex;
				if(!string.IsNullOrWhiteSpace(this._searchString) && (searchIndex = _contents[currentLine].ToLowerInvariant().IndexOf(this._searchString)) >= 0) {
					//Draw colored search string
					string searchString = _contents[currentLine].Substring(searchIndex, this._searchString.Length);
					StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic) { FormatFlags = StringFormatFlags.MeasureTrailingSpaces };
					float searchStringWidth = g.MeasureString(searchString, this.Font, Int32.MaxValue, stringFormat).Width;
					g.DrawString(_contents[currentLine].Substring(0, searchIndex), this.Font, fgBrush, marginLeft, positionY);

					float offsetX = g.MeasureString(_contents[currentLine].Substring(0, searchIndex), this.Font, Int32.MaxValue, stringFormat).Width;
					using(Brush selBrush = new SolidBrush(Color.White), selBgBrush = new SolidBrush(Color.CornflowerBlue)) {
						g.FillRectangle(selBgBrush, marginLeft+offsetX+1, positionY, searchStringWidth+2, this.LineHeight);
						g.DrawString(searchString, this.Font, selBrush, marginLeft+offsetX, positionY);
					}
					offsetX += searchStringWidth;

					g.DrawString(_contents[currentLine].Substring(searchIndex+this._searchString.Length), this.Font, fgBrush, marginLeft+offsetX, positionY);
				} else {
					g.DrawString(_contents[currentLine], this.Font, fgBrush, marginLeft, positionY);
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
