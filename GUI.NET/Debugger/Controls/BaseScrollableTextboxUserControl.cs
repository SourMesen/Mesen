using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public class BaseScrollableTextboxUserControl : BaseControl
	{
		virtual protected ctrlScrollableTextbox ScrollableTextbox
		{
			get
			{
				return null;
			}
		}

		public void OpenSearchBox()
		{
			this.ScrollableTextbox.OpenSearchBox();
		}

		public void FindNext()
		{
			this.ScrollableTextbox.FindNext();
		}

		public void FindPrevious()
		{
			this.ScrollableTextbox.FindPrevious();
		}

		public void GoToAddress()
		{
			this.ScrollableTextbox.GoToAddress();
		}

		public void ScrollToLineNumber(int lineNumber, bool scrollToTop = false)
		{
			this.ScrollableTextbox.ScrollToLineNumber(lineNumber, eHistoryType.Always, scrollToTop);
		}

		public void ScrollToLineIndex(int lineIndex)
		{
			this.ScrollableTextbox.ScrollToLineIndex(lineIndex);
		}

		public bool HideSelection
		{
			get { return this.ScrollableTextbox.HideSelection; }
			set { this.ScrollableTextbox.HideSelection = value; }
		}

		public int GetCurrentLine()
		{
			return this.ScrollableTextbox.CurrentLine;
		}

		public void ScrollToTop()
		{
			this.ScrollableTextbox.ScrollToLineNumber(0);
		}

		public string GetWordUnderLocation(Point position)
		{
			return this.ScrollableTextbox.GetWordUnderLocation(position);
		}
	}
}
