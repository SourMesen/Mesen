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
		private bool _cursorHidden = false;

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
			if(_cursorHidden) {
				Cursor.Show();
				_cursorHidden = false;
			}
			tmrMouse.Stop();

			if(InteropEmu.HasZapper()) {
				this.Cursor = Cursors.Cross;
			} else {
				this.Cursor = Cursors.Default;

				//Only hide mouse if no zapper (otherwise this could be pretty annoying)
				tmrMouse.Start();
			}

			if(_rightButtonDown) {
				InteropEmu.ZapperSetPosition(0, -1, -1);
				InteropEmu.ZapperSetPosition(1, -1, -1);
			} else {
				double xPos = (double)e.X / this.Width;
				double yPos = (double)e.Y / this.Height;
				InteropEmu.ZapperSetPosition(0, xPos, yPos);
				InteropEmu.ZapperSetPosition(1, xPos, yPos);
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

		private void tmrMouse_Tick(object sender, EventArgs e)
		{
			_cursorHidden = true;
			Cursor.Hide();
		}

		private void ctrlRenderer_MouseLeave(object sender, EventArgs e)
		{
			tmrMouse.Stop();
			if(_cursorHidden) {
				_cursorHidden = false;
				Cursor.Show();
			}
		}
	}
}
