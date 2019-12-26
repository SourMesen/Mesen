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

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlScanlineCycleSelect : UserControl
	{
		private int _ppuViewerId = 0;
		private int _scanline = 241;
		private int _cycle = 0;

		public int Scanline { get { return _scanline; } }
		public int Cycle { get { return _cycle; } }
		public int ViewerId { get { return _ppuViewerId; } }

		public ctrlScanlineCycleSelect()
		{
			InitializeComponent();
		}

		public void Initialize(int ppuViewerId, int scanline, int cycle)
		{
			_ppuViewerId = ppuViewerId;
			_scanline = scanline;
			_cycle = cycle;

			this.nudScanline.Value = _scanline;
			this.nudCycle.Value = _cycle;

			InteropEmu.DebugSetPpuViewerScanlineCycle(_ppuViewerId, _scanline, _cycle);
		}

		public void RefreshSettings()
		{
			InteropEmu.DebugSetPpuViewerScanlineCycle(_ppuViewerId, _scanline, _cycle);
		}

		private void SetUpdateScanlineCycle(int scanline, int cycle)
		{
			_scanline = scanline;
			_cycle = cycle;
			RefreshSettings();
		}

		private void nudScanlineCycle_ValueChanged(object sender, EventArgs e)
		{
			SetUpdateScanlineCycle((int)this.nudScanline.Value, (int)this.nudCycle.Value);
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			this.nudScanline.Value = 241;
			this.nudCycle.Value = 0;
		}
	}
}
