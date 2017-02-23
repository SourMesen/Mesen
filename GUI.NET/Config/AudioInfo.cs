using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class AudioInfo
	{
		public string AudioDevice = "";
		public bool EnableAudio = true;
		public UInt32 AudioLatency = 50;
		public UInt32 MasterVolume = 25;
		public UInt32 Square1Volume = 100;
		public UInt32 Square2Volume = 100;
		public UInt32 TriangleVolume = 100;
		public UInt32 NoiseVolume = 100;
		public UInt32 DmcVolume = 100;
		public UInt32 FdsVolume = 100;
		public UInt32 Mmc5Volume = 100;
		public UInt32 Vrc6Volume = 100;
		public UInt32 Vrc7Volume = 100;
		public UInt32 Namco163Volume = 100;
		public UInt32 Sunsoft5bVolume = 100;

		public Int32 Square1Panning = 0;
		public Int32 Square2Panning = 0;
		public Int32 TrianglePanning = 0;
		public Int32 NoisePanning = 0;
		public Int32 DmcPanning = 0;
		public Int32 FdsPanning = 0;
		public Int32 Mmc5Panning = 0;
		public Int32 Vrc6Panning = 0;
		public Int32 Vrc7Panning = 0;
		public Int32 Namco163Panning = 0;
		public Int32 Sunsoft5bPanning = 0;

		public UInt32 SampleRate = 44100;
		public bool ReduceSoundInBackground = true;
		public bool MuteSoundInBackground = false;
		public bool SwapDutyCycles = false;
		public bool SilenceTriangleHighFreq = false;
		public bool ReduceDmcPopping = false;
		public bool DisableNoiseModeFlag = false;
		public InteropEmu.StereoFilter StereoFilter;
		public Int32 StereoDelay = 15;
		public Int32 StereoPanningAngle = 15;
		public bool ReverbEnabled = false;
		public UInt32 ReverbStrength = 5;
		public UInt32 ReverbDelay = 10;
		public bool CrossFeedEnabled = false;
		public UInt32 CrossFeedRatio = 0;

		public AudioInfo()
		{
		}

		static private double ConvertVolume(UInt32 volume)
		{
			if(ConfigManager.Config.AudioInfo.EnableAudio) {
				return ((double)volume / 100d);
			} else {
				return 0;
			}
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
			InteropEmu.SetMasterVolume(audioInfo.MasterVolume / 10d);
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

			InteropEmu.SetSampleRate(audioInfo.SampleRate);

			InteropEmu.SetFlag(EmulationFlags.MuteSoundInBackground, audioInfo.MuteSoundInBackground);
			InteropEmu.SetFlag(EmulationFlags.ReduceSoundInBackground, audioInfo.ReduceSoundInBackground);

			InteropEmu.SetFlag(EmulationFlags.SwapDutyCycles, audioInfo.SwapDutyCycles);
			InteropEmu.SetFlag(EmulationFlags.SilenceTriangleHighFreq, audioInfo.SilenceTriangleHighFreq);
			InteropEmu.SetFlag(EmulationFlags.ReduceDmcPopping, audioInfo.ReduceDmcPopping);
			InteropEmu.SetFlag(EmulationFlags.DisableNoiseModeFlag, audioInfo.DisableNoiseModeFlag);

			InteropEmu.SetStereoFilter(audioInfo.StereoFilter);
			InteropEmu.SetStereoPanningAngle((double)audioInfo.StereoPanningAngle/180*Math.PI);
			InteropEmu.SetStereoDelay(audioInfo.StereoDelay);

			if(audioInfo.ReverbEnabled) {
				InteropEmu.SetReverbParameters(audioInfo.ReverbStrength/10.0, audioInfo.ReverbDelay/10.0);
			} else {
				InteropEmu.SetReverbParameters(0, 0);
			}

			if(audioInfo.CrossFeedEnabled) {
				InteropEmu.SetCrossFeedRatio(audioInfo.CrossFeedRatio);
			} else {
				InteropEmu.SetCrossFeedRatio(0);
			}
		}
	}
}
