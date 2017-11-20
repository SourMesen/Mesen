using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public class BaseInputConfigControl : BaseControl
	{
		public event EventHandler Change;
		protected KeyMappings _mappings;
		protected HashSet<Button> _buttons = new HashSet<Button>();

		public enum MappedKeyType
		{
			None,
			Keyboard,
			Controller
		}

		private BaseInputConfigControl() { }

		public BaseInputConfigControl(KeyMappings mappings)
		{
			_mappings = mappings;
		}

		public virtual void Initialize(KeyMappings mappings) { }
		public virtual void UpdateKeyMappings() { }

		protected void InitButton(Button btn, UInt32 scanCode)
		{
			if(!_buttons.Contains(btn)) {
				_buttons.Add(btn);
				btn.Click += btnMapping_Click;
			}
			btn.Text = InteropEmu.GetKeyName(scanCode);
			btn.Tag = scanCode;
		}

		protected void OnChange()
		{
			this.Change?.Invoke(this, EventArgs.Empty);
		}

		public MappedKeyType GetKeyType()
		{
			MappedKeyType keyType = MappedKeyType.None;
			foreach(Button btn in _buttons) {
				if((int)btn.Tag > 0xFFFF) {
					return MappedKeyType.Controller;
				} else if((int)btn.Tag > 0) {
					keyType = MappedKeyType.Keyboard;
				}
			}
			return keyType;
		}

		public void ClearKeys()
		{
			foreach(Button btn in _buttons) {
				InitButton(btn, 0);
			}
			this.OnChange();
		}

		protected void btnMapping_Click(object sender, EventArgs e)
		{
			using(frmGetKey frm = new frmGetKey(true)) {
				frm.ShowDialog();
				((Button)sender).Text = frm.ShortcutKey.ToString();
				((Button)sender).Tag = frm.ShortcutKey.Key1;
			}
			this.OnChange();
		}
	}
}
