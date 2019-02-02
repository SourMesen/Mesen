﻿using System;
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

		public ctrlRenderer()
		{
			InitializeComponent();
			ThemeHelper.ExcludeFromTheme(this);
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
			CursorManager.OnMouseMove(this);

			if(CursorManager.NeedMouseIcon) {
				this.Cursor = Cursors.Cross;
			}

			double xPos = (double)e.X / this.Width;
			double yPos = (double)e.Y / this.Height;

			xPos = Math.Max(0.0, Math.Min(1.0, xPos));
			yPos = Math.Max(0.0, Math.Min(1.0, yPos));

			InteropEmu.SetMousePosition(xPos, yPos);
		}

		private void ctrlRenderer_MouseLeave(object sender, EventArgs e)
		{
			CursorManager.OnMouseLeave();
			InteropEmu.SetMousePosition(-1, -1);
		}
	}
}
