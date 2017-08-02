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
		private static Point _lastPosition;
		private static Timer _tmrHideMouse = new Timer();
		private static Timer _tmrCheckMouseMove = new Timer();

		static CursorManager()
		{
			_tmrHideMouse.Interval = 3000;
			_tmrHideMouse.Tick += tmrHideMouse_Tick;
			_tmrCheckMouseMove.Interval = 500;
			_tmrCheckMouseMove.Tick += tmrCheckMouseMove_Tick;
			_tmrCheckMouseMove.Start();
		}

		private static void tmrCheckMouseMove_Tick(object sender, EventArgs e)
		{
			//Rarely the cursor becomes hidden despite leaving the window or moving
			//Have not been able to find a reliable way to reproduce it yet
			//This is a patch to prevent that bug from having any negative impact
			if(_lastPosition != Cursor.Position) {
				if(!InteropEmu.HasArkanoidPaddle()) {
					ShowMouse();
				}
				_lastPosition = Cursor.Position;
			}
		}

		private static void tmrHideMouse_Tick(object sender, EventArgs e)
		{
			if(InteropEmu.IsRunning() && !InteropEmu.IsPaused()) {
				HideMouse();
			} else {
				ShowMouse();
				_tmrHideMouse.Stop();
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

			_tmrHideMouse.Stop();

			if(!CursorManager.NeedMouseIcon) {
				ctrl.Cursor = Cursors.Default;

				//Only hide mouse if no zapper (otherwise this could be pretty annoying)
				_tmrHideMouse.Start();
			}
		}

		public static void OnMouseLeave()
		{
			_tmrHideMouse.Stop();
			ShowMouse();
		}
	}
}
