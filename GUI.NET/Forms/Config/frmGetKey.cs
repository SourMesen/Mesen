using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmGetKey : BaseForm, IMessageFilter
	{
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;

		private string[] _invalidKeys = new string[] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };

		public frmGetKey()
		{
			InitializeComponent();
			InteropEmu.UpdateInputDevices();
			InteropEmu.ResetKeyState();
			Application.AddMessageFilter(this);
			this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if(m.Msg == WM_KEYUP) {
				int virtualKeyCode = (Int32)m.WParam;
				int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
				InteropEmu.SetKeyState(scanCode, false);
			}

			return false;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(msg.Msg == WM_KEYDOWN) {
				int virtualKeyCode = (Int32)((Int64)msg.WParam & 0xFF);
				int scanCode = (Int32)(((Int64)msg.LParam & 0x1FF0000) >> 16);
				InteropEmu.SetKeyState(scanCode, true);
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		public string BindedKeyName { get; set; }
		public UInt32 BindedKeyCode { get; set; }

		private void tmrCheckKey_Tick(object sender, EventArgs e)
		{	
			UInt32 scanCode = InteropEmu.GetPressedKey();
			string pressedKey = InteropEmu.GetKeyName(scanCode);
			if(!string.IsNullOrWhiteSpace(pressedKey) && !_invalidKeys.Contains(pressedKey)) {
				BindedKeyName = pressedKey;
				BindedKeyCode = scanCode;
				this.Close();
			}
		}
	}
}
