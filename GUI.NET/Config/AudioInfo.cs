using System;

namespace Mesen.GUI.Config
{
	public class AudioInfo
	{
		public string AudioDevice = "";
		public bool EnableAudio = true;

		public bool DisableDynamicSampleRate = false;

		[MinMax(15, 300)] public UInt32 AudioLatency = 60;

		[MinMax(0, 100)] public UInt32 MasterVolume = 25;
		[MinMax(0, 100)] public UInt32 Square1Volume = 100;
		[MinMax(0, 100)] public UInt32 Square2Volume = 100;
		[MinMax(0, 100)] public UInt32 TriangleVolume = 100;
		[MinMax(0, 100)] public UInt32 NoiseVolume = 100;
		[MinMax(0, 100)] public UInt32 DmcVolume = 100;
		[MinMax(0, 100)] public UInt32 FdsVolume = 100;
		[MinMax(0, 100)] public UInt32 Mmc5Volume = 100;
		[MinMax(0, 100)] public UInt32 Vrc6Volume = 100;
		[MinMax(0, 100)] public UInt32 Vrc7Volume = 100;
		[MinMax(0, 100)] public UInt32 Namco163Volume = 100;
		[MinMax(0, 100)] public UInt32 Sunsoft5bVolume = 100;

		[MinMax(-100, 100)] public Int32 Square1Panning = 0;
		[MinMax(-100, 100)] public Int32 Square2Panning = 0;
		[MinMax(-100, 100)] public Int32 TrianglePanning = 0;
		[MinMax(-100, 100)] public Int32 NoisePanning = 0;
		[MinMax(-100, 100)] public Int32 DmcPanning = 0;
		[MinMax(-100, 100)] public Int32 FdsPanning = 0;
		[MinMax(-100, 100)] public Int32 Mmc5Panning = 0;
		[MinMax(-100, 100)] public Int32 Vrc6Panning = 0;
		[MinMax(-100, 100)] public Int32 Vrc7Panning = 0;
		[MinMax(-100, 100)] public Int32 Namco163Panning = 0;
		[MinMax(-100, 100)] public Int32 Sunsoft5bPanning = 0;

		[ValidValues(11025, 22050, 44100, 48000, 96000)] public UInt32 SampleRate = 48000;
		public bool ReduceSoundInBackground = true;
		public bool ReduceSoundInFastForward = false;
		[MinMax(0, 100)] public int VolumeReduction = 75;
		
		public bool MuteSoundInBackground = false;
		public bool SwapDutyCycles = false;
		public bool SilenceTriangleHighFreq = false;
		public bool ReduceDmcPopping = false;
		public bool DisableNoiseModeFlag = false;
		public InteropEmu.StereoFilter StereoFilter;
		[MinMax(0, 100)] public Int32 StereoDelay = 15;
		[MinMax(-180, 180)] public Int32 StereoPanningAngle = 15;
		[MinMax(1, 100)] public Int32 StereoCombFilterDelay = 5;
		[MinMax(1, 200)] public Int32 StereoCombFilterStrength = 100;
		
		public bool ReverbEnabled = false;
		[MinMax(1, 10)] public UInt32 ReverbStrength = 5;
		[MinMax(1, 30)] public UInt32 ReverbDelay = 10;

		public bool CrossFeedEnabled = false;
		[MinMax(0, 100)] public UInt32 CrossFeedRatio = 0;

		public bool EnableEqualizer = false;
		public EqualizerFilterType EqualizerFilterType = EqualizerFilterType.Butterworth;
		public EqualizerPreset EqualizerPreset = EqualizerPreset.Custom;
		[MinMax(-200, 200)] public Int32 Band1Gain = 0;
		[MinMax(-200, 200)] public Int32 Band2Gain = 0;
		[MinMax(-200, 200)] public Int32 Band3Gain = 0;
		[MinMax(-200, 200)] public Int32 Band4Gain = 0;
		[MinMax(-200, 200)] public Int32 Band5Gain = 0;
		[MinMax(-200, 200)] public Int32 Band6Gain = 0;
		[MinMax(-200, 200)] public Int32 Band7Gain = 0;
		[MinMax(-200, 200)] public Int32 Band8Gain = 0;
		[MinMax(-200, 200)] public Int32 Band9Gain = 0;
		[MinMax(-200, 200)] public Int32 Band10Gain = 0;
		[MinMax(-200, 200)] public Int32 Band11Gain = 0;
		[MinMax(-200, 200)] public Int32 Band12Gain = 0;
		[MinMax(-200, 200)] public Int32 Band13Gain = 0;
		[MinMax(-200, 200)] public Int32 Band14Gain = 0;
		[MinMax(-200, 200)] public Int32 Band15Gain = 0;
		[MinMax(-200, 200)] public Int32 Band16Gain = 0;
		[MinMax(-200, 200)] public Int32 Band17Gain = 0;
		[MinMax(-200, 200)] public Int32 Band18Gain = 0;
		[MinMax(-200, 200)] public Int32 Band19Gain = 0;
		[MinMax(-200, 200)] public Int32 Band20Gain = 0;

