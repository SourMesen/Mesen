using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Controls
{
	public class ctrlMesenToolStrip : ToolStrip
	{
		private const int WM_MOUSEACTIVATE = 0x21;
		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM_MOUSEACTIVATE && this.CanFocus && !this.Focused) {
				this.Focus();
			}
			base.WndProc(ref m);
		}
	}
}
