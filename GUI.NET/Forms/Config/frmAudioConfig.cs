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

			if(!Program.IsMono) {
				this.trkReverbDelay.BackColor = System.Drawing.SystemColors.ControlLightLight;
				this.trkReverbStrength.BackColor = System.Drawing.SystemColors.ControlLightLight;
			}

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

			AddBinding("Square1Panning", trkSquare1Pan);
			AddBinding("Square2Panning", trkSquare2Pan);
			AddBinding("TrianglePanning", trkTrianglePan);
			AddBinding("NoisePanning", trkNoisePan);
			AddBinding("DmcPanning", trkDmcPan);
			AddBinding("FdsPanning", trkFdsPan);
			AddBinding("Mmc5Panning", trkMmc5Pan);
			AddBinding("Vrc6Panning", trkVrc6Pan);
			AddBinding("Vrc7Panning", trkVrc7Pan);
			AddBinding("Namco163Panning", trkNamcoPan);
			AddBinding("Sunsoft5bPanning", trkSunsoftPan);

			AddBinding("AudioLatency", nudLatency);
			AddBinding("SampleRate", cboSampleRate);
			AddBinding("AudioDevice", cboAudioDevice);

			AddBinding("ReduceSoundInBackground", chkReduceSoundInBackground);
			AddBinding("MuteSoundInBackground", chkMuteSoundInBackground);

			AddBinding("SwapDutyCycles", chkSwapDutyCycles);
			AddBinding("SilenceTriangleHighFreq", chkSilenceTriangleHighFreq);
			AddBinding("ReduceDmcPopping", chkReduceDmcPopping);
			AddBinding("DisableNoiseModeFlag", chkDisableNoiseModeFlag);
			
			radStereoDisabled.Tag = InteropEmu.StereoFilter.None;
			radStereoDelay.Tag = InteropEmu.StereoFilter.Delay;
			radStereoPanning.Tag = InteropEmu.StereoFilter.Panning;

			AddBinding("StereoFilter", tlpStereoFilter);
			AddBinding("StereoDelay", nudStereoDelay);
			AddBinding("StereoPanningAngle", nudStereoPanning);

			AddBinding("ReverbEnabled", chkReverbEnabled);
			AddBinding("ReverbDelay", trkReverbDelay);
			AddBinding("ReverbStrength", trkReverbStrength);

			AddBinding("CrossFeedEnabled", chkCrossFeedEnabled);
			AddBinding("CrossFeedRatio", nudCrossFeedRatio);
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

		private void nudLatency_ValueChanged(object sender, EventArgs e)
		{
			picLatencyWarning.Visible = nudLatency.Value <= 30;
			lblLatencyWarning.Visible = nudLatency.Value <= 30;
		}
	}
}
