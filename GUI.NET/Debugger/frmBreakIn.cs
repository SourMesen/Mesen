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
			radCpuInstructions.Checked = ConfigManager.Config.DebugInfo.BreakInMetric == BreakInMetric.CpuInstructions;
			radCpuCycles.Checked = ConfigManager.Config.DebugInfo.BreakInMetric == BreakInMetric.CpuCycles;
			radPpuCycles.Checked = ConfigManager.Config.DebugInfo.BreakInMetric == BreakInMetric.PpuCycles;
			radScanlines.Checked = ConfigManager.Config.DebugInfo.BreakInMetric == BreakInMetric.Scanlines;
			radFrames.Checked = ConfigManager.Config.DebugInfo.BreakInMetric == BreakInMetric.Frames;
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
				UInt32 count = (UInt32)nudCount.Value;
				ConfigManager.Config.DebugInfo.BreakInCount = (int)count;
				if(radCpuInstructions.Checked) {
					InteropEmu.DebugStep(count);
					ConfigManager.Config.DebugInfo.BreakInMetric = BreakInMetric.CpuInstructions;
				} else if(radCpuCycles.Checked) {
					InteropEmu.DebugStepCycles(count);
					ConfigManager.Config.DebugInfo.BreakInMetric = BreakInMetric.CpuCycles;
				} else if(radPpuCycles.Checked) {
					InteropEmu.DebugPpuStep(count);
					ConfigManager.Config.DebugInfo.BreakInMetric = BreakInMetric.PpuCycles;
				} else if(radScanlines.Checked) {
					InteropEmu.DebugPpuStep(count * 341);
					ConfigManager.Config.DebugInfo.BreakInMetric = BreakInMetric.Scanlines;
				} else {
					InteropEmu.DebugPpuStep(count * 89342);
					ConfigManager.Config.DebugInfo.BreakInMetric = BreakInMetric.Frames;
				}
				ConfigManager.ApplyChanges();
			}
		}
	}
}
