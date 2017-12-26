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
	public partial class ctrlBandaiMicrophone : BaseInputConfigControl
	{
		private List<Button> _keyIndexes;
		
		public ctrlBandaiMicrophone()
		{
			InitializeComponent();

			_keyIndexes = new List<Button>() {
				btn1, btn2, btn3
			};
		}

		public override void Initialize(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				InitButton(_keyIndexes[i], mappings.BandaiMicrophoneButtons[i]);
			}
		}

		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				mappings.BandaiMicrophoneButtons[i] = (UInt32)_keyIndexes[i].Tag;
			}
		}
	}
}
