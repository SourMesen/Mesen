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
		public ctrlPowerPadConfig(KeyMappings mappings) : base(mappings)
		{
			InitializeComponent();
			Initialize(mappings);
		}

		public override void Initialize(KeyMappings mappings)
		{
			InitButton(btn1, mappings.PowerPadButtons[0]);
			InitButton(btn2, mappings.PowerPadButtons[1]);
			InitButton(btn3, mappings.PowerPadButtons[2]);
			InitButton(btn4, mappings.PowerPadButtons[3]);
			InitButton(btn5, mappings.PowerPadButtons[4]);
			InitButton(btn6, mappings.PowerPadButtons[5]);
			InitButton(btn7, mappings.PowerPadButtons[6]);
			InitButton(btn8, mappings.PowerPadButtons[7]);
			InitButton(btn9, mappings.PowerPadButtons[8]);
			InitButton(btn10, mappings.PowerPadButtons[9]);
			InitButton(btn11, mappings.PowerPadButtons[10]);
			InitButton(btn12, mappings.PowerPadButtons[11]);
		}
		
		public override void UpdateKeyMappings()
		{
			_mappings.PowerPadButtons[0] = (UInt32)btn1.Tag;
			_mappings.PowerPadButtons[1] = (UInt32)btn2.Tag;
			_mappings.PowerPadButtons[2] = (UInt32)btn3.Tag;
			_mappings.PowerPadButtons[3] = (UInt32)btn4.Tag;
			_mappings.PowerPadButtons[4] = (UInt32)btn5.Tag;
			_mappings.PowerPadButtons[5] = (UInt32)btn6.Tag;
			_mappings.PowerPadButtons[6] = (UInt32)btn7.Tag;
			_mappings.PowerPadButtons[7] = (UInt32)btn8.Tag;
			_mappings.PowerPadButtons[8] = (UInt32)btn9.Tag;
			_mappings.PowerPadButtons[9] = (UInt32)btn10.Tag;
			_mappings.PowerPadButtons[10] = (UInt32)btn11.Tag;
			_mappings.PowerPadButtons[11] = (UInt32)btn12.Tag;
		}
	}
}
