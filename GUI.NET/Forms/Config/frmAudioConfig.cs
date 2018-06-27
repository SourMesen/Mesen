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

			tlpEqualizer.Enabled = false;

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

			AddBinding("EnableEqualizer", chkEnableEqualizer);
			//TODO: Uncomment when equalizer presets are implemented
			//AddBinding("EqualizerPreset", cboEqualizerPreset);
			//AddBinding("EqualizerFilterType", cboEqualizerFilterType);

			AddBinding("Band1Gain", trkBand1Gain);
			AddBinding("Band2Gain", trkBand2Gain);
			AddBinding("Band3Gain", trkBand3Gain);
			AddBinding("Band4Gain", trkBand4Gain);
			AddBinding("Band5Gain", trkBand5Gain);
			AddBinding("Band6Gain", trkBand6Gain);
			AddBinding("Band7Gain", trkBand7Gain);
			AddBinding("Band8Gain", trkBand8Gain);
			AddBinding("Band9Gain", trkBand9Gain);
			AddBinding("Band10Gain", trkBand10Gain);
			AddBinding("Band11Gain", trkBand11Gain);
			AddBinding("Band12Gain", trkBand12Gain);
			AddBinding("Band13Gain", trkBand13Gain);
			AddBinding("Band14Gain", trkBand14Gain);
			AddBinding("Band15Gain", trkBand15Gain);
			AddBinding("Band16Gain", trkBand16Gain);
			AddBinding("Band17Gain", trkBand17Gain);
			AddBinding("Band18Gain", trkBand18Gain);
			AddBinding("Band19Gain", trkBand19Gain);
			AddBinding("Band20Gain", trkBand20Gain);

			AddBinding("ReduceSoundInBackground", chkReduceSoundInBackground);
			AddBinding("MuteSoundInBackground", chkMuteSoundInBackground);
			AddBinding("ReduceSoundInFastForward", chkReduceSoundInFastForward);
			AddBinding("VolumeReduction", trkVolumeReduction);

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

			UpdateLatencyWarning();

			//TODO: Uncomment when equalizer presets are implemented
			/*cboEqualizerFilterType.Items.RemoveAt(0); //Remove "None" from dropdown
			if(cboEqualizerFilterType.SelectedItem == null) {
				cboEqualizerFilterType.SelectedIndex = 0;
			}*/
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

		private void UpdateLatencyWarning()
		{
			picLatencyWarning.Visible = nudLatency.Value <= 55;
			lblLatencyWarning.Visible = nudLatency.Value <= 55;
		}

		private void nudLatency_ValueChanged(object sender, EventArgs e)
		{
			UpdateLatencyWarning();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.AudioInfo = new AudioInfo();
			Entity = ConfigManager.Config.AudioInfo;
			UpdateUI();
			AudioInfo.ApplyConfig();
		}

		private void cboEqualizerPreset_SelectedIndexChanged(object sender, EventArgs e)
		{
			EqualizerPreset preset = cboEqualizerPreset.GetEnumValue<EqualizerPreset>();

			foreach(Control ctrl in tlpEqualizer.Controls) {
				if(ctrl is ctrlTrackbar) {
					ctrl.Enabled = preset < EqualizerPreset.TwinFamicom60;
				}
			}

			AudioInfo audio = (AudioInfo)Entity;
			audio.EqualizerPreset = preset;
			switch(preset) {
				case EqualizerPreset.TwinFamicom:
					audio.Band1Gain = -44;
					audio.Band2Gain = -22;
					audio.Band3Gain = -10;
					audio.Band4Gain = -7;
					audio.Band5Gain = -3;
					audio.Band6Gain = 0;
					audio.Band7Gain = -2;
					audio.Band8Gain = -3;
					audio.Band9Gain = 0;
					audio.Band10Gain = -3;
					audio.Band11Gain = -4;
					audio.Band12Gain = -9;
					audio.Band13Gain = -17;
					audio.Band14Gain = -28;
					audio.Band15Gain = -44;
					audio.Band16Gain = -65;
					audio.Band17Gain = -83;
					audio.Band18Gain = -103;
					audio.Band19Gain = -109;
					audio.Band20Gain = -94;
					UpdateUI();
					break;
			}

			if(cboEqualizerPreset.GetEnumValue<EqualizerPreset>() != preset) {
				this.BeginInvoke((Action)(() => {
					cboEqualizerPreset.SetEnumValue(preset);
				}));
			}
		}

		private void trkBandGain_ValueChanged(object sender, EventArgs e)
		{
			if(!this.Updating) {
				cboEqualizerPreset.SetEnumValue(EqualizerPreset.Custom);
			}
		}

		private void chkEnableEqualizer_CheckedChanged(object sender, EventArgs e)
		{
			tlpEqualizer.Enabled = chkEnableEqualizer.Checked;
		}

		private void chkMuteWhenInBackground_CheckedChanged(object sender, EventArgs e)
		{
			UpdateVolumeOptions();
		}

		private void chkReduceVolume_CheckedChanged(object sender, EventArgs e)
		{
			UpdateVolumeOptions();
		}

		private void UpdateVolumeOptions()
		{
			chkReduceSoundInBackground.Enabled = !chkMuteSoundInBackground.Checked;
			trkVolumeReduction.Enabled = chkReduceSoundInFastForward.Checked || (chkReduceSoundInBackground.Checked && chkReduceSoundInBackground.Enabled);
		}
	}
}
