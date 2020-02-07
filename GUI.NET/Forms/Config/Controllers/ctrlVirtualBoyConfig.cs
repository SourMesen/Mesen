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
	public partial class ctrlVirtualBoyConfig : BaseInputConfigControl
	{
		private List<Button> _keyIndexes;

		public ctrlVirtualBoyConfig()
		{
			InitializeComponent();

			_keyIndexes = new List<Button>() {
				btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12, btn13
			};
		}

		public override void Initialize(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				InitButton(_keyIndexes[i], mappings.VirtualBoyButtons[i]);
			}
		}

		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				mappings.VirtualBoyButtons[i] = (UInt32)_keyIndexes[i].Tag;
			}
		}
	}
}