		public AudioInfo()
		{
		}

		static private double ConvertVolume(UInt32 volume)
		{
			return ((double)volume / 100d);
		}

		static private double ConvertPanning(Int32 panning)
		{
			return (double)((panning + 100) / 100d);
		}

		static public void ApplyConfig()
		{
			AudioInfo audioInfo = ConfigManager.Config.AudioInfo;
			InteropEmu.SetAudioDevice(audioInfo.AudioDevice);
			InteropEmu.SetAudioLatency(audioInfo.AudioLatency);
			InteropEmu.SetMasterVolume(ConfigManager.Config.AudioInfo.EnableAudio ? audioInfo.MasterVolume / 10d : 0, audioInfo.VolumeReduction/ 100d);
			InteropEmu.SetChannelVolume(AudioChannel.Square1, ConvertVolume(audioInfo.Square1Volume));
			InteropEmu.SetChannelVolume(AudioChannel.Square2, ConvertVolume(audioInfo.Square2Volume));
			InteropEmu.SetChannelVolume(AudioChannel.Triangle, ConvertVolume(audioInfo.TriangleVolume));
			InteropEmu.SetChannelVolume(AudioChannel.Noise, ConvertVolume(audioInfo.NoiseVolume));
			InteropEmu.SetChannelVolume(AudioChannel.DMC, ConvertVolume(audioInfo.DmcVolume));
			InteropEmu.SetChannelVolume(AudioChannel.FDS, ConvertVolume(audioInfo.FdsVolume));
			InteropEmu.SetChannelVolume(AudioChannel.MMC5, ConvertVolume(audioInfo.Mmc5Volume));
			InteropEmu.SetChannelVolume(AudioChannel.VRC6, ConvertVolume(audioInfo.Vrc6Volume));
			InteropEmu.SetChannelVolume(AudioChannel.VRC7, ConvertVolume(audioInfo.Vrc7Volume));
			InteropEmu.SetChannelVolume(AudioChannel.Namco163, ConvertVolume(audioInfo.Namco163Volume));
			InteropEmu.SetChannelVolume(AudioChannel.Sunsoft5B, ConvertVolume(audioInfo.Sunsoft5bVolume));

			InteropEmu.SetChannelPanning(AudioChannel.Square1, ConvertPanning(audioInfo.Square1Panning));
			InteropEmu.SetChannelPanning(AudioChannel.Square2, ConvertPanning(audioInfo.Square2Panning));
			InteropEmu.SetChannelPanning(AudioChannel.Triangle, ConvertPanning(audioInfo.TrianglePanning));
			InteropEmu.SetChannelPanning(AudioChannel.Noise, ConvertPanning(audioInfo.NoisePanning));
			InteropEmu.SetChannelPanning(AudioChannel.DMC, ConvertPanning(audioInfo.DmcPanning));
			InteropEmu.SetChannelPanning(AudioChannel.FDS, ConvertPanning(audioInfo.FdsPanning));
			InteropEmu.SetChannelPanning(AudioChannel.MMC5, ConvertPanning(audioInfo.Mmc5Panning));
			InteropEmu.SetChannelPanning(AudioChannel.VRC6, ConvertPanning(audioInfo.Vrc6Panning));
			InteropEmu.SetChannelPanning(AudioChannel.VRC7, ConvertPanning(audioInfo.Vrc7Panning));
			InteropEmu.SetChannelPanning(AudioChannel.Namco163, ConvertPanning(audioInfo.Namco163Panning));
			InteropEmu.SetChannelPanning(AudioChannel.Sunsoft5B, ConvertPanning(audioInfo.Sunsoft5bPanning));

			InteropEmu.SetEqualizerFilterType(audioInfo.EnableEqualizer ? audioInfo.EqualizerFilterType : EqualizerFilterType.None);

			if(audioInfo.EqualizerPreset < EqualizerPreset.TwinFamicom60) {
				double[] defaultBands = new double[20] { 40, 56, 80, 113, 160, 225, 320, 450, 600, 750, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 10000, 12500, 15000 };
				InteropEmu.SetEqualizerBands(defaultBands, (UInt32)defaultBands.Length);

				InteropEmu.SetBandGain(0, (double)audioInfo.Band1Gain / 10);
				InteropEmu.SetBandGain(1, (double)audioInfo.Band2Gain / 10);
				InteropEmu.SetBandGain(2, (double)audioInfo.Band3Gain / 10);
				InteropEmu.SetBandGain(3, (double)audioInfo.Band4Gain / 10);
				InteropEmu.SetBandGain(4, (double)audioInfo.Band5Gain / 10);
				InteropEmu.SetBandGain(5, (double)audioInfo.Band6Gain / 10);
				InteropEmu.SetBandGain(6, (double)audioInfo.Band7Gain / 10);
				InteropEmu.SetBandGain(7, (double)audioInfo.Band8Gain / 10);
				InteropEmu.SetBandGain(8, (double)audioInfo.Band9Gain / 10);
				InteropEmu.SetBandGain(9, (double)audioInfo.Band10Gain / 10);
				InteropEmu.SetBandGain(10, (double)audioInfo.Band11Gain / 10);
				InteropEmu.SetBandGain(11, (double)audioInfo.Band12Gain / 10);
				InteropEmu.SetBandGain(12, (double)audioInfo.Band13Gain / 10);
				InteropEmu.SetBandGain(13, (double)audioInfo.Band14Gain / 10);
				InteropEmu.SetBandGain(14, (double)audioInfo.Band15Gain / 10);
				InteropEmu.SetBandGain(15, (double)audioInfo.Band16Gain / 10);
				InteropEmu.SetBandGain(16, (double)audioInfo.Band17Gain / 10);
				InteropEmu.SetBandGain(17, (double)audioInfo.Band18Gain / 10);
				InteropEmu.SetBandGain(18, (double)audioInfo.Band19Gain / 10);
				InteropEmu.SetBandGain(19, (double)audioInfo.Band20Gain / 10);
			} else {
				switch(audioInfo.EqualizerPreset) {
					case EqualizerPreset.TwinFamicom60:
						double[] bands = new double[60] {
							20, 22.5, 25.3, 28.4, 31.9, 35.9, 40.4, 45.4, 51.0, 57.4, 64.5, 72.5, 81.5, 91.6, 103, 116, 130, 146, 165, 185, 208, 234,
							263, 295, 332, 373, 420, 472, 531, 596, 671, 754, 848, 953, 1070, 1200, 1350, 1520, 1710, 1920, 2160, 2430, 2730,
							3070, 3450, 3880, 4370, 4910, 5520, 6200, 6970, 7840, 8810, 9910, 11100, 12500, 14100, 15800, 17300, 20000
						};
						InteropEmu.SetEqualizerBands(bands, (UInt32)bands.Length);

						double[] gains = new double[60] { -4.4, -4.1, -3.8, -3.5, -3.1, -2.8, -2.3, -1.9, -1.6, -1.3, -1.1, -0.9, -0.7, -0.7, -0.6, -0.6, -0.4, -0.1, 0.0, -0.2, -0.3, -0.4, -0.3, -0.5, -0.3, -0.4, -0.2, -0.2, -0.2, -0.3, -0.3, -0.4, -0.4, -0.3, -0.2, -0.6, -0.5, -0.7, -0.7, -1.1, -1.4, -1.6, -1.9, -2.3, -2.7, -3.1, -3.6, -4.3, -4.7, -5.4, -6.2, -7.0, -7.8, -8.2, -8.8, -9.4, -10.0, -10.6, -10.9, -9.5 };
						for(int i = 0; i < 60; i++) {
							InteropEmu.SetBandGain(i, gains[i]);
						}
						break;
				}
			}

			InteropEmu.SetSampleRate(audioInfo.SampleRate);

			InteropEmu.SetFlag(EmulationFlags.MuteSoundInBackground, audioInfo.MuteSoundInBackground);
			InteropEmu.SetFlag(EmulationFlags.ReduceSoundInBackground, audioInfo.ReduceSoundInBackground);
			InteropEmu.SetFlag(EmulationFlags.ReduceSoundInFastForward, audioInfo.ReduceSoundInFastForward);

			InteropEmu.SetFlag(EmulationFlags.DisableDynamicSampleRate, audioInfo.DisableDynamicSampleRate);
			InteropEmu.SetFlag(EmulationFlags.SwapDutyCycles, audioInfo.SwapDutyCycles);
			InteropEmu.SetFlag(EmulationFlags.SilenceTriangleHighFreq, audioInfo.SilenceTriangleHighFreq);
			InteropEmu.SetFlag(EmulationFlags.ReduceDmcPopping, audioInfo.ReduceDmcPopping);
			InteropEmu.SetFlag(EmulationFlags.DisableNoiseModeFlag, audioInfo.DisableNoiseModeFlag);

			InteropEmu.SetAudioFilterSettings(new InteropEmu.AudioFilterSettings() {
				Filter = audioInfo.StereoFilter,
				Angle = (double)audioInfo.StereoPanningAngle / 180 * Math.PI,
				Delay = audioInfo.StereoFilter == InteropEmu.StereoFilter.Delay ? audioInfo.StereoDelay : audioInfo.StereoCombFilterDelay,
				Strength = audioInfo.StereoCombFilterStrength,
				ReverbDelay = audioInfo.ReverbEnabled ? audioInfo.ReverbDelay / 10.0 : 0,
				ReverbStrength = audioInfo.ReverbEnabled ? audioInfo.ReverbStrength / 10.0 : 0,
				CrossFeedRatio = audioInfo.CrossFeedEnabled ? (int)audioInfo.CrossFeedRatio : 0
			});
		}
	}

	public enum EqualizerPreset
	{
		Custom = 0,
		TwinFamicom,
		TwinFamicom60
	}

	public enum DynamicRateAdjustmentType
	{
		None = 0,
		Low = 1,
		Medium = 2,
		High = 3
	}
}
