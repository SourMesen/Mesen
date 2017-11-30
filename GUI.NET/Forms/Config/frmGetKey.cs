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
	public partial class frmGetKey : BaseInputForm
	{
		private bool _singleKeyMode = false;
		private List<UInt32> _prevScanCodes = new List<UInt32>();

		public KeyCombination ShortcutKey { get; set; }

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
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			InteropEmu.DisableAllKeys(false);
			base.OnFormClosing(e);
		}
		
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
