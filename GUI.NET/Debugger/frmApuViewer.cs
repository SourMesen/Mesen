using Mesen.GUI.Config;
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

				if(Program.IsMono) {
					this.Width = (int)(this.Width * 1.2);
				}
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			//Restore normal volume settings
			AudioInfo.ApplyConfig();
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

		private void chkSoundChannel_CheckedChanged(object sender, EventArgs e)
		{
			InteropEmu.SetChannelVolume(AudioChannel.Square1, chkSquare1.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.Square2, chkSquare2.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.Triangle, chkTriangle.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.Noise, chkNoise.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.DMC, chkDmc.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.FDS, chkFds.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.Namco163, chkNamco.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.VRC6, chkVrc6.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.VRC7, chkVrc7.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.MMC5, chkMmc5.Checked ? 1 : 0);
			InteropEmu.SetChannelVolume(AudioChannel.Sunsoft5B, chkSunsoft.Checked ? 1 : 0);
		}
	}
}
