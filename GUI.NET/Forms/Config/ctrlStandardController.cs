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

		public void Initialize(KeyMappings mappings)
		{
			btnA.Text = mappings.A;
			btnB.Text = mappings.B;
			btnStart.Text = mappings.Start;
			btnSelect.Text = mappings.Select;
			btnUp.Text = mappings.Up;
			btnDown.Text = mappings.Down;
			btnLeft.Text = mappings.Left;
			btnRight.Text = mappings.Right;
			btnTurboA.Text = mappings.TurboA;
			btnTurboB.Text = mappings.TurboB;
		}

		public void ClearKeys()
		{
			btnA.Text = "";
			btnB.Text = "";
			btnStart.Text = "";
			btnSelect.Text = "";
			btnUp.Text = "";
			btnDown.Text = "";
			btnLeft.Text = "";
			btnRight.Text = "";
			btnTurboA.Text = "";
			btnTurboB.Text = "";
		}

		private void btnMapping_Click(object sender, EventArgs e)
		{
			frmGetKey frm = new frmGetKey();
			frm.ShowDialog();
			((Button)sender).Text = frm.BindedKey;
		}

		public KeyMappings GetKeyMappings()
		{
			KeyMappings mappings = new KeyMappings() {
				A = btnA.Text,
				B = btnB.Text,
				Start = btnStart.Text,
				Select = btnSelect.Text,
				Up = btnUp.Text,
				Down = btnDown.Text,
				Left = btnLeft.Text,
				Right = btnRight.Text,
				TurboA = btnTurboA.Text,
				TurboB = btnTurboB.Text,
				TurboSelect = string.Empty,
				TurboStart = string.Empty,
			};
			return mappings;
		}
	}
}
