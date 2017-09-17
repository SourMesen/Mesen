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
		const int WM_SYSKEYDOWN = 0x104;
		const int WM_SYSKEYUP = 0x105;

		private bool _singleKeyMode = false;

		public frmGetKey(bool singleKeyMode)
		{
			InitializeComponent();
			_singleKeyMode = singleKeyMode;
			if(_singleKeyMode) {
				tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;
				tableLayoutPanel1.RowStyles[1].Height = 0;
			}
			if(_singleKeyMode) {
				lblCurrentKeys.Height = 1;
				lblCurrentKeys.Visible = false;
			}
			ShortcutKey = new KeyCombination();
			InteropEmu.UpdateInputDevices();
			InteropEmu.ResetKeyState();
			
			//Prevent other keybindings from interfering/activating
			InteropEmu.DisableAllKeys(true);

			Application.AddMessageFilter(this);
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if(m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP) {
				int virtualKeyCode = (Int32)m.WParam;
				int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
				InteropEmu.SetKeyState(scanCode, false);
			} else if(m.Msg == WM_SYSKEYDOWN || m.Msg == WM_KEYDOWN) {
				int virtualKeyCode = (Int32)((Int64)m.WParam & 0xFF);
				int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
				InteropEmu.SetKeyState(scanCode, true);
			}

			return false;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Application.RemoveMessageFilter(this);
			InteropEmu.ResetKeyState();
			InteropEmu.DisableAllKeys(false);
			base.OnFormClosing(e);
		}

		public KeyCombination ShortcutKey { get; set; }
		private List<UInt32> _prevScanCodes = new List<UInt32>();

		private void tmrCheckKey_Tick(object sender, EventArgs e)
		{	
			List<UInt32> scanCodes = InteropEmu.GetPressedKeys();

			KeyCombination key = new KeyCombination(_prevScanCodes);
			lblCurrentKeys.Text = key.ToString();

			if(_singleKeyMode && _prevScanCodes.Count > 0 || scanCodes.Count < _prevScanCodes.Count) {
				if(!string.IsNullOrWhiteSpace(key.ToString())) {
					ShortcutKey = key;
					this.Close();
				}
			}

			_prevScanCodes = scanCodes;
		}
	}
}
