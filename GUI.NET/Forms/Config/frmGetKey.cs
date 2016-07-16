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
	public partial class frmGetKey : BaseForm
	{
		public frmGetKey()
		{
			InitializeComponent();
			InteropEmu.UpdateInputDevices();
		}

		public string BindedKey { get; set; }

		private void tmrCheckKey_Tick(object sender, EventArgs e)
		{	
			string pressedKey = InteropEmu.GetKeyName(InteropEmu.GetPressedKey());
			if(!string.IsNullOrWhiteSpace(pressedKey)) {
				BindedKey = pressedKey;
				this.Close();
			}
		}
	}
}
