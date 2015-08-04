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
	public partial class ctrlScrollableTextbox : UserControl
	{
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

		public ctrlScrollableTextbox()
		{
			InitializeComponent();

			this.ctrlTextbox.ShowLineNumbers = true;
			this.ctrlTextbox.ShowLineInHex = true;
			this.vScrollBar.ValueChanged += vScrollBar_ValueChanged;
			this.ctrlTextbox.ScrollPositionChanged += ctrlTextbox_ScrollPositionChanged;

			new ToolTip().SetToolTip(picCloseSearch, "Close");
			new ToolTip().SetToolTip(picSearchNext, "Find Next (F3)");
			new ToolTip().SetToolTip(picSearchPrevious, "Find Previous (Shift-F3)");
		}

		public string GetWordUnderLocation(Point position)
		{
			return this.ctrlTextbox.GetWordUnderLocation(position);
		}

		private void ctrlTextbox_ScrollPositionChanged(object sender, EventArgs e)
		{
			this.vScrollBar.Value = this.ctrlTextbox.ScrollPosition;
		}

		public void ClearLineStyles()
		{
			this.ctrlTextbox.ClearLineStyles();
		}

		public void SetLineColor(int lineNumber, Color? fgColor = null, Color? bgColor = null, Color? outlineColor = null, LineSymbol symbol = LineSymbol.None)
		{
			this.ctrlTextbox.SetLineColor(lineNumber, fgColor, bgColor, outlineColor, symbol);
		}

		public int GetLineIndex(int lineNumber)
		{
			return this.ctrlTextbox.GetLineIndex(lineNumber);
		}

		public int GetLineNumber(int lineIndex)
		{
			return this.ctrlTextbox.GetLineNumber(lineIndex);
		}

		public void ScrollToLineNumber(int lineNumber)
		{
			this.ctrlTextbox.ScrollToLineNumber(lineNumber);
		}

		public int CurrentLine
		{
			get { return this.ctrlTextbox.CurrentLine; }
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.vScrollBar.Value = Math.Min(this.vScrollBar.Maximum, Math.Max(0, this.vScrollBar.Value - e.Delta / 40));
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch(keyData) {
				case Keys.Down:
				case Keys.Right:
					this.ctrlTextbox.CursorPosition++;
					return true;
				case Keys.Up:
				case Keys.Left:
					this.ctrlTextbox.CursorPosition--;
					return true;

				case Keys.Home:
					this.ctrlTextbox.CursorPosition = 0;
					return true;

				case Keys.End:
					this.ctrlTextbox.CursorPosition = this.ctrlTextbox.LineCount - 1;
					return true;

				case Keys.PageUp:
					this.ctrlTextbox.CursorPosition-=20;
					return true;

				case Keys.PageDown:
					this.ctrlTextbox.CursorPosition+=20;
					return true;

				case Keys.Control | Keys.F:
					this.OpenSearchBox();
					return true;

				case Keys.Escape:
					this.CloseSearchBox();
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			this.ctrlTextbox.ScrollPosition = this.vScrollBar.Value;
		}
		
		public string[] TextLines
		{
			set
			{
				this.ctrlTextbox.TextLines = value;
				this.vScrollBar.Maximum = this.ctrlTextbox.LineCount;
			}
		}
		
		public int[] LineNumbers
		{
			set
			{
				this.ctrlTextbox.CustomLineNumbers = value;
			}
		}

		public void OpenSearchBox()
		{
			bool focus = !this.panelSearch.Visible;
			this.panelSearch.Visible = true;
			if(focus) {
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
	}
}
