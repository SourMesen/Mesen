using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlScrollableTextbox : BaseControl
	{
		public event EventHandler ScrollPositionChanged;

		private bool _showScrollbars = true;

		public new event MouseEventHandler MouseUp
		{
			add { this.ctrlTextbox.MouseUp += value; }
			remove { this.ctrlTextbox.MouseUp -= value; }
		}

		public new event MouseEventHandler MouseMove
		{
			add { this.ctrlTextbox.MouseMove += value; }
			remove { this.ctrlTextbox.MouseMove -= value; }
		}

		public new event MouseEventHandler MouseDown
		{
			add { this.ctrlTextbox.MouseDown += value; }
			remove { this.ctrlTextbox.MouseDown -= value; }
		}

		public new event MouseEventHandler MouseDoubleClick
		{
			add { this.ctrlTextbox.MouseDoubleClick += value; }
			remove { this.ctrlTextbox.MouseDoubleClick -= value; }
		}

		public new event EventHandler MouseLeave
		{
			add { this.ctrlTextbox.MouseLeave += value; }
			remove { this.ctrlTextbox.MouseLeave -= value; }
		}

		public event EventHandler FontSizeChanged;

		public ctrlScrollableTextbox()
		{
			InitializeComponent();

			this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.panelSearch.Location = new System.Drawing.Point(this.Width - this.panelSearch.Width - 20, -1);

			this.ctrlTextbox.ShowLineNumbers = true;
			this.ctrlTextbox.ShowLineInHex = true;
			this.ctrlTextbox.Font = new System.Drawing.Font(BaseControl.MonospaceFontFamily, 13F);
			
			this.hScrollBar.ValueChanged += hScrollBar_ValueChanged;
			this.vScrollBar.ValueChanged += vScrollBar_ValueChanged;
			this.ctrlTextbox.ScrollPositionChanged += ctrlTextbox_ScrollPositionChanged;

			new ToolTip().SetToolTip(picCloseSearch, "Close");
			new ToolTip().SetToolTip(picSearchNext, "Find Next (F3)");
			new ToolTip().SetToolTip(picSearchPrevious, "Find Previous (Shift-F3)");
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.panelSearch.Location = new System.Drawing.Point(this.Width - this.panelSearch.Width - 20, -1);
		}

		public bool ShowScrollbars
		{
			set
			{
				this._showScrollbars = value;
				this.hScrollBar.Visible = value;
				this.vScrollBar.Visible = value;
			}
		}

		public float FontSize
		{
			get { return this.ctrlTextbox.Font.SizeInPoints; }
			set
			{
				if(value >= 6 && value <= 20) {
					this.ctrlTextbox.Font = new Font(BaseControl.MonospaceFontFamily, value);
					UpdateHorizontalScrollbar();
					this.ctrlTextbox.Invalidate();

					if(this.FontSizeChanged != null) {
						this.FontSizeChanged(this, null);
					}
				}
			}
		}

		public string GetWordUnderLocation(Point position, bool useCompareText = false)
		{
			return this.ctrlTextbox.GetWordUnderLocation(position, useCompareText);
		}

		private void ctrlTextbox_ScrollPositionChanged(object sender, EventArgs e)
		{
			this.vScrollBar.Value = this.ctrlTextbox.ScrollPosition;
			this.hScrollBar.Value = this.ctrlTextbox.HorizontalScrollPosition;
			UpdateHorizontalScrollbar();
			UpdateVerticalScrollbar();

			ScrollPositionChanged?.Invoke(null, null);
		}

		private void UpdateVerticalScrollbar()
		{
			this.vScrollBar.Maximum = Math.Max(0, this.ctrlTextbox.LineCount + this.vScrollBar.LargeChange - this.ctrlTextbox.GetNumberVisibleLines() + 1);
		}

		private void UpdateHorizontalScrollbar()
		{
			this.hScrollBar.Visible = this.ctrlTextbox.HorizontalScrollWidth > 0 && _showScrollbars;
			int newMax = this.ctrlTextbox.HorizontalScrollWidth + this.hScrollBar.LargeChange - 1;
			if(this.hScrollBar.Value > this.ctrlTextbox.HorizontalScrollWidth) {
				this.hScrollBar.Value = this.ctrlTextbox.HorizontalScrollWidth;
			}
			this.hScrollBar.Maximum = newMax;
		}

		public ctrlTextbox.ILineStyleProvider StyleProvider { set { this.ctrlTextbox.StyleProvider = value; } }

		public int GetLineIndex(int lineNumber)
		{
			return this.ctrlTextbox.GetLineIndex(lineNumber);
		}

		public int GetLineIndexAtPosition(int yPos)
		{
			return this.ctrlTextbox.GetLineIndexAtPosition(yPos);
		}

		public int GetLineNumber(int lineIndex)
		{
			return this.ctrlTextbox.GetLineNumber(lineIndex);
		}

		public int GetLineNumberAtPosition(int yPos)
		{
			return this.GetLineNumber(this.GetLineIndexAtPosition(yPos));
		}

		public void ScrollToLineIndex(int lineIndex)
		{
			this.ctrlTextbox.ScrollToLineIndex(lineIndex);
		}

		public void ScrollToLineNumber(int lineNumber, eHistoryType historyType = eHistoryType.Always, bool scrollToTop = false)
		{
			this.ctrlTextbox.ScrollToLineNumber(lineNumber, historyType, scrollToTop);
		}

		public void CopySelection()
		{
			this.ctrlTextbox.CopySelection();
		}
		
		public int CurrentLine
		{
			get { return this.ctrlTextbox.CurrentLine; }
		}
		
		public int SelectedLine
		{
			get { return this.ctrlTextbox.SelectedLine; }
		}

		public int LastSelectedLine
		{
			get { return this.ctrlTextbox.LastSelectedLine; }
		}

		public int CodeMargin
		{
			get { return this.ctrlTextbox.CodeMargin; }
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.vScrollBar.Value = Math.Min(this.vScrollBar.Maximum, Math.Max(0, this.vScrollBar.Value - e.Delta / 40));
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!this.cboSearch.Focused) {
				switch(keyData) {
					case Keys.Right | Keys.Shift:
					case Keys.Down | Keys.Shift:
						this.ctrlTextbox.MoveSelectionDown();
						return true;

					case Keys.Down:
					case Keys.Right:
						this.ctrlTextbox.SelectionStart = this.ctrlTextbox.SelectedLine + 1;
						this.ctrlTextbox.SelectionLength = 0;
						return true;

					case Keys.Up | Keys.Shift:
					case Keys.Left | Keys.Shift:
						this.ctrlTextbox.MoveSelectionUp();
						return true;

					case Keys.Up:
					case Keys.Left:
						this.ctrlTextbox.SelectionStart = this.ctrlTextbox.SelectedLine - 1;
						this.ctrlTextbox.SelectionLength = 0;
						return true;

					case Keys.Home | Keys.Shift:
						this.ctrlTextbox.MoveSelectionUp(this.ctrlTextbox.LineCount);
						break;

					case Keys.End | Keys.Shift:
						this.ctrlTextbox.MoveSelectionDown(this.ctrlTextbox.LineCount);
						break;

					case Keys.A | Keys.Control:
						this.ctrlTextbox.SelectionStart = 0;
						this.ctrlTextbox.SelectionLength = this.ctrlTextbox.LineCount;
						break;

					case Keys.Home:
						this.ctrlTextbox.SelectionStart = 0;
						this.ctrlTextbox.SelectionLength = 0;
						return true;

					case Keys.End:
						this.ctrlTextbox.SelectionStart = this.ctrlTextbox.LineCount - 1;
						this.ctrlTextbox.SelectionLength = 0;
						return true;
				}
			}

			switch(keyData) {
				case Keys.PageUp | Keys.Shift:
					this.ctrlTextbox.MoveSelectionUp(20);
					return true;

				case Keys.PageUp:
					this.ctrlTextbox.SelectionStart-=20;
					this.ctrlTextbox.SelectionLength = 0;
					return true;

				case Keys.PageDown | Keys.Shift:
					this.ctrlTextbox.MoveSelectionDown(20);
					return true;

				case Keys.PageDown:
					this.ctrlTextbox.SelectionStart+=20;
					this.ctrlTextbox.SelectionLength = 0;
					return true;

				case Keys.Control | Keys.F:
					this.OpenSearchBox(true);
					return true;

				case Keys.Escape:
					this.CloseSearchBox();
					return true;

				case Keys.Control | Keys.Oemplus:
					this.FontSize++;
					return true;

				case Keys.Control | Keys.OemMinus:
					this.FontSize--;
					return true;

				case Keys.Control | Keys.D0:
					this.FontSize = BaseControl.DefaultFontSize;
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			this.ctrlTextbox.ScrollPosition = this.vScrollBar.Value;
		}
		
		private void hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			this.ctrlTextbox.HorizontalScrollPosition = this.hScrollBar.Value;
		}

		public string[] Addressing { set { this.ctrlTextbox.Addressing = value; } }
		public string[] Comments { set { this.ctrlTextbox.Comments = value; } }
		public int[] LineIndentations{ set { this.ctrlTextbox.LineIndentations = value; } }

		public string[] TextLines
		{
			set
			{
				this.ctrlTextbox.TextLines = value;
				UpdateVerticalScrollbar();
				UpdateHorizontalScrollbar();
			}
		}

		public string[] TextLineNotes
		{
			set
			{
				this.ctrlTextbox.TextLineNotes = value;
			}
		}

		public string[] CompareLines
		{
			set
			{
				this.ctrlTextbox.CompareLines = value;
			}
		}
		
		public int[] LineNumbers
		{
			set
			{
				this.ctrlTextbox.LineNumbers = value;
			}
		}

		public string[] LineNumberNotes
		{
			set
			{
				this.ctrlTextbox.LineNumberNotes = value;
			}
		}

		public bool ShowSingleContentLineNotes
		{
			get { return this.ctrlTextbox.ShowSingleContentLineNotes; }
			set { this.ctrlTextbox.ShowSingleContentLineNotes = value; }
		}

		public bool ShowContentNotes
		{
			get { return this.ctrlTextbox.ShowContentNotes; }
			set { this.ctrlTextbox.ShowContentNotes = value; }
		}

		public bool ShowLineNumberNotes
		{
			get { return this.ctrlTextbox.ShowLineNumberNotes; }
			set { this.ctrlTextbox.ShowLineNumberNotes = value; }
		}

		public bool ShowSingleLineLineNumberNotes
		{
			get { return this.ctrlTextbox.ShowSingleLineLineNumberNotes; }
			set { this.ctrlTextbox.ShowSingleLineLineNumberNotes = value; }
		}

		public int LineCount { get { return this.ctrlTextbox.LineCount; } }
		public int SelectionStart { get { return this.ctrlTextbox.SelectionStart; } }
		public int SelectionLength { get { return this.ctrlTextbox.SelectionLength; } }

		public string Header
		{
			set
			{
				this.ctrlTextbox.Header = value;
			}
		}

		public int MarginWidth { set { this.ctrlTextbox.MarginWidth = value; } }

		public void OpenSearchBox(bool forceFocus = false)
		{
			bool focus = !this.panelSearch.Visible;
			this.panelSearch.Visible = true;
			if(focus || forceFocus) {
				this.cboSearch.Focus();
				this.cboSearch.SelectAll();
			}
		}

		private void CloseSearchBox()
		{
			this.ctrlTextbox.Search(null, false, false);
			this.panelSearch.Visible = false;
			this.Focus();
		}

		public void FindNext()
		{
			this.OpenSearchBox();
			this.ctrlTextbox.Search(this.cboSearch.Text, false, false);
		}

		public void FindPrevious()
		{
			this.OpenSearchBox();
			this.ctrlTextbox.Search(this.cboSearch.Text, true, false);
		}

		private void picCloseSearch_Click(object sender, EventArgs e)
		{
			this.CloseSearchBox();
		}

		private void picSearchPrevious_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindPrevious();
		}

		private void picSearchNext_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindNext();
		}

		private void cboSearch_TextUpdate(object sender, EventArgs e)
		{
			if(!this.ctrlTextbox.Search(this.cboSearch.Text, false, true)) {
				this.cboSearch.BackColor = Color.Coral;
			} else {
				this.cboSearch.BackColor = Color.Empty;
			}
		}

		private void cboSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter) {
				this.FindNext();
				if(this.cboSearch.Items.Contains(this.cboSearch.Text)) {
					this.cboSearch.Items.Remove(this.cboSearch.Text);
				}
				this.cboSearch.Items.Insert(0, this.cboSearch.Text);

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		public void GoToAddress()
		{
			GoToAddress address = new GoToAddress();

			int currentAddr = this.CurrentLine;
			int lineIndex = this.ctrlTextbox.SelectionStart;
			while(currentAddr < 0) {
				lineIndex++;
				currentAddr = this.ctrlTextbox.GetLineNumber(lineIndex);
			}

			address.Address = (UInt32)currentAddr;

			frmGoToLine frm = new frmGoToLine(address);
			frm.StartPosition = FormStartPosition.Manual;
			Point topLeft = this.PointToScreen(new Point(0, 0));
			frm.Location = new Point(topLeft.X + (this.Width - frm.Width) / 2, topLeft.Y + (this.Height - frm.Height) / 2);
			if(frm.ShowDialog() == DialogResult.OK) {
				this.ctrlTextbox.ScrollToLineNumber((int)address.Address);
			}
		}

		public List<Tuple<int, int, string>> FindAllOccurrences(string text, bool matchWholeWord, bool matchCase)
		{
			return this.ctrlTextbox.FindAllOccurrences(text, matchWholeWord, matchCase);
		}

		public void NavigateForward()
		{
			this.ctrlTextbox.NavigateForward();
		}

		public void NavigateBackward()
		{
			this.ctrlTextbox.NavigateBackward();
		}
	}
}
