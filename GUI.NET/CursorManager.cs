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
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	public class CursorManager
	{
		private static bool _cursorHidden = false;
		private static Point _lastPosition;
		private static Timer _tmrHideMouse = new Timer();
		private static Timer _tmrCheckMouseMove = new Timer();
		private static bool _mouseCaptured = false;

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
			if(!_mouseCaptured && _lastPosition != Cursor.Position) {
				ShowMouse();
				_lastPosition = Cursor.Position;
			}
		}

		private static void tmrHideMouse_Tick(object sender, EventArgs e)
		{
			if(InteropEmu.IsRunning() && !InteropEmu.IsPaused()) {
				HideMouse();
				_tmrHideMouse.Stop();
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
			get {
				switch(InteropEmu.GetExpansionDevice()) {
					case InteropEmu.ExpansionPortDevice.OekaKidsTablet:
					case InteropEmu.ExpansionPortDevice.BandaiHyperShot:
						return true;
				}
				if(InteropEmu.HasZapper()) {
					return true;
				}
				return false;
			}
		}

		public static void OnMouseMove(Control ctrl)
		{
			if(_mouseCaptured && AllowMouseCapture) {
				HideMouse();
				_tmrHideMouse.Stop();
				Form frm = Application.OpenForms[0];
				Point centerPos = frm.PointToScreen(new Point(frm.Width / 2, frm.Height / 2));
				Point diff = new Point(Cursor.Position.X - centerPos.X, Cursor.Position.Y - centerPos.Y);
				if(diff.X != 0 || diff.Y != 0) {
					InteropEmu.SetMouseMovement((Int16)diff.X, (Int16)diff.Y);
					Cursor.Position = centerPos;
				}
			} else {
				_mouseCaptured = false;

				if(!InteropEmu.IsRunning() || InteropEmu.IsPaused()) {
					ShowMouse();
				}

				_tmrHideMouse.Stop();

				if(!CursorManager.NeedMouseIcon) {
					//Only hide mouse if no zapper (otherwise this could be pretty annoying)
					ctrl.Cursor = Cursors.Default;

					if(InteropEmu.IsRunning() && !InteropEmu.IsPaused()) {
						_tmrHideMouse.Start();
					}
				}
			}
		}

		public static void OnMouseLeave()
		{
			_tmrHideMouse.Stop();
			ShowMouse();
		}

		public static bool AllowMouseCapture
		{
			get
			{
				if(InteropEmu.IsPaused()) {
					return false;
				}

				if(InteropEmu.CheckFlag(EmulationFlags.InBackground)) {
					return false;
				}

				switch(InteropEmu.GetExpansionDevice()) {
					case InteropEmu.ExpansionPortDevice.ArkanoidController:
					case InteropEmu.ExpansionPortDevice.HoriTrack:
						return true;
				}
				for(int i = 0; i < 4; i++) {
					switch(InteropEmu.GetControllerType(i)) {
						case InteropEmu.ControllerType.ArkanoidController:
						case InteropEmu.ControllerType.SnesMouse:
						case InteropEmu.ControllerType.SuborMouse:
							return true;
					}
				}
				return false;
			}
		}

		public static void ReleaseMouse()
		{
			_mouseCaptured = false;
			ShowMouse();
		}

		public static void CaptureMouse()
		{
			if(AllowMouseCapture) {
				if(!_mouseCaptured) {
					InteropEmu.DisplayMessage("Input", ResourceHelper.GetMessage("MouseModeEnabled"));
				}
				_mouseCaptured = true;
				HideMouse();
				Form frm = Application.OpenForms[0];
				Point centerPos = frm.PointToScreen(new Point(frm.Width / 2, frm.Height / 2));
				Cursor.Position = centerPos;
			}
		}
	}
}
