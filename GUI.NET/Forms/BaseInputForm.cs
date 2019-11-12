﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public class BaseInputForm : BaseForm, IMessageFilter
	{
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_SYSKEYDOWN = 0x104;
		private const int WM_SYSKEYUP = 0x105;

		public BaseInputForm()
		{
			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				Application.AddMessageFilter(this);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Escape) {
				CursorManager.ReleaseMouse();
			}

			bool result = base.ProcessCmdKey(ref msg, keyData);

			if(keyData == Keys.F10 || keyData == (Keys.F10 | Keys.Shift)) {
				//Prevent default Windows behavior on F10 / Shift+F10 presses, which causes issues
				//such as the Shift key not being processed as being released, etc.
				return true;
			} else if(keyData.HasFlag(Keys.Alt) && InteropEmu.IsKeyboardMode()) {
				//Prevent Alt key from activating menu when in keyboard mode
				return true;
			}

			return result;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if(this.ContainsFocus) {
				if(m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP) {
					int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
					InteropEmu.SetKeyState(scanCode, false);
				} else if(m.Msg == WM_SYSKEYDOWN || m.Msg == WM_KEYDOWN) {
					int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
					InteropEmu.SetKeyState(scanCode, true);
				}
			}
			return false;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			Application.RemoveMessageFilter(this);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(Program.IsMono) {
				//Mono does not trigger the activate/deactivate events when opening a modal popup, but it does set the form to disabled
				//Use this to reset key states
				this.EnabledChanged += (object s, EventArgs evt) => {
					InteropEmu.ResetKeyState();
				};
			}
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			InteropEmu.ResetKeyState();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			InteropEmu.ResetKeyState();
		}
	}
}
