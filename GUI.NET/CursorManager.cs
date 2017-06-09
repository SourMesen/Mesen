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

namespace Mesen.GUI
{
	public class CursorManager
	{
		private static bool _cursorHidden = false;
		private static Timer _tmrMouse = new Timer();

		static CursorManager()
		{
			_tmrMouse.Interval = 3000;
			_tmrMouse.Tick += tmrMouse_Tick;
		}

		private static void tmrMouse_Tick(object sender, EventArgs e)
		{
			if(InteropEmu.IsRunning() && !InteropEmu.IsPaused()) {
				HideMouse();
			} else {
				ShowMouse();
				_tmrMouse.Stop();
			}
		}

		private static void ShowMouse()
		{
			if(_cursorHidden) {
				Cursor.Show();
				_cursorHidden = false;
			}
		}

		private static void HideMouse()
		{
			if(!_cursorHidden) {
				Cursor.Hide();
				_cursorHidden = true;
			}
		}

		public static bool NeedMouseIcon
		{
			get { return InteropEmu.GetExpansionDevice() == InteropEmu.ExpansionPortDevice.OekaKidsTablet || InteropEmu.HasZapper(); }
		}

		public static void OnMouseMove(Control ctrl)
		{
			if(!InteropEmu.IsRunning() || InteropEmu.IsPaused() || !InteropEmu.HasArkanoidPaddle()) {
				ShowMouse();
			} else if(InteropEmu.HasArkanoidPaddle() && !CursorManager.NeedMouseIcon) {
				HideMouse();
			}

			_tmrMouse.Stop();

			if(!CursorManager.NeedMouseIcon) {
				ctrl.Cursor = Cursors.Default;

				//Only hide mouse if no zapper (otherwise this could be pretty annoying)
				_tmrMouse.Start();
			}
		}

		public static void OnMouseLeave()
		{
			_tmrMouse.Stop();
			ShowMouse();
		}
	}
}
