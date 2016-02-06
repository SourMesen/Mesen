using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Controls
{
	public partial class ctrlRenderer : UserControl
	{
		private bool _rightButtonDown = false;

		public ctrlRenderer()
		{
			InitializeComponent();
		}

		private void ctrlRenderer_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				InteropEmu.ZapperSetTriggerState(0, true);
				InteropEmu.ZapperSetTriggerState(1, true);
			} else if(e.Button == MouseButtons.Right) {
				_rightButtonDown = true;
			}
		}

		private void ctrlRenderer_MouseMove(object sender, MouseEventArgs e)
		{
			if(InteropEmu.HasZapper()) {
				this.Cursor = Cursors.Cross;
			} else {
				this.Cursor = Cursors.Default;
			}

			if(_rightButtonDown) {
				InteropEmu.ZapperSetPosition(0, -1, -1);
				InteropEmu.ZapperSetPosition(1, -1, -1);
			} else {
				//TODO
				InteropEmu.ZapperSetPosition(0, e.X/4, e.Y/4);
				InteropEmu.ZapperSetPosition(1, e.X/4, e.Y/4);
			}
		}

		private void ctrlRenderer_MouseUp(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				InteropEmu.ZapperSetTriggerState(0, false);
				InteropEmu.ZapperSetTriggerState(1, false);
			} else if(e.Button == MouseButtons.Right) {
				_rightButtonDown = false;
			}
		}
	}
}
