using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmApuViewer : BaseForm
	{
		public frmApuViewer()
		{
			InitializeComponent();

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				tmrUpdate.Start();
			}
		}

		private void tmrUpdate_Tick(object sender, EventArgs e)
		{
			ApuState state = new ApuState();
			InteropEmu.DebugGetApuState(ref state);

			ctrlSquareInfo1.ProcessState(ref state.Square1);
			ctrlSquareInfo2.ProcessState(ref state.Square2);
			ctrlTriangleInfo.ProcessState(ref state.Triangle);
			ctrlNoiseInfo.ProcessState(ref state.Noise);
			ctrlDmcInfo.ProcessState(ref state.Dmc);
			ctrlFrameCounterInfo.ProcessState(ref state.FrameCounter);
		}
	}
}
