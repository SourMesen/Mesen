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

namespace Mesen.GUI.Forms.Config
{
	public partial class frmAudioConfig : BaseConfigForm
	{
		public frmAudioConfig()
		{
			InitializeComponent();

			Icon = Properties.Resources.Audio;
			Entity = ConfigManager.Config.AudioInfo;

			cboAudioDevice.Items.AddRange(InteropEmu.GetAudioDevices().ToArray());

			AddBinding("EnableAudio", chkEnableAudio);
			AddBinding("MasterVolume", trkMaster);
			AddBinding("Square1Volume", trkSquare1Vol);
			AddBinding("Square2Volume", trkSquare2Vol);
			AddBinding("TriangleVolume", trkTriangleVol);
			AddBinding("NoiseVolume", trkNoiseVol);
			AddBinding("DmcVolume", trkDmcVol);
			AddBinding("FdsVolume", trkFdsVol);
			AddBinding("Mmc5Volume", trkMmc5Vol);
			AddBinding("Vrc6Volume", trkVrc6Vol);
			AddBinding("Vrc7Volume", trkVrc7Vol);
			AddBinding("Namco163Volume", trkNamco163Vol);
			AddBinding("Sunsoft5bVolume", trkSunsoft5b);

			AddBinding("AudioLatency", nudLatency);
			AddBinding("SampleRate", cboSampleRate);
			AddBinding("AudioDevice", cboAudioDevice);

			AddBinding("ReduceSoundInBackground", chkReduceSoundInBackground);
			AddBinding("MuteSoundInBackground", chkMuteSoundInBackground);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			AudioInfo.ApplyConfig();
		}

		protected override bool ValidateInput()
		{
			UpdateObject();
			AudioInfo.ApplyConfig();

			return true;
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.AudioInfo = new AudioInfo();
			Entity = ConfigManager.Config.AudioInfo;
			UpdateUI();
			AudioInfo.ApplyConfig();
		}

		private void chkMuteWhenInBackground_CheckedChanged(object sender, EventArgs e)
		{
			chkReduceSoundInBackground.Enabled = !chkMuteSoundInBackground.Checked;
		}
	}
}
