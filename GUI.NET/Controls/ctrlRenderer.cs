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
	public partial class ctrlRenderer : BaseControl
	{
		private const int LeftMouseButtonKeyCode = 0x200;
		private const int RightMouseButtonKeyCode = 0x201;
		private const int MiddleMouseButtonKeyCode = 0x202;

		private bool _cursorHidden = false;

		public ctrlRenderer()
		{
			InitializeComponent();
		}

		public bool NeedMouseIcon
		{
			get { return InteropEmu.GetExpansionDevice() == InteropEmu.ExpansionPortDevice.OekaKidsTablet || InteropEmu.HasZapper(); }
		}

		private void ShowMouse()
		{
			if(_cursorHidden) {
				Cursor.Show();
				_cursorHidden = false;
			}
		}

		private void HideMouse()
		{
			if(!_cursorHidden) {
				Cursor.Hide();
				_cursorHidden = true;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			SetMouseButtonState(System.Windows.Forms.Control.MouseButtons);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			SetMouseButtonState(System.Windows.Forms.Control.MouseButtons);
		}		

		private void SetMouseButtonState(MouseButtons pressedButtons)
		{
			InteropEmu.SetKeyState(LeftMouseButtonKeyCode, pressedButtons.HasFlag(MouseButtons.Left));
			InteropEmu.SetKeyState(RightMouseButtonKeyCode, pressedButtons.HasFlag(MouseButtons.Right));
			InteropEmu.SetKeyState(MiddleMouseButtonKeyCode, pressedButtons.HasFlag(MouseButtons.Middle));
		}

		private void ctrlRenderer_MouseMove(object sender, MouseEventArgs e)
		{
			if(!InteropEmu.IsRunning() || InteropEmu.IsPaused() || !InteropEmu.HasArkanoidPaddle()) {
				ShowMouse();
			} else if(InteropEmu.HasArkanoidPaddle() && !this.NeedMouseIcon) {
				HideMouse();
			}

			tmrMouse.Stop();

			if(this.NeedMouseIcon) {
				this.Cursor = Cursors.Cross;
			} else {
				this.Cursor = Cursors.Default;

				//Only hide mouse if no zapper (otherwise this could be pretty annoying)
				tmrMouse.Start();
			}

			double xPos = (double)e.X / this.Width;
			double yPos = (double)e.Y / this.Height;

			xPos = Math.Max(0.0, Math.Min(1.0, xPos));
			yPos = Math.Max(0.0, Math.Min(1.0, yPos));

			InteropEmu.SetMousePosition(xPos, yPos);
		}
		
		private void tmrMouse_Tick(object sender, EventArgs e)
		{
			HideMouse();
		}

		private void ctrlRenderer_MouseLeave(object sender, EventArgs e)
		{
			tmrMouse.Stop();
			ShowMouse();
			InteropEmu.SetMousePosition(-1, -1);
		}
	}
}
