using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Controls
{
	class MyListView : ListView
	{
		private bool preventCheck = false;

		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			if(this.preventCheck) {
				e.NewValue = e.CurrentValue;
				this.preventCheck = false;
			} else
				base.OnItemCheck(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left && e.Clicks > 1) {
				this.preventCheck = true;
			} else {
				this.preventCheck = false;
			}
			base.OnMouseDown(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.preventCheck = false;
			base.OnKeyDown(e);
		}
	}
}
