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

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlStandardController : UserControl
	{
		public ctrlStandardController()
		{
			InitializeComponent();
		}

		private void InitButton(Button btn, UInt32 scanCode)
		{
			btn.Text = InteropEmu.GetKeyName(scanCode);
			btn.Tag = scanCode;
		}

		public void Initialize(KeyMappings mappings)
		{
			InitButton(btnA, mappings.A);
			InitButton(btnB, mappings.B);
			InitButton(btnStart, mappings.Start);
			InitButton(btnSelect, mappings.Select);
			InitButton(btnUp, mappings.Up);
			InitButton(btnDown, mappings.Down);
			InitButton(btnLeft, mappings.Left);
			InitButton(btnRight, mappings.Right);
			InitButton(btnTurboA, mappings.TurboA);
			InitButton(btnTurboB, mappings.TurboB);
		}

		public void ClearKeys()
		{
			InitButton(btnA, 0);
			InitButton(btnB, 0);
			InitButton(btnStart, 0);
			InitButton(btnSelect, 0);
			InitButton(btnUp, 0);
			InitButton(btnDown, 0);
			InitButton(btnLeft, 0);
			InitButton(btnRight, 0);
			InitButton(btnTurboA, 0);
			InitButton(btnTurboB, 0);
		}

		private void btnMapping_Click(object sender, EventArgs e)
		{
			frmGetKey frm = new frmGetKey();
			frm.ShowDialog();
			((Button)sender).Text = frm.BindedKeyName;
			((Button)sender).Tag = frm.BindedKeyCode;
		}

		public KeyMappings GetKeyMappings()
		{
			KeyMappings mappings = new KeyMappings() {
				A = (UInt32)btnA.Tag,
				B = (UInt32)btnB.Tag,
				Start = (UInt32)btnStart.Tag,
				Select = (UInt32)btnSelect.Tag,
				Up = (UInt32)btnUp.Tag,
				Down = (UInt32)btnDown.Tag,
				Left = (UInt32)btnLeft.Tag,
				Right = (UInt32)btnRight.Tag,
				TurboA = (UInt32)btnTurboA.Tag,
				TurboB = (UInt32)btnTurboB.Tag,
				TurboSelect = 0,
				TurboStart = 0,
			};
			return mappings;
		}
	}
}
