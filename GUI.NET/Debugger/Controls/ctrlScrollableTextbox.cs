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

		public void ScrollIntoView(int lineNumber)
		{
			this.ctrlTextbox.ScrollIntoView(lineNumber);
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
					this.ctrlTextbox.CursorPosition++;
					return true;
				case Keys.Up:
					this.ctrlTextbox.CursorPosition--;
					return true;

				case Keys.PageUp:
					this.ctrlTextbox.CursorPosition-=20;
					return true;

				case Keys.PageDown:
					this.ctrlTextbox.CursorPosition+=20;
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
	}
}
