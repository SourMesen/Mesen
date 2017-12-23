using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlPowerPadConfig : BaseInputConfigControl
	{
		private List<Button> _keyIndexes;

		public ctrlPowerPadConfig()
		{
			InitializeComponent();

			_keyIndexes = new List<Button>() {
				btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12
			};

			ShowSideA = false;
		}

		public bool ShowSideA
		{
			set
			{
				btn1.BackColor = value ? SystemColors.ControlDark : Color.LightBlue;
				btn2.BackColor = Color.LightBlue;
				btn3.BackColor = value ? Color.LightBlue : Color.IndianRed;
				btn4.BackColor = value ? SystemColors.ControlDark : Color.IndianRed;
				btn5.BackColor = Color.LightBlue;
				btn6.BackColor = value ? Color.IndianRed : Color.LightBlue;
				btn7.BackColor = Color.IndianRed;
				btn8.BackColor = value ? Color.LightBlue : Color.IndianRed;
				btn9.BackColor = value ? SystemColors.ControlDark : Color.LightBlue;
				btn10.BackColor = Color.LightBlue;
				btn11.BackColor = value ? Color.LightBlue : Color.IndianRed;
				btn12.BackColor = value ? SystemColors.ControlDark : Color.IndianRed;

				lbl1.Visible = lbl2.Visible =
				lbl3.Visible = lbl4.Visible =
				lbl5.Visible = lbl6.Visible =
				lbl7.Visible = lbl8.Visible =
				lbl9.Visible = lbl10.Visible =
				lbl11.Visible = lbl12.Visible = !value;
			}
		}

		public override void Initialize(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				InitButton(_keyIndexes[i], mappings.PowerPadButtons[i]);
			}
		}

		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				mappings.PowerPadButtons[i] = (UInt32)_keyIndexes[i].Tag;
			}
		}
	}
}
