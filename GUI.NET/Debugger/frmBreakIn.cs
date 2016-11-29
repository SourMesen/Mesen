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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmBreakIn : BaseConfigForm
	{
		public frmBreakIn()
		{
			InitializeComponent();

			nudCount.Value = ConfigManager.Config.DebugInfo.BreakInCount;
			radCpuInstructions.Checked = !ConfigManager.Config.DebugInfo.BreakInPpuCycles;
			radPpuCycles.Checked = ConfigManager.Config.DebugInfo.BreakInPpuCycles;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			nudCount.Focus();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			if(this.DialogResult == DialogResult.OK) {

				if(radCpuInstructions.Checked) {
					InteropEmu.DebugStepCycles((UInt32)nudCount.Value);
				} else {
					InteropEmu.DebugPpuStep((UInt32)nudCount.Value);
				}

				ConfigManager.Config.DebugInfo.BreakInCount = (int)nudCount.Value;
				ConfigManager.Config.DebugInfo.BreakInPpuCycles = radPpuCycles.Checked;
				ConfigManager.ApplyChanges();
			}
		}
	}
}
